using System.Collections;
using System.Collections.Generic;

namespace WDBXEditor.Archives.Misc
{
    public class ByteArrayComparer : IEqualityComparer, IEqualityComparer<object>
    {
        public new bool Equals(object x, object y)
        {
            var eq = x as IStructuralEquatable;
            return eq == null ? object.Equals(x, y) : eq.Equals(y, this);
        }

        public int GetHashCode(object obj)
        {
            var eq = obj as IStructuralEquatable;
            return eq == null ? EqualityComparer<object>.Default.GetHashCode(obj) : eq.GetHashCode(this);
        }
    }
}
