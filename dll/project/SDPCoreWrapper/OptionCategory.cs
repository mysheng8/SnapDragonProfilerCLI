using System;
using System.Runtime.InteropServices;

// Token: 0x0200004F RID: 79
public class OptionCategory : CommonObject
{
	// Token: 0x06000521 RID: 1313 RVA: 0x0000D787 File Offset: 0x0000B987
	internal OptionCategory(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionCategory_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0000D7A3 File Offset: 0x0000B9A3
	internal static HandleRef getCPtr(OptionCategory obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0000D7BC File Offset: 0x0000B9BC
	~OptionCategory()
	{
		this.Dispose();
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0000D7E8 File Offset: 0x0000B9E8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionCategory(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0000D86C File Offset: 0x0000BA6C
	public OptionList GetOptions()
	{
		return new OptionList(SDPCorePINVOKE.OptionCategory_GetOptions(this.swigCPtr), false);
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0000D88C File Offset: 0x0000BA8C
	public OptionCategory GetParentCategory()
	{
		IntPtr intPtr = SDPCorePINVOKE.OptionCategory_GetParentCategory(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new OptionCategory(intPtr, false);
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
	public SWIGTYPE_p_std__vectorT_SDP__OptionCategory_p_t GetSubCategories()
	{
		return new SWIGTYPE_p_std__vectorT_SDP__OptionCategory_p_t(SDPCorePINVOKE.OptionCategory_GetSubCategories(this.swigCPtr), false);
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0000D8E0 File Offset: 0x0000BAE0
	public override bool Publish(SWIGTYPE_p_SDP__NetCommandClient network)
	{
		return SDPCorePINVOKE.OptionCategory_Publish(this.swigCPtr, SWIGTYPE_p_SDP__NetCommandClient.getCPtr(network));
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0000D900 File Offset: 0x0000BB00
	public override bool PublishStatus()
	{
		return SDPCorePINVOKE.OptionCategory_PublishStatus(this.swigCPtr);
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0000D91A File Offset: 0x0000BB1A
	public virtual void Update(string categoryName, string categoryDesc, OptionCategory parentCategory)
	{
		SDPCorePINVOKE.OptionCategory_Update__SWIG_0(this.swigCPtr, categoryName, categoryDesc, OptionCategory.getCPtr(parentCategory));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x0000D93C File Offset: 0x0000BB3C
	public virtual void Update(string categoryName, string categoryDesc)
	{
		SDPCorePINVOKE.OptionCategory_Update__SWIG_1(this.swigCPtr, categoryName, categoryDesc);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x0000D958 File Offset: 0x0000BB58
	public void Clear()
	{
		SDPCorePINVOKE.OptionCategory_Clear(this.swigCPtr);
	}

	// Token: 0x04000132 RID: 306
	private HandleRef swigCPtr;
}
