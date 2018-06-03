using WDBXEditor.Reader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static WDBXEditor.Common.Constants;
using System.Threading.Tasks.Dataflow;
using System.Data;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows.Forms;

namespace WDBXEditor.Storage
{
	class Database
	{
		public static Definition Definitions { get; set; } = new Definition();
		public static List<DBEntry> Entries { get; set; } = new List<DBEntry>();
		public static int BuildNumber { get; set; }


		#region Load
		internal enum ErrorType
		{
			Warning,
			Error
		}

		private static string FormatError(string f, ErrorType t, string s)
		{
			return $"{t.ToString().ToUpper()} {Path.GetFileName(f)} : {s}";
		}

		public static async Task<List<string>> LoadFiles(IEnumerable<string> filenames)
		{
			ConcurrentBag<string> _errors = new ConcurrentBag<string>();
			ConcurrentQueue<string> files = new ConcurrentQueue<string>(filenames.Distinct().OrderBy(x => x).ThenByDescending(x => Path.GetExtension(x)));
			string firstFile = files.First();

			var batchBlock = new BatchBlock<string>(100, new GroupingDataflowBlockOptions { BoundedCapacity = 100 });
			var actionBlock = new ActionBlock<string[]>(t =>
			{
				for (int i = 0; i < t.Length; i++)
				{
					files.TryDequeue(out string file);
					try
					{
						DBReader reader = new DBReader();
						DBEntry entry = reader.Read(file);
						if (entry != null)
						{
							var current = Entries.FirstOrDefault(x => x.FileName == entry.FileName && x.Build == entry.Build);
							if (current != null)
								Entries.Remove(current);

							Entries.Add(entry);
							//if (file != firstFile)
							//    entry.Detach();

							if (!string.IsNullOrWhiteSpace(reader.ErrorMessage))
								_errors.Add(FormatError(file, ErrorType.Warning, reader.ErrorMessage));
						}
					}
					catch (ConstraintException ex) { _errors.Add(FormatError(file, ErrorType.Error, "Id column contains duplicates.")); }
					catch (Exception ex) { _errors.Add(FormatError(file, ErrorType.Error, ex.Message)); }
				}

				ForceGC();
			});
			batchBlock.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });

			foreach (string i in files)
				await batchBlock.SendAsync(i); // wait synchronously for the block to accept.

			batchBlock.Complete();
			await actionBlock.Completion;

			files = null;
			return _errors.ToList();
		}

		public static async Task<List<string>> LoadFiles(ConcurrentDictionary<string, MemoryStream> streams)
		{
			List<string> _errors = new List<string>();
			Queue<KeyValuePair<string, MemoryStream>> files = new Queue<KeyValuePair<string, MemoryStream>>(streams);

			var batchBlock = new BatchBlock<KeyValuePair<string, MemoryStream>>(75, new GroupingDataflowBlockOptions { BoundedCapacity = 100 });
			var actionBlock = new ActionBlock<KeyValuePair<string, MemoryStream>[]>(t =>
			{
				for (int i = 0; i < t.Length; i++)
				{
					var s = files.Dequeue();
					try
					{
						DBReader reader = new DBReader();
						DBEntry entry = reader.Read(s.Value, s.Key);
						if (entry != null)
						{
							var current = Entries.FirstOrDefault(x => x.FileName == entry.FileName && x.Build == entry.Build);
							if (current != null)
								Entries.Remove(current);

							Entries.Add(entry);

							if (!string.IsNullOrWhiteSpace(reader.ErrorMessage))
								_errors.Add(FormatError(s.Key, ErrorType.Warning, reader.ErrorMessage));
						}
					}
					catch (ConstraintException ex)
					{
						_errors.Add(FormatError(s.Key, ErrorType.Error, "Id column contains duplicates."));
					}
					catch (Exception ex)
					{
						_errors.Add(FormatError(s.Key, ErrorType.Error, ex.Message));
					}

					if (i % 100 == 0 && i > 0)
						ForceGC();
				}

				ForceGC();
			});
			batchBlock.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });

			foreach (KeyValuePair<string, MemoryStream> i in streams)
				await batchBlock.SendAsync(i); // wait synchronously for the block to accept.

			batchBlock.Complete();
			await actionBlock.Completion;

			ForceGC();

			return _errors;
		}
		#endregion

		#region Save
		public static async Task<List<string>> SaveFiles(string path)
		{
			List<string> _errors = new List<string>();
			Queue<DBEntry> files = new Queue<DBEntry>(Entries);

			var batchBlock = new BatchBlock<int>(100, new GroupingDataflowBlockOptions { BoundedCapacity = 100 });
			var actionBlock = new ActionBlock<int[]>(t =>
			{
				for (int i = 0; i < t.Length; i++)
				{
					DBEntry file = files.Dequeue();
					try
					{
						new DBReader().Write(file, Path.Combine(path, file.FileName));
					}
					catch (Exception ex) { _errors.Add($"{file} : {ex.Message}"); }
				}

				ForceGC();
			});
			batchBlock.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });

			foreach (int i in Enumerable.Range(0, Entries.Count))
				await batchBlock.SendAsync(i); // wait synchronously for the block to accept.

			batchBlock.Complete();
			await actionBlock.Completion;

			return _errors;
		}
		#endregion

		#region Defintions
		public static async Task LoadDefinitions()
		{
			await Task.Factory.StartNew(() =>
			{
				foreach (var file in Directory.GetFiles(DEFINITION_DIR, "*.xml"))
					Definitions.LoadDefinition(file);
			});
		}
		#endregion

		public static void ForceGC()
		{
			GC.Collect();
			GC.WaitForFullGCComplete();

#if DEBUG
			Debug.WriteLine((GC.GetTotalMemory(false) / 1024 / 1024).ToString() + "mb");
#endif
		}
	}
}
