using System;

namespace Sdp
{
	// Token: 0x0200020D RID: 525
	public class Data
	{
		// Token: 0x060007C9 RID: 1993 RVA: 0x000154AB File Offset: 0x000136AB
		public Data(Metric metric, uint pid, ulong timestamp)
		{
			this.Metric = metric;
			this.Timestamp = timestamp;
			this.ProcessID = pid;
		}

		// Token: 0x04000769 RID: 1897
		public Metric Metric;

		// Token: 0x0400076A RID: 1898
		public uint ProcessID;

		// Token: 0x0400076B RID: 1899
		public ulong Timestamp;
	}
}
