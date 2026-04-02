using System;

namespace Sdp
{
	// Token: 0x020001B0 RID: 432
	public class BufferViewerController : IViewController
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0000C433 File Offset: 0x0000A633
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0000C43C File Offset: 0x0000A63C
		public ViewDesc SaveSettings()
		{
			ViewDesc viewDesc = null;
			if (this.m_view != null)
			{
				viewDesc = new ViewDesc();
				viewDesc.TypeName = this.m_view.TypeName;
			}
			return viewDesc;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0000C46B File Offset: 0x0000A66B
		public BufferViewerController(IBufferViewerView view)
		{
			this.m_view = view;
			BufferViewerEvents bufferViewerEvents = SdpApp.EventsManager.BufferViewerEvents;
			bufferViewerEvents.Invalidate = (EventHandler<BufferViewerInvalidateEventArgs>)Delegate.Combine(bufferViewerEvents.Invalidate, new EventHandler<BufferViewerInvalidateEventArgs>(this.bufferViewerEvents_Invalidate));
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0000C4A5 File Offset: 0x0000A6A5
		private void bufferViewerEvents_Invalidate(object o, BufferViewerInvalidateEventArgs e)
		{
			if (e != null)
			{
				this.m_view.Buffer = e.Buffer;
			}
		}

		// Token: 0x04000656 RID: 1622
		private IBufferViewerView m_view;
	}
}
