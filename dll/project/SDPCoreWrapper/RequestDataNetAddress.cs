using System;
using System.Runtime.InteropServices;

// Token: 0x02000080 RID: 128
public class RequestDataNetAddress : CommandMsg
{
	// Token: 0x060007F9 RID: 2041 RVA: 0x00013EBD File Offset: 0x000120BD
	internal RequestDataNetAddress(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestDataNetAddress_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00013ED9 File Offset: 0x000120D9
	internal static HandleRef getCPtr(RequestDataNetAddress obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00013EF0 File Offset: 0x000120F0
	~RequestDataNetAddress()
	{
		this.Dispose();
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00013F1C File Offset: 0x0001211C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestDataNetAddress(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00013FA0 File Offset: 0x000121A0
	public RequestDataNetAddress()
		: this(SDPCorePINVOKE.new_RequestDataNetAddress(), true)
	{
	}

	// Token: 0x04000179 RID: 377
	private HandleRef swigCPtr;
}
