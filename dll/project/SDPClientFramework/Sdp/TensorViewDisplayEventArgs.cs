using System;

namespace Sdp
{
	// Token: 0x02000172 RID: 370
	public class TensorViewDisplayEventArgs : EventArgs
	{
		// Token: 0x0400054B RID: 1355
		public uint CaptureID;

		// Token: 0x0400054C RID: 1356
		public ulong TensorID;

		// Token: 0x0400054D RID: 1357
		public string FormatName;

		// Token: 0x0400054E RID: 1358
		public long[] Dims;

		// Token: 0x0400054F RID: 1359
		public int ElementSize;

		// Token: 0x04000550 RID: 1360
		public int NumChannels;

		// Token: 0x04000551 RID: 1361
		public double[][][] Matrices;

		// Token: 0x04000552 RID: 1362
		public string Tiling;
	}
}
