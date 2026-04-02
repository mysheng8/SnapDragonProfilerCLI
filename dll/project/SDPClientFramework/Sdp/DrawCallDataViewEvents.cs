using System;

namespace Sdp
{
	// Token: 0x0200009C RID: 156
	public class DrawCallDataViewEvents
	{
		// Token: 0x06000326 RID: 806 RVA: 0x0000977B File Offset: 0x0000797B
		public DrawCallDataViewEvents()
		{
			this.InvalidateViews = (EventHandler<InvalidateViewEventArgs>)Delegate.Combine(this.InvalidateViews, new EventHandler<InvalidateViewEventArgs>(this.SetVisible));
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000097A5 File Offset: 0x000079A5
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("DrawCallDataView", null, false, false);
		}

		// Token: 0x04000223 RID: 547
		public EventHandler<DrawCallSelectedEventArgs> DrawCallSelected;

		// Token: 0x04000224 RID: 548
		public EventHandler<InvalidateViewEventArgs> InvalidateViews;

		// Token: 0x04000225 RID: 549
		public EventHandler<SetStatusEventArgs> SetStatus;

		// Token: 0x04000226 RID: 550
		public const string DRAWCALL_VIEW_TYPENAME = "DrawCallDataView";
	}
}
