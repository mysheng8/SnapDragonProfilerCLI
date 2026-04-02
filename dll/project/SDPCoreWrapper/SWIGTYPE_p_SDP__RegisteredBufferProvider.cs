using System;
using System.Runtime.InteropServices;

// Token: 0x020000A7 RID: 167
public class SWIGTYPE_p_SDP__RegisteredBufferProvider
{
	// Token: 0x060013CF RID: 5071 RVA: 0x00018A00 File Offset: 0x00016C00
	internal SWIGTYPE_p_SDP__RegisteredBufferProvider(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x00018A15 File Offset: 0x00016C15
	protected SWIGTYPE_p_SDP__RegisteredBufferProvider()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00018A2E File Offset: 0x00016C2E
	internal static HandleRef getCPtr(SWIGTYPE_p_SDP__RegisteredBufferProvider obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D8 RID: 472
	private HandleRef swigCPtr;
}
