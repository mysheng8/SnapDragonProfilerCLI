using System;
using System.Runtime.InteropServices;

// Token: 0x0200005E RID: 94
public class ProcessAdded : CommandMsg
{
	// Token: 0x06000606 RID: 1542 RVA: 0x0000FD8B File Offset: 0x0000DF8B
	internal ProcessAdded(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ProcessAdded_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0000FDA7 File Offset: 0x0000DFA7
	internal static HandleRef getCPtr(ProcessAdded obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0000FDC0 File Offset: 0x0000DFC0
	~ProcessAdded()
	{
		this.Dispose();
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0000FDEC File Offset: 0x0000DFEC
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessAdded(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x0600060B RID: 1547 RVA: 0x0000FE80 File Offset: 0x0000E080
	// (set) Token: 0x0600060A RID: 1546 RVA: 0x0000FE70 File Offset: 0x0000E070
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ProcessAdded_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x0600060D RID: 1549 RVA: 0x0000FEA8 File Offset: 0x0000E0A8
	// (set) Token: 0x0600060C RID: 1548 RVA: 0x0000FE9A File Offset: 0x0000E09A
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ProcessAdded_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x0600060F RID: 1551 RVA: 0x0000FEE4 File Offset: 0x0000E0E4
	// (set) Token: 0x0600060E RID: 1550 RVA: 0x0000FEC2 File Offset: 0x0000E0C2
	public SWIGTYPE_p_uint8_t isAvailable
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ProcessAdded_isAvailable_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_isAvailable_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000611 RID: 1553 RVA: 0x0000FF34 File Offset: 0x0000E134
	// (set) Token: 0x06000610 RID: 1552 RVA: 0x0000FF11 File Offset: 0x0000E111
	public SWIGTYPE_p_uint8_t hasIcon
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ProcessAdded_hasIcon_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_hasIcon_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000613 RID: 1555 RVA: 0x0000FF70 File Offset: 0x0000E170
	// (set) Token: 0x06000612 RID: 1554 RVA: 0x0000FF61 File Offset: 0x0000E161
	public string name
	{
		get
		{
			return SDPCorePINVOKE.ProcessAdded_name_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_name_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000615 RID: 1557 RVA: 0x0000FFA0 File Offset: 0x0000E1A0
	// (set) Token: 0x06000614 RID: 1556 RVA: 0x0000FF8A File Offset: 0x0000E18A
	public SWIGTYPE_p_uint8_t iconMem
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.ProcessAdded_iconMem_get(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_uint8_t(intPtr, false);
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_iconMem_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000617 RID: 1559 RVA: 0x0000FFE0 File Offset: 0x0000E1E0
	// (set) Token: 0x06000616 RID: 1558 RVA: 0x0000FFD2 File Offset: 0x0000E1D2
	public uint warningFlagsRealTime
	{
		get
		{
			return SDPCorePINVOKE.ProcessAdded_warningFlagsRealTime_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_warningFlagsRealTime_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000619 RID: 1561 RVA: 0x00010008 File Offset: 0x0000E208
	// (set) Token: 0x06000618 RID: 1560 RVA: 0x0000FFFA File Offset: 0x0000E1FA
	public uint warningFlagsTrace
	{
		get
		{
			return SDPCorePINVOKE.ProcessAdded_warningFlagsTrace_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_warningFlagsTrace_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x00010030 File Offset: 0x0000E230
	// (set) Token: 0x0600061A RID: 1562 RVA: 0x00010022 File Offset: 0x0000E222
	public uint warningFlagsSnapshot
	{
		get
		{
			return SDPCorePINVOKE.ProcessAdded_warningFlagsSnapshot_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_warningFlagsSnapshot_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600061D RID: 1565 RVA: 0x00010058 File Offset: 0x0000E258
	// (set) Token: 0x0600061C RID: 1564 RVA: 0x0001004A File Offset: 0x0000E24A
	public uint warningFlagsSampling
	{
		get
		{
			return SDPCorePINVOKE.ProcessAdded_warningFlagsSampling_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProcessAdded_warningFlagsSampling_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00010074 File Offset: 0x0000E274
	public ProcessAdded(uint uid, string processName, SWIGTYPE_p_uint8_t available, SWIGTYPE_p_uint8_t icon, uint provider, SWIGTYPE_p_uint8_t iconData, uint _warningFlagsRealTime, uint _warningFlagsTrace, uint _warningFlagsSnapshot, uint _warningFlagsSampling)
		: this(SDPCorePINVOKE.new_ProcessAdded__SWIG_0(uid, processName, SWIGTYPE_p_uint8_t.getCPtr(available), SWIGTYPE_p_uint8_t.getCPtr(icon), provider, SWIGTYPE_p_uint8_t.getCPtr(iconData), _warningFlagsRealTime, _warningFlagsTrace, _warningFlagsSnapshot, _warningFlagsSampling), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x000100BC File Offset: 0x0000E2BC
	public ProcessAdded(uint uid, string processName, SWIGTYPE_p_uint8_t available, SWIGTYPE_p_uint8_t icon, uint provider, SWIGTYPE_p_uint8_t iconData, uint _warningFlagsRealTime, uint _warningFlagsTrace, uint _warningFlagsSnapshot)
		: this(SDPCorePINVOKE.new_ProcessAdded__SWIG_1(uid, processName, SWIGTYPE_p_uint8_t.getCPtr(available), SWIGTYPE_p_uint8_t.getCPtr(icon), provider, SWIGTYPE_p_uint8_t.getCPtr(iconData), _warningFlagsRealTime, _warningFlagsTrace, _warningFlagsSnapshot), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x00010100 File Offset: 0x0000E300
	public ProcessAdded(uint uid, string processName, SWIGTYPE_p_uint8_t available, SWIGTYPE_p_uint8_t icon, uint provider, SWIGTYPE_p_uint8_t iconData, uint _warningFlagsRealTime, uint _warningFlagsTrace)
		: this(SDPCorePINVOKE.new_ProcessAdded__SWIG_2(uid, processName, SWIGTYPE_p_uint8_t.getCPtr(available), SWIGTYPE_p_uint8_t.getCPtr(icon), provider, SWIGTYPE_p_uint8_t.getCPtr(iconData), _warningFlagsRealTime, _warningFlagsTrace), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00010142 File Offset: 0x0000E342
	public ProcessAdded(uint uid, string processName, SWIGTYPE_p_uint8_t available, SWIGTYPE_p_uint8_t icon, uint provider, SWIGTYPE_p_uint8_t iconData, uint _warningFlagsRealTime)
		: this(SDPCorePINVOKE.new_ProcessAdded__SWIG_3(uid, processName, SWIGTYPE_p_uint8_t.getCPtr(available), SWIGTYPE_p_uint8_t.getCPtr(icon), provider, SWIGTYPE_p_uint8_t.getCPtr(iconData), _warningFlagsRealTime), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00010177 File Offset: 0x0000E377
	public ProcessAdded(uint uid, string processName, SWIGTYPE_p_uint8_t available, SWIGTYPE_p_uint8_t icon, uint provider, SWIGTYPE_p_uint8_t iconData)
		: this(SDPCorePINVOKE.new_ProcessAdded__SWIG_4(uid, processName, SWIGTYPE_p_uint8_t.getCPtr(available), SWIGTYPE_p_uint8_t.getCPtr(icon), provider, SWIGTYPE_p_uint8_t.getCPtr(iconData)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x000101AA File Offset: 0x0000E3AA
	public ProcessAdded(uint uid, string processName, SWIGTYPE_p_uint8_t available, SWIGTYPE_p_uint8_t icon, uint provider)
		: this(SDPCorePINVOKE.new_ProcessAdded__SWIG_5(uid, processName, SWIGTYPE_p_uint8_t.getCPtr(available), SWIGTYPE_p_uint8_t.getCPtr(icon), provider), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x000101D6 File Offset: 0x0000E3D6
	public ProcessAdded(uint uid, string processName, SWIGTYPE_p_uint8_t available, SWIGTYPE_p_uint8_t icon)
		: this(SDPCorePINVOKE.new_ProcessAdded__SWIG_6(uid, processName, SWIGTYPE_p_uint8_t.getCPtr(available), SWIGTYPE_p_uint8_t.getCPtr(icon)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000145 RID: 325
	private HandleRef swigCPtr;
}
