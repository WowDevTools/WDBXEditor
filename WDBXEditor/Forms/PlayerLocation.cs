using System;
using System.Diagnostics;
using System.Windows.Forms;
using WDBXEditor.Reader.Memory;
using System.Linq;
using static WDBXEditor.Common.Constants;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Globalization;

namespace WDBXEditor.Forms
{
    public partial class PlayerLocation : Form
    {
        private Process target = null;
        private MemoryReader reader = null;
        private OffsetMap offsets;
        private BindingSource _binding = new BindingSource();
        private bool _closing = false;

        private uint ClientConnection = 0;
        private uint ObjectManager = 0;
        private uint FirstObject = 0;

        public PlayerLocation()
        {
            InitializeComponent();
        }

        private void PlayerLocation_Load(object sender, EventArgs e)
        {
            LoadBindings();
            LoadBuilds();
            LoadProcesses();
        }

        #region Dropdowns
        private void LoadBuilds()
        {
            cbBuildSelector.Items.Add(new KeyValuePair<string, OffsetMap>("Custom", new OffsetMap()));

            cbBuildSelector.Items.Add(new KeyValuePair<string, OffsetMap>(BuildText((int)ExpansionFinalBuild.Classic), Offsets.Classic));
            cbBuildSelector.Items.Add(new KeyValuePair<string, OffsetMap>(BuildText((int)ExpansionFinalBuild.TBC), Offsets.TBC));
            cbBuildSelector.Items.Add(new KeyValuePair<string, OffsetMap>(BuildText((int)ExpansionFinalBuild.WotLK), Offsets.WotLK));
            cbBuildSelector.Items.Add(new KeyValuePair<string, OffsetMap>(BuildText((int)ExpansionFinalBuild.Cata), Offsets.Cata));
            cbBuildSelector.Items.Add(new KeyValuePair<string, OffsetMap>(BuildText((int)ExpansionFinalBuild.MoP) + " (x86)", Offsets.Mopx86));
            cbBuildSelector.Items.Add(new KeyValuePair<string, OffsetMap>(BuildText((int)ExpansionFinalBuild.MoP) + " (x64)", Offsets.Mopx86));
            cbBuildSelector.Items.Add(new KeyValuePair<string, OffsetMap>(BuildText((int)ExpansionFinalBuild.WoD) + " (x86)", Offsets.Mopx86));
            cbBuildSelector.Items.Add(new KeyValuePair<string, OffsetMap>(BuildText((int)ExpansionFinalBuild.WoD) + " (x64)", Offsets.Mopx86));

            cbBuildSelector.DisplayMember = "Key";
            cbBuildSelector.ValueMember = "Value";
        }

        private void LoadProcesses()
        {
            var procs = Process.GetProcesses().Where(x => x.ProcessName.IndexOf("wow", IGNORECASE) >= 0);
            foreach (var proc in procs)
                cbProcessSelector.Items.Add(new KeyValuePair<string, Process>($"{proc.ProcessName} : {proc.Id}", proc));

            cbProcessSelector.Items.Insert(0, new KeyValuePair<string, Process>("", null));
            cbProcessSelector.ValueMember = "Value";
            cbProcessSelector.DisplayMember = "Key";
        }

        private void LoadBindings()
        {
            _binding.DataSource = new OffsetMap();
            txtClientConnection.DataBindings.Add("Text", _binding, "ClientConnection", true);
            txtFirstObject.DataBindings.Add("Text", _binding, "FirstObjectOffset", true);
            txtGUID.DataBindings.Add("Text", _binding, "Guid", true);
            txtLocalGUID.DataBindings.Add("Text", _binding, "LocalGuidOffset", true);
            txtMapId.DataBindings.Add("Text", _binding, "MapID", true);
            txtNextObject.DataBindings.Add("Text", _binding, "NextObjectOffset", true);
            txtObjectManager.DataBindings.Add("Text", _binding, "ObjectManager", true);
            txtPosX.DataBindings.Add("Text", _binding, "Pos_X", true);
        }

        private void cbProcessSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            target = ((KeyValuePair<string, Process>)cbProcessSelector.SelectedItem).Value;
            var version = target.MainModule.FileVersionInfo.FileVersion.Split(' ').Last();

            int finalbuild = 0;
            if (int.TryParse(version, out finalbuild))
            {
                string key = string.Empty;

                switch (finalbuild)
                {
                    case (int)ExpansionFinalBuild.Classic:
                    case (int)ExpansionFinalBuild.TBC:
                    case (int)ExpansionFinalBuild.WotLK:
                    case (int)ExpansionFinalBuild.Cata:
                        key = BuildText(finalbuild);
                        break;
                    case (int)ExpansionFinalBuild.MoP:
                    case (int)ExpansionFinalBuild.WoD:
                        key = BuildText(finalbuild) + (Is64Bit(target) ? " x64" : " x86");
                        break;
                    default:
                        key = "Custom";
                        break;
                }

                for (int i = 0; i < cbBuildSelector.Items.Count; i++)
                {
                    if (((KeyValuePair<string, OffsetMap>)cbBuildSelector.Items[i]).Key == key)
                    {
                        cbBuildSelector.SelectedIndex = i;
                        break;
                    }
                }
            }

            btnTarget.Enabled = true;
        }

        private void cbBuildSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            _binding.DataSource = ((KeyValuePair<string, OffsetMap>)cbBuildSelector.SelectedItem).Value;
            offsets = (OffsetMap)_binding.DataSource;
        }
        #endregion
        
        public static bool Is64Bit(Process process)
        {
            if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "x86")
                return false;

            byte[] data = new byte[4096];
            using (Stream s = new FileStream(process.MainModule.FileName, FileMode.Open, FileAccess.Read))
                s.Read(data, 0, 4096);

            int PE_HEADER_ADDR = BitConverter.ToInt32(data, 0x3C);
            return BitConverter.ToUInt16(data, PE_HEADER_ADDR + 0x4) != 0x014c; //32bit check
        }

        #region Form Events
        private void PlayerLocation_Activated(object sender, EventArgs e)
        {
            if (_closing) return;
            this.Opacity = 1;
        }

        private void PlayerLocation_Deactivate(object sender, EventArgs e)
        {
            if (_closing) return;
            this.Opacity = 0.75f;
        }

        private void PlayerLocation_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closing = true;
        }
        #endregion

        private void Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);

            ulong dmp;
            string text = (((TextBox)sender).Text + e.KeyChar).ToUpper();
            if (text.IndexOf("0X") == 0 && text.Length > 2) //Trim hex prefix to pass Hex validation
                text = text.Substring(2);

            if (!ulong.TryParse(text, out dmp) && //Number parse
                !ulong.TryParse(text, NumberStyles.HexNumber, null,  out dmp) && //Hex parse
                text != "0X" && //Hex prefix
                !char.IsControl(e.KeyChar)) //Control char
                e.Handled = true;

            if (e.KeyChar == 'X')
                e.KeyChar = char.ToLower(e.KeyChar); //Lower case X in hex prefix
        }

        #region Player Scan
        private bool LoadAddresses()
        {
            try
            {
                ClientConnection = reader.Read<uint>((IntPtr)(offsets.ObjectManager));
                ObjectManager = reader.Read<uint>((IntPtr)(ClientConnection + offsets.ObjectManager));
                FirstObject = reader.Read<uint>((IntPtr)(ObjectManager + offsets.FirstObjectOffset));
                return true;
                //LocalPlayer.Guid = reader.Read<ulong>((IntPtr)(ObjectManager + offsets.LocalGuidOffset));
                //return LocalPlayer.Guid != 0;
            }
            catch { return false; }
        }
        #endregion

        private void btnTarget_Click(object sender, EventArgs e)
        {
            reader = new MemoryReader(target);
        }
    }
}
