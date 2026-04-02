using System;
using System.Collections.Generic;
using System.Threading;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x020001A2 RID: 418
	public class SamplingModel
	{
		// Token: 0x06000517 RID: 1303 RVA: 0x0000B90C File Offset: 0x00009B0C
		public SamplingModel()
		{
			this.CaptureDockWindows = new List<IDockWindow>();
			this.m_nextSamplingNumber = 1;
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.StartSamplingRequest = (EventHandler)Delegate.Combine(connectionEvents.StartSamplingRequest, new EventHandler(this.connectionEvents_StartSamplingRequest));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.EnableMetric = (EventHandler<EnableMetricEventArgs>)Delegate.Combine(connectionEvents2.EnableMetric, new EventHandler<EnableMetricEventArgs>(this.connectionEvents_EnableMetric));
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000B9B0 File Offset: 0x00009BB0
		public int NextSamplingNumber()
		{
			int nextSamplingNumber = this.m_nextSamplingNumber;
			this.m_nextSamplingNumber = nextSamplingNumber + 1;
			return nextSamplingNumber;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000B9D0 File Offset: 0x00009BD0
		public HashSet<uint> GetActiveMetricsForCapture(uint captureID)
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("CaptureManager");
			ModelObject modelObject = model.GetModelObject("CaptureMetrics");
			ModelObjectDataList modelObjectData = dataModel.GetModelObjectData(modelObject, "captureID", captureID.ToString());
			HashSet<uint> hashSet = new HashSet<uint>();
			foreach (ModelObjectData modelObjectData2 in modelObjectData)
			{
				hashSet.Add(UintConverter.Convert(modelObjectData2.GetValue("metricID")));
			}
			return hashSet;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000BA70 File Offset: 0x00009C70
		private void connectionEvents_EnableMetric(object sender, EnableMetricEventArgs e)
		{
			if (e.Mode == CaptureType.Sampling)
			{
				if (e.Enable)
				{
					this.m_currentCaptureMetrics.Add(new Tuple<uint, uint>(e.PID, e.MetricId));
					return;
				}
				this.m_currentCaptureMetrics.Remove(new Tuple<uint, uint>(e.PID, e.MetricId));
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000BAC8 File Offset: 0x00009CC8
		private void connectionEvents_StartSamplingRequest(object sender, EventArgs e)
		{
			Thread thread = new Thread(delegate
			{
				this.RecordMetrics();
			});
			thread.Start();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0000BAF0 File Offset: 0x00009CF0
		private void RecordMetrics()
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("CaptureManager");
			ModelObject modelObject = model.GetModelObject("CaptureMetrics");
			foreach (Tuple<uint, uint> tuple in this.m_currentCaptureMetrics)
			{
				Process process = ProcessManager.Get().GetProcess(tuple.Item1);
				if (tuple.Item1 == 4294967295U || (process != null && process.IsValid()))
				{
					ModelObjectData modelObjectData = modelObject.NewData();
					modelObjectData.SetAttributeValue("captureID", this.CurrentSamplingController.CaptureId.ToString());
					modelObjectData.SetAttributeValue("processID", tuple.Item1.ToString());
					modelObjectData.SetAttributeValue("metricID", tuple.Item2.ToString());
					modelObjectData.Save();
				}
			}
			MetricsRecordedForCaptureArgs metricsRecordedForCaptureArgs = new MetricsRecordedForCaptureArgs();
			metricsRecordedForCaptureArgs.CaptureID = this.CurrentSamplingController.CaptureId;
			metricsRecordedForCaptureArgs.Mode = CaptureType.Sampling;
			SdpApp.EventsManager.Raise<MetricsRecordedForCaptureArgs>(SdpApp.EventsManager.ConnectionEvents.MetricsRecordedForCapture, this, metricsRecordedForCaptureArgs);
		}

		// Token: 0x04000636 RID: 1590
		private int m_nextSamplingNumber;

		// Token: 0x04000637 RID: 1591
		public Dictionary<IdNamePair, List<IdNamePair>> CurrentSources = new Dictionary<IdNamePair, List<IdNamePair>>(new IdNamePairComparer());

		// Token: 0x04000638 RID: 1592
		public SamplingController CurrentSamplingController;

		// Token: 0x04000639 RID: 1593
		public readonly Dictionary<int, SamplingController> SamplingControllers = new Dictionary<int, SamplingController>();

		// Token: 0x0400063A RID: 1594
		public List<IDockWindow> CaptureDockWindows;

		// Token: 0x0400063B RID: 1595
		private List<Tuple<uint, uint>> m_currentCaptureMetrics = new List<Tuple<uint, uint>>();
	}
}
