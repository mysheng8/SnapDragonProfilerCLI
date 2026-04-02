using System;

namespace Sdp
{
	// Token: 0x020002EB RID: 747
	public interface ITensorView : IView
	{
		// Token: 0x06000F42 RID: 3906
		void DisplayTensor(string formatName, long[] dims, int numChannels, double[][][] matrices, string tiling, double formatMaxValue);

		// Token: 0x06000F43 RID: 3907
		void Clear();
	}
}
