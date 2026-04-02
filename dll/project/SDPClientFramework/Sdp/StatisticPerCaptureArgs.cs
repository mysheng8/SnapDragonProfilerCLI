using System;

namespace Sdp
{
	// Token: 0x02000153 RID: 339
	public class StatisticPerCaptureArgs : EventArgs
	{
		// Token: 0x040004CA RID: 1226
		public IStatistic Statistic;

		// Token: 0x040004CB RID: 1227
		public StatisticState State;

		// Token: 0x040004CC RID: 1228
		public int CaptureID;
	}
}
