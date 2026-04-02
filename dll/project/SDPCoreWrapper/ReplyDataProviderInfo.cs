using System;
using System.Runtime.InteropServices;

// Token: 0x0200006D RID: 109
public class ReplyDataProviderInfo : CommandMsg
{
	// Token: 0x06000702 RID: 1794 RVA: 0x00011E40 File Offset: 0x00010040
	internal ReplyDataProviderInfo(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyDataProviderInfo_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00011E5C File Offset: 0x0001005C
	internal static HandleRef getCPtr(ReplyDataProviderInfo obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00011E74 File Offset: 0x00010074
	~ReplyDataProviderInfo()
	{
		this.Dispose();
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00011EA0 File Offset: 0x000100A0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyDataProviderInfo(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000707 RID: 1799 RVA: 0x00011F38 File Offset: 0x00010138
	// (set) Token: 0x06000706 RID: 1798 RVA: 0x00011F24 File Offset: 0x00010124
	public ProviderDesc providerDesc
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.ReplyDataProviderInfo_providerDesc_get(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new ProviderDesc(intPtr, false);
		}
		set
		{
			SDPCorePINVOKE.ReplyDataProviderInfo_providerDesc_set(this.swigCPtr, ProviderDesc.getCPtr(value));
		}
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00011F6A File Offset: 0x0001016A
	public ReplyDataProviderInfo(ProviderDesc desc)
		: this(SDPCorePINVOKE.new_ReplyDataProviderInfo(ProviderDesc.getCPtr(desc)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000166 RID: 358
	private HandleRef swigCPtr;
}
