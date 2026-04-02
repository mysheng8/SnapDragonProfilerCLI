using System;

// Token: 0x02000029 RID: 41
public enum DeviceConnectionState
{
	// Token: 0x0400006E RID: 110
	Scanning,
	// Token: 0x0400006F RID: 111
	Discovered,
	// Token: 0x04000070 RID: 112
	Installing,
	// Token: 0x04000071 RID: 113
	InstallFailed,
	// Token: 0x04000072 RID: 114
	Uninstalling,
	// Token: 0x04000073 RID: 115
	Ready,
	// Token: 0x04000074 RID: 116
	Connecting,
	// Token: 0x04000075 RID: 117
	Connected,
	// Token: 0x04000076 RID: 118
	Disconnecting,
	// Token: 0x04000077 RID: 119
	MaxDeviceConnectionStates,
	// Token: 0x04000078 RID: 120
	Unknown
}
