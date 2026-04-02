using System;
using System.Runtime.InteropServices;

// Token: 0x02000010 RID: 16
public class BinaryData : IDisposable
{
	// Token: 0x0600004C RID: 76 RVA: 0x00002477 File Offset: 0x00000677
	internal BinaryData(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00002493 File Offset: 0x00000693
	internal static HandleRef getCPtr(BinaryData obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000024AC File Offset: 0x000006AC
	~BinaryData()
	{
		this.Dispose();
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000024D8 File Offset: 0x000006D8
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_BinaryData(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000051 RID: 81 RVA: 0x00002568 File Offset: 0x00000768
	// (set) Token: 0x06000050 RID: 80 RVA: 0x00002558 File Offset: 0x00000758
	public uint capture
	{
		get
		{
			return SDPCorePINVOKE.BinaryData_capture_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.BinaryData_capture_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000053 RID: 83 RVA: 0x00002590 File Offset: 0x00000790
	// (set) Token: 0x06000052 RID: 82 RVA: 0x00002582 File Offset: 0x00000782
	public uint bufferCategory
	{
		get
		{
			return SDPCorePINVOKE.BinaryData_bufferCategory_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.BinaryData_bufferCategory_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000055 RID: 85 RVA: 0x000025B8 File Offset: 0x000007B8
	// (set) Token: 0x06000054 RID: 84 RVA: 0x000025AA File Offset: 0x000007AA
	public uint bufferID
	{
		get
		{
			return SDPCorePINVOKE.BinaryData_bufferID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.BinaryData_bufferID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000025D2 File Offset: 0x000007D2
	public BinaryData()
		: this(SDPCorePINVOKE.new_BinaryData(), true)
	{
	}

	// Token: 0x04000008 RID: 8
	private HandleRef swigCPtr;

	// Token: 0x04000009 RID: 9
	protected bool swigCMemOwn;
}
