using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x0200003D RID: 61
public class MetricDelegate : IDisposable
{
	// Token: 0x06000396 RID: 918 RVA: 0x00009E7B File Offset: 0x0000807B
	internal MetricDelegate(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000397 RID: 919 RVA: 0x00009E97 File Offset: 0x00008097
	internal static HandleRef getCPtr(MetricDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00009EB0 File Offset: 0x000080B0
	~MetricDelegate()
	{
		this.Dispose();
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00009EDC File Offset: 0x000080DC
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricDelegate(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00009F5C File Offset: 0x0000815C
	public virtual void OnMetricAdded(uint metricID)
	{
		if (this.SwigDerivedClassHasMethod("OnMetricAdded", MetricDelegate.swigMethodTypes0))
		{
			SDPCorePINVOKE.MetricDelegate_OnMetricAddedSwigExplicitMetricDelegate(this.swigCPtr, metricID);
			return;
		}
		SDPCorePINVOKE.MetricDelegate_OnMetricAdded(this.swigCPtr, metricID);
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00009F89 File Offset: 0x00008189
	public virtual void OnMetricListReceived(uint providerID)
	{
		if (this.SwigDerivedClassHasMethod("OnMetricListReceived", MetricDelegate.swigMethodTypes1))
		{
			SDPCorePINVOKE.MetricDelegate_OnMetricListReceivedSwigExplicitMetricDelegate(this.swigCPtr, providerID);
			return;
		}
		SDPCorePINVOKE.MetricDelegate_OnMetricListReceived(this.swigCPtr, providerID);
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00009FB6 File Offset: 0x000081B6
	public virtual void OnMetricActivated(uint metricID, uint pid)
	{
		if (this.SwigDerivedClassHasMethod("OnMetricActivated", MetricDelegate.swigMethodTypes2))
		{
			SDPCorePINVOKE.MetricDelegate_OnMetricActivatedSwigExplicitMetricDelegate(this.swigCPtr, metricID, pid);
			return;
		}
		SDPCorePINVOKE.MetricDelegate_OnMetricActivated(this.swigCPtr, metricID, pid);
	}

	// Token: 0x0600039D RID: 925 RVA: 0x00009FE5 File Offset: 0x000081E5
	public virtual void OnMetricDeactivated(uint metricID, uint pid)
	{
		if (this.SwigDerivedClassHasMethod("OnMetricDeactivated", MetricDelegate.swigMethodTypes3))
		{
			SDPCorePINVOKE.MetricDelegate_OnMetricDeactivatedSwigExplicitMetricDelegate(this.swigCPtr, metricID, pid);
			return;
		}
		SDPCorePINVOKE.MetricDelegate_OnMetricDeactivated(this.swigCPtr, metricID, pid);
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0000A014 File Offset: 0x00008214
	public virtual void OnMetricHiddenToggled(uint metricID, bool hidden)
	{
		if (this.SwigDerivedClassHasMethod("OnMetricHiddenToggled", MetricDelegate.swigMethodTypes4))
		{
			SDPCorePINVOKE.MetricDelegate_OnMetricHiddenToggledSwigExplicitMetricDelegate(this.swigCPtr, metricID, hidden);
			return;
		}
		SDPCorePINVOKE.MetricDelegate_OnMetricHiddenToggled(this.swigCPtr, metricID, hidden);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0000A043 File Offset: 0x00008243
	public virtual void OnMetricDataReceived(uint metricID, uint pid, uint tid, long timestamp, double value)
	{
		if (this.SwigDerivedClassHasMethod("OnMetricDataReceived", MetricDelegate.swigMethodTypes5))
		{
			SDPCorePINVOKE.MetricDelegate_OnMetricDataReceivedSwigExplicitMetricDelegate(this.swigCPtr, metricID, pid, tid, timestamp, value);
			return;
		}
		SDPCorePINVOKE.MetricDelegate_OnMetricDataReceived(this.swigCPtr, metricID, pid, tid, timestamp, value);
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0000A07C File Offset: 0x0000827C
	public MetricDelegate()
		: this(SDPCorePINVOKE.new_MetricDelegate(), true)
	{
		this.SwigDirectorConnect();
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0000A090 File Offset: 0x00008290
	private void SwigDirectorConnect()
	{
		if (this.SwigDerivedClassHasMethod("OnMetricAdded", MetricDelegate.swigMethodTypes0))
		{
			this.swigDelegate0 = new MetricDelegate.SwigDelegateMetricDelegate_0(this.SwigDirectorOnMetricAdded);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricListReceived", MetricDelegate.swigMethodTypes1))
		{
			this.swigDelegate1 = new MetricDelegate.SwigDelegateMetricDelegate_1(this.SwigDirectorOnMetricListReceived);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricActivated", MetricDelegate.swigMethodTypes2))
		{
			this.swigDelegate2 = new MetricDelegate.SwigDelegateMetricDelegate_2(this.SwigDirectorOnMetricActivated);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricDeactivated", MetricDelegate.swigMethodTypes3))
		{
			this.swigDelegate3 = new MetricDelegate.SwigDelegateMetricDelegate_3(this.SwigDirectorOnMetricDeactivated);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricHiddenToggled", MetricDelegate.swigMethodTypes4))
		{
			this.swigDelegate4 = new MetricDelegate.SwigDelegateMetricDelegate_4(this.SwigDirectorOnMetricHiddenToggled);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricDataReceived", MetricDelegate.swigMethodTypes5))
		{
			this.swigDelegate5 = new MetricDelegate.SwigDelegateMetricDelegate_5(this.SwigDirectorOnMetricDataReceived);
		}
		SDPCorePINVOKE.MetricDelegate_director_connect(this.swigCPtr, this.swigDelegate0, this.swigDelegate1, this.swigDelegate2, this.swigDelegate3, this.swigDelegate4, this.swigDelegate5);
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0000A1A4 File Offset: 0x000083A4
	private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes)
	{
		MethodInfo method = base.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, methodTypes, null);
		return method.DeclaringType.IsSubclassOf(typeof(MetricDelegate));
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0000A1DA File Offset: 0x000083DA
	private void SwigDirectorOnMetricAdded(uint metricID)
	{
		this.OnMetricAdded(metricID);
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0000A1E3 File Offset: 0x000083E3
	private void SwigDirectorOnMetricListReceived(uint providerID)
	{
		this.OnMetricListReceived(providerID);
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0000A1EC File Offset: 0x000083EC
	private void SwigDirectorOnMetricActivated(uint metricID, uint pid)
	{
		this.OnMetricActivated(metricID, pid);
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0000A1F6 File Offset: 0x000083F6
	private void SwigDirectorOnMetricDeactivated(uint metricID, uint pid)
	{
		this.OnMetricDeactivated(metricID, pid);
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0000A200 File Offset: 0x00008400
	private void SwigDirectorOnMetricHiddenToggled(uint metricID, bool hidden)
	{
		this.OnMetricHiddenToggled(metricID, hidden);
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0000A20A File Offset: 0x0000840A
	private void SwigDirectorOnMetricDataReceived(uint metricID, uint pid, uint tid, long timestamp, double value)
	{
		this.OnMetricDataReceived(metricID, pid, tid, timestamp, value);
	}

	// Token: 0x040000F2 RID: 242
	private HandleRef swigCPtr;

	// Token: 0x040000F3 RID: 243
	protected bool swigCMemOwn;

	// Token: 0x040000F4 RID: 244
	private MetricDelegate.SwigDelegateMetricDelegate_0 swigDelegate0;

	// Token: 0x040000F5 RID: 245
	private MetricDelegate.SwigDelegateMetricDelegate_1 swigDelegate1;

	// Token: 0x040000F6 RID: 246
	private MetricDelegate.SwigDelegateMetricDelegate_2 swigDelegate2;

	// Token: 0x040000F7 RID: 247
	private MetricDelegate.SwigDelegateMetricDelegate_3 swigDelegate3;

	// Token: 0x040000F8 RID: 248
	private MetricDelegate.SwigDelegateMetricDelegate_4 swigDelegate4;

	// Token: 0x040000F9 RID: 249
	private MetricDelegate.SwigDelegateMetricDelegate_5 swigDelegate5;

	// Token: 0x040000FA RID: 250
	private static Type[] swigMethodTypes0 = new Type[] { typeof(uint) };

	// Token: 0x040000FB RID: 251
	private static Type[] swigMethodTypes1 = new Type[] { typeof(uint) };

	// Token: 0x040000FC RID: 252
	private static Type[] swigMethodTypes2 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040000FD RID: 253
	private static Type[] swigMethodTypes3 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040000FE RID: 254
	private static Type[] swigMethodTypes4 = new Type[]
	{
		typeof(uint),
		typeof(bool)
	};

	// Token: 0x040000FF RID: 255
	private static Type[] swigMethodTypes5 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(long),
		typeof(double)
	};

	// Token: 0x020000DE RID: 222
	// (Invoke) Token: 0x060014D9 RID: 5337
	public delegate void SwigDelegateMetricDelegate_0(uint metricID);

	// Token: 0x020000DF RID: 223
	// (Invoke) Token: 0x060014DD RID: 5341
	public delegate void SwigDelegateMetricDelegate_1(uint providerID);

	// Token: 0x020000E0 RID: 224
	// (Invoke) Token: 0x060014E1 RID: 5345
	public delegate void SwigDelegateMetricDelegate_2(uint metricID, uint pid);

	// Token: 0x020000E1 RID: 225
	// (Invoke) Token: 0x060014E5 RID: 5349
	public delegate void SwigDelegateMetricDelegate_3(uint metricID, uint pid);

	// Token: 0x020000E2 RID: 226
	// (Invoke) Token: 0x060014E9 RID: 5353
	public delegate void SwigDelegateMetricDelegate_4(uint metricID, bool hidden);

	// Token: 0x020000E3 RID: 227
	// (Invoke) Token: 0x060014ED RID: 5357
	public delegate void SwigDelegateMetricDelegate_5(uint metricID, uint pid, uint tid, long timestamp, double value);
}
