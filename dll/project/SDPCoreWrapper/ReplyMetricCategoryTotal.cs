using System;
using System.Runtime.InteropServices;

// Token: 0x02000072 RID: 114
public class ReplyMetricCategoryTotal : CommandMsg
{
	// Token: 0x06000750 RID: 1872 RVA: 0x000128C1 File Offset: 0x00010AC1
	internal ReplyMetricCategoryTotal(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyMetricCategoryTotal_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x000128DD File Offset: 0x00010ADD
	internal static HandleRef getCPtr(ReplyMetricCategoryTotal obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x000128F4 File Offset: 0x00010AF4
	~ReplyMetricCategoryTotal()
	{
		this.Dispose();
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00012920 File Offset: 0x00010B20
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyMetricCategoryTotal(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000755 RID: 1877 RVA: 0x000129B4 File Offset: 0x00010BB4
	// (set) Token: 0x06000754 RID: 1876 RVA: 0x000129A4 File Offset: 0x00010BA4
	public uint numCategories
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricCategoryTotal_numCategories_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricCategoryTotal_numCategories_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x000129CE File Offset: 0x00010BCE
	public ReplyMetricCategoryTotal(uint totalCatgories)
		: this(SDPCorePINVOKE.new_ReplyMetricCategoryTotal(totalCatgories), true)
	{
	}

	// Token: 0x0400016B RID: 363
	private HandleRef swigCPtr;
}
