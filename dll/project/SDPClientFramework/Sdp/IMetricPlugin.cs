using System;

namespace Sdp
{
	// Token: 0x02000206 RID: 518
	public interface IMetricPlugin
	{
		// Token: 0x060007A7 RID: 1959
		bool HandlesMetric(MetricDescription metricDesc);

		// Token: 0x060007A8 RID: 1960
		MetricTrackType GetMetricTrackType(MetricDescription metricDesc);

		// Token: 0x060007A9 RID: 1961
		void Shutdown();

		// Token: 0x060007AA RID: 1962
		string MetricDisplayName(Metric m);

		// Token: 0x060007AB RID: 1963
		void StartCapture(MetricDescription metricDesc);
	}
}
