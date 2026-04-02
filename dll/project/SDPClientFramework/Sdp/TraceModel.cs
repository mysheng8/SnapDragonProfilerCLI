using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x0200021F RID: 543
	public class TraceModel
	{
		// Token: 0x06000841 RID: 2113 RVA: 0x000163FC File Offset: 0x000145FC
		public TraceModel()
		{
			this.CurrentSources = new Dictionary<IdNamePair, List<IdNamePair>>(new IdNamePairComparer());
			this.CurrentCaptureSettings = new TraceCaptureSettings();
			this.m_nextTraceCaptureNumber = 1;
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.StartCaptureRequest = (EventHandler<TakeCaptureArgs>)Delegate.Combine(connectionEvents.StartCaptureRequest, new EventHandler<TakeCaptureArgs>(this.connectionEvents_StartCaptureRequest));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.EnableMetric = (EventHandler<EnableMetricEventArgs>)Delegate.Combine(connectionEvents2.EnableMetric, new EventHandler<EnableMetricEventArgs>(this.connectionEvents_EnableMetric));
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x000164A8 File Offset: 0x000146A8
		public long NormalizeTimestamp(long ts)
		{
			if (SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController != null)
			{
				return ts - this.GetFirstTimestamp(this.CurrentCaptureGroupLayoutController.CaptureId);
			}
			return 0L;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000164D1 File Offset: 0x000146D1
		public long NormalizeTimestamp(uint captureId, long ts)
		{
			return ts - this.GetFirstTimestamp(captureId);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x000164DC File Offset: 0x000146DC
		public int NextTraceCaptureNumber()
		{
			int num = this.m_nextTraceCaptureNumber + 1;
			this.m_nextTraceCaptureNumber = num;
			return num - 1;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x000164FC File Offset: 0x000146FC
		private long GetFirstTimestamp(uint captureId)
		{
			return this.m_cachedStartTimes.GetOrAdd(captureId, (uint id) => SdpApp.ConnectionManager.GetFirstTimestampTOD(id));
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001652C File Offset: 0x0001472C
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

		// Token: 0x06000847 RID: 2119 RVA: 0x000165CC File Offset: 0x000147CC
		private void connectionEvents_EnableMetric(object sender, EnableMetricEventArgs e)
		{
			if (e.Mode == CaptureType.Trace)
			{
				if (e.Enable)
				{
					this.m_currentCaptureMetrics.Add(new Tuple<uint, uint>(e.PID, e.MetricId));
					return;
				}
				this.m_currentCaptureMetrics.Remove(new Tuple<uint, uint>(e.PID, e.MetricId));
			}
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00016624 File Offset: 0x00014824
		private void connectionEvents_StartCaptureRequest(object sender, EventArgs e)
		{
			Thread thread = new Thread(delegate
			{
				this.RecordMetrics();
			});
			thread.Start();
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001664C File Offset: 0x0001484C
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
					modelObjectData.SetAttributeValue("captureID", this.CurrentCaptureGroupLayoutController.CaptureId.ToString());
					modelObjectData.SetAttributeValue("processID", tuple.Item1.ToString());
					modelObjectData.SetAttributeValue("metricID", tuple.Item2.ToString());
					modelObjectData.Save();
				}
			}
			MetricsRecordedForCaptureArgs metricsRecordedForCaptureArgs = new MetricsRecordedForCaptureArgs();
			metricsRecordedForCaptureArgs.CaptureID = this.CurrentCaptureGroupLayoutController.CaptureId;
			metricsRecordedForCaptureArgs.Mode = CaptureType.Trace;
			SdpApp.EventsManager.Raise<MetricsRecordedForCaptureArgs>(SdpApp.EventsManager.ConnectionEvents.MetricsRecordedForCapture, this, metricsRecordedForCaptureArgs);
		}

		// Token: 0x040007B4 RID: 1972
		private readonly ConcurrentDictionary<uint, long> m_cachedStartTimes = new ConcurrentDictionary<uint, long>();

		// Token: 0x040007B5 RID: 1973
		public Dictionary<IdNamePair, List<IdNamePair>> CurrentSources;

		// Token: 0x040007B6 RID: 1974
		public TraceCaptureSettings CurrentCaptureSettings;

		// Token: 0x040007B7 RID: 1975
		public GroupLayoutController CurrentCaptureGroupLayoutController;

		// Token: 0x040007B8 RID: 1976
		public readonly Dictionary<int, GroupLayoutController> GroupLayoutControllers = new Dictionary<int, GroupLayoutController>();

		// Token: 0x040007B9 RID: 1977
		private int m_nextTraceCaptureNumber;

		// Token: 0x040007BA RID: 1978
		private List<Tuple<uint, uint>> m_currentCaptureMetrics = new List<Tuple<uint, uint>>();
	}
}
