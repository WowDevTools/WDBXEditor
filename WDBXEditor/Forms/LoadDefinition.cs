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

            var builds = Database.Definitions.Tables.Select(x => x.Build).ToList().Distinct().OrderBy(x => x);

            if ((Files?.Count() ?? 0) > 0)
            {
                //Get compatible builds only
                bool db2 = Files.Any(x => Path.GetExtension(x).IndexOf("db2", StringComparison.CurrentCultureIgnoreCase) >= 0);
                bool adb = Files.Any(x => Path.GetExtension(x).IndexOf("adb", StringComparison.CurrentCultureIgnoreCase) >= 0);
                var files = Files.Select(x => Path.GetFileNameWithoutExtension(x).ToLower());
                builds = Database.Definitions.Tables.Where(x => files.Contains(x.Name.ToLower()))
                                                   .Select(x => x.Build).ToList().Distinct()
                                                   .OrderBy(x => x);
                //Filter out non DB2/ADB clients
                if (db2 || adb)
                    builds = builds.Where(x => x > (int)ExpansionFinalBuild.WotLK).OrderBy(x => x);
            }

            //Create a datasource of true build number and nice text i.e. 12340, "WotLK 3.3.5 - 12340" 
            lbDefinitions.BeginUpdate();

            var datasource = builds.Select(x => new { Key = x, Value = BuildText(x) });
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
