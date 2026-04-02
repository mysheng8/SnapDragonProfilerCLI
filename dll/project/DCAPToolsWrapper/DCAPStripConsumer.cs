using System;
using System.Runtime.InteropServices;

// Token: 0x0200000E RID: 14
public class DCAPStripConsumer : DCAPCompressConsumer
{
	// Token: 0x0600009A RID: 154 RVA: 0x00003061 File Offset: 0x00001261
	internal DCAPStripConsumer(IntPtr cPtr, bool cMemoryOwn)
		: base(libDCAPPINVOKE.DCAPStripConsumer_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0000307D File Offset: 0x0000127D
	internal static HandleRef getCPtr(DCAPStripConsumer obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00003094 File Offset: 0x00001294
	~DCAPStripConsumer()
	{
		this.Dispose();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000030C0 File Offset: 0x000012C0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_DCAPStripConsumer(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00003144 File Offset: 0x00001344
	public DCAPStripConsumer(DataWriter dataWriter)
		: this(libDCAPPINVOKE.new_DCAPStripConsumer(DataWriter.getCPtr(dataWriter)), true)
	{
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00003165 File Offset: 0x00001365
	public override void ProcessFileStart(SWIGTYPE_p_Data__FileHeader header)
	{
		libDCAPPINVOKE.DCAPStripConsumer_ProcessFileStart(this.swigCPtr, SWIGTYPE_p_Data__FileHeader.getCPtr(header));
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00003185 File Offset: 0x00001385
	public override void ProcessMethodCall(SWIGTYPE_p_Data__SubBlockHeader header, SWIGTYPE_p_Data__MethodCallDesc desc, IntPtr pParameters, uint size)
	{
		libDCAPPINVOKE.DCAPStripConsumer_ProcessMethodCall(this.swigCPtr, SWIGTYPE_p_Data__SubBlockHeader.getCPtr(header), SWIGTYPE_p_Data__MethodCallDesc.getCPtr(desc), pParameters, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x000031AE File Offset: 0x000013AE
	public override void ProcessFunctionCall(SWIGTYPE_p_Data__SubBlockHeader header, SWIGTYPE_p_Data__FunctionCallDesc desc, IntPtr pParameters, uint size)
	{
		libDCAPPINVOKE.DCAPStripConsumer_ProcessFunctionCall(this.swigCPtr, SWIGTYPE_p_Data__SubBlockHeader.getCPtr(header), SWIGTYPE_p_Data__FunctionCallDesc.getCPtr(desc), pParameters, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x000031D7 File Offset: 0x000013D7
	public override void ProcessMetadata(SWIGTYPE_p_Data__SubBlockHeader header, SWIGTYPE_p_Data__MetadataDesc desc, IntPtr pMetadata, uint size)
	{
		libDCAPPINVOKE.DCAPStripConsumer_ProcessMetadata(this.swigCPtr, SWIGTYPE_p_Data__SubBlockHeader.getCPtr(header), SWIGTYPE_p_Data__MetadataDesc.getCPtr(desc), pMetadata, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x040004F8 RID: 1272
	private HandleRef swigCPtr;
}
