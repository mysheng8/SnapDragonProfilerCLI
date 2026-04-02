using System;
using System.Runtime.InteropServices;

// Token: 0x0200009F RID: 159
public class SWIGTYPE_p_long_long
{
	// Token: 0x060013B7 RID: 5047 RVA: 0x000187D8 File Offset: 0x000169D8
	internal SWIGTYPE_p_long_long(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x000187ED File Offset: 0x000169ED
	protected SWIGTYPE_p_long_long()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013B9 RID: 5049 RVA: 0x00018806 File Offset: 0x00016A06
	internal static HandleRef getCPtr(SWIGTYPE_p_long_long obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D0 RID: 464
	private HandleRef swigCPtr;
}
