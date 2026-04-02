using System;

namespace Sdp
{
	// Token: 0x020002AF RID: 687
	public class InternalMetricCategoryDelegate : MetricCategoryDelegate
	{
		// Token: 0x06000DD7 RID: 3543 RVA: 0x0002AC3F File Offset: 0x00028E3F
		public InternalMetricCategoryDelegate(ConnectionManager container)
		{
			this.m_container = container;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0002AC50 File Offset: 0x00028E50
		public override void OnMetricCategoryAdded(uint categoryID)
		{
			MetricCategoryAddedEventArgs metricCategoryAddedEventArgs = new MetricCategoryAddedEventArgs();
			metricCategoryAddedEventArgs.MetricCategory = MetricManager.Get().GetMetricCategory(categoryID);
			SdpApp.EventsManager.Raise<MetricCategoryAddedEventArgs>(SdpApp.EventsManager.ConnectionEvents.MetricCategoryAdded, this, metricCategoryAddedEventArgs);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void OnMetricCategoryListReceived()
		{
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void OnMetricCategoryActivated(uint categoryID)
		{
		}

		// Token: 0x0400097C RID: 2428
		private ConnectionManager m_container;
	}
}
