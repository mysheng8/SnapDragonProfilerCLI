using System;
using System.Runtime.InteropServices;

// Token: 0x020000B7 RID: 183
public class SWIGTYPE_p_unsigned_int
{
	// Token: 0x060013FF RID: 5119 RVA: 0x00018E50 File Offset: 0x00017050
	internal SWIGTYPE_p_unsigned_int(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x00018E65 File Offset: 0x00017065
	protected SWIGTYPE_p_unsigned_int()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x00018E7E File Offset: 0x0001707E
	internal static HandleRef getCPtr(SWIGTYPE_p_unsigned_int obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001E8 RID: 488
	private HandleRef swigCPtr;
}
