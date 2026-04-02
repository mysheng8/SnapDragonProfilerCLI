using System;
using System.Runtime.InteropServices;

// Token: 0x020000B3 RID: 179
public class SWIGTYPE_p_std__vectorT_long_long_t
{
	// Token: 0x060013F3 RID: 5107 RVA: 0x00018D3C File Offset: 0x00016F3C
	internal SWIGTYPE_p_std__vectorT_long_long_t(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x00018D51 File Offset: 0x00016F51
	protected SWIGTYPE_p_std__vectorT_long_long_t()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x00018D6A File Offset: 0x00016F6A
	internal static HandleRef getCPtr(SWIGTYPE_p_std__vectorT_long_long_t obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001E4 RID: 484
	private HandleRef swigCPtr;
}
