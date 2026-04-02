using System;
using System.Runtime.InteropServices;

// Token: 0x0200000E RID: 14
public class AppStartResponse : IDisposable
{
	// Token: 0x0600002D RID: 45 RVA: 0x00002048 File Offset: 0x00000248
	internal AppStartResponse(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002064 File Offset: 0x00000264
	internal static HandleRef getCPtr(AppStartResponse obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x0000207C File Offset: 0x0000027C
	~AppStartResponse()
	{
		this.Dispose();
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000020A8 File Offset: 0x000002A8
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_AppStartResponse(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000032 RID: 50 RVA: 0x00002138 File Offset: 0x00000338
	// (set) Token: 0x06000031 RID: 49 RVA: 0x00002128 File Offset: 0x00000328
	public bool result
	{
		get
		{
			return SDPCorePINVOKE.AppStartResponse_result_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.AppStartResponse_result_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000034 RID: 52 RVA: 0x00002160 File Offset: 0x00000360
	// (set) Token: 0x06000033 RID: 51 RVA: 0x00002152 File Offset: 0x00000352
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.AppStartResponse_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.AppStartResponse_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x0000217A File Offset: 0x0000037A
	public AppStartResponse(bool result, uint pid)
		: this(SDPCorePINVOKE.new_AppStartResponse__SWIG_0(result, pid), true)
	{
	}

	// Token: 0x06000036 RID: 54 RVA: 0x0000218A File Offset: 0x0000038A
	public AppStartResponse(bool result)
		: this(SDPCorePINVOKE.new_AppStartResponse__SWIG_1(result), true)
	{
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002199 File Offset: 0x00000399
	public AppStartResponse()
		: this(SDPCorePINVOKE.new_AppStartResponse__SWIG_2(), true)
	{
	}

	// Token: 0x04000004 RID: 4
	private HandleRef swigCPtr;

	// Token: 0x04000005 RID: 5
	protected bool swigCMemOwn;
}
