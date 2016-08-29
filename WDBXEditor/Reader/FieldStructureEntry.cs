using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBXEditor.Reader
{
    public class FieldStructureEntry
    {
        public short Bits;
        public ushort Count;
        public int ByteCount
        {
            get
            {
                int value = (32 - Bits) >> 3;
                return (value < 0 ? Math.Abs(value) + 4 : value);
            }
        }

        public FieldStructureEntry(short bits, ushort count)
        {
            this.Bits = bits;
            this.Count = count;
        }
    }
}
