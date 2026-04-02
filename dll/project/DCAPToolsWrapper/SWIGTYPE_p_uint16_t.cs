using System;
using System.Runtime.InteropServices;

// Token: 0x0200002E RID: 46
public class SWIGTYPE_p_uint16_t
{
	// Token: 0x06000996 RID: 2454 RVA: 0x0001A07F File Offset: 0x0001827F
	internal SWIGTYPE_p_uint16_t(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0001A094 File Offset: 0x00018294
	protected SWIGTYPE_p_uint16_t()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0001A0AD File Offset: 0x000182AD
	internal static HandleRef getCPtr(SWIGTYPE_p_uint16_t obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B53 RID: 2899
	private HandleRef swigCPtr;
}
