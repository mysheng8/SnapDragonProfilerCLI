using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200010E RID: 270
	public class ResourceItem
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00009D7D File Offset: 0x00007F7D
		public int NumberOfColumns
		{
			get
			{
				return 5 + this.CustomFilterObjects.Length;
			}
		}

		// Token: 0x040003C3 RID: 963
		public long Id;

		// Token: 0x040003C4 RID: 964
		public string Name;

		// Token: 0x040003C5 RID: 965
		public string Tooltip;

		// Token: 0x040003C6 RID: 966
		public byte[] Data = new byte[0];

		// Token: 0x040003C7 RID: 967
		public List<ResourceItem> Children = new List<ResourceItem>();

		// Token: 0x040003C8 RID: 968
		public object[] CustomFilterObjects = new object[0];
	}
}
