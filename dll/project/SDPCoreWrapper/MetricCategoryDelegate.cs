using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x02000039 RID: 57
public class MetricCategoryDelegate : IDisposable
{
	// Token: 0x06000340 RID: 832 RVA: 0x000092B4 File Offset: 0x000074B4
	internal MetricCategoryDelegate(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x000092D0 File Offset: 0x000074D0
	internal static HandleRef getCPtr(MetricCategoryDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000342 RID: 834 RVA: 0x000092E8 File Offset: 0x000074E8
	~MetricCategoryDelegate()
	{
		this.Dispose();
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00009314 File Offset: 0x00007514
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricCategoryDelegate(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00009394 File Offset: 0x00007594
	public virtual void OnMetricCategoryAdded(uint categoryID)
	{
		SDPCorePINVOKE.MetricCategoryDelegate_OnMetricCategoryAdded(this.swigCPtr, categoryID);
	}

	// Token: 0x06000345 RID: 837 RVA: 0x000093A2 File Offset: 0x000075A2
	public virtual void OnMetricCategoryListReceived()
	{
		SDPCorePINVOKE.MetricCategoryDelegate_OnMetricCategoryListReceived(this.swigCPtr);
	}

	// Token: 0x06000346 RID: 838 RVA: 0x000093AF File Offset: 0x000075AF
	public virtual void OnMetricCategoryActivated(uint categoryID)
	{
		SDPCorePINVOKE.MetricCategoryDelegate_OnMetricCategoryActivated(this.swigCPtr, categoryID);
	}

	// Token: 0x06000347 RID: 839 RVA: 0x000093BD File Offset: 0x000075BD
	public MetricCategoryDelegate()
		: this(SDPCorePINVOKE.new_MetricCategoryDelegate(), true)
	{
		this.SwigDirectorConnect();
	}

	// Token: 0x06000348 RID: 840 RVA: 0x000093D4 File Offset: 0x000075D4
	private void SwigDirectorConnect()
	{
		if (this.SwigDerivedClassHasMethod("OnMetricCategoryAdded", MetricCategoryDelegate.swigMethodTypes0))
		{
			this.swigDelegate0 = new MetricCategoryDelegate.SwigDelegateMetricCategoryDelegate_0(this.SwigDirectorOnMetricCategoryAdded);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricCategoryListReceived", MetricCategoryDelegate.swigMethodTypes1))
		{
			this.swigDelegate1 = new MetricCategoryDelegate.SwigDelegateMetricCategoryDelegate_1(this.SwigDirectorOnMetricCategoryListReceived);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricCategoryActivated", MetricCategoryDelegate.swigMethodTypes2))
		{
			this.swigDelegate2 = new MetricCategoryDelegate.SwigDelegateMetricCategoryDelegate_2(this.SwigDirectorOnMetricCategoryActivated);
		}
		SDPCorePINVOKE.MetricCategoryDelegate_director_connect(this.swigCPtr, this.swigDelegate0, this.swigDelegate1, this.swigDelegate2);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x0000946C File Offset: 0x0000766C
	private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes)
	{
		MethodInfo method = base.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, methodTypes, null);
		return method.DeclaringType.IsSubclassOf(typeof(MetricCategoryDelegate));
	}

	// Token: 0x0600034A RID: 842 RVA: 0x000094A2 File Offset: 0x000076A2
	private void SwigDirectorOnMetricCategoryAdded(uint categoryID)
	{
		this.OnMetricCategoryAdded(categoryID);
	}

	// Token: 0x0600034B RID: 843 RVA: 0x000094AB File Offset: 0x000076AB
	private void SwigDirectorOnMetricCategoryListReceived()
	{
		this.OnMetricCategoryListReceived();
	}

	// Token: 0x0600034C RID: 844 RVA: 0x000094B3 File Offset: 0x000076B3
	private void SwigDirectorOnMetricCategoryActivated(uint categoryID)
	{
		this.OnMetricCategoryActivated(categoryID);
	}

	// Token: 0x040000E4 RID: 228
	private HandleRef swigCPtr;

	// Token: 0x040000E5 RID: 229
	protected bool swigCMemOwn;

	// Token: 0x040000E6 RID: 230
	private MetricCategoryDelegate.SwigDelegateMetricCategoryDelegate_0 swigDelegate0;

	// Token: 0x040000E7 RID: 231
	private MetricCategoryDelegate.SwigDelegateMetricCategoryDelegate_1 swigDelegate1;

	// Token: 0x040000E8 RID: 232
	private MetricCategoryDelegate.SwigDelegateMetricCategoryDelegate_2 swigDelegate2;

	// Token: 0x040000E9 RID: 233
	private static Type[] swigMethodTypes0 = new Type[] { typeof(uint) };

	// Token: 0x040000EA RID: 234
	private static Type[] swigMethodTypes1 = new Type[0];

	// Token: 0x040000EB RID: 235
	private static Type[] swigMethodTypes2 = new Type[] { typeof(uint) };

	// Token: 0x020000DA RID: 218
	// (Invoke) Token: 0x060014C7 RID: 5319
	public delegate void SwigDelegateMetricCategoryDelegate_0(uint categoryID);

	// Token: 0x020000DB RID: 219
	// (Invoke) Token: 0x060014CB RID: 5323
	public delegate void SwigDelegateMetricCategoryDelegate_1();

	// Token: 0x020000DC RID: 220
	// (Invoke) Token: 0x060014CF RID: 5327
	public delegate void SwigDelegateMetricCategoryDelegate_2(uint categoryID);
}
