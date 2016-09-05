using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDBXEditor.Storage;

namespace WDBXEditor.ConsoleHandler
{
    public static class ConsoleManager
    {
        public static bool ConsoleMode { get; set; } = false;
        public static bool IsClosing { get; set; } = true;

        public static Dictionary<string, HandleCommand> CommandHandlers = new Dictionary<string, HandleCommand>();
        public delegate void HandleCommand(string[] args);

        public static void ConsoleMain(string[] args)
        {
            Database.LoadDefinitions().Wait();

            if (CommandHandlers.ContainsKey(args[0].ToLower()))
                InvokeHandler(args[0], args.Skip(1).ToArray());

            while (!IsClosing)
            {
                args = Console.ReadLine().Split(' ');
                if (CommandHandlers.ContainsKey(args[0].ToLower()))
                    InvokeHandler(args[0], args.Skip(1).ToArray());
            }

        }

        public static bool InvokeHandler(string command, params string[] args)
        {
            try
            {
                command = command.ToLower();
                if (CommandHandlers.ContainsKey(command))
                {
                    CommandHandlers[command].Invoke(args);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("");
                return false;
            }
        }

        public static void LoadCommandDefinitions()
        {
            //Argument commands
            DefineCommand("-console", ConsoleManager.LoadConsoleMode);
            DefineCommand("-export", ConsoleCommands.ExportArgCommand);
            DefineCommand("-sqldump", ConsoleCommands.SqlDumpArgCommand);
            DefineCommand("-extract", ConsoleCommands.ExtractCommand);

            //Console commands
            DefineCommand("export", ConsoleCommands.ExportConCommand);
            DefineCommand("sqldump", ConsoleCommands.SqlDumpConCommand);
            DefineCommand("extract", ConsoleCommands.ExtractCommand);
            DefineCommand("load", ConsoleCommands.LoadCommand);
            DefineCommand("help", ConsoleCommands.HelpCommand);
            DefineCommand("exit", delegate { Environment.Exit(0); });
            DefineCommand("gui", ConsoleManager.LoadGUI);
        }

        private static void LoadConsoleMode(string[] args)
        {
            IsClosing = false;
            ConsoleMode = true;

            Console.WriteLine("   WDBX Editor - Console Mode");
            Console.WriteLine("Type help to see a list of available commands.");
            Console.WriteLine("");

            //Remove argument methods
            foreach (var k in CommandHandlers.Keys.ToList())
                if (k[0] == '-')
                    CommandHandlers.Remove(k);
        }

        [ConsoleHelp("Loads the GUI version of the program", "", "")]
        private static void LoadGUI(string[] args)
        {
            Process.Start(System.Windows.Forms.Application.ExecutablePath);
            Environment.Exit(0);
        }

        /// <summary>
        /// Converts args into keyvalue pair
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseCommand(string[] args)
        {
            Dictionary<string, string> keyvalues = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i++)
            {
                if (i == args.Length - 1)
                    break;

                string key = args[i].ToLower();
                string value = args[++i];
                if (value[0] == '"' && value[value.Length - 1] == '"')
                    value = value.Substring(1, value.Length - 2);

                keyvalues.Add(key, value);
            }

            return keyvalues;
        }

        private static void DefineCommand(string command, HandleCommand handler)
        {
            CommandHandlers[command.ToLower()] = handler;
        }
    }
}
