using System;
using System.Runtime.InteropServices;

// Token: 0x02000067 RID: 103
public class ProcessRemoved : CommandMsg
{
	// Token: 0x060006A7 RID: 1703 RVA: 0x0001130C File Offset: 0x0000F50C
	internal ProcessRemoved(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ProcessRemoved_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00011328 File Offset: 0x0000F528
	internal static HandleRef getCPtr(ProcessRemoved obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00011340 File Offset: 0x0000F540
	~ProcessRemoved()
	{
		this.Dispose();
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x0001136C File Offset: 0x0000F56C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessRemoved(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060006AC RID: 1708 RVA: 0x00011400 File Offset: 0x0000F600
	// (set) Token: 0x060006AB RID: 1707 RVA: 0x000113F0 File Offset: 0x0000F5F0
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ProcessRemoved_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessRemoved_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060006AE RID: 1710 RVA: 0x00011428 File Offset: 0x0000F628
	// (set) Token: 0x060006AD RID: 1709 RVA: 0x0001141A File Offset: 0x0000F61A
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ProcessRemoved_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessRemoved_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00011442 File Offset: 0x0000F642
	public ProcessRemoved(uint uid, uint provider)
		: this(SDPCorePINVOKE.new_ProcessRemoved__SWIG_0(uid, provider), true)
	{
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00011452 File Offset: 0x0000F652
	public ProcessRemoved(uint uid)
		: this(SDPCorePINVOKE.new_ProcessRemoved__SWIG_1(uid), true)
	{
	}

	// Token: 0x04000156 RID: 342
	private HandleRef swigCPtr;
}
