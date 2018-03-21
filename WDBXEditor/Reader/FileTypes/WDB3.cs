using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WDBXEditor.Common.Constants;
using static WDBXEditor.Reader.FileTypes.WDB5;

namespace WDBXEditor.Reader.FileTypes
{
	public class WDB3 : WDB2
	{
		public override bool HasOffsetTable => Flags.HasFlag(HeaderFlags.OffsetMap);
		public override bool HasIndexTable => Flags.HasFlag(HeaderFlags.IndexMap);

		private readonly uint[] offsetMapDBs = new uint[] { 1344507586 };

		private Dictionary<int, byte[]> ReadOffsetData(BinaryReader dbReader, long pos)
		{
			Dictionary<int, byte[]> CopyTable = new Dictionary<int, byte[]>();
			List<Tuple<int, short>> offsetmap = new List<Tuple<int, short>>();

			int[] m_indexes = null;
			
			long knownSize = pos + (RecordCount * RecordSize) + StringBlockSize + CopyTableSize;

			if (knownSize + (RecordCount * 4) == dbReader.BaseStream.Length) // rough index table check
				Flags |= HeaderFlags.IndexMap;
			else if (knownSize + (RecordCount * 4) < dbReader.BaseStream.Length) // rough offset map check, should parse meta ideally
				Flags |= HeaderFlags.OffsetMap | HeaderFlags.IndexMap;
			else if (offsetMapDBs.Contains(TableHash))
				Flags |= HeaderFlags.OffsetMap | HeaderFlags.IndexMap; // override above check with hardcoded known list 

			//Offset map
			if (HasOffsetTable)
			{
				for (int i = 0; i < (MaxId - MinId + 1); i++)
				{
					int offset = dbReader.ReadInt32();
					short length = dbReader.ReadInt16();

					if (offset == 0 || length == 0) continue;

					offsetmap.Add(new Tuple<int, short>(offset, length));
				}

				pos = dbReader.BaseStream.Position;
			}

			//Index table
			if (HasIndexTable)
			{
				dbReader.Scrub(dbReader.BaseStream.Length - CopyTableSize - (RecordCount * 4));

				m_indexes = new int[RecordCount];
				for (int i = 0; i < RecordCount; i++)
					m_indexes[i] = dbReader.ReadInt32();

				dbReader.Scrub(pos);
			}

			//Extract record data
			for (int i = 0; i < RecordCount; i++)
			{
				if (HasOffsetTable)
				{
					int id = m_indexes[CopyTable.Count];
					var map = offsetmap[i];

					dbReader.Scrub(map.Item1);

					IEnumerable<byte> recordbytes = BitConverter.GetBytes(id).Concat(dbReader.ReadBytes(map.Item2));
					CopyTable.Add(id, recordbytes.ToArray());
				}
				else
				{
					dbReader.Scrub(pos + i * RecordSize);
					byte[] recordbytes = dbReader.ReadBytes((int)RecordSize);

					if (HasIndexTable)
					{
						IEnumerable<byte> newrecordbytes = BitConverter.GetBytes(m_indexes[i]).Concat(recordbytes);
						CopyTable.Add(m_indexes[i], newrecordbytes.ToArray());
					}
					else
					{
						CopyTable.Add(BitConverter.ToInt32(recordbytes, 0), recordbytes);
					}
				}
			}

			//CopyTable
			dbReader.Scrub(dbReader.BaseStream.Length - CopyTableSize);
			long copyTablePos = pos + (HasIndexTable ? 4 * RecordCount : 0);
			if (CopyTableSize != 0 && copyTablePos < dbReader.BaseStream.Length)
			{
				dbReader.Scrub(copyTablePos);
				while (dbReader.BaseStream.Position != dbReader.BaseStream.Length)
				{
					int id = dbReader.ReadInt32();
					int idcopy = dbReader.ReadInt32();

					byte[] copyRow = CopyTable[idcopy];
					byte[] newRow = new byte[copyRow.Length];
					Array.Copy(copyRow, newRow, newRow.Length);
					Array.Copy(BitConverter.GetBytes(id), newRow, sizeof(int));

					CopyTable.Add(id, newRow);
				}
			}

			return CopyTable;
		}

		public override byte[] ReadData(BinaryReader dbReader, long pos)
		{
			Dictionary<int, byte[]> CopyTable = ReadOffsetData(dbReader, pos);
			OffsetLengths = CopyTable.Select(x => x.Value.Length).ToArray();
			return CopyTable.Values.SelectMany(x => x).ToArray();
		}
	}
}
