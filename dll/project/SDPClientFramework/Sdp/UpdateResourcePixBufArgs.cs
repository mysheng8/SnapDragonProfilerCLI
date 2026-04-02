using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000103 RID: 259
	public class UpdateResourcePixBufArgs : EventArgs
	{
		// Token: 0x040003A8 RID: 936
		public int CategoryID;

		// Token: 0x040003A9 RID: 937
		public Dictionary<long, byte[]> Items = new Dictionary<long, byte[]>();
	}
}
