using System;
using System.Collections.Generic;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x0200003A RID: 58
	public struct ShaderInfo
	{
		// Token: 0x06000133 RID: 307 RVA: 0x00011C4C File Offset: 0x0000FE4C
		public ShaderInfo(Dictionary<ShaderStage, Dictionary<int, List<uint>>> stats, Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>> logStats, Dictionary<ShaderStage, Dictionary<int, string>> shaderDisasmData)
		{
			this.shaderStats = stats;
			this.shaderLogStats = logStats;
			this.shaderDisasm = shaderDisasmData;
		}

		// Token: 0x040003D0 RID: 976
		public readonly Dictionary<ShaderStage, Dictionary<int, List<uint>>> shaderStats;

		// Token: 0x040003D1 RID: 977
		public readonly Dictionary<ShaderStage, Dictionary<int, List<ShaderLogStat>>> shaderLogStats;

		// Token: 0x040003D2 RID: 978
		public readonly Dictionary<ShaderStage, Dictionary<int, string>> shaderDisasm;
	}
}
