using System;

namespace Sdp
{
	// Token: 0x0200006A RID: 106
	public class NewSnapshotWindowCommand : Command
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00008254 File Offset: 0x00006454
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000825C File Offset: 0x0000645C
		public uint CaptureID { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00008265 File Offset: 0x00006465
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000826D File Offset: 0x0000646D
		public string Name { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00008276 File Offset: 0x00006476
		// (set) Token: 0x06000260 RID: 608 RVA: 0x0000827E File Offset: 0x0000647E
		public SnapshotController Result { get; set; }

		// Token: 0x06000261 RID: 609 RVA: 0x00008288 File Offset: 0x00006488
		protected override void OnExecute()
		{
			string text = this.Name;
			int num = 1;
			while (SdpApp.UIManager.ContainsWindow(text))
			{
				text = this.Name + " (" + num.ToString() + ")";
				num++;
			}
			IViewController viewController = SdpApp.UIManager.CreateCaptureWindowTabbedWith(text, "Snapshot", "Start Page", true, this.Layout, true);
			SnapshotController snapshotController = viewController as SnapshotController;
			if (snapshotController != null)
			{
				snapshotController.WindowName = text;
				snapshotController.CaptureId = this.CaptureID;
			}
			SdpApp.AnalyticsManager.TrackWindow(CaptureType.Snapshot);
			this.Result = snapshotController;
		}

		// Token: 0x0400018E RID: 398
		public string Layout = "Guide";
	}
}
