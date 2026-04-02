using System;
using System.Runtime.InteropServices;

// Token: 0x0200004A RID: 74
public class NetworkManager : IDisposable
{
	// Token: 0x060004B7 RID: 1207 RVA: 0x0000C804 File Offset: 0x0000AA04
	internal NetworkManager(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x0000C820 File Offset: 0x0000AA20
	internal static HandleRef getCPtr(NetworkManager obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0000C838 File Offset: 0x0000AA38
	~NetworkManager()
	{
		this.Dispose();
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0000C864 File Offset: 0x0000AA64
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_NetworkManager(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0000C8E4 File Offset: 0x0000AAE4
	public static NetworkManager Get()
	{
		return new NetworkManager(SDPCorePINVOKE.NetworkManager_Get(), false);
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0000C8FE File Offset: 0x0000AAFE
	public void Reset()
	{
		SDPCorePINVOKE.NetworkManager_Reset(this.swigCPtr);
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0000C90B File Offset: 0x0000AB0B
	public void ShutDown()
	{
		SDPCorePINVOKE.NetworkManager_ShutDown(this.swigCPtr);
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0000C918 File Offset: 0x0000AB18
	public void RegisterEventDelegate(NetworkManagerDelegate arg0)
	{
		SDPCorePINVOKE.NetworkManager_RegisterEventDelegate(this.swigCPtr, NetworkManagerDelegate.getCPtr(arg0));
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0000C92B File Offset: 0x0000AB2B
	public void UnregisterEventDelegate(NetworkManagerDelegate arg0)
	{
		SDPCorePINVOKE.NetworkManager_UnregisterEventDelegate(this.swigCPtr, NetworkManagerDelegate.getCPtr(arg0));
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0000C93E File Offset: 0x0000AB3E
	public void AddCmdNetClient(SWIGTYPE_p_SDP__NetCommandClient net)
	{
		SDPCorePINVOKE.NetworkManager_AddCmdNetClient(this.swigCPtr, SWIGTYPE_p_SDP__NetCommandClient.getCPtr(net));
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x0000C951 File Offset: 0x0000AB51
	public void AddCmdNetServer(SWIGTYPE_p_SDP__NetCommandServer net)
	{
		SDPCorePINVOKE.NetworkManager_AddCmdNetServer(this.swigCPtr, SWIGTYPE_p_SDP__NetCommandServer.getCPtr(net));
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0000C964 File Offset: 0x0000AB64
	public SWIGTYPE_p_SDP__NetCommandClient GetCmdNetClient(uint netId)
	{
		IntPtr intPtr = SDPCorePINVOKE.NetworkManager_GetCmdNetClient(this.swigCPtr, netId);
		return (intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_SDP__NetCommandClient(intPtr, false);
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x0000C998 File Offset: 0x0000AB98
	public SWIGTYPE_p_SDP__NetCommandServer GetCmdNetServer()
	{
		IntPtr intPtr = SDPCorePINVOKE.NetworkManager_GetCmdNetServer(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_SDP__NetCommandServer(intPtr, false);
	}

	// Token: 0x0400012A RID: 298
	private HandleRef swigCPtr;

	// Token: 0x0400012B RID: 299
	protected bool swigCMemOwn;
}
