using System;
using System.Runtime.InteropServices;

namespace Sdp
{
	// Token: 0x020001CE RID: 462
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public class QSTREAMCLRooflinelData
	{
		// Token: 0x0400069B RID: 1691
		public uint version;

		// Token: 0x0400069C RID: 1692
		public uint enabled;

		// Token: 0x0400069D RID: 1693
		public float peakRooflinePerf;

		// Token: 0x0400069E RID: 1694
		public uint peakRooflineMaxFreq;

		// Token: 0x0400069F RID: 1695
		public float peakRooflineMemBW;

		// Token: 0x040006A0 RID: 1696
		public uint pid;

		// Token: 0x040006A1 RID: 1697
		public uint rooflineID;

		// Token: 0x040006A2 RID: 1698
		public uint aluUtilizationID;

		// Token: 0x040006A3 RID: 1699
		public uint percentL2ID;

		// Token: 0x040006A4 RID: 1700
		public uint totalBytesReadID;

		// Token: 0x040006A5 RID: 1701
		public uint totalBytesWrittenID;

		// Token: 0x040006A6 RID: 1702
		public uint totalKernelCyclesID;

		// Token: 0x040006A7 RID: 1703
		public uint fullALUsID;

		// Token: 0x040006A8 RID: 1704
		public uint halfALUsID;
	}
}
