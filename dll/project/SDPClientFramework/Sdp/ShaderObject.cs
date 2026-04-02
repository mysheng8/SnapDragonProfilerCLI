using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000136 RID: 310
	public class ShaderObject
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x0000A63F File Offset: 0x0000883F
		public ShaderObject(ShaderStage shaderType, ulong shaderModuleID, string source)
		{
			this.ShaderType = shaderType;
			this.ShaderModuleID = shaderModuleID;
			this.Source = source;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000A667 File Offset: 0x00008867
		public ShaderObject(ShaderStage shaderType, string source)
		{
			this.ShaderType = shaderType;
			this.ShaderModuleID = 0UL;
			this.Source = source;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0000A690 File Offset: 0x00008890
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x0000A698 File Offset: 0x00008898
		public ShaderStage ShaderType { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0000A6A1 File Offset: 0x000088A1
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x0000A6A9 File Offset: 0x000088A9
		public ulong ShaderModuleID { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000A6B2 File Offset: 0x000088B2
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x0000A6BA File Offset: 0x000088BA
		public uint ShaderIndex { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000A6C3 File Offset: 0x000088C3
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0000A6CB File Offset: 0x000088CB
		public string Source { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0000A6D4 File Offset: 0x000088D4
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0000A6DC File Offset: 0x000088DC
		public ulong ShaderCycleCount { get; set; }

		// Token: 0x0400046B RID: 1131
		public Dictionary<uint, Tuple<uint, uint>> HitCyclePercentages = new Dictionary<uint, Tuple<uint, uint>>();
	}
}
