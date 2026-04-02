using System;

namespace Sdp
{
	// Token: 0x020000B0 RID: 176
	public class ConfigureDeviceCompleteArgs : EventArgs
	{
		// Token: 0x04000288 RID: 648
		public ConnectionSettings ConnectionSettings;

		// Token: 0x04000289 RID: 649
		public bool EditConnection;

		// Token: 0x0400028A RID: 650
		public bool RenameOnly;
	}
}
