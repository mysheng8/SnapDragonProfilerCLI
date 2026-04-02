using System;
using System.Runtime.InteropServices;

// Token: 0x02000027 RID: 39
public class SWIGTYPE_p_Data__MetadataDesc
{
	// Token: 0x06000981 RID: 2433 RVA: 0x00019E9C File Offset: 0x0001809C
	internal SWIGTYPE_p_Data__MetadataDesc(IntPtr cPtr, bool futureUse)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00019EB1 File Offset: 0x000180B1
	protected SWIGTYPE_p_Data__MetadataDesc()
	{
		this.swigCPtr = new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00019ECA File Offset: 0x000180CA
	internal static HandleRef getCPtr(SWIGTYPE_p_Data__MetadataDesc obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x04000B4C RID: 2892
	private HandleRef swigCPtr;
}
