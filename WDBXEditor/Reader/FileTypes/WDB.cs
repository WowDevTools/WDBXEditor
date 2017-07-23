using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDBXEditor.Common;
using WDBXEditor.Storage;

namespace WDBXEditor.Reader.FileTypes
{
    public class WDB : DBHeader
    {
        public new string Locale { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Build { get; set; }

        public override void ReadHeader(ref BinaryReader dbReader, string signature)
        {
            this.Signature = signature;
            Build = dbReader.ReadInt32();
            Locale = dbReader.ReadString(4).Reverse();                  

            //WDB files < 1.6: Header length is 16 bytes
            //WDB files >= 1.6: Header length is 20 bytes(Verified till 1.9.4)
            //WDB files >= 3.0.8 - 9506: Header length is 24 bytes

            if (Build >= 4499)
                RecordSize = dbReader.ReadUInt32();

            if (Build >= 9506)
            {
                Unknown1 = dbReader.ReadInt32();
                Unknown2 = dbReader.ReadInt32();
            }
        }

        public byte[] ReadData(BinaryReader dbReader)
        {
            List<byte> data = new List<byte>();

            //Stored as Index, Size then Data
            while (dbReader.BaseStream.Position != dbReader.BaseStream.Length)
            {
                int index = dbReader.ReadInt32();
                if (index == 0 && dbReader.BaseStream.Position == dbReader.BaseStream.Length)
                    break;

                int size = dbReader.ReadInt32();
                if (index == 0 && size == 0 && dbReader.BaseStream.Position == dbReader.BaseStream.Length)
                    break;

                data.AddRange(BitConverter.GetBytes(index));
                data.AddRange(dbReader.ReadBytes(size));

                RecordCount++;
            }

            return data.ToArray();
        }
    }
}
