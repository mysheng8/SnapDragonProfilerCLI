using System;

namespace Sdp
{
	// Token: 0x020000AE RID: 174
	public class EnableMetricEventArgs : EventArgs
	{
		// Token: 0x06000345 RID: 837 RVA: 0x000098FE File Offset: 0x00007AFE
		public EnableMetricEventArgs()
		{
			this.Mode = CaptureType.Realtime;
		}

		// Token: 0x0400027F RID: 639
		public uint CaptureId;

		// Token: 0x04000280 RID: 640
		public uint PID;

		// Token: 0x04000281 RID: 641
		public uint MetricId;

		// Token: 0x04000282 RID: 642
		public bool Enable;

		// Token: 0x04000283 RID: 643
		public CaptureType Mode;
	}
}
