using System;
using System.Runtime.InteropServices;

// Token: 0x02000085 RID: 133
public class RequestMetrics : CommandMsg
{
	// Token: 0x06000820 RID: 2080 RVA: 0x000144ED File Offset: 0x000126ED
	internal RequestMetrics(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestMetrics_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00014509 File Offset: 0x00012709
	internal static HandleRef getCPtr(RequestMetrics obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00014520 File Offset: 0x00012720
	~RequestMetrics()
	{
		this.Dispose();
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0001454C File Offset: 0x0001274C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestMetrics(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000825 RID: 2085 RVA: 0x000145E0 File Offset: 0x000127E0
	// (set) Token: 0x06000824 RID: 2084 RVA: 0x000145D0 File Offset: 0x000127D0
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.RequestMetrics_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestMetrics_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x000145FA File Offset: 0x000127FA
	public RequestMetrics(uint provider)
		: this(SDPCorePINVOKE.new_RequestMetrics(provider), true)
	{
	}

	// Token: 0x0400017E RID: 382
	private HandleRef swigCPtr;
}
