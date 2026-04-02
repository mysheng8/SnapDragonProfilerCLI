using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000298 RID: 664
	public class MetricDroppedEventArgs : EventArgs
	{
		// Token: 0x04000923 RID: 2339
		public uint MetricId;

		// Token: 0x04000924 RID: 2340
		public List<uint> Pids;

		// Token: 0x04000925 RID: 2341
		public string MetricName;
	}
}
