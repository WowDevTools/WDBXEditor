using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WDBXEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            SetDllDirectory(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), (Environment.Is64BitProcess ? "x64" : "x86")));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length > 0)
                Application.Run(new Main(args[0]));
            else
                Application.Run(new Main());           
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDllDirectory(string path);
    }
}
