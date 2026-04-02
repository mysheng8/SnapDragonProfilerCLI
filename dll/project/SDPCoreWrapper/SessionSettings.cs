using System;
using System.Runtime.InteropServices;

// Token: 0x02000095 RID: 149
public class SessionSettings : IDisposable
{
	// Token: 0x0600134D RID: 4941 RVA: 0x00017A63 File Offset: 0x00015C63
	internal SessionSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x00017A7F File Offset: 0x00015C7F
	internal static HandleRef getCPtr(SessionSettings obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x00017A98 File Offset: 0x00015C98
	~SessionSettings()
	{
		this.Dispose();
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x00017AC4 File Offset: 0x00015CC4
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_SessionSettings(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001352 RID: 4946 RVA: 0x00017B60 File Offset: 0x00015D60
	// (set) Token: 0x06001351 RID: 4945 RVA: 0x00017B44 File Offset: 0x00015D44
	public string SessionDirectoryRootPath
	{
		get
		{
			string text = SDPCorePINVOKE.SessionSettings_SessionDirectoryRootPath_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.SessionSettings_SessionDirectoryRootPath_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06001354 RID: 4948 RVA: 0x00017B98 File Offset: 0x00015D98
	// (set) Token: 0x06001353 RID: 4947 RVA: 0x00017B87 File Offset: 0x00015D87
	public uint MaxTotalSessionsSizeMB
	{
		get
		{
			return SDPCorePINVOKE.SessionSettings_MaxTotalSessionsSizeMB_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.SessionSettings_MaxTotalSessionsSizeMB_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06001356 RID: 4950 RVA: 0x00017BC0 File Offset: 0x00015DC0
	// (set) Token: 0x06001355 RID: 4949 RVA: 0x00017BB2 File Offset: 0x00015DB2
	public bool CreateTimestampedSubDirectory
	{
		get
		{
			return SDPCorePINVOKE.SessionSettings_CreateTimestampedSubDirectory_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.SessionSettings_CreateTimestampedSubDirectory_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x00017BDA File Offset: 0x00015DDA
	public SessionSettings()
		: this(SDPCorePINVOKE.new_SessionSettings(), true)
	{
	}

	// Token: 0x040001C4 RID: 452
	private HandleRef swigCPtr;

	// Token: 0x040001C5 RID: 453
	protected bool swigCMemOwn;
}
