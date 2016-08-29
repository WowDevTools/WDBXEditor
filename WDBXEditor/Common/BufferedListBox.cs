using System.Windows.Forms;

namespace WDBXEditor.Common
{
    class BufferedListBox : ListBox
    {
        public BufferedListBox()
        {
            this.DoubleBuffered = true;
        }
    }
}
