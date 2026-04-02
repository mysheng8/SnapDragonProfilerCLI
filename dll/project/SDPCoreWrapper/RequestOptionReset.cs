using System;
using System.Runtime.InteropServices;

// Token: 0x02000088 RID: 136
public class RequestOptionReset : CommandMsg
{
	// Token: 0x06000835 RID: 2101 RVA: 0x00014841 File Offset: 0x00012A41
	internal RequestOptionReset(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.RequestOptionReset_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0001485D File Offset: 0x00012A5D
	internal static HandleRef getCPtr(RequestOptionReset obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x00014874 File Offset: 0x00012A74
	~RequestOptionReset()
	{
		this.Dispose();
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x000148A0 File Offset: 0x00012AA0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_RequestOptionReset(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x0600083A RID: 2106 RVA: 0x00014934 File Offset: 0x00012B34
	// (set) Token: 0x06000839 RID: 2105 RVA: 0x00014924 File Offset: 0x00012B24
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.RequestOptionReset_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestOptionReset_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x0600083C RID: 2108 RVA: 0x0001495C File Offset: 0x00012B5C
	// (set) Token: 0x0600083B RID: 2107 RVA: 0x0001494E File Offset: 0x00012B4E
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.RequestOptionReset_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.RequestOptionReset_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00014976 File Offset: 0x00012B76
	public RequestOptionReset(uint optionID, uint processId)
		: this(SDPCorePINVOKE.new_RequestOptionReset(optionID, processId), true)
	{
	}

	// Token: 0x04000181 RID: 385
	private HandleRef swigCPtr;
}
