using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using WDBXEditor.Common;
using System.IO.Compression;

namespace WDBXEditor
{
	static class UpdateManager
	{
		private const string TEMP_DIRECTORY = ".tmp";
		private static readonly string CUR_DIRECTORY = Application.StartupPath;
		private static readonly string TEMP_UPDATE_ZIP = Path.Combine(CUR_DIRECTORY, "update.zip");

		private static bool RestartOnComplete = false;

		public static void Clean()
		{
			if (Directory.Exists(TEMP_DIRECTORY))
				Directory.Delete(TEMP_DIRECTORY, true);
		}

		public static async Task CheckForUpdate()
		{
			if (File.Exists(TEMP_UPDATE_ZIP))
			{
				ExtractFiles();
			}
			else
			{
				using (var client = new WebClient())
				{
					string releaseURL = Properties.Settings.Default["ReleaseURL"].ToString();
					string releaseAPI = Properties.Settings.Default["ReleaseAPI"].ToString();
					string userAgent = Properties.Settings.Default["UserAgent"].ToString();
					client.Headers["User-Agent"] = userAgent + Constants.VERSION;

					string json = await client.DownloadStringTaskAsync(releaseAPI);
					var serializer = new JavaScriptSerializer();

					GithubReleaseModel model = serializer.Deserialize<GithubReleaseModel>(json);
					if (model != null && model.tag_name != Constants.VERSION)
					{
						string text = $"An update is available.\r\n Click \"Yes\" to upgrade to {model.tag_name}. (This will restart the application)";

						DialogResult dialogResult = MessageBox.Show(text, "Update Available", MessageBoxButtons.YesNo);
						RestartOnComplete = (dialogResult == DialogResult.Yes);
						await DoDownload(model.assets[0].browser_download_url);
					}
				}
			}
		}

		private static async Task DoDownload(string releaseURL)
		{
			using (var client = new WebClient())
			{
				string userAgent = Properties.Settings.Default["UserAgent"].ToString();
				client.Headers["User-Agent"] = userAgent + Constants.VERSION;

				try
				{
					byte[] buffer = await client.DownloadDataTaskAsync(releaseURL);
					using (var fs = File.Create(TEMP_UPDATE_ZIP))
						fs.Write(buffer, 0, buffer.Length);
				}
				catch
				{
					File.Delete(TEMP_UPDATE_ZIP);
					return;
				}

				ExtractFiles();
			}
		}

		private static void ExtractFiles()
		{
			UPDATE_STATE state = UPDATE_STATE.SUCCESS;
			string backupdirectory = Path.Combine(CUR_DIRECTORY, TEMP_DIRECTORY);
			var fileMap = new Dictionary<string, string>();

			Directory.CreateDirectory(backupdirectory);

			try
			{
				using (var archive = ZipFile.OpenRead(TEMP_UPDATE_ZIP))
				{
					foreach (ZipArchiveEntry file in archive.Entries)
					{
						if (string.IsNullOrWhiteSpace(file.Name))
							continue;

						string originalFileName = Path.Combine(CUR_DIRECTORY, file.FullName);
						string backupFolder = Path.Combine(backupdirectory, Path.GetDirectoryName(file.FullName));
						string backupFileName = Path.Combine(backupFolder, file.Name);

						// store file old, new paths for rollback
						fileMap[originalFileName] = backupFileName;

						try
						{
							Directory.CreateDirectory(backupFolder);
							File.Move(originalFileName, backupFileName); // attempt to backup and replace old files
							file.ExtractToFile(originalFileName, true);
						}
						catch
						{
							state = UPDATE_STATE.FAILED_FILE; // access error usually
							break;
						}
					}
				}
			}
			catch // FileNotFoundException InvalidDataException
			{
				state = UPDATE_STATE.FAILED_ZIP;
			}

			// remove the update folder regardless of success/broken zip
			File.Delete(TEMP_UPDATE_ZIP);

			// either rollback if an error or restart
			if (state == UPDATE_STATE.SUCCESS && RestartOnComplete)
				Application.Restart();
			else
				Rollback(fileMap);

			if (state != UPDATE_STATE.SUCCESS && RestartOnComplete)
			{
				MessageBox.Show(
					"Updating failed.\r\n WDBX Editor will try again next restart. You can also manually extract `Update.zip`.",
					"Update Failed",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation
				);
			}
		}

		private static void Rollback(Dictionary<string, string> fileMap)
		{
			foreach (var map in fileMap)
			{
				File.Delete(map.Key);
				File.Move(map.Value, map.Key); // move everything back
			}
			Clean();
		}

		internal enum UPDATE_STATE
		{
			FAILED_ZIP,
			FAILED_FILE,
			SUCCESS
		}
	}
}
