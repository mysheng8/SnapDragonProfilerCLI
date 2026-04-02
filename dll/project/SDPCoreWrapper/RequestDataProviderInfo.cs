using System;
using System.Runtime.InteropServices;

// Token: 0x02000081 RID: 129
public class RequestDataProviderInfo : CommandMsg
{
	// Token: 0x060007FE RID: 2046 RVA: 0x00013FAE File Offset: 0x000121AE
	internal RequestDataProviderInfo(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestDataProviderInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00013FCA File Offset: 0x000121CA
	internal static HandleRef getCPtr(RequestDataProviderInfo obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00013FE4 File Offset: 0x000121E4
	~RequestDataProviderInfo()
	{
		this.Dispose();
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00014010 File Offset: 0x00012210
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestDataProviderInfo(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00014094 File Offset: 0x00012294
	public RequestDataProviderInfo()
		: this(SDPCorePINVOKE.new_RequestDataProviderInfo(), true)
	{
	}

	// Token: 0x0400017A RID: 378
	private HandleRef swigCPtr;
}
