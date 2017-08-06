using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WDBXEditor.Reader.FileTypes;
using WDBXEditor.Storage;

namespace WDBXEditor.Forms
{
    public partial class LoadHotfix : Form
    {
        private DBEntry Hotfix;

        public LoadHotfix()
        {
            InitializeComponent();
        }

        private void LoadHotfix_Load(object sender, EventArgs e)
        {
            Hotfix = Database.Entries.FirstOrDefault(x => x.Header.IsTypeOf<HTFX>()); //Get our hotfix entry

            //Get all loaded entries that are contained in our hotfix entry
            var datasource = Database.Entries.Where(x => ((HTFX)Hotfix.Header).HasEntry(x.Header))
                                     .Select(x => new
                                     {
                                         Key = x,
                                         Value = $"{x.FileName} ({x.TableStructure.Build})"
                                     });

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
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var counterpart = (lbDefinitions.SelectedValue as DBEntry)?.Header;
            (Hotfix.Header as HTFX).Read(counterpart, Hotfix);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
