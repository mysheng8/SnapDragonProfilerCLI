using System;
using System.Runtime.InteropServices;

namespace Sdp
{
	// Token: 0x02000182 RID: 386
	public class QonvertWrapper
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x0000AAF1 File Offset: 0x00008CF1
		public static uint Qonvert(TQonvertImage srcImg, TQonvertImage dstImg)
		{
			return QonvertWrapper.Qonvert(srcImg, dstImg, IntPtr.Zero);
		}

		// Token: 0x06000458 RID: 1112
		[DllImport("TextureConverter.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint Qonvert(TQonvertImage srcImg, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] TQonvertImage dstImg, IntPtr options);

		// Token: 0x04000603 RID: 1539
		public const string Location = "TextureConverter.dll";
	}
}
