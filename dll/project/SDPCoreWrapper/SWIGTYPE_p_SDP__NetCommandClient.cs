using System;
using System.Runtime.InteropServices;

// Token: 0x020000A3 RID: 163
public class SWIGTYPE_p_SDP__NetCommandClient
{
	// Token: 0x060013C3 RID: 5059 RVA: 0x000188EC File Offset: 0x00016AEC
	internal SWIGTYPE_p_SDP__NetCommandClient(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x00018901 File Offset: 0x00016B01
	protected SWIGTYPE_p_SDP__NetCommandClient()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x0001891A File Offset: 0x00016B1A
	internal static HandleRef getCPtr(SWIGTYPE_p_SDP__NetCommandClient obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D4 RID: 468
	private HandleRef swigCPtr;
}
