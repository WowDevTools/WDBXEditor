using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBXEditor.Common
{
    public class ORowComparer : IEqualityComparer<ORow>
    {
        public bool Equals(ORow x, ORow y)
        {
            var xa = x.Array;
            var ya = y.Array;

            for (int i = 0; i < xa.Length; i++)
                if (!xa[i].Equals(ya[i]))
                    return false;

            return true;
        }

        public int GetHashCode(ORow obj)
        {
            unchecked
            {
                var a = obj.Array;
                int hash = (int)2166136261;
                for (int i = 0; i < a.Length; i++)
                    hash = (hash * 16777619) ^ a[i].GetHashCode();
                return hash;
            }
        }
    }

    public class OArrayComparer : IEqualityComparer<object[]>
    {
        public bool Equals(object[] x, object[] y)
        {
            for (int i = 0; i < x.Length; i++)
                if (!x[i].Equals(y[i]))
                    return false;

            return true;
        }

        public int GetHashCode(object[] obj)
        {
            unchecked
            {
                int hash = (int)2166136261;
                for (int i = 0; i < obj.Length; i++)
                    hash = (hash * 16777619) ^ obj[i].GetHashCode();
                return hash;
            }
        }
    }

    public class ORow
    {
        public int Index;
        public object[] Array;

        public ORow(int index, object[] array)
        {
            Index = index;
            Array = array;
        }
    }


}
