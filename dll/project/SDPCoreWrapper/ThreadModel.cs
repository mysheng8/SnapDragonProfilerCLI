using System;
using System.Runtime.InteropServices;

// Token: 0x020000BB RID: 187
public class ThreadModel : IDisposable
{
	// Token: 0x06001434 RID: 5172 RVA: 0x000194C0 File Offset: 0x000176C0
	internal ThreadModel(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x000194DC File Offset: 0x000176DC
	internal static HandleRef getCPtr(ThreadModel obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x000194F4 File Offset: 0x000176F4
	~ThreadModel()
	{
		this.Dispose();
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x00019520 File Offset: 0x00017720
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ThreadModel(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x000195A0 File Offset: 0x000177A0
	public ThreadModel()
		: this(SDPCorePINVOKE.new_ThreadModel(), true)
	{
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x0600143A RID: 5178 RVA: 0x000195BC File Offset: 0x000177BC
	// (set) Token: 0x06001439 RID: 5177 RVA: 0x000195AE File Offset: 0x000177AE
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ThreadModel_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ThreadModel_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x0600143C RID: 5180 RVA: 0x000195E4 File Offset: 0x000177E4
	// (set) Token: 0x0600143B RID: 5179 RVA: 0x000195D6 File Offset: 0x000177D6
	public uint tid
	{
		get
		{
			return SDPCorePINVOKE.ThreadModel_tid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ThreadModel_tid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x040001ED RID: 493
	private HandleRef swigCPtr;

	// Token: 0x040001EE RID: 494
	protected bool swigCMemOwn;
}
