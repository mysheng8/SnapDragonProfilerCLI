using System;

namespace Sdp
{
	// Token: 0x020001B3 RID: 435
	public class InternalDeviceDelegate : DeviceDelegate
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0000D1C3 File Offset: 0x0000B3C3
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0000D1CB File Offset: 0x0000B3CB
		public DeviceConnectionState PreviousState { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0000D1D4 File Offset: 0x0000B3D4
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x0000D1DC File Offset: 0x0000B3DC
		public DeviceConnectionState CurrentState { get; set; }

		// Token: 0x0600056A RID: 1386 RVA: 0x0000D1E8 File Offset: 0x0000B3E8
		public InternalDeviceDelegate(string lookupName, DeviceConnectionState currentState, ConnectionController container)
		{
			this.m_lookupName = lookupName;
			this.m_container = container;
			this.PreviousState = currentState;
			this.CurrentState = currentState;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0000D219 File Offset: 0x0000B419
		public string Name
		{
			get
			{
				return this.m_lookupName;
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void OnDeviceConnected(string name)
		{
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void OnDeviceDisconnected(string name)
		{
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0000D224 File Offset: 0x0000B424
		public override void OnDeviceStateChanged(string name)
		{
			Device device = DeviceManager.Get().GetDevice(this.m_lookupName);
			string deviceGUID = SdpApp.ModelManager.SettingsModel.UserPreferences.GetDeviceGUID(this.m_lookupName, ConnectionManager.ParseDeviceOSAttribute(device.GetDeviceAttributes().osType));
			if (device != null && this.CurrentState != device.GetDeviceState())
			{
				this.PreviousState = this.CurrentState;
				this.CurrentState = device.GetDeviceState();
				if (this.m_container != null)
				{
					this.m_container.InvalidateDeviceState(name, deviceGUID, true);
				}
			}
		}

		// Token: 0x0400065C RID: 1628
		private string m_lookupName;

		// Token: 0x0400065D RID: 1629
		private ConnectionController m_container;
	}
}
