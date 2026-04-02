using System;
using System.Runtime.InteropServices;

// Token: 0x0200006A RID: 106
public class ProviderDesc : IDisposable
{
	// Token: 0x060006B1 RID: 1713 RVA: 0x00011461 File Offset: 0x0000F661
	internal ProviderDesc(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0001147D File Offset: 0x0000F67D
	internal static HandleRef getCPtr(ProviderDesc obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x00011494 File Offset: 0x0000F694
	~ProviderDesc()
	{
		this.Dispose();
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x000114C0 File Offset: 0x0000F6C0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProviderDesc(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00011550 File Offset: 0x0000F750
	// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00011540 File Offset: 0x0000F740
	public uint m_ID
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_m_ID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_m_ID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00011578 File Offset: 0x0000F778
	// (set) Token: 0x060006B7 RID: 1719 RVA: 0x0001156A File Offset: 0x0000F76A
	public uint filePort
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_filePort_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_filePort_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x060006BA RID: 1722 RVA: 0x000115A0 File Offset: 0x0000F7A0
	// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00011592 File Offset: 0x0000F792
	public uint optionPort
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_optionPort_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_optionPort_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060006BC RID: 1724 RVA: 0x000115DC File Offset: 0x0000F7DC
	// (set) Token: 0x060006BB RID: 1723 RVA: 0x000115BA File Offset: 0x0000F7BA
	public SWIGTYPE_p_uint8_t isClient
	{
		get
		{
			SWIGTYPE_p_uint8_t swigtype_p_uint8_t = new SWIGTYPE_p_uint8_t(SDPCorePINVOKE.ProviderDesc_isClient_get(this.swigCPtr), true);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_uint8_t;
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_isClient_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x060006BE RID: 1726 RVA: 0x00011618 File Offset: 0x0000F818
	// (set) Token: 0x060006BD RID: 1725 RVA: 0x00011609 File Offset: 0x0000F809
	public string Name
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_Name_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_Name_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x060006C0 RID: 1728 RVA: 0x00011640 File Offset: 0x0000F840
	// (set) Token: 0x060006BF RID: 1727 RVA: 0x00011632 File Offset: 0x0000F832
	public string Description
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_Description_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_Description_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00011668 File Offset: 0x0000F868
	// (set) Token: 0x060006C1 RID: 1729 RVA: 0x0001165A File Offset: 0x0000F85A
	public uint ProcessID
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_ProcessID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_ProcessID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00011690 File Offset: 0x0000F890
	// (set) Token: 0x060006C3 RID: 1731 RVA: 0x00011682 File Offset: 0x0000F882
	public string ProcessName
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_ProcessName_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_ProcessName_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060006C6 RID: 1734 RVA: 0x000116B8 File Offset: 0x0000F8B8
	// (set) Token: 0x060006C5 RID: 1733 RVA: 0x000116AA File Offset: 0x0000F8AA
	public uint ThreadID
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_ThreadID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_ThreadID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060006C8 RID: 1736 RVA: 0x000116E0 File Offset: 0x0000F8E0
	// (set) Token: 0x060006C7 RID: 1735 RVA: 0x000116D2 File Offset: 0x0000F8D2
	public string ThreadName
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_ThreadName_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_ThreadName_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060006CA RID: 1738 RVA: 0x00011708 File Offset: 0x0000F908
	// (set) Token: 0x060006C9 RID: 1737 RVA: 0x000116FA File Offset: 0x0000F8FA
	public uint DataHandlerPluginID
	{
		get
		{
			return SDPCorePINVOKE.ProviderDesc_DataHandlerPluginID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ProviderDesc_DataHandlerPluginID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04000161 RID: 353
	private HandleRef swigCPtr;

	// Token: 0x04000162 RID: 354
	protected bool swigCMemOwn;
}
