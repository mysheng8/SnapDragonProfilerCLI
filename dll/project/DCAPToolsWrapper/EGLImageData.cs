using System;

namespace DCAPToolsWrapper
{
	// Token: 0x02000037 RID: 55
	public struct EGLImageData
	{
		// Token: 0x04000B97 RID: 2967
		public uint m_width;

		// Token: 0x04000B98 RID: 2968
		public uint m_height;

		// Token: 0x04000B99 RID: 2969
		public uint m_qctFormat;

		// Token: 0x04000B9A RID: 2970
		public uint m_bpp;

		// Token: 0x04000B9B RID: 2971
		public uint m_numLayers;

		// Token: 0x04000B9C RID: 2972
		public uint m_numPlanes;

		// Token: 0x04000B9D RID: 2973
		public unsafe EGLImagePlaneData* m_pPlanes;
	}
}
