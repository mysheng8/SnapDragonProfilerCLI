using System;
using System.Runtime.InteropServices;

// Token: 0x02000077 RID: 119
public class ReplyOptionCategory : CommandMsg
{
	// Token: 0x06000793 RID: 1939 RVA: 0x00013138 File Offset: 0x00011338
	internal ReplyOptionCategory(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyOptionCategory_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00013154 File Offset: 0x00011354
	internal static HandleRef getCPtr(ReplyOptionCategory obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0001316C File Offset: 0x0001136C
	~ReplyOptionCategory()
	{
		this.Dispose();
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00013198 File Offset: 0x00011398
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyOptionCategory(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000798 RID: 1944 RVA: 0x0001322C File Offset: 0x0001142C
	// (set) Token: 0x06000797 RID: 1943 RVA: 0x0001321C File Offset: 0x0001141C
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionCategory_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionCategory_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x0600079A RID: 1946 RVA: 0x00013254 File Offset: 0x00011454
	// (set) Token: 0x06000799 RID: 1945 RVA: 0x00013246 File Offset: 0x00011446
	public uint categoryID
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionCategory_categoryID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionCategory_categoryID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x0600079C RID: 1948 RVA: 0x0001327C File Offset: 0x0001147C
	// (set) Token: 0x0600079B RID: 1947 RVA: 0x0001326E File Offset: 0x0001146E
	public uint parentCategoryID
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionCategory_parentCategoryID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionCategory_parentCategoryID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x0600079E RID: 1950 RVA: 0x000132A4 File Offset: 0x000114A4
	// (set) Token: 0x0600079D RID: 1949 RVA: 0x00013296 File Offset: 0x00011496
	public string name
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionCategory_name_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionCategory_name_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000132CC File Offset: 0x000114CC
	// (set) Token: 0x0600079F RID: 1951 RVA: 0x000132BE File Offset: 0x000114BE
	public string description
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionCategory_description_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionCategory_description_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x000132E6 File Offset: 0x000114E6
	public ReplyOptionCategory(uint provider, uint category, string categoryName, string categoryDesc, uint parentCategory)
		: this(SDPCorePINVOKE.new_ReplyOptionCategory__SWIG_0(provider, category, categoryName, categoryDesc, parentCategory), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00013308 File Offset: 0x00011508
	public ReplyOptionCategory(uint provider, uint category, string categoryName, string categoryDesc)
		: this(SDPCorePINVOKE.new_ReplyOptionCategory__SWIG_1(provider, category, categoryName, categoryDesc), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000170 RID: 368
	private HandleRef swigCPtr;
}
