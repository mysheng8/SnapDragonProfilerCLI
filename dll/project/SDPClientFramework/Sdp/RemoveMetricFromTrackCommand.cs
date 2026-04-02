using System;

namespace Sdp
{
	// Token: 0x0200007E RID: 126
	public class RemoveMetricFromTrackCommand : Command
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00008DA7 File Offset: 0x00006FA7
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x00008DAF File Offset: 0x00006FAF
		public TrackControllerBase Track { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x00008DB8 File Offset: 0x00006FB8
		// (set) Token: 0x060002BA RID: 698 RVA: 0x00008DC0 File Offset: 0x00006FC0
		public uint MetricId { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00008DC9 File Offset: 0x00006FC9
		// (set) Token: 0x060002BC RID: 700 RVA: 0x00008DD1 File Offset: 0x00006FD1
		public string MetricName { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00008DDA File Offset: 0x00006FDA
		// (set) Token: 0x060002BE RID: 702 RVA: 0x00008DE2 File Offset: 0x00006FE2
		public uint PID { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002BF RID: 703 RVA: 0x00008DEB File Offset: 0x00006FEB
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x00008DF3 File Offset: 0x00006FF3
		public bool ForceDeleteTrackIfEmpty { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x00008DFC File Offset: 0x00006FFC
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x00008E04 File Offset: 0x00007004
		public bool IsPreview { get; set; }

		// Token: 0x060002C3 RID: 707 RVA: 0x00008E10 File Offset: 0x00007010
		protected override void OnExecute()
		{
			if (this.Track != null)
			{
				this.Track.RemoveMetric(this.MetricId, this.MetricName, this.PID, this.ForceDeleteTrackIfEmpty, this.IsPreview);
				if (!this.IsPreview)
				{
					EnableMetricEventArgs enableMetricEventArgs = new EnableMetricEventArgs();
					enableMetricEventArgs.CaptureId = this.Track.CaptureId;
					enableMetricEventArgs.Enable = false;
					enableMetricEventArgs.PID = this.PID;
					enableMetricEventArgs.MetricId = this.MetricId;
					SdpApp.EventsManager.Raise<EnableMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.EnableMetric, this, enableMetricEventArgs);
				}
			}
		}
	}
}
