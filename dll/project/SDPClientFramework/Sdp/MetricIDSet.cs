using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200019D RID: 413
	public class MetricIDSet : Comparer<MetricIDSet>
	{
		// Token: 0x06000503 RID: 1283 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public MetricIDSet(uint mid, uint pid, uint cid, uint capid)
		{
			this.MetricID = mid;
			this.ProcessID = pid;
			this.CategoryID = cid;
			this.CaptureID = capid;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000B7C5 File Offset: 0x000099C5
		public override int Compare(MetricIDSet a, MetricIDSet b)
		{
			if (a.MetricID == b.MetricID && a.ProcessID == b.ProcessID)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x04000629 RID: 1577
		public uint MetricID;

		// Token: 0x0400062A RID: 1578
		public uint ProcessID;

		// Token: 0x0400062B RID: 1579
		public uint CategoryID;

		// Token: 0x0400062C RID: 1580
		public uint CaptureID;
	}
}
