using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WDBXEditor.ConsoleHandler;

namespace WDBXEditor
{
    static class Program
    {
        public static bool PrimaryInstance = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            InstanceManager.InstanceCheck(args); //Check to see if we can run this instance

            SetDllDirectory(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), (Environment.Is64BitProcess ? "x64" : "x86")));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length > 0)
            {
                ConsoleManager.LoadCommandDefinitions();

                if (ConsoleManager.CommandHandlers.ContainsKey(args[0].ToLower()))
                {
                    if (!AttachConsole(-1)) //Attempt to attach to existing console window
                        AllocConsole(); //Create a new console

                    ConsoleManager.ConsoleMain(args); //Console mode
                }
                else
                {
                    Application.Run(new Main(args)); //Load file(s)
                }
            }
            else
            {
                Application.Run(new Main()); //Default
            }            

            InstanceManager.Stop();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDllDirectory(string path);
        [DllImport("kernel32")]
        private static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int pid);        
    }
}
