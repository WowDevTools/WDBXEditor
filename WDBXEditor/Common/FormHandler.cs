using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WDBXEditor.Storage;

namespace WDBXEditor.Common
{
    static class FormHandler
    {
        private static FindReplace form;
        private static ADGV.AdvancedDataGridView gridview;
        private static bool initialised = false;

        public static void Init(ADGV.AdvancedDataGridView adgv)
        {
            gridview = adgv;
            initialised = true;
        }

        public static void ShowReplaceForm(bool replace = false)
        {
            if (!initialised) return;

            if (form == null)
            {
                form = new FindReplace(ref gridview);
                form.TopMost = true;
                form.Replace = replace;
                form.Show();
                form.FormClosed += delegate { form = null; };
            }
            else
            {
                if (form.Replace && !replace)
                    form.SetScreenType(false);
                else if (!form.Replace && replace)
                    form.SetScreenType(true);

                form.Activate();
                form.TopMost = true;
            }
        }

        public static void Close()
        {
            form?.Close();
        }
    }
}
