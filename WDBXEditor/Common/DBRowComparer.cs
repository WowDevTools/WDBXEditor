using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBXEditor.Common
{
    public class DBRowComparer : IEqualityComparer<DataRow>
    {
        public int IdColumnIndex => idcolumn;
        private int idcolumn = 0;

        public DBRowComparer(int idcolumn)
        {
            this.idcolumn = idcolumn;
        }

        public bool Equals(DataRow x, DataRow y)
        {
            var xa = x.ItemArray;
            var ya = y.ItemArray;

            for (int i = 0; i < xa.Length; i++)
            {
                if (idcolumn == i) continue;
                if (!xa[i].Equals(ya[i])) return false;
            }

            return true;
        }

        public int GetHashCode(DataRow obj)
        {
            int result = 0;
            var items = obj.ItemArray;
            for (int i = 0; i < items.Length; i++)
            {
                if (idcolumn == i)
                    continue;

                result ^= items[i].GetHashCode();
            }
            return result;
        }
    }
}
