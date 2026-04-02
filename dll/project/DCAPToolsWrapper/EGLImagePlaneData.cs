using System;

namespace DCAPToolsWrapper
{
	// Token: 0x02000036 RID: 54
	public struct EGLImagePlaneData
	{
		// Token: 0x04000B93 RID: 2963
		public uint m_pitch;

		// Token: 0x04000B94 RID: 2964
		public ulong m_slicePitch;

		// Token: 0x04000B95 RID: 2965
		public ulong m_offset;

		// Token: 0x04000B96 RID: 2966
		public unsafe byte* m_pData;
	}
}
