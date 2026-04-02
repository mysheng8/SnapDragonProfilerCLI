using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000100 RID: 256
	public class PrepopulateCategoryArgs : EventArgs
	{
		// Token: 0x0400039B RID: 923
		public int Source;

		// Token: 0x0400039C RID: 924
		public int DrawcallId;

		// Token: 0x0400039D RID: 925
		public int CategoryId;

		// Token: 0x0400039E RID: 926
		public List<long> ResourceIds = new List<long>();
	}
}
