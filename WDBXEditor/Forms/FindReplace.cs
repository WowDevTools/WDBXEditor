using ADGV;
using WDBXEditor.Common;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WDBXEditor.Storage;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace WDBXEditor
{
    public partial class FindReplace : Form
    {
        public bool Replace { get; set; } = false;

        private AdvancedDataGridView _data;
        private bool _closing = false;
        private RegexOptions replaceOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
        private StringComparison compare = StringComparison.CurrentCultureIgnoreCase;

        public FindReplace()
        {
            InitializeComponent();
        }

        private void FindReplace_Load(object sender, EventArgs e)
        {
            _data = (AdvancedDataGridView)((Main)Owner).Controls.Find("advancedDataGridView", true)[0];
            SetScreenType(Replace);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        public void SetScreenType(bool replace)
        {
            if (replace)
            {
                this.Text = "Replace";
                txtReplace.Enabled = true;
                btnReplace.Enabled = true;
                btnReplaceAll.Enabled = true;
            }
            else
            {
                this.Text = "Find";
                txtReplace.Enabled = false;
                btnReplace.Enabled = false;
                btnReplaceAll.Enabled = false;
            }
        }

        #region Button Event
        /// <summary>
        /// Finds the next occurence of the search term
        /// <para>Ignores currently selected cell</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (this.Opacity != 1) return;

            DataGridViewCell c = _data.CurrentCell;
            Point r = new Point(-1, -1);

            if (!rdoFlag.Checked)
                r = _data.Search(txtFind.Text, chkExact.Checked, compare);
            else if (rdoFlag.Checked && GetHex(txtFind.Text, out long Flag))
                r = _data.SearchFlag(Flag);

            if (r.X == -1 || r.Y == -1)
            {
                lblResult.Text = "No results found.";
                lblResult.Visible = true;
            }
            else if (r.X == c.RowIndex && r.Y == c.ColumnIndex)
            {
                lblResult.Text = "One result found.";
                lblResult.Visible = true;
            }
            else
            {
                _data.CurrentCell = _data.Rows[r.X].Cells[r.Y];
                lblResult.Visible = false;
            }

            Database.ForceGC();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Replaces the next occurence of the search term
        /// <para>Includes the currently selected cell</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (this.Opacity != 1) return;

            DataGridViewCell c = _data.CurrentCell;
            Point r = new Point(-1, -1);


            long findflag = 0;
            long replaceflag = 0;

            if (!rdoFlag.Checked)
            {
                r = _data.Search(txtFind.Text, chkExact.Checked, compare, true);
            }
            else if (rdoFlag.Checked && GetHex(txtFind.Text, out findflag))
            {
                r = _data.SearchFlag(findflag, true);
                GetHex(txtReplace.Text, out replaceflag);
            }

            if ((r.X == -1 || r.Y == -1))
            {
                lblResult.Text = "No results found.";
                lblResult.Visible = true;
                return;
            }
            else
            {
                _data.CurrentCell = _data.Rows[r.X].Cells[r.Y];
                lblResult.Visible = false;

                _data.BeginEdit(false);

                if (!rdoFlag.Checked)
                {
                    string previous = _data.CurrentCell.Value.ToString();
                    _data.CurrentCell.Value = previous.Replace(txtFind.Text, txtReplace.Text, replaceOptions);
                }
                else
                {
                    long previous = Convert.ToInt64(_data.CurrentCell.Value);
                    previous &= ~findflag;
                    previous |= replaceflag;
                    _data.CurrentCell.Value = Convert.ChangeType(previous, _data.CurrentCell.Value.GetType());
                }


                _data.EndEdit();
            }

            Database.ForceGC();
        }

        /// <summary>
        /// Replace all iterates the entire datagrid and performs replacements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            int found = 0;
            var start = _data.CurrentCell;
            long findflag = 0;
            GetHex(txtReplace.Text, out long replaceflag);

            HashSet<Point> completedCells = new HashSet<Point>();

            bool exit = false;
            while (!exit)
            {
                Point cell = new Point(-1, -1);

                if (!rdoFlag.Checked)
                {
                    cell = _data.Search(txtFind.Text, chkExact.Checked, compare, true);
                }
                else if (rdoFlag.Checked && GetHex(txtFind.Text, out findflag))
                {
                    cell = _data.SearchFlag(findflag, true, completedCells);                    
                }

                if (cell.X == -1 || cell.Y == -1)
                {
                    exit = true;
                }
                else
                {
                    completedCells.Add(cell);

                    found++;
                    _data.CurrentCell = _data.Rows[cell.X].Cells[cell.Y];

                    _data.BeginEdit(false);

                    if (!rdoFlag.Checked)
                    {
                        string previous = _data.Rows[cell.X].Cells[cell.Y].Value.ToString();
                        _data.CurrentCell.Value = previous.Replace(txtFind.Text, txtReplace.Text, replaceOptions);
                    }
                    else
                    {
                        long previous = Convert.ToInt64(_data.Rows[cell.X].Cells[cell.Y].Value);
                        previous &= ~findflag;
                        previous |= replaceflag;
                        _data.CurrentCell.Value = Convert.ChangeType(previous, _data.CurrentCell.Value.GetType());
                    }

                    _data.EndEdit();
                }
            }

            lblResult.Text = $"Replaced {found} records.";
            lblResult.Visible = true;
        }

        private void chkCase_CheckedChanged(object sender, EventArgs e)
        {
            compare = chkCase.Checked ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

            if (chkCase.Checked)
                replaceOptions = RegexOptions.Multiline;
            else
                replaceOptions = (RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
        #endregion

        #region Text Event
        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            btnFind.Enabled = txtFind.Text.Length > 0;
            if (Replace)
            {
                btnReplace.Enabled = txtFind.Text.Length > 0;
                btnReplaceAll.Enabled = txtFind.Text.Length > 0;
            }
        }
        #endregion

        #region Form Events
        private void FindReplace_Activated(object sender, EventArgs e)
        {
            if (_closing) return;
            this.Opacity = 1;
        }

        private void FindReplace_Deactivate(object sender, EventArgs e)
        {
            if (_closing) return;
            this.Opacity = 0.75f;
        }

        private void FindReplace_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closing = true;
        }
        #endregion


        private bool GetHex(string value, out long flag)
        {
            if (value.StartsWith("0x"))
                value = value.Substring(2);

            bool success = long.TryParse(value, NumberStyles.HexNumber, null, out long l);
            flag = l;
            return success;
        }

    }
}
