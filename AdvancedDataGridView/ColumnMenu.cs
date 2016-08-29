using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADGV
{

    [System.ComponentModel.DesignerCategory("")]
    internal partial class ColumnMenu : ContextMenuStrip
    {
        public enum FilterType : byte
        {
            None = 0,
            Custom = 1,
            CheckList = 2,
            Loaded = 3
        }
        public enum SortType : byte
        {
            None = 0,
            ASC = 1,
            DESC = 2
        }

        public event EventHandler SortChanged;
        public event EventHandler FilterChanged;
        public event EventHandler HexChanged;
        public event EventHandler HideChanged;

        public SortType ActiveSortType
        {
            get
            {
                return _activeSortType;
            }
        }
        public FilterType ActiveFilterType
        {
            get
            {
                return _activeFilterType;
            }
        }
        public Type DataType { get; private set; }
        public bool IsSortEnabled { get; set; }
        public bool IsFilterEnabled { get; set; }

        private Hashtable _textStrings = new Hashtable();
        private List<DataGridViewRow> _filterRows = new List<DataGridViewRow>();
        private FilterType _activeFilterType = FilterType.None;
        private SortType _activeSortType = SortType.None;
        private string _sortString = null;
        private string _filterString = null;
        private static Point _resizeStartPoint = new Point(1, 1);
        private Point _resizeEndPoint = new Point(-1, -1);
        private bool _activated = false;

        #region Constructor/Events
        public ColumnMenu(Type dataType) : base()
        {
            _textStrings.Add("SORTDATETIMEASC", "Sort Oldest to Newest");
            _textStrings.Add("SORTDATETIMEDESC", "Sort Newest to Oldest");
            _textStrings.Add("SORTBOOLASC", "Sort by False/True");
            _textStrings.Add("SORTBOOLDESC", "Sort by True/False");
            _textStrings.Add("SORTNUMASC", "Sort Smallest to Largest");
            _textStrings.Add("SORTNUMDESC", "Sort Largest to Smallest");
            _textStrings.Add("SORTTEXTASC", "Sort А to Z");
            _textStrings.Add("SORTTEXTDESC", "Sort Z to A");
            _textStrings.Add("ADDCUSTOMFILTER", "Add a Filter");
            _textStrings.Add("CUSTOMFILTER", "Filter");
            _textStrings.Add("CLEARFILTER", "Clear Filter");
            _textStrings.Add("CLEARSORT", "Clear Sort");
            _textStrings.Add("BUTTONOK", "Filter");
            _textStrings.Add("BUTTONCANCEL", "Cancel");
            _textStrings.Add("NODESELECTALL", "(Select All)");
            _textStrings.Add("NODESELECTEMPTY", "(Blanks)");
            _textStrings.Add("HIDECOLUMN", "Hide");
            
            InitializeComponent();
            
            DataType = dataType;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            Activate();
            base.OnVisibleChanged(e);
        }

        private void Activate()
        {
            if (_activated) return;

            //Load this only when required, massive performance increase

            sortASCMenuItem.Image = Properties.Resources.MenuStrip_OrderASCtxt;
            sortDESCMenuItem.Image = Properties.Resources.MenuStrip_OrderDESCtxt;
            customFilterLastFiltersListMenuItem.Image = Properties.Resources.ColumnHeader_Filtered;
            customFilterLastFiltersListMenuItem.Text = _textStrings["CUSTOMFILTER"].ToString();

            //set components values
            Type[] numberTypes = new[] { typeof(int), typeof(long), typeof(short), typeof(uint), typeof(ulong), typeof(ushort),
                                         typeof(byte), typeof(sbyte), typeof(decimal), typeof(float), typeof(double) };

            Type[] nonhexTypes = new[] { typeof(decimal), typeof(float), typeof(double) };

            if (numberTypes.Contains(DataType))
            {
                sortASCMenuItem.Text = _textStrings["SORTNUMASC"].ToString();
                sortDESCMenuItem.Text = _textStrings["SORTNUMDESC"].ToString();
                hexDisplayMenuItem.Visible = !nonhexTypes.Contains(DataType);
                toolStripSeparator2MenuItem.Visible = hexDisplayMenuItem.Visible;
            }
            else
            {
                sortASCMenuItem.Text = _textStrings["SORTTEXTASC"].ToString();
                sortDESCMenuItem.Text = _textStrings["SORTTEXTDESC"].ToString();
                hexDisplayMenuItem.Visible = false;
                toolStripSeparator2MenuItem.Visible = false;
            }

            customFilterLastFiltersListMenuItem.Enabled = DataType != typeof(bool);
            customFilterLastFiltersListMenuItem.Checked = ActiveFilterType == FilterType.Custom;
            customFilterLastFiltersListMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            sortDESCMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            sortASCMenuItem.ImageScaling = ToolStripItemImageScaling.None;

            MinimumSize = new Size(PreferredSize.Width, PreferredSize.Height);
            ResizeBox(MinimumSize.Width, MinimumSize.Height);

            _activated = true;
        }

        private void MenuStrip_Closed(Object sender, EventArgs e)
        {
            ResizeClean();
        }

        private void MenuStrip_LostFocus(Object sender, EventArgs e)
        {
            if (!ContainsFocus)
                Close();
        }

        private ImageList GetCheckListStateImages()
        {
            ImageList images = new System.Windows.Forms.ImageList();
            Bitmap unCheckImg = new Bitmap(16, 16);
            Bitmap checkImg = new Bitmap(16, 16);
            Bitmap mixedImg = new Bitmap(16, 16);

            using (Bitmap img = new Bitmap(16, 16))
            {
                using (Graphics g = Graphics.FromImage(img))
                {
                    CheckBoxRenderer.DrawCheckBox(g, new Point(0, 1), System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
                    unCheckImg = (Bitmap)img.Clone();
                    CheckBoxRenderer.DrawCheckBox(g, new Point(0, 1), System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal);
                    checkImg = (Bitmap)img.Clone();
                    CheckBoxRenderer.DrawCheckBox(g, new Point(0, 1), System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal);
                    mixedImg = (Bitmap)img.Clone();
                }
            }

            images.Images.Add("uncheck", unCheckImg);
            images.Images.Add("check", checkImg);
            images.Images.Add("mixed", mixedImg);
            return images;
        }

        #endregion

        #region Enablers

        public void SetSortEnabled(bool enabled)
        {
            if (!IsSortEnabled)
                enabled = false;

            this.cancelSortMenuItem.Enabled = enabled;

            this.sortASCMenuItem.Enabled = enabled;
            this.sortDESCMenuItem.Enabled = enabled;
        }

        public void SetFilterEnabled(bool enabled)
        {
            if (!IsFilterEnabled)
                enabled = false;

            this.cancelFilterMenuItem.Enabled = enabled;
            if (enabled)
                customFilterLastFiltersListMenuItem.Enabled = DataType != typeof(bool);
            else
                customFilterLastFiltersListMenuItem.Enabled = false;
        }

        #endregion
        
        public void SetLoadedMode(bool enabled)
        {
            cancelFilterMenuItem.Enabled = enabled;
            if (enabled)
            {
                _activeFilterType = FilterType.Loaded;
                _sortString = null;
                _filterString = null;
                customFilterLastFiltersListMenuItem.Checked = false;
                SetSortEnabled(false);
                SetFilterEnabled(false);
            }
            else
            {
                _activeFilterType = FilterType.None;

                SetSortEnabled(true);
                SetFilterEnabled(true);
            }
        }

        public void Show(Control control, int x, int y, IEnumerable<DataGridViewCell> vals)
        {

            base.Show(control, x, y);
        }

        public void Show(Control control, int x, int y, bool _restoreFilter)
        {
            base.Show(control, x, y);
        }

        public static IEnumerable<DataGridViewCell> GetValuesForFilter(DataGridView grid, string columnName)
        {
            return from DataGridViewRow nulls in grid.Rows select nulls.Cells[columnName];
        }

        #region Sort Methods
        public void SortASC()
        {
            sortASCMenuItem_Click(this, null);
        }

        public void SortDESC()
        {
            sortDESCMenuItem_Click(this, null);
        }

        public string SortString
        {
            get
            {
                return _sortString == null ? "" : _sortString;
            }
            private set
            {
                cancelSortMenuItem.Enabled = (value != null && value.Length > 0);
                _sortString = value;
            }
        }

        public void CleanSort()
        {
            string oldsort = SortString;
            sortASCMenuItem.Checked = false;
            sortDESCMenuItem.Checked = false;
            _activeSortType = SortType.None;
            SortString = null;
        }

        #endregion

        #region Filter Methods
        public string FilterString
        {
            get
            {
                return _filterString == null ? "" : _filterString;
            }

            private set
            {
                cancelFilterMenuItem.Enabled = (value != null && value.Length > 0);
                _filterString = value;
            }
        }

        public void CleanFilter()
        {
            _activeFilterType = FilterType.None;
            string oldsort = FilterString;
            FilterString = null;
            customFilterLastFiltersListMenuItem.Checked = false;
            _filterRows.Clear();
        }
        #endregion

        #region Filter Events
        private void cancelFilterMenuItem_Click(object sender, EventArgs e)
        {
            string oldfilter = FilterString;

            //clean Filter
            CleanFilter();

            //fire Filter changed
            if (oldfilter != FilterString && FilterChanged != null)
                FilterChanged(this, new EventArgs());
        }

        private void cancelFilterMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Enabled)
                (sender as ToolStripMenuItem).Select();
        }

        private void customFilterMenuItem_Click(object sender, EventArgs e)
        {
            //open a new Custom filter window
            FilterForm flt = new FilterForm(DataType);
            flt.FilterRows = _filterRows;

            if (flt.ShowDialog() == DialogResult.OK)
            {
                //add the new Filter presets
                _filterRows = flt.FilterRows;
                string filterString = flt.FilterString ?? "";

                _activeFilterType = (string.IsNullOrWhiteSpace(filterString) ? FilterType.None : FilterType.Custom);

                //get Filter string
                string oldfilter = FilterString;
                FilterString = filterString;

                //fire Filter changed
                if (oldfilter != FilterString && FilterChanged != null)
                    FilterChanged(this, new EventArgs());
            }
        }

        private void customFilterLastFilter1MenuItem_VisibleChanged(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).VisibleChanged -= customFilterLastFilter1MenuItem_VisibleChanged;
        }

        private void customFilterLastFilterMenuItem_TextChanged(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).Available = true;
            (sender as ToolStripMenuItem).TextChanged -= customFilterLastFilterMenuItem_TextChanged;
        }

        #endregion

        #region Click Events
        private void hexDisplayMenuItem_Click(object sender, EventArgs e)
        {
            hexDisplayMenuItem.Checked = !hexDisplayMenuItem.Checked;
            HexChanged(this, new EventArgs());
        }

        private void hideMenuItem_Click(object sender, EventArgs e)
        {
            HideChanged(this, new EventArgs());
        }

        private void sortASCMenuItem_Click(object sender, EventArgs e)
        {
            sortASCMenuItem.Checked = true;
            sortDESCMenuItem.Checked = false;
            _activeSortType = SortType.ASC;

            //get Sort String
            string oldsort = SortString;
            SortString = "[{0}] ASC";

            //fire Sort Changed
            if (oldsort != SortString && SortChanged != null)
                SortChanged(this, new EventArgs());
        }

        private void sortASCMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Enabled)
                (sender as ToolStripMenuItem).Select();
        }

        private void sortDESCMenuItem_Click(object sender, EventArgs e)
        {
            sortASCMenuItem.Checked = false;
            sortDESCMenuItem.Checked = true;
            _activeSortType = SortType.DESC;

            //get Sort String
            string oldsort = SortString;
            SortString = "[{0}] DESC";

            //fire Sort Changed
            if (oldsort != SortString && SortChanged != null)
                SortChanged(this, new EventArgs());
        }

        private void sortDESCMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Enabled)
                (sender as ToolStripMenuItem).Select();
        }

        private void cancelSortMenuItem_Click(object sender, EventArgs e)
        {
            string oldsort = SortString;
            //clean Sort
            CleanSort();
            //fire Sort changed
            if (oldsort != SortString && SortChanged != null)
                SortChanged(this, new EventArgs());
        }

        private void cancelSortMenuItem_MouseEnter(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Enabled)
                (sender as ToolStripMenuItem).Select();
        }

        #endregion

        #region Resize
        private void ResizeBox(int w, int h)
        {
            sortASCMenuItem.Width = w - 1;
            sortDESCMenuItem.Width = w - 1;
            cancelSortMenuItem.Width = w - 1;
            cancelFilterMenuItem.Width = w - 1;
            customFilterLastFiltersListMenuItem.Width = w - 1;

            Size = new Size(w, h);
        }

        /// <summary>
        /// Clean box for Resize
        /// </summary>
        private void ResizeClean()
        {
            if (_resizeEndPoint.X != -1)
            {
                Point startPoint = PointToScreen(ColumnMenu._resizeStartPoint);

                Rectangle rc = new Rectangle(startPoint.X, startPoint.Y, _resizeEndPoint.X, _resizeEndPoint.Y);

                rc.X = Math.Min(startPoint.X, _resizeEndPoint.X);
                rc.Width = Math.Abs(startPoint.X - _resizeEndPoint.X);

                rc.Y = Math.Min(startPoint.Y, _resizeEndPoint.Y);
                rc.Height = Math.Abs(startPoint.Y - _resizeEndPoint.Y);

                ControlPaint.DrawReversibleFrame(rc, Color.Black, FrameStyle.Dashed);

                _resizeEndPoint.X = -1;
            }
        }

        #endregion
    }
}