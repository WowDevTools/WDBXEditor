using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDBXEditor.Storage;
using System.Data;
using System.Diagnostics;

namespace WDBXEditor.Reader.FileTypes
{
    public class WDB2 : DBHeader
    {
        public int Build { get; set; }
        public int TimeStamp { get; set; }
        public int[] IndexMap { get; set; } //Maps index to row for all indicies between min and max
        public short[] StringLengths { get; set; } //Length of each string including the 0 byte character

        public override bool ExtendedStringTable => Build > 18273; //WoD has two null bytes

        public override void ReadHeader(ref BinaryReader dbReader, string signature)
        {
            base.ReadHeader(ref dbReader, signature);

            TableHash = dbReader.ReadUInt32();
            Build = dbReader.ReadInt32();
            TimeStamp = dbReader.ReadInt32();
            MinId = dbReader.ReadInt32();
            MaxId = dbReader.ReadInt32();
            Locale = dbReader.ReadInt32();
            CopyTableSize = dbReader.ReadInt32();

            if (MaxId != 0 && Build > 12880)
            {
                int diff = MaxId - MinId + 1; //Calculate the array sizes
                IndexMap = new int[diff];
                StringLengths = new short[diff];

                //Populate the arrays
                for (int i = 0; i < diff; i++)
                    IndexMap[i] = dbReader.ReadInt32();

                for (int i = 0; i < diff; i++)
                    StringLengths[i] = dbReader.ReadInt16();
            }
        }

        public override void WriteHeader(BinaryWriter bw, DBEntry entry)
        {
            base.WriteHeader(bw, entry);

            Tuple<int, int> minmax = entry.MinMax();
            if (MaxId == 0) //Irrelevant if header doesn't use this
                minmax = new Tuple<int, int>(0, 0);

            bw.Write(TableHash);
            bw.Write(Build);
            bw.Write(TimeStamp);

            bw.Write(minmax.Item1);
            bw.Write(minmax.Item2);

            bw.Write(Locale);
            bw.Write(CopyTableSize);

            if (MaxId != 0 && Build > 12880)
            {
                List<int> IndiciesTable = new List<int>();
                List<short> StringLengthTable = new List<short>();

                Dictionary<int, short> stringlengths = entry.GetStringLengths();
                int x = 0;
                for (int i = minmax.Item1; i <= minmax.Item2; i++)
                {
                    if (stringlengths.ContainsKey(i))
                    {
                        StringLengthTable.Add(stringlengths[i]);
                        IndiciesTable.Add(++x);
                    }
                    else
                    {
                        IndiciesTable.Add(0);
                        StringLengthTable.Add(0);
                    }
                }

                //Write the data
                bw.WriteArray(IndiciesTable.ToArray());
                bw.WriteArray(StringLengthTable.ToArray());                    
            }
        }
    }
}
