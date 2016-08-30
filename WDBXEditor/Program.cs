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

            if (args != null && args.Length > 0 && args[0] == "-console")
            {
                new MainConsole(args);
                return;
            }

            // Hide console window
            var handle = GetConsoleWindow();
            const int SW_HIDE = 0;
            //const int SW_SHOW = 5;
            ShowWindow(handle, SW_HIDE);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length > 0)
                Application.Run(new Main(args[0]));
            else
                Application.Run(new Main());           
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDllDirectory(string path);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
