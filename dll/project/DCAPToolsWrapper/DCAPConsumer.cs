using System;
using System.Runtime.InteropServices;

// Token: 0x0200000B RID: 11
public class DCAPConsumer : IDisposable
{
	// Token: 0x0600006A RID: 106 RVA: 0x00002BE0 File Offset: 0x00000DE0
	internal DCAPConsumer(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00002BFC File Offset: 0x00000DFC
	internal static HandleRef getCPtr(DCAPConsumer obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00002C14 File Offset: 0x00000E14
	~DCAPConsumer()
	{
		this.Dispose();
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00002C40 File Offset: 0x00000E40
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_DCAPConsumer(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00002CC0 File Offset: 0x00000EC0
	public virtual void ProcessFileStart(SWIGTYPE_p_Data__FileHeader header)
	{
		libDCAPPINVOKE.DCAPConsumer_ProcessFileStart(this.swigCPtr, SWIGTYPE_p_Data__FileHeader.getCPtr(header));
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00002CE0 File Offset: 0x00000EE0
	public virtual void ProcessFileEnd()
	{
		libDCAPPINVOKE.DCAPConsumer_ProcessFileEnd(this.swigCPtr);
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00002CED File Offset: 0x00000EED
	public virtual void ProcessBlockStart(SWIGTYPE_p_Data__BlockHeader header)
	{
		libDCAPPINVOKE.DCAPConsumer_ProcessBlockStart(this.swigCPtr, SWIGTYPE_p_Data__BlockHeader.getCPtr(header));
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00002D0D File Offset: 0x00000F0D
	public virtual void ProcessBlockEnd(ulong trailer)
	{
		libDCAPPINVOKE.DCAPConsumer_ProcessBlockEnd(this.swigCPtr, trailer);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00002D1B File Offset: 0x00000F1B
	public virtual void ProcessFrameNumber(uint frameNumber, ulong size)
	{
		libDCAPPINVOKE.DCAPConsumer_ProcessFrameNumber(this.swigCPtr, frameNumber, size);
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00002D2A File Offset: 0x00000F2A
	public virtual void ProcessMethodCall(SWIGTYPE_p_Data__SubBlockHeader header, SWIGTYPE_p_Data__MethodCallDesc desc, IntPtr pParameters, uint size)
	{
		libDCAPPINVOKE.DCAPConsumer_ProcessMethodCall(this.swigCPtr, SWIGTYPE_p_Data__SubBlockHeader.getCPtr(header), SWIGTYPE_p_Data__MethodCallDesc.getCPtr(desc), pParameters, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00002D53 File Offset: 0x00000F53
	public virtual void ProcessFunctionCall(SWIGTYPE_p_Data__SubBlockHeader header, SWIGTYPE_p_Data__FunctionCallDesc desc, IntPtr pParameters, uint size)
	{
		libDCAPPINVOKE.DCAPConsumer_ProcessFunctionCall(this.swigCPtr, SWIGTYPE_p_Data__SubBlockHeader.getCPtr(header), SWIGTYPE_p_Data__FunctionCallDesc.getCPtr(desc), pParameters, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00002D7C File Offset: 0x00000F7C
	public virtual void ProcessMetadata(SWIGTYPE_p_Data__SubBlockHeader header, SWIGTYPE_p_Data__MetadataDesc desc, IntPtr pMetadata, uint size)
	{
		libDCAPPINVOKE.DCAPConsumer_ProcessMetadata(this.swigCPtr, SWIGTYPE_p_Data__SubBlockHeader.getCPtr(header), SWIGTYPE_p_Data__MetadataDesc.getCPtr(desc), pMetadata, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x040004E8 RID: 1256
	private HandleRef swigCPtr;

	// Token: 0x040004E9 RID: 1257
	protected bool swigCMemOwn;
}
