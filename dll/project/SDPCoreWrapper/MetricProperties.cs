using System;
using System.Runtime.InteropServices;

// Token: 0x02000043 RID: 67
public class MetricProperties : IDisposable
{
	// Token: 0x0600042F RID: 1071 RVA: 0x0000B5EA File Offset: 0x000097EA
	internal MetricProperties(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0000B606 File Offset: 0x00009806
	internal static HandleRef getCPtr(MetricProperties obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0000B620 File Offset: 0x00009820
	~MetricProperties()
	{
		this.Dispose();
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0000B64C File Offset: 0x0000984C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_MetricProperties(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000434 RID: 1076 RVA: 0x0000B6E8 File Offset: 0x000098E8
	// (set) Token: 0x06000433 RID: 1075 RVA: 0x0000B6CC File Offset: 0x000098CC
	public string name
	{
		get
		{
			string text = SDPCorePINVOKE.MetricProperties_name_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_name_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000B72C File Offset: 0x0000992C
	// (set) Token: 0x06000435 RID: 1077 RVA: 0x0000B70F File Offset: 0x0000990F
	public string description
	{
		get
		{
			string text = SDPCorePINVOKE.MetricProperties_description_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_description_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000438 RID: 1080 RVA: 0x0000B764 File Offset: 0x00009964
	// (set) Token: 0x06000437 RID: 1079 RVA: 0x0000B753 File Offset: 0x00009953
	public MetricState state
	{
		get
		{
			return (MetricState)SDPCorePINVOKE.MetricProperties_state_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_state_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600043A RID: 1082 RVA: 0x0000B78C File Offset: 0x0000998C
	// (set) Token: 0x06000439 RID: 1081 RVA: 0x0000B77E File Offset: 0x0000997E
	public uint type
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_type_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_type_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600043C RID: 1084 RVA: 0x0000B7B4 File Offset: 0x000099B4
	// (set) Token: 0x0600043B RID: 1083 RVA: 0x0000B7A6 File Offset: 0x000099A6
	public uint global
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_global_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_global_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000B7DC File Offset: 0x000099DC
	// (set) Token: 0x0600043D RID: 1085 RVA: 0x0000B7CE File Offset: 0x000099CE
	public float sampleRate
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_sampleRate_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_sampleRate_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000440 RID: 1088 RVA: 0x0000B804 File Offset: 0x00009A04
	// (set) Token: 0x0600043F RID: 1087 RVA: 0x0000B7F6 File Offset: 0x000099F6
	public uint captureTypeMask
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_captureTypeMask_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_captureTypeMask_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000B82C File Offset: 0x00009A2C
	// (set) Token: 0x06000441 RID: 1089 RVA: 0x0000B81E File Offset: 0x00009A1E
	public uint captureState
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_captureState_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_captureState_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000B854 File Offset: 0x00009A54
	// (set) Token: 0x06000443 RID: 1091 RVA: 0x0000B846 File Offset: 0x00009A46
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000B87C File Offset: 0x00009A7C
	// (set) Token: 0x06000445 RID: 1093 RVA: 0x0000B86E File Offset: 0x00009A6E
	public uint categoryID
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_categoryID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_categoryID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000B8A4 File Offset: 0x00009AA4
	// (set) Token: 0x06000447 RID: 1095 RVA: 0x0000B896 File Offset: 0x00009A96
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000B8CC File Offset: 0x00009ACC
	// (set) Token: 0x06000449 RID: 1097 RVA: 0x0000B8BE File Offset: 0x00009ABE
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600044C RID: 1100 RVA: 0x0000B8F4 File Offset: 0x00009AF4
	// (set) Token: 0x0600044B RID: 1099 RVA: 0x0000B8E6 File Offset: 0x00009AE6
	public long lastUpdated
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_lastUpdated_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_lastUpdated_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600044E RID: 1102 RVA: 0x0000B91C File Offset: 0x00009B1C
	// (set) Token: 0x0600044D RID: 1101 RVA: 0x0000B90E File Offset: 0x00009B0E
	public bool hidden
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_hidden_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_hidden_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000B944 File Offset: 0x00009B44
	// (set) Token: 0x0600044F RID: 1103 RVA: 0x0000B936 File Offset: 0x00009B36
	public uint userData
	{
		get
		{
			return SDPCorePINVOKE.MetricProperties_userData_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.MetricProperties_userData_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x0000B95E File Offset: 0x00009B5E
	public MetricProperties()
		: this(SDPCorePINVOKE.new_MetricProperties__SWIG_0(), true)
	{
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0000B96C File Offset: 0x00009B6C
	public MetricProperties(MetricProperties p)
		: this(SDPCorePINVOKE.new_MetricProperties__SWIG_1(MetricProperties.getCPtr(p)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0000B990 File Offset: 0x00009B90
	public MetricProperties(string _name, string _description, SDPDataType _type, uint _category, bool _global, float _sampleRate, uint _captureTypeMask, uint _providerID, uint _pid, bool _hidden, uint _userData)
		: this(SDPCorePINVOKE.new_MetricProperties__SWIG_2(_name, _description, (int)_type, _category, _global, _sampleRate, _captureTypeMask, _providerID, _pid, _hidden, _userData), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0000B9CC File Offset: 0x00009BCC
	public MetricProperties(string _name, string _description, SDPDataType _type, uint _category, bool _global, float _sampleRate, uint _captureTypeMask, uint _providerID, uint _pid, bool _hidden)
		: this(SDPCorePINVOKE.new_MetricProperties__SWIG_3(_name, _description, (int)_type, _category, _global, _sampleRate, _captureTypeMask, _providerID, _pid, _hidden), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0000BA03 File Offset: 0x00009C03
	public void Equal(MetricProperties p)
	{
		SDPCorePINVOKE.MetricProperties_Equal(this.swigCPtr, MetricProperties.getCPtr(p));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0400011A RID: 282
	private HandleRef swigCPtr;

	// Token: 0x0400011B RID: 283
	protected bool swigCMemOwn;
}
