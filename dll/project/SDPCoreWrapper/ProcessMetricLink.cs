using System;
using System.Runtime.InteropServices;

// Token: 0x02000062 RID: 98
public class ProcessMetricLink : IDisposable
{
	// Token: 0x06000662 RID: 1634 RVA: 0x00010A40 File Offset: 0x0000EC40
	internal ProcessMetricLink(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00010A5C File Offset: 0x0000EC5C
	internal static HandleRef getCPtr(ProcessMetricLink obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00010A74 File Offset: 0x0000EC74
	~ProcessMetricLink()
	{
		this.Dispose();
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00010AA0 File Offset: 0x0000ECA0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessMetricLink(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000667 RID: 1639 RVA: 0x00010B30 File Offset: 0x0000ED30
	// (set) Token: 0x06000666 RID: 1638 RVA: 0x00010B20 File Offset: 0x0000ED20
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ProcessMetricLink_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessMetricLink_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000669 RID: 1641 RVA: 0x00010B58 File Offset: 0x0000ED58
	// (set) Token: 0x06000668 RID: 1640 RVA: 0x00010B4A File Offset: 0x0000ED4A
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.ProcessMetricLink_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessMetricLink_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00010B72 File Offset: 0x0000ED72
	public ProcessMetricLink()
		: this(SDPCorePINVOKE.new_ProcessMetricLink(), true)
	{
	}

	// Token: 0x0400014C RID: 332
	private HandleRef swigCPtr;

	// Token: 0x0400014D RID: 333
	protected bool swigCMemOwn;
}
