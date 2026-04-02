using System;
using System.Runtime.InteropServices;

namespace Sdp
{
	// Token: 0x02000180 RID: 384
	[StructLayout(LayoutKind.Sequential)]
	public class TFormatFlags
	{
		// Token: 0x040005F0 RID: 1520
		public uint Stride;

		// Token: 0x040005F1 RID: 1521
		public uint MaskRed;

		// Token: 0x040005F2 RID: 1522
		public uint MaskGreen;

		// Token: 0x040005F3 RID: 1523
		public uint MaskBlue;

		// Token: 0x040005F4 RID: 1524
		public uint MaskAlpha;

		// Token: 0x040005F5 RID: 1525
		public uint FlipX;

		// Token: 0x040005F6 RID: 1526
		public uint FlipY;

		// Token: 0x040005F7 RID: 1527
		public uint EncodeFlag;

		// Token: 0x040005F8 RID: 1528
		public uint ScaleFilterFlag;

		// Token: 0x040005F9 RID: 1529
		public uint NormalMapFlag;

		// Token: 0x040005FA RID: 1530
		public uint NormalMapScale;

		// Token: 0x040005FB RID: 1531
		public uint NormalMapWrap;

		// Token: 0x040005FC RID: 1532
		public uint DebugFlags;
	}
}
