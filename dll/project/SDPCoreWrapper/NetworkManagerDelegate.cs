using System;
using System.Runtime.InteropServices;

// Token: 0x0200004B RID: 75
public class NetworkManagerDelegate : IDisposable
{
	// Token: 0x060004C4 RID: 1220 RVA: 0x0000C9CA File Offset: 0x0000ABCA
	internal NetworkManagerDelegate(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0000C9E6 File Offset: 0x0000ABE6
	internal static HandleRef getCPtr(NetworkManagerDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x0000CA00 File Offset: 0x0000AC00
	~NetworkManagerDelegate()
	{
		this.Dispose();
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0000CA2C File Offset: 0x0000AC2C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_NetworkManagerDelegate(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0000CAAC File Offset: 0x0000ACAC
	public virtual void OnCmdNetServerCreated(uint arg0)
	{
		SDPCorePINVOKE.NetworkManagerDelegate_OnCmdNetServerCreated(this.swigCPtr, arg0);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0000CABA File Offset: 0x0000ACBA
	public virtual void OnCmdNetClientCreated(uint arg0)
	{
		SDPCorePINVOKE.NetworkManagerDelegate_OnCmdNetClientCreated(this.swigCPtr, arg0);
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0000CAC8 File Offset: 0x0000ACC8
	public NetworkManagerDelegate()
		: this(SDPCorePINVOKE.new_NetworkManagerDelegate(), true)
	{
	}

	// Token: 0x0400012C RID: 300
	private HandleRef swigCPtr;

	// Token: 0x0400012D RID: 301
	protected bool swigCMemOwn;
}
