using System.Collections.Generic;
using System.IO;
using WDBXEditor.Archives.CASC.Structures;
using WDBXEditor.Archives.Misc;

namespace WDBXEditor.Archives.CASC.Handlers
{
    public class IndexFile
    {
        public IndexEntry this[byte[] hash]
        {
            get
            {
                IndexEntry entry;

                if (entries.TryGetValue(hash, out entry))
                    return entry;

                return default(IndexEntry);
            }
        }

        public Dictionary<byte[], IndexEntry> entries = new Dictionary<byte[], IndexEntry>(new ByteArrayComparer());

        public IndexFile(string idx, bool cdnIndex = false, ushort fileIndex = 0)
        {
            if (cdnIndex)
            {
                var nullHash = new byte[16];

                using (var br = new BinaryReader(File.OpenRead(idx)))
                {
                    br.BaseStream.Position = br.BaseStream.Length - 12;

                    var entries = br.ReadUInt32();

                    br.BaseStream.Position = 0;

                    for (var i = 0; i < entries; i++)
                    {
                        var hash = br.ReadBytes(16);

                        if (hash.Compare(nullHash))
                            hash = br.ReadBytes(16);

                        var entry = new IndexEntry
                        {
                            Index = fileIndex,
                            Size = br.ReadBEUInt32(),
                            Offset = br.ReadBEUInt32()
                        };

                        if (this.entries.ContainsKey(hash))
                            continue;

                        this.entries.Add(hash, entry);
                    }
                }
            }
            else
            {
                using (var br = new BinaryReader(File.OpenRead(idx)))
                {
                    br.BaseStream.Position = 0x20;

                    var dataLength = br.ReadUInt32();

                    br.BaseStream.Position += 4;

                    // 18 bytes per entry.
                    for (var i = 0; i < dataLength / 18; i++)
                    {
                        var hash = br.ReadBytes(9);
                        var index = br.ReadByte();
                        var offset = br.ReadBEUInt32();

                        var entry = new IndexEntry();

                        entry.Size = br.ReadUInt32();
                        entry.Index = (ushort)((ushort)(index << 2) | (offset >> 30));
                        entry.Offset = (uint)(offset & 0x3FFFFFFF);

                        if (entries.ContainsKey(hash))
                            continue;

                        entries.Add(hash, entry);
                    }
                }
            }
        }
    }
}
