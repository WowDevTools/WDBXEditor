using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADGV
{

    [System.ComponentModel.DesignerCategory("")]
    public partial class AdvancedDataGridView : DataGridView
    {
        public event EventHandler SortStringChanged;
        public event EventHandler FilterStringChanged;

        public ContextMenu HeaderContext { get; set; }
        private DataCache Cache = null;
        public bool FilterAndSortEnabled { get; set; }
        public string FilterString
        {
            get
            {
                return filterString;
            }
            private set
            {
                string old = value;
                if (old != filterString)
                {
                    filterString = value;
                    FilterStringChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public string SortString
        {
            get
            {
                return sortString;
            }
            private set
            {
                string old = value;
                if (old != sortString)
                {
                    sortString = value;
                    SortStringChanged(this, new EventArgs());
                }
            }
        }


        private List<string> sortorderList = new List<string>();
        private List<string> filterorderList = new List<string>();
        private List<string> filteredColumns = new List<string>();

        private string sortString = string.Empty;
        private string filterString = string.Empty;
        private object[] _copydata;
        private DataColumn primarykey => Cache.PrimaryKey;

        public AdvancedDataGridView()
        {
            DoubleBuffered = true;
            Cache = new DataCache(this);
        }


        #region Cell Methods
        private IEnumerable<ColumnHeader> FilterableCells
        {
            get
            {
                return from DataGridViewColumn c in Columns
                       where c.HeaderCell != null && c.HeaderCell is ColumnHeader
                       select (c.HeaderCell as ColumnHeader);
            }
        }

        #endregion


        #region Column Events

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColumnHeader cell = new ColumnHeader(e.Column.HeaderCell, FilterAndSortEnabled);
            SetEvents(cell);
            e.Column.MinimumWidth = cell.MinimumSize.Width;
            if (ColumnHeadersHeight < cell.MinimumSize.Height)
                ColumnHeadersHeight = cell.MinimumSize.Height;
            e.Column.HeaderCell = cell;

            base.OnColumnAdded(e);
        }

        public void SetEvents(DataGridViewColumnHeaderCell header)
        {
            var cell = header as ColumnHeader;
            cell.SortChanged += new ColumnHeaderCellEventHandler(cell_SortChanged);
            cell.FilterChanged += new ColumnHeaderCellEventHandler(cell_FilterChanged);
            cell.FilterPopup += new ColumnHeaderCellEventHandler(cell_FilterPopup);
            cell.HideChanged += new ColumnHeaderCellEventHandler(cell_HideChanged);
            cell.HexChanged += new ColumnHeaderCellEventHandler(cell_HexChanged);
        }

        protected override void OnColumnRemoved(DataGridViewColumnEventArgs e)
        {
            filteredColumns.Remove(e.Column.Name);
            filterorderList.Remove(e.Column.Name);
            sortorderList.Remove(e.Column.Name);

            ColumnHeader cell = e.Column.HeaderCell as ColumnHeader;
            if (cell != null)
            {
                cell.SortChanged -= cell_SortChanged;
                cell.FilterChanged -= cell_FilterChanged;
                cell.FilterPopup -= cell_FilterPopup;
                cell.HideChanged -= cell_HideChanged;
                cell.HexChanged -= cell_HexChanged;
            }
            base.OnColumnRemoved(e);
        }
        
        #endregion


        #region Row Events

        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            filteredColumns.Clear();
            base.OnRowsAdded(e);
        }

        protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
        {
            filteredColumns.Clear();
            base.OnRowsRemoved(e);
        }

        public void SelectRow(int index)
        {
            ClearSelection();
            Rows[index].Selected = true;
            CurrentCell = Rows[index].Cells[0];
        }

        #endregion


        #region Cell Events
        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            if (Rows[e.RowIndex].Cells[e.ColumnIndex].Value == DBNull.Value && Columns[e.ColumnIndex].ValueType == typeof(string))
                Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Empty;

            filteredColumns.Remove(Columns[e.ColumnIndex].Name);
            base.OnCellValueChanged(e);
        }
        #endregion


        #region Filter Methods

        private string BuildFilterString()
        {
            StringBuilder sb = new StringBuilder("");
            string appx = "";

            foreach (string filterOrder in filterorderList)
            {
                DataGridViewColumn Column = Columns[filterOrder];

                if (Column != null)
                {
                    ColumnHeader cell = Column.HeaderCell as ColumnHeader;
                    if (cell != null)
                    {
                        if (cell.FilterAndSortEnabled && cell.ActiveFilterType != ColumnMenu.FilterType.None)
                        {
                            sb.AppendFormat(appx + "(" + cell.FilterString.Trim() + ")", Column.DataPropertyName);
                            appx = " AND ";
                        }
                    }
                }
            }
            return sb.ToString();
        }

        private void cell_FilterPopup(object sender, ColumnHeaderCellEventArgs e)
        {
            if (Columns.Contains(e.Column))
            {
                ColumnMenu filterMenu = e.FilterMenu;
                DataGridViewColumn column = e.Column;

                System.Drawing.Rectangle rect = GetCellDisplayRectangle(column.Index, -1, true);

                if (filteredColumns.Contains(column.Name))
                    filterMenu.Show(this, rect.Left, rect.Bottom, false);
                else
                {
                    filteredColumns.Add(column.Name);
                    if (filterorderList.Count() > 0 && filterorderList.Last() == column.Name)
                        filterMenu.Show(this, rect.Left, rect.Bottom, true);
                    else
                        filterMenu.Show(this, rect.Left, rect.Bottom, ColumnMenu.GetValuesForFilter(this, column.Name));
                }
            }
        }

        private void cell_FilterChanged(object sender, ColumnHeaderCellEventArgs e)
        {
            if (Columns.Contains(e.Column))
            {
                ColumnMenu filterMenu = e.FilterMenu;
                DataGridViewColumn column = e.Column;

                filterorderList.Remove(column.Name);
                if (filterMenu.ActiveFilterType != ColumnMenu.FilterType.None)
                    filterorderList.Add(column.Name);

                FilterString = BuildFilterString();
            }
        }

        #endregion


        #region Sort Methods

        private string BuildSortString()
        {
            StringBuilder sb = new StringBuilder("");
            string appx = "";

            foreach (string sortOrder in sortorderList)
            {
                DataGridViewColumn column = Columns[sortOrder];

                if (column != null)
                {
                    ColumnHeader cell = column.HeaderCell as ColumnHeader;
                    if (cell != null)
                    {
                        if (cell.FilterAndSortEnabled && cell.ActiveSortType != ColumnMenu.SortType.None)
                        {
                            sb.AppendFormat(appx + cell.SortString, column.DataPropertyName);
                            appx = ", ";
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private void cell_SortChanged(object sender, ColumnHeaderCellEventArgs e)
        {
            if (Columns.Contains(e.Column))
            {
                ColumnMenu filterMenu = e.FilterMenu;
                DataGridViewColumn column = e.Column;

                sortorderList.Remove(column.Name);
                if (filterMenu.ActiveSortType != ColumnMenu.SortType.None)
                    sortorderList.Add(column.Name);
                SortString = BuildSortString();
            }
        }

        private void cell_HideChanged(object sender, ColumnHeaderCellEventArgs e)
        {
            if (Columns.Contains(e.Column))
            {
                if (e.Column.Name == primarykey.ColumnName)
                    return;

                e.Column.Visible = false;
            }
        }

        private void cell_HexChanged(object sender, ColumnHeaderCellEventArgs e)
        {
            if (e.Column.DefaultCellStyle.Tag?.ToString().IndexOf('X') == 0)
                e.Column.DefaultCellStyle.Tag = "";
            else
                e.Column.DefaultCellStyle.Tag = $"X";

            this.Refresh();
        }

        #endregion


        #region Hex
        protected override void OnCellParsing(DataGridViewCellParsingEventArgs e)
        {
            if (e != null && e.Value != null)
            {
                if ((this.Columns[e.ColumnIndex].DefaultCellStyle.Tag?.ToString().IndexOf('X') ?? -1) == 0)
                {
                    string value = e.Value.ToString();
                    if (value.StartsWith("0x"))
                        value = value.Substring(2);

                    long l = 0; ulong u = 0;
                    if (long.TryParse(value, NumberStyles.HexNumber, null, out l))
                    {
                        e.Value = Convert.ChangeType(l, e.DesiredType);
                        e.ParsingApplied = true;
                    }
                    else if (ulong.TryParse(value, NumberStyles.HexNumber, null, out u))
                    {
                        e.Value = Convert.ChangeType(u, e.DesiredType);
                        e.ParsingApplied = true;
                    }
                }
            }
            else
                base.OnCellParsing(e);
        }

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            string tag = this.Columns[e.ColumnIndex].DefaultCellStyle.Tag?.ToString() ?? "";

            if (e != null && tag.IndexOf('X') == 0)
            {
                if (e.Value != null)
                {
                    long value = 0;
                    if (long.TryParse(e.Value.ToString(), out value))
                    {
                        e.Value = "0x" + value.ToString(tag);
                        e.FormattingApplied = true;
                    }
                }
            }
            else
                base.OnCellFormatting(e);
        }
        #endregion


        #region Copy Data
        public void SetCopyData()
        {
            if (SelectedRows.Count > 0)
                _copydata = ((DataRowView)CurrentRow.DataBoundItem).Row.ItemArray;
        }

        public void PasteCopyData(DataRow row)
        {
            if (_copydata.Length == 0) return;

            var pk = ((DataTable)((BindingSource)DataSource).DataSource).PrimaryKey[0];
            _copydata[pk.Ordinal] = row.ItemArray[pk.Ordinal];
            row.ItemArray = _copydata;
        }

        public void ClearCopyData()
        {
            _copydata = new object[0];
        }
        #endregion

        public void SetVisible(int index, bool value)
        {
            if (index == primarykey.Ordinal) return;
            Columns[index].Visible = value;
        }

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            Task.Run(() => Cache.Init());
            base.OnDataBindingComplete(e);
        }

        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            displayErrorDialogIfNoHandler = false;
            base.OnDataError(displayErrorDialogIfNoHandler, e);
        }

        public Point Search(string text, bool exact, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase, bool includestart = false)
        {
            return Cache.Search(text, exact, comparison, includestart);
        }
    }
}