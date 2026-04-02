using System;
using System.Runtime.InteropServices;

// Token: 0x02000040 RID: 64
public class MetricManager : IDisposable
{
	// Token: 0x060003EB RID: 1003 RVA: 0x0000AB9A File Offset: 0x00008D9A
	internal MetricManager(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0000ABB6 File Offset: 0x00008DB6
	internal static HandleRef getCPtr(MetricManager obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0000ABD0 File Offset: 0x00008DD0
	~MetricManager()
	{
		this.Dispose();
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0000ABFC File Offset: 0x00008DFC
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricManager(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0000AC7C File Offset: 0x00008E7C
	public static MetricManager Get()
	{
		return new MetricManager(SDPCorePINVOKE.MetricManager_Get(), false);
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0000AC96 File Offset: 0x00008E96
	public void RegisterMetricEventDelegate(MetricDelegate arg0)
	{
		SDPCorePINVOKE.MetricManager_RegisterMetricEventDelegate(this.swigCPtr, MetricDelegate.getCPtr(arg0));
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0000ACA9 File Offset: 0x00008EA9
	public void UnregisterMetricEventDelegate(MetricDelegate arg0)
	{
		SDPCorePINVOKE.MetricManager_UnregisterMetricEventDelegate(this.swigCPtr, MetricDelegate.getCPtr(arg0));
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0000ACBC File Offset: 0x00008EBC
	public void RegisterMetricCategoryEventDelegate(MetricCategoryDelegate arg0)
	{
		SDPCorePINVOKE.MetricManager_RegisterMetricCategoryEventDelegate(this.swigCPtr, MetricCategoryDelegate.getCPtr(arg0));
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0000ACCF File Offset: 0x00008ECF
	public void UnregisterMetricCategoryEventDelegate(MetricCategoryDelegate arg0)
	{
		SDPCorePINVOKE.MetricManager_UnregisterMetricCategoryEventDelegate(this.swigCPtr, MetricCategoryDelegate.getCPtr(arg0));
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0000ACE2 File Offset: 0x00008EE2
	public void Reset()
	{
		SDPCorePINVOKE.MetricManager_Reset(this.swigCPtr);
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0000ACEF File Offset: 0x00008EEF
	public void ShutDown()
	{
		SDPCorePINVOKE.MetricManager_ShutDown(this.swigCPtr);
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0000ACFC File Offset: 0x00008EFC
	public void DisableDataModel()
	{
		SDPCorePINVOKE.MetricManager_DisableDataModel(this.swigCPtr);
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0000AD0C File Offset: 0x00008F0C
	public bool IsDataModelActive()
	{
		return SDPCorePINVOKE.MetricManager_IsDataModelActive(this.swigCPtr);
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0000AD28 File Offset: 0x00008F28
	public MetricCategory AddMetricCategory(string name, string description, uint parent, bool notify)
	{
		MetricCategory metricCategory = new MetricCategory(SDPCorePINVOKE.MetricManager_AddMetricCategory__SWIG_0(this.swigCPtr, name, description, parent, notify), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategory;
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0000AD5C File Offset: 0x00008F5C
	public MetricCategory AddMetricCategory(string name, string description, uint parent)
	{
		MetricCategory metricCategory = new MetricCategory(SDPCorePINVOKE.MetricManager_AddMetricCategory__SWIG_1(this.swigCPtr, name, description, parent), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategory;
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0000AD8C File Offset: 0x00008F8C
	public MetricCategory AddMetricCategory(string name, string description)
	{
		MetricCategory metricCategory = new MetricCategory(SDPCorePINVOKE.MetricManager_AddMetricCategory__SWIG_2(this.swigCPtr, name, description), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategory;
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0000ADBC File Offset: 0x00008FBC
	public MetricCategory AddMetricCategory(MetricCategoryProperties props)
	{
		MetricCategory metricCategory = new MetricCategory(SDPCorePINVOKE.MetricManager_AddMetricCategory__SWIG_3(this.swigCPtr, MetricCategoryProperties.getCPtr(props)), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategory;
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0000ADF0 File Offset: 0x00008FF0
	public MetricCategory GetMetricCategoryByName(string name)
	{
		MetricCategory metricCategory = new MetricCategory(SDPCorePINVOKE.MetricManager_GetMetricCategoryByName(this.swigCPtr, name), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategory;
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0000AE20 File Offset: 0x00009020
	public MetricCategory GetMetricCategory(uint categoryID)
	{
		return new MetricCategory(SDPCorePINVOKE.MetricManager_GetMetricCategory(this.swigCPtr, categoryID), true);
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0000AE44 File Offset: 0x00009044
	public MetricCategoryList GetAllMetricCategories()
	{
		return new MetricCategoryList(SDPCorePINVOKE.MetricManager_GetAllMetricCategories(this.swigCPtr), true);
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0000AE64 File Offset: 0x00009064
	public Metric AddMetric(MetricProperties props, bool notify)
	{
		Metric metric = new Metric(SDPCorePINVOKE.MetricManager_AddMetric__SWIG_0(this.swigCPtr, MetricProperties.getCPtr(props), notify), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0000AE98 File Offset: 0x00009098
	public Metric AddMetric(MetricProperties props)
	{
		Metric metric = new Metric(SDPCorePINVOKE.MetricManager_AddMetric__SWIG_1(this.swigCPtr, MetricProperties.getCPtr(props)), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0000AECC File Offset: 0x000090CC
	public Metric GetMetric(uint mid)
	{
		return new Metric(SDPCorePINVOKE.MetricManager_GetMetric(this.swigCPtr, mid), true);
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0000AEF0 File Offset: 0x000090F0
	public Metric GetMetricByName(string name)
	{
		Metric metric = new Metric(SDPCorePINVOKE.MetricManager_GetMetricByName(this.swigCPtr, name), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0000AF20 File Offset: 0x00009120
	public MetricList GetAllMetrics()
	{
		return new MetricList(SDPCorePINVOKE.MetricManager_GetAllMetrics(this.swigCPtr), true);
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0000AF40 File Offset: 0x00009140
	public MetricList GetMetricsByProvider(uint provider)
	{
		return new MetricList(SDPCorePINVOKE.MetricManager_GetMetricsByProvider(this.swigCPtr, provider), true);
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0000AF64 File Offset: 0x00009164
	public bool LogMetricValue(uint mid, uint arg1, long timestamp, double value, uint pid)
	{
		return SDPCorePINVOKE.MetricManager_LogMetricValue__SWIG_0(this.swigCPtr, mid, arg1, timestamp, value, pid);
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0000AF88 File Offset: 0x00009188
	public bool LogMetricValue(uint mid, uint arg1, long timestamp, double value)
	{
		return SDPCorePINVOKE.MetricManager_LogMetricValue__SWIG_1(this.swigCPtr, mid, arg1, timestamp, value);
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0000AFA8 File Offset: 0x000091A8
	public MetricManager.SessionEventHandler GetSessionEventHandler()
	{
		return new MetricManager.SessionEventHandler(SDPCorePINVOKE.MetricManager_GetSessionEventHandler(this.swigCPtr), false);
	}

	// Token: 0x04000104 RID: 260
	private HandleRef swigCPtr;

	// Token: 0x04000105 RID: 261
	protected bool swigCMemOwn;

	// Token: 0x020000E6 RID: 230
	public class SessionEventHandler : SessionManagerDelegate
	{
		// Token: 0x060014FC RID: 5372 RVA: 0x00019F59 File Offset: 0x00018159
		internal SessionEventHandler(IntPtr cPtr, bool cMemoryOwn)
			: base(SDPCorePINVOKE.MetricManager_SessionEventHandler_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00019F75 File Offset: 0x00018175
		internal static HandleRef getCPtr(MetricManager.SessionEventHandler obj)
		{
			if (obj != null)
			{
				return obj.swigCPtr;
			}
			return new HandleRef(null, IntPtr.Zero);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00019F8C File Offset: 0x0001818C
		~SessionEventHandler()
		{
			this.Dispose();
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00019FB8 File Offset: 0x000181B8
		public override void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						SDPCorePINVOKE.delete_MetricManager_SessionEventHandler(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0001A03C File Offset: 0x0001823C
		public override void OnSessionOpened()
		{
			SDPCorePINVOKE.MetricManager_SessionEventHandler_OnSessionOpened(this.swigCPtr);
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0001A049 File Offset: 0x00018249
		public override void OnSessionClosed()
		{
			SDPCorePINVOKE.MetricManager_SessionEventHandler_OnSessionClosed(this.swigCPtr);
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0001A056 File Offset: 0x00018256
		public SessionEventHandler()
			: this(SDPCorePINVOKE.new_MetricManager_SessionEventHandler(), true)
		{
		}

		// Token: 0x0400020C RID: 524
		private HandleRef swigCPtr;
	}
}
