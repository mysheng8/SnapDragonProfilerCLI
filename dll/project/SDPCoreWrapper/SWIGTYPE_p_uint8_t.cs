using System;
using System.Runtime.InteropServices;

// Token: 0x020000B6 RID: 182
public class SWIGTYPE_p_uint8_t
{
	// Token: 0x060013FC RID: 5116 RVA: 0x00018E0B File Offset: 0x0001700B
	internal SWIGTYPE_p_uint8_t(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x00018E20 File Offset: 0x00017020
	protected SWIGTYPE_p_uint8_t()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x00018E39 File Offset: 0x00017039
	internal static HandleRef getCPtr(SWIGTYPE_p_uint8_t obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001E7 RID: 487
	private HandleRef swigCPtr;
}
