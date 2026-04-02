using System;
using System.Runtime.InteropServices;

// Token: 0x0200002E RID: 46
public class DeviceManagerDelegate : IDisposable
{
	// Token: 0x0600029E RID: 670 RVA: 0x00007D26 File Offset: 0x00005F26
	internal DeviceManagerDelegate(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00007D42 File Offset: 0x00005F42
	internal static HandleRef getCPtr(DeviceManagerDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00007D5C File Offset: 0x00005F5C
	~DeviceManagerDelegate()
	{
		this.Dispose();
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00007D88 File Offset: 0x00005F88
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DeviceManagerDelegate(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00007E08 File Offset: 0x00006008
	public virtual void OnDeviceAdded(string name, string ip)
	{
		SDPCorePINVOKE.DeviceManagerDelegate_OnDeviceAdded(this.swigCPtr, name, ip);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00007E24 File Offset: 0x00006024
	public virtual void OnDeviceRemoved(string name)
	{
		SDPCorePINVOKE.DeviceManagerDelegate_OnDeviceRemoved(this.swigCPtr, name);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0400008A RID: 138
	private HandleRef swigCPtr;

	// Token: 0x0400008B RID: 139
	protected bool swigCMemOwn;
}
