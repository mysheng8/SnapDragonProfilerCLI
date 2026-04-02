using System;
using System.Runtime.InteropServices;

namespace Sdp
{
	// Token: 0x02000177 RID: 375
	public class MetricsAPIWrapper
	{
		// Token: 0x06000448 RID: 1096
		[DllImport("MetricsAPI", CallingConvention = CallingConvention.Cdecl, EntryPoint = "metricsapi_init")]
		public static extern uint Init([MarshalAs(UnmanagedType.LPStr)] [In] string name, out int version, [In] uint maxGpuFreq, [In] uint mode, out IntPtr ppInstance);

		// Token: 0x06000449 RID: 1097
		[DllImport("MetricsAPI", CallingConvention = CallingConvention.Cdecl, EntryPoint = "metricsapi_destroy")]
		public static extern uint Destroy([In] IntPtr pInstance);

		// Token: 0x0600044A RID: 1098
		[DllImport("MetricsAPI", CallingConvention = CallingConvention.Cdecl, EntryPoint = "metricsapi_GetCountablesForMetricByKey")]
		public static extern uint GetCountablesForMetric([In] IntPtr pInstance, [MarshalAs(UnmanagedType.LPStr)] [In] string name, [In] [Out] IntPtr countablesRequired, out int length);

		// Token: 0x0600044B RID: 1099
		[DllImport("MetricsAPI", CallingConvention = CallingConvention.Cdecl, EntryPoint = "metricsapi_GetMetricValueByKey")]
		public static extern double GetMetricValue([In] IntPtr pInstance, [MarshalAs(UnmanagedType.LPStr)] [In] string name, double deltaT, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] GGPMCountable[] countablesRequired, int length, [In] uint pipeMask = 4294967295U, [In] IntPtr pCorrectionParams = default(IntPtr));

		// Token: 0x04000567 RID: 1383
		public const string Location = "MetricsAPI";

		// Token: 0x0200036E RID: 878
		public enum GGPMMode
		{
			// Token: 0x04000C09 RID: 3081
			GGPMModeGL,
			// Token: 0x04000C0A RID: 3082
			GGPMModeDEBUGFS,
			// Token: 0x04000C0B RID: 3083
			GGPMModeKERNEL
		}
	}
}
