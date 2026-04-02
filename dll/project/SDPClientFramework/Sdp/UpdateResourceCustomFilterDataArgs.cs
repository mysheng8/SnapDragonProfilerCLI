using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000104 RID: 260
	public class UpdateResourceCustomFilterDataArgs : EventArgs
	{
		// Token: 0x040003AA RID: 938
		public long Id;

		// Token: 0x040003AB RID: 939
		public object Data;

		// Token: 0x040003AC RID: 940
		public int Column;

		// Token: 0x040003AD RID: 941
		public int CategoryID;

		// Token: 0x040003AE RID: 942
		public Dictionary<long, object> Items = new Dictionary<long, object>();
	}
}
