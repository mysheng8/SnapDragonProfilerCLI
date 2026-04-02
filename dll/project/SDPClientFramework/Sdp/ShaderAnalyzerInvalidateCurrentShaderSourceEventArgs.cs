using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200012F RID: 303
	public class ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs : EventArgs
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0000A48D File Offset: 0x0000868D
		// (set) Token: 0x060003DE RID: 990 RVA: 0x0000A495 File Offset: 0x00008695
		public Dictionary<ShaderStage, Dictionary<int, bool>> Succeeded { get; set; } = new Dictionary<ShaderStage, Dictionary<int, bool>>();

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000A49E File Offset: 0x0000869E
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0000A4A6 File Offset: 0x000086A6
		public Dictionary<ShaderStage, Dictionary<int, string>> ShaderSources { get; set; } = new Dictionary<ShaderStage, Dictionary<int, string>>();

		// Token: 0x04000452 RID: 1106
		public ulong ResourceID;
	}
}
