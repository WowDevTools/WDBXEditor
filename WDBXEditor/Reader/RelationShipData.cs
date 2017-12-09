using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBXEditor.Reader
{
	public class RelationShipData
	{
		public uint Records;
		public uint MinId;
		public uint MaxId;
		public List<RelationShipEntry> Entries;
	}

	public class RelationShipEntry
	{
		public uint Id;
		public uint Index;

		public RelationShipEntry(uint id, uint index)
		{
			Id = id;
			Index = index;
		}
	}
}
