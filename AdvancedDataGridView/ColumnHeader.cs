using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADGV
{
    [System.ComponentModel.DesignerCategory("")]
    internal class ColumnHeader : DataGridViewColumnHeaderCell
    {
        public event ColumnHeaderCellEventHandler FilterPopup;
        public event ColumnHeaderCellEventHandler SortChanged;
        public event ColumnHeaderCellEventHandler FilterChanged;
        public event ColumnHeaderCellEventHandler HideChanged;
        public event ColumnHeaderCellEventHandler HexChanged;

        public ColumnMenu MenuStrip { get; private set; }
        public bool FilterAndSortEnabled
        {
            get
            {
                return _filterEnabled;
            }
            set
            {
                if (!value)
                {
                    _filterButtonPressed = false;
                    _filterButtonOver = false;
                }

                if (value != _filterEnabled)
                {
                    _filterEnabled = value;
                    bool refreshed = false;
                    if (MenuStrip.FilterString.Length > 0)
                    {
                        menuStrip_FilterChanged(this, new EventArgs());
                        refreshed = true;
                    }
                    if (MenuStrip.SortString.Length > 0)
                    {
                        menuStrip_SortChanged(this, new EventArgs());
                        refreshed = true;
                    }
                    if (!refreshed)
                        RepaintCell();
                }
            }
        }
        public ColumnMenu.SortType ActiveSortType
        {
            get
            {
                return (MenuStrip != null && FilterAndSortEnabled ? MenuStrip.ActiveSortType : ColumnMenu.SortType.None);
            }
        }
        public ColumnMenu.FilterType ActiveFilterType
        {
            get
            {
                return (MenuStrip != null && FilterAndSortEnabled ? MenuStrip.ActiveFilterType : ColumnMenu.FilterType.None);
            }
        }
        public string SortString
        {
            get
            {
                return (MenuStrip != null && FilterAndSortEnabled ? MenuStrip.SortString : "");
            }
        }
        public string FilterString
        {
            get
            {
                return (MenuStrip != null && FilterAndSortEnabled ? MenuStrip.FilterString : "");
            }
        }
        public Size MinimumSize
        {
            get
            {
                return new Size(_filterButtonImageSize.Width + _filterButtonMargin.Left + _filterButtonMargin.Right,
                    _filterButtonImageSize.Height + _filterButtonMargin.Bottom + _filterButtonMargin.Top);
            }
        }
        public bool IsSortEnabled
        {
            get
            {
                return MenuStrip.IsSortEnabled;
            }
            set
            {
                MenuStrip.IsSortEnabled = value;
            }
        }
        public bool IsFilterEnabled
        {
            get
            {
                return MenuStrip.IsFilterEnabled;
            }
            set
            {
                MenuStrip.IsFilterEnabled = value;
            }
        }

        private Image _filterImage = Properties.Resources.ColumnHeader_UnFiltered;
        private Size _filterButtonImageSize = new Size(16, 16);
        private bool _filterButtonPressed = false;
        private bool _filterButtonOver = false;
        private Rectangle _filterButtonOffsetBounds = Rectangle.Empty;
        private Rectangle _filterButtonImageBounds = Rectangle.Empty;
        private Padding _filterButtonMargin = new Padding(3, 4, 3, 4);
        private bool _filterEnabled = false;

        public ColumnHeader(DataGridViewColumnHeaderCell cell, bool filterEnabled)
        {
            Tag = cell.Tag;
            ErrorText = cell.ErrorText;
            ToolTipText = cell.ToolTipText;
            Value = cell.Value;
            ValueType = cell.ValueType;
            ContextMenuStrip = cell.ContextMenuStrip;
            Style = cell.Style;
            _filterEnabled = filterEnabled;

            ColumnHeader oldCellt = cell as ColumnHeader;

            if (oldCellt != null && oldCellt.MenuStrip != null)
            {
                MenuStrip = oldCellt.MenuStrip;
                _filterImage = oldCellt._filterImage;
                _filterButtonPressed = oldCellt._filterButtonPressed;
                _filterButtonOver = oldCellt._filterButtonOver;
                _filterButtonOffsetBounds = oldCellt._filterButtonOffsetBounds;
                _filterButtonImageBounds = oldCellt._filterButtonImageBounds;
                MenuStrip.FilterChanged += new EventHandler(menuStrip_FilterChanged);
                MenuStrip.SortChanged += new EventHandler(menuStrip_SortChanged);
                MenuStrip.HexChanged += new EventHandler(menuStrip_HexChanged);
                MenuStrip.HideChanged += new EventHandler(menuStrip_HideChanged);
            }
            else
            {
                MenuStrip = new ColumnMenu(cell.OwningColumn.ValueType);
                MenuStrip.FilterChanged += new EventHandler(menuStrip_FilterChanged);
                MenuStrip.SortChanged += new EventHandler(menuStrip_SortChanged);
                MenuStrip.HexChanged += new EventHandler(menuStrip_HexChanged);
                MenuStrip.HideChanged += new EventHandler(menuStrip_HideChanged);
                MenuStrip.IsSortEnabled = true;
                MenuStrip.IsFilterEnabled = true;
            }
        }

        ~ColumnHeader()
        {
            if (MenuStrip != null)
            {
                MenuStrip.FilterChanged -= menuStrip_FilterChanged;
                MenuStrip.SortChanged -= menuStrip_SortChanged;
                MenuStrip.HexChanged -= menuStrip_HexChanged;
                MenuStrip.HideChanged -= menuStrip_HideChanged;
            }
        }

        public override object Clone()
        {
            return new ColumnHeader(this, FilterAndSortEnabled);
        }

        #region Public Methods
        public void SetLoadedMode(bool enabled)
        {
            MenuStrip.SetLoadedMode(enabled);
            RefreshImage();
            RepaintCell();
        }

        public void CleanSort()
        {
            if (MenuStrip != null && FilterAndSortEnabled)
            {
                MenuStrip.CleanSort();
                RefreshImage();
                RepaintCell();
            }
        }

        public void CleanFilter()
        {
            if (MenuStrip != null && FilterAndSortEnabled)
            {
                MenuStrip.CleanFilter();
                RefreshImage();
                RepaintCell();
            }
        }

        public void SortASC()
        {
            if (MenuStrip != null && FilterAndSortEnabled)
                MenuStrip.SortASC();
        }

        public void SortDESC()
        {
            if (MenuStrip != null && FilterAndSortEnabled)
                MenuStrip.SortDESC();
        }

        public void SetSortEnabled(bool enabled)
        {
            if (MenuStrip != null)
            {
                MenuStrip.IsSortEnabled = enabled;
                MenuStrip.SetSortEnabled(enabled);
            }
        }

        public void SetFilterEnabled(bool enabled)
        {
            if (MenuStrip != null)
            {
                MenuStrip.IsFilterEnabled = enabled;
                MenuStrip.SetFilterEnabled(enabled);
            }
        }
        #endregion


        #region Events
        private void menuStrip_FilterChanged(object sender, EventArgs e)
        {
            RefreshImage();
            RepaintCell();
            if (FilterAndSortEnabled && FilterChanged != null)
                FilterChanged(this, new ColumnHeaderCellEventArgs(MenuStrip, OwningColumn));
        }

        private void menuStrip_SortChanged(object sender, EventArgs e)
        {
            RefreshImage();
            RepaintCell();
            if (FilterAndSortEnabled && SortChanged != null)
                SortChanged(this, new ColumnHeaderCellEventArgs(MenuStrip, OwningColumn));
        }

        private void menuStrip_HideChanged(object sender, EventArgs e)
        {
            if (HideChanged != null)
                HideChanged(this, new ColumnHeaderCellEventArgs(MenuStrip, OwningColumn));
        }

        private void menuStrip_HexChanged(object sender, EventArgs e)
        {
            if (HexChanged != null)
                HexChanged(this, new ColumnHeaderCellEventArgs(MenuStrip, OwningColumn));
        }
        #endregion


        #region Painting
        private void RepaintCell()
        {
            if (Displayed && DataGridView != null)
                DataGridView.InvalidateCell(this);
        }

        private void RefreshImage()
        {
            if (ActiveFilterType == ColumnMenu.FilterType.Loaded)
            {
                _filterImage = Properties.Resources.ColumnHeader_UnFiltered;
            }
            else
            {
                if (ActiveFilterType == ColumnMenu.FilterType.None)
                {
                    if (ActiveSortType == ColumnMenu.SortType.None)
                        _filterImage = Properties.Resources.ColumnHeader_UnFiltered;
                    else if (ActiveSortType == ColumnMenu.SortType.ASC)
                        _filterImage = Properties.Resources.ColumnHeader_OrderedASC;
                    else
                        _filterImage = Properties.Resources.ColumnHeader_OrderedDESC;
                }
                else
                {
                    if (ActiveSortType == ColumnMenu.SortType.None)
                        _filterImage = Properties.Resources.ColumnHeader_Filtered;
                    else if (ActiveSortType == ColumnMenu.SortType.ASC)
                        _filterImage = Properties.Resources.ColumnHeader_FilteredAndOrderedASC;
                    else
                        _filterImage = Properties.Resources.ColumnHeader_FilteredAndOrderedDESC;
                }
            }
        }

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates cellState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            if (SortGlyphDirection != SortOrder.None)
                SortGlyphDirection = SortOrder.None;

            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                cellState, value, formattedValue,
                errorText, cellStyle, advancedBorderStyle, paintParts);

            if (FilterAndSortEnabled && paintParts.HasFlag(DataGridViewPaintParts.ContentBackground))
            {
                _filterButtonOffsetBounds = GetFilterBounds(true);
                _filterButtonImageBounds = GetFilterBounds(false);
                Rectangle buttonBounds = _filterButtonOffsetBounds;
                if (buttonBounds != null && clipBounds.IntersectsWith(buttonBounds))
                {
                    ControlPaint.DrawBorder(graphics, buttonBounds, Color.Gray, ButtonBorderStyle.Solid);
                    buttonBounds.Inflate(-1, -1);
                    using (Brush b = new SolidBrush(_filterButtonOver ? Color.WhiteSmoke : Color.White))
                        graphics.FillRectangle(b, buttonBounds);
                    graphics.DrawImage(_filterImage, buttonBounds);
                }
            }
        }

        private Rectangle GetFilterBounds(bool withOffset = true)
        {
            Rectangle cell = DataGridView.GetCellDisplayRectangle(ColumnIndex, -1, false);

            Point p = new Point(
                (withOffset ? cell.Right : cell.Width) - _filterButtonImageSize.Width - (_filterButtonMargin.Right / 2) - 1,
                (withOffset ? cell.Bottom : cell.Height) - _filterButtonImageSize.Height - ((cell.Height - _filterButtonImageSize.Height) / 2));

            return new Rectangle(p, _filterButtonImageSize);
        }
        #endregion


        #region Mouse Events
        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            if (FilterAndSortEnabled)
            {
                if (_filterButtonImageBounds.Contains(e.X, e.Y) && !_filterButtonOver)
                {
                    _filterButtonOver = true;
                    RepaintCell();
                }
                else if (!_filterButtonImageBounds.Contains(e.X, e.Y) && _filterButtonOver)
                {
                    _filterButtonOver = false;
                    RepaintCell();
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (FilterAndSortEnabled && _filterButtonImageBounds.Contains(e.X, e.Y))
            {
                if (e.Button == MouseButtons.Left && !_filterButtonPressed)
                {
                    _filterButtonPressed = true;
                    _filterButtonOver = true;
                    RepaintCell();
                }
            }
            else
                base.OnMouseDown(e);
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if (FilterAndSortEnabled && e.Button == MouseButtons.Left && _filterButtonPressed)
            {
                _filterButtonPressed = false;
                _filterButtonOver = false;
                RepaintCell();
                if (_filterButtonImageBounds.Contains(e.X, e.Y) && FilterPopup != null)
                {
                    FilterPopup(this, new ColumnHeaderCellEventArgs(MenuStrip, OwningColumn));
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            if (FilterAndSortEnabled && _filterButtonOver)
            {
                _filterButtonOver = false;
                RepaintCell();
            }

            base.OnMouseLeave(rowIndex);
        }

        #endregion

    }

    internal delegate void ColumnHeaderCellEventHandler(object sender, ColumnHeaderCellEventArgs e);
    internal class ColumnHeaderCellEventArgs : EventArgs
    {
        public ColumnMenu FilterMenu { get; private set; }

        public DataGridViewColumn Column { get; private set; }

        public ColumnHeaderCellEventArgs(ColumnMenu filterMenu, DataGridViewColumn column)
        {
            FilterMenu = filterMenu;
            Column = column;
        }
    }

}