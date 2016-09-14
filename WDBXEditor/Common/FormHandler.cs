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
        private static Main parent;
        private static Dictionary<Type, Form> _Forms = new Dictionary<Type, Form>();

        static FormHandler()
        {
            parent = (Main)Application.OpenForms.Cast<Form>().First(x => x.GetType() == typeof(Main));
        }

        public static T Show<T>() where T : Form, new()
        {
            if (typeof(T) == typeof(FindReplace))
                throw new TypeLoadException();

            var type = typeof(T);
            Form f = null;
            if (_Forms.TryGetValue(type, out f))
            {
                f.BringToFront();
            }
            else
            {
                f = new T();
                f.FormClosing += (s, e) => _Forms.Remove(s.GetType());
                _Forms.Add(type, f);
                f.Show(parent);
            }
            return (T)f;
        }

        public static void ShowReplaceForm(bool replace = false)
        {
            if (form == null)
            {
                form = new FindReplace();
                form.TopMost = true;
                form.Replace = replace;
                form.Show(parent);
                form.FormClosed += delegate { form = null; };
            }
            else
            {
                form.SetScreenType(replace);
                form.Activate();
                form.TopMost = true;
            }
        }

        public static void Close()
        {
            form?.Close();
            foreach (var form in _Forms.Values)
                form?.Close();
        }
    }
}
