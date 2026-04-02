using System;
using System.Runtime.InteropServices;

// Token: 0x02000028 RID: 40
public class SWIGTYPE_p_Data__MethodCallDesc
{
	// Token: 0x06000984 RID: 2436 RVA: 0x00019EE1 File Offset: 0x000180E1
	internal SWIGTYPE_p_Data__MethodCallDesc(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x00019EF6 File Offset: 0x000180F6
	protected SWIGTYPE_p_Data__MethodCallDesc()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x00019F0F File Offset: 0x0001810F
	internal static HandleRef getCPtr(SWIGTYPE_p_Data__MethodCallDesc obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B4D RID: 2893
	private HandleRef swigCPtr;
}
