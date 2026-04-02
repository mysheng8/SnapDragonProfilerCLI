using System;
using System.Runtime.InteropServices;

// Token: 0x0200006F RID: 111
public class ReplyMetricAvailable : CommandMsg
{
	// Token: 0x0600072D RID: 1837 RVA: 0x000123C7 File Offset: 0x000105C7
	internal ReplyMetricAvailable(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyMetricAvailable_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x000123E3 File Offset: 0x000105E3
	internal static HandleRef getCPtr(ReplyMetricAvailable obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x000123FC File Offset: 0x000105FC
	~ReplyMetricAvailable()
	{
		this.Dispose();
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00012428 File Offset: 0x00010628
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyMetricAvailable(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000732 RID: 1842 RVA: 0x000124BC File Offset: 0x000106BC
	// (set) Token: 0x06000731 RID: 1841 RVA: 0x000124AC File Offset: 0x000106AC
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricAvailable_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricAvailable_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000734 RID: 1844 RVA: 0x000124F8 File Offset: 0x000106F8
	// (set) Token: 0x06000733 RID: 1843 RVA: 0x000124D6 File Offset: 0x000106D6
	public SWIGTYPE_p_uint8_t available
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ReplyMetricAvailable_available_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricAvailable_available_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00012525 File Offset: 0x00010725
	public ReplyMetricAvailable(uint metric, bool avail)
		: this(SDPCorePINVOKE.new_ReplyMetricAvailable(metric, avail), true)
	{
	}

	// Token: 0x04000168 RID: 360
	private HandleRef swigCPtr;
}
