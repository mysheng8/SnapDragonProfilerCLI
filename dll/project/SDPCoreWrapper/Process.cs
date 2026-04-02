using System;
using System.Runtime.InteropServices;

// Token: 0x0200005D RID: 93
public class Process : IDisposable
{
	// Token: 0x060005F1 RID: 1521 RVA: 0x0000FAD5 File Offset: 0x0000DCD5
	internal Process(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0000FAF1 File Offset: 0x0000DCF1
	internal static HandleRef getCPtr(Process obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0000FB08 File Offset: 0x0000DD08
	~Process()
	{
		this.Dispose();
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0000FB34 File Offset: 0x0000DD34
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_Process(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0000FBB4 File Offset: 0x0000DDB4
	public Process(Process p)
		: this(SDPCorePINVOKE.new_Process(Process.getCPtr(p)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0000FBD5 File Offset: 0x0000DDD5
	public void Equal(Process p)
	{
		SDPCorePINVOKE.Process_Equal(this.swigCPtr, Process.getCPtr(p));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0000FBF8 File Offset: 0x0000DDF8
	public bool IsValid()
	{
		return SDPCorePINVOKE.Process_IsValid(this.swigCPtr);
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0000FC14 File Offset: 0x0000DE14
	public ProcessProperties GetProperties()
	{
		return new ProcessProperties(SDPCorePINVOKE.Process_GetProperties(this.swigCPtr), false);
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0000FC34 File Offset: 0x0000DE34
	public bool IsProviderLinked(uint providerID)
	{
		return SDPCorePINVOKE.Process_IsProviderLinked(this.swigCPtr, providerID);
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0000FC50 File Offset: 0x0000DE50
	public MetricIDList GetLinkedProviders()
	{
		return new MetricIDList(SDPCorePINVOKE.Process_GetLinkedProviders(this.swigCPtr), true);
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0000FC70 File Offset: 0x0000DE70
	public bool IsMetricLinked(uint metricID)
	{
		return SDPCorePINVOKE.Process_IsMetricLinked(this.swigCPtr, metricID);
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0000FC8C File Offset: 0x0000DE8C
	public MetricIDList GetLinkedMetrics()
	{
		return new MetricIDList(SDPCorePINVOKE.Process_GetLinkedMetrics(this.swigCPtr), true);
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0000FCAC File Offset: 0x0000DEAC
	public SWIGTYPE_p_uint8_t GetIcon()
	{
		IntPtr intPtr = SDPCorePINVOKE.Process_GetIcon(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_uint8_t(intPtr, false);
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0000FCE0 File Offset: 0x0000DEE0
	public bool SetIcon(SWIGTYPE_p_uint8_t icon)
	{
		return SDPCorePINVOKE.Process_SetIcon(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(icon));
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0000FD00 File Offset: 0x0000DF00
	public bool SetState(ProcessState newState, long timestamp)
	{
		return SDPCorePINVOKE.Process_SetState(this.swigCPtr, (int)newState, timestamp);
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0000FD1C File Offset: 0x0000DF1C
	public bool LinkToProvider(uint providerID)
	{
		return SDPCorePINVOKE.Process_LinkToProvider(this.swigCPtr, providerID);
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0000FD38 File Offset: 0x0000DF38
	public bool LinkToMetric(uint metricID)
	{
		return SDPCorePINVOKE.Process_LinkToMetric(this.swigCPtr, metricID);
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0000FD53 File Offset: 0x0000DF53
	public void AddRealTimeWarning(uint warning)
	{
		SDPCorePINVOKE.Process_AddRealTimeWarning(this.swigCPtr, warning);
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0000FD61 File Offset: 0x0000DF61
	public void AddTraceWarning(uint warning)
	{
		SDPCorePINVOKE.Process_AddTraceWarning(this.swigCPtr, warning);
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0000FD6F File Offset: 0x0000DF6F
	public void AddSnapshotWarning(uint warning)
	{
		SDPCorePINVOKE.Process_AddSnapshotWarning(this.swigCPtr, warning);
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0000FD7D File Offset: 0x0000DF7D
	public void AddSamplingWarning(uint warning)
	{
		SDPCorePINVOKE.Process_AddSamplingWarning(this.swigCPtr, warning);
	}

	// Token: 0x04000143 RID: 323
	private HandleRef swigCPtr;

	// Token: 0x04000144 RID: 324
	protected bool swigCMemOwn;
}
