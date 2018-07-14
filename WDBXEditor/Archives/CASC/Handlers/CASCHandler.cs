using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WDBXEditor.Archives.CASC.Constants;
using WDBXEditor.Archives.CASC.Structures;
using WDBXEditor.Archives.FileSystem.Structures;
using WDBXEditor.Archives.Misc;

namespace WDBXEditor.Archives.CASC.Handlers
{
    public class CASCHandler : IDisposable
    {
        public string BasePath { get; set; }
        
        List<IndexFile> idxFiles = new List<IndexFile>();
        List<string> indexFiles = new List<string>();
        ConcurrentDictionary<uint, DataFile> dataFiles = new ConcurrentDictionary<uint, DataFile>();

        BuildInfo buildInfo;
        BuildConfig buildConfig;
        CDNConfig cdnConfig;

        EncodingFile encodingFile;
        RootFile rootFile;

        Lookup3 lookup3;

        public CASCHandler(string basePath)
        {
            BasePath = basePath;

            lookup3 = new Lookup3();

            InitConfigKeys();

            // Get idx files.
            for (var i = 0; i <= 0xF; i++)
            {
                // Always get the last element in sequence for latest file data.
                var idxFile = Directory.GetFiles(BasePath + "/Data/data", $"{i :x2}*.idx").Last();

                idxFiles.Add(new IndexFile(idxFile));
            }

            // Get CDN indices.
            var indices = cdnConfig["archives"];

            for (var i = 0; i < indices.Length; i++)
            {
                indexFiles.Add(indices[i]);

                idxFiles.Add(new IndexFile($"{basePath}/Data/indices/{indices[i]}.index", true, (ushort)i));
            }

            // Get available data.### files.
            Parallel.ForEach(Directory.GetFiles(BasePath + "/Data/data", "data.*"), f =>
            {
                var dataFile = new DataFile(File.OpenRead(f));
                var index = Convert.ToUInt32(Path.GetExtension(f).Remove(0, 1));
                dataFiles.TryAdd(index, dataFile);
            });

            // Get encoding key.
            var encodingKey = buildConfig["encoding"][1];

            if (encodingKey.Length / 2 > 16)
                throw new InvalidOperationException("Encoding key too long");
            else if (encodingKey.Length / 2 < 16)
                throw new InvalidOperationException("Encoding key too short");

            encodingFile = new EncodingFile(encodingKey.ToByteArray());

            // Get idx file & entry which contains the encoding key (first 9 bytes)
            idxFiles.ForEach(idx =>
            {
                var idxEntry = idx[encodingFile.Key];

                if (idxEntry.Size != 0)
                    encodingFile.LoadEntries(dataFiles[idxEntry.Index], idxEntry);
            });

            // Get root key
            var rootKey = buildConfig["root"][0];

            if (rootKey.Length / 2 > 16)
                throw new InvalidOperationException("Root key too long");
            else if (rootKey.Length / 2 < 16)
                throw new InvalidOperationException("Root key too short");

            rootFile = new RootFile();

            idxFiles.ForEach(idx =>
            {
                var encodingEntry = encodingFile[rootKey.ToByteArray()];

                if (encodingEntry.Size != 0 && encodingEntry.Keys.Length > 0)
                {
                    var idxEntry = idx[encodingEntry.Keys[0].Slice(0, 9)];

                    if (idxEntry.Size != 0)
                        rootFile.LoadEntries(dataFiles[idxEntry.Index], idxEntry);
                }
            });
        }

        public void InitConfigKeys()
        {
            buildInfo = new BuildInfo(BasePath + "/.build.info");

            var buildConfigKey = buildInfo["Build Key"];

            if (buildConfigKey != null)
            {
                if (buildConfigKey.Length / 2 > 16)
                    throw new InvalidOperationException("Build config key too long");
                else if (buildConfigKey.Length / 2 < 16)
                    throw new InvalidOperationException("Build config key too short");

                buildConfig = new BuildConfig(BasePath, buildConfigKey);

                if (buildConfig == null)
                    throw new InvalidOperationException("Can't create build config.");
            }

            var cdnConfigKey = buildInfo["CDN Key"];

            if (cdnConfigKey != null)
            {
                if (cdnConfigKey.Length / 2 > 16)
                    throw new InvalidOperationException("CDN config key too long");
                else if (cdnConfigKey.Length / 2 < 16)
                    throw new InvalidOperationException("CDN config key too short");

                cdnConfig = new CDNConfig(BasePath, cdnConfigKey);

                cdnConfig.Path = buildInfo["CDN Path"];
                cdnConfig.Host = buildInfo["CDN Hosts"].Split(new[] { ' ' })[0];

                if (cdnConfig == null)
                    throw new InvalidOperationException("Can't create cdn config.");
            }
        }

        public MemoryStream ReadFile(RootEntry[] rootEntries, Locales locales = Locales.EnUS)
        {
            for (var i = 0; i < rootEntries.Length; i++)
            {
                if ((rootEntries[i].Locales & locales) == locales || rootEntries[i].Locales == Locales.All_WoW)
                {
                    var encodingEntry = encodingFile[rootEntries[i].MD5];

                    if (encodingEntry.Size != 0 && encodingEntry.Keys.Length > 0)
                    {
                        for (var j = 0; j < 0x10; j++)
                        {
                            IndexEntry idxEntry = default(IndexEntry);

                            foreach (var k in encodingEntry.Keys)
                            {
                                if ((idxEntry = idxFiles[j][k.Slice(0, 9)]).Size != 0)
                                {
                                    var dataFile = dataFiles[idxEntry.Index];

                                    if (dataFile == null)
                                        throw new InvalidOperationException("Invalid data file.");

                                    var ret = DataFile.LoadBLTEEntry(idxEntry, dataFile.readStream);

                                    if (ret == null)
                                        break;

                                    return ret;
                                }
                            }

                            if (idxEntry.Size != 0)
                                break;
                        }

                        // CDN indices
                        for (var j = 0x10; j < idxFiles.Count; j++)
                        {
                            IndexEntry idxEntry = default(IndexEntry);

                            foreach (var k in encodingEntry.Keys)
                            {
                                if ((idxEntry = idxFiles[j][k]).Size != 0)
                                    return DataFile.LoadBLTEEntry(idxEntry, cdnConfig.DownloadFile(indexFiles[idxEntry.Index], idxEntry));
                            }

                            if (idxEntry.Size != 0)
                                break;
                        }
                    }
                }
            }

            return null;
        }

        public MemoryStream ReadFile(string name, Locales locales = Locales.EnUS)
        {
            var hash = lookup3.Hash(name.ToUpperInvariant());

            return ReadFile(rootFile[hash], locales);
        }

        public IEnumerable<Tuple<ulong, MemoryStream>> ReadFile(Locales locales = Locales.EnUS)
        {
            foreach (var entry in rootFile.Entries)
            {
                var rootEntries = rootFile[entry.Key];

                for (var i = 0; i < rootEntries.Length; i++)
                {
                    if ((rootEntries[i].Locales & locales) == locales)
                    {
                        var encodingEntry = encodingFile[rootEntries[i].MD5];

                        if (encodingEntry.Size != 0 && encodingEntry.Keys.Length > 0)
                        {
                            MemoryStream blteStream = null;

                            for (var j = 0; j < 0x10; j++)
                            {
                                IndexEntry idxEntry = default(IndexEntry);

                                foreach (var k in encodingEntry.Keys)
                                {
                                    if ((idxEntry = idxFiles[j][k.Slice(0, 9)]).Size != 0)
                                    {
                                        var dataFile = dataFiles[idxEntry.Index];

                                        if (dataFile == null)
                                            throw new InvalidOperationException("Invalid data file.");

                                        yield return Tuple.Create(entry.Key, blteStream = DataFile.LoadBLTEEntry(idxEntry, dataFile.readStream));
                                    }
                                }

                                if (idxEntry.Size != 0)
                                    break;
                            }

                            if (blteStream == null)
                            {
                                for (var j = 0x10; j < idxFiles.Count; j++)
                                {
                                    IndexEntry idxEntry = default(IndexEntry);

                                    foreach (var k in encodingEntry.Keys)
                                    {
                                        if ((idxEntry = idxFiles[j][k]).Size != 0)
                                            yield return Tuple.Create(entry.Key, DataFile.LoadBLTEEntry(idxEntry, cdnConfig.DownloadFile(indexFiles[idxEntry.Index], idxEntry)));
                                    }

                                    if (idxEntry.Size != 0)
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //return null;
        }

        public ConcurrentDictionary<ulong, MemoryStream> ReadFiles(byte[] signature, Locales locales = Locales.EnUS)
        {
            var files = new ConcurrentDictionary<ulong, MemoryStream>();

            foreach (var entry in rootFile.Entries)
            {
                var rootEntries = rootFile[entry.Key];

                for (var i = 0; i < rootEntries.Length; i++)
                {
                    if ((rootEntries[i].Locales & locales) == locales)
                    {
                        var encodingEntry = encodingFile[rootEntries[i].MD5];

                        if (encodingEntry.Size != 0 && encodingEntry.Keys.Length > 0)
                        {
                            for (var j = 0; j < 0x10; j++)
                            {
                                IndexEntry idxEntry = default(IndexEntry);

                                foreach (var k in encodingEntry.Keys)
                                {
                                    if ((idxEntry = idxFiles[j][k.Slice(0, 9)]).Size != 0)
                                    {
                                        var dataFile = dataFiles[idxEntry.Index];

                                        if (dataFile == null)
                                            throw new InvalidOperationException("Invalid data file.");

                                        var sigBuffer = new byte[signature.Length];
                                        var stream = DataFile.LoadBLTEEntry(idxEntry, dataFile.readStream);

                                        stream?.Read(sigBuffer, 0, sigBuffer.Length);

                                        if (sigBuffer.Compare(signature))
                                            files.TryAdd(entry.Key, stream);
                                    }
                                }

                                if (idxEntry.Size != 0)
                                    break;
                            }

                            for (var j = 0x10; j < idxFiles.Count; j++)
                            {
                                IndexEntry idxEntry = default(IndexEntry);

                                foreach (var k in encodingEntry.Keys)
                                {
                                    if ((idxEntry = idxFiles[j][k]).Size != 0)
                                    {
                                        var sigBuffer = new byte[signature.Length];
                                        var stream = DataFile.LoadBLTEEntry(idxEntry, cdnConfig.DownloadFile(indexFiles[idxEntry.Index], idxEntry));

                                        stream?.Read(sigBuffer, 0, sigBuffer.Length);

                                        if (sigBuffer.Compare(signature))
                                            files.TryAdd(entry.Key, stream);
                                    }
                                }

                                if (idxEntry.Size != 0)
                                    break;
                            }
                        }
                    }
                }
            }

            return files;
        }

        public IEnumerable<Tuple<string, MemoryStream>> ReadFiles(string[] names, Locales locales = Locales.EnUS)
        {
            for (var i = 0; i < names.Length; i++)
                yield return Tuple.Create(names[i], ReadFile(names[i], locales));
        }

        public void Dispose()
        {
            
        }
    }
}
