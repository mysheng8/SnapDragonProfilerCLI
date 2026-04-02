using System;
using System.Runtime.InteropServices;

// Token: 0x02000016 RID: 22
public class CaptureMetric : IDisposable
{
	// Token: 0x0600008E RID: 142 RVA: 0x00002E57 File Offset: 0x00001057
	internal CaptureMetric(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00002E73 File Offset: 0x00001073
	internal static HandleRef getCPtr(CaptureMetric obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00002E8C File Offset: 0x0000108C
	~CaptureMetric()
	{
		this.Dispose();
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00002EB8 File Offset: 0x000010B8
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CaptureMetric(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000093 RID: 147 RVA: 0x00002F48 File Offset: 0x00001148
	// (set) Token: 0x06000092 RID: 146 RVA: 0x00002F38 File Offset: 0x00001138
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.CaptureMetric_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureMetric_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000095 RID: 149 RVA: 0x00002F70 File Offset: 0x00001170
	// (set) Token: 0x06000094 RID: 148 RVA: 0x00002F62 File Offset: 0x00001162
	public uint processID
	{
		get
		{
			return SDPCorePINVOKE.CaptureMetric_processID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureMetric_processID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000097 RID: 151 RVA: 0x00002F98 File Offset: 0x00001198
	// (set) Token: 0x06000096 RID: 150 RVA: 0x00002F8A File Offset: 0x0000118A
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.CaptureMetric_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureMetric_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00002FB2 File Offset: 0x000011B2
	public CaptureMetric()
		: this(SDPCorePINVOKE.new_CaptureMetric(), true)
	{
	}

	// Token: 0x04000013 RID: 19
	private HandleRef swigCPtr;

	// Token: 0x04000014 RID: 20
	protected bool swigCMemOwn;
}
