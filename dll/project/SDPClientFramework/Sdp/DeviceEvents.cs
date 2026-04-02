using System;

namespace Sdp
{
	// Token: 0x020000CA RID: 202
	public class DeviceEvents
	{
		// Token: 0x040002CD RID: 717
		public EventHandler DeviceListChanged;

		// Token: 0x040002CE RID: 718
		public EventHandler ClientConnectACK;

		// Token: 0x040002CF RID: 719
		public EventHandler ClientDisconnectACK;

		// Token: 0x040002D0 RID: 720
		public EventHandler ADBNotFoundByClient;

		// Token: 0x040002D1 RID: 721
		public EventHandler HighlightedDeviceChanged;

		// Token: 0x040002D2 RID: 722
		public EventHandler<DeviceEventArgs> ConnectToDevice;

		// Token: 0x040002D3 RID: 723
		public EventHandler<DeviceEventArgs> DisconnectFromDevice;

		// Token: 0x040002D4 RID: 724
		public EventHandler<DeviceEventArgs> RetryInstallOnDevice;
	}
}
