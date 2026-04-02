using System;
using System.Runtime.InteropServices;

// Token: 0x02000099 RID: 153
public class StopCaptureTimeTOD : CommandMsg
{
	// Token: 0x06001376 RID: 4982 RVA: 0x00017FF5 File Offset: 0x000161F5
	internal StopCaptureTimeTOD(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.StopCaptureTimeTOD_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x00018011 File Offset: 0x00016211
	internal static HandleRef getCPtr(StopCaptureTimeTOD obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x00018028 File Offset: 0x00016228
	~StopCaptureTimeTOD()
	{
		this.Dispose();
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x00018054 File Offset: 0x00016254
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_StopCaptureTimeTOD(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x0600137B RID: 4987 RVA: 0x000180E8 File Offset: 0x000162E8
	// (set) Token: 0x0600137A RID: 4986 RVA: 0x000180D8 File Offset: 0x000162D8
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.StopCaptureTimeTOD_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.StopCaptureTimeTOD_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x0600137D RID: 4989 RVA: 0x00018110 File Offset: 0x00016310
	// (set) Token: 0x0600137C RID: 4988 RVA: 0x00018102 File Offset: 0x00016302
	public long stopTimeTOD
	{
		get
		{
			return SDPCorePINVOKE.StopCaptureTimeTOD_stopTimeTOD_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.StopCaptureTimeTOD_stopTimeTOD_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x0001812A File Offset: 0x0001632A
	public StopCaptureTimeTOD(uint capture, long stopTOD)
		: this(SDPCorePINVOKE.new_StopCaptureTimeTOD(capture, stopTOD), true)
	{
	}

	// Token: 0x040001C9 RID: 457
	private HandleRef swigCPtr;
}
