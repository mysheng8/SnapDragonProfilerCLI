using System;
using System.Runtime.InteropServices;

// Token: 0x02000065 RID: 101
public class ProcessProperties : IDisposable
{
	// Token: 0x06000681 RID: 1665 RVA: 0x00010EC4 File Offset: 0x0000F0C4
	internal ProcessProperties(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00010EE0 File Offset: 0x0000F0E0
	internal static HandleRef getCPtr(ProcessProperties obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00010EF8 File Offset: 0x0000F0F8
	~ProcessProperties()
	{
		this.Dispose();
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00010F24 File Offset: 0x0000F124
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessProperties(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000686 RID: 1670 RVA: 0x00010FC0 File Offset: 0x0000F1C0
	// (set) Token: 0x06000685 RID: 1669 RVA: 0x00010FA4 File Offset: 0x0000F1A4
	public string uid
	{
		get
		{
			string text = SDPCorePINVOKE.ProcessProperties_uid_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_uid_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000688 RID: 1672 RVA: 0x00011004 File Offset: 0x0000F204
	// (set) Token: 0x06000687 RID: 1671 RVA: 0x00010FE7 File Offset: 0x0000F1E7
	public string name
	{
		get
		{
			string text = SDPCorePINVOKE.ProcessProperties_name_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_name_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001103C File Offset: 0x0000F23C
	// (set) Token: 0x06000689 RID: 1673 RVA: 0x0001102B File Offset: 0x0000F22B
	public ProcessState state
	{
		get
		{
			return (ProcessState)SDPCorePINVOKE.ProcessProperties_state_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_state_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600068C RID: 1676 RVA: 0x00011064 File Offset: 0x0000F264
	// (set) Token: 0x0600068B RID: 1675 RVA: 0x00011056 File Offset: 0x0000F256
	public long icon
	{
		get
		{
			return SDPCorePINVOKE.ProcessProperties_icon_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_icon_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x0600068E RID: 1678 RVA: 0x0001108C File Offset: 0x0000F28C
	// (set) Token: 0x0600068D RID: 1677 RVA: 0x0001107E File Offset: 0x0000F27E
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ProcessProperties_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000690 RID: 1680 RVA: 0x000110B4 File Offset: 0x0000F2B4
	// (set) Token: 0x0600068F RID: 1679 RVA: 0x000110A6 File Offset: 0x0000F2A6
	public uint ppid
	{
		get
		{
			return SDPCorePINVOKE.ProcessProperties_ppid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_ppid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000692 RID: 1682 RVA: 0x000110DC File Offset: 0x0000F2DC
	// (set) Token: 0x06000691 RID: 1681 RVA: 0x000110CE File Offset: 0x0000F2CE
	public long created
	{
		get
		{
			return SDPCorePINVOKE.ProcessProperties_created_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_created_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000694 RID: 1684 RVA: 0x00011104 File Offset: 0x0000F304
	// (set) Token: 0x06000693 RID: 1683 RVA: 0x000110F6 File Offset: 0x0000F2F6
	public long lastUpdated
	{
		get
		{
			return SDPCorePINVOKE.ProcessProperties_lastUpdated_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_lastUpdated_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000696 RID: 1686 RVA: 0x0001112C File Offset: 0x0000F32C
	// (set) Token: 0x06000695 RID: 1685 RVA: 0x0001111E File Offset: 0x0000F31E
	public uint warningFlagsRealTime
	{
		get
		{
			return SDPCorePINVOKE.ProcessProperties_warningFlagsRealTime_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_warningFlagsRealTime_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000698 RID: 1688 RVA: 0x00011154 File Offset: 0x0000F354
	// (set) Token: 0x06000697 RID: 1687 RVA: 0x00011146 File Offset: 0x0000F346
	public uint warningFlagsTrace
	{
		get
		{
			return SDPCorePINVOKE.ProcessProperties_warningFlagsTrace_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_warningFlagsTrace_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001117C File Offset: 0x0000F37C
	// (set) Token: 0x06000699 RID: 1689 RVA: 0x0001116E File Offset: 0x0000F36E
	public uint warningFlagsSnapshot
	{
		get
		{
			return SDPCorePINVOKE.ProcessProperties_warningFlagsSnapshot_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_warningFlagsSnapshot_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x0600069C RID: 1692 RVA: 0x000111A4 File Offset: 0x0000F3A4
	// (set) Token: 0x0600069B RID: 1691 RVA: 0x00011196 File Offset: 0x0000F396
	public uint warningFlagsSampling
	{
		get
		{
			return SDPCorePINVOKE.ProcessProperties_warningFlagsSampling_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProperties_warningFlagsSampling_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x000111BE File Offset: 0x0000F3BE
	public ProcessProperties()
		: this(SDPCorePINVOKE.new_ProcessProperties(), true)
	{
	}

	// Token: 0x04000152 RID: 338
	private HandleRef swigCPtr;

	// Token: 0x04000153 RID: 339
	protected bool swigCMemOwn;
}
