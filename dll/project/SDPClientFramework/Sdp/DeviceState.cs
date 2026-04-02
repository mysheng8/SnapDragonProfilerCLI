using System;

namespace Sdp
{
	// Token: 0x02000237 RID: 567
	public enum DeviceState
	{
		// Token: 0x040007FE RID: 2046
		Unknown,
		// Token: 0x040007FF RID: 2047
		Installing,
		// Token: 0x04000800 RID: 2048
		InstallFailed,
		// Token: 0x04000801 RID: 2049
		Ready,
		// Token: 0x04000802 RID: 2050
		Connecting,
		// Token: 0x04000803 RID: 2051
		ConnectingError,
		// Token: 0x04000804 RID: 2052
		Connected
	}
}
