using System;
using System.Runtime.InteropServices;

// Token: 0x020000A4 RID: 164
public class SWIGTYPE_p_SDP__NetCommandServer
{
	// Token: 0x060013C6 RID: 5062 RVA: 0x00018931 File Offset: 0x00016B31
	internal SWIGTYPE_p_SDP__NetCommandServer(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x00018946 File Offset: 0x00016B46
	protected SWIGTYPE_p_SDP__NetCommandServer()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x0001895F File Offset: 0x00016B5F
	internal static HandleRef getCPtr(SWIGTYPE_p_SDP__NetCommandServer obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D5 RID: 469
	private HandleRef swigCPtr;
}
