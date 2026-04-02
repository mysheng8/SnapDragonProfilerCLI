using System;
using System.Runtime.InteropServices;

// Token: 0x0200000F RID: 15
public class Decoder : IDisposable
{
	// Token: 0x060000A3 RID: 163 RVA: 0x00003200 File Offset: 0x00001400
	internal Decoder(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x0000321C File Offset: 0x0000141C
	internal static HandleRef getCPtr(Decoder obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00003234 File Offset: 0x00001434
	~Decoder()
	{
		this.Dispose();
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00003260 File Offset: 0x00001460
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_Decoder(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x000032E0 File Offset: 0x000014E0
	public void SetCurrentThreadId(uint id)
	{
		libDCAPPINVOKE.Decoder_SetCurrentThreadId(this.swigCPtr, id);
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000032EE File Offset: 0x000014EE
	public uint GetCurrentThreadId()
	{
		return libDCAPPINVOKE.Decoder_GetCurrentThreadId(this.swigCPtr);
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x000032FB File Offset: 0x000014FB
	public virtual bool SupportsId(ApiCallType id)
	{
		return libDCAPPINVOKE.Decoder_SupportsId(this.swigCPtr, (int)id);
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00003309 File Offset: 0x00001509
	public virtual void ProcessFunctionCall(ApiCallType callId, IntPtr pParamBuffer, uint paramBufferSize)
	{
		libDCAPPINVOKE.Decoder_ProcessFunctionCall(this.swigCPtr, (int)callId, pParamBuffer, paramBufferSize);
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00003319 File Offset: 0x00001519
	public virtual void ProcessMethodCall(ApiCallType callId, uint objectId, IntPtr pParamBuffer, uint paramBufferSize)
	{
		libDCAPPINVOKE.Decoder_ProcessMethodCall(this.swigCPtr, (int)callId, objectId, pParamBuffer, paramBufferSize);
	}

	// Token: 0x040004F9 RID: 1273
	private HandleRef swigCPtr;

	// Token: 0x040004FA RID: 1274
	protected bool swigCMemOwn;
}
