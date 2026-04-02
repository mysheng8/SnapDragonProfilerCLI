using System;

namespace Sdp
{
	// Token: 0x02000091 RID: 145
	public class BufferViewerEvents
	{
		// Token: 0x06000319 RID: 793 RVA: 0x000096E4 File Offset: 0x000078E4
		public BufferViewerEvents()
		{
			this.Invalidate = (EventHandler<BufferViewerInvalidateEventArgs>)Delegate.Combine(this.Invalidate, new EventHandler<BufferViewerInvalidateEventArgs>(this.SetVisible));
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000970E File Offset: 0x0000790E
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("BufferViewer", null, false, false);
		}

		// Token: 0x0400020A RID: 522
		public EventHandler<BufferViewerInvalidateEventArgs> Invalidate;

		// Token: 0x0400020B RID: 523
		public const string BUFFER_VIEW_TYPENAME = "BufferViewer";
	}
}
