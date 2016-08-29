using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WDBXEditor.Forms
{
    public partial class ErrorReport : Form
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set;}

        public ErrorReport()
        {
            InitializeComponent();
        }

        private void ErrorReport_Load(object sender, EventArgs e)
        {
            txtErrors.Clear();
            txtErrors.SelectionBullet = true;
            txtErrors.SelectedText = string.Join("\n", Errors);
        }
    }
}
