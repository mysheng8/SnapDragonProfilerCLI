using System;
using System.Runtime.InteropServices;

// Token: 0x02000014 RID: 20
public class Capture : IDisposable
{
	// Token: 0x06000077 RID: 119 RVA: 0x00002AF1 File Offset: 0x00000CF1
	internal Capture(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00002B0D File Offset: 0x00000D0D
	internal static HandleRef getCPtr(Capture obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00002B24 File Offset: 0x00000D24
	~Capture()
	{
		this.Dispose();
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00002B50 File Offset: 0x00000D50
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_Capture(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00002BD0 File Offset: 0x00000DD0
	public bool Start(CaptureSettings settings)
	{
		bool flag = SDPCorePINVOKE.Capture_Start(this.swigCPtr, CaptureSettings.getCPtr(settings));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00002C00 File Offset: 0x00000E00
	public bool Stop()
	{
		return SDPCorePINVOKE.Capture_Stop(this.swigCPtr);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00002C1C File Offset: 0x00000E1C
	public bool Pause(bool pause)
	{
		return SDPCorePINVOKE.Capture_Pause__SWIG_0(this.swigCPtr, pause);
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00002C38 File Offset: 0x00000E38
	public bool Pause()
	{
		return SDPCorePINVOKE.Capture_Pause__SWIG_1(this.swigCPtr);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00002C54 File Offset: 0x00000E54
	public bool Cancel()
	{
		return SDPCorePINVOKE.Capture_Cancel(this.swigCPtr);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00002C70 File Offset: 0x00000E70
	public bool IsValid()
	{
		return SDPCorePINVOKE.Capture_IsValid(this.swigCPtr);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00002C8C File Offset: 0x00000E8C
	public CaptureProperties GetProperties()
	{
		return new CaptureProperties(SDPCorePINVOKE.Capture_GetProperties(this.swigCPtr), false);
	}

	// Token: 0x0400000F RID: 15
	private HandleRef swigCPtr;

	// Token: 0x04000010 RID: 16
	protected bool swigCMemOwn;
}
