using System;
using System.Runtime.InteropServices;

// Token: 0x0200001D RID: 29
public class Client : CoreObject
{
	// Token: 0x060000E6 RID: 230 RVA: 0x00003956 File Offset: 0x00001B56
	internal Client(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.Client_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00003972 File Offset: 0x00001B72
	internal static HandleRef getCPtr(Client obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000398C File Offset: 0x00001B8C
	~Client()
	{
		this.Dispose();
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x000039B8 File Offset: 0x00001BB8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_Client(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00003A3C File Offset: 0x00001C3C
	public Client()
		: this(SDPCorePINVOKE.new_Client(), true)
	{
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00003A4C File Offset: 0x00001C4C
	public bool Init(SessionSettings settings, string service)
	{
		bool flag = SDPCorePINVOKE.Client_Init__SWIG_0(this.swigCPtr, SessionSettings.getCPtr(settings), service);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00003A7C File Offset: 0x00001C7C
	public bool Init(SessionSettings settings)
	{
		bool flag = SDPCorePINVOKE.Client_Init__SWIG_1(this.swigCPtr, SessionSettings.getCPtr(settings));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00003AA9 File Offset: 0x00001CA9
	public void Shutdown()
	{
		SDPCorePINVOKE.Client_Shutdown(this.swigCPtr);
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00003AB8 File Offset: 0x00001CB8
	public bool Update()
	{
		return SDPCorePINVOKE.Client_Update(this.swigCPtr);
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00003AD4 File Offset: 0x00001CD4
	public string GetVersionString()
	{
		return SDPCorePINVOKE.Client_GetVersionString(this.swigCPtr);
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00003AF0 File Offset: 0x00001CF0
	public long GetVersion()
	{
		return SDPCorePINVOKE.Client_GetVersion(this.swigCPtr);
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00003B0C File Offset: 0x00001D0C
	public string GetBuildDate()
	{
		return SDPCorePINVOKE.Client_GetBuildDate(this.swigCPtr);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00003B28 File Offset: 0x00001D28
	public DataModel GetDataModel()
	{
		return new DataModel(SDPCorePINVOKE.Client_GetDataModel(this.swigCPtr), false);
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00003B48 File Offset: 0x00001D48
	public bool ExportAllMetricData(string outFile, Void_UInt_Fn progressFunction)
	{
		bool flag = SDPCorePINVOKE.Client_ExportAllMetricData(this.swigCPtr, outFile, progressFunction);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00003B74 File Offset: 0x00001D74
	public DeviceManager GetDeviceManager()
	{
		return new DeviceManager(SDPCorePINVOKE.Client_GetDeviceManager(this.swigCPtr), false);
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00003B94 File Offset: 0x00001D94
	public CaptureManager GetCaptureManager()
	{
		return new CaptureManager(SDPCorePINVOKE.Client_GetCaptureManager(this.swigCPtr), false);
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00003BB4 File Offset: 0x00001DB4
	public bool PauseRealtimeMetrics(bool paused)
	{
		return SDPCorePINVOKE.Client_PauseRealtimeMetrics__SWIG_0(this.swigCPtr, paused);
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00003BD0 File Offset: 0x00001DD0
	public bool PauseRealtimeMetrics()
	{
		return SDPCorePINVOKE.Client_PauseRealtimeMetrics__SWIG_1(this.swigCPtr);
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00003BEC File Offset: 0x00001DEC
	public ProcessManager GetProcessManager()
	{
		return new ProcessManager(SDPCorePINVOKE.Client_GetProcessManager(this.swigCPtr), false);
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00003C0C File Offset: 0x00001E0C
	public void FetchBuffer(uint provider, uint capture, uint bufferCategory, uint bufferID, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn transferCompleteCallback, IntPtr tag, Void_UInt_UInt_UInt_UInt_UInt_UInt_Fn progressCallback, bool storeInDB, string filename)
	{
		SDPCorePINVOKE.Client_FetchBuffer__SWIG_0(this.swigCPtr, provider, capture, bufferCategory, bufferID, transferCompleteCallback, tag, progressCallback, storeInDB, filename);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00003C40 File Offset: 0x00001E40
	public void FetchBuffer(uint provider, uint capture, uint bufferCategory, uint bufferID, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn transferCompleteCallback, IntPtr tag, Void_UInt_UInt_UInt_UInt_UInt_UInt_Fn progressCallback, bool storeInDB)
	{
		SDPCorePINVOKE.Client_FetchBuffer__SWIG_1(this.swigCPtr, provider, capture, bufferCategory, bufferID, transferCompleteCallback, tag, progressCallback, storeInDB);
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00003C65 File Offset: 0x00001E65
	public void FetchBuffer(uint provider, uint capture, uint bufferCategory, uint bufferID, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn transferCompleteCallback, IntPtr tag, Void_UInt_UInt_UInt_UInt_UInt_UInt_Fn progressCallback)
	{
		SDPCorePINVOKE.Client_FetchBuffer__SWIG_2(this.swigCPtr, provider, capture, bufferCategory, bufferID, transferCompleteCallback, tag, progressCallback);
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00003C7D File Offset: 0x00001E7D
	public void FetchBuffer(uint provider, uint capture, uint bufferCategory, uint bufferID, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn transferCompleteCallback, IntPtr tag)
	{
		SDPCorePINVOKE.Client_FetchBuffer__SWIG_3(this.swigCPtr, provider, capture, bufferCategory, bufferID, transferCompleteCallback, tag);
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00003C93 File Offset: 0x00001E93
	public void FetchBuffer(uint provider, uint capture, uint bufferCategory, uint bufferID, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn transferCompleteCallback)
	{
		SDPCorePINVOKE.Client_FetchBuffer__SWIG_4(this.swigCPtr, provider, capture, bufferCategory, bufferID, transferCompleteCallback);
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00003CA8 File Offset: 0x00001EA8
	public uint GetBufferDataSize(uint capture, uint bufferCategory, uint bufferID)
	{
		return SDPCorePINVOKE.Client_GetBufferDataSize(this.swigCPtr, capture, bufferCategory, bufferID);
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00003CC8 File Offset: 0x00001EC8
	public bool GetBufferData(uint capture, uint bufferCategory, uint bufferID, IntPtr bufferData, uint dataSize)
	{
		return SDPCorePINVOKE.Client_GetBufferData(this.swigCPtr, capture, bufferCategory, bufferID, bufferData, dataSize);
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00003CE9 File Offset: 0x00001EE9
	public void RaisePreDataProcessed(uint captureID, uint bufferCategory, uint bufferID)
	{
		SDPCorePINVOKE.Client_RaisePreDataProcessed(this.swigCPtr, captureID, bufferCategory, bufferID);
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00003CF9 File Offset: 0x00001EF9
	public void RaiseDataProcessed(uint captureID, uint bufferCategory, uint bufferID, string error)
	{
		SDPCorePINVOKE.Client_RaiseDataProcessed__SWIG_0(this.swigCPtr, captureID, bufferCategory, bufferID, error);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00003D18 File Offset: 0x00001F18
	public void RaiseDataProcessed(uint captureID, uint bufferCategory, uint bufferID)
	{
		SDPCorePINVOKE.Client_RaiseDataProcessed__SWIG_1(this.swigCPtr, captureID, bufferCategory, bufferID);
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00003D28 File Offset: 0x00001F28
	public void RaiseBufferTransferProgress(uint providerID, uint bufferCategory, uint bufferID, uint captureID, uint totalBytes, uint bytesReceived)
	{
		SDPCorePINVOKE.Client_RaiseBufferTransferProgress(this.swigCPtr, providerID, bufferCategory, bufferID, captureID, totalBytes, bytesReceived);
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00003D3E File Offset: 0x00001F3E
	public void RaiseMaxCaptureDurationExpired(uint captureID)
	{
		SDPCorePINVOKE.Client_RaiseMaxCaptureDurationExpired(this.swigCPtr, captureID);
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00003D4C File Offset: 0x00001F4C
	public bool PushFile(string localName, string remoteName, bool compress)
	{
		bool flag = SDPCorePINVOKE.Client_PushFile(this.swigCPtr, localName, remoteName, compress);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00003D78 File Offset: 0x00001F78
	public ProviderList GetDataProviders()
	{
		return new ProviderList(SDPCorePINVOKE.Client_GetDataProviders(this.swigCPtr), true);
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00003D98 File Offset: 0x00001F98
	public DeviceList GetDeviceList()
	{
		return new DeviceList(SDPCorePINVOKE.Client_GetDeviceList(this.swigCPtr), true);
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00003DB8 File Offset: 0x00001FB8
	public DataProvider GetProvider(uint providerID)
	{
		IntPtr intPtr = SDPCorePINVOKE.Client_GetProvider(this.swigCPtr, providerID);
		return (intPtr == IntPtr.Zero) ? null : new DataProvider(intPtr, false);
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00003DEC File Offset: 0x00001FEC
	public Option GetOption(string name, uint pid)
	{
		IntPtr intPtr = SDPCorePINVOKE.Client_GetOption__SWIG_0(this.swigCPtr, name, pid);
		Option option = ((intPtr == IntPtr.Zero) ? null : new Option(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return option;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00003E30 File Offset: 0x00002030
	public Option GetOption(uint optionID, uint pid)
	{
		IntPtr intPtr = SDPCorePINVOKE.Client_GetOption__SWIG_1(this.swigCPtr, optionID, pid);
		return (intPtr == IntPtr.Zero) ? null : new Option(intPtr, false);
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00003E64 File Offset: 0x00002064
	public OptionCategory GetOptionCategory(uint optionCategoryID)
	{
		IntPtr intPtr = SDPCorePINVOKE.Client_GetOptionCategory(this.swigCPtr, optionCategoryID);
		return (intPtr == IntPtr.Zero) ? null : new OptionCategory(intPtr, false);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00003E98 File Offset: 0x00002098
	public bool RequestOptionCategories(uint providerID)
	{
		return SDPCorePINVOKE.Client_RequestOptionCategories__SWIG_0(this.swigCPtr, providerID);
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00003EB4 File Offset: 0x000020B4
	public bool RequestOptionCategories()
	{
		return SDPCorePINVOKE.Client_RequestOptionCategories__SWIG_1(this.swigCPtr);
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00003ED0 File Offset: 0x000020D0
	public bool RequestOptions(uint providerID)
	{
		return SDPCorePINVOKE.Client_RequestOptions__SWIG_0(this.swigCPtr, providerID);
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00003EEC File Offset: 0x000020EC
	public bool RequestOptions()
	{
		return SDPCorePINVOKE.Client_RequestOptions__SWIG_1(this.swigCPtr);
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00003F08 File Offset: 0x00002108
	public bool RequestMetricCategories(uint providerID)
	{
		return SDPCorePINVOKE.Client_RequestMetricCategories__SWIG_0(this.swigCPtr, providerID);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00003F24 File Offset: 0x00002124
	public bool RequestMetricCategories()
	{
		return SDPCorePINVOKE.Client_RequestMetricCategories__SWIG_1(this.swigCPtr);
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00003F40 File Offset: 0x00002140
	public bool RequestMetrics(uint providerID)
	{
		return SDPCorePINVOKE.Client_RequestMetrics__SWIG_0(this.swigCPtr, providerID);
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00003F5C File Offset: 0x0000215C
	public bool RequestMetrics()
	{
		return SDPCorePINVOKE.Client_RequestMetrics__SWIG_1(this.swigCPtr);
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00003F76 File Offset: 0x00002176
	public void RegisterEventDelegate(ClientDelegate arg0)
	{
		SDPCorePINVOKE.Client_RegisterEventDelegate(this.swigCPtr, ClientDelegate.getCPtr(arg0));
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00003F89 File Offset: 0x00002189
	public void DeregisterEventDelegate(ClientDelegate arg0)
	{
		SDPCorePINVOKE.Client_DeregisterEventDelegate(this.swigCPtr, ClientDelegate.getCPtr(arg0));
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00003F9C File Offset: 0x0000219C
	public void RegisterOnBufferRegistered(Void_UInt_UInt_UInt_UInt_Fn callback)
	{
		SDPCorePINVOKE.Client_RegisterOnBufferRegistered__SWIG_0(this.swigCPtr, callback);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00003FAA File Offset: 0x000021AA
	public void RegisterOnBufferRegistered(SWIGTYPE_p_std__functionT_void_funsigned_int_const_R_unsigned_int_const_R_unsigned_int_const_R_unsigned_int_const_RF_t callback)
	{
		SDPCorePINVOKE.Client_RegisterOnBufferRegistered__SWIG_1(this.swigCPtr, SWIGTYPE_p_std__functionT_void_funsigned_int_const_R_unsigned_int_const_R_unsigned_int_const_R_unsigned_int_const_RF_t.getCPtr(callback));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00003FCA File Offset: 0x000021CA
	public void RemoveProcessOptions(uint pid)
	{
		SDPCorePINVOKE.Client_RemoveProcessOptions(this.swigCPtr, pid);
	}

	// Token: 0x04000025 RID: 37
	private HandleRef swigCPtr;
}
