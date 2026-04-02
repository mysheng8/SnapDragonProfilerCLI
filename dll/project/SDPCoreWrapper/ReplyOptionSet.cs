using System;
using System.Runtime.InteropServices;

// Token: 0x02000078 RID: 120
public class ReplyOptionSet : CommandMsg
{
	// Token: 0x060007A3 RID: 1955 RVA: 0x00013328 File Offset: 0x00011528
	internal ReplyOptionSet(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyOptionSet_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x00013344 File Offset: 0x00011544
	internal static HandleRef getCPtr(ReplyOptionSet obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0001335C File Offset: 0x0001155C
	~ReplyOptionSet()
	{
		this.Dispose();
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x00013388 File Offset: 0x00011588
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyOptionSet(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001341C File Offset: 0x0001161C
	// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0001340C File Offset: 0x0001160C
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionSet_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionSet_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x060007AA RID: 1962 RVA: 0x00013444 File Offset: 0x00011644
	// (set) Token: 0x060007A9 RID: 1961 RVA: 0x00013436 File Offset: 0x00011636
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionSet_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionSet_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x060007AC RID: 1964 RVA: 0x0001346C File Offset: 0x0001166C
	// (set) Token: 0x060007AB RID: 1963 RVA: 0x0001345E File Offset: 0x0001165E
	public string value
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionSet_value_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionSet_value_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00013486 File Offset: 0x00011686
	public ReplyOptionSet(uint optionID, uint processId, string val)
		: this(SDPCorePINVOKE.new_ReplyOptionSet(optionID, processId, val), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000171 RID: 369
	private HandleRef swigCPtr;
}
