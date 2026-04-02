using System;
using System.Runtime.InteropServices;

// Token: 0x02000029 RID: 41
public class SWIGTYPE_p_Data__SubBlockHeader
{
	// Token: 0x06000987 RID: 2439 RVA: 0x00019F26 File Offset: 0x00018126
	internal SWIGTYPE_p_Data__SubBlockHeader(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00019F3B File Offset: 0x0001813B
	protected SWIGTYPE_p_Data__SubBlockHeader()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00019F54 File Offset: 0x00018154
	internal static HandleRef getCPtr(SWIGTYPE_p_Data__SubBlockHeader obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B4E RID: 2894
	private HandleRef swigCPtr;
}
