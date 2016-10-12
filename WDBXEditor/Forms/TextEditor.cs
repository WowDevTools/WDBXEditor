using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WDBXEditor.Common.Constants;

namespace WDBXEditor.Forms
{
    public partial class TextEditor : Form
    {
        public string CellValue
        {
            get { return txtText.Text; }
            set
            {
                txtText.Text = value;
                txtText.SelectionStart = 0;
                txtText.SelectionLength = 0;
            }
        }

        public TextEditor()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
                return;

            int filterlen = txtFilter.Text.Length;
            int start = txtText.SelectionStart + (txtText.SelectedText.Equals(txtFilter.Text, IGNORECASE) ? filterlen : 0); //Skip currently found if applicable

            StartFind:
            var indexof = txtText.Text.IndexOf(txtFilter.Text, start, IGNORECASE);
            if (indexof == -1)
            {
                if (start == 0) //Have looped and no matches
                {
                    lblFind.Visible = true;
                    return;
                }

                start = 0;
                goto StartFind; //Search from the start of the text
            }
            else
            {
                txtText.SelectionStart = indexof; //Highlight the next found instance
                txtText.SelectionLength = filterlen;
                txtText.ScrollToCaret();
                lblFind.Visible = false;
            }
        }
    }
}
