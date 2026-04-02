using System;
using System.Runtime.InteropServices;

// Token: 0x020000B1 RID: 177
public class SWIGTYPE_p_std__recursive_mutex
{
	// Token: 0x060013ED RID: 5101 RVA: 0x00018CB2 File Offset: 0x00016EB2
	internal SWIGTYPE_p_std__recursive_mutex(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x00018CC7 File Offset: 0x00016EC7
	protected SWIGTYPE_p_std__recursive_mutex()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x00018CE0 File Offset: 0x00016EE0
	internal static HandleRef getCPtr(SWIGTYPE_p_std__recursive_mutex obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001E2 RID: 482
	private HandleRef swigCPtr;
}
