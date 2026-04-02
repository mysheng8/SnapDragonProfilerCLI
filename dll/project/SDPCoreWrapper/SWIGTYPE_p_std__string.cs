using System;
using System.Runtime.InteropServices;

// Token: 0x020000B2 RID: 178
public class SWIGTYPE_p_std__string
{
	// Token: 0x060013F0 RID: 5104 RVA: 0x00018CF7 File Offset: 0x00016EF7
	internal SWIGTYPE_p_std__string(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x00018D0C File Offset: 0x00016F0C
	protected SWIGTYPE_p_std__string()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x00018D25 File Offset: 0x00016F25
	internal static HandleRef getCPtr(SWIGTYPE_p_std__string obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001E3 RID: 483
	private HandleRef swigCPtr;
}
