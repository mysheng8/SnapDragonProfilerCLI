using System;
using System.Runtime.InteropServices;

// Token: 0x02000061 RID: 97
public class ProcessManager : IDisposable
{
	// Token: 0x06000654 RID: 1620 RVA: 0x0001083E File Offset: 0x0000EA3E
	internal ProcessManager(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x0001085A File Offset: 0x0000EA5A
	internal static HandleRef getCPtr(ProcessManager obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x00010874 File Offset: 0x0000EA74
	~ProcessManager()
	{
		this.Dispose();
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x000108A0 File Offset: 0x0000EAA0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessManager(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00010920 File Offset: 0x0000EB20
	public static ProcessManager Get()
	{
		return new ProcessManager(SDPCorePINVOKE.ProcessManager_Get(), false);
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0001093A File Offset: 0x0000EB3A
	public void DisableDataModel()
	{
		SDPCorePINVOKE.ProcessManager_DisableDataModel(this.swigCPtr);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00010947 File Offset: 0x0000EB47
	public void RegisterEventDelegate(SWIGTYPE_p_SDP__ProcessManagerDelegate arg0)
	{
		SDPCorePINVOKE.ProcessManager_RegisterEventDelegate(this.swigCPtr, SWIGTYPE_p_SDP__ProcessManagerDelegate.getCPtr(arg0));
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0001095A File Offset: 0x0000EB5A
	public void Reset()
	{
		SDPCorePINVOKE.ProcessManager_Reset(this.swigCPtr);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00010967 File Offset: 0x0000EB67
	public void ShutDown()
	{
		SDPCorePINVOKE.ProcessManager_ShutDown(this.swigCPtr);
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00010974 File Offset: 0x0000EB74
	public Process AddProcess(uint pid, uint ppid, string name, string user, long timestamp, uint warningFlagsRealTime, uint warningFlagsTrace, uint warningFlagSnapshot, uint warningFlagSampling)
	{
		Process process = new Process(SDPCorePINVOKE.ProcessManager_AddProcess(this.swigCPtr, pid, ppid, name, user, timestamp, warningFlagsRealTime, warningFlagsTrace, warningFlagSnapshot, warningFlagSampling), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return process;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x000109B0 File Offset: 0x0000EBB0
	public bool RemoveProcess(uint pid, long timestamp)
	{
		return SDPCorePINVOKE.ProcessManager_RemoveProcess(this.swigCPtr, pid, timestamp);
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x000109CC File Offset: 0x0000EBCC
	public Process GetProcess(uint pid)
	{
		return new Process(SDPCorePINVOKE.ProcessManager_GetProcess(this.swigCPtr, pid), true);
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x000109F0 File Offset: 0x0000EBF0
	public Process GetProcessByName(string name)
	{
		Process process = new Process(SDPCorePINVOKE.ProcessManager_GetProcessByName(this.swigCPtr, name), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return process;
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00010A20 File Offset: 0x0000EC20
	public ProcessList GetAllProcesses()
	{
		return new ProcessList(SDPCorePINVOKE.ProcessManager_GetAllProcesses(this.swigCPtr), true);
	}

	// Token: 0x0400014A RID: 330
	private HandleRef swigCPtr;

	// Token: 0x0400014B RID: 331
	protected bool swigCMemOwn;
}
