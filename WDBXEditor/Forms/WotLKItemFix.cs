using WDBXEditor.Storage;
using MySqlConnector;
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

namespace WDBXEditor
{
	public partial class WotLKItemFix : Form
	{
		public string ConnectionString => $"Server={txtHost.Text};Port={txtPort.Text};Database={ddlDatabases.Text};Uid={txtUser.Text};Pwd={txtPass.Text};";
		public DBEntry Entry { get; set; }

		private bool validConn = false;


		public WotLKItemFix()
		{
			InitializeComponent();
		}

		private void WotLKItemFix_Load(object sender, EventArgs e)
		{
			LoadSettings();
			PopulateTable();
		}

		#region Buttons
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			ddlDatabases.Items.Clear();
			validConn = false;
			ddlDatabases.Enabled = false;

			try
			{
				string sql = "SHOW DATABASES;";
				using (MySqlConnection connection = new MySqlConnection(ConnectionString))
				{
					connection.Open();
					MySqlCommand command = new MySqlCommand(sql, connection);
					using (var rdr = command.ExecuteReader())
					{
						ddlDatabases.Items.Add("");

						while (rdr.Read())
							ddlDatabases.Items.Add(rdr[0].ToString());

						validConn = true;
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
			string columns = string.Join(",", dgvSchema.Rows.Cast<DataGridViewRow>().Select(x => $"`{x.Cells["Column"].Value.ToString()}` AS `{x.Cells["Field"].Value.ToString()}` "));
			if (columns.IndexOf("``") >= 0)
			{
				MessageBox.Show("Some columns are unmapped.");
				return;
			}

			string ErrorMessage = string.Empty;
			Entry.ImportSQL(UpdateMode.Update, ConnectionString, ddlTable.Text, out ErrorMessage, columns);
			if (!string.IsNullOrWhiteSpace(ErrorMessage))
			{
				MessageBox.Show(ErrorMessage);
				this.DialogResult = DialogResult.Abort;
			}
			else
				this.DialogResult = DialogResult.OK;

			this.Close();
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
					string sql = $"USE {ddlDatabases.Text}; SHOW TABLES;";
					using (MySqlConnection connection = new MySqlConnection(ConnectionString))
					{
						connection.Open();
						MySqlCommand command = new MySqlCommand(sql, connection);
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
							  validConn && //Connection works
							  (!string.IsNullOrWhiteSpace(ddlTable.Text) || !ddlTable.Enabled); //Table selected/not applicable
		}

		private void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnLoad.Enabled = !string.IsNullOrWhiteSpace(ddlDatabases.Text) && //Database selected
							 validConn && //Connection works
							 (!string.IsNullOrWhiteSpace(ddlTable.Text) || !ddlTable.Enabled); //Table selected/not applicable
		}

		private void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
		{
			PopulateTable();
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

		private void PopulateTable()
		{
			DataTable schema = new DataTable();
			schema.Columns.Add("Field");
			schema.Columns.Add("Column");

			var fields = Database.Definitions.Tables.First(x => x.Name.ToLower() == "item" && x.Build == (int)ExpansionFinalBuild.WotLK).Fields;

			if (ddlTemplate.Text == "Trinity")
			{
				for (int i = 0; i < fields.Count; i++)
					schema.Rows.Add(fields[i].Name, TrinityLookup[i]);
			}
			else if (ddlTemplate.Text.IndexOf("Mangos") > -1)
			{
				for (int i = 0; i < fields.Count; i++)
					schema.Rows.Add(fields[i].Name, MangosLookup[i]);
			}
			else
			{
				for (int i = 0; i < fields.Count; i++)
					schema.Rows.Add(fields[i].Name, "");
			}

			dgvSchema.DataSource = schema;
			dgvSchema.Columns[0].ReadOnly = true;
			dgvSchema.Columns[0].HeaderText = "DBC Field";
			dgvSchema.Columns[1].HeaderText = "Database Column";
		}

		private readonly string[] MangosLookup = new[] { "Entry", "Class", "SubClass", "Unk0", "Material", "DisplayId", "InventoryType", "Sheath" };

		private readonly string[] TrinityLookup = new[] { "Entry", "Class", "SubClass", "SoundOverrideSubclass", "Material", "DisplayId", "InventoryType", "Sheath" };

	}
}
