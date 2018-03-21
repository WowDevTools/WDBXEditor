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
	public class WDB4 : WDB3
	{
		public override void ReadHeader(ref BinaryReader dbReader, string signature)
		{
			base.ReadHeader(ref dbReader, signature);
			Flags = (HeaderFlags)dbReader.ReadUInt32();
		}

		private Dictionary<int, byte[]> ReadOffsetData(BinaryReader dbReader, long pos)
		{
			Dictionary<int, byte[]> CopyTable = new Dictionary<int, byte[]>();
			List<Tuple<int, short>> offsetmap = new List<Tuple<int, short>>();
			Dictionary<int, OffsetDuplicate> firstindex = new Dictionary<int, OffsetDuplicate>();

			int[] m_indexes = null;
			long offsetOffset = pos + (RecordCount * RecordSize) + StringBlockSize;
			long indexOffset = offsetOffset + CopyTableSize;

			//Offset Map
			if (HasOffsetTable)
			{
				// Records table
				dbReader.Scrub(StringBlockSize);

				for (int i = 0; i < (MaxId - MinId + 1); i++)
				{
					int offset = dbReader.ReadInt32();
					short length = dbReader.ReadInt16();

					if (offset == 0 || length == 0) continue;

					//Special case, may contain duplicates in the offset map that we don't want
					if (CopyTableSize == 0)
					{
						if (!firstindex.ContainsKey(offset))
							firstindex.Add(offset, new OffsetDuplicate(offsetmap.Count, firstindex.Count));
						else
							OffsetDuplicates.Add(MinId + i, firstindex[offset].VisibleIndex);
					}

					offsetmap.Add(new Tuple<int, short>(offset, length));
				}
			}

			//Index table
			if (HasOffsetTable)
			{
				dbReader.BaseStream.Position = indexOffset;

				m_indexes = new int[RecordCount];
				for (int i = 0; i < RecordCount; i++)
					m_indexes[i] = dbReader.ReadInt32();

				dbReader.BaseStream.Position = pos;
			}

			//Extract record data
			for (int i = 0; i < Math.Max(RecordCount, offsetmap.Count); i++)
			{
				if (HasOffsetTable)
				{
					int id = m_indexes[CopyTable.Count];
					var map = offsetmap[i];

					if (CopyTableSize == 0 && firstindex[map.Item1].HiddenIndex != i) //Ignore duplicates
						continue;

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
			dbReader.BaseStream.Position += StringBlockSize;
			long copyTablePos = pos + (HasIndexTable ? 4 * RecordCount : 0);
			if (CopyTableSize != 0 && copyTablePos != dbReader.BaseStream.Length)
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
