using System.Collections.Generic;
using System.IO;
using WDBXEditor.Archives.CASC.Structures;
using WDBXEditor.Archives.Misc;

namespace WDBXEditor.Archives.CASC.Handlers
{
    public class EncodingFile
    {
        public EncodingEntry this[byte[] md5]
        {
            get
            {
                EncodingEntry entry;

                if (entries.TryGetValue(md5, out entry))
                    return entry;

                return default(EncodingEntry);
            }
        }

        public byte[] Key { get; }

        Dictionary<byte[], EncodingEntry> entries = new Dictionary<byte[], EncodingEntry>(new ByteArrayComparer());

        public EncodingFile(byte[] encodingKey)
        {
            Key = encodingKey.Slice(0, 9);
        }

        public void LoadEntries(DataFile file, IndexEntry indexEntry)
        {
            var blteEntry = new BinaryReader(DataFile.LoadBLTEEntry(indexEntry, file.readStream));

            blteEntry.BaseStream.Position = 9;

            var entries = blteEntry.ReadBEUInt32();

            blteEntry.BaseStream.Position += 5;

            var offsetEntries = blteEntry.ReadBEUInt32();

            blteEntry.BaseStream.Position += offsetEntries + (entries << 5);

            for (var i = 0; i < entries; i++)
            {
                var keys = blteEntry.ReadUInt16();

                while (keys != 0)
                {
                    var encodingEntry = new EncodingEntry
                    {
                        Keys = new byte[keys][],
                        Size = blteEntry.ReadBEUInt32()
                    };

                    var md5 = blteEntry.ReadBytes(16);

                    for (var j = 0; j < keys; j++)
                        encodingEntry.Keys[j] = blteEntry.ReadBytes(16);

                    this.entries.Add(md5, encodingEntry);

                    keys = blteEntry.ReadUInt16();
                }

                while (blteEntry.ReadByte() == 0);

                blteEntry.BaseStream.Position -= 1;
            }

        }
    }
}
