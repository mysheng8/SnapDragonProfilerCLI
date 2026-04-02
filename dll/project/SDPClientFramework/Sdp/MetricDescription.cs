using System;

namespace Sdp
{
	// Token: 0x02000208 RID: 520
	public class MetricDescription
	{
		// Token: 0x060007AC RID: 1964 RVA: 0x00014DF4 File Offset: 0x00012FF4
		public MetricDescription(Metric metric)
		{
			this.Id = metric.GetProperties().id;
			this.Name = metric.GetProperties().name;
			this.CategoryId = metric.GetProperties().categoryID;
			MetricCategory metricCategory = MetricManager.Get().GetMetricCategory(this.CategoryId);
			if (metricCategory != null)
			{
				this.CategoryName = metricCategory.GetProperties().name;
				return;
			}
			this.CategoryName = "";
		}

		// Token: 0x04000757 RID: 1879
		public uint Id;

		// Token: 0x04000758 RID: 1880
		public string Name;

		// Token: 0x04000759 RID: 1881
		public uint CategoryId;

		// Token: 0x0400075A RID: 1882
		public string CategoryName;
	}
}
