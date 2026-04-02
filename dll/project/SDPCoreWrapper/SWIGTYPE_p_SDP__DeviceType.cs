using System;
using System.Runtime.InteropServices;

// Token: 0x020000A1 RID: 161
public class SWIGTYPE_p_SDP__DeviceType
{
	// Token: 0x060013BD RID: 5053 RVA: 0x00018862 File Offset: 0x00016A62
	internal SWIGTYPE_p_SDP__DeviceType(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x00018877 File Offset: 0x00016A77
	protected SWIGTYPE_p_SDP__DeviceType()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x00018890 File Offset: 0x00016A90
	internal static HandleRef getCPtr(SWIGTYPE_p_SDP__DeviceType obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001D2 RID: 466
	private HandleRef swigCPtr;
}
