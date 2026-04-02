using System;
using System.Runtime.InteropServices;

// Token: 0x02000026 RID: 38
public class SWIGTYPE_p_Data__FunctionCallDesc
{
	// Token: 0x0600097E RID: 2430 RVA: 0x00019E57 File Offset: 0x00018057
	internal SWIGTYPE_p_Data__FunctionCallDesc(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00019E6C File Offset: 0x0001806C
	protected SWIGTYPE_p_Data__FunctionCallDesc()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00019E85 File Offset: 0x00018085
	internal static HandleRef getCPtr(SWIGTYPE_p_Data__FunctionCallDesc obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B4B RID: 2891
	private HandleRef swigCPtr;
}
