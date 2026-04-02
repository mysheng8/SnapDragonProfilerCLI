using System;
using System.Runtime.InteropServices;

// Token: 0x02000016 RID: 22
public class GLDecoder : Decoder
{
	// Token: 0x06000551 RID: 1361 RVA: 0x00018656 File Offset: 0x00016856
	internal GLDecoder(IntPtr cPtr, bool cMemoryOwn)
		: base(libDCAPPINVOKE.GLDecoder_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00018672 File Offset: 0x00016872
	internal static HandleRef getCPtr(GLDecoder obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0001868C File Offset: 0x0001688C
	~GLDecoder()
	{
		this.Dispose();
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x000186B8 File Offset: 0x000168B8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_GLDecoder(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0001873C File Offset: 0x0001693C
	public GLDecoder()
		: this(libDCAPPINVOKE.new_GLDecoder(), true)
	{
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0001874A File Offset: 0x0001694A
	public void AddAdapter(GLAdapter pAdapter)
	{
		libDCAPPINVOKE.GLDecoder_AddAdapter(this.swigCPtr, GLAdapter.getCPtr(pAdapter));
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0001875D File Offset: 0x0001695D
	public void RemoveAdapter(GLAdapter pAdapter)
	{
		libDCAPPINVOKE.GLDecoder_RemoveAdapter(this.swigCPtr, GLAdapter.getCPtr(pAdapter));
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x00018770 File Offset: 0x00016970
	public override bool SupportsId(ApiCallType id)
	{
		return libDCAPPINVOKE.GLDecoder_SupportsId(this.swigCPtr, (int)id);
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0001877E File Offset: 0x0001697E
	public override void ProcessFunctionCall(ApiCallType callId, IntPtr buffer, uint len)
	{
		libDCAPPINVOKE.GLDecoder_ProcessFunctionCall(this.swigCPtr, (int)callId, buffer, len);
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0001878E File Offset: 0x0001698E
	public override void ProcessMethodCall(ApiCallType arg0, uint arg1, IntPtr arg2, uint arg3)
	{
		libDCAPPINVOKE.GLDecoder_ProcessMethodCall(this.swigCPtr, (int)arg0, arg1, arg2, arg3);
	}

	// Token: 0x0400099C RID: 2460
	private HandleRef swigCPtr;
}
