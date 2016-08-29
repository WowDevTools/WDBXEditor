using System.Collections.Generic;
using System.IO;
using System.Linq;
using WDBXEditor.Archives.CASC.Constants;
using WDBXEditor.Archives.CASC.Structures;

namespace WDBXEditor.Archives.CASC.Handlers
{
    public class RootFile
    {
        public ILookup<ulong, RootEntry> Entries => entries;
        public RootEntry[] this[ulong hash] => entries.Contains(hash) ? entries[hash].ToArray() : new RootEntry[0];

        ILookup<ulong, RootEntry> entries;

        public void LoadEntries(DataFile file, IndexEntry indexEntry)
        {
            var list = new List<RootEntry>();
            var blteEntry = new BinaryReader(DataFile.LoadBLTEEntry(indexEntry, file.readStream));

            while (blteEntry.BaseStream.Position < blteEntry.BaseStream.Length)
            {
                var entries = new RootEntry[blteEntry.ReadInt32()];

                blteEntry.BaseStream.Position += 4;

                var locales = (Locales)blteEntry.ReadUInt32();

                blteEntry.BaseStream.Position += (entries.Length << 2);

                for (var i = 0; i < entries.Length; i++)
                {
                    list.Add(new RootEntry
                    {
                        MD5 = blteEntry.ReadBytes(16),
                        Hash = blteEntry.ReadUInt64(),
                        Locales = locales
                    });
                }
            }

            entries = list.ToLookup(re => re.Hash);
        }
    }
}
