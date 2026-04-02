using System;

namespace Sdp
{
	// Token: 0x02000221 RID: 545
	public class TraceCaptureTrigger
	{
		// Token: 0x040007C5 RID: 1989
		public uint MetricId;

		// Token: 0x040007C6 RID: 1990
		public double Value;

		// Token: 0x040007C7 RID: 1991
		public TraceCaptureTrigger.TraceCaptureTriggerType Type;

		// Token: 0x020003A4 RID: 932
		public enum TraceCaptureTriggerType
		{
			// Token: 0x04000CDC RID: 3292
			CrossAbove,
			// Token: 0x04000CDD RID: 3293
			CrossBelow
		}
	}
}
