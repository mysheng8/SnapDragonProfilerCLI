using System;

namespace Sdp
{
	// Token: 0x020000AF RID: 175
	public class ChangeMetricDataSourcesColorArgs : EventArgs
	{
		// Token: 0x06000346 RID: 838 RVA: 0x0000990D File Offset: 0x00007B0D
		public ChangeMetricDataSourcesColorArgs()
		{
			this.color = new double[3];
		}

		// Token: 0x04000284 RID: 644
		public uint ProcessId;

		// Token: 0x04000285 RID: 645
		public uint MetricId;

		// Token: 0x04000286 RID: 646
		public uint CaptureId;

		// Token: 0x04000287 RID: 647
		public double[] color;
	}
}
