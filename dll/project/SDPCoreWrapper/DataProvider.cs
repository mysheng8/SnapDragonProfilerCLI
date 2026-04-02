using System;
using System.Runtime.InteropServices;

// Token: 0x02000025 RID: 37
public class DataProvider : CoreObject
{
	// Token: 0x06000196 RID: 406 RVA: 0x00005811 File Offset: 0x00003A11
	internal DataProvider(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.DataProvider_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000582D File Offset: 0x00003A2D
	internal static HandleRef getCPtr(DataProvider obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00005844 File Offset: 0x00003A44
	~DataProvider()
	{
		this.Dispose();
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00005870 File Offset: 0x00003A70
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DataProvider(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600019A RID: 410 RVA: 0x000058F4 File Offset: 0x00003AF4
	public DataProvider(ProviderDesc desc, bool replicate)
		: this(SDPCorePINVOKE.new_DataProvider__SWIG_0(ProviderDesc.getCPtr(desc), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600019B RID: 411 RVA: 0x00005916 File Offset: 0x00003B16
	public DataProvider(string name, string description, bool replicate, bool isClient)
		: this(SDPCorePINVOKE.new_DataProvider__SWIG_1(name, description, replicate, isClient), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00005936 File Offset: 0x00003B36
	public DataProvider(string name, string description, bool replicate)
		: this(SDPCorePINVOKE.new_DataProvider__SWIG_2(name, description, replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00005954 File Offset: 0x00003B54
	public static DataProvider CreateDataProvider(string name, string description, bool replicate)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_CreateDataProvider__SWIG_0(name, description, replicate);
		DataProvider dataProvider = ((intPtr == IntPtr.Zero) ? null : new DataProvider(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return dataProvider;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00005990 File Offset: 0x00003B90
	public static DataProvider CreateDataProvider(string name, string description)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_CreateDataProvider__SWIG_1(name, description);
		DataProvider dataProvider = ((intPtr == IntPtr.Zero) ? null : new DataProvider(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return dataProvider;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x000059CC File Offset: 0x00003BCC
	public static DataProvider CreateClientDataProvider(string name, string description, bool replicate)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_CreateClientDataProvider__SWIG_0(name, description, replicate);
		DataProvider dataProvider = ((intPtr == IntPtr.Zero) ? null : new DataProvider(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return dataProvider;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00005A08 File Offset: 0x00003C08
	public static DataProvider CreateClientDataProvider(string name, string description)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_CreateClientDataProvider__SWIG_1(name, description);
		DataProvider dataProvider = ((intPtr == IntPtr.Zero) ? null : new DataProvider(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return dataProvider;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x00005A43 File Offset: 0x00003C43
	public static void DestroyDataProvider(DataProvider provider)
	{
		SDPCorePINVOKE.DataProvider_DestroyDataProvider(DataProvider.getCPtr(provider));
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00005A50 File Offset: 0x00003C50
	public void SetID(uint id)
	{
		SDPCorePINVOKE.DataProvider_SetID(this.swigCPtr, id);
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00005A60 File Offset: 0x00003C60
	public bool Connect(bool blocking)
	{
		return SDPCorePINVOKE.DataProvider_Connect__SWIG_0(this.swigCPtr, blocking);
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00005A7C File Offset: 0x00003C7C
	public bool Connect()
	{
		return SDPCorePINVOKE.DataProvider_Connect__SWIG_1(this.swigCPtr);
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00005A98 File Offset: 0x00003C98
	public virtual bool Update()
	{
		return SDPCorePINVOKE.DataProvider_Update(this.swigCPtr);
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00005AB4 File Offset: 0x00003CB4
	public virtual bool WaitForCommand()
	{
		return SDPCorePINVOKE.DataProvider_WaitForCommand(this.swigCPtr);
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00005AD0 File Offset: 0x00003CD0
	public ProviderDesc GetProviderDesc()
	{
		return new ProviderDesc(SDPCorePINVOKE.DataProvider_GetProviderDesc(this.swigCPtr), true);
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00005AF0 File Offset: 0x00003CF0
	public virtual bool ReportCaptureComplete(uint captureID)
	{
		return SDPCorePINVOKE.DataProvider_ReportCaptureComplete(this.swigCPtr, captureID);
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00005B0B File Offset: 0x00003D0B
	public void RegisterBuffer(uint captureID, BufferKey bufferKey, IntPtr bufferData, uint bufferSize)
	{
		SDPCorePINVOKE.DataProvider_RegisterBuffer__SWIG_0(this.swigCPtr, captureID, BufferKey.getCPtr(bufferKey), bufferData, bufferSize);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00005B2F File Offset: 0x00003D2F
	public void RegisterBuffer(uint captureID, BufferKey bufferKey, SWIGTYPE_p_SDP__RegisteredBufferProvider provider, uint bufferSize)
	{
		SDPCorePINVOKE.DataProvider_RegisterBuffer__SWIG_1(this.swigCPtr, captureID, BufferKey.getCPtr(bufferKey), SWIGTYPE_p_SDP__RegisteredBufferProvider.getCPtr(provider), bufferSize);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00005B58 File Offset: 0x00003D58
	public void UnregisterBuffer(BufferKey bufferKey)
	{
		SDPCorePINVOKE.DataProvider_UnregisterBuffer(this.swigCPtr, BufferKey.getCPtr(bufferKey));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00005B78 File Offset: 0x00003D78
	public void OnBinaryOptionDataReceived(CoreObject parent, uint optionID, uint pid, uint bufferID, IntPtr data, uint dataSize)
	{
		SDPCorePINVOKE.DataProvider_OnBinaryOptionDataReceived(this.swigCPtr, CoreObject.getCPtr(parent), optionID, pid, bufferID, data, dataSize);
	}

	// Token: 0x060001AD RID: 429 RVA: 0x00005B94 File Offset: 0x00003D94
	public OptionCategory AddOptionCategory(string name, string description, OptionCategory parentCategory)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_AddOptionCategory__SWIG_0(this.swigCPtr, name, description, OptionCategory.getCPtr(parentCategory));
		OptionCategory optionCategory = ((intPtr == IntPtr.Zero) ? null : new OptionCategory(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return optionCategory;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x00005BDC File Offset: 0x00003DDC
	public OptionCategory AddOptionCategory(string name, string description)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_AddOptionCategory__SWIG_1(this.swigCPtr, name, description);
		OptionCategory optionCategory = ((intPtr == IntPtr.Zero) ? null : new OptionCategory(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return optionCategory;
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00005C20 File Offset: 0x00003E20
	public OptionCategory AddOptionCategory(string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_AddOptionCategory__SWIG_2(this.swigCPtr, name);
		OptionCategory optionCategory = ((intPtr == IntPtr.Zero) ? null : new OptionCategory(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return optionCategory;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00005C60 File Offset: 0x00003E60
	public OptionCategory GetOptionCategory(string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_GetOptionCategory__SWIG_0(this.swigCPtr, name);
		OptionCategory optionCategory = ((intPtr == IntPtr.Zero) ? null : new OptionCategory(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return optionCategory;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00005CA0 File Offset: 0x00003EA0
	public OptionCategory GetOptionCategory(uint id)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_GetOptionCategory__SWIG_1(this.swigCPtr, id);
		return (intPtr == IntPtr.Zero) ? null : new OptionCategory(intPtr, false);
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00005CD4 File Offset: 0x00003ED4
	public Option AddOption(string name, SDPDataType dataType, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_AddOption__SWIG_0(this.swigCPtr, name, (int)dataType, initialValue, description, pid, attributes, OptionCategory.getCPtr(category));
		Option option = ((intPtr == IntPtr.Zero) ? null : new Option(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return option;
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00005D24 File Offset: 0x00003F24
	public Option AddOption(string name, SDPDataType dataType, string initialValue, string description, uint pid, uint attributes)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_AddOption__SWIG_1(this.swigCPtr, name, (int)dataType, initialValue, description, pid, attributes);
		Option option = ((intPtr == IntPtr.Zero) ? null : new Option(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return option;
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00005D6C File Offset: 0x00003F6C
	public Option GetOption(string name, uint pid)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_GetOption__SWIG_0(this.swigCPtr, name, pid);
		Option option = ((intPtr == IntPtr.Zero) ? null : new Option(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return option;
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00005DB0 File Offset: 0x00003FB0
	public Option GetOption(uint id, uint pid)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataProvider_GetOption__SWIG_1(this.swigCPtr, id, pid);
		return (intPtr == IntPtr.Zero) ? null : new Option(intPtr, false);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00005DE4 File Offset: 0x00003FE4
	public void RemoveProcessOptions(uint pid)
	{
		SDPCorePINVOKE.DataProvider_RemoveProcessOptions(this.swigCPtr, pid);
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00005DF2 File Offset: 0x00003FF2
	public void AddProcess(uint pid, string name, string uid, uint warningFlagsRealTime, uint warningFlagsTrace, uint warningFlagsSnapshot, uint warningFlagsSampling)
	{
		SDPCorePINVOKE.DataProvider_AddProcess__SWIG_0(this.swigCPtr, pid, name, uid, warningFlagsRealTime, warningFlagsTrace, warningFlagsSnapshot, warningFlagsSampling);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00005E17 File Offset: 0x00004017
	public void AddProcess(uint pid, string name, string uid, uint warningFlagsRealTime, uint warningFlagsTrace, uint warningFlagsSnapshot)
	{
		SDPCorePINVOKE.DataProvider_AddProcess__SWIG_1(this.swigCPtr, pid, name, uid, warningFlagsRealTime, warningFlagsTrace, warningFlagsSnapshot);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00005E3A File Offset: 0x0000403A
	public void AddProcess(uint pid, string name, string uid, uint warningFlagsRealTime, uint warningFlagsTrace)
	{
		SDPCorePINVOKE.DataProvider_AddProcess__SWIG_2(this.swigCPtr, pid, name, uid, warningFlagsRealTime, warningFlagsTrace);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00005E5B File Offset: 0x0000405B
	public void AddProcess(uint pid, string name, string uid, uint warningFlagsRealTime)
	{
		SDPCorePINVOKE.DataProvider_AddProcess__SWIG_3(this.swigCPtr, pid, name, uid, warningFlagsRealTime);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00005E7A File Offset: 0x0000407A
	public void AddProcess(uint pid, string name, string uid)
	{
		SDPCorePINVOKE.DataProvider_AddProcess__SWIG_4(this.swigCPtr, pid, name, uid);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00005E97 File Offset: 0x00004097
	public void AddProcess(uint pid, string name)
	{
		SDPCorePINVOKE.DataProvider_AddProcess__SWIG_5(this.swigCPtr, pid, name);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00005EB3 File Offset: 0x000040B3
	public void AddProcess(uint pid)
	{
		SDPCorePINVOKE.DataProvider_AddProcess__SWIG_6(this.swigCPtr, pid);
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00005EC1 File Offset: 0x000040C1
	public void RemoveProcess(uint pid)
	{
		SDPCorePINVOKE.DataProvider_RemoveProcess(this.swigCPtr, pid);
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00005ED0 File Offset: 0x000040D0
	public bool IsProcessTracked(uint pid)
	{
		return SDPCorePINVOKE.DataProvider_IsProcessTracked(this.swigCPtr, pid);
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00005EEC File Offset: 0x000040EC
	public bool IsProcessSelected(uint pid)
	{
		return SDPCorePINVOKE.DataProvider_IsProcessSelected(this.swigCPtr, pid);
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00005F08 File Offset: 0x00004108
	public string GetProcessName(uint pid)
	{
		return SDPCorePINVOKE.DataProvider_GetProcessName(this.swigCPtr, pid);
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00005F24 File Offset: 0x00004124
	public uint GetProcessUid(uint pid)
	{
		return SDPCorePINVOKE.DataProvider_GetProcessUid(this.swigCPtr, pid);
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00005F40 File Offset: 0x00004140
	public IDList GetProcessList()
	{
		return new IDList(SDPCorePINVOKE.DataProvider_GetProcessList(this.swigCPtr), true);
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00005F60 File Offset: 0x00004160
	public Metric AddMetric(string name, SDPDataType dataType, uint category, bool isGlobal, float maxSampleFrequency, uint captureType, string description, uint pid, bool hidden, uint userData)
	{
		Metric metric = new Metric(SDPCorePINVOKE.DataProvider_AddMetric__SWIG_0(this.swigCPtr, name, (int)dataType, category, isGlobal, maxSampleFrequency, captureType, description, pid, hidden, userData), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00005FA0 File Offset: 0x000041A0
	public Metric AddMetric(string name, SDPDataType dataType, uint category, bool isGlobal, float maxSampleFrequency, uint captureType, string description, uint pid, bool hidden)
	{
		Metric metric = new Metric(SDPCorePINVOKE.DataProvider_AddMetric__SWIG_1(this.swigCPtr, name, (int)dataType, category, isGlobal, maxSampleFrequency, captureType, description, pid, hidden), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00005FDC File Offset: 0x000041DC
	public Metric AddMetric(string name, SDPDataType dataType, uint category, bool isGlobal, float maxSampleFrequency, uint captureType, string description, uint pid)
	{
		Metric metric = new Metric(SDPCorePINVOKE.DataProvider_AddMetric__SWIG_2(this.swigCPtr, name, (int)dataType, category, isGlobal, maxSampleFrequency, captureType, description, pid), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00006018 File Offset: 0x00004218
	public Metric AddMetric(string name, SDPDataType dataType, uint category, bool isGlobal, float maxSampleFrequency, uint captureType, string description)
	{
		Metric metric = new Metric(SDPCorePINVOKE.DataProvider_AddMetric__SWIG_3(this.swigCPtr, name, (int)dataType, category, isGlobal, maxSampleFrequency, captureType, description), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x00006050 File Offset: 0x00004250
	public Metric AddMetric(string name, SDPDataType dataType, uint category, bool isGlobal, float maxSampleFrequency, uint captureType)
	{
		Metric metric = new Metric(SDPCorePINVOKE.DataProvider_AddMetric__SWIG_4(this.swigCPtr, name, (int)dataType, category, isGlobal, maxSampleFrequency, captureType), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metric;
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x00006088 File Offset: 0x00004288
	public MetricCategory AddMetricCategory(string name, string description, uint parent)
	{
		MetricCategory metricCategory = new MetricCategory(SDPCorePINVOKE.DataProvider_AddMetricCategory__SWIG_0(this.swigCPtr, name, description, parent), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategory;
	}

	// Token: 0x060001CA RID: 458 RVA: 0x000060B8 File Offset: 0x000042B8
	public MetricCategory AddMetricCategory(string name, string description)
	{
		MetricCategory metricCategory = new MetricCategory(SDPCorePINVOKE.DataProvider_AddMetricCategory__SWIG_1(this.swigCPtr, name, description), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return metricCategory;
	}

	// Token: 0x060001CB RID: 459 RVA: 0x000060E7 File Offset: 0x000042E7
	public void RegisterOnStartCapture(SWIGTYPE_p_f_unsigned_int_unsigned_int_q_const__p_q_const__SDP__DataProvider__void callback)
	{
		SDPCorePINVOKE.DataProvider_RegisterOnStartCapture__SWIG_0(this.swigCPtr, SWIGTYPE_p_f_unsigned_int_unsigned_int_q_const__p_q_const__SDP__DataProvider__void.getCPtr(callback));
	}

	// Token: 0x060001CC RID: 460 RVA: 0x000060FA File Offset: 0x000042FA
	public void RegisterOnStopCapture(SWIGTYPE_p_f_unsigned_int_q_const__p_q_const__SDP__DataProvider__void callback)
	{
		SDPCorePINVOKE.DataProvider_RegisterOnStopCapture__SWIG_0(this.swigCPtr, SWIGTYPE_p_f_unsigned_int_q_const__p_q_const__SDP__DataProvider__void.getCPtr(callback));
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000610D File Offset: 0x0000430D
	public void RegisterOnCancelCapture(SWIGTYPE_p_f_unsigned_int_q_const__p_q_const__SDP__DataProvider__void callback)
	{
		SDPCorePINVOKE.DataProvider_RegisterOnCancelCapture__SWIG_0(this.swigCPtr, SWIGTYPE_p_f_unsigned_int_q_const__p_q_const__SDP__DataProvider__void.getCPtr(callback));
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00006120 File Offset: 0x00004320
	public void RegisterOnDataProviderConnected(SWIGTYPE_p_std__functionT_void_fF_t callback)
	{
		SDPCorePINVOKE.DataProvider_RegisterOnDataProviderConnected(this.swigCPtr, SWIGTYPE_p_std__functionT_void_fF_t.getCPtr(callback));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001CF RID: 463 RVA: 0x00006140 File Offset: 0x00004340
	public void RegisterOnDataProviderDisconnected(SWIGTYPE_p_std__functionT_void_fF_t callback)
	{
		SDPCorePINVOKE.DataProvider_RegisterOnDataProviderDisconnected(this.swigCPtr, SWIGTYPE_p_std__functionT_void_fF_t.getCPtr(callback));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x00006160 File Offset: 0x00004360
	public void RegisterOnStartCapture(SWIGTYPE_p_std__functionT_void_funsigned_int_unsigned_int_SDP__DataProvider_const_pconstF_t callback)
	{
		SDPCorePINVOKE.DataProvider_RegisterOnStartCapture__SWIG_1(this.swigCPtr, SWIGTYPE_p_std__functionT_void_funsigned_int_unsigned_int_SDP__DataProvider_const_pconstF_t.getCPtr(callback));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00006180 File Offset: 0x00004380
	public void RegisterOnStopCapture(SWIGTYPE_p_std__functionT_void_funsigned_int_SDP__DataProvider_const_pconstF_t callback)
	{
		SDPCorePINVOKE.DataProvider_RegisterOnStopCapture__SWIG_1(this.swigCPtr, SWIGTYPE_p_std__functionT_void_funsigned_int_SDP__DataProvider_const_pconstF_t.getCPtr(callback));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x000061A0 File Offset: 0x000043A0
	public void RegisterOnCancelCapture(SWIGTYPE_p_std__functionT_void_funsigned_int_SDP__DataProvider_const_pconstF_t callback)
	{
		SDPCorePINVOKE.DataProvider_RegisterOnCancelCapture__SWIG_1(this.swigCPtr, SWIGTYPE_p_std__functionT_void_funsigned_int_SDP__DataProvider_const_pconstF_t.getCPtr(callback));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x000061C0 File Offset: 0x000043C0
	public void RegisterOnMetricToggled(SWIGTYPE_p_std__functionT_void_fSDP__Metric_const_R_unsigned_int_const_R_bool_SDP__DataProvider_const_pconstF_t callback)
	{
		SDPCorePINVOKE.DataProvider_RegisterOnMetricToggled(this.swigCPtr, SWIGTYPE_p_std__functionT_void_fSDP__Metric_const_R_unsigned_int_const_R_bool_SDP__DataProvider_const_pconstF_t.getCPtr(callback));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x000061E0 File Offset: 0x000043E0
	public uint GetRealtimeCaptureID()
	{
		return SDPCorePINVOKE.DataProvider_GetRealtimeCaptureID(this.swigCPtr);
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x000061FC File Offset: 0x000043FC
	public string FindPackageName(string processName, SWIGTYPE_p_std__string packageInfo)
	{
		string text = SDPCorePINVOKE.DataProvider_FindPackageName(this.swigCPtr, processName, SWIGTYPE_p_std__string.getCPtr(packageInfo));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x04000064 RID: 100
	private HandleRef swigCPtr;
}
