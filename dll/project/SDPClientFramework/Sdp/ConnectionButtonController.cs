using System;

namespace Sdp
{
	// Token: 0x020001B9 RID: 441
	public class ConnectionButtonController
	{
		// Token: 0x060005AE RID: 1454 RVA: 0x0000D710 File Offset: 0x0000B910
		public ConnectionButtonController(IConnectionButton view)
		{
			this.m_view = view;
			this.m_view.ConnectionButtonPressEvent += this.view_ButtonPressEvent;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0000D738 File Offset: 0x0000B938
		public void Update()
		{
			Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
			this.m_view.SetConnected(connectedDevice != null, ((connectedDevice != null) ? connectedDevice.GetDeviceAttributes().GetProductModel() : null) ?? string.Empty);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0000D77C File Offset: 0x0000B97C
		private void view_ButtonPressEvent(object sender, ButtonPressEventArgs e)
		{
			if (SdpApp.ConnectionManager.GetConnectedDevice() == null)
			{
				SdpApp.CommandManager.ExecuteCommand(new ChangeLayoutCommand("Connect"));
			}
		}

		// Token: 0x04000678 RID: 1656
		private IConnectionButton m_view;
	}
}
