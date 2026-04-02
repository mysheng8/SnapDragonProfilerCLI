using System;
using System.Runtime.InteropServices;

// Token: 0x020000A6 RID: 166
public class SWIGTYPE_p_SDP__ProcessManagerDelegate
{
	// Token: 0x060013CC RID: 5068 RVA: 0x000189BB File Offset: 0x00016BBB
	internal SWIGTYPE_p_SDP__ProcessManagerDelegate(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x000189D0 File Offset: 0x00016BD0
	protected SWIGTYPE_p_SDP__ProcessManagerDelegate()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x000189E9 File Offset: 0x00016BE9
	internal static HandleRef getCPtr(SWIGTYPE_p_SDP__ProcessManagerDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D7 RID: 471
	private HandleRef swigCPtr;
}
