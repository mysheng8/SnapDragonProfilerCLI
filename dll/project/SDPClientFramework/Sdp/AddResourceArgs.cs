using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000101 RID: 257
	public class AddResourceArgs : EventArgs
	{
		// Token: 0x0400039F RID: 927
		public long Id;

		// Token: 0x040003A0 RID: 928
		public string Name;

		// Token: 0x040003A1 RID: 929
		public string Tooltip;

		// Token: 0x040003A2 RID: 930
		public byte[] Data = new byte[0];

		// Token: 0x040003A3 RID: 931
		public object[] CustomFilterObjects = new object[0];

		// Token: 0x040003A4 RID: 932
		public readonly List<ResourceItem> Children = new List<ResourceItem>();

		// Token: 0x040003A5 RID: 933
		public int CategoryID;
	}
}
