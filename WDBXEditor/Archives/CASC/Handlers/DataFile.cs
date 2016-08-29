using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using WDBXEditor.Archives.CASC.Structures;
using WDBXEditor.Archives.Misc;

namespace WDBXEditor.Archives.CASC.Handlers
{
    public class DataFile
    {
        public readonly BinaryReader readStream;

        static object readLock = new object();

        public DataFile(Stream data)
        {
            readStream = new BinaryReader(data);
        }

        public static MemoryStream LoadBLTEEntry(IndexEntry idxEntry, BinaryReader readStream = null)
        {
            lock (readLock)
            {
                if (readStream == null)
                    return null;

                readStream.BaseStream.Position = idxEntry.Offset + 30;

                if (readStream.ReadUInt32() != 0x45544C42)
                {
                    Trace.TraceError($"data.{idxEntry.Index:000}: Invalid BLTE signature at 0x{readStream.BaseStream.Position:X8}.");

                    return null;
                }

                var blte = new BLTEEntry();
                var frameHeaderLength = readStream.ReadBEUInt32();
                var chunks = 0u;
                var size = 0L;

                if (frameHeaderLength == 0)
                {
                    chunks = 1;
                    size = idxEntry.Size - 38;
                }
                else
                {
                    readStream.BaseStream.Position += 1;

                    chunks = readStream.ReadUInt24();
                }

                blte.Chunks = new BLTEChunk[chunks];

                for (var i = 0; i < chunks; i++)
                {
                    if (frameHeaderLength == 0)
                    {
                        blte.Chunks[i].CompressedSize = size;
                        blte.Chunks[i].UncompressedSize = size - 1;
                    }
                    else
                    {
                        blte.Chunks[i].CompressedSize = readStream.ReadBEUInt32();
                        blte.Chunks[i].UncompressedSize = readStream.ReadBEUInt32();

                        // Skip MD5 hash
                        readStream.BaseStream.Position += 16;
                    }
                }

                var data = new MemoryStream();

                for (var i = 0; i < chunks; i++)
                {
                    var formatCode = readStream.ReadByte();
                    var dataBytes = readStream.ReadBytes((int)blte.Chunks[i].CompressedSize - 1);

                    // Not compressed
                    if (formatCode == 0x4E)
                        data.Write(dataBytes, 0, (int)blte.Chunks[i].UncompressedSize);
                    // Compressed
                    else if (formatCode == 0x5A)
                    {
                        using (var decompressed = new MemoryStream())
                        {
                            using (var inflate = new DeflateStream(new MemoryStream(dataBytes, 2, dataBytes.Length - 2), CompressionMode.Decompress))
                                inflate.CopyTo(decompressed);

                            var inflateData = decompressed.ToArray();
                            data.Write(inflateData, 0, inflateData.Length);
                        }
                    }
                }

                data.Position = 0;


                return data;
            }
        }
    }
}
