using System;

namespace Sdp
{
	// Token: 0x0200013D RID: 317
	public class SourceViewEvents
	{
		// Token: 0x06000410 RID: 1040 RVA: 0x0000A6E5 File Offset: 0x000088E5
		public SourceViewEvents()
		{
			this.Add = (EventHandler<SourceViewAddEventArgs>)Delegate.Combine(this.Add, new EventHandler<SourceViewAddEventArgs>(this.SetVisible));
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000A70F File Offset: 0x0000890F
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("SourceView", null, false, false);
		}

		// Token: 0x0400047D RID: 1149
		public EventHandler<SourceViewAddEventArgs> Add;

		// Token: 0x0400047E RID: 1150
		public const string SOURCE_VIEW_TYPENAME = "SourceView";
	}
}
