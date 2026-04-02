using System;

namespace Sdp
{
	// Token: 0x020000EB RID: 235
	public class MetricDataReceivedEventArgs : EventArgs
	{
		// Token: 0x06000383 RID: 899 RVA: 0x00009B64 File Offset: 0x00007D64
		public MetricDataReceivedEventArgs(DoubleData data, uint providerId)
		{
			this.Data = data;
			this.ProviderId = providerId;
		}

		// Token: 0x0400034B RID: 843
		public DoubleData Data;

		// Token: 0x0400034C RID: 844
		public uint ProviderId;
	}
}
