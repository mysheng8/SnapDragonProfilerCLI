using System;

namespace Sdp
{
	// Token: 0x0200009F RID: 159
	public class ClientEvents
	{
		// Token: 0x0400022C RID: 556
		public EventHandler<CaptureWindowEventArgs> WindowVisibilityChanged;

		// Token: 0x0400022D RID: 557
		public EventHandler<SavingSession> SavingChanged;

		// Token: 0x0400022E RID: 558
		public EventHandler<CaptureNameChangedArgs> CaptureNameChanged;

		// Token: 0x0400022F RID: 559
		public EventHandler<CaptureWindowEventArgs> CaptureWindowAdded;

		// Token: 0x04000230 RID: 560
		public EventHandler<EventArgs> AppShutdown;

		// Token: 0x04000231 RID: 561
		public EventHandler<EventArgs> CopyFocusedContent;

		// Token: 0x04000232 RID: 562
		public EventHandler<EventArgs> SelectAllContent;

		// Token: 0x04000233 RID: 563
		public EventHandler<MenuActivatedEventArgs> MenuActivated;

		// Token: 0x04000234 RID: 564
		public EventHandler<ReparentMessageDialogArgs> ReparentMessageDialog;

		// Token: 0x04000235 RID: 565
		public EventHandler<EventArgs> MainWindowShown;
	}
}
