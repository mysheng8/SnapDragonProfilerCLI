using System;

namespace Sdp
{
	// Token: 0x020002DF RID: 735
	public class ProgramViewVariable
	{
		// Token: 0x04000A2D RID: 2605
		public ulong LayoutID;

		// Token: 0x04000A2E RID: 2606
		public ShaderStage ShaderType;

		// Token: 0x04000A2F RID: 2607
		public int DisplayID = -1;

		// Token: 0x04000A30 RID: 2608
		public int ParentDisplayID = -1;

		// Token: 0x04000A31 RID: 2609
		public uint Offset;

		// Token: 0x04000A32 RID: 2610
		public string Name;

		// Token: 0x04000A33 RID: 2611
		public string Value;

		// Token: 0x04000A34 RID: 2612
		public string Type;
	}
}
