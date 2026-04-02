using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;

namespace TextureConverter
{
	// Token: 0x0200000B RID: 11
	public class QonvertWrapper
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002054 File Offset: 0x00000254
		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		public static uint Qonvert(TQonvertImage srcImg, TQonvertImage dstImg)
		{
			uint num = 9U;
			try
			{
				num = QonvertWrapper.Qonvert(srcImg, dstImg, IntPtr.Zero);
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("{0}\n{1}", ex.Message, ex.StackTrace);
				num = 9U;
			}
			return num;
		}

		// Token: 0x06000006 RID: 6
		[DllImport("TextureConverter.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint Qonvert(TQonvertImage srcImg, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] TQonvertImage dstImg, IntPtr options);

		// Token: 0x0400009B RID: 155
		public const string Location = "TextureConverter.dll";
	}
}
