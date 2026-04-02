using System;

namespace Sdp
{
	// Token: 0x020000F6 RID: 246
	public class TrackCreatedEventArgs : EventArgs
	{
		// Token: 0x0400036A RID: 874
		public IMetricPlugin MetricPlugin;

		// Token: 0x0400036B RID: 875
		public MetricDescription TrackMetric;

		// Token: 0x0400036C RID: 876
		public TrackControllerBase TrackController;
	}
}
