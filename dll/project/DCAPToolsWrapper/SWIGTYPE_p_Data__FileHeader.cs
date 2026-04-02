using System;
using System.Runtime.InteropServices;

// Token: 0x02000025 RID: 37
public class SWIGTYPE_p_Data__FileHeader
{
	// Token: 0x0600097B RID: 2427 RVA: 0x00019E12 File Offset: 0x00018012
	internal SWIGTYPE_p_Data__FileHeader(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00019E27 File Offset: 0x00018027
	protected SWIGTYPE_p_Data__FileHeader()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00019E40 File Offset: 0x00018040
	internal static HandleRef getCPtr(SWIGTYPE_p_Data__FileHeader obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B4A RID: 2890
	private HandleRef swigCPtr;
}
