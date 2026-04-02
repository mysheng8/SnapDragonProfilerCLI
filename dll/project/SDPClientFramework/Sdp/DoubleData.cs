using System;

namespace Sdp
{
	// Token: 0x0200020E RID: 526
	public class DoubleData : Data
	{
		// Token: 0x060007CA RID: 1994 RVA: 0x000154C8 File Offset: 0x000136C8
		public DoubleData(Metric metric, uint pid, ulong timestamp, double value)
			: base(metric, pid, timestamp)
		{
			this.Value = value;
		}

		// Token: 0x0400076C RID: 1900
		public double Value;
	}
}
