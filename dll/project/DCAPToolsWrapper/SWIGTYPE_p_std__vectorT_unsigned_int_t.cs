using System;
using System.Runtime.InteropServices;

// Token: 0x0200002D RID: 45
public class SWIGTYPE_p_std__vectorT_unsigned_int_t
{
	// Token: 0x06000993 RID: 2451 RVA: 0x0001A03A File Offset: 0x0001823A
	internal SWIGTYPE_p_std__vectorT_unsigned_int_t(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0001A04F File Offset: 0x0001824F
	protected SWIGTYPE_p_std__vectorT_unsigned_int_t()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0001A068 File Offset: 0x00018268
	internal static HandleRef getCPtr(SWIGTYPE_p_std__vectorT_unsigned_int_t obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B52 RID: 2898
	private HandleRef swigCPtr;
}
