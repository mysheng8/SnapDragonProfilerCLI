using System;
using System.Runtime.InteropServices;

// Token: 0x020000A8 RID: 168
public class SWIGTYPE_p_SDP__Thread
{
	// Token: 0x060013D2 RID: 5074 RVA: 0x00018A45 File Offset: 0x00016C45
	internal SWIGTYPE_p_SDP__Thread(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x00018A5A File Offset: 0x00016C5A
	protected SWIGTYPE_p_SDP__Thread()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x00018A73 File Offset: 0x00016C73
	internal static HandleRef getCPtr(SWIGTYPE_p_SDP__Thread obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D9 RID: 473
	private HandleRef swigCPtr;
}
