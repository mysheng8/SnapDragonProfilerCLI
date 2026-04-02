using System;

namespace Sdp
{
	// Token: 0x020002C3 RID: 707
	public class DebugMarkerViewModel
	{
		// Token: 0x040009AB RID: 2475
		public int Id;

		// Token: 0x040009AC RID: 2476
		public long StartTimeCPU;

		// Token: 0x040009AD RID: 2477
		public long EndTimeCPU;

		// Token: 0x040009AE RID: 2478
		public long StartTimeGPU;

		// Token: 0x040009AF RID: 2479
		public long EndTimeGPU;

		// Token: 0x040009B0 RID: 2480
		public string Label;

		// Token: 0x040009B1 RID: 2481
		public bool IsDebugRegion;

		// Token: 0x040009B2 RID: 2482
		public string Color;

		// Token: 0x040009B3 RID: 2483
		public double[] ColorValues;

		// Token: 0x040009B4 RID: 2484
		public string DrawIds;

		// Token: 0x040009B5 RID: 2485
		public uint BlockID;

		// Token: 0x040009B6 RID: 2486
		public int CaptureID;

		// Token: 0x040009B7 RID: 2487
		public long ContextID;

		// Token: 0x040009B8 RID: 2488
		public long CommandBuffer;

		// Token: 0x040009B9 RID: 2489
		public int FrameIndex;
	}
}
