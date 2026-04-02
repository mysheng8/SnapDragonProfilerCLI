using System;
using System.Runtime.InteropServices;

// Token: 0x0200002A RID: 42
public class SWIGTYPE_p_p_unsigned_char
{
	// Token: 0x0600098A RID: 2442 RVA: 0x00019F6B File Offset: 0x0001816B
	internal SWIGTYPE_p_p_unsigned_char(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00019F80 File Offset: 0x00018180
	protected SWIGTYPE_p_p_unsigned_char()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00019F99 File Offset: 0x00018199
	internal static HandleRef getCPtr(SWIGTYPE_p_p_unsigned_char obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B4F RID: 2895
	private HandleRef swigCPtr;
}
