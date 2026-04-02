using System;
using System.Runtime.InteropServices;

// Token: 0x02000094 RID: 148
public class SessionModel : IDisposable
{
	// Token: 0x06001342 RID: 4930 RVA: 0x000178C2 File Offset: 0x00015AC2
	internal SessionModel(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x000178DE File Offset: 0x00015ADE
	internal static HandleRef getCPtr(SessionModel obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x000178F8 File Offset: 0x00015AF8
	~SessionModel()
	{
		this.Dispose();
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x00017924 File Offset: 0x00015B24
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_SessionModel(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x000179A4 File Offset: 0x00015BA4
	public SessionModel()
		: this(SDPCorePINVOKE.new_SessionModel(), true)
	{
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06001348 RID: 4936 RVA: 0x000179C0 File Offset: 0x00015BC0
	// (set) Token: 0x06001347 RID: 4935 RVA: 0x000179B2 File Offset: 0x00015BB2
	public uint version
	{
		get
		{
			return SDPCorePINVOKE.SessionModel_version_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.SessionModel_version_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x0600134A RID: 4938 RVA: 0x000179F8 File Offset: 0x00015BF8
	// (set) Token: 0x06001349 RID: 4937 RVA: 0x000179DA File Offset: 0x00015BDA
	public string versionString
	{
		get
		{
			string text = SDPCorePINVOKE.SessionModel_versionString_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.SessionModel_versionString_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x0600134C RID: 4940 RVA: 0x00017A3C File Offset: 0x00015C3C
	// (set) Token: 0x0600134B RID: 4939 RVA: 0x00017A1F File Offset: 0x00015C1F
	public string buildDate
	{
		get
		{
			string text = SDPCorePINVOKE.SessionModel_buildDate_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.SessionModel_buildDate_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x040001C2 RID: 450
	private HandleRef swigCPtr;

	// Token: 0x040001C3 RID: 451
	protected bool swigCMemOwn;
}
