using System;
using System.Runtime.InteropServices;

// Token: 0x02000005 RID: 5
public class CaptureFileReader : IDisposable
{
	// Token: 0x06000006 RID: 6 RVA: 0x00002136 File Offset: 0x00000336
	internal CaptureFileReader(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002152 File Offset: 0x00000352
	internal static HandleRef getCPtr(CaptureFileReader obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000216C File Offset: 0x0000036C
	~CaptureFileReader()
	{
		this.Dispose();
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002198 File Offset: 0x00000398
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_CaptureFileReader(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002218 File Offset: 0x00000418
	public CaptureFileReader()
		: this(libDCAPPINVOKE.new_CaptureFileReader(), true)
	{
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002226 File Offset: 0x00000426
	public void AddBlockEventHandler(SWIGTYPE_p_Data__BlockEventHandler pHandler)
	{
		libDCAPPINVOKE.CaptureFileReader_AddBlockEventHandler(this.swigCPtr, SWIGTYPE_p_Data__BlockEventHandler.getCPtr(pHandler));
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002239 File Offset: 0x00000439
	public void RemoveBlockEventHandler(SWIGTYPE_p_Data__BlockEventHandler pHandler)
	{
		libDCAPPINVOKE.CaptureFileReader_RemoveBlockEventHandler(this.swigCPtr, SWIGTYPE_p_Data__BlockEventHandler.getCPtr(pHandler));
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000224C File Offset: 0x0000044C
	public void AddDecoder(Decoder pDecoder)
	{
		libDCAPPINVOKE.CaptureFileReader_AddDecoder(this.swigCPtr, Decoder.getCPtr(pDecoder));
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000225F File Offset: 0x0000045F
	public void RemoveDecoder(Decoder pDecoder)
	{
		libDCAPPINVOKE.CaptureFileReader_RemoveDecoder(this.swigCPtr, Decoder.getCPtr(pDecoder));
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002272 File Offset: 0x00000472
	public void AddMetaHandler(MetaHandler pHandler)
	{
		libDCAPPINVOKE.CaptureFileReader_AddMetaHandler(this.swigCPtr, MetaHandler.getCPtr(pHandler));
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002285 File Offset: 0x00000485
	public void RemoveMetaHandler(MetaHandler pHandler)
	{
		libDCAPPINVOKE.CaptureFileReader_RemoveMetaHandler(this.swigCPtr, MetaHandler.getCPtr(pHandler));
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002298 File Offset: 0x00000498
	public bool IsActive()
	{
		return libDCAPPINVOKE.CaptureFileReader_IsActive(this.swigCPtr);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000022A5 File Offset: 0x000004A5
	public bool IsTrimmedFile()
	{
		return libDCAPPINVOKE.CaptureFileReader_IsTrimmedFile(this.swigCPtr);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000022B2 File Offset: 0x000004B2
	public bool IsCompressedFile()
	{
		return libDCAPPINVOKE.CaptureFileReader_IsCompressedFile(this.swigCPtr);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000022BF File Offset: 0x000004BF
	public bool OmitsTextureData()
	{
		return libDCAPPINVOKE.CaptureFileReader_OmitsTextureData(this.swigCPtr);
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000022CC File Offset: 0x000004CC
	public bool OmitsAllData()
	{
		return libDCAPPINVOKE.CaptureFileReader_OmitsAllData(this.swigCPtr);
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000022D9 File Offset: 0x000004D9
	public bool HasThreadId()
	{
		return libDCAPPINVOKE.CaptureFileReader_HasThreadId(this.swigCPtr);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000022E6 File Offset: 0x000004E6
	public bool HasTimestamp()
	{
		return libDCAPPINVOKE.CaptureFileReader_HasTimestamp(this.swigCPtr);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000022F3 File Offset: 0x000004F3
	public bool HasTrailer()
	{
		return libDCAPPINVOKE.CaptureFileReader_HasTrailer(this.swigCPtr);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002300 File Offset: 0x00000500
	public void Initialize(DataReader pReader, ulong fileSize)
	{
		libDCAPPINVOKE.CaptureFileReader_Initialize__SWIG_0(this.swigCPtr, DataReader.getCPtr(pReader), fileSize);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002321 File Offset: 0x00000521
	public void Initialize(DataReader pReader)
	{
		libDCAPPINVOKE.CaptureFileReader_Initialize__SWIG_1(this.swigCPtr, DataReader.getCPtr(pReader));
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002341 File Offset: 0x00000541
	public ulong GetCurrentBlockPosition()
	{
		return libDCAPPINVOKE.CaptureFileReader_GetCurrentBlockPosition(this.swigCPtr);
	}

	// Token: 0x0600001C RID: 28 RVA: 0x0000234E File Offset: 0x0000054E
	public BlockType GetCurrentBlockType()
	{
		return (BlockType)libDCAPPINVOKE.CaptureFileReader_GetCurrentBlockType(this.swigCPtr);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000235B File Offset: 0x0000055B
	public uint GetCurrentFrame()
	{
		return libDCAPPINVOKE.CaptureFileReader_GetCurrentFrame(this.swigCPtr);
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002368 File Offset: 0x00000568
	public uint GetNumFrames()
	{
		return libDCAPPINVOKE.CaptureFileReader_GetNumFrames(this.swigCPtr);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002375 File Offset: 0x00000575
	public ulong GetFileSize()
	{
		return libDCAPPINVOKE.CaptureFileReader_GetFileSize(this.swigCPtr);
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002382 File Offset: 0x00000582
	public uint GetFileVersion()
	{
		return libDCAPPINVOKE.CaptureFileReader_GetFileVersion(this.swigCPtr);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0000238F File Offset: 0x0000058F
	public SWIGTYPE_p_uint16_t GetFileVersionMajor()
	{
		return new SWIGTYPE_p_uint16_t(libDCAPPINVOKE.CaptureFileReader_GetFileVersionMajor(this.swigCPtr), true);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000023A2 File Offset: 0x000005A2
	public SWIGTYPE_p_uint16_t GetFileVersionMinor()
	{
		return new SWIGTYPE_p_uint16_t(libDCAPPINVOKE.CaptureFileReader_GetFileVersionMinor(this.swigCPtr), true);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000023B5 File Offset: 0x000005B5
	public void SetLoopFrame(uint id)
	{
		libDCAPPINVOKE.CaptureFileReader_SetLoopFrame(this.swigCPtr, id);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000023C3 File Offset: 0x000005C3
	public void SetLoopCount(uint count)
	{
		libDCAPPINVOKE.CaptureFileReader_SetLoopCount(this.swigCPtr, count);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000023D1 File Offset: 0x000005D1
	public uint GetLoopFrame()
	{
		return libDCAPPINVOKE.CaptureFileReader_GetLoopFrame(this.swigCPtr);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000023DE File Offset: 0x000005DE
	public void SetFilterThreads(SWIGTYPE_p_std__vectorT_unsigned_int_t ids)
	{
		libDCAPPINVOKE.CaptureFileReader_SetFilterThreads(this.swigCPtr, SWIGTYPE_p_std__vectorT_unsigned_int_t.getCPtr(ids));
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000027 RID: 39 RVA: 0x000023FE File Offset: 0x000005FE
	public void SetFilterThreadStartFrame(uint startFrame)
	{
		libDCAPPINVOKE.CaptureFileReader_SetFilterThreadStartFrame(this.swigCPtr, startFrame);
	}

	// Token: 0x06000028 RID: 40 RVA: 0x0000240C File Offset: 0x0000060C
	public uint GetCurrentThreadNumber()
	{
		return libDCAPPINVOKE.CaptureFileReader_GetCurrentThreadNumber(this.swigCPtr);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002419 File Offset: 0x00000619
	public uint GetCurrentThreadId()
	{
		return libDCAPPINVOKE.CaptureFileReader_GetCurrentThreadId(this.swigCPtr);
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002426 File Offset: 0x00000626
	public ApiCallType GetLastCallId()
	{
		return (ApiCallType)libDCAPPINVOKE.CaptureFileReader_GetLastCallId(this.swigCPtr);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002433 File Offset: 0x00000633
	public void InvalidateState()
	{
		libDCAPPINVOKE.CaptureFileReader_InvalidateState(this.swigCPtr);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002440 File Offset: 0x00000640
	public bool SetCurrentFrame(uint arg0)
	{
		return libDCAPPINVOKE.CaptureFileReader_SetCurrentFrame(this.swigCPtr, arg0);
	}

	// Token: 0x0600002D RID: 45 RVA: 0x0000244E File Offset: 0x0000064E
	public bool ProcessNextBlock()
	{
		return libDCAPPINVOKE.CaptureFileReader_ProcessNextBlock(this.swigCPtr);
	}

	// Token: 0x0600002E RID: 46 RVA: 0x0000245B File Offset: 0x0000065B
	public bool ProcessNextFrame()
	{
		return libDCAPPINVOKE.CaptureFileReader_ProcessNextFrame(this.swigCPtr);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002468 File Offset: 0x00000668
	public bool GenerateFrameIndex(SWIGTYPE_p_std__vectorT_Data__CaptureFileReader__FrameEntry_t arg0)
	{
		return libDCAPPINVOKE.CaptureFileReader_GenerateFrameIndex(this.swigCPtr, SWIGTYPE_p_std__vectorT_Data__CaptureFileReader__FrameEntry_t.getCPtr(arg0));
	}

	// Token: 0x06000030 RID: 48 RVA: 0x0000247B File Offset: 0x0000067B
	public CompressionAlgorithm GetCompressionAlgorithm()
	{
		return (CompressionAlgorithm)libDCAPPINVOKE.CaptureFileReader_GetCompressionAlgorithm(this.swigCPtr);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002488 File Offset: 0x00000688
	public Compressor GetCompressor()
	{
		IntPtr intPtr = libDCAPPINVOKE.CaptureFileReader_GetCompressor(this.swigCPtr);
		if (!(intPtr == IntPtr.Zero))
		{
			return new Compressor(intPtr, false);
		}
		return null;
	}

	// Token: 0x040004D3 RID: 1235
	private HandleRef swigCPtr;

	// Token: 0x040004D4 RID: 1236
	protected bool swigCMemOwn;

	// Token: 0x02000039 RID: 57
	public class FrameEntry : IDisposable
	{
		// Token: 0x060009A6 RID: 2470 RVA: 0x0001A231 File Offset: 0x00018431
		internal FrameEntry(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0001A24D File Offset: 0x0001844D
		internal static HandleRef getCPtr(CaptureFileReader.FrameEntry obj)
		{
			if (obj != null)
			{
				return obj.swigCPtr;
			}
			return new HandleRef(null, IntPtr.Zero);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0001A264 File Offset: 0x00018464
		~FrameEntry()
		{
			this.Dispose();
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0001A290 File Offset: 0x00018490
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						libDCAPPINVOKE.delete_CaptureFileReader_FrameEntry(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0001A31E File Offset: 0x0001851E
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x0001A310 File Offset: 0x00018510
		public uint number
		{
			get
			{
				return libDCAPPINVOKE.CaptureFileReader_FrameEntry_number_get(this.swigCPtr);
			}
			set
			{
				libDCAPPINVOKE.CaptureFileReader_FrameEntry_number_set(this.swigCPtr, value);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0001A339 File Offset: 0x00018539
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x0001A32B File Offset: 0x0001852B
		public ulong position
		{
			get
			{
				return libDCAPPINVOKE.CaptureFileReader_FrameEntry_position_get(this.swigCPtr);
			}
			set
			{
				libDCAPPINVOKE.CaptureFileReader_FrameEntry_position_set(this.swigCPtr, value);
			}
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0001A346 File Offset: 0x00018546
		public FrameEntry()
			: this(libDCAPPINVOKE.new_CaptureFileReader_FrameEntry(), true)
		{
		}

		// Token: 0x04000BA2 RID: 2978
		private HandleRef swigCPtr;

		// Token: 0x04000BA3 RID: 2979
		protected bool swigCMemOwn;
	}
}
