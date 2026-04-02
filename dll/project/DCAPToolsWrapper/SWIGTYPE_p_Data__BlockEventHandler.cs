using System;
using System.Runtime.InteropServices;

// Token: 0x02000023 RID: 35
public class SWIGTYPE_p_Data__BlockEventHandler
{
	// Token: 0x06000975 RID: 2421 RVA: 0x00019D88 File Offset: 0x00017F88
	internal SWIGTYPE_p_Data__BlockEventHandler(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x00019D9D File Offset: 0x00017F9D
	protected SWIGTYPE_p_Data__BlockEventHandler()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00019DB6 File Offset: 0x00017FB6
	internal static HandleRef getCPtr(SWIGTYPE_p_Data__BlockEventHandler obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B48 RID: 2888
	private HandleRef swigCPtr;
}
