using System;

namespace Sdp
{
	// Token: 0x02000116 RID: 278
	public class ScreenCaptureViewEvents
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x00009DFC File Offset: 0x00007FFC
		public ScreenCaptureViewEvents()
		{
			this.DisplayScreenCapture = (EventHandler<ScreenCaptureViewDisplayEventArgs>)Delegate.Combine(this.DisplayScreenCapture, new EventHandler<ScreenCaptureViewDisplayEventArgs>(this.SetVisible));
			this.Invalidate = (EventHandler<ScreenCaptureViewInvalidateEventArgs>)Delegate.Combine(this.Invalidate, new EventHandler<ScreenCaptureViewInvalidateEventArgs>(this.SetVisible));
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00009E53 File Offset: 0x00008053
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.FocusCaptureWindow(SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.WindowName, "");
		}

		// Token: 0x040003E0 RID: 992
		public EventHandler<ScreenCaptureViewDisplayEventArgs> DisplayScreenCapture;

		// Token: 0x040003E1 RID: 993
		public EventHandler<DrawModeChangedEventArgs> DrawModeChanged;

		// Token: 0x040003E2 RID: 994
		public EventHandler<BinningInfoAddedEventArgs> BinningInfoAdded;

		// Token: 0x040003E3 RID: 995
		public EventHandler<DrawBuffersSetEventArgs> DrawBuffersSet;

		// Token: 0x040003E4 RID: 996
		public EventHandler<LocationSelectedEventArgs> LocationSelected;

		// Token: 0x040003E5 RID: 997
		public EventHandler<DisableReplayEventArgs> DisableReplay;

		// Token: 0x040003E6 RID: 998
		public EventHandler<SelectDrawModeEventArgs> SelectDrawMode;

		// Token: 0x040003E7 RID: 999
		public EventHandler<SetContextEventArgs> SetContext;

		// Token: 0x040003E8 RID: 1000
		public EventHandler<SetSurfaceWidthEventArgs> SetWidth;

		// Token: 0x040003E9 RID: 1001
		public EventHandler<SetSurfaceHeightEventArgs> SetHeight;

		// Token: 0x040003EA RID: 1002
		public EventHandler<ScreenCaptureViewInvalidateEventArgs> Invalidate;

		// Token: 0x040003EB RID: 1003
		public EventHandler<ScreenCaptureViewToolbarConfigEventArgs> ToolbarConfig;

		// Token: 0x040003EC RID: 1004
		public EventHandler<EnableEventArgs> EnablePicking;

		// Token: 0x040003ED RID: 1005
		public const string SCREEN_CAPTURE_VIEW_TYPENAME = "ScreenCaptureView";
	}
}
