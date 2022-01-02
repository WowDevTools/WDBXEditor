using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WDBXEditor.Common
{
    public static class Extensions
    {

        public enum CompareResult
        {
            OK,
            Type,
            DBNull,
            Count
        }

        /// <summary>
        /// Simple comparison of two DataTable's columns
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static CompareResult ShallowCompare(this DataTable d1, DataTable d2, bool checktype = true)
        {
            //Column count - fast and quick
            if (d1.Columns.Count != d2.Columns.Count)
                return CompareResult.Count;

            //Column types - convert to string and compare
            string d1Types = string.Join(string.Empty, d1.Columns.Cast<DataColumn>().Select(x => x.DataType.Name));
            string d2Types = string.Join(string.Empty, d2.Columns.Cast<DataColumn>().Select(x => x.DataType.Name));
            if (!d1Types.Equals(d2Types, StringComparison.CurrentCultureIgnoreCase))
                return CompareResult.Type;

            //Enforce column names
            Parallel.For(0, d2.Columns.Count, (i, state) => d2.Columns[i].ColumnName = d1.Columns[i].ColumnName);

            return CompareResult.OK;
        }

        /// <summary>
        /// Gets rows that are missing from the source datatable based on key value
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<object[]> Except(this DataTable d1, DataTable d2, string key)
        {
            var keys = d1.AsEnumerable().Select(x => x.Field<int>(key)); //Source primary keys
            var dt1 = d2.AsEnumerable().ToDictionary(x => x.Field<int>(key), y => y.ItemArray); //d2 to dictionary of key, row data
            var dkey = dt1.Keys.Except(keys); //Get differential primary keys
            foreach (var d in dkey)
                yield return dt1[d];
        }

        /// <summary>
        /// Converts a DataColumnCollection to an ALTER TABLE Column string
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public static string ToSql(this DataColumnCollection cols, string primaryKey = "")
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn col in cols)
            {
                switch (col.DataType.Name)
                {
                    case "SByte":
                        sb.Append($" `{col.ColumnName}` TINYINT NOT NULL DEFAULT '0',");
                        break;
                    case "Byte":
                    case "Boolean":
                        sb.Append($" `{col.ColumnName}` TINYINT UNSIGNED NOT NULL DEFAULT '0',");
                        break;
                    case "Int16":
                        sb.Append($" `{col.ColumnName}` SMALLINT NOT NULL DEFAULT '0',");
                        break;
                    case "UInt16":
                        sb.Append($" `{col.ColumnName}` SMALLINT UNSIGNED NOT NULL DEFAULT '0',");
                        break;
                    case "Int32":
                        sb.Append($" `{col.ColumnName}` INT NOT NULL DEFAULT '0',");
                        break;
                    case "UInt32":
                        sb.Append($" `{col.ColumnName}` INT UNSIGNED NOT NULL DEFAULT '0',");
                        break;
                    case "Int64":
                        sb.Append($" `{col.ColumnName}` BIGINT NOT NULL DEFAULT '0',");
                        break;
                    case "UInt64":
                        sb.Append($" `{col.ColumnName}` BIGINT UNSIGNED NOT NULL DEFAULT '0',");
                        break;
                    case "Single":
                    case "Float":
                        sb.Append($" `{col.ColumnName}` FLOAT NOT NULL DEFAULT '0',");
                        break;
                    case "String":
                        sb.Append($" `{col.ColumnName}` TEXT NULL,");
                        break;
                    default:
                        throw new Exception($"Unknown data type {col.ColumnName} : {col.DataType.Name}");
                }
            }

            if (!string.IsNullOrWhiteSpace(primaryKey))
                sb.Append($" PRIMARY KEY (`{primaryKey}`)");

            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// Converts a datarow into a formatted SQL value string
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string ToSql(this DataRow row)
        {
            StringBuilder sb = new StringBuilder();
            DataColumnCollection cols = row.Table.Columns;
            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");

            for (int i = 0; i < cols.Count; i++)
            {
                if (cols[i].DataType == typeof(string)) //Escape formatting
                {
                    string val = row[i].ToString().Replace(@"\", @"\\").Replace(@"'", @"\'").Replace(@"""", @"\""");
                    sb.Append("\"" + val + "\",");
                }
                else if (cols[i].DataType == typeof(float))
                {
                    sb.Append(((float)row[i]).ToString(ci) + ",");
                }
                else
                {
                    sb.Append(row[i] + ",");
                }

            }

            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Reverse(this string s)
        {
            return new string(s.ToCharArray().Reverse().ToArray());
        }

        /// <summary>
        /// A more advanced string replace.
        /// <para>Accepts RegexOptions for specific pattern matching i.e. case insensitive</para>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="find"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string Replace(this string text, string find, string replacement, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(text, find, replacement, options);
        }

        public static void Detach(this DataTable table, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
                new BinaryFormatter().Serialize(stream, table);
        }

        public static object DefaultValue<T>(this T type) where T : Type
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

    }
}
