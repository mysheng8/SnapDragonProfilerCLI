using System;
using System.Runtime.InteropServices;

namespace Sdp
{
	// Token: 0x02000173 RID: 371
	public class Bytes
	{
		// Token: 0x06000445 RID: 1093 RVA: 0x0000A954 File Offset: 0x00008B54
		public static byte[] GetBytes(object inputStruct)
		{
			int num = Marshal.SizeOf(inputStruct);
			byte[] array = new byte[num];
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(inputStruct, intPtr, true);
			Marshal.Copy(intPtr, array, 0, num);
			Marshal.FreeHGlobal(intPtr);
			return array;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000A990 File Offset: 0x00008B90
		public static T Cast<T>(byte[] data, bool resize = true, int offset = 0) where T : new()
		{
			T t = new T();
			int num = Marshal.SizeOf<T>(t);
			if (data.Length >= num)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(num);
				Marshal.Copy(data, offset, intPtr, num);
				t = (T)((object)Marshal.PtrToStructure(intPtr, t.GetType()));
				Marshal.FreeHGlobal(intPtr);
				if (resize)
				{
					if (data.Length - num > 0)
					{
						int num2 = data.Length - num;
						Buffer.BlockCopy(data, offset + num, data, offset, num2);
						Array.Resize<byte>(ref data, num2);
					}
					else
					{
						data = null;
					}
				}
			}
			return t;
		}
	}
}
