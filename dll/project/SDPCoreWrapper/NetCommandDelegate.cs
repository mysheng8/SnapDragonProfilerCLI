using System;
using System.Runtime.InteropServices;

// Token: 0x02000049 RID: 73
public class NetCommandDelegate : IDisposable
{
	// Token: 0x060004AD RID: 1197 RVA: 0x0000C6BE File Offset: 0x0000A8BE
	internal NetCommandDelegate(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0000C6DA File Offset: 0x0000A8DA
	internal static HandleRef getCPtr(NetCommandDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
	~NetCommandDelegate()
	{
		this.Dispose();
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0000C720 File Offset: 0x0000A920
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_NetCommandDelegate(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0000C7A0 File Offset: 0x0000A9A0
	public virtual void OnConnected()
	{
		SDPCorePINVOKE.NetCommandDelegate_OnConnected(this.swigCPtr);
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0000C7AD File Offset: 0x0000A9AD
	public virtual void OnDisconnected()
	{
		SDPCorePINVOKE.NetCommandDelegate_OnDisconnected(this.swigCPtr);
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x0000C7BA File Offset: 0x0000A9BA
	public virtual void OnClientConnected(uint arg0)
	{
		SDPCorePINVOKE.NetCommandDelegate_OnClientConnected(this.swigCPtr, arg0);
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
	public virtual void OnClientDisconnected(uint arg0)
	{
		SDPCorePINVOKE.NetCommandDelegate_OnClientDisconnected(this.swigCPtr, arg0);
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0000C7D6 File Offset: 0x0000A9D6
	public virtual void OnMessageReceived(SWIGTYPE_p_SDP__Net__Message arg0)
	{
		SDPCorePINVOKE.NetCommandDelegate_OnMessageReceived(this.swigCPtr, SWIGTYPE_p_SDP__Net__Message.getCPtr(arg0));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0000C7F6 File Offset: 0x0000A9F6
	public NetCommandDelegate()
		: this(SDPCorePINVOKE.new_NetCommandDelegate(), true)
	{
	}

	// Token: 0x04000128 RID: 296
	private HandleRef swigCPtr;

	// Token: 0x04000129 RID: 297
	protected bool swigCMemOwn;
}
