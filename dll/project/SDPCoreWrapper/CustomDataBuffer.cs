using System;
using System.Runtime.InteropServices;

// Token: 0x02000022 RID: 34
public class CustomDataBuffer : IDisposable
{
	// Token: 0x06000174 RID: 372 RVA: 0x000052D4 File Offset: 0x000034D4
	internal CustomDataBuffer(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000175 RID: 373 RVA: 0x000052F0 File Offset: 0x000034F0
	internal static HandleRef getCPtr(CustomDataBuffer obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00005308 File Offset: 0x00003508
	~CustomDataBuffer()
	{
		this.Dispose();
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00005334 File Offset: 0x00003534
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CustomDataBuffer(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000179 RID: 377 RVA: 0x000053C8 File Offset: 0x000035C8
	// (set) Token: 0x06000178 RID: 376 RVA: 0x000053B4 File Offset: 0x000035B4
	public SWIGTYPE_p_uint8_t buffer
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.CustomDataBuffer_buffer_get(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_uint8_t(intPtr, false);
		}
		set
		{
			SDPCorePINVOKE.CustomDataBuffer_buffer_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
		}
	}

	// Token: 0x0600017A RID: 378 RVA: 0x000053FA File Offset: 0x000035FA
	public CustomDataBuffer()
		: this(SDPCorePINVOKE.new_CustomDataBuffer(), true)
	{
	}

	// Token: 0x0400005E RID: 94
	private HandleRef swigCPtr;

	// Token: 0x0400005F RID: 95
	protected bool swigCMemOwn;
}
