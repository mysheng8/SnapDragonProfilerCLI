using System;

namespace Sdp
{
	// Token: 0x02000156 RID: 342
	public class RooflineKernelEventArgs : EventArgs
	{
		// Token: 0x040004D6 RID: 1238
		public uint CaptureID;

		// Token: 0x040004D7 RID: 1239
		public uint KernelID;

		// Token: 0x040004D8 RID: 1240
		public uint ProgramID;

		// Token: 0x040004D9 RID: 1241
		public string KernelName;

		// Token: 0x040004DA RID: 1242
		public string KernelStats;

		// Token: 0x040004DB RID: 1243
		public bool RegisterSpilling;

		// Token: 0x040004DC RID: 1244
		public long Duration;

		// Token: 0x040004DD RID: 1245
		public float AluUtilization;

		// Token: 0x040004DE RID: 1246
		public float PercentL2;

		// Token: 0x040004DF RID: 1247
		public long BytesRead;

		// Token: 0x040004E0 RID: 1248
		public long BytesWritten;

		// Token: 0x040004E1 RID: 1249
		public long KernelCycles;

		// Token: 0x040004E2 RID: 1250
		public long FullALUs;

		// Token: 0x040004E3 RID: 1251
		public long HalfALUs;

		// Token: 0x040004E4 RID: 1252
		public int Iterations;
	}
}
