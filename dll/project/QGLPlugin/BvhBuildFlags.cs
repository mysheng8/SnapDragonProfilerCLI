using System;

namespace QGLPlugin
{
	// Token: 0x02000021 RID: 33
	internal enum BvhBuildFlags
	{
		// Token: 0x04000320 RID: 800
		BvhBuildFlagsAllowUpdate = 1,
		// Token: 0x04000321 RID: 801
		BvhBuildFlagsAllowCompaction,
		// Token: 0x04000322 RID: 802
		BvhBuildFlagsPreferFastTrace = 4,
		// Token: 0x04000323 RID: 803
		BvhBuildFlagsPreferFastBuild = 8,
		// Token: 0x04000324 RID: 804
		BvhBuildFlagsLowMemory = 16
	}
}
