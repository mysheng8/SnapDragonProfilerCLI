using System;
using System.Runtime.InteropServices;

// Token: 0x02000073 RID: 115
public class ReplyMetricStatus : CommandMsg
{
	// Token: 0x06000757 RID: 1879 RVA: 0x000129DD File Offset: 0x00010BDD
	internal ReplyMetricStatus(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyMetricStatus_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x000129F9 File Offset: 0x00010BF9
	internal static HandleRef getCPtr(ReplyMetricStatus obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00012A10 File Offset: 0x00010C10
	~ReplyMetricStatus()
	{
		this.Dispose();
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00012A3C File Offset: 0x00010C3C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyMetricStatus(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x0600075C RID: 1884 RVA: 0x00012AD0 File Offset: 0x00010CD0
	// (set) Token: 0x0600075B RID: 1883 RVA: 0x00012AC0 File Offset: 0x00010CC0
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricStatus_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricStatus_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x0600075E RID: 1886 RVA: 0x00012B0C File Offset: 0x00010D0C
	// (set) Token: 0x0600075D RID: 1885 RVA: 0x00012AEA File Offset: 0x00010CEA
	public SWIGTYPE_p_uint8_t status
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ReplyMetricStatus_status_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricStatus_status_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00012B39 File Offset: 0x00010D39
	public ReplyMetricStatus(uint metric, bool enabled)
		: this(SDPCorePINVOKE.new_ReplyMetricStatus(metric, enabled), true)
	{
	}

	// Token: 0x0400016C RID: 364
	private HandleRef swigCPtr;
}
