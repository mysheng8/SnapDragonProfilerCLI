using System;

namespace Sdp
{
	// Token: 0x020000F1 RID: 241
	public class PixelHistoryEvents
	{
		// Token: 0x06000389 RID: 905 RVA: 0x00009B7A File Offset: 0x00007D7A
		public PixelHistoryEvents()
		{
			this.Invalidate = (EventHandler<PixelHistoryInvalidateEventArgs>)Delegate.Combine(this.Invalidate, new EventHandler<PixelHistoryInvalidateEventArgs>(this.SetVisible));
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00009BA4 File Offset: 0x00007DA4
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("PixelHistoryView", null, false, false);
		}

		// Token: 0x0400035B RID: 859
		public EventHandler<PixelHistoryInvalidateEventArgs> Invalidate;

		// Token: 0x0400035C RID: 860
		public EventHandler<PixelHistoryItemSelectedEventArgs> ItemSelected;

		// Token: 0x0400035D RID: 861
		public EventHandler<SetStatusEventArgs> SetStatus;

		// Token: 0x0400035E RID: 862
		public EventHandler<EventArgs> HideStatus;

		// Token: 0x0400035F RID: 863
		public const string PIXEL_HISTORY_VIEW_TYPENAME = "PixelHistoryView";
	}
}
