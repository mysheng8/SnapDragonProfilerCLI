using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sdp
{
	// Token: 0x02000224 RID: 548
	public class TraceSummaryViewController
	{
		// Token: 0x0600085A RID: 2138 RVA: 0x00016A64 File Offset: 0x00014C64
		public TraceSummaryViewController(ITraceSummaryView view, EventsManager eventsManager)
		{
			this.m_view = view;
			this.m_eventsManager = eventsManager;
			ConnectionEvents connectionEvents = eventsManager.ConnectionEvents;
			connectionEvents.DataProcessed = (EventHandler<DataProcessedEventArgs>)Delegate.Combine(connectionEvents.DataProcessed, new EventHandler<DataProcessedEventArgs>(this.connectionEvents_DataProcessed));
			ConnectionEvents connectionEvents2 = eventsManager.ConnectionEvents;
			connectionEvents2.StartCaptureRequest = (EventHandler<TakeCaptureArgs>)Delegate.Combine(connectionEvents2.StartCaptureRequest, new EventHandler<TakeCaptureArgs>(this.connectionEvents_StartCaptureRequest));
			ConnectionEvents connectionEvents3 = eventsManager.ConnectionEvents;
			connectionEvents3.MaxCaptureDurationExpired = (EventHandler<MaxCaptureDurationExpiredEventArgs>)Delegate.Combine(connectionEvents3.MaxCaptureDurationExpired, new EventHandler<MaxCaptureDurationExpiredEventArgs>(this.connectionEvents_MaxCaptureDurationExpired));
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x00016B1B File Offset: 0x00014D1B
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x00016B23 File Offset: 0x00014D23
		public uint CaptureId { get; set; }

		// Token: 0x0600085D RID: 2141 RVA: 0x00016B2C File Offset: 0x00014D2C
		private void connectionEvents_DataProcessed(object sender, DataProcessedEventArgs args)
		{
			if (args.CaptureID != this.CaptureId)
			{
				return;
			}
			uint captureID = args.CaptureID;
			Capture capture = CaptureManager.Get().GetCapture(captureID);
			long startTimeTOD = capture.GetProperties().startTimeTOD;
			long stopTimeTOD = capture.GetProperties().stopTimeTOD;
			this.UpdateGenericInfo<long>("Duration (us)", stopTimeTOD - startTimeTOD, "General Information", "Duration in microseconds");
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00016B8C File Offset: 0x00014D8C
		private void connectionEvents_StartCaptureRequest(object sender, EventArgs e)
		{
			if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController != null)
			{
				uint captureId = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.CaptureId;
				if (captureId != this.CaptureId)
				{
					return;
				}
				List<uint> globalMetrics = SdpApp.ConnectionManager.GetGlobalMetrics();
				List<uint> list = new List<uint>();
				IdNamePair selectedProcess = SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController.SelectedProcess;
				uint? num = ((selectedProcess != null) ? new uint?(selectedProcess.Id) : null);
				if (num != null)
				{
					string name = ProcessManager.Get().GetProcess(num.Value).GetProperties()
						.name;
					this.UpdateGenericInfo<string>("App Name", name.Split(new char[] { '\\', '/' }).Last<string>(), "General Information", "");
					this.UpdateGenericInfo<string>("Full Path", name, "General Information", "");
					list = SdpApp.ConnectionManager.GetMetricsForProcess(num.Value);
				}
				this.UpdateMetrics(globalMetrics, list, num, true);
			}
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00016C98 File Offset: 0x00014E98
		private void connectionEvents_MaxCaptureDurationExpired(object sender, MaxCaptureDurationExpiredEventArgs args)
		{
			if (args.CaptureId != this.CaptureId)
			{
				return;
			}
			if (!args.DataProcessed)
			{
				this.UpdateErrorsAndWarnings("Max capture duration exceeded", "Errors", "No process metrics collected", "The trace ended because the maximum duration expired. No process metrics data have been collected.\nGo to File -> Settings -> Capture -> Maximum Trace capture duration (ms) to change the maximum duration.");
				return;
			}
			if (args.AnyFrameCollected)
			{
				this.UpdateErrorsAndWarnings("Max capture duration exceeded", "Warnings", "Trace ended because the maximum duration expired", string.Format("Frames collected: [{0} - {1}].\nGo to File -> Settings -> Capture -> Maximum Trace capture duration (ms) to change the maximum duration.", args.FirstFrame, args.LastFrame));
				return;
			}
			this.UpdateErrorsAndWarnings("Max capture duration exceeded", "Errors", "No process metrics collected", "The trace ended because the maximum duration expired. No process metrics data have been collected.\nGo to File -> Settings -> Capture -> Maximum Trace capture duration (ms) to change the maximum duration.");
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00016D30 File Offset: 0x00014F30
		private void UpdateGenericInfo<T>(string propertyName, T value, string parent = "", string description = "")
		{
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<T>(propertyName, typeof(T), value, parent, description, false);
			this.m_genericInfoProperties[propertyName] = propertyDescriptor;
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			propertyGridDescriptionObject.AddPropertyGridDescriptors(this.m_genericInfoProperties.Values.ToList<PropertyDescriptor>());
			this.m_view.SetGenericInfo(propertyGridDescriptionObject);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00016D88 File Offset: 0x00014F88
		private void UpdateMetrics(List<uint> globalMetrics, List<uint> processMetrics, uint? processId, bool checkMetricActive = true)
		{
			Action<List<uint>, uint, string> action = delegate(List<uint> metrics, uint pid, string category)
			{
				foreach (uint num in metrics)
				{
					Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(num);
					if (!checkMetricActive || (checkMetricActive && metricByID.IsActive(pid, 2U)))
					{
						MetricProperties properties = metricByID.GetProperties();
						string name = MetricManager.Get().GetMetricCategory(properties.categoryID).GetProperties()
							.name;
						string text = ((name != "") ? ("Category: " + name + "\n") : "") + ((properties.description != "Uninitialized") ? properties.description : "");
						PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<string>(properties.name, typeof(LabelOnlyAttribute), null, category, text, false);
						this.m_metrics[num] = propertyDescriptor;
					}
				}
			};
			action(globalMetrics, uint.MaxValue, "Global Metrics");
			if (processId != null)
			{
				action(processMetrics, processId.Value, "Process Metrics");
			}
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			propertyGridDescriptionObject.AddPropertyGridDescriptors(this.m_metrics.Values.ToList<PropertyDescriptor>());
			this.m_view.SetMetrics(propertyGridDescriptionObject);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00016E08 File Offset: 0x00015008
		private void UpdateErrorsAndWarnings(string id, string category, string description, string tooltip)
		{
			PropertyDescriptor propertyDescriptor = new SdpPropertyDescriptor<string>(description, typeof(LabelOnlyAttribute), null, category, tooltip, false);
			this.m_errorsWarnings[id] = propertyDescriptor;
			PropertyGridDescriptionObject propertyGridDescriptionObject = new PropertyGridDescriptionObject();
			propertyGridDescriptionObject.AddPropertyGridDescriptors(this.m_errorsWarnings.Values.ToList<PropertyDescriptor>());
			this.m_view.SetErrorsAndWarnings(propertyGridDescriptionObject);
		}

		// Token: 0x040007CB RID: 1995
		private ITraceSummaryView m_view;

		// Token: 0x040007CC RID: 1996
		private Dictionary<string, PropertyDescriptor> m_genericInfoProperties = new Dictionary<string, PropertyDescriptor>();

		// Token: 0x040007CD RID: 1997
		private Dictionary<uint, PropertyDescriptor> m_metrics = new Dictionary<uint, PropertyDescriptor>();

		// Token: 0x040007CE RID: 1998
		private Dictionary<string, PropertyDescriptor> m_errorsWarnings = new Dictionary<string, PropertyDescriptor>();

		// Token: 0x040007CF RID: 1999
		private EventsManager m_eventsManager;
	}
}
