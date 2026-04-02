using System;

namespace Sdp
{
	// Token: 0x020001E3 RID: 483
	public class CopyMenuItem : MenuItemController
	{
		// Token: 0x060006AA RID: 1706 RVA: 0x000100A2 File Offset: 0x0000E2A2
		public CopyMenuItem()
			: base("_Copy", new CopyCommand())
		{
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.MenuActivated = (EventHandler<MenuActivatedEventArgs>)Delegate.Combine(clientEvents.MenuActivated, new EventHandler<MenuActivatedEventArgs>(this.clientEvents_MenuActivated));
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001008F File Offset: 0x0000E28F
		private void clientEvents_MenuActivated(object sender, MenuActivatedEventArgs args)
		{
			this.m_view.Enabled = args.copyable;
		}
	}
}
