using System;
using System.Runtime.InteropServices;

// Token: 0x02000097 RID: 151
public class StartCaptureTimeTOD : CommandMsg
{
	// Token: 0x06001366 RID: 4966 RVA: 0x00017D91 File Offset: 0x00015F91
	internal StartCaptureTimeTOD(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.StartCaptureTimeTOD_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x00017DAD File Offset: 0x00015FAD
	internal static HandleRef getCPtr(StartCaptureTimeTOD obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x00017DC4 File Offset: 0x00015FC4
	~StartCaptureTimeTOD()
	{
		this.Dispose();
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x00017DF0 File Offset: 0x00015FF0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_StartCaptureTimeTOD(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x0600136B RID: 4971 RVA: 0x00017E84 File Offset: 0x00016084
	// (set) Token: 0x0600136A RID: 4970 RVA: 0x00017E74 File Offset: 0x00016074
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.StartCaptureTimeTOD_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.StartCaptureTimeTOD_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x0600136D RID: 4973 RVA: 0x00017EAC File Offset: 0x000160AC
	// (set) Token: 0x0600136C RID: 4972 RVA: 0x00017E9E File Offset: 0x0001609E
	public long startTimeTOD
	{
		get
		{
			return SDPCorePINVOKE.StartCaptureTimeTOD_startTimeTOD_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.StartCaptureTimeTOD_startTimeTOD_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x00017EC6 File Offset: 0x000160C6
	public StartCaptureTimeTOD(uint capture, long startTOD)
		: this(SDPCorePINVOKE.new_StartCaptureTimeTOD(capture, startTOD), true)
	{
	}

	// Token: 0x040001C7 RID: 455
	private HandleRef swigCPtr;
}
