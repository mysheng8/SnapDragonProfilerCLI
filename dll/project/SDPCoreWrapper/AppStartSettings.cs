using System;
using System.Runtime.InteropServices;

// Token: 0x0200000F RID: 15
public class AppStartSettings : IDisposable
{
	// Token: 0x06000038 RID: 56 RVA: 0x000021A7 File Offset: 0x000003A7
	internal AppStartSettings(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000021C3 File Offset: 0x000003C3
	internal static HandleRef getCPtr(AppStartSettings obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000021DC File Offset: 0x000003DC
	~AppStartSettings()
	{
		this.Dispose();
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002208 File Offset: 0x00000408
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_AppStartSettings(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600003D RID: 61 RVA: 0x000022A4 File Offset: 0x000004A4
	// (set) Token: 0x0600003C RID: 60 RVA: 0x00002288 File Offset: 0x00000488
	public string executablePath
	{
		get
		{
			string text = SDPCorePINVOKE.AppStartSettings_executablePath_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.AppStartSettings_executablePath_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600003F RID: 63 RVA: 0x000022E8 File Offset: 0x000004E8
	// (set) Token: 0x0600003E RID: 62 RVA: 0x000022CB File Offset: 0x000004CB
	public string workingDir
	{
		get
		{
			string text = SDPCorePINVOKE.AppStartSettings_workingDir_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.AppStartSettings_workingDir_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000041 RID: 65 RVA: 0x0000232C File Offset: 0x0000052C
	// (set) Token: 0x06000040 RID: 64 RVA: 0x0000230F File Offset: 0x0000050F
	public string commandlineArgs
	{
		get
		{
			string text = SDPCorePINVOKE.AppStartSettings_commandlineArgs_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.AppStartSettings_commandlineArgs_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000043 RID: 67 RVA: 0x00002364 File Offset: 0x00000564
	// (set) Token: 0x06000042 RID: 66 RVA: 0x00002353 File Offset: 0x00000553
	public uint renderingAPIs
	{
		get
		{
			return SDPCorePINVOKE.AppStartSettings_renderingAPIs_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.AppStartSettings_renderingAPIs_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000045 RID: 69 RVA: 0x0000238C File Offset: 0x0000058C
	// (set) Token: 0x06000044 RID: 68 RVA: 0x0000237E File Offset: 0x0000057E
	public uint captureType
	{
		get
		{
			return SDPCorePINVOKE.AppStartSettings_captureType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.AppStartSettings_captureType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000047 RID: 71 RVA: 0x000023C4 File Offset: 0x000005C4
	// (set) Token: 0x06000046 RID: 70 RVA: 0x000023A6 File Offset: 0x000005A6
	public string envVars
	{
		get
		{
			string text = SDPCorePINVOKE.AppStartSettings_envVars_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.AppStartSettings_envVars_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000049 RID: 73 RVA: 0x00002408 File Offset: 0x00000608
	// (set) Token: 0x06000048 RID: 72 RVA: 0x000023EB File Offset: 0x000005EB
	public string launchOptions
	{
		get
		{
			string text = SDPCorePINVOKE.AppStartSettings_launchOptions_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.AppStartSettings_launchOptions_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x0000242F File Offset: 0x0000062F
	public AppStartSettings(string executablePath, string workingDir, string commandlineArgs, uint renderingAPIs, uint captureType, string envVars, string launchOptions)
		: this(SDPCorePINVOKE.new_AppStartSettings__SWIG_0(executablePath, workingDir, commandlineArgs, renderingAPIs, captureType, envVars, launchOptions), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00002455 File Offset: 0x00000655
	public AppStartSettings(string packageAndActivity, string intentArgs, uint renderingAPIs, uint captureType, string launchOptions)
		: this(SDPCorePINVOKE.new_AppStartSettings__SWIG_1(packageAndActivity, intentArgs, renderingAPIs, captureType, launchOptions), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000006 RID: 6
	private HandleRef swigCPtr;

	// Token: 0x04000007 RID: 7
	protected bool swigCMemOwn;
}
