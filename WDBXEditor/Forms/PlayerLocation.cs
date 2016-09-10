using System;
using System.Diagnostics;
using System.Windows.Forms;
using WDBXEditor.Reader.Memory;
using System.Linq;
using static WDBXEditor.Common.Constants;
using System.Collections.Generic;

namespace WDBXEditor.Forms
{
    public partial class PlayerLocation : Form
    {
        private MemoryReader reader = null;
        private OffsetMap offsets;

        public PlayerLocation()
        {
            InitializeComponent();
        }

        private void PlayerLocation_Load(object sender, EventArgs e)
        {
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
        }

        private void LoadProcesses()
        {
            var procs = Process.GetProcesses().Where(x => x.ProcessName.IndexOf("wow", IGNORECASE) >= 0);
            foreach (var proc in procs)
                cbProcessSelector.Items.Add(new KeyValuePair<string,Process>($"{proc.ProcessName} : {proc.Id}", proc));

            cbProcessSelector.Items.Insert(0, new KeyValuePair<string, Process>("", null));
            cbProcessSelector.ValueMember = "Value";
            cbProcessSelector.DisplayMember = "Key";
        }

        #endregion

        private void cbProcessSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var proc = ((KeyValuePair<string, Process>)cbProcessSelector.SelectedItem).Value;
            var version = proc.MainModule.FileVersionInfo.FileVersion.Split(' ').Last();

            int finalbuild = 0;
            if(int.TryParse(version, out finalbuild))
            {
                switch(finalbuild)
                {
                    case (int)ExpansionFinalBuild.Classic:
                    case (int)ExpansionFinalBuild.TBC:
                    case (int)ExpansionFinalBuild.WotLK:
                    case (int)ExpansionFinalBuild.Cata:
                        cbBuildSelector.SelectedText = BuildText(finalbuild);
                        break;
                    case (int)ExpansionFinalBuild.MoP:
                        break;
                    case (int)ExpansionFinalBuild.WoD:
                        break;
                    default:
                        cbBuildSelector.SelectedText = "Custom";
                        break;
                }
            }
        }

    }
}
