using System;

namespace Sdp
{
	// Token: 0x02000175 RID: 373
	public struct GGPMBlock
	{
		// Token: 0x04000557 RID: 1367
		public uint id;

		// Token: 0x04000558 RID: 1368
		public string name;

		// Token: 0x04000559 RID: 1369
		public uint numCountables;

		// Token: 0x0400055A RID: 1370
		public uint numCounters;

		// Token: 0x0400055B RID: 1371
		public IntPtr countables;

		// Token: 0x0400055C RID: 1372
		public IntPtr counterStates;
	}
}
