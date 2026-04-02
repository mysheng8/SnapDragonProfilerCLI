using System;
using System.Runtime.InteropServices;

// Token: 0x02000011 RID: 17
public class EGLDecoder : Decoder
{
	// Token: 0x06000138 RID: 312 RVA: 0x00005C3A File Offset: 0x00003E3A
	internal EGLDecoder(IntPtr cPtr, bool cMemoryOwn)
		: base(libDCAPPINVOKE.EGLDecoder_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00005C56 File Offset: 0x00003E56
	internal static HandleRef getCPtr(EGLDecoder obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00005C70 File Offset: 0x00003E70
	~EGLDecoder()
	{
		this.Dispose();
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00005C9C File Offset: 0x00003E9C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_EGLDecoder(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00005D20 File Offset: 0x00003F20
	public EGLDecoder()
		: this(libDCAPPINVOKE.new_EGLDecoder(), true)
	{
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00005D2E File Offset: 0x00003F2E
	public void AddAdapter(EGLAdapter pAdapter)
	{
		libDCAPPINVOKE.EGLDecoder_AddAdapter(this.swigCPtr, EGLAdapter.getCPtr(pAdapter));
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00005D41 File Offset: 0x00003F41
	public void RemoveAdapter(EGLAdapter pAdapter)
	{
		libDCAPPINVOKE.EGLDecoder_RemoveAdapter(this.swigCPtr, EGLAdapter.getCPtr(pAdapter));
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00005D54 File Offset: 0x00003F54
	public override bool SupportsId(ApiCallType id)
	{
		return libDCAPPINVOKE.EGLDecoder_SupportsId(this.swigCPtr, (int)id);
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00005D62 File Offset: 0x00003F62
	public override void ProcessFunctionCall(ApiCallType callId, IntPtr buffer, uint len)
	{
		libDCAPPINVOKE.EGLDecoder_ProcessFunctionCall(this.swigCPtr, (int)callId, buffer, len);
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00005D72 File Offset: 0x00003F72
	public override void ProcessMethodCall(ApiCallType arg0, uint arg1, IntPtr arg2, uint arg3)
	{
		libDCAPPINVOKE.EGLDecoder_ProcessMethodCall(this.swigCPtr, (int)arg0, arg1, arg2, arg3);
	}

	// Token: 0x04000580 RID: 1408
	private HandleRef swigCPtr;
}
