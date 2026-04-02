using System;
using System.Runtime.InteropServices;

// Token: 0x0200002D RID: 45
public class DeviceManager : IDisposable
{
	// Token: 0x0600025D RID: 605 RVA: 0x000074A4 File Offset: 0x000056A4
	internal DeviceManager(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600025E RID: 606 RVA: 0x000074C0 File Offset: 0x000056C0
	internal static HandleRef getCPtr(DeviceManager obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600025F RID: 607 RVA: 0x000074D8 File Offset: 0x000056D8
	~DeviceManager()
	{
		this.Dispose();
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00007504 File Offset: 0x00005704
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DeviceManager(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00007584 File Offset: 0x00005784
	public static DeviceManager Get()
	{
		return new DeviceManager(SDPCorePINVOKE.DeviceManager_Get(), false);
	}

	// Token: 0x06000262 RID: 610 RVA: 0x000075A0 File Offset: 0x000057A0
	public bool Init()
	{
		return SDPCorePINVOKE.DeviceManager_Init(this.swigCPtr);
	}

	// Token: 0x06000263 RID: 611 RVA: 0x000075BC File Offset: 0x000057BC
	public bool IsInitialized()
	{
		return SDPCorePINVOKE.DeviceManager_IsInitialized(this.swigCPtr);
	}

	// Token: 0x06000264 RID: 612 RVA: 0x000075D8 File Offset: 0x000057D8
	public Device AddDevice(string name, string ip)
	{
		IntPtr intPtr = SDPCorePINVOKE.DeviceManager_AddDevice(this.swigCPtr, name, ip);
		Device device = ((intPtr == IntPtr.Zero) ? null : new Device(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return device;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000761C File Offset: 0x0000581C
	public bool RemoveDevice(string deviceName)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_RemoveDevice(this.swigCPtr, deviceName);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00007644 File Offset: 0x00005844
	public void FindDevices()
	{
		SDPCorePINVOKE.DeviceManager_FindDevices(this.swigCPtr);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00007654 File Offset: 0x00005854
	public DeviceList GetDevices()
	{
		return new DeviceList(SDPCorePINVOKE.DeviceManager_GetDevices(this.swigCPtr), false);
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00007674 File Offset: 0x00005874
	public SWIGTYPE_p_Utils__VectorIteratorT_SDP__DeviceList_t GetDeviceIter()
	{
		return new SWIGTYPE_p_Utils__VectorIteratorT_SDP__DeviceList_t(SDPCorePINVOKE.DeviceManager_GetDeviceIter(this.swigCPtr), true);
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00007694 File Offset: 0x00005894
	public Device GetDevice(string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.DeviceManager_GetDevice(this.swigCPtr, name);
		Device device = ((intPtr == IntPtr.Zero) ? null : new Device(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return device;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x000076D4 File Offset: 0x000058D4
	public uint GetNumKnownDevices()
	{
		return SDPCorePINVOKE.DeviceManager_GetNumKnownDevices(this.swigCPtr);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x000076F0 File Offset: 0x000058F0
	public bool IsConnected()
	{
		return SDPCorePINVOKE.DeviceManager_IsConnected(this.swigCPtr);
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000770C File Offset: 0x0000590C
	public Device GetConnectedDevice()
	{
		IntPtr intPtr = SDPCorePINVOKE.DeviceManager_GetConnectedDevice(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new Device(intPtr, false);
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000773E File Offset: 0x0000593E
	public void RegisterEventDelegate(DeviceManagerDelegate arg0)
	{
		SDPCorePINVOKE.DeviceManager_RegisterEventDelegate(this.swigCPtr, DeviceManagerDelegate.getCPtr(arg0));
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00007751 File Offset: 0x00005951
	public void UnregisterEventDelegate(DeviceManagerDelegate arg0)
	{
		SDPCorePINVOKE.DeviceManager_UnregisterEventDelegate(this.swigCPtr, DeviceManagerDelegate.getCPtr(arg0));
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00007764 File Offset: 0x00005964
	public void Shutdown()
	{
		SDPCorePINVOKE.DeviceManager_Shutdown(this.swigCPtr);
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00007771 File Offset: 0x00005971
	public void Reset()
	{
		SDPCorePINVOKE.DeviceManager_Reset(this.swigCPtr);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00007780 File Offset: 0x00005980
	public string GetEnvironment()
	{
		return SDPCorePINVOKE.DeviceManager_GetEnvironment(this.swigCPtr);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000779C File Offset: 0x0000599C
	public string GetQNXIPAddress()
	{
		return SDPCorePINVOKE.DeviceManager_GetQNXIPAddress(this.swigCPtr);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x000077B8 File Offset: 0x000059B8
	public string GetQNXUsername()
	{
		return SDPCorePINVOKE.DeviceManager_GetQNXUsername(this.swigCPtr);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x000077D4 File Offset: 0x000059D4
	public string GetQNXPassword()
	{
		return SDPCorePINVOKE.DeviceManager_GetQNXPassword(this.swigCPtr);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x000077F0 File Offset: 0x000059F0
	public string GetQNXSSHIdentityFile()
	{
		return SDPCorePINVOKE.DeviceManager_GetQNXSSHIdentityFile(this.swigCPtr);
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000780C File Offset: 0x00005A0C
	public string GetQNXConnectionType()
	{
		return SDPCorePINVOKE.DeviceManager_GetQNXConnectionType(this.swigCPtr);
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00007828 File Offset: 0x00005A28
	public string GetQNXDeployDirectory()
	{
		return SDPCorePINVOKE.DeviceManager_GetQNXDeployDirectory(this.swigCPtr);
	}

	// Token: 0x06000278 RID: 632 RVA: 0x00007844 File Offset: 0x00005A44
	public int GetQNXProcessPriority()
	{
		return SDPCorePINVOKE.DeviceManager_GetQNXProcessPriority(this.swigCPtr);
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00007860 File Offset: 0x00005A60
	public string GetAGLIPAddress()
	{
		return SDPCorePINVOKE.DeviceManager_GetAGLIPAddress(this.swigCPtr);
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000787C File Offset: 0x00005A7C
	public string GetAGLUsername()
	{
		return SDPCorePINVOKE.DeviceManager_GetAGLUsername(this.swigCPtr);
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00007898 File Offset: 0x00005A98
	public string GetAGLPassword()
	{
		return SDPCorePINVOKE.DeviceManager_GetAGLPassword(this.swigCPtr);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x000078B4 File Offset: 0x00005AB4
	public string GetAGLDeployDirectory()
	{
		return SDPCorePINVOKE.DeviceManager_GetAGLDeployDirectory(this.swigCPtr);
	}

	// Token: 0x0600027D RID: 637 RVA: 0x000078D0 File Offset: 0x00005AD0
	public string GetWinARMIPAddress()
	{
		return SDPCorePINVOKE.DeviceManager_GetWinARMIPAddress(this.swigCPtr);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x000078EC File Offset: 0x00005AEC
	public int GetInstallerTimeout()
	{
		return SDPCorePINVOKE.DeviceManager_GetInstallerTimeout(this.swigCPtr);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00007908 File Offset: 0x00005B08
	public string GetLinuxSSHIPAddress()
	{
		return SDPCorePINVOKE.DeviceManager_GetLinuxSSHIPAddress(this.swigCPtr);
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00007924 File Offset: 0x00005B24
	public string GetLinuxSSHUsername()
	{
		return SDPCorePINVOKE.DeviceManager_GetLinuxSSHUsername(this.swigCPtr);
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00007940 File Offset: 0x00005B40
	public string GetLinuxSSHPassword()
	{
		return SDPCorePINVOKE.DeviceManager_GetLinuxSSHPassword(this.swigCPtr);
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000795C File Offset: 0x00005B5C
	public string GetLinuxSSHIdentityFile()
	{
		return SDPCorePINVOKE.DeviceManager_GetLinuxSSHIdentityFile(this.swigCPtr);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00007978 File Offset: 0x00005B78
	public string GetLinuxSSHDeployDirectory()
	{
		return SDPCorePINVOKE.DeviceManager_GetLinuxSSHDeployDirectory(this.swigCPtr);
	}

	// Token: 0x06000284 RID: 644 RVA: 0x00007994 File Offset: 0x00005B94
	public string GetSimpleperfPath()
	{
		return SDPCorePINVOKE.DeviceManager_GetSimpleperfPath(this.swigCPtr);
	}

	// Token: 0x06000285 RID: 645 RVA: 0x000079B0 File Offset: 0x00005BB0
	public bool SetQNXIPAddress(string ipaddress)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetQNXIPAddress(this.swigCPtr, ipaddress);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000286 RID: 646 RVA: 0x000079D8 File Offset: 0x00005BD8
	public bool SetQNXUsername(string userName)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetQNXUsername(this.swigCPtr, userName);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000287 RID: 647 RVA: 0x00007A00 File Offset: 0x00005C00
	public bool SetQNXPassword(string passWord)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetQNXPassword(this.swigCPtr, passWord);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00007A28 File Offset: 0x00005C28
	public bool SetQNXSSHIdentityFile(string identityFile)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetQNXSSHIdentityFile(this.swigCPtr, identityFile);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00007A50 File Offset: 0x00005C50
	public bool SetQNXConnectionType(string connectionType)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetQNXConnectionType(this.swigCPtr, connectionType);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00007A78 File Offset: 0x00005C78
	public bool SetQNXDeployDirectory(string directoryPath)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetQNXDeployDirectory(this.swigCPtr, directoryPath);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00007AA0 File Offset: 0x00005CA0
	public bool SetQNXProcessPriority(int priorityInt)
	{
		return SDPCorePINVOKE.DeviceManager_SetQNXProcessPriority(this.swigCPtr, priorityInt);
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00007ABC File Offset: 0x00005CBC
	public bool SetAGLIPAddress(string ipaddress)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetAGLIPAddress(this.swigCPtr, ipaddress);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00007AE4 File Offset: 0x00005CE4
	public bool SetAGLUsername(string userName)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetAGLUsername(this.swigCPtr, userName);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00007B0C File Offset: 0x00005D0C
	public bool SetAGLPassword(string passWord)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetAGLPassword(this.swigCPtr, passWord);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00007B34 File Offset: 0x00005D34
	public bool SetAGLDeployDirectory(string directoryPath)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetAGLDeployDirectory(this.swigCPtr, directoryPath);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00007B5C File Offset: 0x00005D5C
	public bool SetWinARMIPAddress(string ipaddress)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetWinARMIPAddress(this.swigCPtr, ipaddress);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00007B84 File Offset: 0x00005D84
	public bool SetInstallerTimeout(int timeout)
	{
		return SDPCorePINVOKE.DeviceManager_SetInstallerTimeout(this.swigCPtr, timeout);
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00007BA0 File Offset: 0x00005DA0
	public bool SetLinuxSSHIPAddress(string ipaddress)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetLinuxSSHIPAddress(this.swigCPtr, ipaddress);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x00007BC8 File Offset: 0x00005DC8
	public bool SetLinuxSSHUsername(string userName)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetLinuxSSHUsername(this.swigCPtr, userName);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00007BF0 File Offset: 0x00005DF0
	public bool SetLinuxSSHPassword(string passWord)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetLinuxSSHPassword(this.swigCPtr, passWord);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00007C18 File Offset: 0x00005E18
	public bool SetLinuxSSHIdentityFile(string identityFile)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetLinuxSSHIdentityFile(this.swigCPtr, identityFile);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00007C40 File Offset: 0x00005E40
	public bool SetLinuxSSHDeployDirectory(string directoryPath)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetLinuxSSHDeployDirectory(this.swigCPtr, directoryPath);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00007C68 File Offset: 0x00005E68
	public bool SetSimpleperfPath(string path)
	{
		bool flag = SDPCorePINVOKE.DeviceManager_SetSimpleperfPath(this.swigCPtr, path);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00007C90 File Offset: 0x00005E90
	public bool SetSimpleperfAvailable(bool avail)
	{
		return SDPCorePINVOKE.DeviceManager_SetSimpleperfAvailable(this.swigCPtr, avail);
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00007CAC File Offset: 0x00005EAC
	public bool IsSimpleperfAvailable()
	{
		return SDPCorePINVOKE.DeviceManager_IsSimpleperfAvailable(this.swigCPtr);
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00007CC6 File Offset: 0x00005EC6
	public void SetDeleteServiceFilesOnExit(bool deleteFiles)
	{
		SDPCorePINVOKE.DeviceManager_SetDeleteServiceFilesOnExit(this.swigCPtr, deleteFiles);
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00007CD4 File Offset: 0x00005ED4
	public bool GetDeleteServiceFilesOnExit()
	{
		return SDPCorePINVOKE.DeviceManager_GetDeleteServiceFilesOnExit(this.swigCPtr);
	}

	// Token: 0x0600029C RID: 668 RVA: 0x00007CF0 File Offset: 0x00005EF0
	public bool SetHLMIsEnabled(bool enabled)
	{
		return SDPCorePINVOKE.DeviceManager_SetHLMIsEnabled(this.swigCPtr, enabled);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00007D0C File Offset: 0x00005F0C
	public bool GetHLMIsEnabled()
	{
		return SDPCorePINVOKE.DeviceManager_GetHLMIsEnabled(this.swigCPtr);
	}

	// Token: 0x04000088 RID: 136
	private HandleRef swigCPtr;

	// Token: 0x04000089 RID: 137
	protected bool swigCMemOwn;
}
