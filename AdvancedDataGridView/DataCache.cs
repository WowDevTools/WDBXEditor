using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADGV
{
	partial class AdvancedDataGridView : DataGridView
	{
		public DataColumn PrimaryKey { get; private set; }

		private ConcurrentDictionary<int, string> Cache = new ConcurrentDictionary<int, string>();
		private int[] DataCount;
		private string _tag = string.Empty;

		public void Init(bool force = false)
		{
			if (!force && _tag == this.Parent.Tag + "")
				return;

			_tag = this.Parent.Tag + "";
			Cache.Clear();
			DataCount = new int[0];

			var table = (DataTable)((BindingSource)this.DataSource).DataSource;
			if ((table?.Rows.Count ?? 0) == 0)
				return;

			PrimaryKey = table.PrimaryKey[0]; //Store the primary key

#if DEBUG
			Stopwatch sw = new Stopwatch();
			sw.Start();
#endif
			DataCount = new int[table.Columns.Count];

			Parallel.ForEach(table.Rows.Cast<DataRow>(), row =>
			{
				if ((row?.ItemArray.Length ?? 0) == 0)
					return;

				if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached)
					return;

				if (row[PrimaryKey.ColumnName] == DBNull.Value)
					return;

				int key = (int)row[PrimaryKey.ColumnName];
				var data = row.ItemArray;
				Cache.TryAdd(key, SerializeObject(data)); //Store the JSON variant
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
			var data = row.ItemArray.Select(x => x + "").ToArray();

			if (Cache.ContainsKey(key))
			{
				UpdateColumnDataCount(key, data);
				Cache[key] = SerializeObject(data);
			}
			else
				AddRow(row);
		}

		public void AddRow(DataRow row)
		{
			int key = (int)(row[PrimaryKey.ColumnName] ?? -1);
			if (key == -1) return;

			var data = row.ItemArray;
			Cache.TryAdd(key, SerializeObject(data));
			GetColumnDataCount(data);
		}

		public void RemoveRow(DataRow row)
		{
			int key = (int)row[PrimaryKey.ColumnName];

			if (Cache.ContainsKey(key))
			{
				string _dump;
				if (Cache.TryRemove(key, out _dump))
				{
					string[] vals = DeserializeObject(_dump);
					for (int i = 0; i < vals.Length; i++)
						if (vals[i].Length > 0)
							DataCount[i]--;
				}
			}
		}

		#endregion

		#region Column Value Count

		public IEnumerable<int> GetEmptyColumns()
		{
			for (int i = 0; i < DataCount.Length; i++)
			{
				if (DataCount[i] <= 0)
				{
					DataCount[i] = 0;
					yield return i;
				}
			}
		}

		private void GetColumnDataCount(object[] data)
		{
			for (int x = 0; x < data.Length; x++)
				if ((data[x] + "").Length > 0)
					DataCount[x]++;
		}

		private void UpdateColumnDataCount(int key, string[] data)
		{
			string[] prev = DeserializeObject(Cache[key]);
			for (int i = 0; i < data.Length; i++)
			{
				if (prev[i].Length > 0 && data[i].Length == 0)
					DataCount[i]--;
				else if (prev[i].Length == 0 && data[i].Length > 0)
					DataCount[i]++;
			}
		}

		#endregion

		#region Search
		public Point Search(string text, bool exact, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase, bool includestart = false)
		{
			if (this.CurrentCell == null)
				this.SetSelectedCellCore(0, 0, true);

			var startcell = this.CurrentCell; //Our original
			var startindex = startcell.RowIndex;
			var startcolumn = startcell.ColumnIndex;
			bool looped = false;
			var hidden = new HashSet<int>(this.Columns.Cast<DataGridViewColumn>().Where(x => !x.Visible).Select(x => x.Index));

			BindingSource bs = (BindingSource)this.DataSource;

			//Get actual match
			FinalSearch:
			for (int i = startindex; i < this.Rows.Count - 1; i++)
			{
				var pk = (int)((DataRowView)bs[i]).Row.ItemArray[PrimaryKey.Ordinal]; //Get the Id value
				if (startcolumn > 0 && i != startindex)
					startcolumn = 0;

				if (!Cache.ContainsKey(pk)) //Ignore non-cached - shouldn't happen
					continue;
				if (Cache[pk].IndexOf(text, comparison) == -1) //Check the JSON haystack contains the needle
					continue;

				var data = DeserializeObject(Cache[pk]);
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

		public Point SearchFlag(long flag, bool includestart = false, ICollection<Point> ignore = null)
		{
			if (this.CurrentCell == null)
				this.SetSelectedCellCore(0, 0, true);

			var startcell = this.CurrentCell; //Our original
			var startindex = startcell.RowIndex;
			var startcolumn = startcell.ColumnIndex;
			bool looped = false;
			var hidden = new HashSet<int>(this.Columns.Cast<DataGridViewColumn>().Where(x => !x.Visible).Select(x => x.Index));

			BindingSource bs = (BindingSource)this.DataSource;

			//Get actual match
			FinalSearch:
			for (int i = startindex; i < this.Rows.Count - 1; i++)
			{
				var pk = (int)((DataRowView)bs[i]).Row.ItemArray[PrimaryKey.Ordinal]; //Get the Id value

				if (!Cache.ContainsKey(pk)) //Ignore non-cached - shouldn't happen
					continue;

				var data = DeserializeObject(Cache[pk]);
				//Don't search hidden columns
				if (hidden.Contains(startcolumn))
					return new Point(-1, -1); //No matches

				//Completed a full loop
				if (i >= startcell.RowIndex && looped)
					return new Point(-1, -1);

				//Ignore start cell check
				if (i == startcell.RowIndex && !includestart && !looped)
					continue;


				if (long.TryParse(data[startcolumn], out long value) && (value & flag) == flag)
				{
					Point result = new Point(i, startcolumn);
					if (ignore == null || !ignore.Contains(result))
						return result;
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
		#endregion


		#region Serialization
		private string SerializeObject(Array array)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				foreach (var obj in array)
				{
					var b = Encoding.UTF8.GetBytes(obj + "");
					var s = BitConverter.GetBytes((ushort)b.Length);

					ms.Write(s, 0, s.Length);
					ms.Write(b, 0, b.Length);
				}

				return Encoding.UTF8.GetString(ms.ToArray());
			}
		}

		private string[] DeserializeObject(string value)
		{
			var bytes = Encoding.UTF8.GetBytes(value);
			string[] _output = new string[DataCount.Length];

			using (MemoryStream ms = new MemoryStream(bytes))
			{
				byte[] s = new byte[sizeof(ushort)];
				byte[] b;

				for (int i = 0; i < _output.Length; i++)
				{
					ms.Read(s, 0, s.Length);
					int size = BitConverter.ToUInt16(s, 0);

					b = new byte[size];
					ms.Read(b, 0, b.Length);
					_output[i] = Encoding.UTF8.GetString(b);
				}

				return _output;
			}
		}
		#endregion
	}
}
