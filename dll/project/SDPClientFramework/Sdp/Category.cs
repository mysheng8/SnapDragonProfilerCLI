using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200010D RID: 269
	public class Category
	{
		// Token: 0x040003BE RID: 958
		public int Id;

		// Token: 0x040003BF RID: 959
		public string Name;

		// Token: 0x040003C0 RID: 960
		public ResourcesCategoryStyle Style;

		// Token: 0x040003C1 RID: 961
		public List<ResourceItem> ResourceItems = new List<ResourceItem>();

		// Token: 0x040003C2 RID: 962
		public Type[] CustomFilterColumns = new Type[0];
	}
}
