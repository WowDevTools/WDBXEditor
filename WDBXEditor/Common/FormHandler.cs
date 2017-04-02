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
        private static Main _Parent;
        private static Dictionary<Type, Form> _Forms = new Dictionary<Type, Form>();

        static FormHandler()
        {
            _Parent = GetForm<Main>();
        }

        public static T Show<T>(params object[] args) where T : Form, new()
        {
            var type = typeof(T);
            bool findreplace = typeof(T) == typeof(FindReplace);
            Form f = null;

            if (findreplace) //FindReplace argument check
            {
                if (args.Length == 0)
                    throw new ArgumentException("FindReplace requires Screen Type argument.");
                if (args[0].GetType() != typeof(bool))
                    throw new ArgumentException("FindReplace argument should be a boolean.");
            }

            if (_Forms.TryGetValue(type, out f))
            {
                f.BringToFront();
                f.Activate();
            }
            else
            {
                f = new T();
                f.FormClosing += (s, e) => _Forms.Remove(s.GetType());
                f.TopMost = true;
                f.Show(_Parent);

                _Forms.Add(type, f);
            }

            if (findreplace)
                ((FindReplace)f).SetScreenType((bool)args[0]); //Set FindReplace screen type

            return (T)f;
        }

        public static T GetForm<T>() where T : Form
        {
            return Application.OpenForms.Cast<Form>().FirstOrDefault(x => x.GetType() == typeof(T)) as T;
        }

        public static void Close<T>()
        {
            var type = typeof(T);
            Form f = null;
            if (_Forms.TryGetValue(type, out f))
                f?.Close();
        }

        public static void Close()
        {
            foreach (var form in _Forms.Values)
                form?.Close();
        }
    }
}
