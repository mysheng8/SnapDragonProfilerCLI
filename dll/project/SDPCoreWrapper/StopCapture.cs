using System;
using System.Runtime.InteropServices;

// Token: 0x02000098 RID: 152
public class StopCapture : CommandMsg
{
	// Token: 0x0600136F RID: 4975 RVA: 0x00017ED6 File Offset: 0x000160D6
	internal StopCapture(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.StopCapture_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x00017EF2 File Offset: 0x000160F2
	internal static HandleRef getCPtr(StopCapture obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x00017F0C File Offset: 0x0001610C
	~StopCapture()
	{
		this.Dispose();
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x00017F38 File Offset: 0x00016138
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_StopCapture(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06001374 RID: 4980 RVA: 0x00017FCC File Offset: 0x000161CC
	// (set) Token: 0x06001373 RID: 4979 RVA: 0x00017FBC File Offset: 0x000161BC
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.StopCapture_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.StopCapture_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x00017FE6 File Offset: 0x000161E6
	public StopCapture(uint capture)
		: this(SDPCorePINVOKE.new_StopCapture(capture), true)
	{
	}

	// Token: 0x040001C8 RID: 456
	private HandleRef swigCPtr;
}
