using System;
using System.Runtime.InteropServices;

// Token: 0x02000089 RID: 137
public class RequestOptions : CommandMsg
{
	// Token: 0x0600083E RID: 2110 RVA: 0x00014986 File Offset: 0x00012B86
	internal RequestOptions(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestOptions_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x000149A2 File Offset: 0x00012BA2
	internal static HandleRef getCPtr(RequestOptions obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x000149BC File Offset: 0x00012BBC
	~RequestOptions()
	{
		this.Dispose();
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x000149E8 File Offset: 0x00012BE8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestOptions(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000843 RID: 2115 RVA: 0x00014A7C File Offset: 0x00012C7C
	// (set) Token: 0x06000842 RID: 2114 RVA: 0x00014A6C File Offset: 0x00012C6C
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.RequestOptions_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestOptions_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00014A96 File Offset: 0x00012C96
	public RequestOptions(uint provider)
		: this(SDPCorePINVOKE.new_RequestOptions(provider), true)
	{
	}

	// Token: 0x04000182 RID: 386
	private HandleRef swigCPtr;
}
