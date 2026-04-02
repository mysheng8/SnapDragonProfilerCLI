using System;
using System.Runtime.InteropServices;

// Token: 0x02000096 RID: 150
public class StartCapture : CommandMsg
{
	// Token: 0x06001358 RID: 4952 RVA: 0x00017BE8 File Offset: 0x00015DE8
	internal StartCapture(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.StartCapture_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x00017C04 File Offset: 0x00015E04
	internal static HandleRef getCPtr(StartCapture obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x00017C1C File Offset: 0x00015E1C
	~StartCapture()
	{
		this.Dispose();
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x00017C48 File Offset: 0x00015E48
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_StartCapture(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x0600135D RID: 4957 RVA: 0x00017CDC File Offset: 0x00015EDC
	// (set) Token: 0x0600135C RID: 4956 RVA: 0x00017CCC File Offset: 0x00015ECC
	public uint duration
	{
		get
		{
			return SDPCorePINVOKE.StartCapture_duration_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.StartCapture_duration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x0600135F RID: 4959 RVA: 0x00017D04 File Offset: 0x00015F04
	// (set) Token: 0x0600135E RID: 4958 RVA: 0x00017CF6 File Offset: 0x00015EF6
	public uint startDelay
	{
		get
		{
			return SDPCorePINVOKE.StartCapture_startDelay_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.StartCapture_startDelay_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06001361 RID: 4961 RVA: 0x00017D2C File Offset: 0x00015F2C
	// (set) Token: 0x06001360 RID: 4960 RVA: 0x00017D1E File Offset: 0x00015F1E
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.StartCapture_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.StartCapture_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06001363 RID: 4963 RVA: 0x00017D54 File Offset: 0x00015F54
	// (set) Token: 0x06001362 RID: 4962 RVA: 0x00017D46 File Offset: 0x00015F46
	public uint captureType
	{
		get
		{
			return SDPCorePINVOKE.StartCapture_captureType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.StartCapture_captureType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x00017D6E File Offset: 0x00015F6E
	public StartCapture(uint capture, uint type)
		: this(SDPCorePINVOKE.new_StartCapture__SWIG_0(capture, type), true)
	{
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x00017D7E File Offset: 0x00015F7E
	public StartCapture(uint capture, uint type, uint durationMS, uint startDelayMS)
		: this(SDPCorePINVOKE.new_StartCapture__SWIG_1(capture, type, durationMS, startDelayMS), true)
	{
	}

	// Token: 0x040001C6 RID: 454
	private HandleRef swigCPtr;
}
