using System;
using System.Runtime.InteropServices;

// Token: 0x0200000A RID: 10
public class DCAPCompressConsumer : DCAPConsumer
{
	// Token: 0x06000054 RID: 84 RVA: 0x00002981 File Offset: 0x00000B81
	internal DCAPCompressConsumer(IntPtr cPtr, bool cMemoryOwn)
		: base(libDCAPPINVOKE.DCAPCompressConsumer_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x0000299D File Offset: 0x00000B9D
	internal static HandleRef getCPtr(DCAPCompressConsumer obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000029B4 File Offset: 0x00000BB4
	~DCAPCompressConsumer()
	{
		this.Dispose();
	}

	// Token: 0x06000057 RID: 87 RVA: 0x000029E0 File Offset: 0x00000BE0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_DCAPCompressConsumer(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00002A64 File Offset: 0x00000C64
	public DCAPCompressConsumer(DataWriter dataWriter)
		: this(libDCAPPINVOKE.new_DCAPCompressConsumer(DataWriter.getCPtr(dataWriter)), true)
	{
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00002A85 File Offset: 0x00000C85
	public override void ProcessFileStart(SWIGTYPE_p_Data__FileHeader header)
	{
		libDCAPPINVOKE.DCAPCompressConsumer_ProcessFileStart(this.swigCPtr, SWIGTYPE_p_Data__FileHeader.getCPtr(header));
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00002AA5 File Offset: 0x00000CA5
	public override void ProcessFileEnd()
	{
		libDCAPPINVOKE.DCAPCompressConsumer_ProcessFileEnd(this.swigCPtr);
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00002AB2 File Offset: 0x00000CB2
	public override void ProcessBlockStart(SWIGTYPE_p_Data__BlockHeader header)
	{
		libDCAPPINVOKE.DCAPCompressConsumer_ProcessBlockStart(this.swigCPtr, SWIGTYPE_p_Data__BlockHeader.getCPtr(header));
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00002AD2 File Offset: 0x00000CD2
	public override void ProcessBlockEnd(ulong trailer)
	{
		libDCAPPINVOKE.DCAPCompressConsumer_ProcessBlockEnd(this.swigCPtr, trailer);
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00002AE0 File Offset: 0x00000CE0
	public override void ProcessFrameNumber(uint frameNumber, ulong size)
	{
		libDCAPPINVOKE.DCAPCompressConsumer_ProcessFrameNumber(this.swigCPtr, frameNumber, size);
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00002AEF File Offset: 0x00000CEF
	public override void ProcessMethodCall(SWIGTYPE_p_Data__SubBlockHeader header, SWIGTYPE_p_Data__MethodCallDesc desc, IntPtr pParameters, uint size)
	{
		libDCAPPINVOKE.DCAPCompressConsumer_ProcessMethodCall(this.swigCPtr, SWIGTYPE_p_Data__SubBlockHeader.getCPtr(header), SWIGTYPE_p_Data__MethodCallDesc.getCPtr(desc), pParameters, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00002B18 File Offset: 0x00000D18
	public override void ProcessFunctionCall(SWIGTYPE_p_Data__SubBlockHeader header, SWIGTYPE_p_Data__FunctionCallDesc desc, IntPtr pParameters, uint size)
	{
		libDCAPPINVOKE.DCAPCompressConsumer_ProcessFunctionCall(this.swigCPtr, SWIGTYPE_p_Data__SubBlockHeader.getCPtr(header), SWIGTYPE_p_Data__FunctionCallDesc.getCPtr(desc), pParameters, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00002B41 File Offset: 0x00000D41
	public override void ProcessMetadata(SWIGTYPE_p_Data__SubBlockHeader header, SWIGTYPE_p_Data__MetadataDesc desc, IntPtr pMetadata, uint size)
	{
		libDCAPPINVOKE.DCAPCompressConsumer_ProcessMetadata(this.swigCPtr, SWIGTYPE_p_Data__SubBlockHeader.getCPtr(header), SWIGTYPE_p_Data__MetadataDesc.getCPtr(desc), pMetadata, size);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00002B6A File Offset: 0x00000D6A
	public bool IsInitialized()
	{
		return libDCAPPINVOKE.DCAPCompressConsumer_IsInitialized(this.swigCPtr);
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00002B77 File Offset: 0x00000D77
	public DCAPStatus InitializeDecompression()
	{
		return (DCAPStatus)libDCAPPINVOKE.DCAPCompressConsumer_InitializeDecompression(this.swigCPtr);
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00002B84 File Offset: 0x00000D84
	public DCAPStatus InitializeCompression(CompressionAlgorithm compressionAlgorithm, Compressor.CompressorQuality quality)
	{
		return (DCAPStatus)libDCAPPINVOKE.DCAPCompressConsumer_InitializeCompression(this.swigCPtr, (int)compressionAlgorithm, (int)quality);
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00002B93 File Offset: 0x00000D93
	public uint GetApiCallMaxSize()
	{
		return libDCAPPINVOKE.DCAPCompressConsumer_GetApiCallMaxSize(this.swigCPtr);
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00002BA0 File Offset: 0x00000DA0
	public uint GetApiCallExpandedCount()
	{
		return libDCAPPINVOKE.DCAPCompressConsumer_GetApiCallExpandedCount(this.swigCPtr);
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00002BAD File Offset: 0x00000DAD
	public bool DecompressionEnabled()
	{
		return libDCAPPINVOKE.DCAPCompressConsumer_DecompressionEnabled(this.swigCPtr);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00002BBA File Offset: 0x00000DBA
	public bool CompressionEnabled()
	{
		return libDCAPPINVOKE.DCAPCompressConsumer_CompressionEnabled(this.swigCPtr);
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00002BC7 File Offset: 0x00000DC7
	public CompressionAlgorithm GetCompressionAlgorithm()
	{
		return (CompressionAlgorithm)libDCAPPINVOKE.DCAPCompressConsumer_GetCompressionAlgorithm(this.swigCPtr);
	}

	// Token: 0x040004E6 RID: 1254
	private HandleRef swigCPtr;

	// Token: 0x040004E7 RID: 1255
	public static readonly uint EncodingBufferInitialSize = libDCAPPINVOKE.DCAPCompressConsumer_EncodingBufferInitialSize_get();
}
