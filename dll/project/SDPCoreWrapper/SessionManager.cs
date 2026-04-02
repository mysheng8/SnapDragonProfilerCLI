using System;
using System.Runtime.InteropServices;

// Token: 0x02000092 RID: 146
public class SessionManager : IDisposable
{
	// Token: 0x06001333 RID: 4915 RVA: 0x0001764E File Offset: 0x0001584E
	internal SessionManager(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x0001766A File Offset: 0x0001586A
	internal static HandleRef getCPtr(SessionManager obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x00017684 File Offset: 0x00015884
	~SessionManager()
	{
		this.Dispose();
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x000176B0 File Offset: 0x000158B0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_SessionManager(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x00017730 File Offset: 0x00015930
	public static SessionManager Get()
	{
		return new SessionManager(SDPCorePINVOKE.SessionManager_Get(), false);
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x0001774C File Offset: 0x0001594C
	public bool OpenSession(SessionSettings sesionSetting)
	{
		bool flag = SDPCorePINVOKE.SessionManager_OpenSession(this.swigCPtr, SessionSettings.getCPtr(sesionSetting));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x0001777C File Offset: 0x0001597C
	public bool CloseSession()
	{
		return SDPCorePINVOKE.SessionManager_CloseSession(this.swigCPtr);
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x00017796 File Offset: 0x00015996
	public void RegisterEventDelegate(SessionManagerDelegate arg0)
	{
		SDPCorePINVOKE.SessionManager_RegisterEventDelegate(this.swigCPtr, SessionManagerDelegate.getCPtr(arg0));
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x000177AC File Offset: 0x000159AC
	public string GetSessionPath()
	{
		return SDPCorePINVOKE.SessionManager_GetSessionPath(this.swigCPtr);
	}

	// Token: 0x040001BE RID: 446
	private HandleRef swigCPtr;

	// Token: 0x040001BF RID: 447
	protected bool swigCMemOwn;
}
