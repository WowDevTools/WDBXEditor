using WDBXEditor.Reader;
using WDBXEditor.Storage;
using WDBXEditor.Archives.MPQ;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static WDBXEditor.Common.Constants;
using static WDBXEditor.Forms.InputBox;
using System.Threading.Tasks;
using WDBXEditor.Forms;
using WDBXEditor.Common;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.Script.Serialization;
using WDBXEditor.Reader.FileTypes;

namespace WDBXEditor
{
	public partial class Main : Form
	{
		protected DBEntry LoadedEntry;

		private BindingSource _bindingsource = new BindingSource();
		private FileSystemWatcher watcher = new FileSystemWatcher();

		private bool IsLoaded => (LoadedEntry != null && _bindingsource.DataSource != null);
		private DBEntry GetEntry() => Database.Entries.FirstOrDefault(x => x.FileName == txtCurEntry.Text && x.BuildName == txtCurDefinition.Text);

		public Main()
		{
			InitializeComponent();

			_bindingsource.DataSource = null;
			advancedDataGridView.DataSource = _bindingsource;
		}

		public Main(string[] filenames)
		{
			InitializeComponent();

			_bindingsource.DataSource = null;
			advancedDataGridView.DataSource = _bindingsource;

			Parallel.For(0, filenames.Length, f => InstanceManager.AutoRun.Enqueue(filenames[f]));
		}

		private void Main_Load(object sender, EventArgs e)
		{
#if DEBUG
			wdb5ParserToolStripMenuItem.Visible = true;
#endif

			//Create temp directory
			if (!Directory.Exists(TEMP_FOLDER))
				Directory.CreateDirectory(TEMP_FOLDER);

			//Set open dialog filters
			openFileDialog.Filter = string.Join("|", SupportedFileTypes.Select(x => $"{x.Key} ({x.Value})|{x.Value}"));

			//Allow keyboard shortcuts
			Parallel.ForEach(this.Controls.Cast<Control>(), c => c.KeyDown += new KeyEventHandler(KeyDownEvent));

			//Load definitions + Start FileWatcher
			Task.Run(Database.LoadDefinitions)
				.ContinueWith(x =>
				{
					// Check for Update, enable watcher after completion
					Task.Run(UpdateManager.CheckForUpdate).ContinueWith(y => Watcher(), TaskScheduler.FromCurrentSynchronizationContext());

					// Run preloaded files
					AutoRun();
				}, 
				TaskScheduler.FromCurrentSynchronizationContext());


			//Setup Single Instance Delegate
			InstanceManager.AutoRunAdded += delegate
			{
				this.Invoke((MethodInvoker)delegate
				{
					InstanceManager.FlashWindow(this);
					AutoRun();
				});
			};

			//Load ColumnAutoSizeMode dropdown
			LoadColumnSizeDropdown();

			//Load recent files
			LoadRecentList();

			this.Text = $"WDBX Editor ({VERSION})";
		}

		private void Main_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (Database.Entries.Count(x => x.Changed) > 0)
				if (MessageBox.Show("You have unsaved changes. Do you wish to exit?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.No)
					e.Cancel = true;

			if (!e.Cancel)
			{
				try { Directory.Delete(TEMP_FOLDER, true); } catch { }

				ProgressBarHandle(false, "", false);
				InstanceManager.Stop();
				watcher.EnableRaisingEvents = false;
				FormHandler.Close();
			}
		}

		private void SetSource(DBEntry dt, bool resetcolumns = true)
		{
			//Hotfix file selector
			if (dt?.Header.IsTypeOf<HTFX>() == true && lbFiles.Items.Count > 1)
			{
				if (new LoadHotfix().ShowDialog(this) != DialogResult.OK)
					return;
			}

			advancedDataGridView.RowHeadersVisible = false;
			advancedDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			advancedDataGridView.ColumnHeadersVisible = false;
			advancedDataGridView.SuspendLayout(); //Performance

			if (_bindingsource.IsSorted)
				_bindingsource.RemoveSort(); //Remove Sort

			if (!string.IsNullOrWhiteSpace(_bindingsource.Filter))
				_bindingsource.RemoveFilter(); //Remove Filter

			advancedDataGridView.Columns.Clear();
			_bindingsource.DataSource = null;
			_bindingsource.Clear();

			if (dt != null)
			{
				//if (LoadedEntry != null && LoadedEntry != dt)
				//    Task.Run(() => LoadedEntry?.Detach());
				//dt.Attach();

				this.Tag = dt.Tag;
				this.Text = $"WDBX Editor ({VERSION}) - {dt.FileName} {dt.BuildName}";
				LoadedEntry = dt; //Set current table

				_bindingsource.DataSource = dt.Data; //Change dataset
				_bindingsource.ResetBindings(true);

				columnFilter.Reset(dt.Data.Columns, resetcolumns); //Reset column filter
				advancedDataGridView.Columns[LoadedEntry.Key].ReadOnly = true; //Set primary key as readonly
				advancedDataGridView.ClearSelection();
				advancedDataGridView.CurrentCell = advancedDataGridView.Rows[0].Cells[0];

				txtStats.Text = $"{LoadedEntry.Data.Columns.Count} fields, {LoadedEntry.Data.Rows.Count} rows";
				wotLKItemFixToolStripMenuItem.Enabled = LoadedEntry.IsFileOf("Item", Expansion.WotLK); //Control WotLK Item Fix
				colourPickerToolStripMenuItem.Enabled = (LoadedEntry.IsFileOf("LightIntBand") || LoadedEntry.IsFileOf("LightData")); //Colour picker
				if (!colourPickerToolStripMenuItem.Enabled)
					FormHandler.Close<ColourConverter>(); //Close if different DB file
			}
			else
			{
				this.Text = $"WDBX Editor ({VERSION})";
				this.Tag = string.Empty;
				LoadedEntry = null;

				txtStats.Text = txtCurEntry.Text = txtCurDefinition.Text = "";
				columnFilter.Reset(null, true);
				FormHandler.Close();
			}

			advancedDataGridView.ClearCopyData();
			advancedDataGridView.ClearChanges();
			pasteToolStripMenuItem.Enabled = false;
			undoToolStripMenuItem.Enabled = false;
			redoToolStripMenuItem.Enabled = false;
		}

		#region Data Grid

		private void advancedDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			//Reset speed improvements
			advancedDataGridView.RowHeadersVisible = true;
			advancedDataGridView.ColumnHeadersVisible = true;
			advancedDataGridView.ResumeLayout(false);

			if (cbColumnMode.SelectedItem != null)
				advancedDataGridView.AutoSizeColumnsMode = ((KeyValuePair<string, DataGridViewAutoSizeColumnsMode>)cbColumnMode.SelectedItem).Value;

			ProgressBarHandle(false);
		}

		private void advancedDataGridView_FilterStringChanged(object sender, EventArgs e)
		{
			_bindingsource.Filter = advancedDataGridView.FilterString;
		}

		private void advancedDataGridView_SortStringChanged(object sender, EventArgs e)
		{
			_bindingsource.Sort = advancedDataGridView.SortString;
		}

		private void advancedDataGridView_CurrentCellChanged(object sender, EventArgs e)
		{
			var cell = advancedDataGridView.CurrentCell;
			txtCurrentCell.Text = $"X: {cell?.ColumnIndex ?? 0}, Y: {cell?.RowIndex ?? 0}";
		}

		private void advancedDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			if (IsLoaded && LoadedEntry.Data != null)
				txtStats.Text = $"{LoadedEntry.Data.Columns.Count} fields, {LoadedEntry.Data.Rows.Count} rows";
		}

		private void advancedDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			if (IsLoaded && LoadedEntry.Data != null)
				txtStats.Text = $"{LoadedEntry.Data.Columns.Count} fields, {LoadedEntry.Data.Rows.Count} rows";
		}

		private void columnFilter_ItemCheckChanged(object sender, ItemCheckEventArgs e)
		{
			advancedDataGridView.SetVisible(e.Index, (e.NewValue == CheckState.Checked));
		}

		private void columnFilter_HideEmptyPressed(object sender, EventArgs e)
		{
			if (!IsLoaded)
				return;

			foreach (var c in advancedDataGridView.GetEmptyColumns())
				columnFilter.SetItemChecked(c, false);
		}

		private void cbColumnMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			advancedDataGridView.AutoSizeColumnsMode = ((KeyValuePair<string, DataGridViewAutoSizeColumnsMode>)cbColumnMode.SelectedItem).Value;
		}

		private void advancedDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!LoadedEntry.Changed)
			{
				LoadedEntry.Changed = true; //Flag changed datasets
				UpdateListBox();
			}
		}

		private void advancedDataGridView_UndoRedoChanged(object sender, EventArgs e)
		{
			undoToolStripMenuItem.Enabled = advancedDataGridView.CanUndo;
			redoToolStripMenuItem.Enabled = advancedDataGridView.CanRedo;
		}

		private void advancedDataGridView_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		private void advancedDataGridView_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string file in files)
				if (Regex.IsMatch(file, Constants.FileRegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase))
					InstanceManager.AutoRun.Enqueue(file);

			AutoRun();
		}
		#endregion

		#region Data Grid Context
		/// <summary>
		/// Controls the visible context menu (if any) base on the mouse click and element
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void advancedDataGridView_MouseDown(object sender, MouseEventArgs e)
		{
			DataGridView.HitTestInfo info = advancedDataGridView.HitTest(e.X, e.Y);
			if (e.Button == MouseButtons.Right && (info.Type == DataGridViewHitTestType.RowHeader || info.Type == DataGridViewHitTestType.Cell))
			{
				if (info.RowIndex >= 0 && info.ColumnIndex >= 0 && advancedDataGridView.Rows[info.RowIndex].Cells[info.ColumnIndex].IsInEditMode)
					return;

				if (info.ColumnIndex == -1)
				{
					advancedDataGridView.ClearSelection();
					advancedDataGridView.SelectRow(info.RowIndex);
					viewInEditorToolStripMenuItem.Enabled = false;
				}
				else
				{
					contextMenuStrip.Tag = advancedDataGridView.Rows[info.RowIndex].Cells[info.ColumnIndex]; //Store current cell
					advancedDataGridView.CurrentCell = (DataGridViewCell)contextMenuStrip.Tag;
					viewInEditorToolStripMenuItem.Enabled = true;
				}

				contextMenuStrip.Show(Cursor.Position);
			}
			else if (contextMenuStrip.Visible)
			{
				contextMenuStrip.Tag = null;
				contextMenuStrip.Hide();
			}
		}

		/// <summary>
		/// Copies the data of the selected row
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			advancedDataGridView.SetCopyData();
			pasteToolStripMenuItem.Enabled = true;
		}

		/// <summary>
		/// Pastes the previously copied row data except the Id
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataRowView row;
			if (advancedDataGridView.SelectedRows.Count > 0)
				row = ((DataRowView)advancedDataGridView.CurrentRow.DataBoundItem);
			else if (advancedDataGridView.SelectedCells.Count > 0)
				row = ((DataRowView)advancedDataGridView.CurrentCell.OwningRow.DataBoundItem);
			else
				return;

			if (row?.Row != null)
			{
				advancedDataGridView.PasteCopyData(row.Row); //Update all fields
			}
			else
			{
				//Force new blank row
				_bindingsource.EndEdit();
				advancedDataGridView.NotifyCurrentCellDirty(true);
				advancedDataGridView.EndEdit();
				advancedDataGridView.NotifyCurrentCellDirty(false);

				//Update new row's data
				row = ((DataRowView)advancedDataGridView.CurrentRow.DataBoundItem);
				if (row?.Row != null)
					advancedDataGridView.PasteCopyData(row.Row); //Update all fields

				if (!LoadedEntry.Changed)
				{
					LoadedEntry.Changed = true;
					UpdateListBox();
				}
			}
		}

		private void gotoIdToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GotoLine();
		}

		private void insertLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			InsertLine();
		}

		private void clearLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DefaultRowValues();

			if (!LoadedEntry.Changed)
			{
				LoadedEntry.Changed = true;
				UpdateListBox();
			}
		}

		private void deleteLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;

			if (advancedDataGridView.SelectedRows.Count == 0 && advancedDataGridView.SelectedCells.Count == 0)
				return;

			if (advancedDataGridView.SelectedRows.Count == 0)
				advancedDataGridView.SelectRow(advancedDataGridView.CurrentCell.OwningRow.Index);

			SendKeys.Send("{delete}");
			if (!LoadedEntry.Changed)
			{
				LoadedEntry.Changed = true;
				UpdateListBox();
			}
		}

		private void viewInEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DataGridViewCell cell = (contextMenuStrip.Tag as DataGridViewCell);
			if (cell != null)
			{
				advancedDataGridView.CurrentCell = cell; //Select cell

				using (var form = new TextEditor())
				{
					form.CellValue = cell.Value.ToString();
					if (form.ShowDialog() == DialogResult.OK)
					{
						advancedDataGridView.BeginEdit(false);
						cell.Value = form.CellValue;
						advancedDataGridView.EndEdit();
					}
				}
			}
		}
		#endregion

		#region Menu Items

		private void loadFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				using (var loaddefs = new LoadDefinition())
				{
					loaddefs.Files = openFileDialog.FileNames;
					if (loaddefs.ShowDialog(this) != DialogResult.OK)
						return;
				}

				ProgressBarHandle(true, "Loading files...");
				Task.Run(() => Database.LoadFiles(openFileDialog.FileNames))
				.ContinueWith(x =>
				{
					if (x.Result.Count > 0)
						new ErrorReport(x.Result).ShowDialog(this);

					UpdateRecentList(openFileDialog.FileNames);
					LoadFiles(openFileDialog.FileNames);
					ProgressBarHandle(false);

				}, TaskScheduler.FromCurrentSynchronizationContext());
			}

		}

		private void loadRecentFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] files = new string[] { ((ToolStripMenuItem)sender).Tag.ToString() };

			using (var loaddefs = new LoadDefinition())
			{
				loaddefs.Files = files;
				if (loaddefs.ShowDialog(this) != DialogResult.OK)
					return;
			}

			ProgressBarHandle(true, "Loading files...");
			Task.Run(() => Database.LoadFiles(files))
			.ContinueWith(x =>
			{
				if (x.Result.Count > 0)
					new ErrorReport(x.Result).ShowDialog(this);

				UpdateRecentList(files);
				LoadFiles(files);
				ProgressBarHandle(false);

			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		/// <summary>
		/// Allows the user to select DB* files from an MPQ archive
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void openFromMPQToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var mpq = new LoadMPQ())
			{
				if (mpq.ShowDialog(this) == DialogResult.OK)
				{
					using (var loaddefs = new LoadDefinition())
					{
						loaddefs.Files = mpq.Streams.Keys;
						if (loaddefs.ShowDialog(this) != DialogResult.OK)
							return;
					}

					ProgressBarHandle(true, "Loading files...");
					Task.Run(() => Database.LoadFiles(mpq.Streams))
					.ContinueWith(x =>
					{
						if (x.Result.Count > 0)
							new ErrorReport(x.Result).ShowDialog(this);

						LoadFiles(mpq.Streams.Keys);
						ProgressBarHandle(false);
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		private void openFromCASCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var mpq = new LoadMPQ())
			{
				mpq.IsMPQ = false;

				if (mpq.ShowDialog(this) == DialogResult.OK)
				{
					using (var loaddefs = new LoadDefinition())
					{
						loaddefs.Files = mpq.FileNames.Values;
						if (loaddefs.Files.Count() == 0)
							loaddefs.Files = mpq.Streams.Keys;

						if (loaddefs.ShowDialog(this) != DialogResult.OK)
							return;
					}

					ProgressBarHandle(true, "Loading files...");
					Task.Run(() => Database.LoadFiles(mpq.Streams))
					.ContinueWith(x =>
					{
						if (x.Result.Count > 0)
							new ErrorReport(x.Result).ShowDialog(this);

						LoadFiles(mpq.Streams.Keys);
						ProgressBarHandle(false);
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;
			SaveFile(false);
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;
			SaveFile();
		}

		private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveAll();
		}

		private void editDefinitionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new EditDefinition().ShowDialog(this);
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Find();
		}

		private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Replace();
		}

		private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Reload();
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseFile();
		}

		private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseAllFiles();
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Undo();
		}

		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Redo();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new About().ShowDialog(this);
		}

		private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Help.ShowHelp(this, Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Help.chm"));
		}

		private void insertToolStripMenuItem_Click(object sender, EventArgs e)
		{
			InsertLine();
		}

		private void newLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewLine();
		}

		private void playerLocationRecorderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormHandler.Show<PlayerLocation>();
		}

		private void colourPickerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormHandler.Show<ColourConverter>();
		}

		#endregion

		#region Export Menu Items
		/// <summary>
		/// Exports the current dataset directly into sql
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toSQLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;

			using (var sql = new LoadSQL() { Entry = LoadedEntry, ConnectionOnly = true })
			{
				if (sql.ShowDialog(this) == DialogResult.OK)
				{
					ProgressBarHandle(true, "Exporting to SQL...");
					Task.Factory.StartNew(() => { LoadedEntry.ToSQLTable(sql.ConnectionString); })
					.ContinueWith(x =>
					{
						if (x.IsFaulted)
							MessageBox.Show("An error occured exporting to SQL.");
						else
							MessageBox.Show("Sucessfully exported to SQL.");

						ProgressBarHandle(false);
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		/// <summary>
		/// Exports the current dataset to a MySQL SQL file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toSQLFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;

			using (var sfd = new SaveFileDialog() { FileName = LoadedEntry.TableStructure.Name + ".sql", Filter = "SQL Files|*.sql" })
			{
				if (sfd.ShowDialog(this) == DialogResult.OK)
				{
					ProgressBarHandle(true, "Exporting to SQL file...");
					Task.Factory.StartNew(() =>
					{
						using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
						{
							string sql = LoadedEntry.ToSQL();
							byte[] data = Encoding.UTF8.GetBytes(sql);
							fs.Write(data, 0, data.Length);
						}
					})
					.ContinueWith(x =>
					{
						ProgressBarHandle(false);

						if (x.IsFaulted)
							MessageBox.Show($"Error generating SQL file {x.Exception.Message}");
						else
							MessageBox.Show($"File successfully exported to {sfd.FileName}");

					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		/// <summary>
		/// Exports the current dataset to a CSV/Txt file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toCSVToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;

			using (var sfd = new SaveFileDialog())
			{
				sfd.FileName = LoadedEntry.TableStructure.Name + ".csv";
				sfd.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt";

				if (sfd.ShowDialog(this) == DialogResult.OK)
				{
					ProgressBarHandle(true, "Exporting to CSV...");
					Task.Factory.StartNew(() =>
					{
						using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
						{
							string sql = LoadedEntry.ToCSV();
							byte[] data = Encoding.UTF8.GetBytes(sql);
							fs.Write(data, 0, data.Length);
						}
					})
					.ContinueWith(x =>
					{
						ProgressBarHandle(false);

						if (x.IsFaulted)
							MessageBox.Show($"Error generating CSV file {x.Exception.Message}");
						else
							MessageBox.Show($"File successfully exported to {sfd.FileName}");

					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		/// <summary>
		/// Exports the current dataset to a MPQ file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toMPQToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;

			//Get the correct save settings
			using (var sfd = new SaveFileDialog())
			{
				sfd.InitialDirectory = Path.GetDirectoryName(LoadedEntry.FilePath);
				sfd.OverwritePrompt = false;
				sfd.CheckFileExists = false;

				//Set the correct filter
				switch (Path.GetExtension(LoadedEntry.FilePath).ToLower().TrimStart('.'))
				{
					case "dbc":
					case "db2":
						sfd.FileName = LoadedEntry.TableStructure.Name + ".mpq";
						sfd.Filter = "MPQ Files|*.mpq";
						break;
					default:
						MessageBox.Show("Only DBC and DB2 files can be saved to MPQ.");
						return;
				}

				if (sfd.ShowDialog(this) == DialogResult.OK)
					LoadedEntry.ToMPQ(sfd.FileName);
			}
		}

		/// <summary>
		/// Exports the current dataset to a JSON file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toJSONToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;

			using (var sfd = new SaveFileDialog())
			{
				sfd.FileName = LoadedEntry.TableStructure.Name + ".json";
				sfd.Filter = "JSON files (*.json)|*.json|Text files (*.txt)|*.txt";

				if (sfd.ShowDialog(this) == DialogResult.OK)
				{
					ProgressBarHandle(true, "Exporting to JSON...");
					Task.Factory.StartNew(() =>
					{
						using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
						{
							string sql = LoadedEntry.ToJSON();
							byte[] data = Encoding.UTF8.GetBytes(sql);
							fs.Write(data, 0, data.Length);
						}
					})
					.ContinueWith(x =>
					{
						ProgressBarHandle(false);

						if (x.IsFaulted)
							MessageBox.Show($"Error generating JSON file {x.Exception.Message}");
						else
							MessageBox.Show($"File successfully exported to {sfd.FileName}");

					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		#endregion

		#region Import Menu Items
		/// <summary>
		/// Import data rows from a CSV/Txt File.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fromCSVToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;

			using (var loadCsv = new LoadCSV() { Entry = LoadedEntry })
			{
				switch (loadCsv.ShowDialog(this))
				{
					case DialogResult.OK:
						SetSource(GetEntry(), false);
						advancedDataGridView.CacheData();
						MessageBox.Show("CSV import succeeded.");
						break;
					case DialogResult.Abort:
						ProgressBarHandle(false);
						if (!string.IsNullOrWhiteSpace(loadCsv.ErrorMessage))
							MessageBox.Show("CSV import failed: " + loadCsv.ErrorMessage);
						else
							MessageBox.Show("CSV import failed due to incorrect file format.");
						break;
				}

				ProgressBarHandle(false);
			}
		}

		/// <summary>
		/// Import data rows from SQL database
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fromSQLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!IsLoaded) return;

			using (var importSql = new LoadSQL() { Entry = LoadedEntry })
			{
				switch (importSql.ShowDialog(this))
				{
					case DialogResult.OK:
						SetSource(GetEntry(), false);
						advancedDataGridView.CacheData();
						MessageBox.Show("SQL import succeeded.");
						break;
					case DialogResult.Abort:
						if (!string.IsNullOrWhiteSpace(importSql.ErrorMessage))
							MessageBox.Show(importSql.ErrorMessage);
						else
							MessageBox.Show("SQL import failed due to incorrect file format.");
						break;
				}

				ProgressBarHandle(false);
			}
		}

		#endregion

		#region Tool Menu Items
		/// <summary>
		/// Loads Item.dbc rows from item_template database table
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void wotLKItemFixToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var itemfix = new WotLKItemFix())
			{
				itemfix.Entry = LoadedEntry;
				if (itemfix.ShowDialog(this) == DialogResult.OK)
					SetSource(LoadedEntry);
			}
		}

		private void legionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new LegionParser().ShowDialog(this);
		}
		#endregion

		#region File ListView
		private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			DBEntry selection = (DBEntry)((DataRowView)lbFiles.SelectedItem)["Key"];
			if (LoadedEntry == selection)
				CloseFile();
			else
			{
				Database.Entries.Remove(selection);
				Database.Entries.TrimExcess();
				UpdateListBox();
			}
		}

		private void editToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			DBEntry selection = (DBEntry)((DataRowView)lbFiles.SelectedItem)["Key"];
			if (LoadedEntry != selection)
				SetSource(selection);
		}

		/// <summary>
		/// Show the context menu at the right position on right click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lbFiles_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				int index = lbFiles.IndexFromPoint(e.Location);
				if (index != ListBox.NoMatches)
				{
					lbFiles.SelectedIndex = index;
					filecontextMenuStrip.Show(Cursor.Position);
				}
			}
		}

		/// <summary>
		/// Changes the current dataset
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lbFiles_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int index = lbFiles.IndexFromPoint(e.Location);
			if (index != ListBox.NoMatches)
			{
				DBEntry entry = (DBEntry)((DataRowView)lbFiles.Items[index])["Key"];
				txtCurEntry.Text = entry.FileName;
				txtCurDefinition.Text = entry.BuildName;

				SetSource(GetEntry());
			}
		}
		#endregion

		#region Command Actions
		private void LoadFiles(IEnumerable<string> fileNames)
		{
			//Update the DB list, remove old and add new
			UpdateListBox();

			if (lbFiles.Items.Count == 0)
				return;

			//Refresh the data if the file was reloaded
			if (LoadedEntry != null && fileNames.Any(x => x.Equals(LoadedEntry.FileName, IGNORECASE)))
			{
				var entry = (DBEntry)lbFiles.SelectedValue;
				txtCurEntry.Text = entry.FileName;
				txtCurDefinition.Text = entry.BuildName;
				txtStats.Text = $"{entry.Data.Columns.Count} fields, {entry.Data.Rows.Count} rows";

				SetSource(GetEntry());
			}

			//Current file is no longer open
			if (GetEntry() == null)
			{
				LoadedEntry = null;
				SetSource(null);
			}

			//Load the first item if no data exists
			if (LoadedEntry == null && lbFiles.Items.Count > 0)
			{
				lbFiles.SetSelected(0, true);

				var entry = (DBEntry)lbFiles.SelectedValue;
				txtCurEntry.Text = entry.FileName;
				txtCurDefinition.Text = entry.BuildName;
				txtStats.Text = $"{entry.Data.Columns.Count} fields, {entry.Data.Rows.Count} rows";

				SetSource(GetEntry());
			}

			//Preset options
			if (LoadedEntry != null)
				txtCurDefinition.Text = LoadedEntry.BuildName;
		}

		private void SaveFile(bool saveas = true)
		{
			if (!IsLoaded) return;
			bool save = !saveas;

			//Get the correct save settings if save as
			if (saveas)
			{
				using (var sfd = new SaveFileDialog())
				{
					sfd.InitialDirectory = Path.GetDirectoryName(LoadedEntry.SavePath);
					sfd.FileName = LoadedEntry.SavePath;

					//Set the correct filter
					string ext = Path.GetExtension(LoadedEntry.FilePath).TrimStart('.');
					switch (ext.ToLower())
					{
						case "dbc":
							sfd.Filter = "DBC Files|*.dbc";
							break;
						case "db2":
							sfd.Filter = "DB2 Files|*.db2";
							break;
						case "adb":
							sfd.Filter = "ADB Files|*.adb";
							break;
						case "wdb":
						case "bin":
							MessageBox.Show($"Saving is not implemented for {ext.ToUpper()} files.");
							return;
					}

					if (sfd.ShowDialog(this) == DialogResult.OK)
					{
						save = true;
						LoadedEntry.SavePath = sfd.FileName;
					}
				}
			}

			//Do the actual save
			if (save)
			{
				ProgressBarHandle(true, "Saving file...");
				Task.Factory.StartNew(() => new DBReader().Write(LoadedEntry, LoadedEntry.SavePath))
				.ContinueWith(x =>
				{
					ProgressBarHandle(false);
					LoadedEntry.Changed = false;
					UpdateListBox();

					if (x.IsFaulted)
						MessageBox.Show($"Error exporting to file {x.Exception.Message}");

				}, TaskScheduler.FromCurrentSynchronizationContext());
			}
		}

		/// <summary>
		/// Provides a SaveFolderDialog to bulk save all open files
		/// </summary>
		private void SaveAll()
		{
			using (var fbd = new FolderBrowserDialog())
			{
				if (fbd.ShowDialog(this) == DialogResult.OK)
				{
					ProgressBarHandle(true, "Saving files...");

					Task.Run(() => Database.SaveFiles(fbd.SelectedPath))
						.ContinueWith(x =>
						{
							if (x.Result.Count > 0)
								new ErrorReport(x.Result).ShowDialog(this);

							ProgressBarHandle(false);
						}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		private void GotoLine()
		{
			if (!IsLoaded) return;

			int id = 0;
			string res = "";
			if (ShowInputDialog("Id:", "Go to Id", 0.ToString(), ref res) == DialogResult.OK)
			{
				if (int.TryParse(res, out id)) //Ensure the result is an integer
				{
					int index = _bindingsource.Find(LoadedEntry.Key, id); //See if the Id exists
					if (index >= 0)
						advancedDataGridView.SelectRow(index);
					else
						MessageBox.Show($"Id {id} doesn't exist.");
				}
				else
					MessageBox.Show($"Invalid Id.");
			}
		}

		private void Find()
		{
			if (IsLoaded)
				FormHandler.Show<FindReplace>(false);
		}

		private void Replace()
		{
			if (IsLoaded)
				FormHandler.Show<FindReplace>(true);
		}

		private void Reload()
		{
			if (!IsLoaded) return;

			ProgressBarHandle(true, "Reloading file...");
			Task.Run(() => Database.LoadFiles(new string[] { LoadedEntry.FilePath }))
			.ContinueWith(x =>
			{
				if (x.Result.Count > 0)
					new ErrorReport(x.Result).ShowDialog(this);

				LoadFiles(openFileDialog.FileNames);
				ProgressBarHandle(false);

			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void CloseFile()
		{
			if (!string.IsNullOrWhiteSpace(_bindingsource.Filter))
				_bindingsource.RemoveFilter();

			if (_bindingsource.IsSorted)
				_bindingsource.RemoveSort();

			if (LoadedEntry != null)
			{
				LoadedEntry.Dispose();
				Database.Entries.Remove(LoadedEntry);
				Database.Entries.TrimExcess();
			}

			SetSource(null);
			UpdateListBox();
		}

		private void CloseAllFiles()
		{
			if (!string.IsNullOrWhiteSpace(_bindingsource.Filter))
				_bindingsource.RemoveFilter();

			if (_bindingsource.IsSorted)
				_bindingsource.RemoveSort();

			for (int i = 0; i < Database.Entries.Count; i++)
				Database.Entries[i].Dispose();

			Database.Entries.Clear();
			Database.Entries.TrimExcess();

			SetSource(null);
			UpdateListBox();
		}

		private void Undo()
		{
			advancedDataGridView.Undo();
		}

		private void Redo()
		{
			advancedDataGridView.Redo();
		}

		private void InsertLine()
		{
			if (!IsLoaded) return;

			string res = "";
			if (ShowInputDialog("Id:", "Id to insert", "1", ref res) == DialogResult.OK)
			{
				int keyIndex = advancedDataGridView.Columns[LoadedEntry.Key].Index;

				if (!int.TryParse(res, out int id) || id < 0 /*|| !advancedDataGridView.ValidValue(keyIndex, id)*/)
				{
					MessageBox.Show($"Invalid Id. Out of range of the column min/max value.");
				}
				else
				{
					int index = _bindingsource.Find(LoadedEntry.Key, id); //See if the Id exists
					if (index < 0)
					{
						index = NewLine();
						advancedDataGridView.Rows[index].Cells[LoadedEntry.Key].Value = id;
						DefaultRowValues(index);

						advancedDataGridView.OnUserAddedRow(advancedDataGridView.Rows[index]);

						if (!LoadedEntry.Changed)
						{
							LoadedEntry.Changed = true;
							UpdateListBox();
						}
					}

					advancedDataGridView.SelectRow(index);
				}
			}
		}

		private void DefaultRowValues(int index = -1)
		{
			if (!IsLoaded)
				return;

			if (advancedDataGridView.SelectedRows.Count == 1)
				index = advancedDataGridView.CurrentRow.Index;
			else if (advancedDataGridView.SelectedCells.Count == 1)
				index = advancedDataGridView.CurrentCell.OwningRow.Index;

			if (index == -1)
				return;

			for (int i = 0; i < advancedDataGridView.Columns.Count; i++)
			{
				if (advancedDataGridView.Columns[i].Name == LoadedEntry.Key)
					continue;

				advancedDataGridView.Rows[index].Cells[i].Value = advancedDataGridView.Columns[i].ValueType.DefaultValue();

				if (!LoadedEntry.Changed)
				{
					LoadedEntry.Changed = true;
					UpdateListBox();
				}
			}
		}

		private int NewLine()
		{
			if (!IsLoaded) return 0;

			var row = LoadedEntry.Data.NewRow();
			LoadedEntry.Data.Rows.Add(row);
			int index = _bindingsource.Find(LoadedEntry.Key, row[LoadedEntry.Key]);
			DefaultRowValues(index);
			advancedDataGridView.SelectRow(index);
			return index;
		}
		#endregion

		#region File Filter
		private void LoadBuilds()
		{
			var tables = lbFiles.Items.Cast<DataRowView>()
							.Select(x => ((DBEntry)x["Key"]).TableStructure)
							.OrderBy(x => x.Build)
							.Select(x => x.BuildText).Distinct();

			cbBuild.Items.Clear();
			cbBuild.Items.Add("");
			cbBuild.Items.AddRange(tables.ToArray());
		}

		private void txtFilter_TextChanged(object sender, EventArgs e)
		{
			((BindingSource)lbFiles.DataSource).Filter = $"([Value] LIKE '%{txtFilter.Text}%') AND [Value] LIKE '%{cbBuild.Text}%'";
		}

		private void cbBuild_SelectedIndexChanged(object sender, EventArgs e)
		{
			((BindingSource)lbFiles.DataSource).Filter = $"([Value] LIKE '%{txtFilter.Text}%') AND [Value] LIKE '%{cbBuild.Text}%'";
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			txtFilter.Text = "";
			cbBuild.Text = "";
		}
		#endregion

		private void LoadColumnSizeDropdown()
		{
			cbColumnMode.Items.Add(new KeyValuePair<string, DataGridViewAutoSizeColumnsMode>("None", DataGridViewAutoSizeColumnsMode.None));
			cbColumnMode.Items.Add(new KeyValuePair<string, DataGridViewAutoSizeColumnsMode>("Column Header", DataGridViewAutoSizeColumnsMode.ColumnHeader));
			cbColumnMode.Items.Add(new KeyValuePair<string, DataGridViewAutoSizeColumnsMode>("Displayed Cells", DataGridViewAutoSizeColumnsMode.DisplayedCells));
			cbColumnMode.Items.Add(new KeyValuePair<string, DataGridViewAutoSizeColumnsMode>("Displayed Cells Except Header", DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader));

			cbColumnMode.ValueMember = "Value";
			cbColumnMode.DisplayMember = "Key";
			cbColumnMode.SelectedIndex = 0;
		}

		private void UpdateListBox()
		{
			//Update the DB list, remove old and add new
			DataTable dt = new DataTable();
			dt.Columns.Add("Key", typeof(DBEntry));
			dt.Columns.Add("Value", typeof(string));

			var entries = Database.Entries.OrderBy(x => x.Build).ThenBy(x => x.FileName);
			foreach (var entry in entries)
				dt.Rows.Add(entry, $"{entry.FileName} - {entry.BuildName}{(entry.Changed ? "*" : "")}");

			lbFiles.BeginUpdate();
			lbFiles.DataSource = new BindingSource(dt, null);

			if (Database.Entries.Count > 0)
			{
				lbFiles.ValueMember = "Key";
				lbFiles.DisplayMember = "Value";
			}
			else
			{
				((BindingSource)lbFiles.DataSource).DataSource = null;
				((BindingSource)lbFiles.DataSource).Clear();
			}

			lbFiles.EndUpdate();

			LoadBuilds();
		}

		private void KeyDownEvent(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.S) //Save
				SaveFile(false);
			else if (e.Control && e.KeyCode == Keys.G) //Goto
				GotoLine();
			else if (e.Control && e.Shift && e.KeyCode == Keys.S) //Save All
				SaveAll();
			else if (e.Control && e.KeyCode == Keys.F) //Find
				Find();
			else if (e.Control && e.KeyCode == Keys.H) //Replace
				Replace();
			else if (e.Control && e.KeyCode == Keys.R) //Reload
				Reload();
			else if (e.Control && e.KeyCode == Keys.W) //Close
				CloseFile();
			else if (e.Control && e.KeyCode == Keys.Z) //Undo
				Undo();
			else if ((e.Control && e.Shift && e.KeyCode == Keys.Z) || (e.Control && e.KeyCode == Keys.Y)) //Redo
				Redo();
			else if (e.Control && e.KeyCode == Keys.N) //Newline
				NewLine();
			else if (e.Control && e.KeyCode == Keys.I) //Insert
				InsertLine();
			else if (e.KeyCode == Keys.F12) //Save As
				SaveFile();
			else if (e.Control && e.Shift && e.KeyCode == Keys.W) //Close All
				CloseAllFiles();
		}

		public void ProgressBarHandle(bool start, string currentTask = "", bool clear = true)
		{
			if (start)
				progressBar.Start();
			else
				progressBar.Stop(clear);

			lblCurrentProcess.Text = currentTask;
			lblCurrentProcess.Visible = !string.IsNullOrWhiteSpace(currentTask) && start;

			menuStrip.Enabled = !start;
			columnFilter.Enabled = !start;
			gbSettings.Enabled = !start;
			gbFilter.Enabled = !start;
			advancedDataGridView.ReadOnly = start;
			advancedDataGridView.Refresh();
		}

		private void AutoRun()
		{
			if (InstanceManager.AutoRun.Any(x => File.Exists(x)))
			{
				//Dequeue all stored files
				IEnumerable<string> filenames = InstanceManager.GetFilesToOpen();

				//See if we can use an existing LoadDefinition
				var loaddef = FormHandler.GetForm<LoadDefinition>();
				if (loaddef != null)
				{
					loaddef.UpdateFiles(filenames);
					return;
				}

				//Load definition picker
				using (var loaddefs = new LoadDefinition())
				{
					loaddefs.Files = filenames;
					if (loaddefs.ShowDialog(this) != DialogResult.OK)
						return;
					else
						filenames = loaddefs.Files;
				}

				//Load the files
				ProgressBarHandle(true, "Loading files...");
				Task.Run(() => Database.LoadFiles(filenames))
				.ContinueWith(x =>
				{
					if (x.Result.Count > 0)
						new ErrorReport(x.Result).ShowDialog(this);

					LoadFiles(filenames);
					ProgressBarHandle(false);

				}, TaskScheduler.FromCurrentSynchronizationContext());
			}
		}

		private void Watcher()
		{
			watcher = new FileSystemWatcher
			{
				Path = Path.GetDirectoryName(DEFINITION_DIR),
				NotifyFilter = NotifyFilters.LastWrite,
				Filter = "*.xml",
				EnableRaisingEvents = true
			};
			watcher.Changed += delegate { Task.Run(() => Database.LoadDefinitions()); };
		}

		private void LoadRecentList()
		{
			recentToolStripMenuItem.DropDownItems.Clear();

			// create
			if (Properties.Settings.Default.RecentFiles == null)
			{
				Properties.Settings.Default.RecentFiles = new System.Collections.Specialized.StringCollection();
				Properties.Settings.Default.Save();
				recentToolStripMenuItem.Visible = false;
				return;
			}

			recentToolStripMenuItem.Visible = Properties.Settings.Default.RecentFiles.Count > 0;

			foreach (var recent in Properties.Settings.Default.RecentFiles)
			{
				if (!File.Exists(recent))
					continue;

				ToolStripMenuItem menuItem = new ToolStripMenuItem(recent, null, loadRecentFilesToolStripMenuItem_Click)
				{
					Tag = recent,
					DisplayStyle = ToolStripItemDisplayStyle.Text
				};
				recentToolStripMenuItem.DropDownItems.Add(menuItem);
			}
		}

		private void UpdateRecentList(string[] files)
		{
			// create list of all recent files. newest first
			string[] recentTmp = files;
			Array.Resize(ref recentTmp, Properties.Settings.Default.RecentFiles.Count + recentTmp.Length);
			Properties.Settings.Default.RecentFiles.CopyTo(recentTmp, files.Length);

			// distinct and remove non-existant, take first 10
			var recentFiles = recentTmp.Distinct().Where(x => File.Exists(x)).Take(10);

			// take 10 most recent files
			Properties.Settings.Default.RecentFiles.Clear();
			Properties.Settings.Default.RecentFiles.AddRange(recentFiles.ToArray());

			LoadRecentList();
		}
	}
}
