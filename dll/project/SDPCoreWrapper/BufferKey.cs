using System;
using System.Runtime.InteropServices;

// Token: 0x02000012 RID: 18
public class BufferKey : IDisposable
{
	// Token: 0x06000065 RID: 101 RVA: 0x00002833 File Offset: 0x00000A33
	internal BufferKey(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000066 RID: 102 RVA: 0x0000284F File Offset: 0x00000A4F
	internal static HandleRef getCPtr(BufferKey obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00002868 File Offset: 0x00000A68
	~BufferKey()
	{
		this.Dispose();
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00002894 File Offset: 0x00000A94
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_BufferKey(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600006A RID: 106 RVA: 0x00002924 File Offset: 0x00000B24
	// (set) Token: 0x06000069 RID: 105 RVA: 0x00002914 File Offset: 0x00000B14
	public uint category
	{
		get
		{
			return SDPCorePINVOKE.BufferKey_category_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.BufferKey_category_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600006C RID: 108 RVA: 0x0000294C File Offset: 0x00000B4C
	// (set) Token: 0x0600006B RID: 107 RVA: 0x0000293E File Offset: 0x00000B3E
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.BufferKey_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.BufferKey_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00002966 File Offset: 0x00000B66
	public BufferKey(uint cat, uint _id)
		: this(SDPCorePINVOKE.new_BufferKey(cat, _id), true)
	{
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00002978 File Offset: 0x00000B78
	public bool EqualTo(BufferKey right)
	{
		bool flag = SDPCorePINVOKE.BufferKey_EqualTo(this.swigCPtr, BufferKey.getCPtr(right));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x000029A8 File Offset: 0x00000BA8
	public bool LessThan(BufferKey right)
	{
		bool flag = SDPCorePINVOKE.BufferKey_LessThan(this.swigCPtr, BufferKey.getCPtr(right));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0400000C RID: 12
	private HandleRef swigCPtr;

	// Token: 0x0400000D RID: 13
	protected bool swigCMemOwn;
}
