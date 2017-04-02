using WDBXEditor.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WDBXEditor.Common.Constants;

namespace WDBXEditor
{
    public partial class EditDefinition : Form
    {
        private Table currentTable;
        private BindingSource bindingSource = new BindingSource();

        public EditDefinition()
        {
            InitializeComponent();
        }

        private void EditDefinition_Load(object sender, EventArgs e)
        {
            LoadDefinitions();

            dgvDefintion.GetType()
                        .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                        .SetValue(dgvDefintion, true, null);

            if (lbFiles.Items.Count > 0)
                LoadTable((Table)(((DataRowView)lbFiles.Items[0]).Row[0]));
        }

        private void LoadDefinitions(bool applyfilter = false)
        {
            string buildFilter = cbBuild.Text;
            string textFilter = txtFilter.Text;

            DataTable dt = new DataTable();
            dt.Columns.Add("Key", typeof(Table));
            dt.Columns.Add("Value", typeof(string));

            var tables = Database.Definitions.Tables.OrderBy(x => x.Name).ThenBy(x => x.Build);
            foreach (var t in tables)
                dt.Rows.Add(t, $"{t.Name} - {t.BuildText}");

            bindingSource.DataSource = dt;
            lbFiles.BeginUpdate();
            lbFiles.ValueMember = "Key";
            lbFiles.DisplayMember = "Value";
            lbFiles.DataSource = bindingSource;
            lbFiles.EndUpdate();

            //Load dropdown filter
            var builds = tables.OrderBy(x => x.Build).Select(x => x.BuildText).Distinct().ToArray();
            cbBuild.Items.Clear();
            cbBuild.Items.Add("");
            cbBuild.Items.AddRange(builds);

            //Reapply filter
            if (applyfilter)
            {
                if (cbBuild.Items.Contains(buildFilter))
                    cbBuild.Text = buildFilter;

                txtFilter.Text = textFilter;
            }
        }

        private void LoadTable(Table tbl)
        {
            txtBuild.Text = tbl.Build.ToString();
            txtFileName.Text = tbl.Name;
            this.Text = $"Edit Definition - {tbl.Name} {tbl.BuildText}";

            dgvDefintion.RowHeadersVisible = false;
            dgvDefintion.ColumnHeadersVisible = false;
            dgvDefintion.SuspendLayout(); //Performance
            dgvDefintion.DataSource = new BindingSource(tbl.Fields, null);
            dgvDefintion.ResumeLayout();
            currentTable = tbl;
        }


        #region Button Events
        private void btnSave_Click(object sender, EventArgs e)
        {
            int build;
            List<Field> dataSource = (List<Field>)(((BindingSource)dgvDefintion.DataSource).DataSource);

            //Build check
            if (!int.TryParse(txtBuild.Text, out build) || build == 0)
            {
                MessageBox.Show("Please enter a valid build number.");
                return;
            }

            //Name check
            if (string.IsNullOrWhiteSpace(txtFileName.Text))
            {
                MessageBox.Show("Please enter a valid table name.");
                return;
            }

            //Index check
            if (dataSource.Count(x => x.IsIndex) != 1 || dataSource.FirstOrDefault(x => x.IsIndex)?.Type != "int")
            {
                MessageBox.Show("Please ensure there is exactly 1 index of int type.");
                return;
            }

            //Unqiue field name check
            if (dataSource.Count != dataSource.Select(x => x.Name.ToLower()).Distinct().Count())
            {
                MessageBox.Show("Fields must have unqiue names.");
                return;
            }

            currentTable.Build = int.Parse(txtBuild.Text);
            currentTable.Name = Path.GetFileNameWithoutExtension(txtFileName.Text);
            currentTable.Fields = new List<Field>(dataSource);
            currentTable.Load();

            //Check the amount of matches we have to prevent duplicate definitions
            int matches = Database.Definitions.Tables.Count(x => x.Build == currentTable.Build && x.Name == currentTable.Name);
            if (matches == 0)
            {
                Database.Definitions.Tables.Add(currentTable);
            }
            else if (matches == 1 && Database.Definitions.Tables.First(x => x.Build == currentTable.Build && x.Name == currentTable.Name) != currentTable)
            {
                MessageBox.Show("Matching definition has been found.");
                return;
            }
            else if (matches > 1)
            {
                MessageBox.Show("Multiple matching definitions have been found.");
                return;
            }

            if (Database.Definitions.SaveDefinitions())
            {
                LoadDefinitions(true);
                MessageBox.Show("Definitions Saved");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Table tmp = new Table();
            tmp.Build = 0;
            tmp.Name = "";
            tmp.Fields = new List<Field>();
            LoadTable(tmp);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (lbFiles.SelectedItem == null)
                return;

            Table tbl = (Table)(((DataRowView)lbFiles.SelectedItem).Row[0]);
            Table tmp = new Table();
            tmp.Name = tbl.Name;
            tmp.Fields = new List<Field>(tbl.Fields);
            LoadTable(tmp);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbFiles.SelectedItem == null)
                return;

            Table tbl = (Table)(((DataRowView)lbFiles.SelectedItem).Row[0]);
            Database.Definitions.Tables.Remove(tbl);
            LoadDefinitions(true);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            cbBuild.Text = "";
        }
        #endregion

        #region ListBox Events
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            bindingSource.Filter = $"([Value] LIKE '%{txtFilter.Text}%') AND [Value] LIKE '%{cbBuild.Text}%'";
        }

        private void cbBuild_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindingSource.Filter = $"([Value] LIKE '%{txtFilter.Text}%') AND [Value] LIKE '%{cbBuild.Text}%'";
        }

        private void lbFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lbFiles.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                Table tbl = (Table)(((DataRowView)lbFiles.Items[index]).Row[0]);
                LoadTable(tbl);
            }
        }
        #endregion]

        #region DataGridView Events
        private void dgvDefintion_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (currentTable == null) return;

            currentTable.Changed = true;

            //Update to prevent multiple indexes
            if (e.ColumnIndex == dgvDefintion.Columns["colIndex"].Index)
            {
                bool check = bool.Parse(dgvDefintion.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                if (check)
                {
                    for (int i = 0; i < dgvDefintion.Rows.Count; i++)
                    {
                        if (i == e.RowIndex) continue;
                        dgvDefintion.Rows[i].Cells[e.ColumnIndex].Value = false;
                    }
                }
            }
        }

        private void dgvDefintion_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView.HitTestInfo info = dgvDefintion.HitTest(e.X, e.Y);
            if (info.Type == DataGridViewHitTestType.Cell)
            {
                if ((dgvDefintion.Rows[info.RowIndex].Cells[info.ColumnIndex] as DataGridViewComboBoxCell) != null)
                    dgvDefintion.CurrentCell = dgvDefintion.Rows[info.RowIndex].Cells[info.ColumnIndex];
            }
        }

        private void dgvDefintion_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Reset speed improvements
            dgvDefintion.RowHeadersVisible = true;
            dgvDefintion.ColumnHeadersVisible = true;
            dgvDefintion.ResumeLayout(false);
        }
        #endregion

        private void dgvDefintion_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            Size textSize = TextRenderer.MeasureText(rowIdx, this.Font);
            if (grid.RowHeadersWidth < textSize.Width + 40)
                grid.RowHeadersWidth = textSize.Width + 40;

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
    }
}
