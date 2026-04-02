using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x0200002B RID: 43
public class DeviceDelegate : IDisposable
{
	// Token: 0x06000223 RID: 547 RVA: 0x00006C18 File Offset: 0x00004E18
	internal DeviceDelegate(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000224 RID: 548 RVA: 0x00006C34 File Offset: 0x00004E34
	internal static HandleRef getCPtr(DeviceDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000225 RID: 549 RVA: 0x00006C4C File Offset: 0x00004E4C
	~DeviceDelegate()
	{
		this.Dispose();
	}

	// Token: 0x06000226 RID: 550 RVA: 0x00006C78 File Offset: 0x00004E78
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DeviceDelegate(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000227 RID: 551 RVA: 0x00006CF8 File Offset: 0x00004EF8
	public virtual void OnDeviceConnected(string name)
	{
		if (this.SwigDerivedClassHasMethod("OnDeviceConnected", DeviceDelegate.swigMethodTypes0))
		{
			SDPCorePINVOKE.DeviceDelegate_OnDeviceConnectedSwigExplicitDeviceDelegate(this.swigCPtr, name);
		}
		else
		{
			SDPCorePINVOKE.DeviceDelegate_OnDeviceConnected(this.swigCPtr, name);
		}
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000228 RID: 552 RVA: 0x00006D33 File Offset: 0x00004F33
	public virtual void OnDeviceDisconnected(string name)
	{
		if (this.SwigDerivedClassHasMethod("OnDeviceDisconnected", DeviceDelegate.swigMethodTypes1))
		{
			SDPCorePINVOKE.DeviceDelegate_OnDeviceDisconnectedSwigExplicitDeviceDelegate(this.swigCPtr, name);
		}
		else
		{
			SDPCorePINVOKE.DeviceDelegate_OnDeviceDisconnected(this.swigCPtr, name);
		}
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x00006D6E File Offset: 0x00004F6E
	public virtual void OnDeviceStateChanged(string name)
	{
		if (this.SwigDerivedClassHasMethod("OnDeviceStateChanged", DeviceDelegate.swigMethodTypes2))
		{
			SDPCorePINVOKE.DeviceDelegate_OnDeviceStateChangedSwigExplicitDeviceDelegate(this.swigCPtr, name);
		}
		else
		{
			SDPCorePINVOKE.DeviceDelegate_OnDeviceStateChanged(this.swigCPtr, name);
		}
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00006DA9 File Offset: 0x00004FA9
	public DeviceDelegate()
		: this(SDPCorePINVOKE.new_DeviceDelegate(), true)
	{
		this.SwigDirectorConnect();
	}

	// Token: 0x0600022B RID: 555 RVA: 0x00006DC0 File Offset: 0x00004FC0
	private void SwigDirectorConnect()
	{
		if (this.SwigDerivedClassHasMethod("OnDeviceConnected", DeviceDelegate.swigMethodTypes0))
		{
			this.swigDelegate0 = new DeviceDelegate.SwigDelegateDeviceDelegate_0(this.SwigDirectorOnDeviceConnected);
		}
		if (this.SwigDerivedClassHasMethod("OnDeviceDisconnected", DeviceDelegate.swigMethodTypes1))
		{
			this.swigDelegate1 = new DeviceDelegate.SwigDelegateDeviceDelegate_1(this.SwigDirectorOnDeviceDisconnected);
		}
		if (this.SwigDerivedClassHasMethod("OnDeviceStateChanged", DeviceDelegate.swigMethodTypes2))
		{
			this.swigDelegate2 = new DeviceDelegate.SwigDelegateDeviceDelegate_2(this.SwigDirectorOnDeviceStateChanged);
		}
		SDPCorePINVOKE.DeviceDelegate_director_connect(this.swigCPtr, this.swigDelegate0, this.swigDelegate1, this.swigDelegate2);
	}

	// Token: 0x0600022C RID: 556 RVA: 0x00006E58 File Offset: 0x00005058
	private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes)
	{
		MethodInfo method = base.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, methodTypes, null);
		return method.DeclaringType.IsSubclassOf(typeof(DeviceDelegate));
	}

	// Token: 0x0600022D RID: 557 RVA: 0x00006E8E File Offset: 0x0000508E
	private void SwigDirectorOnDeviceConnected(string name)
	{
		this.OnDeviceConnected(name);
	}

	// Token: 0x0600022E RID: 558 RVA: 0x00006E97 File Offset: 0x00005097
	private void SwigDirectorOnDeviceDisconnected(string name)
	{
		this.OnDeviceDisconnected(name);
	}

	// Token: 0x0600022F RID: 559 RVA: 0x00006EA0 File Offset: 0x000050A0
	private void SwigDirectorOnDeviceStateChanged(string name)
	{
		this.OnDeviceStateChanged(name);
	}

	// Token: 0x0400007E RID: 126
	private HandleRef swigCPtr;

	// Token: 0x0400007F RID: 127
	protected bool swigCMemOwn;

	// Token: 0x04000080 RID: 128
	private DeviceDelegate.SwigDelegateDeviceDelegate_0 swigDelegate0;

	// Token: 0x04000081 RID: 129
	private DeviceDelegate.SwigDelegateDeviceDelegate_1 swigDelegate1;

	// Token: 0x04000082 RID: 130
	private DeviceDelegate.SwigDelegateDeviceDelegate_2 swigDelegate2;

	// Token: 0x04000083 RID: 131
	private static Type[] swigMethodTypes0 = new Type[] { typeof(string) };

	// Token: 0x04000084 RID: 132
	private static Type[] swigMethodTypes1 = new Type[] { typeof(string) };

	// Token: 0x04000085 RID: 133
	private static Type[] swigMethodTypes2 = new Type[] { typeof(string) };

	// Token: 0x020000D4 RID: 212
	// (Invoke) Token: 0x0600149E RID: 5278
	public delegate void SwigDelegateDeviceDelegate_0(string name);

	// Token: 0x020000D5 RID: 213
	// (Invoke) Token: 0x060014A2 RID: 5282
	public delegate void SwigDelegateDeviceDelegate_1(string name);

	// Token: 0x020000D6 RID: 214
	// (Invoke) Token: 0x060014A6 RID: 5286
	public delegate void SwigDelegateDeviceDelegate_2(string name);
}
