using System;
using System.Runtime.InteropServices;

// Token: 0x02000021 RID: 33
public class CoreObject : IDisposable
{
	// Token: 0x0600016C RID: 364 RVA: 0x0000519E File Offset: 0x0000339E
	internal CoreObject(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600016D RID: 365 RVA: 0x000051BA File Offset: 0x000033BA
	internal static HandleRef getCPtr(CoreObject obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600016E RID: 366 RVA: 0x000051D4 File Offset: 0x000033D4
	~CoreObject()
	{
		this.Dispose();
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00005200 File Offset: 0x00003400
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CoreObject(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00005280 File Offset: 0x00003480
	public CoreObject()
		: this(SDPCorePINVOKE.new_CoreObject(), true)
	{
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00005290 File Offset: 0x00003490
	public uint GetID()
	{
		return SDPCorePINVOKE.CoreObject_GetID(this.swigCPtr);
	}

	// Token: 0x06000172 RID: 370 RVA: 0x000052AC File Offset: 0x000034AC
	public static long GetSystemTimeUS()
	{
		return SDPCorePINVOKE.CoreObject_GetSystemTimeUS();
	}

	// Token: 0x06000173 RID: 371 RVA: 0x000052C0 File Offset: 0x000034C0
	public static long GetSystemTimeMS()
	{
		return SDPCorePINVOKE.CoreObject_GetSystemTimeMS();
	}

	// Token: 0x0400005C RID: 92
	private HandleRef swigCPtr;

	// Token: 0x0400005D RID: 93
	protected bool swigCMemOwn;
}
