using System;
using System.Runtime.InteropServices;

// Token: 0x02000018 RID: 24
public class CaptureProperties : IDisposable
{
	// Token: 0x060000B0 RID: 176 RVA: 0x00003256 File Offset: 0x00001456
	internal CaptureProperties(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00003272 File Offset: 0x00001472
	internal static HandleRef getCPtr(CaptureProperties obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0000328C File Offset: 0x0000148C
	~CaptureProperties()
	{
		this.Dispose();
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000032B8 File Offset: 0x000014B8
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CaptureProperties(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003348 File Offset: 0x00001548
	// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003338 File Offset: 0x00001538
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.CaptureProperties_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureProperties_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060000B7 RID: 183 RVA: 0x00003370 File Offset: 0x00001570
	// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003362 File Offset: 0x00001562
	public uint captureType
	{
		get
		{
			return SDPCorePINVOKE.CaptureProperties_captureType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureProperties_captureType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060000B9 RID: 185 RVA: 0x00003398 File Offset: 0x00001598
	// (set) Token: 0x060000B8 RID: 184 RVA: 0x0000338A File Offset: 0x0000158A
	public uint processID
	{
		get
		{
			return SDPCorePINVOKE.CaptureProperties_processID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureProperties_processID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060000BB RID: 187 RVA: 0x000033C0 File Offset: 0x000015C0
	// (set) Token: 0x060000BA RID: 186 RVA: 0x000033B2 File Offset: 0x000015B2
	public long startTimeTOD
	{
		get
		{
			return SDPCorePINVOKE.CaptureProperties_startTimeTOD_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureProperties_startTimeTOD_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060000BD RID: 189 RVA: 0x000033E8 File Offset: 0x000015E8
	// (set) Token: 0x060000BC RID: 188 RVA: 0x000033DA File Offset: 0x000015DA
	public long stopTimeTOD
	{
		get
		{
			return SDPCorePINVOKE.CaptureProperties_stopTimeTOD_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureProperties_stopTimeTOD_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060000BF RID: 191 RVA: 0x00003410 File Offset: 0x00001610
	// (set) Token: 0x060000BE RID: 190 RVA: 0x00003402 File Offset: 0x00001602
	public uint startDelay
	{
		get
		{
			return SDPCorePINVOKE.CaptureProperties_startDelay_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureProperties_startDelay_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003438 File Offset: 0x00001638
	// (set) Token: 0x060000C0 RID: 192 RVA: 0x0000342A File Offset: 0x0000162A
	public uint duration
	{
		get
		{
			return SDPCorePINVOKE.CaptureProperties_duration_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureProperties_duration_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003470 File Offset: 0x00001670
	// (set) Token: 0x060000C2 RID: 194 RVA: 0x00003452 File Offset: 0x00001652
	public string rendererString
	{
		get
		{
			string text = SDPCorePINVOKE.CaptureProperties_rendererString_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.CaptureProperties_rendererString_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060000C5 RID: 197 RVA: 0x000034B4 File Offset: 0x000016B4
	// (set) Token: 0x060000C4 RID: 196 RVA: 0x00003497 File Offset: 0x00001697
	public string captureName
	{
		get
		{
			string text = SDPCorePINVOKE.CaptureProperties_captureName_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.CaptureProperties_captureName_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000034DB File Offset: 0x000016DB
	public CaptureProperties()
		: this(SDPCorePINVOKE.new_CaptureProperties(), true)
	{
	}

	// Token: 0x04000017 RID: 23
	private HandleRef swigCPtr;

	// Token: 0x04000018 RID: 24
	protected bool swigCMemOwn;
}
