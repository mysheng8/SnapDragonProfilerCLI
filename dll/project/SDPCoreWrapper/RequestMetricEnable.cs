using System;
using System.Runtime.InteropServices;

// Token: 0x02000083 RID: 131
public class RequestMetricEnable : CommandMsg
{
	// Token: 0x0600080A RID: 2058 RVA: 0x000141C1 File Offset: 0x000123C1
	internal RequestMetricEnable(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestMetricEnable_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x000141DD File Offset: 0x000123DD
	internal static HandleRef getCPtr(RequestMetricEnable obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x000141F4 File Offset: 0x000123F4
	~RequestMetricEnable()
	{
		this.Dispose();
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00014220 File Offset: 0x00012420
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestMetricEnable(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x0600080F RID: 2063 RVA: 0x000142B4 File Offset: 0x000124B4
	// (set) Token: 0x0600080E RID: 2062 RVA: 0x000142A4 File Offset: 0x000124A4
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.RequestMetricEnable_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestMetricEnable_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000811 RID: 2065 RVA: 0x000142DC File Offset: 0x000124DC
	// (set) Token: 0x06000810 RID: 2064 RVA: 0x000142CE File Offset: 0x000124CE
	public uint processID
	{
		get
		{
			return SDPCorePINVOKE.RequestMetricEnable_processID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestMetricEnable_processID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000813 RID: 2067 RVA: 0x00014304 File Offset: 0x00012504
	// (set) Token: 0x06000812 RID: 2066 RVA: 0x000142F6 File Offset: 0x000124F6
	public uint captureState
	{
		get
		{
			return SDPCorePINVOKE.RequestMetricEnable_captureState_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestMetricEnable_captureState_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000815 RID: 2069 RVA: 0x00014340 File Offset: 0x00012540
	// (set) Token: 0x06000814 RID: 2068 RVA: 0x0001431E File Offset: 0x0001251E
	public SWIGTYPE_p_uint8_t enable
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.RequestMetricEnable_enable_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.RequestMetricEnable_enable_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0001436D File Offset: 0x0001256D
	public RequestMetricEnable(uint metric, uint process, uint capState, bool enableState)
		: this(SDPCorePINVOKE.new_RequestMetricEnable(metric, process, capState, enableState), true)
	{
	}

	// Token: 0x0400017C RID: 380
	private HandleRef swigCPtr;
}
