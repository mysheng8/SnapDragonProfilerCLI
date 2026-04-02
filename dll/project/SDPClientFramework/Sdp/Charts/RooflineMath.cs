using System;

namespace Sdp.Charts
{
	// Token: 0x020002F1 RID: 753
	public class RooflineMath
	{
		// Token: 0x06000F4E RID: 3918 RVA: 0x0002FB8A File Offset: 0x0002DD8A
		public static double LogToLinear(double d)
		{
			return Math.Log(d, (double)RooflineMath.LogScale) * (double)RooflineMath.Precision;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0002FB9F File Offset: 0x0002DD9F
		public static double LinearToLog(double d)
		{
			return Math.Pow((double)RooflineMath.LogScale, d / (double)RooflineMath.Precision);
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0002FBB4 File Offset: 0x0002DDB4
		public static double DistanceFromPeak(float x, float y)
		{
			double num = (double)SdpApp.ModelManager.RooflineModel.PeakRooflinePerf / (double)SdpApp.ModelManager.RooflineModel.PeakRooflineMemBW;
			if ((double)x > num)
			{
				return 0.0;
			}
			return Math.Sqrt(Math.Pow(RooflineMath.LogToLinear((double)x) - RooflineMath.LogToLinear(num), 2.0) + Math.Pow(RooflineMath.LogToLinear((double)y) - RooflineMath.LogToLinear((double)SdpApp.ModelManager.RooflineModel.PeakRooflinePerf), 2.0));
		}

		// Token: 0x04000A78 RID: 2680
		private static int LogScale = 10;

		// Token: 0x04000A79 RID: 2681
		public static int Precision = 1000;
	}
}
