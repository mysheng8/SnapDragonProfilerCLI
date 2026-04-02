using System;
using System.Runtime.InteropServices;

// Token: 0x0200007F RID: 127
public class RequestCustomData : CommandMsg
{
	// Token: 0x060007F2 RID: 2034 RVA: 0x00013D9E File Offset: 0x00011F9E
	internal RequestCustomData(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestCustomData_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x00013DBA File Offset: 0x00011FBA
	internal static HandleRef getCPtr(RequestCustomData obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00013DD4 File Offset: 0x00011FD4
	~RequestCustomData()
	{
		this.Dispose();
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00013E00 File Offset: 0x00012000
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestCustomData(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00013E94 File Offset: 0x00012094
	// (set) Token: 0x060007F6 RID: 2038 RVA: 0x00013E84 File Offset: 0x00012084
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.RequestCustomData_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestCustomData_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00013EAE File Offset: 0x000120AE
	public RequestCustomData(uint provider)
		: this(SDPCorePINVOKE.new_RequestCustomData(provider), true)
	{
	}

	// Token: 0x04000178 RID: 376
	private HandleRef swigCPtr;
}
