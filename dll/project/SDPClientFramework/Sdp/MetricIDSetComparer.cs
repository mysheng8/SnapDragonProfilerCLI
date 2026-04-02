using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200019E RID: 414
	public class MetricIDSetComparer : IEqualityComparer<MetricIDSet>
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x0000B7E6 File Offset: 0x000099E6
		public bool Equals(MetricIDSet x, MetricIDSet y)
		{
			return x.MetricID == y.MetricID && x.ProcessID == y.ProcessID;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0000B806 File Offset: 0x00009A06
		public int GetHashCode(MetricIDSet pair)
		{
			return (int)(pair.MetricID + pair.ProcessID);
		}
	}
}
