using System;
using System.Runtime.InteropServices;

// Token: 0x0200009B RID: 155
public class SWIGTYPE_p_float
{
	// Token: 0x060013AB RID: 5035 RVA: 0x000186C4 File Offset: 0x000168C4
	internal SWIGTYPE_p_float(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060013AC RID: 5036 RVA: 0x000186D9 File Offset: 0x000168D9
	protected SWIGTYPE_p_float()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x000186F2 File Offset: 0x000168F2
	internal static HandleRef getCPtr(SWIGTYPE_p_float obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x040001CC RID: 460
	private HandleRef swigCPtr;
}
