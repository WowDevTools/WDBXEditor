using MySqlConnector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WDBXEditor.Archives.CASC.Handlers;
using WDBXEditor.Archives.MPQ;
using WDBXEditor.Common;
using WDBXEditor.Storage;
using static WDBXEditor.Common.Constants;

namespace WDBXEditor.ConsoleHandler
{
    class ConsoleCommands
    {
        #region Load
        /// <summary>
        /// Loads a file into the console
        /// <para>load -f "*.dbc" -s ".mpq/wow dir" -b 11802</para>
        /// </summary>
        /// <param name="args"></param>
        /// 
        public static void LoadCommand(string[] args)
        {
            var pmap = ConsoleManager.ParseCommand(args);
            string file = ParamCheck<string>(pmap, "-f");
            string filename = Path.GetFileName(file);
            string filenoext = Path.GetFileNameWithoutExtension(file);
            string source = ParamCheck<string>(pmap, "-s", false);
            int build = ParamCheck<int>(pmap, "-b");
            SourceType sType = GetSourceType(source);

            //Check file exists if loaded from the filesystem
            if (!File.Exists(file) && sType == SourceType.File)
                throw new Exception($"   File not found {file}.");

            //Check the required definition exists
            var def = Database.Definitions.Tables.FirstOrDefault(x => x.Build == build && x.Name.Equals(filenoext, IGNORECASE));
            if (def == null)
                throw new Exception($"   Could not find definition for {Path.GetFileName(file)} build {build}.");

            Database.BuildNumber = build;
            var dic = new ConcurrentDictionary<string, MemoryStream>();
            string error = string.Empty;

            switch (sType)
            {
                case SourceType.MPQ:
                    Console.WriteLine("Loading from MPQ archive...");
                    using (MpqArchive archive = new MpqArchive(source, FileAccess.Read))
                    {
                        string line = string.Empty;
                        bool loop = true;
                        using (MpqFileStream listfile = archive.OpenFile("(listfile)"))
                        using (StreamReader sr = new StreamReader(listfile))
                        {
                            while ((line = sr.ReadLine()) != null && loop)
                            {
                                if (line.EndsWith(filename, IGNORECASE))
                                {
                                    loop = false;
                                    var ms = new MemoryStream();
                                    archive.OpenFile(line).CopyTo(ms);
                                    dic.TryAdd(filename, ms);

                                    error = Database.LoadFiles(dic).Result.FirstOrDefault();
                                }
                            }
                        }
                    }
                    break;
                case SourceType.CASC:
                    Console.WriteLine("Loading from CASC directory...");
                    using (var casc = new CASCHandler(source))
                    {
                        string fullname = filename;
                        if (!fullname.StartsWith("DBFilesClient", IGNORECASE))
                            fullname = "DBFilesClient\\" + filename; //Ensure we have the current file name structure

                        var stream = casc.ReadFile(fullname);
                        if (stream != null)
                        {
                            dic.TryAdd(filename, stream);

                            error = Database.LoadFiles(dic).Result.FirstOrDefault();
                        }
                    }
                    break;
                default:
                    error = Database.LoadFiles(new string[] { file }).Result.FirstOrDefault();
                    break;
            }

            dic.Clear();

            if (!string.IsNullOrWhiteSpace(error))
                throw new Exception("   " + error);

            if (Database.Entries.Count == 0)
                throw new Exception("   File could not be loaded.");

            Console.WriteLine($"{Path.GetFileName(file)} loaded.");
            Console.WriteLine("");
        }

        public static void ExtractCommand(string[] args)
        {
            var pmap = ConsoleManager.ParseCommand(args);
            string filter = ParamCheck<string>(pmap, "-f", false);
            string source = ParamCheck<string>(pmap, "-s");
            string output = ParamCheck<string>(pmap, "-o");
            SourceType sType = GetSourceType(source);
            
            if (string.IsNullOrWhiteSpace(filter))
                filter = "*";

            string regexfilter = "(" + Regex.Escape(filter).Replace(@"\*", @".*").Replace(@"\?", ".") + ")";
            Func<string, bool> TypeCheck = t => Path.GetExtension(t).ToLower() == ".dbc" || Path.GetExtension(t).ToLower() == ".db2";

            
            var dic = new ConcurrentDictionary<string, MemoryStream>();
            switch (sType)
            {
                case SourceType.MPQ:
                    Console.WriteLine("Loading from MPQ archive...");
                    using (MpqArchive archive = new MpqArchive(source, FileAccess.Read))
                    {
                        string line = string.Empty;
                        using (MpqFileStream listfile = archive.OpenFile("(listfile)"))
                        using (StreamReader sr = new StreamReader(listfile))
                        {
                            while ((line = sr.ReadLine()) != null)
                            {
                                if(TypeCheck(line) && Regex.IsMatch(line, regexfilter, RegexOptions.Compiled | RegexOptions.IgnoreCase))
                                {
                                    var ms = new MemoryStream();
                                    archive.OpenFile(line).CopyTo(ms);
                                    dic.TryAdd(Path.GetFileName(line), ms);
                                }
                            }
                        }
                    }
                    break;
                case SourceType.CASC:
                    Console.WriteLine("Loading from CASC directory...");
                    using (var casc = new CASCHandler(source))
                    {
                        var files = Constants.ClientDBFileNames.Where(x => Regex.IsMatch(Path.GetFileName(x), regexfilter, RegexOptions.Compiled | RegexOptions.IgnoreCase));
                        foreach(var file in files)
                        {
                            var stream = casc.ReadFile(file);
                            if (stream != null)
                                dic.TryAdd(Path.GetFileName(file), stream);
                        }
                    }
                    break;
            }

            if (dic.Count == 0)
                throw new Exception("   No matching files found.");

            if (!Directory.Exists(output))
                Directory.CreateDirectory(output);

            foreach(var d in dic)
            {
                using (var fs = new FileStream(Path.Combine(output, d.Key), FileMode.Create))
                {
                    fs.Write(d.Value.ToArray(), 0, (int)d.Value.Length);
                    fs.Close();
                }
            }

            dic.Clear();

            Console.WriteLine($"   Successfully extracted files.");
            Console.WriteLine("");
        }
        #endregion
        
        #region Export
        /// <summary>
        /// Exports a file to either SQL, JSON or CSV
        /// <para>-export -f "*.dbc" -s ".mpq/wow dir" -b 11802 -o "*.sql|*.csv"</para>
        /// </summary>
        /// <param name="args"></param>
        public static void ExportArgCommand(string[] args)
        {
            var pmap = ConsoleManager.ParseCommand(args);
            string output = ParamCheck<string>(pmap, "-o");
            OutputType oType = GetOutputType(output);

            LoadCommand(args);

            var entry = Database.Entries[0];
            using (FileStream fs = new FileStream(output, FileMode.Create))
            {
                byte[] data = new byte[0];
                switch (oType)
                {
                    case OutputType.CSV:
                        data = Encoding.UTF8.GetBytes(entry.ToCSV());
                        break;
                    case OutputType.JSON:
                        data = Encoding.UTF8.GetBytes(entry.ToJSON());
                        break;
                    case OutputType.SQL:
                        data = Encoding.UTF8.GetBytes(entry.ToSQL());
                        break;
                }

                fs.Write(data, 0, data.Length);

                Console.WriteLine($"Successfully exported to {output}.");
            }
        }

        #endregion
        
        #region SQL Dump
        /// <summary>
        /// Exports a file directly into a SQL database
        /// <para>-sqldump -f "*.dbc" -s ".mpq/wow dir" -b 11802 -c "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;"</para>
        /// </summary>
        /// <param name="args"></param>
        public static void SqlDumpArgCommand(string[] args)
        {
            var pmap = ConsoleManager.ParseCommand(args);
            string connection = ParamCheck<string>(pmap, "-c");

            LoadCommand(args);

            var entry = Database.Entries[0];
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    conn.Open();
                }
                catch { throw new Exception("   Incorrect MySQL login details."); }

                entry.ToSQLTable(connection);

                Console.WriteLine($"Successfully exported to {conn.Database}.");
            }
        }

        #endregion  

        #region Helpers
        private static T ParamCheck<T>(Dictionary<string, string> map, string field, bool required = true)
        {
            if (map.ContainsKey(field))
            {
                try
                {
                    return (T)Convert.ChangeType(map[field], typeof(T));
                }
                catch
                {
                    if (required) throw new Exception($"   Parameter {field} is invalid");
                }
            }

            if (required)
                throw new Exception($"   Missing parameter '{field}'");

            object defaultval = (typeof(T) == typeof(string) ? (object)string.Empty : (object)0);
            return (T)Convert.ChangeType(defaultval, typeof(T));
        }

        private static SourceType GetSourceType(string source)
        {
            if (string.IsNullOrWhiteSpace(source)) //No source
                return SourceType.File;

            string extension = Path.GetExtension(source).ToLower().TrimStart('.');

            if (File.Exists(source) && extension == "mpq") //MPQ
                return SourceType.MPQ;
            else if (Directory.Exists(source)) //CASC
                return SourceType.CASC;

            throw new Exception($"   Invalid source selected. Options are .MPQ, WoW Directory or blank.");
        }

        private static OutputType GetOutputType(string output)
        {
            string extension = Path.GetExtension(output).ToLower();
            switch (extension)
            {
                case ".csv":
                    return OutputType.CSV;
                case ".sql":
                    return OutputType.SQL;
                case ".json":
                    return OutputType.JSON;
            }

            throw new Exception("   Invalid output type. Options are CSV, JSON or SQL.");
        }


        internal enum SourceType
        {
            File,
            MPQ,
            CASC
        }

        internal enum OutputType
        {
            CSV,
            SQL,
            MPQ,
            JSON
        }
        #endregion
    }
}
