using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200022F RID: 559
	public class IdNamePair : Comparer<IdNamePair>
	{
		// Token: 0x060008E1 RID: 2273 RVA: 0x0001A76E File Offset: 0x0001896E
		public IdNamePair()
		{
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001A776 File Offset: 0x00018976
		public IdNamePair(uint id, string name)
		{
			this.Id = id;
			this.Name = name;
			this.DisplayName = name;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001A793 File Offset: 0x00018993
		public IdNamePair(uint id, string name, bool enabled)
		{
			this.Id = id;
			this.Name = name;
			this.DisplayName = name;
			this.Enabled = enabled;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001A7B7 File Offset: 0x000189B7
		public override int Compare(IdNamePair a, IdNamePair b)
		{
			return a.Name.CompareTo(b.Name);
		}

		// Token: 0x040007E9 RID: 2025
		public uint Id;

		// Token: 0x040007EA RID: 2026
		public string Name;

		// Token: 0x040007EB RID: 2027
		public bool Enabled;

		// Token: 0x040007EC RID: 2028
		public string Tooltip;

		// Token: 0x040007ED RID: 2029
		public string DisplayName;
	}
}
