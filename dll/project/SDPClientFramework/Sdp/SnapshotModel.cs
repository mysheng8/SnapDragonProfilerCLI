using System;
using System.Collections.Generic;
using System.Threading;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x020001A3 RID: 419
	public class SnapshotModel
	{
		// Token: 0x0600051E RID: 1310 RVA: 0x0000BC40 File Offset: 0x00009E40
		public SnapshotModel()
		{
			this.CurrentSources = new Dictionary<IdNamePair, List<IdNamePair>>(new IdNamePairComparer());
			this.CaptureDockWindows = new List<IDockWindow>();
			this.m_nextSnapshotNumber = 1;
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.SnapshotRequest = (EventHandler)Delegate.Combine(connectionEvents.SnapshotRequest, new EventHandler(this.connectionEvents_SnapshotRequest));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.EnableMetric = (EventHandler<EnableMetricEventArgs>)Delegate.Combine(connectionEvents2.EnableMetric, new EventHandler<EnableMetricEventArgs>(this.connectionEvents_EnableMetric));
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000BCEC File Offset: 0x00009EEC
		public int NextSnapshotNumber()
		{
			int num = this.m_nextSnapshotNumber + 1;
			this.m_nextSnapshotNumber = num;
			return num - 1;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000BD0C File Offset: 0x00009F0C
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

		// Token: 0x06000521 RID: 1313 RVA: 0x0000BDAC File Offset: 0x00009FAC
		private void connectionEvents_EnableMetric(object sender, EnableMetricEventArgs e)
		{
			if (e.Mode == CaptureType.Snapshot)
			{
				if (e.Enable)
				{
					this.m_currentCaptureMetrics.Add(new Tuple<uint, uint>(e.PID, e.MetricId));
					return;
				}
				this.m_currentCaptureMetrics.Remove(new Tuple<uint, uint>(e.PID, e.MetricId));
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000BE08 File Offset: 0x0000A008
		private void connectionEvents_SnapshotRequest(object sender, EventArgs e)
		{
			Thread thread = new Thread(delegate
			{
				this.RecordMetrics();
			});
			thread.Start();
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000BE30 File Offset: 0x0000A030
		private void RecordMetrics()
		{
			DataModel dataModel = SdpApp.ConnectionManager.GetDataModel();
			Model model = dataModel.GetModel("CaptureManager");
			ModelObject modelObject = model.GetModelObject("CaptureMetrics");
			foreach (Tuple<uint, uint> tuple in this.m_currentCaptureMetrics)
			{
				if (tuple.Item1 == 4294967295U || tuple.Item1 == this.CurrentSnapshotController.SelectedProcess.Id)
				{
					ModelObjectData modelObjectData = modelObject.NewData();
					modelObjectData.SetAttributeValue("captureID", this.CurrentSnapshotController.CaptureId.ToString());
					modelObjectData.SetAttributeValue("processID", tuple.Item1.ToString());
					modelObjectData.SetAttributeValue("metricID", tuple.Item2.ToString());
					modelObjectData.Save();
				}
			}
			MetricsRecordedForCaptureArgs metricsRecordedForCaptureArgs = new MetricsRecordedForCaptureArgs();
			metricsRecordedForCaptureArgs.CaptureID = this.CurrentSnapshotController.CaptureId;
			metricsRecordedForCaptureArgs.Mode = CaptureType.Snapshot;
			SdpApp.EventsManager.Raise<MetricsRecordedForCaptureArgs>(SdpApp.EventsManager.ConnectionEvents.MetricsRecordedForCapture, this, metricsRecordedForCaptureArgs);
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0000BF70 File Offset: 0x0000A170
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x0000BF78 File Offset: 0x0000A178
		public SnapshotController CurrentSnapshotController
		{
			get
			{
				return this.m_currentSnapshotController;
			}
			set
			{
				this.m_currentSnapshotController = value;
				SdpApp.EventsManager.Raise(SdpApp.EventsManager.SnapshotEvents.CurrentSnapshotControllerChanged, this, EventArgs.Empty);
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x0000BFA8 File Offset: 0x0000A1A8
		public int NumSnapshotHandlers
		{
			get
			{
				return this.m_numSnapshotHandlers;
			}
			set
			{
				this.m_numSnapshotHandlers = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0000BFB1 File Offset: 0x0000A1B1
		public bool IsFirstSnapshot
		{
			get
			{
				return this.m_nextSnapshotNumber == 2;
			}
		}

		// Token: 0x0400063C RID: 1596
		public Dictionary<uint, string> DataFilenames = new Dictionary<uint, string>();

		// Token: 0x0400063D RID: 1597
		public Dictionary<uint, string> StrippedDataFilenames = new Dictionary<uint, string>();

		// Token: 0x0400063E RID: 1598
		private int m_nextSnapshotNumber;

		// Token: 0x0400063F RID: 1599
		public Dictionary<IdNamePair, List<IdNamePair>> CurrentSources;

		// Token: 0x04000640 RID: 1600
		public List<IDockWindow> CaptureDockWindows;

		// Token: 0x04000641 RID: 1601
		private HashSet<Tuple<uint, uint>> m_currentCaptureMetrics = new HashSet<Tuple<uint, uint>>();

		// Token: 0x04000642 RID: 1602
		private SnapshotController m_currentSnapshotController;

		// Token: 0x04000643 RID: 1603
		private int m_numSnapshotHandlers;
	}
}
