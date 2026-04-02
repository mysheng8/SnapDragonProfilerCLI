using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sdp
{
	// Token: 0x020002AE RID: 686
	public class InternalMetricDelegate : MetricDelegate
	{
		// Token: 0x06000DCF RID: 3535 RVA: 0x0002A8B5 File Offset: 0x00028AB5
		public InternalMetricDelegate(ConnectionManager container)
		{
			this.m_container = container;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0002A8C4 File Offset: 0x00028AC4
		public override void OnMetricAdded(uint metricID)
		{
			MetricAddedEventArgs metricAddedEventArgs = new MetricAddedEventArgs();
			metricAddedEventArgs.Metric = MetricManager.Get().GetMetric(metricID);
			if (metricAddedEventArgs.Metric == null || !metricAddedEventArgs.Metric.IsValid())
			{
				return;
			}
			this.CheckMetricSupportsCapture(metricAddedEventArgs.Metric);
			if (!this.m_container.RegisteredMetricsList.Contains(metricAddedEventArgs.Metric.GetProperties().id))
			{
				this.m_container.RegisteredMetricsList.Add(metricAddedEventArgs.Metric.GetProperties().id);
			}
			MetricCategory metricCategory = MetricManager.Get().GetMetricCategory(metricAddedEventArgs.Metric.GetProperties().categoryID);
			if (metricCategory != null)
			{
				SdpApp.ModelManager.ConnectionModel.AddMetricCategoryIdToMetricDictionary(metricCategory.GetProperties().id);
			}
			SdpApp.EventsManager.Raise<MetricAddedEventArgs>(SdpApp.EventsManager.ConnectionEvents.MetricAdded, this, metricAddedEventArgs);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0002A9A0 File Offset: 0x00028BA0
		public void CheckMetricSupportsCapture(Metric metric)
		{
			MetricProperties properties = metric.GetProperties();
			uint captureTypeMask = properties.captureTypeMask;
			if ((captureTypeMask & 1U) != 0U)
			{
				this.m_container.AddSupportedCaptureType(CaptureType.Realtime, properties.name, properties.userData);
			}
			if ((captureTypeMask & 2U) != 0U)
			{
				this.m_container.AddSupportedCaptureType(CaptureType.Trace, properties.name, properties.userData);
			}
			if ((captureTypeMask & 4U) != 0U)
			{
				this.m_container.AddSupportedCaptureType(CaptureType.Snapshot, properties.name, properties.userData);
			}
			if ((captureTypeMask & 8U) != 0U)
			{
				this.m_container.AddSupportedCaptureType(CaptureType.Sampling, properties.name, properties.userData);
			}
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002AA30 File Offset: 0x00028C30
		public override void OnMetricListReceived(uint providerID)
		{
			MetricList metricsByProvider = MetricManager.Get().GetMetricsByProvider(providerID);
			if (metricsByProvider != null)
			{
				List<uint> list = new List<uint>();
				foreach (Metric metric in metricsByProvider)
				{
					if (metric != null)
					{
						list.Add(metric.GetProperties().id);
						Delegate @delegate;
						if (!this.m_container.RegisteredMetricsList.Contains(metric.GetProperties().id) && this.m_container.DelegateDictionary != null && this.m_container.DelegateDictionary.TryGetValue((SDPDataType)metric.GetProperties().type, out @delegate))
						{
							IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate(@delegate);
							if (functionPointerForDelegate != IntPtr.Zero)
							{
								this.m_container.RegisteredMetricsList.Add(metric.GetProperties().id);
							}
						}
					}
				}
				SdpApp.ModelManager.ConnectionModel.AddMetricCategoryListToMetricDictionary(list);
				MetricDictionaryChangedArgs metricDictionaryChangedArgs = new MetricDictionaryChangedArgs();
				metricDictionaryChangedArgs.Added = metricsByProvider;
				SdpApp.EventsManager.Raise<MetricDictionaryChangedArgs>(SdpApp.EventsManager.ConnectionEvents.MetricDictionaryChanged, this, metricDictionaryChangedArgs);
			}
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void OnMetricActivated(uint metricID, uint pid)
		{
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void OnMetricDeactivated(uint metricID, uint pid)
		{
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0002AB60 File Offset: 0x00028D60
		public override void OnMetricHiddenToggled(uint metricID, bool hidden)
		{
			MetricHiddenToggledEventArgs metricHiddenToggledEventArgs = new MetricHiddenToggledEventArgs();
			metricHiddenToggledEventArgs.MetricID = metricID;
			metricHiddenToggledEventArgs.Hidden = hidden;
			SdpApp.EventsManager.Raise<MetricHiddenToggledEventArgs>(SdpApp.EventsManager.ConnectionEvents.MetricHiddenToggled, this, metricHiddenToggledEventArgs);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0002AB9C File Offset: 0x00028D9C
		public override void OnMetricDataReceived(uint metricID, uint pid, uint tid, long timestamp, double value)
		{
			if (!this.m_container.haveFirst)
			{
				this.m_container.firstTimestamp = (ulong)timestamp;
				this.m_container.haveFirst = true;
			}
			Metric metric = MetricManager.Get().GetMetric(metricID);
			uint num = 0U;
			if (metric != null)
			{
				num = metric.GetProperties().providerID;
				SdpApp.ModelManager.ConnectionModel.AddData(new DoubleData(metric, pid, (ulong)(timestamp - (long)this.m_container.firstTimestamp), value));
			}
			MetricDataReceivedEventArgs metricDataReceivedEventArgs = new MetricDataReceivedEventArgs(new DoubleData(metric, pid, (ulong)timestamp, value), num);
			SdpApp.EventsManager.Raise<MetricDataReceivedEventArgs>(SdpApp.EventsManager.MetricEvents.MetricDataReceived, this, metricDataReceivedEventArgs);
		}

		// Token: 0x0400097B RID: 2427
		private ConnectionManager m_container;
	}
}
