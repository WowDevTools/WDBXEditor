using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace ADGV
{
    class DataCache
    {
        public DataColumn PrimaryKey = null;

        private ConcurrentDictionary<int, string> Cache = new ConcurrentDictionary<int, string>();
        private ConcurrentDictionary<int, int> DataCount = new ConcurrentDictionary<int, int>();
        private AdvancedDataGridView gridview = null;
        private string _tag = string.Empty;

        public DataCache(AdvancedDataGridView datagrid)
        {
            gridview = datagrid;
        }

        public void Init(bool force = false)
        {
            if (!force && _tag == (gridview.Parent.Tag?.ToString() ?? string.Empty))
                return;

            _tag = gridview.Parent.Tag?.ToString() ?? string.Empty;
            Cache.Clear();
            DataCount.Clear();

            var table = (DataTable)((BindingSource)gridview.DataSource).DataSource;
            if ((table?.Rows.Count ?? 0) == 0)
                return;

            PrimaryKey = table.PrimaryKey[0]; //Store the primary key

#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif

            Parallel.For(0, table.Columns.Count, x => DataCount.TryAdd(x, 0)); //Prefill the DataCount set

            Parallel.ForEach(table.Rows.Cast<DataRow>(), row =>
            {
                if ((row?.ItemArray.Length ?? 0) == 0)
                    return;

                int key = (int)row[PrimaryKey.ColumnName];
                var data = row.ItemArray;

                Cache.TryAdd(key, new JavaScriptSerializer().Serialize(data)); //Store the JSON variant
                GetColumnDataCount(data); //Update the empty column count
            });

#if DEBUG
            sw.Stop();
            Debug.WriteLine(sw.ElapsedMilliseconds);
#endif
        }

        #region Cache Changes

        public void ChangeValue(DataRow row)
        {
            int key = (int)row[PrimaryKey.ColumnName];
            var data = row.ItemArray.Select(x => x?.ToString() ?? "").ToArray();

            if (Cache.ContainsKey(key))
            {
                UpdateColumnDataCount(key, data);
                Cache[key] = new JavaScriptSerializer().Serialize(data);
            }
            else
                AddRow(row);
        }

        public void AddRow(DataRow row)
        {
            int key = (int)(row[PrimaryKey.ColumnName] ?? -1);
            if (key == -1) return;

            var data = row.ItemArray;
            Cache.TryAdd(key, new JavaScriptSerializer().Serialize(data));
            GetColumnDataCount(data);
        }

        public void RemoveRow(DataRow row)
        {
            int key = (int)row[PrimaryKey.ColumnName];

            if (Cache.ContainsKey(key))
            {
                string _dump;
                if(Cache.TryRemove(key, out _dump))
                {
                    string[] vals = new JavaScriptSerializer().Deserialize<string[]>(_dump);
                    for (int i = 0; i < vals.Length; i++)
                        if (vals[i].Length > 0)
                            DataCount[i]--;
                }
            }
        }

        #endregion

        #region Column Value Count

        public List<int> GetEmptyColumns()
        {
            List<int> result = new List<int>();
            foreach (var c in DataCount)
            {
                if (c.Value <= 0)
                {
                    DataCount[c.Key] = 0;
                    result.Add(c.Key);
                }
            }

            return result;
        }

        private void GetColumnDataCount(object[] data)
        {
            for (int x = 0; x < data.Length; x++)
                if (data[x] != null && data[x].ToString().Length > 0)
                    DataCount[x]++;
        }

        private void UpdateColumnDataCount(int key, string[] data)
        {
            string[] prev = new JavaScriptSerializer().Deserialize<string[]>(Cache[key]);
            for (int i = 0; i < data.Length; i++)
            {
                if (prev[i].Length > 0 && data[i].Length == 0)
                    DataCount[i]--;
                else if (prev[i].Length == 0 && data[i].Length > 0)
                    DataCount[i]++;
            }
        }

        #endregion

        public Point Search(string text, bool exact, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase, bool includestart = false)
        {
            var startcell = gridview.CurrentCell; //Our original
            var startindex = startcell.RowIndex;
            var startcolumn = startcell.ColumnIndex;
            bool looped = false;
            var hidden = gridview.Columns.Cast<DataGridViewColumn>().Where(x => !x.Visible).Select(x => x.Index).ToArray();

            BindingSource bs = (BindingSource)gridview.DataSource;

            //Get actual match
            FinalSearch:
            for (int i = startindex; i < gridview.Rows.Count - 1; i++)
            {
                var pk = (int)((DataRowView)bs[i]).Row.ItemArray[PrimaryKey.Ordinal]; //Get the Id value
                if (startcolumn > 0 && i != startindex)
                    startcolumn = 0;

                if (!Cache.ContainsKey(pk)) //Ignore non-cached - shouldn't happen
                    continue;
                if (Cache[pk].IndexOf(text, comparison) == -1) //Check the JSON haystack contains the needle
                    continue;

                var data = new JavaScriptSerializer().Deserialize<string[]>(Cache[pk]);
                for (int x = startcolumn; x < data.Length; x++)
                {
                    //Don't search hidden columns
                    if (hidden.Contains(x))
                        continue;

                    //Completed a full loop
                    if (i == startcell.RowIndex && x == startcell.ColumnIndex && looped)
                        return new Point(-1, -1);

                    //Ignore start cell check
                    if (i == startcell.RowIndex && x == startcell.ColumnIndex && !includestart && !looped)
                        continue;

                    if (exact && data[x].Equals(text, comparison))
                        return new Point(i, x);
                    else if (!exact && data[x].IndexOf(text, comparison) >= 0)
                        return new Point(i, x);
                }
            }

            //Restart from the beginning
            if (!looped && startcell.RowIndex > 0)
            {
                startindex = 0;
                looped = true;
                goto FinalSearch;
            }

            return new Point(-1, -1); //No matches
        }
    }
}
