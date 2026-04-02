using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020000AD RID: 173
	public class MetricBeginDragArgs : EventArgs
	{
		// Token: 0x0400027C RID: 636
		public List<uint> ProcessIDs = new List<uint>();

		// Token: 0x0400027D RID: 637
		public uint MetricID;

		// Token: 0x0400027E RID: 638
		public uint CategoryID;
	}
}
