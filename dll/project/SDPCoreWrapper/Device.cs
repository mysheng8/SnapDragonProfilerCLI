using System;
using System.Runtime.InteropServices;

// Token: 0x02000026 RID: 38
public class Device : IDisposable
{
	// Token: 0x060001D6 RID: 470 RVA: 0x0000622A File Offset: 0x0000442A
	internal Device(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00006246 File Offset: 0x00004446
	internal static HandleRef getCPtr(Device obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00006260 File Offset: 0x00004460
	~Device()
	{
		this.Dispose();
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000628C File Offset: 0x0000448C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_Device(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000630C File Offset: 0x0000450C
	public virtual void Init(string serviceName)
	{
		SDPCorePINVOKE.Device_Init__SWIG_0(this.swigCPtr, serviceName);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00006327 File Offset: 0x00004527
	public virtual void Init()
	{
		SDPCorePINVOKE.Device_Init__SWIG_1(this.swigCPtr);
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00006334 File Offset: 0x00004534
	public virtual bool Shutdown()
	{
		return SDPCorePINVOKE.Device_Shutdown(this.swigCPtr);
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000634E File Offset: 0x0000454E
	public virtual void RetryInstall()
	{
		SDPCorePINVOKE.Device_RetryInstall(this.swigCPtr);
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000635B File Offset: 0x0000455B
	public virtual void Connect(uint timeoutSeconds, uint basePort)
	{
		SDPCorePINVOKE.Device_Connect__SWIG_0(this.swigCPtr, timeoutSeconds, basePort);
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000636A File Offset: 0x0000456A
	public virtual void Connect(uint timeoutSeconds)
	{
		SDPCorePINVOKE.Device_Connect__SWIG_1(this.swigCPtr, timeoutSeconds);
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00006378 File Offset: 0x00004578
	public virtual void Connect()
	{
		SDPCorePINVOKE.Device_Connect__SWIG_2(this.swigCPtr);
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00006385 File Offset: 0x00004585
	public virtual void Disconnect()
	{
		SDPCorePINVOKE.Device_Disconnect(this.swigCPtr);
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x00006392 File Offset: 0x00004592
	public virtual void SetDeviceConnectionType(DeviceConnectionType connectionType)
	{
		SDPCorePINVOKE.Device_SetDeviceConnectionType(this.swigCPtr, (int)connectionType);
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x000063A0 File Offset: 0x000045A0
	public virtual DeviceConnectionState GetDeviceState()
	{
		return (DeviceConnectionState)SDPCorePINVOKE.Device_GetDeviceState(this.swigCPtr);
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x000063BC File Offset: 0x000045BC
	public virtual DeviceConnectionType GetDeviceConnectionType()
	{
		return (DeviceConnectionType)SDPCorePINVOKE.Device_GetDeviceConnectionType(this.swigCPtr);
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x000063D8 File Offset: 0x000045D8
	public virtual string GetDeviceStateMsg()
	{
		return SDPCorePINVOKE.Device_GetDeviceStateMsg(this.swigCPtr);
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x000063F4 File Offset: 0x000045F4
	public virtual SWIGTYPE_p_SDP__NetCommandServer GetCommandNet()
	{
		IntPtr intPtr = SDPCorePINVOKE.Device_GetCommandNet(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_SDP__NetCommandServer(intPtr, false);
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x00006428 File Offset: 0x00004628
	public virtual uint GetCommandNetPort()
	{
		return SDPCorePINVOKE.Device_GetCommandNetPort(this.swigCPtr);
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x00006442 File Offset: 0x00004642
	public virtual void RegisterEventDelegate(DeviceDelegate arg0)
	{
		SDPCorePINVOKE.Device_RegisterEventDelegate(this.swigCPtr, DeviceDelegate.getCPtr(arg0));
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00006455 File Offset: 0x00004655
	public virtual void DeregisterEventDelegate(DeviceDelegate arg0)
	{
		SDPCorePINVOKE.Device_DeregisterEventDelegate(this.swigCPtr, DeviceDelegate.getCPtr(arg0));
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00006468 File Offset: 0x00004668
	public virtual string GetName()
	{
		return SDPCorePINVOKE.Device_GetName(this.swigCPtr);
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00006484 File Offset: 0x00004684
	public virtual string GetIP()
	{
		return SDPCorePINVOKE.Device_GetIP(this.swigCPtr);
	}

	// Token: 0x060001EC RID: 492 RVA: 0x000064A0 File Offset: 0x000046A0
	public virtual SWIGTYPE_p_SDP__DeviceType GetDeviceType()
	{
		IntPtr intPtr = SDPCorePINVOKE.Device_GetDeviceType(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_SDP__DeviceType(intPtr, false);
	}

	// Token: 0x060001ED RID: 493 RVA: 0x000064D4 File Offset: 0x000046D4
	public virtual DeviceAttributes GetDeviceAttributes()
	{
		return new DeviceAttributes(SDPCorePINVOKE.Device_GetDeviceAttributes(this.swigCPtr), false);
	}

	// Token: 0x060001EE RID: 494 RVA: 0x000064F4 File Offset: 0x000046F4
	public virtual bool SuspendAllApps()
	{
		return SDPCorePINVOKE.Device_SuspendAllApps(this.swigCPtr);
	}

	// Token: 0x060001EF RID: 495 RVA: 0x00006510 File Offset: 0x00004710
	public virtual AppStartResponse StartApp(AppStartSettings settings)
	{
		AppStartResponse appStartResponse = new AppStartResponse(SDPCorePINVOKE.Device_StartApp(this.swigCPtr, AppStartSettings.getCPtr(settings)), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return appStartResponse;
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00006544 File Offset: 0x00004744
	public virtual bool StopApp(string appName)
	{
		bool flag = SDPCorePINVOKE.Device_StopApp__SWIG_0(this.swigCPtr, appName);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000656C File Offset: 0x0000476C
	public virtual bool StopApp(uint processid)
	{
		return SDPCorePINVOKE.Device_StopApp__SWIG_1(this.swigCPtr, processid);
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00006588 File Offset: 0x00004788
	public virtual bool SetProperty(DeviceSettings setting, string value)
	{
		bool flag = SDPCorePINVOKE.Device_SetProperty(this.swigCPtr, (int)setting, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x000065B4 File Offset: 0x000047B4
	public virtual string GetProperty(DeviceSettings setting)
	{
		return SDPCorePINVOKE.Device_GetProperty(this.swigCPtr, (int)setting);
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x000065D0 File Offset: 0x000047D0
	public virtual bool EnableAppPermission(string appName, AppPermissions permission, bool enable)
	{
		bool flag = SDPCorePINVOKE.Device_EnableAppPermission(this.swigCPtr, appName, (int)permission, enable);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x04000065 RID: 101
	private HandleRef swigCPtr;

	// Token: 0x04000066 RID: 102
	protected bool swigCMemOwn;
}
