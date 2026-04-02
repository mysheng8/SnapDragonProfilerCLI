using System;
using System.Runtime.InteropServices;

// Token: 0x02000038 RID: 56
public class MetricCategory : IDisposable
{
	// Token: 0x06000333 RID: 819 RVA: 0x000090C7 File Offset: 0x000072C7
	internal MetricCategory(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000334 RID: 820 RVA: 0x000090E3 File Offset: 0x000072E3
	internal static HandleRef getCPtr(MetricCategory obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000335 RID: 821 RVA: 0x000090FC File Offset: 0x000072FC
	~MetricCategory()
	{
		this.Dispose();
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00009128 File Offset: 0x00007328
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricCategory(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000337 RID: 823 RVA: 0x000091A8 File Offset: 0x000073A8
	public MetricCategory()
		: this(SDPCorePINVOKE.new_MetricCategory__SWIG_0(), true)
	{
	}

	// Token: 0x06000338 RID: 824 RVA: 0x000091B6 File Offset: 0x000073B6
	public MetricCategory(MetricCategoryProperties props)
		: this(SDPCorePINVOKE.new_MetricCategory__SWIG_1(MetricCategoryProperties.getCPtr(props)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000339 RID: 825 RVA: 0x000091D7 File Offset: 0x000073D7
	public MetricCategory(MetricCategory c)
		: this(SDPCorePINVOKE.new_MetricCategory__SWIG_2(MetricCategory.getCPtr(c)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600033A RID: 826 RVA: 0x000091F8 File Offset: 0x000073F8
	public void Equal(MetricCategory c)
	{
		SDPCorePINVOKE.MetricCategory_Equal(this.swigCPtr, MetricCategory.getCPtr(c));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00009218 File Offset: 0x00007418
	public bool IsValid()
	{
		return SDPCorePINVOKE.MetricCategory_IsValid(this.swigCPtr);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00009234 File Offset: 0x00007434
	public MetricCategoryProperties GetProperties()
	{
		return new MetricCategoryProperties(SDPCorePINVOKE.MetricCategory_GetProperties(this.swigCPtr), false);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00009254 File Offset: 0x00007454
	public MetricList GetMetrics()
	{
		return new MetricList(SDPCorePINVOKE.MetricCategory_GetMetrics(this.swigCPtr), true);
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00009274 File Offset: 0x00007474
	public MetricCategory GetParent()
	{
		return new MetricCategory(SDPCorePINVOKE.MetricCategory_GetParent(this.swigCPtr), true);
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00009294 File Offset: 0x00007494
	public MetricCategoryList GetChildren()
	{
		return new MetricCategoryList(SDPCorePINVOKE.MetricCategory_GetChildren(this.swigCPtr), true);
	}

	// Token: 0x040000E2 RID: 226
	private HandleRef swigCPtr;

	// Token: 0x040000E3 RID: 227
	protected bool swigCMemOwn;
}
