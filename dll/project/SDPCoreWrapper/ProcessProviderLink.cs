using System;
using System.Runtime.InteropServices;

// Token: 0x02000066 RID: 102
public class ProcessProviderLink : IDisposable
{
	// Token: 0x0600069E RID: 1694 RVA: 0x000111CC File Offset: 0x0000F3CC
	internal ProcessProviderLink(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x000111E8 File Offset: 0x0000F3E8
	internal static HandleRef getCPtr(ProcessProviderLink obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00011200 File Offset: 0x0000F400
	~ProcessProviderLink()
	{
		this.Dispose();
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0001122C File Offset: 0x0000F42C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessProviderLink(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060006A3 RID: 1699 RVA: 0x000112BC File Offset: 0x0000F4BC
	// (set) Token: 0x060006A2 RID: 1698 RVA: 0x000112AC File Offset: 0x0000F4AC
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ProcessProviderLink_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProviderLink_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060006A5 RID: 1701 RVA: 0x000112E4 File Offset: 0x0000F4E4
	// (set) Token: 0x060006A4 RID: 1700 RVA: 0x000112D6 File Offset: 0x0000F4D6
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ProcessProviderLink_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessProviderLink_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x000112FE File Offset: 0x0000F4FE
	public ProcessProviderLink()
		: this(SDPCorePINVOKE.new_ProcessProviderLink(), true)
	{
	}

	// Token: 0x04000154 RID: 340
	private HandleRef swigCPtr;

	// Token: 0x04000155 RID: 341
	protected bool swigCMemOwn;
}
