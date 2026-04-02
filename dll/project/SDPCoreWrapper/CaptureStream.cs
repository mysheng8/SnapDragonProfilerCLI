using System;
using System.Runtime.InteropServices;

// Token: 0x0200001B RID: 27
public class CaptureStream : IDisposable
{
	// Token: 0x060000D6 RID: 214 RVA: 0x000036CF File Offset: 0x000018CF
	internal CaptureStream(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x000036EB File Offset: 0x000018EB
	internal static HandleRef getCPtr(CaptureStream obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00003704 File Offset: 0x00001904
	~CaptureStream()
	{
		this.Dispose();
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00003730 File Offset: 0x00001930
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CaptureStream(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060000DA RID: 218 RVA: 0x000037B0 File Offset: 0x000019B0
	public virtual uint GetProcessID()
	{
		return SDPCorePINVOKE.CaptureStream_GetProcessID(this.swigCPtr);
	}

	// Token: 0x060000DB RID: 219 RVA: 0x000037CC File Offset: 0x000019CC
	public virtual uint GetThreadID()
	{
		return SDPCorePINVOKE.CaptureStream_GetThreadID(this.swigCPtr);
	}

	// Token: 0x04000021 RID: 33
	private HandleRef swigCPtr;

	// Token: 0x04000022 RID: 34
	protected bool swigCMemOwn;
}
