using WDBXEditor.Storage;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WDBXEditor.Common.Constants;

namespace WDBXEditor
{
    public partial class LoadSQL : Form
    {
        public string ConnectionString => $"Server={txtHost.Text};Port={txtPort.Text};Database={ddlDatabases.Text};Uid={txtUser.Text};Pwd={txtPass.Text};";
        public bool ConnectionOnly { get; set; } = false;
        public DBEntry Entry { get; set; }
        public string ErrorMessage = string.Empty;

        private bool testedconnection = false;

        public LoadSQL()
        {
            InitializeComponent();
        }

        private void LoadSQL_Load(object sender, EventArgs e)
        {
            if (ConnectionOnly)
            {
                ddlTable.Enabled = false;
                radNewOnly.Enabled = false;
                radOverride.Enabled = false;
                radUpdate.Enabled = false;
                label6.Enabled = false;
                label7.Enabled = false;
                label8.Enabled = false;
                btnLoad.Text = "Export";
                ddlDatabases.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            LoadSettings();
        }

        #region Buttons
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ddlDatabases.Items.Clear();
            testedconnection = false;
            ddlDatabases.Enabled = false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SHOW DATABASES;", connection);
                    using (var rdr = command.ExecuteReader())
                    {
                        ddlDatabases.Items.Add("");

                        while (rdr.Read())
                            ddlDatabases.Items.Add(rdr[0].ToString());

                        testedconnection = true;
                        ddlDatabases.Enabled = true;
                        SaveSettings();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!ConnectionOnly)
            {
                ((Main)this.Owner).ProgressBarHandle(true, "Importing SQL...");
                this.Enabled = false;

                string table = ddlTable.Text;
                string constring = ConnectionString;

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        UpdateMode mode = UpdateMode.Insert;
                        if (radUpdate.Checked)
                            mode = UpdateMode.Update;
                        else if (radOverride.Checked)
                            mode = UpdateMode.Replace;

                        if (Entry.ImportSQL(mode, constring, table, out ErrorMessage))
                            return DialogResult.OK;
                        else
                            return DialogResult.Abort;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return DialogResult.Abort;
                    }

                })
				.ContinueWith(x =>
                {
                    this.Enabled = true;
                    this.DialogResult = x.Result;
                    this.Close();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        #endregion

        #region Dropdown Methods
        private void ddlDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTable.Enabled)
            {
                ddlTable.Items.Clear();

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand($"USE {ddlDatabases.Text}; SHOW TABLES;", connection);
                        using (var rdr = command.ExecuteReader())
                        {
                            ddlTable.Items.Add("");
                            while (rdr.Read())
                                ddlTable.Items.Add(rdr[0].ToString());
                        }
                    }
                }
                catch { return; }
            }

            btnLoad.Enabled = !string.IsNullOrWhiteSpace(ddlDatabases.Text) && //Database selected
                              testedconnection && //Connection works
                              (!string.IsNullOrWhiteSpace(ddlTable.Text) || !ddlTable.Enabled); //Table selected/not applicable
        }

        private void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLoad.Enabled = !string.IsNullOrWhiteSpace(ddlDatabases.Text) && //Database selected
                             testedconnection && //Connection works
                             (!string.IsNullOrWhiteSpace(ddlTable.Text) || !ddlTable.Enabled); //Table selected/not applicable
        }
        #endregion

        private void SaveSettings()
        {
            Properties.Settings.Default["Host"] = txtHost.Text;
            Properties.Settings.Default["Port"] = txtPort.Text;
            Properties.Settings.Default["User"] = txtUser.Text;
            Properties.Settings.Default["Password"] = txtPass.Text;
            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            txtHost.Text = Properties.Settings.Default["Host"].ToString();
            txtPort.Text = Properties.Settings.Default["Port"].ToString();
            txtUser.Text = Properties.Settings.Default["User"].ToString();
            txtPass.Text = Properties.Settings.Default["Password"].ToString();
        }

    }
}
