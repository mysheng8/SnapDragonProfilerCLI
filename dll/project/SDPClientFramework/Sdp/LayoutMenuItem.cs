using System;

namespace Sdp
{
	// Token: 0x0200025B RID: 603
	public class LayoutMenuItem : RadioMenuItemController
	{
		// Token: 0x06000A09 RID: 2569 RVA: 0x0001C144 File Offset: 0x0001A344
		public LayoutMenuItem(string label, ICommand command)
			: base(label, command)
		{
			base.Enabled = false;
			SdpApp.UIManager.SelectedLayoutChanged += this.uiManager_SelectedLayoutChanged;
			this.Invalidate();
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.ClientConnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientConnectACK, new EventHandler(this.deviceEventsConnectionChanged));
			DeviceEvents deviceEvents2 = SdpApp.EventsManager.DeviceEvents;
			deviceEvents2.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents2.ClientDisconnectACK, new EventHandler(this.deviceEventsConnectionChanged));
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0001C1D4 File Offset: 0x0001A3D4
		public void Activate()
		{
			IRadioMenuItemView radioMenuItemView = this.m_view as IRadioMenuItemView;
			if (radioMenuItemView != null)
			{
				radioMenuItemView.Activate();
			}
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0001C1F6 File Offset: 0x0001A3F6
		private void uiManager_SelectedLayoutChanged(object sender, EventArgs e)
		{
			this.Invalidate();
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0001C1FE File Offset: 0x0001A3FE
		private void Invalidate()
		{
			if (string.Compare(SdpApp.UIManager.SelectedLayout, base.Text) == 0)
			{
				this.Activate();
			}
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0001C21D File Offset: 0x0001A41D
		private void deviceEventsConnectionChanged(object sender, EventArgs e)
		{
			this.m_view.Enabled = SdpApp.ConnectionManager.IsConnected();
		}
	}
}
