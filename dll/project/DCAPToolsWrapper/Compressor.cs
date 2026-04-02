using System;
using System.Runtime.InteropServices;

// Token: 0x02000007 RID: 7
public class Compressor : IDisposable
{
	// Token: 0x06000032 RID: 50 RVA: 0x000024B7 File Offset: 0x000006B7
	internal Compressor(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x000024D3 File Offset: 0x000006D3
	internal static HandleRef getCPtr(Compressor obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x000024EC File Offset: 0x000006EC
	~Compressor()
	{
		this.Dispose();
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002518 File Offset: 0x00000718
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_Compressor(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002598 File Offset: 0x00000798
	public virtual bool Encode(IntPtr pRawByteStreamIn, uint sizeIn, SWIGTYPE_p_p_unsigned_char ppCompressorByteStreamOut, IntPtr pSizeOut)
	{
		return libDCAPPINVOKE.Compressor_Encode(this.swigCPtr, pRawByteStreamIn, sizeIn, SWIGTYPE_p_p_unsigned_char.getCPtr(ppCompressorByteStreamOut), pSizeOut);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x000025AF File Offset: 0x000007AF
	public virtual bool Decode(IntPtr pCompressorByteStreamIn, uint sizeIn, SWIGTYPE_p_p_unsigned_char ppRawByteStreamOut, IntPtr pSizeInOut)
	{
		return libDCAPPINVOKE.Compressor_Decode__SWIG_0(this.swigCPtr, pCompressorByteStreamIn, sizeIn, SWIGTYPE_p_p_unsigned_char.getCPtr(ppRawByteStreamOut), pSizeInOut);
	}

	// Token: 0x06000038 RID: 56 RVA: 0x000025C6 File Offset: 0x000007C6
	public virtual bool Decode(IntPtr pCompressorByteStreamIn, uint sizeIn, IntPtr pRawByteStreamOut, IntPtr pSizeInOut)
	{
		return libDCAPPINVOKE.Compressor_Decode__SWIG_1(this.swigCPtr, pCompressorByteStreamIn, sizeIn, pRawByteStreamOut, pSizeInOut);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000025D8 File Offset: 0x000007D8
	public virtual Compressor.CompressorQuality GetQuality()
	{
		return (Compressor.CompressorQuality)libDCAPPINVOKE.Compressor_GetQuality(this.swigCPtr);
	}

	// Token: 0x040004DE RID: 1246
	private HandleRef swigCPtr;

	// Token: 0x040004DF RID: 1247
	protected bool swigCMemOwn;

	// Token: 0x040004E0 RID: 1248
	public static readonly Compressor.CompressorQuality CompressorQualityDefault = (Compressor.CompressorQuality)libDCAPPINVOKE.Compressor_CompressorQualityDefault_get();

	// Token: 0x040004E1 RID: 1249
	public static readonly uint CompressorBufferSizeInBytesDefault = libDCAPPINVOKE.Compressor_CompressorBufferSizeInBytesDefault_get();

	// Token: 0x0200003A RID: 58
	public enum CompressorQuality
	{
		// Token: 0x04000BA5 RID: 2981
		CompressorQualityNone,
		// Token: 0x04000BA6 RID: 2982
		CompressorQualityLow,
		// Token: 0x04000BA7 RID: 2983
		CompressorQualityMedium,
		// Token: 0x04000BA8 RID: 2984
		CompressorQualityHigh
	}
}
