using System;
using System.Runtime.InteropServices;

// Token: 0x02000086 RID: 134
public class RequestMetricStatus : CommandMsg
{
	// Token: 0x06000827 RID: 2087 RVA: 0x00014609 File Offset: 0x00012809
	internal RequestMetricStatus(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestMetricStatus_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00014625 File Offset: 0x00012825
	internal static HandleRef getCPtr(RequestMetricStatus obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0001463C File Offset: 0x0001283C
	~RequestMetricStatus()
	{
		this.Dispose();
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x00014668 File Offset: 0x00012868
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestMetricStatus(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x0600082C RID: 2092 RVA: 0x000146FC File Offset: 0x000128FC
	// (set) Token: 0x0600082B RID: 2091 RVA: 0x000146EC File Offset: 0x000128EC
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.RequestMetricStatus_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestMetricStatus_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00014716 File Offset: 0x00012916
	public RequestMetricStatus(uint metric)
		: this(SDPCorePINVOKE.new_RequestMetricStatus(metric), true)
	{
	}

	// Token: 0x0400017F RID: 383
	private HandleRef swigCPtr;
}
