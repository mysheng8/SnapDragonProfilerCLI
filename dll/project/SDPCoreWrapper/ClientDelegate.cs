using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x0200001E RID: 30
public class ClientDelegate : IDisposable
{
	// Token: 0x06000119 RID: 281 RVA: 0x00003FD8 File Offset: 0x000021D8
	internal ClientDelegate(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00003FF4 File Offset: 0x000021F4
	internal static HandleRef getCPtr(ClientDelegate obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0000400C File Offset: 0x0000220C
	~ClientDelegate()
	{
		this.Dispose();
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00004038 File Offset: 0x00002238
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ClientDelegate(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600011D RID: 285 RVA: 0x000040B8 File Offset: 0x000022B8
	public virtual void OnClientConnected()
	{
		if (this.SwigDerivedClassHasMethod("OnClientConnected", ClientDelegate.swigMethodTypes0))
		{
			SDPCorePINVOKE.ClientDelegate_OnClientConnectedSwigExplicitClientDelegate(this.swigCPtr);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnClientConnected(this.swigCPtr);
	}

	// Token: 0x0600011E RID: 286 RVA: 0x000040E3 File Offset: 0x000022E3
	public virtual void OnClientDisconnected()
	{
		if (this.SwigDerivedClassHasMethod("OnClientDisconnected", ClientDelegate.swigMethodTypes1))
		{
			SDPCorePINVOKE.ClientDelegate_OnClientDisconnectedSwigExplicitClientDelegate(this.swigCPtr);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnClientDisconnected(this.swigCPtr);
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000410E File Offset: 0x0000230E
	public virtual void OnCaptureComplete(uint providerID, uint captureID)
	{
		if (this.SwigDerivedClassHasMethod("OnCaptureComplete", ClientDelegate.swigMethodTypes2))
		{
			SDPCorePINVOKE.ClientDelegate_OnCaptureCompleteSwigExplicitClientDelegate(this.swigCPtr, providerID, captureID);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnCaptureComplete(this.swigCPtr, providerID, captureID);
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0000413D File Offset: 0x0000233D
	public virtual void OnProviderListReceived()
	{
		if (this.SwigDerivedClassHasMethod("OnProviderListReceived", ClientDelegate.swigMethodTypes3))
		{
			SDPCorePINVOKE.ClientDelegate_OnProviderListReceivedSwigExplicitClientDelegate(this.swigCPtr);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnProviderListReceived(this.swigCPtr);
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00004168 File Offset: 0x00002368
	public virtual void OnProcessStateChanged(uint pid)
	{
		if (this.SwigDerivedClassHasMethod("OnProcessStateChanged", ClientDelegate.swigMethodTypes4))
		{
			SDPCorePINVOKE.ClientDelegate_OnProcessStateChangedSwigExplicitClientDelegate(this.swigCPtr, pid);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnProcessStateChanged(this.swigCPtr, pid);
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00004195 File Offset: 0x00002395
	public virtual void OnProcessAdded(uint pid)
	{
		if (this.SwigDerivedClassHasMethod("OnProcessAdded", ClientDelegate.swigMethodTypes5))
		{
			SDPCorePINVOKE.ClientDelegate_OnProcessAddedSwigExplicitClientDelegate(this.swigCPtr, pid);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnProcessAdded(this.swigCPtr, pid);
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000041C2 File Offset: 0x000023C2
	public virtual void OnProcessRemoved(uint pid)
	{
		if (this.SwigDerivedClassHasMethod("OnProcessRemoved", ClientDelegate.swigMethodTypes6))
		{
			SDPCorePINVOKE.ClientDelegate_OnProcessRemovedSwigExplicitClientDelegate(this.swigCPtr, pid);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnProcessRemoved(this.swigCPtr, pid);
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000041EF File Offset: 0x000023EF
	public virtual void OnProcessMetricLinked(uint pid, uint mid)
	{
		if (this.SwigDerivedClassHasMethod("OnProcessMetricLinked", ClientDelegate.swigMethodTypes7))
		{
			SDPCorePINVOKE.ClientDelegate_OnProcessMetricLinkedSwigExplicitClientDelegate(this.swigCPtr, pid, mid);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnProcessMetricLinked(this.swigCPtr, pid, mid);
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000421E File Offset: 0x0000241E
	public virtual void OnThreadListReceived()
	{
		if (this.SwigDerivedClassHasMethod("OnThreadListReceived", ClientDelegate.swigMethodTypes8))
		{
			SDPCorePINVOKE.ClientDelegate_OnThreadListReceivedSwigExplicitClientDelegate(this.swigCPtr);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnThreadListReceived(this.swigCPtr);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00004249 File Offset: 0x00002449
	public virtual void OnMetricListReceived(uint arg0)
	{
		if (this.SwigDerivedClassHasMethod("OnMetricListReceived", ClientDelegate.swigMethodTypes9))
		{
			SDPCorePINVOKE.ClientDelegate_OnMetricListReceivedSwigExplicitClientDelegate(this.swigCPtr, arg0);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnMetricListReceived(this.swigCPtr, arg0);
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00004276 File Offset: 0x00002476
	public virtual void OnProviderConnected(uint arg0)
	{
		if (this.SwigDerivedClassHasMethod("OnProviderConnected", ClientDelegate.swigMethodTypes10))
		{
			SDPCorePINVOKE.ClientDelegate_OnProviderConnectedSwigExplicitClientDelegate(this.swigCPtr, arg0);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnProviderConnected(this.swigCPtr, arg0);
	}

	// Token: 0x06000128 RID: 296 RVA: 0x000042A4 File Offset: 0x000024A4
	public virtual void OnProviderDisconnected(Client arg0, DataProvider arg1)
	{
		if (this.SwigDerivedClassHasMethod("OnProviderDisconnected", ClientDelegate.swigMethodTypes11))
		{
			SDPCorePINVOKE.ClientDelegate_OnProviderDisconnectedSwigExplicitClientDelegate(this.swigCPtr, Client.getCPtr(arg0), DataProvider.getCPtr(arg1));
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnProviderDisconnected(this.swigCPtr, Client.getCPtr(arg0), DataProvider.getCPtr(arg1));
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000042F2 File Offset: 0x000024F2
	public virtual void OnDeviceListUpdated()
	{
		if (this.SwigDerivedClassHasMethod("OnDeviceListUpdated", ClientDelegate.swigMethodTypes12))
		{
			SDPCorePINVOKE.ClientDelegate_OnDeviceListUpdatedSwigExplicitClientDelegate(this.swigCPtr);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnDeviceListUpdated(this.swigCPtr);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0000431D File Offset: 0x0000251D
	public virtual void OnBufferRegistered(uint providerID, uint captureID, uint bufferCategory, uint bufferID)
	{
		if (this.SwigDerivedClassHasMethod("OnBufferRegistered", ClientDelegate.swigMethodTypes13))
		{
			SDPCorePINVOKE.ClientDelegate_OnBufferRegisteredSwigExplicitClientDelegate(this.swigCPtr, providerID, captureID, bufferCategory, bufferID);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnBufferRegistered(this.swigCPtr, providerID, captureID, bufferCategory, bufferID);
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00004352 File Offset: 0x00002552
	public virtual void OnOptionAdded(uint providerID, uint optionID, uint processID)
	{
		if (this.SwigDerivedClassHasMethod("OnOptionAdded", ClientDelegate.swigMethodTypes14))
		{
			SDPCorePINVOKE.ClientDelegate_OnOptionAddedSwigExplicitClientDelegate(this.swigCPtr, providerID, optionID, processID);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnOptionAdded(this.swigCPtr, providerID, optionID, processID);
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00004383 File Offset: 0x00002583
	public virtual void OnOptionCategoryAdded(uint providerID, uint categoryID)
	{
		if (this.SwigDerivedClassHasMethod("OnOptionCategoryAdded", ClientDelegate.swigMethodTypes15))
		{
			SDPCorePINVOKE.ClientDelegate_OnOptionCategoryAddedSwigExplicitClientDelegate(this.swigCPtr, providerID, categoryID);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnOptionCategoryAdded(this.swigCPtr, providerID, categoryID);
	}

	// Token: 0x0600012D RID: 301 RVA: 0x000043B2 File Offset: 0x000025B2
	public virtual void OnMetricAdded(uint providerID, uint metricID)
	{
		if (this.SwigDerivedClassHasMethod("OnMetricAdded", ClientDelegate.swigMethodTypes16))
		{
			SDPCorePINVOKE.ClientDelegate_OnMetricAddedSwigExplicitClientDelegate(this.swigCPtr, providerID, metricID);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnMetricAdded(this.swigCPtr, providerID, metricID);
	}

	// Token: 0x0600012E RID: 302 RVA: 0x000043E1 File Offset: 0x000025E1
	public virtual void OnMetricCategoryAdded(uint providerID, uint metricCategoryID)
	{
		if (this.SwigDerivedClassHasMethod("OnMetricCategoryAdded", ClientDelegate.swigMethodTypes17))
		{
			SDPCorePINVOKE.ClientDelegate_OnMetricCategoryAddedSwigExplicitClientDelegate(this.swigCPtr, providerID, metricCategoryID);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnMetricCategoryAdded(this.swigCPtr, providerID, metricCategoryID);
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00004410 File Offset: 0x00002610
	public virtual void OnPreDataProcessed(uint captureID, uint bufferCategory, uint bufferID)
	{
		if (this.SwigDerivedClassHasMethod("OnPreDataProcessed", ClientDelegate.swigMethodTypes18))
		{
			SDPCorePINVOKE.ClientDelegate_OnPreDataProcessedSwigExplicitClientDelegate(this.swigCPtr, captureID, bufferCategory, bufferID);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnPreDataProcessed(this.swigCPtr, captureID, bufferCategory, bufferID);
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00004444 File Offset: 0x00002644
	public virtual void OnDataProcessed(uint captureID, uint bufferCategory, uint bufferID, string error)
	{
		if (this.SwigDerivedClassHasMethod("OnDataProcessed", ClientDelegate.swigMethodTypes19))
		{
			SDPCorePINVOKE.ClientDelegate_OnDataProcessedSwigExplicitClientDelegate__SWIG_0(this.swigCPtr, captureID, bufferCategory, bufferID, error);
		}
		else
		{
			SDPCorePINVOKE.ClientDelegate_OnDataProcessed__SWIG_0(this.swigCPtr, captureID, bufferCategory, bufferID, error);
		}
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00004492 File Offset: 0x00002692
	public virtual void OnDataProcessed(uint captureID, uint bufferCategory, uint bufferID)
	{
		if (this.SwigDerivedClassHasMethod("OnDataProcessed", ClientDelegate.swigMethodTypes20))
		{
			SDPCorePINVOKE.ClientDelegate_OnDataProcessedSwigExplicitClientDelegate__SWIG_1(this.swigCPtr, captureID, bufferCategory, bufferID);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnDataProcessed__SWIG_1(this.swigCPtr, captureID, bufferCategory, bufferID);
	}

	// Token: 0x06000132 RID: 306 RVA: 0x000044C3 File Offset: 0x000026C3
	public virtual void OnBufferTransferProgress(uint providerID, uint captureID, uint bufferCategory, uint bufferID, uint totalBytes, uint bytesReceived)
	{
		if (this.SwigDerivedClassHasMethod("OnBufferTransferProgress", ClientDelegate.swigMethodTypes21))
		{
			SDPCorePINVOKE.ClientDelegate_OnBufferTransferProgressSwigExplicitClientDelegate(this.swigCPtr, providerID, captureID, bufferCategory, bufferID, totalBytes, bytesReceived);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnBufferTransferProgress(this.swigCPtr, providerID, captureID, bufferCategory, bufferID, totalBytes, bytesReceived);
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00004500 File Offset: 0x00002700
	public virtual void OnDeviceMemoryLow()
	{
		if (this.SwigDerivedClassHasMethod("OnDeviceMemoryLow", ClientDelegate.swigMethodTypes22))
		{
			SDPCorePINVOKE.ClientDelegate_OnDeviceMemoryLowSwigExplicitClientDelegate(this.swigCPtr);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnDeviceMemoryLow(this.swigCPtr);
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0000452B File Offset: 0x0000272B
	public virtual void OnMaxCaptureDurationExpired(uint captureID)
	{
		if (this.SwigDerivedClassHasMethod("OnMaxCaptureDurationExpired", ClientDelegate.swigMethodTypes23))
		{
			SDPCorePINVOKE.ClientDelegate_OnMaxCaptureDurationExpiredSwigExplicitClientDelegate(this.swigCPtr, captureID);
			return;
		}
		SDPCorePINVOKE.ClientDelegate_OnMaxCaptureDurationExpired(this.swigCPtr, captureID);
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00004558 File Offset: 0x00002758
	public ClientDelegate()
		: this(SDPCorePINVOKE.new_ClientDelegate(), true)
	{
		this.SwigDirectorConnect();
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000456C File Offset: 0x0000276C
	private void SwigDirectorConnect()
	{
		if (this.SwigDerivedClassHasMethod("OnClientConnected", ClientDelegate.swigMethodTypes0))
		{
			this.swigDelegate0 = new ClientDelegate.SwigDelegateClientDelegate_0(this.SwigDirectorOnClientConnected);
		}
		if (this.SwigDerivedClassHasMethod("OnClientDisconnected", ClientDelegate.swigMethodTypes1))
		{
			this.swigDelegate1 = new ClientDelegate.SwigDelegateClientDelegate_1(this.SwigDirectorOnClientDisconnected);
		}
		if (this.SwigDerivedClassHasMethod("OnCaptureComplete", ClientDelegate.swigMethodTypes2))
		{
			this.swigDelegate2 = new ClientDelegate.SwigDelegateClientDelegate_2(this.SwigDirectorOnCaptureComplete);
		}
		if (this.SwigDerivedClassHasMethod("OnProviderListReceived", ClientDelegate.swigMethodTypes3))
		{
			this.swigDelegate3 = new ClientDelegate.SwigDelegateClientDelegate_3(this.SwigDirectorOnProviderListReceived);
		}
		if (this.SwigDerivedClassHasMethod("OnProcessStateChanged", ClientDelegate.swigMethodTypes4))
		{
			this.swigDelegate4 = new ClientDelegate.SwigDelegateClientDelegate_4(this.SwigDirectorOnProcessStateChanged);
		}
		if (this.SwigDerivedClassHasMethod("OnProcessAdded", ClientDelegate.swigMethodTypes5))
		{
			this.swigDelegate5 = new ClientDelegate.SwigDelegateClientDelegate_5(this.SwigDirectorOnProcessAdded);
		}
		if (this.SwigDerivedClassHasMethod("OnProcessRemoved", ClientDelegate.swigMethodTypes6))
		{
			this.swigDelegate6 = new ClientDelegate.SwigDelegateClientDelegate_6(this.SwigDirectorOnProcessRemoved);
		}
		if (this.SwigDerivedClassHasMethod("OnProcessMetricLinked", ClientDelegate.swigMethodTypes7))
		{
			this.swigDelegate7 = new ClientDelegate.SwigDelegateClientDelegate_7(this.SwigDirectorOnProcessMetricLinked);
		}
		if (this.SwigDerivedClassHasMethod("OnThreadListReceived", ClientDelegate.swigMethodTypes8))
		{
			this.swigDelegate8 = new ClientDelegate.SwigDelegateClientDelegate_8(this.SwigDirectorOnThreadListReceived);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricListReceived", ClientDelegate.swigMethodTypes9))
		{
			this.swigDelegate9 = new ClientDelegate.SwigDelegateClientDelegate_9(this.SwigDirectorOnMetricListReceived);
		}
		if (this.SwigDerivedClassHasMethod("OnProviderConnected", ClientDelegate.swigMethodTypes10))
		{
			this.swigDelegate10 = new ClientDelegate.SwigDelegateClientDelegate_10(this.SwigDirectorOnProviderConnected);
		}
		if (this.SwigDerivedClassHasMethod("OnProviderDisconnected", ClientDelegate.swigMethodTypes11))
		{
			this.swigDelegate11 = new ClientDelegate.SwigDelegateClientDelegate_11(this.SwigDirectorOnProviderDisconnected);
		}
		if (this.SwigDerivedClassHasMethod("OnDeviceListUpdated", ClientDelegate.swigMethodTypes12))
		{
			this.swigDelegate12 = new ClientDelegate.SwigDelegateClientDelegate_12(this.SwigDirectorOnDeviceListUpdated);
		}
		if (this.SwigDerivedClassHasMethod("OnBufferRegistered", ClientDelegate.swigMethodTypes13))
		{
			this.swigDelegate13 = new ClientDelegate.SwigDelegateClientDelegate_13(this.SwigDirectorOnBufferRegistered);
		}
		if (this.SwigDerivedClassHasMethod("OnOptionAdded", ClientDelegate.swigMethodTypes14))
		{
			this.swigDelegate14 = new ClientDelegate.SwigDelegateClientDelegate_14(this.SwigDirectorOnOptionAdded);
		}
		if (this.SwigDerivedClassHasMethod("OnOptionCategoryAdded", ClientDelegate.swigMethodTypes15))
		{
			this.swigDelegate15 = new ClientDelegate.SwigDelegateClientDelegate_15(this.SwigDirectorOnOptionCategoryAdded);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricAdded", ClientDelegate.swigMethodTypes16))
		{
			this.swigDelegate16 = new ClientDelegate.SwigDelegateClientDelegate_16(this.SwigDirectorOnMetricAdded);
		}
		if (this.SwigDerivedClassHasMethod("OnMetricCategoryAdded", ClientDelegate.swigMethodTypes17))
		{
			this.swigDelegate17 = new ClientDelegate.SwigDelegateClientDelegate_17(this.SwigDirectorOnMetricCategoryAdded);
		}
		if (this.SwigDerivedClassHasMethod("OnPreDataProcessed", ClientDelegate.swigMethodTypes18))
		{
			this.swigDelegate18 = new ClientDelegate.SwigDelegateClientDelegate_18(this.SwigDirectorOnPreDataProcessed);
		}
		if (this.SwigDerivedClassHasMethod("OnDataProcessed", ClientDelegate.swigMethodTypes19))
		{
			this.swigDelegate19 = new ClientDelegate.SwigDelegateClientDelegate_19(this.SwigDirectorOnDataProcessed__SWIG_0);
		}
		if (this.SwigDerivedClassHasMethod("OnDataProcessed", ClientDelegate.swigMethodTypes20))
		{
			this.swigDelegate20 = new ClientDelegate.SwigDelegateClientDelegate_20(this.SwigDirectorOnDataProcessed__SWIG_1);
		}
		if (this.SwigDerivedClassHasMethod("OnBufferTransferProgress", ClientDelegate.swigMethodTypes21))
		{
			this.swigDelegate21 = new ClientDelegate.SwigDelegateClientDelegate_21(this.SwigDirectorOnBufferTransferProgress);
		}
		if (this.SwigDerivedClassHasMethod("OnDeviceMemoryLow", ClientDelegate.swigMethodTypes22))
		{
			this.swigDelegate22 = new ClientDelegate.SwigDelegateClientDelegate_22(this.SwigDirectorOnDeviceMemoryLow);
		}
		if (this.SwigDerivedClassHasMethod("OnMaxCaptureDurationExpired", ClientDelegate.swigMethodTypes23))
		{
			this.swigDelegate23 = new ClientDelegate.SwigDelegateClientDelegate_23(this.SwigDirectorOnMaxCaptureDurationExpired);
		}
		SDPCorePINVOKE.ClientDelegate_director_connect(this.swigCPtr, this.swigDelegate0, this.swigDelegate1, this.swigDelegate2, this.swigDelegate3, this.swigDelegate4, this.swigDelegate5, this.swigDelegate6, this.swigDelegate7, this.swigDelegate8, this.swigDelegate9, this.swigDelegate10, this.swigDelegate11, this.swigDelegate12, this.swigDelegate13, this.swigDelegate14, this.swigDelegate15, this.swigDelegate16, this.swigDelegate17, this.swigDelegate18, this.swigDelegate19, this.swigDelegate20, this.swigDelegate21, this.swigDelegate22, this.swigDelegate23);
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00004974 File Offset: 0x00002B74
	private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes)
	{
		MethodInfo method = base.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, methodTypes, null);
		return method.DeclaringType.IsSubclassOf(typeof(ClientDelegate));
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000049AA File Offset: 0x00002BAA
	private void SwigDirectorOnClientConnected()
	{
		this.OnClientConnected();
	}

	// Token: 0x06000139 RID: 313 RVA: 0x000049B2 File Offset: 0x00002BB2
	private void SwigDirectorOnClientDisconnected()
	{
		this.OnClientDisconnected();
	}

	// Token: 0x0600013A RID: 314 RVA: 0x000049BA File Offset: 0x00002BBA
	private void SwigDirectorOnCaptureComplete(uint providerID, uint captureID)
	{
		this.OnCaptureComplete(providerID, captureID);
	}

	// Token: 0x0600013B RID: 315 RVA: 0x000049C4 File Offset: 0x00002BC4
	private void SwigDirectorOnProviderListReceived()
	{
		this.OnProviderListReceived();
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000049CC File Offset: 0x00002BCC
	private void SwigDirectorOnProcessStateChanged(uint pid)
	{
		this.OnProcessStateChanged(pid);
	}

	// Token: 0x0600013D RID: 317 RVA: 0x000049D5 File Offset: 0x00002BD5
	private void SwigDirectorOnProcessAdded(uint pid)
	{
		this.OnProcessAdded(pid);
	}

	// Token: 0x0600013E RID: 318 RVA: 0x000049DE File Offset: 0x00002BDE
	private void SwigDirectorOnProcessRemoved(uint pid)
	{
		this.OnProcessRemoved(pid);
	}

	// Token: 0x0600013F RID: 319 RVA: 0x000049E7 File Offset: 0x00002BE7
	private void SwigDirectorOnProcessMetricLinked(uint pid, uint mid)
	{
		this.OnProcessMetricLinked(pid, mid);
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000049F1 File Offset: 0x00002BF1
	private void SwigDirectorOnThreadListReceived()
	{
		this.OnThreadListReceived();
	}

	// Token: 0x06000141 RID: 321 RVA: 0x000049F9 File Offset: 0x00002BF9
	private void SwigDirectorOnMetricListReceived(uint arg0)
	{
		this.OnMetricListReceived(arg0);
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00004A02 File Offset: 0x00002C02
	private void SwigDirectorOnProviderConnected(uint arg0)
	{
		this.OnProviderConnected(arg0);
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00004A0B File Offset: 0x00002C0B
	private void SwigDirectorOnProviderDisconnected(IntPtr arg0, IntPtr arg1)
	{
		this.OnProviderDisconnected((arg0 == IntPtr.Zero) ? null : new Client(arg0, false), (arg1 == IntPtr.Zero) ? null : new DataProvider(arg1, false));
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00004A41 File Offset: 0x00002C41
	private void SwigDirectorOnDeviceListUpdated()
	{
		this.OnDeviceListUpdated();
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00004A49 File Offset: 0x00002C49
	private void SwigDirectorOnBufferRegistered(uint providerID, uint captureID, uint bufferCategory, uint bufferID)
	{
		this.OnBufferRegistered(providerID, captureID, bufferCategory, bufferID);
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00004A56 File Offset: 0x00002C56
	private void SwigDirectorOnOptionAdded(uint providerID, uint optionID, uint processID)
	{
		this.OnOptionAdded(providerID, optionID, processID);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00004A61 File Offset: 0x00002C61
	private void SwigDirectorOnOptionCategoryAdded(uint providerID, uint categoryID)
	{
		this.OnOptionCategoryAdded(providerID, categoryID);
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00004A6B File Offset: 0x00002C6B
	private void SwigDirectorOnMetricAdded(uint providerID, uint metricID)
	{
		this.OnMetricAdded(providerID, metricID);
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00004A75 File Offset: 0x00002C75
	private void SwigDirectorOnMetricCategoryAdded(uint providerID, uint metricCategoryID)
	{
		this.OnMetricCategoryAdded(providerID, metricCategoryID);
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00004A7F File Offset: 0x00002C7F
	private void SwigDirectorOnPreDataProcessed(uint captureID, uint bufferCategory, uint bufferID)
	{
		this.OnPreDataProcessed(captureID, bufferCategory, bufferID);
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00004A8A File Offset: 0x00002C8A
	private void SwigDirectorOnDataProcessed__SWIG_0(uint captureID, uint bufferCategory, uint bufferID, string error)
	{
		this.OnDataProcessed(captureID, bufferCategory, bufferID, error);
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00004A97 File Offset: 0x00002C97
	private void SwigDirectorOnDataProcessed__SWIG_1(uint captureID, uint bufferCategory, uint bufferID)
	{
		this.OnDataProcessed(captureID, bufferCategory, bufferID);
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00004AA2 File Offset: 0x00002CA2
	private void SwigDirectorOnBufferTransferProgress(uint providerID, uint captureID, uint bufferCategory, uint bufferID, uint totalBytes, uint bytesReceived)
	{
		this.OnBufferTransferProgress(providerID, captureID, bufferCategory, bufferID, totalBytes, bytesReceived);
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00004AB3 File Offset: 0x00002CB3
	private void SwigDirectorOnDeviceMemoryLow()
	{
		this.OnDeviceMemoryLow();
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00004ABB File Offset: 0x00002CBB
	private void SwigDirectorOnMaxCaptureDurationExpired(uint captureID)
	{
		this.OnMaxCaptureDurationExpired(captureID);
	}

	// Token: 0x04000026 RID: 38
	private HandleRef swigCPtr;

	// Token: 0x04000027 RID: 39
	protected bool swigCMemOwn;

	// Token: 0x04000028 RID: 40
	private ClientDelegate.SwigDelegateClientDelegate_0 swigDelegate0;

	// Token: 0x04000029 RID: 41
	private ClientDelegate.SwigDelegateClientDelegate_1 swigDelegate1;

	// Token: 0x0400002A RID: 42
	private ClientDelegate.SwigDelegateClientDelegate_2 swigDelegate2;

	// Token: 0x0400002B RID: 43
	private ClientDelegate.SwigDelegateClientDelegate_3 swigDelegate3;

	// Token: 0x0400002C RID: 44
	private ClientDelegate.SwigDelegateClientDelegate_4 swigDelegate4;

	// Token: 0x0400002D RID: 45
	private ClientDelegate.SwigDelegateClientDelegate_5 swigDelegate5;

	// Token: 0x0400002E RID: 46
	private ClientDelegate.SwigDelegateClientDelegate_6 swigDelegate6;

	// Token: 0x0400002F RID: 47
	private ClientDelegate.SwigDelegateClientDelegate_7 swigDelegate7;

	// Token: 0x04000030 RID: 48
	private ClientDelegate.SwigDelegateClientDelegate_8 swigDelegate8;

	// Token: 0x04000031 RID: 49
	private ClientDelegate.SwigDelegateClientDelegate_9 swigDelegate9;

	// Token: 0x04000032 RID: 50
	private ClientDelegate.SwigDelegateClientDelegate_10 swigDelegate10;

	// Token: 0x04000033 RID: 51
	private ClientDelegate.SwigDelegateClientDelegate_11 swigDelegate11;

	// Token: 0x04000034 RID: 52
	private ClientDelegate.SwigDelegateClientDelegate_12 swigDelegate12;

	// Token: 0x04000035 RID: 53
	private ClientDelegate.SwigDelegateClientDelegate_13 swigDelegate13;

	// Token: 0x04000036 RID: 54
	private ClientDelegate.SwigDelegateClientDelegate_14 swigDelegate14;

	// Token: 0x04000037 RID: 55
	private ClientDelegate.SwigDelegateClientDelegate_15 swigDelegate15;

	// Token: 0x04000038 RID: 56
	private ClientDelegate.SwigDelegateClientDelegate_16 swigDelegate16;

	// Token: 0x04000039 RID: 57
	private ClientDelegate.SwigDelegateClientDelegate_17 swigDelegate17;

	// Token: 0x0400003A RID: 58
	private ClientDelegate.SwigDelegateClientDelegate_18 swigDelegate18;

	// Token: 0x0400003B RID: 59
	private ClientDelegate.SwigDelegateClientDelegate_19 swigDelegate19;

	// Token: 0x0400003C RID: 60
	private ClientDelegate.SwigDelegateClientDelegate_20 swigDelegate20;

	// Token: 0x0400003D RID: 61
	private ClientDelegate.SwigDelegateClientDelegate_21 swigDelegate21;

	// Token: 0x0400003E RID: 62
	private ClientDelegate.SwigDelegateClientDelegate_22 swigDelegate22;

	// Token: 0x0400003F RID: 63
	private ClientDelegate.SwigDelegateClientDelegate_23 swigDelegate23;

	// Token: 0x04000040 RID: 64
	private static Type[] swigMethodTypes0 = new Type[0];

	// Token: 0x04000041 RID: 65
	private static Type[] swigMethodTypes1 = new Type[0];

	// Token: 0x04000042 RID: 66
	private static Type[] swigMethodTypes2 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000043 RID: 67
	private static Type[] swigMethodTypes3 = new Type[0];

	// Token: 0x04000044 RID: 68
	private static Type[] swigMethodTypes4 = new Type[] { typeof(uint) };

	// Token: 0x04000045 RID: 69
	private static Type[] swigMethodTypes5 = new Type[] { typeof(uint) };

	// Token: 0x04000046 RID: 70
	private static Type[] swigMethodTypes6 = new Type[] { typeof(uint) };

	// Token: 0x04000047 RID: 71
	private static Type[] swigMethodTypes7 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000048 RID: 72
	private static Type[] swigMethodTypes8 = new Type[0];

	// Token: 0x04000049 RID: 73
	private static Type[] swigMethodTypes9 = new Type[] { typeof(uint) };

	// Token: 0x0400004A RID: 74
	private static Type[] swigMethodTypes10 = new Type[] { typeof(uint) };

	// Token: 0x0400004B RID: 75
	private static Type[] swigMethodTypes11 = new Type[]
	{
		typeof(Client),
		typeof(DataProvider)
	};

	// Token: 0x0400004C RID: 76
	private static Type[] swigMethodTypes12 = new Type[0];

	// Token: 0x0400004D RID: 77
	private static Type[] swigMethodTypes13 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400004E RID: 78
	private static Type[] swigMethodTypes14 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400004F RID: 79
	private static Type[] swigMethodTypes15 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000050 RID: 80
	private static Type[] swigMethodTypes16 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000051 RID: 81
	private static Type[] swigMethodTypes17 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000052 RID: 82
	private static Type[] swigMethodTypes18 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000053 RID: 83
	private static Type[] swigMethodTypes19 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(string)
	};

	// Token: 0x04000054 RID: 84
	private static Type[] swigMethodTypes20 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000055 RID: 85
	private static Type[] swigMethodTypes21 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000056 RID: 86
	private static Type[] swigMethodTypes22 = new Type[0];

	// Token: 0x04000057 RID: 87
	private static Type[] swigMethodTypes23 = new Type[] { typeof(uint) };

	// Token: 0x020000BC RID: 188
	// (Invoke) Token: 0x0600143E RID: 5182
	public delegate void SwigDelegateClientDelegate_0();

	// Token: 0x020000BD RID: 189
	// (Invoke) Token: 0x06001442 RID: 5186
	public delegate void SwigDelegateClientDelegate_1();

	// Token: 0x020000BE RID: 190
	// (Invoke) Token: 0x06001446 RID: 5190
	public delegate void SwigDelegateClientDelegate_2(uint providerID, uint captureID);

	// Token: 0x020000BF RID: 191
	// (Invoke) Token: 0x0600144A RID: 5194
	public delegate void SwigDelegateClientDelegate_3();

	// Token: 0x020000C0 RID: 192
	// (Invoke) Token: 0x0600144E RID: 5198
	public delegate void SwigDelegateClientDelegate_4(uint pid);

	// Token: 0x020000C1 RID: 193
	// (Invoke) Token: 0x06001452 RID: 5202
	public delegate void SwigDelegateClientDelegate_5(uint pid);

	// Token: 0x020000C2 RID: 194
	// (Invoke) Token: 0x06001456 RID: 5206
	public delegate void SwigDelegateClientDelegate_6(uint pid);

	// Token: 0x020000C3 RID: 195
	// (Invoke) Token: 0x0600145A RID: 5210
	public delegate void SwigDelegateClientDelegate_7(uint pid, uint mid);

	// Token: 0x020000C4 RID: 196
	// (Invoke) Token: 0x0600145E RID: 5214
	public delegate void SwigDelegateClientDelegate_8();

	// Token: 0x020000C5 RID: 197
	// (Invoke) Token: 0x06001462 RID: 5218
	public delegate void SwigDelegateClientDelegate_9(uint arg0);

	// Token: 0x020000C6 RID: 198
	// (Invoke) Token: 0x06001466 RID: 5222
	public delegate void SwigDelegateClientDelegate_10(uint arg0);

	// Token: 0x020000C7 RID: 199
	// (Invoke) Token: 0x0600146A RID: 5226
	public delegate void SwigDelegateClientDelegate_11(IntPtr arg0, IntPtr arg1);

	// Token: 0x020000C8 RID: 200
	// (Invoke) Token: 0x0600146E RID: 5230
	public delegate void SwigDelegateClientDelegate_12();

	// Token: 0x020000C9 RID: 201
	// (Invoke) Token: 0x06001472 RID: 5234
	public delegate void SwigDelegateClientDelegate_13(uint providerID, uint captureID, uint bufferCategory, uint bufferID);

	// Token: 0x020000CA RID: 202
	// (Invoke) Token: 0x06001476 RID: 5238
	public delegate void SwigDelegateClientDelegate_14(uint providerID, uint optionID, uint processID);

	// Token: 0x020000CB RID: 203
	// (Invoke) Token: 0x0600147A RID: 5242
	public delegate void SwigDelegateClientDelegate_15(uint providerID, uint categoryID);

	// Token: 0x020000CC RID: 204
	// (Invoke) Token: 0x0600147E RID: 5246
	public delegate void SwigDelegateClientDelegate_16(uint providerID, uint metricID);

	// Token: 0x020000CD RID: 205
	// (Invoke) Token: 0x06001482 RID: 5250
	public delegate void SwigDelegateClientDelegate_17(uint providerID, uint metricCategoryID);

	// Token: 0x020000CE RID: 206
	// (Invoke) Token: 0x06001486 RID: 5254
	public delegate void SwigDelegateClientDelegate_18(uint captureID, uint bufferCategory, uint bufferID);

	// Token: 0x020000CF RID: 207
	// (Invoke) Token: 0x0600148A RID: 5258
	public delegate void SwigDelegateClientDelegate_19(uint captureID, uint bufferCategory, uint bufferID, string error);

	// Token: 0x020000D0 RID: 208
	// (Invoke) Token: 0x0600148E RID: 5262
	public delegate void SwigDelegateClientDelegate_20(uint captureID, uint bufferCategory, uint bufferID);

	// Token: 0x020000D1 RID: 209
	// (Invoke) Token: 0x06001492 RID: 5266
	public delegate void SwigDelegateClientDelegate_21(uint providerID, uint captureID, uint bufferCategory, uint bufferID, uint totalBytes, uint bytesReceived);

	// Token: 0x020000D2 RID: 210
	// (Invoke) Token: 0x06001496 RID: 5270
	public delegate void SwigDelegateClientDelegate_22();

	// Token: 0x020000D3 RID: 211
	// (Invoke) Token: 0x0600149A RID: 5274
	public delegate void SwigDelegateClientDelegate_23(uint captureID);
}
