using System;

namespace Sdp
{
	// Token: 0x020001F3 RID: 499
	public class NewTraceCaptureMenuItem : MenuItemController
	{
		// Token: 0x06000737 RID: 1847 RVA: 0x00013108 File Offset: 0x00011308
		public NewTraceCaptureMenuItem()
			: base("New Trace", new NewCaptureCommand())
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.EnableAction = (EventHandler<EnableActionArgs>)Delegate.Combine(connectionEvents.EnableAction, new EventHandler<EnableActionArgs>(this.deviceEventsEnableAction));
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientDisconnectACK, new EventHandler(this.deviceEventsConnectionChanged));
			base.Enabled = false;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00013182 File Offset: 0x00011382
		private void deviceEventsConnectionChanged(object sender, EventArgs e)
		{
			this.m_view.Enabled = SdpApp.ConnectionManager.IsConnected();
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00013199 File Offset: 0x00011399
		private void deviceEventsEnableAction(object sender, EnableActionArgs e)
		{
			if (e.action == ActionEnum.NewCapture)
			{
				this.m_view.Enabled = true;
			}
		}
	}
}
