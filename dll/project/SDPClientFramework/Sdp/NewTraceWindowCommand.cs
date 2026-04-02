using System;

namespace Sdp
{
	// Token: 0x0200006B RID: 107
	public class NewTraceWindowCommand : Command
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000832E File Offset: 0x0000652E
		// (set) Token: 0x06000264 RID: 612 RVA: 0x00008336 File Offset: 0x00006536
		public uint CaptureID { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000833F File Offset: 0x0000653F
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00008347 File Offset: 0x00006547
		public string Name { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00008350 File Offset: 0x00006550
		// (set) Token: 0x06000268 RID: 616 RVA: 0x00008358 File Offset: 0x00006558
		public GroupLayoutController Result { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00008361 File Offset: 0x00006561
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00008369 File Offset: 0x00006569
		public string SDPVersion { get; set; } = SdpApp.ModelManager.ApplicationModel.Version;

		// Token: 0x0600026B RID: 619 RVA: 0x00008374 File Offset: 0x00006574
		protected override void OnExecute()
		{
			string text = this.Name;
			int num = 1;
			while (SdpApp.UIManager.ContainsWindow(text))
			{
				text = this.Name + " (" + num.ToString() + ")";
				num++;
			}
			IViewController viewController = SdpApp.UIManager.CreateCaptureWindowTabbedWith(text, NewTraceWindowCommand.TypeName, "Start Page", true, this.Layout, true);
			GroupLayoutController groupLayoutController = viewController as GroupLayoutController;
			if (groupLayoutController != null)
			{
				groupLayoutController.WindowName = text;
				this.Name = text;
				groupLayoutController.CaptureType = SdpApp.UIManager.GetCaptureTypeForLayout(NewTraceWindowCommand.TypeName, this.Layout);
				groupLayoutController.SDPVersion = this.SDPVersion;
				SdpApp.ModelManager.TimeModelCollection.TimeModels.Add(this.CaptureID, new TimeModel(this.CaptureID));
				SdpApp.EventsManager.TimeEventsCollection.TimeEvents.Add(this.CaptureID, new TimeEvents());
				groupLayoutController.CaptureId = this.CaptureID;
			}
			SdpApp.AnalyticsManager.TrackWindow(CaptureType.Trace);
			this.Result = groupLayoutController;
		}

		// Token: 0x04000193 RID: 403
		public string Layout = "Guide";

		// Token: 0x04000194 RID: 404
		private static string TypeName = "GroupLayoutView";
	}
}
