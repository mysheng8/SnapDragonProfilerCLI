using System;
using System.Runtime.InteropServices;

// Token: 0x0200008A RID: 138
public class RequestProcessStatus : CommandMsg
{
	// Token: 0x06000845 RID: 2117 RVA: 0x00014AA5 File Offset: 0x00012CA5
	internal RequestProcessStatus(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestProcessStatus_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00014AC1 File Offset: 0x00012CC1
	internal static HandleRef getCPtr(RequestProcessStatus obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00014AD8 File Offset: 0x00012CD8
	~RequestProcessStatus()
	{
		this.Dispose();
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00014B04 File Offset: 0x00012D04
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestProcessStatus(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x0600084A RID: 2122 RVA: 0x00014B98 File Offset: 0x00012D98
	// (set) Token: 0x06000849 RID: 2121 RVA: 0x00014B88 File Offset: 0x00012D88
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.RequestProcessStatus_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestProcessStatus_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00014BB2 File Offset: 0x00012DB2
	public RequestProcessStatus(uint process)
		: this(SDPCorePINVOKE.new_RequestProcessStatus(process), true)
	{
	}

	// Token: 0x04000183 RID: 387
	private HandleRef swigCPtr;
}
