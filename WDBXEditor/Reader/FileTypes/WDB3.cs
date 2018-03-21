using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WDBXEditor.Common.Constants;

namespace WDBXEditor.Reader.FileTypes
{
	public class WDB3 : WDB2
	{
		private Dictionary<int, byte[]> ReadOffsetData(BinaryReader dbReader, long pos)
		{
			Dictionary<int, byte[]> CopyTable = new Dictionary<int, byte[]>();

			int[] m_indexes = null;
			long indexOffset = pos + (RecordCount * RecordSize) + StringBlockSize + CopyTableSize;
			
			//Index table
			if (indexOffset < dbReader.BaseStream.Length)
			{
				Flags |= HeaderFlags.IndexMap;
				dbReader.BaseStream.Position = indexOffset;

				m_indexes = new int[RecordCount];
				for (int i = 0; i < RecordCount; i++)
					m_indexes[i] = dbReader.ReadInt32();

				dbReader.Scrub(pos);
			}

			//Extract record data
			for (int i = 0; i < RecordCount; i++)
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
