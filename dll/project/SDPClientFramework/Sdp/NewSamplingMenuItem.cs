using System;

namespace Sdp
{
	// Token: 0x020001E1 RID: 481
	public class NewSamplingMenuItem : MenuItemController
	{
		// Token: 0x060006A5 RID: 1701 RVA: 0x0000FF90 File Offset: 0x0000E190
		public NewSamplingMenuItem()
			: base("New Sampling", new NewSamplingCommand())
		{
			base.Enabled = false;
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.EnableAction = (EventHandler<EnableActionArgs>)Delegate.Combine(connectionEvents.EnableAction, new EventHandler<EnableActionArgs>(this.deviceEventsEnableAction));
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientDisconnectACK, new EventHandler(this.deviceEventsConnectionChanged));
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001000C File Offset: 0x0000E20C
		private void deviceEventsConnectionChanged(object sender, EventArgs e)
		{
			bool flag = DeviceManager.Get().IsSimpleperfAvailable();
			this.m_view.Enabled = SdpApp.ConnectionManager.IsConnected() && flag;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001003B File Offset: 0x0000E23B
		private void deviceEventsEnableAction(object sender, EnableActionArgs e)
		{
			if (e.action == ActionEnum.Sampling)
			{
				this.m_view.Enabled = true;
			}
		}
	}
}
