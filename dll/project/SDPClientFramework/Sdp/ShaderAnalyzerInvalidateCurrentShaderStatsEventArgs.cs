using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200012C RID: 300
	public class ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs : EventArgs
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000A3E9 File Offset: 0x000085E9
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x0000A3F1 File Offset: 0x000085F1
		public Dictionary<ShaderStage, Dictionary<int, List<uint>>> Stats { get; set; } = new Dictionary<ShaderStage, Dictionary<int, List<uint>>>();

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000A3FA File Offset: 0x000085FA
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x0000A402 File Offset: 0x00008602
		public Dictionary<ShaderStage, Dictionary<int, string>> Errors { get; set; } = new Dictionary<ShaderStage, Dictionary<int, string>>();

		// Token: 0x04000449 RID: 1097
		public bool IsOlderVersion;

		// Token: 0x0400044A RID: 1098
		public ulong ResourceID;
	}
}
