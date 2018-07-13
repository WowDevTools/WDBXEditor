using WDBXEditor.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static WDBXEditor.Common.Constants;

namespace WDBXEditor
{
    public partial class LoadDefinition : Form
    {
        public IEnumerable<string> Files { get; set; }

        public LoadDefinition()
        {
            InitializeComponent();
            openFileDialog.InitialDirectory = DEFINITION_DIR;
        }

        private void LoadDefinition_Load(object sender, EventArgs e)
        {
            btnLoad.Enabled = false;
            LoadBuilds();
        }

        public void UpdateFiles(IEnumerable<string> files)
        {
            Files = Files.Concat(files);
            LoadBuilds();
        }

        #region Button Events
        private void btnLoad_Click(object sender, EventArgs e)
        {
            int build = (int)lbDefinitions.SelectedValue;
            Database.BuildNumber = build;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnNewWindow_Click(object sender, EventArgs e)
        {
            if (InstanceManager.LoadNewInstance(Files))
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
        #endregion

        #region Listbox
        private void lbDefinitions_SelectedValueChanged(object sender, EventArgs e)
        {
            btnLoad.Enabled = lbDefinitions.SelectedItems.Count > 0;
        }

        private void lbDefinitions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbDefinitions.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                int build = (int)lbDefinitions.SelectedValue;
                Database.BuildNumber = build;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        #endregion

        private void LoadBuilds(bool mostRecent = true)
        {
            if (Database.Definitions.Tables.Count == 0)
            {
                MessageBox.Show("No defintions found.");
                return;
            }

            if (Files?.Count() == 0)
            {
                SetFileText();
                lbDefinitions.DataSource = null;
                MessageBox.Show("No files to load.");
                return;
            }

            //Get compatible builds only
            bool db2 = Files.Any(x => Path.GetExtension(x).IndexOf("db2", IGNORECASE) >= 0) || Files.Any(x => Path.GetExtension(x).IndexOf("adb", IGNORECASE) >= 0);

            var files = Files.Select(x => Path.GetFileNameWithoutExtension(x).ToLower());
			var datasource = Database.Definitions.Tables
												 .Where(x => files.Contains(x.Name.ToLower()))
												 .Select(x => new { Key = x.Build, Value = x.BuildText })
												 .Distinct()
												 .Where(x => db2 ? x.Key > (int)ExpansionFinalBuild.WotLK : true); // filter out non DB2/ADB clients

			// filter to the latest build for each version
			if (mostRecent)
				datasource = datasource.GroupBy(x => x.Value.Split('(').First()).Select(x => x.Aggregate((a, b) => a.Key > b.Key ? a : b));

			// order
			datasource = datasource.OrderBy(x => x.Key);


			lbDefinitions.BeginUpdate();
            
            if (datasource.Count() == 0)
            {
                lbDefinitions.DataSource = null;
            }
            else
            {
                lbDefinitions.DataSource = new BindingSource(datasource, null);
                lbDefinitions.DisplayMember = "Value";
                lbDefinitions.ValueMember = "Key";
            }

            SetFileText();
            lbDefinitions.EndUpdate();
        }

        private void SetFileText()
        {
            lblFiles.Text = Files.Count() == 1 ? "1 file" : Files.Count() + " files";
        }

		private void chkBuildFilter_CheckedChanged(object sender, EventArgs e)
		{
			LoadBuilds(!chkBuildFilter.Checked);
		}
	}
}
