using System;
using System.Runtime.InteropServices;

// Token: 0x020000A0 RID: 160
public class SWIGTYPE_p_SDP__CaptureManagerDelegate
{
	// Token: 0x060013BA RID: 5050 RVA: 0x0001881D File Offset: 0x00016A1D
	internal SWIGTYPE_p_SDP__CaptureManagerDelegate(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x00018832 File Offset: 0x00016A32
	protected SWIGTYPE_p_SDP__CaptureManagerDelegate()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013BC RID: 5052 RVA: 0x0001884B File Offset: 0x00016A4B
	internal static HandleRef getCPtr(SWIGTYPE_p_SDP__CaptureManagerDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D1 RID: 465
	private HandleRef swigCPtr;
}
