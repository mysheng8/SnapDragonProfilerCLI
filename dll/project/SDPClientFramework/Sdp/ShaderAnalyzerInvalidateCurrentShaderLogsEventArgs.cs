using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200012D RID: 301
	public class ShaderAnalyzerInvalidateCurrentShaderLogsEventArgs : EventArgs
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000A429 File Offset: 0x00008629
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000A431 File Offset: 0x00008631
		public Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>> LogStats { get; set; } = new Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>>();

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000A43A File Offset: 0x0000863A
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000A442 File Offset: 0x00008642
		public Dictionary<ShaderStage, Dictionary<int, string>> LogErrors { get; set; } = new Dictionary<ShaderStage, Dictionary<int, string>>();

		// Token: 0x0400044D RID: 1101
		public ulong ResourceID;
	}
}
