using System;
using System.Runtime.InteropServices;

// Token: 0x02000019 RID: 25
public class CaptureSettings : IDisposable
{
	// Token: 0x060000C7 RID: 199 RVA: 0x000034E9 File Offset: 0x000016E9
	internal CaptureSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00003505 File Offset: 0x00001705
	internal static HandleRef getCPtr(CaptureSettings obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x0000351C File Offset: 0x0000171C
	~CaptureSettings()
	{
		this.Dispose();
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00003548 File Offset: 0x00001748
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CaptureSettings(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000035C8 File Offset: 0x000017C8
	public CaptureSettings(uint captureType, uint pid, uint s, uint d, string r)
		: this(SDPCorePINVOKE.new_CaptureSettings(captureType, pid, s, d, r), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060000CD RID: 205 RVA: 0x000035F8 File Offset: 0x000017F8
	// (set) Token: 0x060000CC RID: 204 RVA: 0x000035EA File Offset: 0x000017EA
	public uint captureType
	{
		get
		{
			return SDPCorePINVOKE.CaptureSettings_captureType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureSettings_captureType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060000CF RID: 207 RVA: 0x00003620 File Offset: 0x00001820
	// (set) Token: 0x060000CE RID: 206 RVA: 0x00003612 File Offset: 0x00001812
	public uint processID
	{
		get
		{
			return SDPCorePINVOKE.CaptureSettings_processID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureSettings_processID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003648 File Offset: 0x00001848
	// (set) Token: 0x060000D0 RID: 208 RVA: 0x0000363A File Offset: 0x0000183A
	public uint startDelay
	{
		get
		{
			return SDPCorePINVOKE.CaptureSettings_startDelay_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureSettings_startDelay_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060000D3 RID: 211 RVA: 0x00003670 File Offset: 0x00001870
	// (set) Token: 0x060000D2 RID: 210 RVA: 0x00003662 File Offset: 0x00001862
	public uint duration
	{
		get
		{
			return SDPCorePINVOKE.CaptureSettings_duration_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureSettings_duration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060000D5 RID: 213 RVA: 0x000036A8 File Offset: 0x000018A8
	// (set) Token: 0x060000D4 RID: 212 RVA: 0x0000368A File Offset: 0x0000188A
	public string rendererString
	{
		get
		{
			string text = SDPCorePINVOKE.CaptureSettings_rendererString_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.CaptureSettings_rendererString_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x04000019 RID: 25
	private HandleRef swigCPtr;

	// Token: 0x0400001A RID: 26
	protected bool swigCMemOwn;
}
