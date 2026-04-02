using System;
using System.Runtime.InteropServices;

// Token: 0x0200002F RID: 47
public class SWIGTYPE_p_unsigned_long_long
{
	// Token: 0x06000999 RID: 2457 RVA: 0x0001A0C4 File Offset: 0x000182C4
	internal SWIGTYPE_p_unsigned_long_long(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0001A0D9 File Offset: 0x000182D9
	protected SWIGTYPE_p_unsigned_long_long()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0001A0F2 File Offset: 0x000182F2
	internal static HandleRef getCPtr(SWIGTYPE_p_unsigned_long_long obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B54 RID: 2900
	private HandleRef swigCPtr;
}
