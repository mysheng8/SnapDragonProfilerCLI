using System;
using System.Runtime.InteropServices;

// Token: 0x02000079 RID: 121
public class ReportBufferRegistered : CommandMsg
{
	// Token: 0x060007AE RID: 1966 RVA: 0x000134A4 File Offset: 0x000116A4
	internal ReportBufferRegistered(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReportBufferRegistered_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x000134C0 File Offset: 0x000116C0
	internal static HandleRef getCPtr(ReportBufferRegistered obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x000134D8 File Offset: 0x000116D8
	~ReportBufferRegistered()
	{
		this.Dispose();
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00013504 File Offset: 0x00011704
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReportBufferRegistered(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00013598 File Offset: 0x00011798
	// (set) Token: 0x060007B2 RID: 1970 RVA: 0x00013588 File Offset: 0x00011788
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReportBufferRegistered_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportBufferRegistered_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x060007B5 RID: 1973 RVA: 0x000135C8 File Offset: 0x000117C8
	// (set) Token: 0x060007B4 RID: 1972 RVA: 0x000135B2 File Offset: 0x000117B2
	public BufferKey bufferKey
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.ReportBufferRegistered_bufferKey_get(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new BufferKey(intPtr, false);
		}
		set
		{
			SDPCorePINVOKE.ReportBufferRegistered_bufferKey_set(this.swigCPtr, BufferKey.getCPtr(value));
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00013608 File Offset: 0x00011808
	// (set) Token: 0x060007B6 RID: 1974 RVA: 0x000135FA File Offset: 0x000117FA
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.ReportBufferRegistered_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportBufferRegistered_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00013622 File Offset: 0x00011822
	public ReportBufferRegistered(uint provider, uint captureID, BufferKey key)
		: this(SDPCorePINVOKE.new_ReportBufferRegistered(provider, captureID, BufferKey.getCPtr(key)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000172 RID: 370
	private HandleRef swigCPtr;
}
