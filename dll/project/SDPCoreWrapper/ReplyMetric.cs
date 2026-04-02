using System;
using System.Runtime.InteropServices;

// Token: 0x0200006E RID: 110
public class ReplyMetric : CommandMsg
{
	// Token: 0x06000709 RID: 1801 RVA: 0x00011F8B File Offset: 0x0001018B
	internal ReplyMetric(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyMetric_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00011FA7 File Offset: 0x000101A7
	internal static HandleRef getCPtr(ReplyMetric obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00011FC0 File Offset: 0x000101C0
	~ReplyMetric()
	{
		this.Dispose();
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00011FEC File Offset: 0x000101EC
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyMetric(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600070E RID: 1806 RVA: 0x00012080 File Offset: 0x00010280
	// (set) Token: 0x0600070D RID: 1805 RVA: 0x00012070 File Offset: 0x00010270
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06000710 RID: 1808 RVA: 0x000120A8 File Offset: 0x000102A8
	// (set) Token: 0x0600070F RID: 1807 RVA: 0x0001209A File Offset: 0x0001029A
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06000712 RID: 1810 RVA: 0x000120D0 File Offset: 0x000102D0
	// (set) Token: 0x06000711 RID: 1809 RVA: 0x000120C2 File Offset: 0x000102C2
	public uint metricCategoryID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_metricCategoryID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_metricCategoryID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000714 RID: 1812 RVA: 0x0001210C File Offset: 0x0001030C
	// (set) Token: 0x06000713 RID: 1811 RVA: 0x000120EA File Offset: 0x000102EA
	public SWIGTYPE_p_uint8_t isEnabled
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ReplyMetric_isEnabled_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_isEnabled_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000716 RID: 1814 RVA: 0x0001215C File Offset: 0x0001035C
	// (set) Token: 0x06000715 RID: 1813 RVA: 0x00012139 File Offset: 0x00010339
	public SWIGTYPE_p_uint8_t isAvailable
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ReplyMetric_isAvailable_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_isAvailable_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000718 RID: 1816 RVA: 0x000121AC File Offset: 0x000103AC
	// (set) Token: 0x06000717 RID: 1815 RVA: 0x00012189 File Offset: 0x00010389
	public SWIGTYPE_p_uint8_t isGlobal
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ReplyMetric_isGlobal_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_isGlobal_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x0600071A RID: 1818 RVA: 0x000121E8 File Offset: 0x000103E8
	// (set) Token: 0x06000719 RID: 1817 RVA: 0x000121D9 File Offset: 0x000103D9
	public SDPDataType dataType
	{
		get
		{
			return (SDPDataType)SDPCorePINVOKE.ReplyMetric_dataType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_dataType_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600071C RID: 1820 RVA: 0x00012210 File Offset: 0x00010410
	// (set) Token: 0x0600071B RID: 1819 RVA: 0x00012202 File Offset: 0x00010402
	public float sampleFrequency
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_sampleFrequency_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_sampleFrequency_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x0600071E RID: 1822 RVA: 0x00012238 File Offset: 0x00010438
	// (set) Token: 0x0600071D RID: 1821 RVA: 0x0001222A File Offset: 0x0001042A
	public uint captureType
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_captureType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_captureType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000720 RID: 1824 RVA: 0x00012260 File Offset: 0x00010460
	// (set) Token: 0x0600071F RID: 1823 RVA: 0x00012252 File Offset: 0x00010452
	public uint dataRefType
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_dataRefType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_dataRefType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000722 RID: 1826 RVA: 0x00012288 File Offset: 0x00010488
	// (set) Token: 0x06000721 RID: 1825 RVA: 0x0001227A File Offset: 0x0001047A
	public string name
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_name_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_name_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000724 RID: 1828 RVA: 0x000122B0 File Offset: 0x000104B0
	// (set) Token: 0x06000723 RID: 1827 RVA: 0x000122A2 File Offset: 0x000104A2
	public string description
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_description_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_description_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000726 RID: 1830 RVA: 0x000122D8 File Offset: 0x000104D8
	// (set) Token: 0x06000725 RID: 1829 RVA: 0x000122CA File Offset: 0x000104CA
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000728 RID: 1832 RVA: 0x00012300 File Offset: 0x00010500
	// (set) Token: 0x06000727 RID: 1831 RVA: 0x000122F2 File Offset: 0x000104F2
	public bool hidden
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_hidden_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_hidden_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x0600072A RID: 1834 RVA: 0x00012328 File Offset: 0x00010528
	// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001231A File Offset: 0x0001051A
	public uint userData
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetric_userData_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetric_userData_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00012344 File Offset: 0x00010544
	public ReplyMetric(uint provider, uint category, uint metric, string metricName, string desc, bool enabled, bool available, bool global, SDPDataType metricDataType, float frequency, uint capType, uint refType, uint _pid, bool _hidden, uint _userData)
		: this(SDPCorePINVOKE.new_ReplyMetric__SWIG_0(provider, category, metric, metricName, desc, enabled, available, global, (int)metricDataType, frequency, capType, refType, _pid, _hidden, _userData), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00012388 File Offset: 0x00010588
	public ReplyMetric(uint provider, uint category, uint metric, string metricName, string desc, bool enabled, bool available, bool global, SDPDataType metricDataType, float frequency, uint capType, uint refType, uint _pid, bool _hidden)
		: this(SDPCorePINVOKE.new_ReplyMetric__SWIG_1(provider, category, metric, metricName, desc, enabled, available, global, (int)metricDataType, frequency, capType, refType, _pid, _hidden), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000167 RID: 359
	private HandleRef swigCPtr;
}
