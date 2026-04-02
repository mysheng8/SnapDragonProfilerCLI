using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200012E RID: 302
	public class ShaderAnalyzerInvalidateCurrentShaderDisasmEventArgs : EventArgs
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000A469 File Offset: 0x00008669
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000A471 File Offset: 0x00008671
		public Dictionary<ShaderStage, Dictionary<int, string>> ShaderDisasm { get; set; } = new Dictionary<ShaderStage, Dictionary<int, string>>();

		// Token: 0x04000450 RID: 1104
		public ulong ResourceID;
	}
}
