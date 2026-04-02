using System;
using System.Runtime.InteropServices;

// Token: 0x020000B8 RID: 184
public class SWIGTYPE_p_unsigned_long_long
{
	// Token: 0x06001402 RID: 5122 RVA: 0x00018E95 File Offset: 0x00017095
	internal SWIGTYPE_p_unsigned_long_long(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x00018EAA File Offset: 0x000170AA
	protected SWIGTYPE_p_unsigned_long_long()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001404 RID: 5124 RVA: 0x00018EC3 File Offset: 0x000170C3
	internal static HandleRef getCPtr(SWIGTYPE_p_unsigned_long_long obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001E9 RID: 489
	private HandleRef swigCPtr;
}
