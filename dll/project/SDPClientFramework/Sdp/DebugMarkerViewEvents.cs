using System;

namespace Sdp
{
	// Token: 0x02000099 RID: 153
	public class DebugMarkerViewEvents
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000972A File Offset: 0x0000792A
		public DebugMarkerViewEvents()
		{
			this.Invalidate = (EventHandler<InvalidateDebugMarkerViewEventArgs>)Delegate.Combine(this.Invalidate, new EventHandler<InvalidateDebugMarkerViewEventArgs>(this.SetVisible));
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00009754 File Offset: 0x00007954
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("DebugMarkerView", null, false, false);
		}

		// Token: 0x04000217 RID: 535
		public EventHandler<InvalidateDebugMarkerViewEventArgs> Invalidate;

		// Token: 0x04000218 RID: 536
		public EventHandler<DebugMarkerSelectedEventArgs> DebugMarkerSelected;

		// Token: 0x04000219 RID: 537
		public const string DEBUG_MARKER_VIEW_TYPENAME = "DebugMarkerView";
	}
}
