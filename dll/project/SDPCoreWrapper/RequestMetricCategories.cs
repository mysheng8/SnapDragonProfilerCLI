using System;
using System.Runtime.InteropServices;

// Token: 0x02000082 RID: 130
public class RequestMetricCategories : CommandMsg
{
	// Token: 0x06000803 RID: 2051 RVA: 0x000140A2 File Offset: 0x000122A2
	internal RequestMetricCategories(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestMetricCategories_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x000140BE File Offset: 0x000122BE
	internal static HandleRef getCPtr(RequestMetricCategories obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x000140D8 File Offset: 0x000122D8
	~RequestMetricCategories()
	{
		this.Dispose();
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00014104 File Offset: 0x00012304
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestMetricCategories(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000808 RID: 2056 RVA: 0x00014198 File Offset: 0x00012398
	// (set) Token: 0x06000807 RID: 2055 RVA: 0x00014188 File Offset: 0x00012388
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.RequestMetricCategories_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestMetricCategories_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x000141B2 File Offset: 0x000123B2
	public RequestMetricCategories(uint provider)
		: this(SDPCorePINVOKE.new_RequestMetricCategories(provider), true)
	{
	}

	// Token: 0x0400017B RID: 379
	private HandleRef swigCPtr;
}
