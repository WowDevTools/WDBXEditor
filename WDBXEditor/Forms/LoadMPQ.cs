using System;
using System.Windows.Forms;
using WDBXEditor.Archives.MPQ;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static WDBXEditor.Common.Constants;
using WDBXEditor.Archives.Misc;
using WDBXEditor.Archives.CASC.Handlers;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace WDBXEditor
{
	public partial class LoadMPQ : Form
	{
		public ConcurrentDictionary<string, string> FileNames = new ConcurrentDictionary<string, string>();
		public ConcurrentDictionary<string, MemoryStream> Streams = new ConcurrentDictionary<string, MemoryStream>();
		public bool IsMPQ { get; set; } = true;

		private string filePath;
		private readonly string[] fileExtensions = new[] { ".dbc", ".db2" };

		public LoadMPQ()
		{
			InitializeComponent();
		}

		#region Button Events
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			if (!IsMPQ)
			{
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
				{
					btnParse.Enabled = false;
					filePath = folderBrowserDialog.SelectedPath;

					Properties.Settings.Default.RecentCASC = filePath;
					Properties.Settings.Default.Save();

					LoadStart();
					Task.Run(LoadCASCDBFiles).ContinueWith(x => LoadComplete(), TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
			else
			{
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					btnParse.Enabled = false;
					filePath = openFileDialog.FileName;

					Properties.Settings.Default.RecentMPQ = filePath;
					Properties.Settings.Default.Save();

					LoadStart();
					Task.Run(LoadMPQDBFiles).ContinueWith(x => LoadComplete(), TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			FileNames.Clear();

			ExtractDBFiles();

			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnParse_Click(object sender, EventArgs e)
		{
			filePath = txtFilePath.Text;

			LoadStart();

			if (IsMPQ)
			{
				Task.Run(LoadMPQDBFiles).ContinueWith(x => LoadComplete(), TaskScheduler.FromCurrentSynchronizationContext());
			}
			else
			{
				Task.Run(LoadCASCDBFiles).ContinueWith(x => LoadComplete(), TaskScheduler.FromCurrentSynchronizationContext());
			}			
		}
		#endregion

		#region DB Events
		private async Task LoadMPQDBFiles()
		{
			lstFiles.DataSource = null;
			FileNames.Clear();

			await Task.Factory.StartNew(() =>
			{
				using (MpqArchive archive = new MpqArchive(filePath, FileAccess.Read))
				{
					string line = string.Empty;
					using (MpqFileStream file = archive.OpenFile("(listfile)"))
					using (StreamReader sr = new StreamReader(file))
					{
						while ((line = sr.ReadLine()) != null)
						{
							if (fileExtensions.Contains(Path.GetExtension(line).ToLower()))
							{
								FileNames.TryAdd(line, Path.GetFileName(line));

								var ms = new MemoryStream();
								archive.OpenFile(line).CopyTo(ms);
								Streams.TryAdd(line, ms);
							}
						}
					}
				}
			});
		}

		private void ExtractDBFiles()
		{
			ConcurrentDictionary<string, MemoryStream> _streams = new ConcurrentDictionary<string, MemoryStream>();
			var selected = lstFiles.SelectedItems.Cast<KeyValuePair<string, string>>();
			Parallel.ForEach(selected, dbfile => _streams.TryAdd(dbfile.Key, Streams[dbfile.Key]));
			Streams = _streams;
		}

		private async Task LoadCASCDBFiles()
		{
			lstFiles.DataSource = null;
			FileNames.Clear();

			try
			{
				await Task.Factory.StartNew(() =>
				{
					using (var casc = new CASCHandler(filePath))
					{

						Parallel.ForEach(ClientDBFileNames, file =>
						{
							var stream = casc.ReadFile(file);
							if (stream != null)
							{
								FileNames.TryAdd(file, Path.GetFileName(file));
								Streams.TryAdd(file, stream);
							}
						});
					}
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		#endregion

		#region FormEvents
		private void LoadMPQ_Load(object sender, EventArgs e)
		{
			if (IsMPQ)
			{
				if (File.Exists(Properties.Settings.Default.RecentMPQ))
				{
					openFileDialog.FileName = Properties.Settings.Default.RecentMPQ;
					txtFilePath.Text = Properties.Settings.Default.RecentMPQ;
					btnParse.Enabled = true;
				}					
			}
			else
			{
				if (Directory.Exists(Properties.Settings.Default.RecentCASC))
				{
					folderBrowserDialog.SelectedPath = Properties.Settings.Default.RecentCASC;
					txtFilePath.Text = Properties.Settings.Default.RecentCASC;
					btnParse.Enabled = true;
				}

				this.Text = "Load From CASC";
			}
		}

		private void LoadStart()
		{
			btnBrowse.Enabled = false;
			btnLoad.Enabled = false;
			txtFilePath.Text = filePath;
			progressBar.Start();
		}

		private void LoadComplete()
		{
			if (FileNames.Count == 0)
			{
				btnBrowse.Enabled = true;
			}
			else
			{
				lstFiles.DataSource = new BindingSource(FileNames, null);
				lstFiles.DisplayMember = "Value";
				lstFiles.ValueMember = "Key";
				btnBrowse.Enabled = true;
				btnLoad.Enabled = true;
			}

			progressBar.Stop();
			progressBar.Value = 0;
		}
		#endregion

	}
}
