using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDBXEditor.Storage;

namespace WDBXEditor.Reader.FileTypes
{
    public class WDBC : DBHeader
    {
        // A raw layout is used only when the selected WotLK build has no definition.
        // On-disk values remain opaque: no string-offset or float interpretation is guessed.
        public bool IsRawLayout { get; set; }
        public byte[] OriginalStringBlock { get; set; } = new byte[0];
        public int[] EditableStringIndices { get; set; } = new int[0];
        public string[] EditableStringColumns { get; set; } = new string[0];
        public Dictionary<DataRow, string[]> OriginalEditableStrings { get; } = new Dictionary<DataRow, string[]>();
        public int[] EditableFloatIndices { get; set; } = new int[0];
        public string[] EditableFloatColumns { get; set; } = new string[0];
        public Dictionary<DataRow, float[]> OriginalEditableFloats { get; } = new Dictionary<DataRow, float[]>();
        public override void ReadHeader(ref BinaryReader dbReader, string signature)
        {
            base.ReadHeader(ref dbReader, signature);
        }
    }
}
