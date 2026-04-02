using System;

namespace Sdp
{
	// Token: 0x0200006F RID: 111
	internal class RemovePreviewMetricsCommand : Command
	{
		// Token: 0x06000274 RID: 628 RVA: 0x00008558 File Offset: 0x00006758
		protected override void OnExecute()
		{
			object previewLock = this.PreviewLock;
			lock (previewLock)
			{
				foreach (PreviewMetricsModel.MetricPair metricPair in SdpApp.ModelManager.PreviewMetricsModel.Metrics)
				{
					SdpApp.ExecuteCommand(new RemoveMetricFromTrackCommand
					{
						MetricId = metricPair.MetricID,
						PID = metricPair.ProcessID,
						Track = this.Controller.GetMetricTrack(MetricDesc.CreateMetricDesc(metricPair.MetricID, metricPair.ProcessID, true)),
						ForceDeleteTrackIfEmpty = true,
						IsPreview = true
					});
				}
				SdpApp.ModelManager.PreviewMetricsModel.Track = null;
			}
		}

		// Token: 0x04000196 RID: 406
		public GroupLayoutController Controller;

		// Token: 0x04000197 RID: 407
		public object PreviewLock = new object();
	}
}
