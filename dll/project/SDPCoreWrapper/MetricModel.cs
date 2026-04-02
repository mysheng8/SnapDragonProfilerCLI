using System;
using System.Runtime.InteropServices;

// Token: 0x02000042 RID: 66
public class MetricModel : IDisposable
{
	// Token: 0x06000420 RID: 1056 RVA: 0x0000B414 File Offset: 0x00009614
	internal MetricModel(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0000B430 File Offset: 0x00009630
	internal static HandleRef getCPtr(MetricModel obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0000B448 File Offset: 0x00009648
	~MetricModel()
	{
		this.Dispose();
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0000B474 File Offset: 0x00009674
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricModel(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0000B4F4 File Offset: 0x000096F4
	public MetricModel()
		: this(SDPCorePINVOKE.new_MetricModel(), true)
	{
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000B510 File Offset: 0x00009710
	// (set) Token: 0x06000425 RID: 1061 RVA: 0x0000B502 File Offset: 0x00009702
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.MetricModel_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricModel_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000428 RID: 1064 RVA: 0x0000B548 File Offset: 0x00009748
	// (set) Token: 0x06000427 RID: 1063 RVA: 0x0000B52A File Offset: 0x0000972A
	public string name
	{
		get
		{
			string text = SDPCorePINVOKE.MetricModel_name_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.MetricModel_name_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x0600042A RID: 1066 RVA: 0x0000B580 File Offset: 0x00009780
	// (set) Token: 0x06000429 RID: 1065 RVA: 0x0000B56F File Offset: 0x0000976F
	public uint category
	{
		get
		{
			return SDPCorePINVOKE.MetricModel_category_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricModel_category_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x0600042C RID: 1068 RVA: 0x0000B5A8 File Offset: 0x000097A8
	// (set) Token: 0x0600042B RID: 1067 RVA: 0x0000B59A File Offset: 0x0000979A
	public SDPDataType type
	{
		get
		{
			return (SDPDataType)SDPCorePINVOKE.MetricModel_type_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricModel_type_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600042E RID: 1070 RVA: 0x0000B5D0 File Offset: 0x000097D0
	// (set) Token: 0x0600042D RID: 1069 RVA: 0x0000B5C2 File Offset: 0x000097C2
	public uint captureType
	{
		get
		{
			return SDPCorePINVOKE.MetricModel_captureType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricModel_captureType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04000118 RID: 280
	private HandleRef swigCPtr;

	// Token: 0x04000119 RID: 281
	protected bool swigCMemOwn;
}
