using System;
using System.Runtime.InteropServices;

// Token: 0x02000070 RID: 112
public class ReplyMetricCategory : CommandMsg
{
	// Token: 0x06000736 RID: 1846 RVA: 0x00012535 File Offset: 0x00010735
	internal ReplyMetricCategory(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyMetricCategory_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00012551 File Offset: 0x00010751
	internal static HandleRef getCPtr(ReplyMetricCategory obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x00012568 File Offset: 0x00010768
	~ReplyMetricCategory()
	{
		this.Dispose();
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00012594 File Offset: 0x00010794
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyMetricCategory(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x0600073B RID: 1851 RVA: 0x00012628 File Offset: 0x00010828
	// (set) Token: 0x0600073A RID: 1850 RVA: 0x00012618 File Offset: 0x00010818
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricCategory_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricCategory_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x0600073D RID: 1853 RVA: 0x00012650 File Offset: 0x00010850
	// (set) Token: 0x0600073C RID: 1852 RVA: 0x00012642 File Offset: 0x00010842
	public uint categoryID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricCategory_categoryID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricCategory_categoryID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x0600073F RID: 1855 RVA: 0x00012678 File Offset: 0x00010878
	// (set) Token: 0x0600073E RID: 1854 RVA: 0x0001266A File Offset: 0x0001086A
	public uint parentID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricCategory_parentID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricCategory_parentID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000741 RID: 1857 RVA: 0x000126A0 File Offset: 0x000108A0
	// (set) Token: 0x06000740 RID: 1856 RVA: 0x00012692 File Offset: 0x00010892
	public string name
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricCategory_name_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricCategory_name_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000743 RID: 1859 RVA: 0x000126C8 File Offset: 0x000108C8
	// (set) Token: 0x06000742 RID: 1858 RVA: 0x000126BA File Offset: 0x000108BA
	public string description
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricCategory_description_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricCategory_description_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000745 RID: 1861 RVA: 0x00012704 File Offset: 0x00010904
	// (set) Token: 0x06000744 RID: 1860 RVA: 0x000126E2 File Offset: 0x000108E2
	public SWIGTYPE_p_uint8_t isEnabled
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ReplyMetricCategory_isEnabled_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricCategory_isEnabled_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00012731 File Offset: 0x00010931
	public ReplyMetricCategory(uint provider, uint category, uint parent, string categoryName, string categoryDesc, bool enabled)
		: this(SDPCorePINVOKE.new_ReplyMetricCategory(provider, category, parent, categoryName, categoryDesc, enabled), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000169 RID: 361
	private HandleRef swigCPtr;
}
