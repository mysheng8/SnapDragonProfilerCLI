using System;
using System.Runtime.InteropServices;

// Token: 0x02000037 RID: 55
public class Metric : IDisposable
{
	// Token: 0x06000313 RID: 787 RVA: 0x00008CB8 File Offset: 0x00006EB8
	internal Metric(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00008CD4 File Offset: 0x00006ED4
	internal static HandleRef getCPtr(Metric obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00008CEC File Offset: 0x00006EEC
	~Metric()
	{
		this.Dispose();
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00008D18 File Offset: 0x00006F18
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_Metric(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00008D98 File Offset: 0x00006F98
	public Metric()
		: this(SDPCorePINVOKE.new_Metric__SWIG_0(), true)
	{
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00008DA6 File Offset: 0x00006FA6
	public Metric(Metric m)
		: this(SDPCorePINVOKE.new_Metric__SWIG_1(Metric.getCPtr(m)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00008DC7 File Offset: 0x00006FC7
	public Metric(MetricProperties props)
		: this(SDPCorePINVOKE.new_Metric__SWIG_2(MetricProperties.getCPtr(props)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00008DE8 File Offset: 0x00006FE8
	public void Equal(Metric m)
	{
		SDPCorePINVOKE.Metric_Equal(this.swigCPtr, Metric.getCPtr(m));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00008E08 File Offset: 0x00007008
	public bool IsValid()
	{
		return SDPCorePINVOKE.Metric_IsValid(this.swigCPtr);
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00008E24 File Offset: 0x00007024
	public MetricProperties GetProperties()
	{
		return new MetricProperties(SDPCorePINVOKE.Metric_GetProperties(this.swigCPtr), false);
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00008E44 File Offset: 0x00007044
	public bool IsActive(uint pid, uint captureType)
	{
		return SDPCorePINVOKE.Metric_IsActive(this.swigCPtr, pid, captureType);
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00008E60 File Offset: 0x00007060
	public bool IsPaused()
	{
		return SDPCorePINVOKE.Metric_IsPaused(this.swigCPtr);
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00008E7C File Offset: 0x0000707C
	public bool IsGlobal()
	{
		return SDPCorePINVOKE.Metric_IsGlobal(this.swigCPtr);
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00008E98 File Offset: 0x00007098
	public bool IsRealtimeMetric()
	{
		return SDPCorePINVOKE.Metric_IsRealtimeMetric(this.swigCPtr);
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00008EB4 File Offset: 0x000070B4
	public bool IsTraceMetric()
	{
		return SDPCorePINVOKE.Metric_IsTraceMetric(this.swigCPtr);
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00008ED0 File Offset: 0x000070D0
	public bool IsSnapshotMetric()
	{
		return SDPCorePINVOKE.Metric_IsSnapshotMetric(this.swigCPtr);
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00008EEC File Offset: 0x000070EC
	public bool Activate(uint pid, uint captureType)
	{
		return SDPCorePINVOKE.Metric_Activate__SWIG_0(this.swigCPtr, pid, captureType);
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00008F08 File Offset: 0x00007108
	public bool Activate(uint pid)
	{
		return SDPCorePINVOKE.Metric_Activate__SWIG_1(this.swigCPtr, pid);
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00008F24 File Offset: 0x00007124
	public bool Activate()
	{
		return SDPCorePINVOKE.Metric_Activate__SWIG_2(this.swigCPtr);
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00008F40 File Offset: 0x00007140
	public bool Deactivate(uint pid, uint captureType)
	{
		return SDPCorePINVOKE.Metric_Deactivate__SWIG_0(this.swigCPtr, pid, captureType);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00008F5C File Offset: 0x0000715C
	public bool Deactivate(uint pid)
	{
		return SDPCorePINVOKE.Metric_Deactivate__SWIG_1(this.swigCPtr, pid);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00008F78 File Offset: 0x00007178
	public bool Deactivate()
	{
		return SDPCorePINVOKE.Metric_Deactivate__SWIG_2(this.swigCPtr);
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00008F94 File Offset: 0x00007194
	public bool Pause(bool paused)
	{
		return SDPCorePINVOKE.Metric_Pause__SWIG_0(this.swigCPtr, paused);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00008FB0 File Offset: 0x000071B0
	public bool Pause()
	{
		return SDPCorePINVOKE.Metric_Pause__SWIG_1(this.swigCPtr);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00008FCC File Offset: 0x000071CC
	public bool IsLogReady(long now)
	{
		return SDPCorePINVOKE.Metric_IsLogReady(this.swigCPtr, now);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00008FE8 File Offset: 0x000071E8
	public bool Log(uint arg0, long timestamp, double value, uint pid)
	{
		return SDPCorePINVOKE.Metric_Log__SWIG_0(this.swigCPtr, arg0, timestamp, value, pid);
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00009008 File Offset: 0x00007208
	public bool Log(uint arg0, long timestamp, double value)
	{
		return SDPCorePINVOKE.Metric_Log__SWIG_1(this.swigCPtr, arg0, timestamp, value);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00009028 File Offset: 0x00007228
	public bool Log(uint captureID, long timestamp, double value, uint pid, uint tid)
	{
		return SDPCorePINVOKE.Metric_Log__SWIG_2(this.swigCPtr, captureID, timestamp, value, pid, tid);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0000904C File Offset: 0x0000724C
	public double GetValue()
	{
		return SDPCorePINVOKE.Metric_GetValue(this.swigCPtr);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00009068 File Offset: 0x00007268
	public IDList GetActiveProcesses(uint captureType)
	{
		return new IDList(SDPCorePINVOKE.Metric_GetActiveProcesses__SWIG_0(this.swigCPtr, captureType), true);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0000908C File Offset: 0x0000728C
	public IDList GetActiveProcesses()
	{
		return new IDList(SDPCorePINVOKE.Metric_GetActiveProcesses__SWIG_1(this.swigCPtr), true);
	}

	// Token: 0x06000332 RID: 818 RVA: 0x000090AC File Offset: 0x000072AC
	public bool SetHidden(bool hide)
	{
		return SDPCorePINVOKE.Metric_SetHidden(this.swigCPtr, hide);
	}

	// Token: 0x040000E0 RID: 224
	private HandleRef swigCPtr;

	// Token: 0x040000E1 RID: 225
	protected bool swigCMemOwn;
}
