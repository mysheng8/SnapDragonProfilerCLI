using System;
using System.Runtime.InteropServices;

// Token: 0x02000017 RID: 23
public class CaptureModel : IDisposable
{
	// Token: 0x06000099 RID: 153 RVA: 0x00002FC0 File Offset: 0x000011C0
	internal CaptureModel(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00002FDC File Offset: 0x000011DC
	internal static HandleRef getCPtr(CaptureModel obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00002FF4 File Offset: 0x000011F4
	~CaptureModel()
	{
		this.Dispose();
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00003020 File Offset: 0x00001220
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CaptureModel(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000030A0 File Offset: 0x000012A0
	public CaptureModel()
		: this(SDPCorePINVOKE.new_CaptureModel(), true)
	{
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600009F RID: 159 RVA: 0x000030BC File Offset: 0x000012BC
	// (set) Token: 0x0600009E RID: 158 RVA: 0x000030AE File Offset: 0x000012AE
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.CaptureModel_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureModel_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060000A1 RID: 161 RVA: 0x000030E4 File Offset: 0x000012E4
	// (set) Token: 0x060000A0 RID: 160 RVA: 0x000030D6 File Offset: 0x000012D6
	public long startTimeMono
	{
		get
		{
			return SDPCorePINVOKE.CaptureModel_startTimeMono_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureModel_startTimeMono_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000310C File Offset: 0x0000130C
	// (set) Token: 0x060000A2 RID: 162 RVA: 0x000030FE File Offset: 0x000012FE
	public long stopTimeMono
	{
		get
		{
			return SDPCorePINVOKE.CaptureModel_stopTimeMono_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureModel_stopTimeMono_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003144 File Offset: 0x00001344
	// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003126 File Offset: 0x00001326
	public string name
	{
		get
		{
			string text = SDPCorePINVOKE.CaptureModel_name_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.CaptureModel_name_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000317C File Offset: 0x0000137C
	// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000316B File Offset: 0x0000136B
	public uint deviceID
	{
		get
		{
			return SDPCorePINVOKE.CaptureModel_deviceID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureModel_deviceID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060000A9 RID: 169 RVA: 0x000031B4 File Offset: 0x000013B4
	// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003196 File Offset: 0x00001396
	public string device
	{
		get
		{
			string text = SDPCorePINVOKE.CaptureModel_device_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.CaptureModel_device_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060000AB RID: 171 RVA: 0x000031EC File Offset: 0x000013EC
	// (set) Token: 0x060000AA RID: 170 RVA: 0x000031DB File Offset: 0x000013DB
	public uint captureType
	{
		get
		{
			return SDPCorePINVOKE.CaptureModel_captureType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureModel_captureType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060000AD RID: 173 RVA: 0x00003214 File Offset: 0x00001414
	// (set) Token: 0x060000AC RID: 172 RVA: 0x00003206 File Offset: 0x00001406
	public uint startDelay
	{
		get
		{
			return SDPCorePINVOKE.CaptureModel_startDelay_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureModel_startDelay_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060000AF RID: 175 RVA: 0x0000323C File Offset: 0x0000143C
	// (set) Token: 0x060000AE RID: 174 RVA: 0x0000322E File Offset: 0x0000142E
	public uint duration
	{
		get
		{
			return SDPCorePINVOKE.CaptureModel_duration_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureModel_duration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04000015 RID: 21
	private HandleRef swigCPtr;

	// Token: 0x04000016 RID: 22
	protected bool swigCMemOwn;
}
