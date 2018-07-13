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
            InstanceManager.LoadDll("StormLib.dll"); //Loads the correct StormLib library

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			UpdateManager.Clean();

            if (args != null && args.Length > 0)
            {
                ConsoleManager.LoadCommandDefinitions();

                if (ConsoleManager.CommandHandlers.ContainsKey(args[0].ToLower()))
                    ConsoleManager.ConsoleMain(args); //Console mode
                else
                    Application.Run(new Main(args)); //Load file(s)
            }
            else
            {
                Application.Run(new Main()); //Default
            }            

            InstanceManager.Stop();
        }    
    }
}
