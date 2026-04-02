using System;
using System.Runtime.InteropServices;

// Token: 0x02000087 RID: 135
public class RequestOptionCategories : CommandMsg
{
	// Token: 0x0600082E RID: 2094 RVA: 0x00014725 File Offset: 0x00012925
	internal RequestOptionCategories(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestOptionCategories_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00014741 File Offset: 0x00012941
	internal static HandleRef getCPtr(RequestOptionCategories obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00014758 File Offset: 0x00012958
	~RequestOptionCategories()
	{
		this.Dispose();
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00014784 File Offset: 0x00012984
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestOptionCategories(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000833 RID: 2099 RVA: 0x00014818 File Offset: 0x00012A18
	// (set) Token: 0x06000832 RID: 2098 RVA: 0x00014808 File Offset: 0x00012A08
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.RequestOptionCategories_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestOptionCategories_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00014832 File Offset: 0x00012A32
	public RequestOptionCategories(uint provider)
		: this(SDPCorePINVOKE.new_RequestOptionCategories(provider), true)
	{
	}

	// Token: 0x04000180 RID: 384
	private HandleRef swigCPtr;
}
