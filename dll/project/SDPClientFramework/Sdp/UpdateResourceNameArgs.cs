using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000102 RID: 258
	public class UpdateResourceNameArgs : EventArgs
	{
		// Token: 0x040003A6 RID: 934
		public int CategoryID;

		// Token: 0x040003A7 RID: 935
		public Dictionary<long, string> Items = new Dictionary<long, string>();
	}
}
