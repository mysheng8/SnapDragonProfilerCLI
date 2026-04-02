using System;
using System.Runtime.InteropServices;

// Token: 0x0200000C RID: 12
public class DCAPFileProcessor : IDisposable
{
	// Token: 0x06000076 RID: 118 RVA: 0x00002DA5 File Offset: 0x00000FA5
	internal DCAPFileProcessor(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00002DC1 File Offset: 0x00000FC1
	internal static HandleRef getCPtr(DCAPFileProcessor obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00002DD8 File Offset: 0x00000FD8
	~DCAPFileProcessor()
	{
		this.Dispose();
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00002E04 File Offset: 0x00001004
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_DCAPFileProcessor(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00002E84 File Offset: 0x00001084
	public DCAPFileProcessor()
		: this(libDCAPPINVOKE.new_DCAPFileProcessor(), true)
	{
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00002E92 File Offset: 0x00001092
	public bool IsActive()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_IsActive(this.swigCPtr);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00002E9F File Offset: 0x0000109F
	public bool IsTrimmedFile()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_IsTrimmedFile(this.swigCPtr);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00002EAC File Offset: 0x000010AC
	public bool IsCompressedFile()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_IsCompressedFile(this.swigCPtr);
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00002EB9 File Offset: 0x000010B9
	public bool OmitsTextureData()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_OmitsTextureData(this.swigCPtr);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00002EC6 File Offset: 0x000010C6
	public bool OmitsAllData()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_OmitsAllData(this.swigCPtr);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00002ED3 File Offset: 0x000010D3
	public bool HasThreadId()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_HasThreadId(this.swigCPtr);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00002EE0 File Offset: 0x000010E0
	public bool HasTimestamp()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_HasTimestamp(this.swigCPtr);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00002EED File Offset: 0x000010ED
	public bool HasTrailer()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_HasTrailer(this.swigCPtr);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00002EFA File Offset: 0x000010FA
	public ulong GetCurrentBlockPosition()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_GetCurrentBlockPosition(this.swigCPtr);
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00002F07 File Offset: 0x00001107
	public BlockType GetCurrentBlockType()
	{
		return (BlockType)libDCAPPINVOKE.DCAPFileProcessor_GetCurrentBlockType(this.swigCPtr);
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00002F14 File Offset: 0x00001114
	public uint GetCurrentFrame()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_GetCurrentFrame(this.swigCPtr);
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00002F21 File Offset: 0x00001121
	public uint GetNumFrames()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_GetNumFrames(this.swigCPtr);
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00002F2E File Offset: 0x0000112E
	public ulong GetFileSize()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_GetFileSize(this.swigCPtr);
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00002F3B File Offset: 0x0000113B
	public uint GetFileVersion()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_GetFileVersion(this.swigCPtr);
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00002F48 File Offset: 0x00001148
	public SWIGTYPE_p_uint16_t GetFileVersionMajor()
	{
		return new SWIGTYPE_p_uint16_t(libDCAPPINVOKE.DCAPFileProcessor_GetFileVersionMajor(this.swigCPtr), true);
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00002F5B File Offset: 0x0000115B
	public SWIGTYPE_p_uint16_t GetFileVersionMinor()
	{
		return new SWIGTYPE_p_uint16_t(libDCAPPINVOKE.DCAPFileProcessor_GetFileVersionMinor(this.swigCPtr), true);
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00002F6E File Offset: 0x0000116E
	public CompressionAlgorithm GetCompressionAlgorithm()
	{
		return (CompressionAlgorithm)libDCAPPINVOKE.DCAPFileProcessor_GetCompressionAlgorithm(this.swigCPtr);
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00002F7B File Offset: 0x0000117B
	public ulong GetBytesRead()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_GetBytesRead(this.swigCPtr);
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00002F88 File Offset: 0x00001188
	public void SetLoopFrame(uint id)
	{
		libDCAPPINVOKE.DCAPFileProcessor_SetLoopFrame(this.swigCPtr, id);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00002F96 File Offset: 0x00001196
	public void SetLoopCount(uint count)
	{
		libDCAPPINVOKE.DCAPFileProcessor_SetLoopCount(this.swigCPtr, count);
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00002FA4 File Offset: 0x000011A4
	public uint GetLoopFrame()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_GetLoopFrame(this.swigCPtr);
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00002FB1 File Offset: 0x000011B1
	public void SetFilterThreads(SWIGTYPE_p_std__vectorT_unsigned_int_t threads)
	{
		libDCAPPINVOKE.DCAPFileProcessor_SetFilterThreads(this.swigCPtr, SWIGTYPE_p_std__vectorT_unsigned_int_t.getCPtr(threads));
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00002FD1 File Offset: 0x000011D1
	public void SetFilterThreadStartFrame(uint startFrame)
	{
		libDCAPPINVOKE.DCAPFileProcessor_SetFilterThreadStartFrame(this.swigCPtr, startFrame);
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00002FDF File Offset: 0x000011DF
	public uint GetCurrentThreadNumber()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_GetCurrentThreadNumber(this.swigCPtr);
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00002FEC File Offset: 0x000011EC
	public uint GetCurrentThreadId()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_GetCurrentThreadId(this.swigCPtr);
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00002FF9 File Offset: 0x000011F9
	public ApiCallType GetLastApiCallId()
	{
		return (ApiCallType)libDCAPPINVOKE.DCAPFileProcessor_GetLastApiCallId(this.swigCPtr);
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00003006 File Offset: 0x00001206
	public DCAPStatus Initialize(DCAPConsumer pConsumer, DataReader pReader, ulong fileSize)
	{
		DCAPStatus dcapstatus = (DCAPStatus)libDCAPPINVOKE.DCAPFileProcessor_Initialize(this.swigCPtr, DCAPConsumer.getCPtr(pConsumer), DataReader.getCPtr(pReader), fileSize);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return dcapstatus;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x0000302D File Offset: 0x0000122D
	public bool AtEof()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_AtEof(this.swigCPtr);
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0000303A File Offset: 0x0000123A
	public void InvalidateState()
	{
		libDCAPPINVOKE.DCAPFileProcessor_InvalidateState(this.swigCPtr);
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00003047 File Offset: 0x00001247
	public bool ProcessNextBlock()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_ProcessNextBlock(this.swigCPtr);
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00003054 File Offset: 0x00001254
	public bool ProcessNextFrame()
	{
		return libDCAPPINVOKE.DCAPFileProcessor_ProcessNextFrame(this.swigCPtr);
	}

	// Token: 0x040004EA RID: 1258
	private HandleRef swigCPtr;

	// Token: 0x040004EB RID: 1259
	protected bool swigCMemOwn;
}
