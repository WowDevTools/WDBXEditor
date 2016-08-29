using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WDBXEditor.Storage;
using static System.Windows.Forms.CheckedListBox;

namespace WDBXEditor.Common
{
    public partial class DropdownCheckList : UserControl
    {
        public int ListHeight { get; set; } = 100;
        public ObjectCollection Items => lbItems.Items;
        public CheckedItemCollection CheckedItems => lbItems.CheckedItems;

        private bool _loading = false;

        public DropdownCheckList()
        {
            InitializeComponent();
            SetState(false);
            cbBox.SelectedIndex = 0;
        }

        public void ToggleCheck(bool check)
        {
            _loading = true; //Prevent flashing text when loading items

            for (int i = 0; i < lbItems.Items.Count; i++)
            {
                if (i == lbItems.Items.Count - 1)
                {
                    //Last item force update                    
                    lbItems.SetItemChecked(i, false);
                    _loading = false;
                    lbItems.SetItemChecked(i, true);
                }
                else
                    lbItems.SetItemChecked(i, check);
            }
        }

        public void SetItemChecked(int index, bool value)
        {
            lbItems.SetItemChecked(index, value);
        }

        #region Dropdown Override
        private void cbBox_DropDown(object sender, EventArgs e)
        {
            if (lbItems.Items.Count > 0)
                SetState(true);
        }

        private void cbBox_DropDownClosed(object sender, EventArgs e)
        {
            if (lbItems.ClientRectangle.Contains(this.PointToClient(MousePosition)))
                return;

            SetState(false);
        }
        #endregion

        #region Mouse Leave Check
        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.MouseLeave += MouseLeaveCheck;
            base.OnControlAdded(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseLeaveCheck(this, e);
        }

        private void MouseLeaveCheck(object sender, EventArgs e)
        {
            if (this.ClientRectangle.Contains(this.PointToClient(MousePosition)))
                return;

            SetState(false);
            base.OnMouseLeave(e);
        }

        private void DropdownCheckList_Leave(object sender, EventArgs e)
        {
            SetState(false);
        }
        #endregion

        private void SetState(bool open)
        {
            if (open)
            {
                this.Height = cbBox.Height + ListHeight;
                lbItems.Visible = true;
                lbItems.Height = this.ListHeight;
                lbItems.BringToFront();

                if (lbItems.Items.Count > 0)
                {
                    cbBox.Enabled = false;
                    cbBox.Enabled = true;
                    lbItems.Focus();
                    lbItems.SelectedIndex = 0;
                    lbItems.Focus();
                    ActiveControl = lbItems;
                }
            }
            else
            {
                lbItems.Height = 0;
                lbItems.Visible = false;
                this.Height = cbBox.Height;
            }
        }

        #region Button Events
        private void btnReset_Click(object sender, EventArgs e)
        {
            ToggleCheck(true);
        }
        #endregion

        private void DropdownCheckList_EnabledChanged(object sender, EventArgs e)
        {
            cbBox.Enabled = this.Enabled;
            lbItems.Enabled = this.Enabled;
        }

        private void lbItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_loading) return;

            List<string> items = new List<string>();

            if (e.NewValue == CheckState.Checked)
            {
                if (lbItems.CheckedItems.Count == lbItems.Items.Count - 1)
                    items.Add("[All]");
                else if (lbItems.CheckedItems.Count == 0 && e.NewValue == CheckState.Checked)
                    items.Add(lbItems.Items[e.Index].ToString());
                else
                    for (int i = 0; i < lbItems.Items.Count; i++)
                        if (e.Index == i || lbItems.CheckedIndices.Contains(i))
                            items.Add(lbItems.Items[i].ToString());
            }
            else
            {
                if (lbItems.CheckedItems.Count == 1)
                    items.Add("");
                else if (lbItems.CheckedItems.Count == 2)
                    items.Add(lbItems.CheckedItems[0].ToString());
                else
                    for (int i = 0; i < lbItems.Items.Count; i++)
                        if (e.Index != i && lbItems.CheckedIndices.Contains(i))
                            items.Add(lbItems.Items[i].ToString());
            }

            cbBox.Items[0] = string.Join(", ", items);
        }

        private void cbBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;

            if (e.Index >= 0)
            {
                string s = (string)cbBox.Items[e.Index];
                if (s.Length > (cbBox.Width - cbBox.Margin.Left - cbBox.Margin.Right) / 6f)
                    s = s.Substring(0, (int)Math.Floor((cbBox.Width - cbBox.Margin.Left - cbBox.Margin.Right) / 6f) - 3) + "...";

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black), 2), r);

                e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                e.Graphics.DrawString(s, cbBox.Font, new SolidBrush(Color.Black), r, sf);
                e.DrawFocusRectangle();
            }
        }

        public void Reset(DataColumnCollection columns, bool resetitems)
        {
            lbItems.Items.Clear(); //Update column filter

            if (columns != null)
                lbItems.Items.AddRange(columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray());
            else
                cbBox.Items[0] = "[All]";

            if (resetitems)
                ToggleCheck(true);
        }


        public event ItemCheckEventHandler ItemCheckChanged
        {
            add { lbItems.ItemCheck += value; }
            remove { lbItems.ItemCheck -= value; }
        }

        public event EventHandler HideEmptyPressed
        {
            add { btnEmpty.Click += value; }
            remove { btnEmpty.Click -= value; }
        }
    }
}
