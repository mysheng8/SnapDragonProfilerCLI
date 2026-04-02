using System;

namespace Sdp
{
	// Token: 0x0200025D RID: 605
	public class ViewMenuItem : ToggleMenuItemController
	{
		// Token: 0x06000A11 RID: 2577 RVA: 0x0001C2EC File Offset: 0x0001A4EC
		public ViewMenuItem(string label, ICommand command)
			: base(label, command)
		{
			base.Enabled = false;
			base.Active = false;
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.WindowVisibilityChanged = (EventHandler<CaptureWindowEventArgs>)Delegate.Combine(clientEvents.WindowVisibilityChanged, new EventHandler<CaptureWindowEventArgs>(this.clientEvents_WindowVisibilityChanged));
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.ClientConnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientConnectACK, new EventHandler(this.deviceEventsConnectionChanged));
			DeviceEvents deviceEvents2 = SdpApp.EventsManager.DeviceEvents;
			deviceEvents2.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents2.ClientDisconnectACK, new EventHandler(this.deviceEventsConnectionChanged));
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0001C390 File Offset: 0x0001A590
		private void clientEvents_WindowVisibilityChanged(object sender, CaptureWindowEventArgs e)
		{
			if (string.Compare(e.Window.Name, base.Text) == 0 && this.m_view != null)
			{
				base.Active = e.Window.IsVisible;
				IToggleMenuItemView toggleMenuItemView = this.m_view as IToggleMenuItemView;
				toggleMenuItemView.Toggled = e.Window.IsVisible;
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0001C3EB File Offset: 0x0001A5EB
		private void deviceEventsConnectionChanged(object sender, EventArgs e)
		{
			if (this.m_view != null)
			{
				this.m_view.Enabled = SdpApp.ConnectionManager.IsConnected();
			}
		}
	}
}
