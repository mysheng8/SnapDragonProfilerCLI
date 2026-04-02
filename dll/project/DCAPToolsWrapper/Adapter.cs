using System;
using System.Runtime.InteropServices;

// Token: 0x02000002 RID: 2
public class Adapter : IDisposable
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
	internal Adapter(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
	internal static HandleRef getCPtr(Adapter obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000207C File Offset: 0x0000027C
	~Adapter()
	{
		this.Dispose();
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000020A8 File Offset: 0x000002A8
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_Adapter(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002128 File Offset: 0x00000328
	public virtual void SetCurrentThread(uint id)
	{
		libDCAPPINVOKE.Adapter_SetCurrentThread(this.swigCPtr, id);
	}

	// Token: 0x04000001 RID: 1
	private HandleRef swigCPtr;

	// Token: 0x04000002 RID: 2
	protected bool swigCMemOwn;
}
