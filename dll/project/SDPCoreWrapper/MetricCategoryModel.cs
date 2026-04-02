using System;
using System.Runtime.InteropServices;

// Token: 0x0200003B RID: 59
public class MetricCategoryModel : IDisposable
{
	// Token: 0x06000376 RID: 886 RVA: 0x00009A02 File Offset: 0x00007C02
	internal MetricCategoryModel(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00009A1E File Offset: 0x00007C1E
	internal static HandleRef getCPtr(MetricCategoryModel obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00009A38 File Offset: 0x00007C38
	~MetricCategoryModel()
	{
		this.Dispose();
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00009A64 File Offset: 0x00007C64
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricCategoryModel(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00009AE4 File Offset: 0x00007CE4
	public MetricCategoryModel()
		: this(SDPCorePINVOKE.new_MetricCategoryModel(), true)
	{
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600037C RID: 892 RVA: 0x00009B00 File Offset: 0x00007D00
	// (set) Token: 0x0600037B RID: 891 RVA: 0x00009AF2 File Offset: 0x00007CF2
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.MetricCategoryModel_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricCategoryModel_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x0600037E RID: 894 RVA: 0x00009B38 File Offset: 0x00007D38
	// (set) Token: 0x0600037D RID: 893 RVA: 0x00009B1A File Offset: 0x00007D1A
	public string name
	{
		get
		{
			string text = SDPCorePINVOKE.MetricCategoryModel_name_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.MetricCategoryModel_name_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x06000380 RID: 896 RVA: 0x00009B7C File Offset: 0x00007D7C
	// (set) Token: 0x0600037F RID: 895 RVA: 0x00009B5F File Offset: 0x00007D5F
	public string description
	{
		get
		{
			string text = SDPCorePINVOKE.MetricCategoryModel_description_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.MetricCategoryModel_description_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x040000EE RID: 238
	private HandleRef swigCPtr;

	// Token: 0x040000EF RID: 239
	protected bool swigCMemOwn;
}
