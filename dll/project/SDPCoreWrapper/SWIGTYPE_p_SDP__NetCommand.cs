using System;
using System.Runtime.InteropServices;

// Token: 0x020000A2 RID: 162
public class SWIGTYPE_p_SDP__NetCommand
{
	// Token: 0x060013C0 RID: 5056 RVA: 0x000188A7 File Offset: 0x00016AA7
	internal SWIGTYPE_p_SDP__NetCommand(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x000188BC File Offset: 0x00016ABC
	protected SWIGTYPE_p_SDP__NetCommand()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x000188D5 File Offset: 0x00016AD5
	internal static HandleRef getCPtr(SWIGTYPE_p_SDP__NetCommand obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D3 RID: 467
	private HandleRef swigCPtr;
}
