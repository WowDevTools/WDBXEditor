using ADGV;
using System;
using System.Drawing;
using System.Windows.Forms;
using WDBXEditor.Common;

namespace WDBXEditor.Forms
{
    public partial class ColourConverter : Form
    {
        private AdvancedDataGridView _data;
        private bool _closing = false;

        private Func<Color, uint> ColourToInt = c => BitConverter.ToUInt32(new byte[] { c.B, c.G, c.R, 0 }, 0);
        private Func<uint, Color> UIntToColor = i =>
        {
            var bytes = BitConverter.GetBytes(i);
            return Color.FromArgb(0, bytes[2], bytes[1], bytes[0]); //Alpha always 0
        };

        #region Colour Events
        public ColourConverter()
        {
            InitializeComponent();
            colourWheelChanged(colourWheel, null);
        }

        private void ColourConverter_Load(object sender, EventArgs e)
        {
            _data = (AdvancedDataGridView)((Main)Owner).Controls.Find("advancedDataGridView", true)[0];
        }

        private void colourWheelChanged(object sender, EventArgs e)
        {
            txtRed.Text = colourWheel.CurrentColour.R.ToString();
            txtGreen.Text = colourWheel.CurrentColour.G.ToString();
            txtBlue.Text = colourWheel.CurrentColour.B.ToString();
            picColour.BackColor = colourWheel.CurrentColour;
            txtWoWVal.Text = ColourToInt(colourWheel.CurrentColour).ToString();
        }

        private void txtWoWVal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtWoWVal_KeyUp(object sender, KeyEventArgs e)
        {
            ulong dmp;
            if (!ulong.TryParse(txtWoWVal.Text, out dmp))
                txtWoWVal.Text = "0";
            else if (dmp > uint.MaxValue)
                txtWoWVal.Text = uint.MaxValue.ToString();

            colourWheel.CurrentColour = UIntToColor(Convert.ToUInt32(txtWoWVal.Text));
            txtRed.Text = colourWheel.CurrentColour.R.ToString();
            txtGreen.Text = colourWheel.CurrentColour.G.ToString();
            txtBlue.Text = colourWheel.CurrentColour.B.ToString();
            picColour.BackColor = colourWheel.CurrentColour;
        }

        private void txtColourKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }
        }

        private void txtColourKeyUp(object sender, KeyEventArgs e)
        {
            ulong dmp;
            if (!ulong.TryParse(txtBlue.Text, out dmp)) //Check blue
                txtBlue.Text = "0";
            else if (dmp > 255)
                txtBlue.Text = "255";

            if (!ulong.TryParse(txtRed.Text, out dmp)) //Check red
                txtRed.Text = "0";
            else if (dmp > 255)
                txtRed.Text = "255";

            if (!ulong.TryParse(txtGreen.Text, out dmp)) //Check green
                txtGreen.Text = "0";
            else if (dmp > 255)
                txtGreen.Text = "255";

            colourWheel.CurrentColour = Color.FromArgb(0, Convert.ToInt32(txtRed.Text), Convert.ToInt32(txtGreen.Text), Convert.ToInt32(txtBlue.Text));
            picColour.BackColor = colourWheel.CurrentColour;
            txtWoWVal.Text = ColourToInt(colourWheel.CurrentColour).ToString();
        }
        #endregion

        #region Button Events
        private void btnGet_Click(object sender, EventArgs e)
        {
            ulong dmp;
            ulong.TryParse(_data.CurrentCell.Value.ToString(), out dmp);
            if (dmp > uint.MaxValue)
                dmp = uint.MaxValue;

            colourWheel.CurrentColour = UIntToColor((uint)dmp);
            colourWheelChanged(colourWheel, null);
        }

        private void betSet_Click(object sender, EventArgs e)
        {
            uint value = ColourToInt(colourWheel.CurrentColour);

            _data.BeginEdit(true);
            _data.CurrentCell.Value = value;
            _data.EndEdit();
        }
        #endregion

        #region Form Events
        private void ColourConverter_Activated(object sender, EventArgs e)
        {
            if (_closing) return;
            this.Opacity = 1;
        }

        private void ColourConverter_Deactivate(object sender, EventArgs e)
        {
            if (_closing) return;
            this.Opacity = 0.75f;
        }

        private void ColourConverter_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closing = true;
        }


        #endregion

    }
}
