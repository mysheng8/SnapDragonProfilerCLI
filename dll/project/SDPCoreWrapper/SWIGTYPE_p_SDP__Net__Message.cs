using System;
using System.Runtime.InteropServices;

// Token: 0x020000A5 RID: 165
public class SWIGTYPE_p_SDP__Net__Message
{
	// Token: 0x060013C9 RID: 5065 RVA: 0x00018976 File Offset: 0x00016B76
	internal SWIGTYPE_p_SDP__Net__Message(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x0001898B File Offset: 0x00016B8B
	protected SWIGTYPE_p_SDP__Net__Message()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x000189A4 File Offset: 0x00016BA4
	internal static HandleRef getCPtr(SWIGTYPE_p_SDP__Net__Message obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D6 RID: 470
	private HandleRef swigCPtr;
}
