using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020000FE RID: 254
	public class AddCategoryArgs : EventArgs
	{
		// Token: 0x04000394 RID: 916
		public int ID;

		// Token: 0x04000395 RID: 917
		public string Name;

		// Token: 0x04000396 RID: 918
		public ResourcesCategoryStyle Style;

		// Token: 0x04000397 RID: 919
		public List<ResourceItem> ResourceItems = new List<ResourceItem>();

		// Token: 0x04000398 RID: 920
		public Type[] CustomFilterColumns = new Type[0];

		// Token: 0x04000399 RID: 921
		public int CostBar;
	}
}
