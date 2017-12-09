using WDBXEditor.Reader;
using WDBXEditor.Reader.FileTypes;
using WDBXEditor.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading.Tasks.Dataflow;
using System.Collections.Concurrent;
using static WDBXEditor.Common.Constants;
using WDBXEditor.Common;

namespace WDBXEditor.Forms
{
	public partial class LegionParser : Form
	{
		private ConcurrentDictionary<string, MemoryStream> streams = new ConcurrentDictionary<string, MemoryStream>();
		private bool casc = false;
		private List<Table> tables = new List<Table>();


		public LegionParser()
		{
			InitializeComponent();
		}

		#region Parser
		private void Parse(string file)
		{
			Parse(new MemoryStream(System.IO.File.ReadAllBytes(file)), file);
		}

		private DBHeader ExtractHeader(BinaryReader dbReader)
		{
			DBHeader header = null;
			string signature = dbReader.ReadString(4);

			if (string.IsNullOrWhiteSpace(signature))
				return null;

			switch (signature)
			{
				case "WDB5":
					header = new WDB5();
					break;
				case "WDB6":
					header = new WDB6();
					break;
				case "WDC1":
					header = new WDC1();
					break;
				default:
					return null;
			}

			header?.ReadHeader(ref dbReader, signature);
			return header;
		}

		private void Parse(MemoryStream stream, string file)
		{
			ParseWDC1(stream, file);
			return;

			stream.Position = 0;

			using (var dbReader = new BinaryReader(stream, Encoding.UTF8))
			{
				DBHeader header = ExtractHeader(dbReader);
				if (header == null)
					return;

				long pos = dbReader.BaseStream.Position;

				int CopyTableSize = header.CopyTableSize; //Only WDB5 has a copy table
				uint CommonDataTableSize = header.CommonDataTableSize;

				//StringTable - only if applicable
				long copyTablePos = dbReader.BaseStream.Length - CommonDataTableSize - CopyTableSize;
				long indexTablePos = copyTablePos - (header.HasIndexTable ? header.RecordCount * 4 : 0);
				long wch7TablePos = indexTablePos - (header.UnknownWCH7 * 4);
				long stringTableStart = wch7TablePos - header.StringBlockSize;

				Dictionary<int, string> StringTable = new Dictionary<int, string>();
				if (!header.HasOffsetTable)
				{
					dbReader.Scrub(stringTableStart);
					StringTable = new StringTable().Read(dbReader, stringTableStart, stringTableStart + header.StringBlockSize);
					dbReader.Scrub(pos);
				}

				Dictionary<int, FieldType> FieldTypes = new Dictionary<int, FieldType>()
				{
					{4, FieldType.UNKNOWN },
					{3, FieldType.INT },
					{2, FieldType.USHORT},
					{1, FieldType.BYTE },
				};

				//Read data
				List<byte[]> copytabledata = new List<byte[]>();
				if (header.IsTypeOf<WDB6>())
					copytabledata = (header as WDB6).ReadOffsetData(dbReader, pos).Values.ToList();
				else
					copytabledata = (header as WDB5).ReadOffsetData(dbReader, pos).Values.ToList();

				//String table
				bool stringtableused = StringTable.Values.Any(x => !string.IsNullOrWhiteSpace(x)) && !header.HasOffsetTable;

				//Calculate known field types
				List<FieldInfo> fields = new List<FieldInfo>();
				for (int i = 0; i < header.FieldStructure.Count; i++)
				{
					int bytecount = header.FieldStructure[i].ByteCount;

					FieldInfo fi = new FieldInfo();
					fi.ArraySize = GetArraySize(ref header, i);

					if (i == header.IdIndex)
						fi.Type = FieldType.INT;
					else
						fi.Type = FieldTypes[bytecount];

					//WDB6 Common Data check
					if (header.FieldStructure[i].CommonDataColumn)
					{
						switch (header.FieldStructure[i].CommonDataType)
						{
							case 0:
								fi.Type = FieldType.STRING;
								break;
							case 1:
								fi.Type = FieldType.USHORT;
								break;
							case 2:
								fi.Type = FieldType.BYTE;
								break;
							case 3:
								fi.Type = FieldType.FLOAT;
								break;
							case 4:
								fi.Type = FieldType.INT;
								break;
						}
					}

					fields.Add(fi);
				}

				//Attempt to figure out unknown types
				for (int i = 0; i < fields.Count; i++)
				{
					if (fields[i].Type != FieldType.UNKNOWN)
						continue;

					List<FieldType> options = new List<FieldType>() { FieldType.INT, FieldType.UINT, FieldType.FLOAT, FieldType.STRING };
					if (!stringtableused)
						options.Remove(FieldType.STRING); //Stringtable not used

					List<int> intvals = new List<int>();
					List<string> stringvals = new List<string>();
					List<float> floatvals = new List<float>();

					for (int d = 0; d < copytabledata.Count; d++)
					{
						byte[] cdata = copytabledata[d];

						int start = header.FieldStructure[i].Offset;
						if (header.HasOffsetTable)
						{
							start = 0;
							for (int x = 0; x < i; x++)
							{
								if (fields[x].Type != FieldType.STRING)
								{
									int bytecount = header.FieldStructure[x].ByteCount;
									start += bytecount * fields[x].ArraySize;
								}
								else
									start += cdata.Skip(start).TakeWhile(b => b != 0).Count() + 1;
							}
						}

						byte[] data = cdata.Skip(start).Take(4).ToArray();
						if (!header.HasOffsetTable && data.All(x => x == 0)) continue; //Ignore 0 byte columns as they could be anything

						//Get int value
						int intval = BitConverter.ToInt32(data, 0);
						intvals.Add(intval);

						//String check
						if (options.Contains(FieldType.STRING))
						{
							if (header.HasOffsetTable)
							{
								//Check for control and nonunicode chars
								string stringval = Encoding.UTF8.GetString(cdata.Skip(start).TakeWhile(x => x != 0).ToArray());
								if (stringval.Length >= 1 && stringval.Any(x => char.IsControl(x) || x == 0xFFFD))
									options.Remove(FieldType.STRING);
								else
									stringvals.Add(stringval);
							}
							else
							{
								//Check it is in the stringtable and more than -1
								if (intval < 0 || !StringTable.ContainsKey(intval))
									options.Remove(FieldType.STRING);
							}
						}

						//Float check
						if (options.Contains(FieldType.FLOAT))
						{
							//Basic float checks
							float single = BitConverter.ToSingle(data, 0);
							if (!float.IsInfinity(single) && !float.IsNaN(single) && (single >= 9.99999997475243E-07 && single <= 100000.0))
								floatvals.Add(single);
							else if (single != 0) //Ignore 0s
								options.Remove(FieldType.FLOAT);
						}

						//UInt check
						if (options.Contains(FieldType.UINT) && intval < 0) //If less than 0 must be signed
							options.Remove(FieldType.UINT);
					}

					var uniquestr = new HashSet<string>(stringvals);
					var uniqueint = new HashSet<int>(intvals);
					var uniquefloat = new HashSet<float>(floatvals);

					if (uniqueint.Count == 1 && uniqueint.First() == 0) //All 0s
					{
						fields[i].Type = FieldType.INT;
					}
					else if (!header.HasOffsetTable && options.Contains(FieldType.STRING)) //Int if only 1 Int else String
					{
						fields[i].Type = (uniqueint.Count == 1 ? FieldType.INT : FieldType.STRING);
					}
					else if (header.HasOffsetTable && options.Contains(FieldType.STRING) && uniquestr.Count > 1) //More than 1 string
					{
						fields[i].Type = FieldType.STRING;
					}
					else if (header.HasOffsetTable && options.Contains(FieldType.STRING) && uniquefloat.Count <= 1) //1 or less float and string
					{
						fields[i].Type = FieldType.STRING;
					}
					else if (options.Contains(FieldType.FLOAT) && floatvals.Count > 0) //Floats count more than 1
					{
						fields[i].Type = FieldType.FLOAT;
					}
					else if (options.Contains(FieldType.UINT)) //Uint over Int
					{
						fields[i].Type = FieldType.UINT;
					}
					else
					{
						fields[i].Type = FieldType.INT;
					}
				}

				Table table = new Table();
				table.Name = Path.GetFileNameWithoutExtension(file);
				table.Fields = new List<Field>();
				string format = $"X{header.FieldStructure.Max(x => x.Offset).ToString().Length}"; //X2, X3 etc

				for (int i = 0; i < fields.Count; i++)
				{
					Field field = new Field();
					field.Name = (i == header.IdIndex ? "ID" : $"field{header.FieldStructure[i].Offset.ToString(format)}");
					field.IsIndex = (i == header.IdIndex);
					field.ArraySize = (field.IsIndex ? 1 : fields[i].ArraySize);
					field.Type = fields[i].Type.ToString().ToLower();
					table.Fields.Add(field);
					Console.WriteLine($"Name: {field.Name} | Array: {field.ArraySize} | Type: {field.Type}");
				}

				tables.Add(table);
				Database.ForceGC();
			}
		}


		private void ParseWDC1(MemoryStream stream, string file)
		{
			stream.Position = 0;

			Dictionary<int, FieldType> FieldTypes = new Dictionary<int, FieldType>()
			{
				{8, FieldType.ULONG},
				{4, FieldType.INT },
				{2, FieldType.USHORT},
				{1, FieldType.BYTE },
			};

			using (var dbReader = new BinaryReader(stream, Encoding.UTF8))
			{
				WDC1 header = ExtractHeader(dbReader) as WDC1;
				if (header == null)
					return;
				if (header.RecordCount == 0 || header.RecordSize == 0)
					return;

				long pos = dbReader.BaseStream.Position;
				Dictionary<int, string> StringTable = new StringTable().Read(dbReader, pos, pos + header.StringBlockSize);
				bool stringtableused = StringTable.Values.Any(x => !string.IsNullOrWhiteSpace(x)) && !header.HasOffsetTable;

				List<FieldInfo> fields = new List<FieldInfo>();
				var copyTable = header.ReadOffsetData(dbReader, dbReader.BaseStream.Position);

				for (int f = 0; f < header.ColumnMeta.Count; f++)
				{
					FieldType byteType;
					int arraySize = 1;

					if (f == header.IdIndex || (f == 0 && header.HasIndexTable))
					{
						fields.Add(new FieldInfo() { ArraySize = 1, Type = FieldType.INT });
						continue;
					}

					if (header.ColumnMeta[f].CompressionType == CompressionType.None)
					{
						int bitSize = header.FieldStructure[f].BitCount;
						byteType = FieldTypes[NextPow2(~~(bitSize + 7) / 8)];
						arraySize = Math.Max(header.ColumnMeta[f].Size / bitSize, 1);
					}
					else if(header.ColumnMeta[f].CompressionType > CompressionType.Immediate)
					{
						arraySize = Math.Max(header.ColumnMeta[f].Cardinality, 1);
						byteType = FieldType.INT;
					}
					else
					{
						byteType = FieldTypes[NextPow2(~~(header.ColumnMeta[f].BitWidth + 7) / 8)];
					}

					fields.Add(new FieldInfo()
					{
						ArraySize = arraySize,
						Type = byteType == FieldType.INT ? FieldType.UNKNOWN : byteType
					});
				}

				int offset = 0;
				for (int i = 0; i < fields.Count; i++)
				{
					switch (fields[i].Type)
					{
						case FieldType.BYTE:
							offset++;
							continue;
						case FieldType.USHORT:
							offset += 2;
							continue;
						case FieldType.INT:
							offset += 4;
							continue;
						case FieldType.ULONG:
							offset += 8;
							continue;
					}

					List<FieldType> options = new List<FieldType>() { FieldType.INT, FieldType.FLOAT, FieldType.STRING };
					if (!stringtableused)
						options.Remove(FieldType.STRING); //Stringtable not used

					List<int> ints = new List<int>();
					List<float> floats = new List<float>();
					foreach (var c in copyTable)
					{
						for (int x = 0; x < fields[i].ArraySize; x++)
						{
							int asInt = BitConverter.ToInt32(c.Value.Skip(offset + (4 * x)).Take(4).ToArray(), 0);
							if(asInt > 0)
							{
								ints.Add(asInt);

								if (FloatUtil.IsLikelyFloat(asInt))
									floats.Add(BitConverter.ToSingle(BitConverter.GetBytes(asInt), 0));
							}
						}						
					}

					// remove 0's as they could be anything - if all removed then guess its an int
					ints.RemoveAll(x => x == 0);
					if(ints.Count == 0)
					{
						fields[i].Type = FieldType.INT;
						offset += (4 * fields[i].ArraySize);
						continue;
					}

					// stringtable doesn't contain string so cant be a string
					if (options.Contains(FieldType.STRING) && ints.Any(x => !StringTable.ContainsKey(x)))
						options.Remove(FieldType.STRING);

					if(floats.Count / (float)ints.Count >= 0.85)
						fields[i].Type = FieldType.FLOAT;
					else if(options.Contains(FieldType.STRING))
						fields[i].Type = FieldType.STRING;
					else
						fields[i].Type = FieldType.INT;

					offset += (4 * fields[i].ArraySize);
				}

				Table table = new Table();
				table.Name = Path.GetFileNameWithoutExtension(file);
				table.Fields = new List<Field>();
				string format = $"X{fields.Count.ToString("X").Length}"; //X2, X3 etc

				for (int i = 0; i < fields.Count; i++)
				{
					Field field = new Field();
					field.Name = (i == header.IdIndex ? "ID" : $"field{i.ToString(format)}");
					field.IsIndex = (i == header.IdIndex);
					field.ArraySize = (field.IsIndex ? 1 : fields[i].ArraySize);
					field.Type = fields[i].Type.ToString().ToLower();
					table.Fields.Add(field);
					Console.WriteLine($"Name: {field.Name} | Array: {field.ArraySize} | Type: {field.Type}");
				}

				tables.Add(table);
				Database.ForceGC();
			}
		}

		private int NextPow2(int v)
		{
			v--;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v++;
			return Math.Max(v, 1);
		}

		private int GetArraySize(ref DBHeader header, int index)
		{
			var f = header.FieldStructure[index];

			if (f.CommonDataColumn)
				return 1;
			else if (index == header.FieldStructure.Count - 1)
				return Math.Max(1, ((int)header.RecordSize - f.Offset) / f.ByteCount);
			else
				return Math.Max(1, (header.FieldStructure[index + 1].Offset - f.Offset) / f.ByteCount);
		}

		internal class FieldInfo
		{
			public FieldType Type { get; set; }
			public int ArraySize { get; set; }
		}

		internal enum FieldType
		{
			INT,
			STRING,
			FLOAT,
			UINT,
			BYTE,
			USHORT,
			ULONG,
			UNKNOWN
		}

		#endregion

		#region Button Events
		private void btnSelect_Click(object sender, EventArgs e)
		{
			streams.Clear();

			if (rdoCASC.Checked)
			{
				casc = true;
				using (var mpq = new LoadMPQ())
				{
					mpq.IsMPQ = false;
					if (mpq.ShowDialog() == DialogResult.OK)
						Parallel.ForEach(mpq.Streams, x => streams.TryAdd(x.Key, x.Value));
				}
			}
			else
			{
				casc = false;
				using (var fd = new OpenFileDialog())
				{
					fd.Filter = "DB2 Files|*db2";
					fd.Multiselect = true;
					if (fd.ShowDialog() == DialogResult.OK)
						Parallel.ForEach(fd.FileNames, f => streams.TryAdd(f, null));
				}
			}

			dgFiles.Rows.Clear();

			if (streams.Count > 0)
				foreach (var s in streams.Keys)
					dgFiles.Rows.Add(Path.GetFileName(s), false, false);

			btnParse.Enabled = streams.Count > 0;
		}

		private void btnParse_Click(object sender, EventArgs e)
		{
			autoProgressBar1.Start();
			panel1.Enabled = false;
			tables.Clear();

			Task.Run(() => RunParser())
				.ContinueWith(x =>
			{
				autoProgressBar1.Stop();
				panel1.Enabled = true;
				btnSave.Enabled = true;
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private async Task RunParser()
		{
			Queue<DataGridViewRow> rows = new Queue<DataGridViewRow>(dgFiles.Rows.Cast<DataGridViewRow>());

			var batchBlock = new BatchBlock<int>(75, new GroupingDataflowBlockOptions { BoundedCapacity = 100 });
			var actionBlock = new ActionBlock<int[]>(t =>
			{
				for (int i = 0; i < t.Length; i++)
				{
					var row = rows.Dequeue();
					var filename = Path.GetFileName((string)row.Cells["File"].Value);
					var s = streams.FirstOrDefault(x => Path.GetFileName(x.Key).Equals(filename, StringComparison.CurrentCultureIgnoreCase));
					if (casc)
						Parse(s.Value, s.Key);
					else
						Parse(s.Key);

					this.Invoke((MethodInvoker)delegate { row.Cells["Parsed"].Value = true; });
				}
			});
			batchBlock.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });

			for (int i = 0; i < dgFiles.Rows.Count; i++)
				await batchBlock.SendAsync(i); //wait synchronously for the block to accept

			batchBlock.Complete();
			await actionBlock.Completion;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			int build = 0;
			if (int.TryParse(txtBuild.Text, out build))
			{
				if (build < 21479)
				{
					MessageBox.Show("Build must be greater than or equal to Legion 7.0.3 (21479).");
					return;
				}

				var names = dgFiles.Rows.Cast<DataGridViewRow>().Select(x => Path.GetFileNameWithoutExtension(x.Cells["File"].Value.ToString()));
				var existing = Database.Definitions.Tables.Where(x => x.Build == build && names.Contains(x.Name)).ToList();

				string msg = $"This will overwrite {existing.Count} defintions. Do you wish to continue?";
				if (existing.Count > 0 && MessageBox.Show(msg, "Overwrite", MessageBoxButtons.YesNo) == DialogResult.No)
					return;

				while (existing.Count > 0)
				{
					Database.Definitions.Tables.Remove(existing[0]);
					existing.RemoveAt(0);
				}

				tables.ForEach(x => { x.Build = build; x.Load(); });
				Database.Definitions.Tables.UnionWith(tables);

				if (!Database.Definitions.SaveDefinitions())
					MessageBox.Show("Unable to save definitions.");
			}
			else
				MessageBox.Show("Please enter a numeric build number.");
		}
		#endregion
	}
}
