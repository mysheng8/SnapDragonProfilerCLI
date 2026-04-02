using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000085 RID: 133
	public class SnapshotCommand : Command
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x000091C3 File Offset: 0x000073C3
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x000091CB File Offset: 0x000073CB
		public bool StartCapture { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x000091D4 File Offset: 0x000073D4
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x000091DC File Offset: 0x000073DC
		public uint PID { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000091E5 File Offset: 0x000073E5
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x000091ED File Offset: 0x000073ED
		public Metric SnapshotMetric { get; set; }

		// Token: 0x060002F8 RID: 760 RVA: 0x000091F8 File Offset: 0x000073F8
		private void RequestSnapshotForMetric(Metric metric)
		{
			SdpApp.ConnectionManager.CurrentRendererString = SdpApp.ConnectionManager.ConnectedDeviceRendererString;
			IDList activeProcesses = metric.GetActiveProcesses(4U);
			foreach (uint num in activeProcesses)
			{
				if (num != this.PID)
				{
					metric.Deactivate(num, 4U);
				}
			}
			metric.Activate(this.PID, 4U);
			SdpApp.EventsManager.Raise(SdpApp.EventsManager.ConnectionEvents.SnapshotRequest, this, EventArgs.Empty);
			if (SdpApp.ModelManager.SnapshotModel.CurrentSources != null && SdpApp.ModelManager.SnapshotModel.CurrentSources.Count > 0)
			{
				foreach (KeyValuePair<IdNamePair, List<IdNamePair>> keyValuePair in SdpApp.ModelManager.SnapshotModel.CurrentSources)
				{
					IdNamePair key = keyValuePair.Key;
					List<IdNamePair> value = keyValuePair.Value;
					IdNamePair selectedProcess = SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.SelectedProcess;
					if (value.Exists((IdNamePair x) => x.Id == selectedProcess.Id && x.Name == selectedProcess.Name))
					{
						Metric metricByID = SdpApp.ConnectionManager.GetMetricByID(key.Id);
						MetricDescription metricDescription = new MetricDescription(metricByID);
						IMetricPlugin metricPlugin = SdpApp.PluginManager.GetMetricPlugin(metricDescription);
						if (metricPlugin != null)
						{
							metricPlugin.StartCapture(metricDescription);
						}
					}
				}
			}
			SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.AlreadyCaptured = true;
			metric.Deactivate(this.PID, 4U);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000093B4 File Offset: 0x000075B4
		protected override void OnExecute()
		{
			Process process = ProcessManager.Get().GetProcess(this.PID);
			if (process != null && process.IsValid())
			{
				this.RequestSnapshotForMetric(this.SnapshotMetric);
			}
		}
	}
}
