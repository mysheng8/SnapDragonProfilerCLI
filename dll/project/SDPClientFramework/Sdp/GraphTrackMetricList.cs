using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200021C RID: 540
	public class GraphTrackMetricList : List<GraphTrackMetric>
	{
		// Token: 0x06000820 RID: 2080 RVA: 0x00015F94 File Offset: 0x00014194
		public bool ContainsMetric(MetricDesc desc)
		{
			GraphTrackMetric graphTrackMetric = this.FindMetric(desc);
			return graphTrackMetric != null;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00015FB0 File Offset: 0x000141B0
		public GraphTrackMetric FindMetric(MetricDesc desc)
		{
			GraphTrackMetric graphTrackMetric = null;
			foreach (GraphTrackMetric graphTrackMetric2 in this)
			{
				if (desc == graphTrackMetric2.Descriptor)
				{
					graphTrackMetric = graphTrackMetric2;
					break;
				}
			}
			return graphTrackMetric;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001600C File Offset: 0x0001420C
		public GraphTrackMetric FindMetricByID(uint metricId)
		{
			foreach (GraphTrackMetric graphTrackMetric in this)
			{
				if (metricId == graphTrackMetric.MetricId)
				{
					return graphTrackMetric;
				}
			}
			return null;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00016064 File Offset: 0x00014264
		public GraphTrackMetric FindMetricByIDAndPID(uint metricId, uint processId)
		{
			foreach (GraphTrackMetric graphTrackMetric in this)
			{
				if (metricId == graphTrackMetric.MetricId && processId == graphTrackMetric.ProcessId)
				{
					return graphTrackMetric;
				}
			}
			return null;
		}
	}
}
