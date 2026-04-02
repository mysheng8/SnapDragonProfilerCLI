using System;

namespace Sdp
{
	// Token: 0x0200012A RID: 298
	public class ShaderAnalyzerProgramEventArgs : EventArgs
	{
		// Token: 0x0400043E RID: 1086
		public int CategoryID;

		// Token: 0x0400043F RID: 1087
		public int ResourceID;

		// Token: 0x04000440 RID: 1088
		public bool EnableOverride;

		// Token: 0x04000441 RID: 1089
		public bool IsModified;

		// Token: 0x04000442 RID: 1090
		public ShaderGroup ShaderObjectGroup;

		// Token: 0x04000443 RID: 1091
		public int Source;

		// Token: 0x04000444 RID: 1092
		public bool IsEditable;

		// Token: 0x04000445 RID: 1093
		public bool IsDX12Shader;

		// Token: 0x04000446 RID: 1094
		public bool IsWaiting;
	}
}
