using System;
using System.Runtime.InteropServices;

// Token: 0x02000024 RID: 36
public class SWIGTYPE_p_Data__BlockHeader
{
	// Token: 0x06000978 RID: 2424 RVA: 0x00019DCD File Offset: 0x00017FCD
	internal SWIGTYPE_p_Data__BlockHeader(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00019DE2 File Offset: 0x00017FE2
	protected SWIGTYPE_p_Data__BlockHeader()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00019DFB File Offset: 0x00017FFB
	internal static HandleRef getCPtr(SWIGTYPE_p_Data__BlockHeader obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B49 RID: 2889
	private HandleRef swigCPtr;
}
