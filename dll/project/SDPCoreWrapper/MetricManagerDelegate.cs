using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x02000041 RID: 65
public class MetricManagerDelegate : IDisposable
{
	// Token: 0x06000408 RID: 1032 RVA: 0x0000AFC8 File Offset: 0x000091C8
	internal MetricManagerDelegate(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0000AFE4 File Offset: 0x000091E4
	internal static HandleRef getCPtr(MetricManagerDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0000AFFC File Offset: 0x000091FC
	~MetricManagerDelegate()
	{
		this.Dispose();
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0000B028 File Offset: 0x00009228
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricManagerDelegate(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0000B0A8 File Offset: 0x000092A8
	public virtual void OnMetricCategoryAdded(uint categoryID)
	{
		SDPCorePINVOKE.MetricManagerDelegate_OnMetricCategoryAdded(this.swigCPtr, categoryID);
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x0000B0B6 File Offset: 0x000092B6
	public virtual void OnMetricCategoryListReceived()
	{
		SDPCorePINVOKE.MetricManagerDelegate_OnMetricCategoryListReceived(this.swigCPtr);
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x0000B0C3 File Offset: 0x000092C3
	public virtual void OnMetricCategoryActivated(uint categoryID)
	{
		SDPCorePINVOKE.MetricManagerDelegate_OnMetricCategoryActivated(this.swigCPtr, categoryID);
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0000B0D1 File Offset: 0x000092D1
	public virtual void OnMetricAdded(uint metricID)
	{
		SDPCorePINVOKE.MetricManagerDelegate_OnMetricAdded(this.swigCPtr, metricID);
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x0000B0DF File Offset: 0x000092DF
	public virtual void OnMetricListReceived(uint providerID)
	{
		SDPCorePINVOKE.MetricManagerDelegate_OnMetricListReceived(this.swigCPtr, providerID);
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0000B0ED File Offset: 0x000092ED
	public virtual void OnMetricActivated(uint metricID)
	{
		SDPCorePINVOKE.MetricManagerDelegate_OnMetricActivated(this.swigCPtr, metricID);
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0000B0FB File Offset: 0x000092FB
	public virtual void OnMetricDeactivated(uint metricID)
	{
		SDPCorePINVOKE.MetricManagerDelegate_OnMetricDeactivated(this.swigCPtr, metricID);
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0000B109 File Offset: 0x00009309
	public virtual void OnMetricDataReceived(uint metricID, uint pid, uint tid, long timestamp, double value)
	{
		SDPCorePINVOKE.MetricManagerDelegate_OnMetricDataReceived(this.swigCPtr, metricID, pid, tid, timestamp, value);
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0000B11D File Offset: 0x0000931D
	public MetricManagerDelegate()
		: this(SDPCorePINVOKE.new_MetricManagerDelegate(), true)
	{
		this.SwigDirectorConnect();
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0000B134 File Offset: 0x00009334
	private void SwigDirectorConnect()
	{
		if (this.SwigDerivedClassHasMethod("OnMetricCategoryAdded", MetricManagerDelegate.swigMethodTypes0))
		{
			this.swigDelegate0 = new MetricManagerDelegate.SwigDelegateMetricManagerDelegate_0(this.SwigDirectorOnMetricCategoryAdded);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricCategoryListReceived", MetricManagerDelegate.swigMethodTypes1))
		{
			this.swigDelegate1 = new MetricManagerDelegate.SwigDelegateMetricManagerDelegate_1(this.SwigDirectorOnMetricCategoryListReceived);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricCategoryActivated", MetricManagerDelegate.swigMethodTypes2))
		{
			this.swigDelegate2 = new MetricManagerDelegate.SwigDelegateMetricManagerDelegate_2(this.SwigDirectorOnMetricCategoryActivated);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricAdded", MetricManagerDelegate.swigMethodTypes3))
		{
			this.swigDelegate3 = new MetricManagerDelegate.SwigDelegateMetricManagerDelegate_3(this.SwigDirectorOnMetricAdded);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricListReceived", MetricManagerDelegate.swigMethodTypes4))
		{
			this.swigDelegate4 = new MetricManagerDelegate.SwigDelegateMetricManagerDelegate_4(this.SwigDirectorOnMetricListReceived);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricActivated", MetricManagerDelegate.swigMethodTypes5))
		{
			this.swigDelegate5 = new MetricManagerDelegate.SwigDelegateMetricManagerDelegate_5(this.SwigDirectorOnMetricActivated);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricDeactivated", MetricManagerDelegate.swigMethodTypes6))
		{
			this.swigDelegate6 = new MetricManagerDelegate.SwigDelegateMetricManagerDelegate_6(this.SwigDirectorOnMetricDeactivated);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricDataReceived", MetricManagerDelegate.swigMethodTypes7))
		{
			this.swigDelegate7 = new MetricManagerDelegate.SwigDelegateMetricManagerDelegate_7(this.SwigDirectorOnMetricDataReceived);
		}
		SDPCorePINVOKE.MetricManagerDelegate_director_connect(this.swigCPtr, this.swigDelegate0, this.swigDelegate1, this.swigDelegate2, this.swigDelegate3, this.swigDelegate4, this.swigDelegate5, this.swigDelegate6, this.swigDelegate7);
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0000B29C File Offset: 0x0000949C
	private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes)
	{
		MethodInfo method = base.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, methodTypes, null);
		return method.DeclaringType.IsSubclassOf(typeof(MetricManagerDelegate));
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0000B2D2 File Offset: 0x000094D2
	private void SwigDirectorOnMetricCategoryAdded(uint categoryID)
	{
		this.OnMetricCategoryAdded(categoryID);
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x0000B2DB File Offset: 0x000094DB
	private void SwigDirectorOnMetricCategoryListReceived()
	{
		this.OnMetricCategoryListReceived();
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0000B2E3 File Offset: 0x000094E3
	private void SwigDirectorOnMetricCategoryActivated(uint categoryID)
	{
		this.OnMetricCategoryActivated(categoryID);
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0000B2EC File Offset: 0x000094EC
	private void SwigDirectorOnMetricAdded(uint metricID)
	{
		this.OnMetricAdded(metricID);
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0000B2F5 File Offset: 0x000094F5
	private void SwigDirectorOnMetricListReceived(uint providerID)
	{
		this.OnMetricListReceived(providerID);
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0000B2FE File Offset: 0x000094FE
	private void SwigDirectorOnMetricActivated(uint metricID)
	{
		this.OnMetricActivated(metricID);
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0000B307 File Offset: 0x00009507
	private void SwigDirectorOnMetricDeactivated(uint metricID)
	{
		this.OnMetricDeactivated(metricID);
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0000B310 File Offset: 0x00009510
	private void SwigDirectorOnMetricDataReceived(uint metricID, uint pid, uint tid, long timestamp, double value)
	{
		this.OnMetricDataReceived(metricID, pid, tid, timestamp, value);
	}

	// Token: 0x04000106 RID: 262
	private HandleRef swigCPtr;

	// Token: 0x04000107 RID: 263
	protected bool swigCMemOwn;

	// Token: 0x04000108 RID: 264
	private MetricManagerDelegate.SwigDelegateMetricManagerDelegate_0 swigDelegate0;

	// Token: 0x04000109 RID: 265
	private MetricManagerDelegate.SwigDelegateMetricManagerDelegate_1 swigDelegate1;

	// Token: 0x0400010A RID: 266
	private MetricManagerDelegate.SwigDelegateMetricManagerDelegate_2 swigDelegate2;

	// Token: 0x0400010B RID: 267
	private MetricManagerDelegate.SwigDelegateMetricManagerDelegate_3 swigDelegate3;

	// Token: 0x0400010C RID: 268
	private MetricManagerDelegate.SwigDelegateMetricManagerDelegate_4 swigDelegate4;

	// Token: 0x0400010D RID: 269
	private MetricManagerDelegate.SwigDelegateMetricManagerDelegate_5 swigDelegate5;

	// Token: 0x0400010E RID: 270
	private MetricManagerDelegate.SwigDelegateMetricManagerDelegate_6 swigDelegate6;

	// Token: 0x0400010F RID: 271
	private MetricManagerDelegate.SwigDelegateMetricManagerDelegate_7 swigDelegate7;

	// Token: 0x04000110 RID: 272
	private static Type[] swigMethodTypes0 = new Type[] { typeof(uint) };

	// Token: 0x04000111 RID: 273
	private static Type[] swigMethodTypes1 = new Type[0];

	// Token: 0x04000112 RID: 274
	private static Type[] swigMethodTypes2 = new Type[] { typeof(uint) };

	// Token: 0x04000113 RID: 275
	private static Type[] swigMethodTypes3 = new Type[] { typeof(uint) };

	// Token: 0x04000114 RID: 276
	private static Type[] swigMethodTypes4 = new Type[] { typeof(uint) };

	// Token: 0x04000115 RID: 277
	private static Type[] swigMethodTypes5 = new Type[] { typeof(uint) };

	// Token: 0x04000116 RID: 278
	private static Type[] swigMethodTypes6 = new Type[] { typeof(uint) };

	// Token: 0x04000117 RID: 279
	private static Type[] swigMethodTypes7 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(long),
		typeof(double)
	};

	// Token: 0x020000E7 RID: 231
	// (Invoke) Token: 0x06001504 RID: 5380
	public delegate void SwigDelegateMetricManagerDelegate_0(uint categoryID);

	// Token: 0x020000E8 RID: 232
	// (Invoke) Token: 0x06001508 RID: 5384
	public delegate void SwigDelegateMetricManagerDelegate_1();

	// Token: 0x020000E9 RID: 233
	// (Invoke) Token: 0x0600150C RID: 5388
	public delegate void SwigDelegateMetricManagerDelegate_2(uint categoryID);

	// Token: 0x020000EA RID: 234
	// (Invoke) Token: 0x06001510 RID: 5392
	public delegate void SwigDelegateMetricManagerDelegate_3(uint metricID);

	// Token: 0x020000EB RID: 235
	// (Invoke) Token: 0x06001514 RID: 5396
	public delegate void SwigDelegateMetricManagerDelegate_4(uint providerID);

	// Token: 0x020000EC RID: 236
	// (Invoke) Token: 0x06001518 RID: 5400
	public delegate void SwigDelegateMetricManagerDelegate_5(uint metricID);

	// Token: 0x020000ED RID: 237
	// (Invoke) Token: 0x0600151C RID: 5404
	public delegate void SwigDelegateMetricManagerDelegate_6(uint metricID);

	// Token: 0x020000EE RID: 238
	// (Invoke) Token: 0x06001520 RID: 5408
	public delegate void SwigDelegateMetricManagerDelegate_7(uint metricID, uint pid, uint tid, long timestamp, double value);
}
