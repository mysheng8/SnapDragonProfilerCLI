using System;

namespace Sdp
{
	// Token: 0x020002E8 RID: 744
	public struct ShaderCostValue
	{
		// Token: 0x06000F18 RID: 3864 RVA: 0x0002EBB8 File Offset: 0x0002CDB8
		public ShaderCostValue(uint index, ShaderStage shaderStage, uint shaderIndex, ulong cycleValue)
		{
			this.Index = index;
			this.Stage = shaderStage;
			this.ShaderIndex = shaderIndex;
			this.CycleValue = cycleValue;
		}

		// Token: 0x04000A54 RID: 2644
		public uint Index;

		// Token: 0x04000A55 RID: 2645
		public ShaderStage Stage;

		// Token: 0x04000A56 RID: 2646
		public uint ShaderIndex;

		// Token: 0x04000A57 RID: 2647
		public ulong CycleValue;
	}
}
