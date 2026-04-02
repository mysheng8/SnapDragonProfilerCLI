using System;
using System.Runtime.InteropServices;

namespace TextureConverter
{
	// Token: 0x02000009 RID: 9
	[StructLayout(LayoutKind.Sequential)]
	public class TFormatFlags
	{
		// Token: 0x04000088 RID: 136
		public uint Stride;

		// Token: 0x04000089 RID: 137
		public uint MaskRed;

		// Token: 0x0400008A RID: 138
		public uint MaskGreen;

		// Token: 0x0400008B RID: 139
		public uint MaskBlue;

		// Token: 0x0400008C RID: 140
		public uint MaskAlpha;

		// Token: 0x0400008D RID: 141
		public uint FlipX;

		// Token: 0x0400008E RID: 142
		public uint FlipY;

		// Token: 0x0400008F RID: 143
		public uint EncodeFlag;

		// Token: 0x04000090 RID: 144
		public uint ScaleFilterFlag;

		// Token: 0x04000091 RID: 145
		public uint NormalMapFlag;

		// Token: 0x04000092 RID: 146
		public uint NormalMapScale;

		// Token: 0x04000093 RID: 147
		public uint NormalMapWrap;

		// Token: 0x04000094 RID: 148
		public uint DebugFlags;
	}
}
