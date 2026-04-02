using System;

namespace Sdp
{
	// Token: 0x020001E2 RID: 482
	public class SelectAllMenuItem : MenuItemController
	{
		// Token: 0x060006A8 RID: 1704 RVA: 0x00010052 File Offset: 0x0000E252
		public SelectAllMenuItem()
			: base("_Select All", new SelectAllCommand())
		{
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.MenuActivated = (EventHandler<MenuActivatedEventArgs>)Delegate.Combine(clientEvents.MenuActivated, new EventHandler<MenuActivatedEventArgs>(this.clientEvents_MenuActivated));
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001008F File Offset: 0x0000E28F
		private void clientEvents_MenuActivated(object sender, MenuActivatedEventArgs args)
		{
			this.m_view.Enabled = args.copyable;
		}
	}
}
