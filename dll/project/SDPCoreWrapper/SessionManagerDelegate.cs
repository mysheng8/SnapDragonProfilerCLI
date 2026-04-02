using System;
using System.Runtime.InteropServices;

// Token: 0x02000093 RID: 147
public class SessionManagerDelegate : IDisposable
{
	// Token: 0x0600133C RID: 4924 RVA: 0x000177C6 File Offset: 0x000159C6
	internal SessionManagerDelegate(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x000177E2 File Offset: 0x000159E2
	internal static HandleRef getCPtr(SessionManagerDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x000177FC File Offset: 0x000159FC
	~SessionManagerDelegate()
	{
		this.Dispose();
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x00017828 File Offset: 0x00015A28
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_SessionManagerDelegate(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x000178A8 File Offset: 0x00015AA8
	public virtual void OnSessionOpened()
	{
		SDPCorePINVOKE.SessionManagerDelegate_OnSessionOpened(this.swigCPtr);
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x000178B5 File Offset: 0x00015AB5
	public virtual void OnSessionClosed()
	{
		SDPCorePINVOKE.SessionManagerDelegate_OnSessionClosed(this.swigCPtr);
	}

	// Token: 0x040001C0 RID: 448
	private HandleRef swigCPtr;

	// Token: 0x040001C1 RID: 449
	protected bool swigCMemOwn;
}
