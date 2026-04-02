using System;
using System.Runtime.InteropServices;

// Token: 0x02000020 RID: 32
public class CommonObject : IDisposable
{
	// Token: 0x06000157 RID: 343 RVA: 0x00004EFA File Offset: 0x000030FA
	internal CommonObject(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00004F16 File Offset: 0x00003116
	internal static HandleRef getCPtr(CommonObject obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00004F30 File Offset: 0x00003130
	~CommonObject()
	{
		this.Dispose();
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00004F5C File Offset: 0x0000315C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CommonObject(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600015B RID: 347 RVA: 0x00004FDC File Offset: 0x000031DC
	public uint GetID()
	{
		return SDPCorePINVOKE.CommonObject_GetID(this.swigCPtr);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00004FF6 File Offset: 0x000031F6
	public void SetName(string name)
	{
		SDPCorePINVOKE.CommonObject_SetName(this.swigCPtr, name);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00005014 File Offset: 0x00003214
	public string GetName()
	{
		return SDPCorePINVOKE.CommonObject_GetName(this.swigCPtr);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000502E File Offset: 0x0000322E
	public void SetDescription(string description)
	{
		SDPCorePINVOKE.CommonObject_SetDescription(this.swigCPtr, description);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000504C File Offset: 0x0000324C
	public string GetDescription()
	{
		return SDPCorePINVOKE.CommonObject_GetDescription(this.swigCPtr);
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00005066 File Offset: 0x00003266
	public virtual void SetAvailable(bool available)
	{
		SDPCorePINVOKE.CommonObject_SetAvailable__SWIG_0(this.swigCPtr, available);
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00005074 File Offset: 0x00003274
	public virtual void SetAvailable()
	{
		SDPCorePINVOKE.CommonObject_SetAvailable__SWIG_1(this.swigCPtr);
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00005084 File Offset: 0x00003284
	public virtual bool IsAvailable()
	{
		return SDPCorePINVOKE.CommonObject_IsAvailable(this.swigCPtr);
	}

	// Token: 0x06000163 RID: 355 RVA: 0x000050A0 File Offset: 0x000032A0
	public bool IsEnabled()
	{
		return SDPCorePINVOKE.CommonObject_IsEnabled(this.swigCPtr);
	}

	// Token: 0x06000164 RID: 356 RVA: 0x000050BC File Offset: 0x000032BC
	public virtual bool Enable(bool arg0)
	{
		return SDPCorePINVOKE.CommonObject_Enable__SWIG_0(this.swigCPtr, arg0);
	}

	// Token: 0x06000165 RID: 357 RVA: 0x000050D8 File Offset: 0x000032D8
	public virtual bool Enable()
	{
		return SDPCorePINVOKE.CommonObject_Enable__SWIG_1(this.swigCPtr);
	}

	// Token: 0x06000166 RID: 358 RVA: 0x000050F4 File Offset: 0x000032F4
	public virtual bool Disable(bool arg0)
	{
		return SDPCorePINVOKE.CommonObject_Disable__SWIG_0(this.swigCPtr, arg0);
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00005110 File Offset: 0x00003310
	public virtual bool Disable()
	{
		return SDPCorePINVOKE.CommonObject_Disable__SWIG_1(this.swigCPtr);
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000512C File Offset: 0x0000332C
	public virtual bool Publish(SWIGTYPE_p_SDP__NetCommandClient network)
	{
		return SDPCorePINVOKE.CommonObject_Publish(this.swigCPtr, SWIGTYPE_p_SDP__NetCommandClient.getCPtr(network));
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000514C File Offset: 0x0000334C
	public virtual bool PublishStatus()
	{
		return SDPCorePINVOKE.CommonObject_PublishStatus(this.swigCPtr);
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00005168 File Offset: 0x00003368
	public long GetSystemTimeUS()
	{
		return SDPCorePINVOKE.CommonObject_GetSystemTimeUS(this.swigCPtr);
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00005184 File Offset: 0x00003384
	public long GetSystemTimeMS()
	{
		return SDPCorePINVOKE.CommonObject_GetSystemTimeMS(this.swigCPtr);
	}

	// Token: 0x0400005A RID: 90
	private HandleRef swigCPtr;

	// Token: 0x0400005B RID: 91
	protected bool swigCMemOwn;
}
