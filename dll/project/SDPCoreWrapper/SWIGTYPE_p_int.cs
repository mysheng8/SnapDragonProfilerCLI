using System;
using System.Runtime.InteropServices;

// Token: 0x0200009E RID: 158
public class SWIGTYPE_p_int
{
	// Token: 0x060013B4 RID: 5044 RVA: 0x00018793 File Offset: 0x00016993
	internal SWIGTYPE_p_int(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013B5 RID: 5045 RVA: 0x000187A8 File Offset: 0x000169A8
	protected SWIGTYPE_p_int()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x000187C1 File Offset: 0x000169C1
	internal static HandleRef getCPtr(SWIGTYPE_p_int obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001CF RID: 463
	private HandleRef swigCPtr;
}
