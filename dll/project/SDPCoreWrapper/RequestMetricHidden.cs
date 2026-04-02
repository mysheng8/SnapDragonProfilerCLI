using System;
using System.Runtime.InteropServices;

// Token: 0x02000084 RID: 132
public class RequestMetricHidden : CommandMsg
{
	// Token: 0x06000817 RID: 2071 RVA: 0x00014380 File Offset: 0x00012580
	internal RequestMetricHidden(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestMetricHidden_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x0001439C File Offset: 0x0001259C
	internal static HandleRef getCPtr(RequestMetricHidden obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x000143B4 File Offset: 0x000125B4
	~RequestMetricHidden()
	{
		this.Dispose();
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x000143E0 File Offset: 0x000125E0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestMetricHidden(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x0600081C RID: 2076 RVA: 0x00014474 File Offset: 0x00012674
	// (set) Token: 0x0600081B RID: 2075 RVA: 0x00014464 File Offset: 0x00012664
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.RequestMetricHidden_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestMetricHidden_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x0600081E RID: 2078 RVA: 0x000144B0 File Offset: 0x000126B0
	// (set) Token: 0x0600081D RID: 2077 RVA: 0x0001448E File Offset: 0x0001268E
	public SWIGTYPE_p_uint8_t hidden
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.RequestMetricHidden_hidden_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.RequestMetricHidden_hidden_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x000144DD File Offset: 0x000126DD
	public RequestMetricHidden(uint metric, bool hide)
		: this(SDPCorePINVOKE.new_RequestMetricHidden(metric, hide), true)
	{
	}

	// Token: 0x0400017D RID: 381
	private HandleRef swigCPtr;
}
