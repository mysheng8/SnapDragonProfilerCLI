using System;
using System.Runtime.InteropServices;

// Token: 0x02000071 RID: 113
public class ReplyMetricCategoryAvailable : CommandMsg
{
	// Token: 0x06000747 RID: 1863 RVA: 0x00012755 File Offset: 0x00010955
	internal ReplyMetricCategoryAvailable(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyMetricCategoryAvailable_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x00012771 File Offset: 0x00010971
	internal static HandleRef getCPtr(ReplyMetricCategoryAvailable obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00012788 File Offset: 0x00010988
	~ReplyMetricCategoryAvailable()
	{
		this.Dispose();
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x000127B4 File Offset: 0x000109B4
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyMetricCategoryAvailable(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x0600074C RID: 1868 RVA: 0x00012848 File Offset: 0x00010A48
	// (set) Token: 0x0600074B RID: 1867 RVA: 0x00012838 File Offset: 0x00010A38
	public uint categoryID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricCategoryAvailable_categoryID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricCategoryAvailable_categoryID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x0600074E RID: 1870 RVA: 0x00012884 File Offset: 0x00010A84
	// (set) Token: 0x0600074D RID: 1869 RVA: 0x00012862 File Offset: 0x00010A62
	public SWIGTYPE_p_uint8_t available
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ReplyMetricCategoryAvailable_available_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricCategoryAvailable_available_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x000128B1 File Offset: 0x00010AB1
	public ReplyMetricCategoryAvailable(uint category, bool avail)
		: this(SDPCorePINVOKE.new_ReplyMetricCategoryAvailable(category, avail), true)
	{
	}

	// Token: 0x0400016A RID: 362
	private HandleRef swigCPtr;
}
