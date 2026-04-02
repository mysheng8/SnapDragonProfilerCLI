using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020000C4 RID: 196
	public class AddMetricEventArgs : EventArgs
	{
		// Token: 0x040002C2 RID: 706
		public string renderString;

		// Token: 0x040002C3 RID: 707
		public List<string> metricsList = new List<string>();
	}
}
