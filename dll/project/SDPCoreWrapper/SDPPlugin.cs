using System;
using System.Runtime.InteropServices;

// Token: 0x0200008F RID: 143
public class SDPPlugin : IDisposable
{
	// Token: 0x0600131A RID: 4890 RVA: 0x00017224 File Offset: 0x00015424
	internal SDPPlugin(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x00017240 File Offset: 0x00015440
	internal static HandleRef getCPtr(SDPPlugin obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x00017258 File Offset: 0x00015458
	~SDPPlugin()
	{
		this.Dispose();
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x00017284 File Offset: 0x00015484
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_SDPPlugin(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x00017304 File Offset: 0x00015504
	public virtual bool Initialize(IntPtr initData)
	{
		return SDPCorePINVOKE.SDPPlugin_Initialize__SWIG_0(this.swigCPtr, initData);
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x00017320 File Offset: 0x00015520
	public virtual bool Initialize()
	{
		return SDPCorePINVOKE.SDPPlugin_Initialize__SWIG_1(this.swigCPtr);
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x0001733C File Offset: 0x0001553C
	public virtual bool Shutdown()
	{
		return SDPCorePINVOKE.SDPPlugin_Shutdown(this.swigCPtr);
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x00017358 File Offset: 0x00015558
	public virtual string GetName()
	{
		return SDPCorePINVOKE.SDPPlugin_GetName(this.swigCPtr);
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x00017374 File Offset: 0x00015574
	public virtual uint GetProviderID()
	{
		return SDPCorePINVOKE.SDPPlugin_GetProviderID(this.swigCPtr);
	}

	// Token: 0x040001B9 RID: 441
	private HandleRef swigCPtr;

	// Token: 0x040001BA RID: 442
	protected bool swigCMemOwn;
}
