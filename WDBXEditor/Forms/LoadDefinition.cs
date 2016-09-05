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

        public void UpdateFiles(IEnumerable<string> files)
        {
            Files = Files.Concat(files);
            LoadBuilds();
        }

        private void LoadDefinition_Load(object sender, EventArgs e)
        {
            btnLoad.Enabled = false;
            LoadBuilds();
        }

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

        private void LoadBuilds()
        {
            if (Database.Definitions.Tables.Count == 0)
            {
                MessageBox.Show("No defintions found.");
                return;
            }

            if (Files?.Count() == 0)
            {
                lbDefinitions.DataSource = null;
                MessageBox.Show("No files to load.");
                return;
            }

            //Get compatible builds only
            bool db2 = Files.Any(x => Path.GetExtension(x).IndexOf("db2", IGNORECASE) >= 0);
            bool adb = Files.Any(x => Path.GetExtension(x).IndexOf("adb", IGNORECASE) >= 0);

            var files = Files.Select(x => Path.GetFileNameWithoutExtension(x).ToLower());
            var datasource = Database.Definitions.Tables
                                                 .Where(x => files.Contains(x.Name.ToLower()))
                                                 .Select(x => new { Key = x.Build, Value = x.BuildText })
                                                 .Distinct()
                                                 .OrderBy(x => x.Key);
            //Filter out non DB2/ADB clients
            if (db2 || adb)
                datasource = datasource.Where(x => x.Key > (int)ExpansionFinalBuild.WotLK).OrderBy(x => x.Key);

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

            lbDefinitions.EndUpdate();
        }
    }
}
