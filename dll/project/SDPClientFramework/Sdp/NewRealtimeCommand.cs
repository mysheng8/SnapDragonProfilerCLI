using System;

namespace Sdp
{
	// Token: 0x02000066 RID: 102
	internal class NewRealtimeCommand : Command
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00007E27 File Offset: 0x00006027
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00007E2F File Offset: 0x0000602F
		public uint CaptureID { get; set; }

		// Token: 0x0600024C RID: 588 RVA: 0x00007E38 File Offset: 0x00006038
		protected override void OnExecute()
		{
			SdpApp.Platform.Invoke(delegate
			{
				int nextRealtimeCaptureNumber = SdpApp.ModelManager.RealtimeModel.GetNextRealtimeCaptureNumber();
				string text = "Realtime " + nextRealtimeCaptureNumber.ToString();
				IViewController viewController = SdpApp.UIManager.CreateCaptureWindowTabbedWith(text, "GroupLayoutView", "Start Page", false, "Realtime", false);
				GroupLayoutController groupLayoutController = viewController as GroupLayoutController;
				if (groupLayoutController != null)
				{
					groupLayoutController.WindowName = text;
					groupLayoutController.CaptureType = CaptureType.Realtime;
					IGroupLayoutView groupLayoutView = viewController.View as IGroupLayoutView;
					groupLayoutView.SetName(CaptureType.Realtime, nextRealtimeCaptureNumber);
					SdpApp.ModelManager.TimeModelCollection.TimeModels.Add(this.CaptureID, new TimeModel(this.CaptureID));
					SdpApp.EventsManager.TimeEventsCollection.TimeEvents.Add(this.CaptureID, new TimeEvents());
					groupLayoutController.CaptureId = this.CaptureID;
					SdpApp.ModelManager.RealtimeModel.CurrentGroupLayoutController = groupLayoutController;
				}
			});
		}
	}
}
