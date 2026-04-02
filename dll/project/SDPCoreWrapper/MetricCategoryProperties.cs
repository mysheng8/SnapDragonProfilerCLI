using System;
using System.Runtime.InteropServices;

// Token: 0x0200003C RID: 60
public class MetricCategoryProperties : IDisposable
{
	// Token: 0x06000381 RID: 897 RVA: 0x00009BA3 File Offset: 0x00007DA3
	internal MetricCategoryProperties(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00009BBF File Offset: 0x00007DBF
	internal static HandleRef getCPtr(MetricCategoryProperties obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00009BD8 File Offset: 0x00007DD8
	~MetricCategoryProperties()
	{
		this.Dispose();
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00009C04 File Offset: 0x00007E04
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricCategoryProperties(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000386 RID: 902 RVA: 0x00009C94 File Offset: 0x00007E94
	// (set) Token: 0x06000385 RID: 901 RVA: 0x00009C84 File Offset: 0x00007E84
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.MetricCategoryProperties_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricCategoryProperties_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000388 RID: 904 RVA: 0x00009CBC File Offset: 0x00007EBC
	// (set) Token: 0x06000387 RID: 903 RVA: 0x00009CAE File Offset: 0x00007EAE
	public uint parent
	{
		get
		{
			return SDPCorePINVOKE.MetricCategoryProperties_parent_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricCategoryProperties_parent_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x0600038A RID: 906 RVA: 0x00009CF4 File Offset: 0x00007EF4
	// (set) Token: 0x06000389 RID: 905 RVA: 0x00009CD6 File Offset: 0x00007ED6
	public string name
	{
		get
		{
			string text = SDPCorePINVOKE.MetricCategoryProperties_name_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.MetricCategoryProperties_name_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x0600038C RID: 908 RVA: 0x00009D38 File Offset: 0x00007F38
	// (set) Token: 0x0600038B RID: 907 RVA: 0x00009D1B File Offset: 0x00007F1B
	public string description
	{
		get
		{
			string text = SDPCorePINVOKE.MetricCategoryProperties_description_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.MetricCategoryProperties_description_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x0600038E RID: 910 RVA: 0x00009D74 File Offset: 0x00007F74
	// (set) Token: 0x0600038D RID: 909 RVA: 0x00009D5F File Offset: 0x00007F5F
	public IDList children
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.MetricCategoryProperties_children_get(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new IDList(intPtr, false);
		}
		set
		{
			SDPCorePINVOKE.MetricCategoryProperties_children_set(this.swigCPtr, IDList.getCPtr(value));
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000390 RID: 912 RVA: 0x00009DBC File Offset: 0x00007FBC
	// (set) Token: 0x0600038F RID: 911 RVA: 0x00009DA6 File Offset: 0x00007FA6
	public IDList metrics
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.MetricCategoryProperties_metrics_get(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new IDList(intPtr, false);
		}
		set
		{
			SDPCorePINVOKE.MetricCategoryProperties_metrics_set(this.swigCPtr, IDList.getCPtr(value));
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00009DEE File Offset: 0x00007FEE
	public MetricCategoryProperties()
		: this(SDPCorePINVOKE.new_MetricCategoryProperties__SWIG_0(), true)
	{
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00009DFC File Offset: 0x00007FFC
	public MetricCategoryProperties(MetricCategoryProperties props)
		: this(SDPCorePINVOKE.new_MetricCategoryProperties__SWIG_1(MetricCategoryProperties.getCPtr(props)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00009E1D File Offset: 0x0000801D
	public MetricCategoryProperties(uint id, string name, string description, uint parent)
		: this(SDPCorePINVOKE.new_MetricCategoryProperties__SWIG_2(id, name, description, parent), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00009E3D File Offset: 0x0000803D
	public MetricCategoryProperties(uint id, string name, string description)
		: this(SDPCorePINVOKE.new_MetricCategoryProperties__SWIG_3(id, name, description), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00009E5B File Offset: 0x0000805B
	public void Equal(MetricCategoryProperties c)
	{
		SDPCorePINVOKE.MetricCategoryProperties_Equal(this.swigCPtr, MetricCategoryProperties.getCPtr(c));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x040000F0 RID: 240
	private HandleRef swigCPtr;

	// Token: 0x040000F1 RID: 241
	protected bool swigCMemOwn;
}
