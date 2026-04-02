using System;

namespace Sdp
{
	// Token: 0x0200025C RID: 604
	public class NewSnapshotMenuItem : MenuItemController
	{
		// Token: 0x06000A0E RID: 2574 RVA: 0x0001C234 File Offset: 0x0001A434
		public NewSnapshotMenuItem()
			: base("New Snapshot", new NewSnapshotCommand())
		{
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientDisconnectACK, new EventHandler(this.deviceEventsConnectionChanged));
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.EnableAction = (EventHandler<EnableActionArgs>)Delegate.Combine(connectionEvents.EnableAction, new EventHandler<EnableActionArgs>(this.deviceEventsEnableAction));
			base.Enabled = false;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0001C2AE File Offset: 0x0001A4AE
		private void deviceEventsConnectionChanged(object sender, EventArgs e)
		{
			this.m_view.Enabled = SdpApp.ConnectionManager.IsConnected() && SdpApp.ConnectionManager.SupportsCaptureType(CaptureType.Snapshot);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0001C2D5 File Offset: 0x0001A4D5
		private void deviceEventsEnableAction(object sender, EnableActionArgs e)
		{
			if (e.action == ActionEnum.Snapshot)
			{
				this.m_view.Enabled = true;
			}
		}
	}
}
