using System;
using System.Runtime.InteropServices;

// Token: 0x02000063 RID: 99
public class ProcessModel : IDisposable
{
	// Token: 0x0600066B RID: 1643 RVA: 0x00010B80 File Offset: 0x0000ED80
	internal ProcessModel(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00010B9C File Offset: 0x0000ED9C
	internal static HandleRef getCPtr(ProcessModel obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00010BB4 File Offset: 0x0000EDB4
	~ProcessModel()
	{
		this.Dispose();
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00010BE0 File Offset: 0x0000EDE0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessModel(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00010C60 File Offset: 0x0000EE60
	public ProcessModel()
		: this(SDPCorePINVOKE.new_ProcessModel(), true)
	{
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000671 RID: 1649 RVA: 0x00010C7C File Offset: 0x0000EE7C
	// (set) Token: 0x06000670 RID: 1648 RVA: 0x00010C6E File Offset: 0x0000EE6E
	public uint capture
	{
		get
		{
			return SDPCorePINVOKE.ProcessModel_capture_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessModel_capture_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000673 RID: 1651 RVA: 0x00010CA4 File Offset: 0x0000EEA4
	// (set) Token: 0x06000672 RID: 1650 RVA: 0x00010C96 File Offset: 0x0000EE96
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ProcessModel_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessModel_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000675 RID: 1653 RVA: 0x00010CDC File Offset: 0x0000EEDC
	// (set) Token: 0x06000674 RID: 1652 RVA: 0x00010CBE File Offset: 0x0000EEBE
	public string name
	{
		get
		{
			string text = SDPCorePINVOKE.ProcessModel_name_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.ProcessModel_name_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x0400014E RID: 334
	private HandleRef swigCPtr;

	// Token: 0x0400014F RID: 335
	protected bool swigCMemOwn;
}
