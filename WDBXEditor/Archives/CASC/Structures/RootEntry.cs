using WDBXEditor.Archives.CASC.Constants;

namespace WDBXEditor.Archives.CASC.Structures
{
    public struct RootEntry
    {
        public byte[] MD5 { get; set; }
        public ulong Hash { get; set; }
        public Locales Locales { get; set; }
    }
}
