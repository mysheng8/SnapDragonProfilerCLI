using System;

namespace Sdp
{
	// Token: 0x02000154 RID: 340
	public class RooflineEvents
	{
		// Token: 0x06000429 RID: 1065 RVA: 0x0000A898 File Offset: 0x00008A98
		public RooflineEvents()
		{
			this.RooflinePresent = (EventHandler)Delegate.Combine(this.RooflinePresent, new EventHandler(this.SetVisible));
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000A8C2 File Offset: 0x00008AC2
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("RooflineView", null, true, false);
		}

		// Token: 0x040004CD RID: 1229
		public EventHandler<RooflinePeaksEventArgs> SetPeaksEvent;

		// Token: 0x040004CE RID: 1230
		public EventHandler<RooflineKernelEventArgs> ProcessKernelDataEvent;

		// Token: 0x040004CF RID: 1231
		public EventHandler<RooflineCompleteEventArgs> RooflineCompleteEvent;

		// Token: 0x040004D0 RID: 1232
		public EventHandler<RooflineKernelCreatedEventArgs> KernelCreatedEvent;

		// Token: 0x040004D1 RID: 1233
		public EventHandler RooflinePresent;

		// Token: 0x040004D2 RID: 1234
		public const string ROOFLINE_VIEW_TYPENAME = "RooflineView";
	}
}
