using System;
using System.Runtime.InteropServices;

// Token: 0x02000075 RID: 117
public class ReplyOption : CommandMsg
{
	// Token: 0x06000769 RID: 1897 RVA: 0x00012C8E File Offset: 0x00010E8E
	internal ReplyOption(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyOption_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x00012CAA File Offset: 0x00010EAA
	internal static HandleRef getCPtr(ReplyOption obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x00012CC4 File Offset: 0x00010EC4
	~ReplyOption()
	{
		this.Dispose();
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x00012CF0 File Offset: 0x00010EF0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyOption(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x0600076E RID: 1902 RVA: 0x00012D84 File Offset: 0x00010F84
	// (set) Token: 0x0600076D RID: 1901 RVA: 0x00012D74 File Offset: 0x00010F74
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReplyOption_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOption_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000770 RID: 1904 RVA: 0x00012DAC File Offset: 0x00010FAC
	// (set) Token: 0x0600076F RID: 1903 RVA: 0x00012D9E File Offset: 0x00010F9E
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.ReplyOption_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOption_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000772 RID: 1906 RVA: 0x00012DD4 File Offset: 0x00010FD4
	// (set) Token: 0x06000771 RID: 1905 RVA: 0x00012DC6 File Offset: 0x00010FC6
	public SDPDataType dataType
	{
		get
		{
			return (SDPDataType)SDPCorePINVOKE.ReplyOption_dataType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOption_dataType_set(this.swigCPtr, (int)value);
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000774 RID: 1908 RVA: 0x00012DFC File Offset: 0x00010FFC
	// (set) Token: 0x06000773 RID: 1907 RVA: 0x00012DEE File Offset: 0x00010FEE
	public uint attributes
	{
		get
		{
			return SDPCorePINVOKE.ReplyOption_attributes_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOption_attributes_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000776 RID: 1910 RVA: 0x00012E24 File Offset: 0x00011024
	// (set) Token: 0x06000775 RID: 1909 RVA: 0x00012E16 File Offset: 0x00011016
	public uint categoryID
	{
		get
		{
			return SDPCorePINVOKE.ReplyOption_categoryID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOption_categoryID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000778 RID: 1912 RVA: 0x00012E4C File Offset: 0x0001104C
	// (set) Token: 0x06000777 RID: 1911 RVA: 0x00012E3E File Offset: 0x0001103E
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ReplyOption_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOption_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x0600077A RID: 1914 RVA: 0x00012E74 File Offset: 0x00011074
	// (set) Token: 0x06000779 RID: 1913 RVA: 0x00012E66 File Offset: 0x00011066
	public string name
	{
		get
		{
			return SDPCorePINVOKE.ReplyOption_name_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOption_name_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x0600077C RID: 1916 RVA: 0x00012E9C File Offset: 0x0001109C
	// (set) Token: 0x0600077B RID: 1915 RVA: 0x00012E8E File Offset: 0x0001108E
	public string description
	{
		get
		{
			return SDPCorePINVOKE.ReplyOption_description_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOption_description_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x0600077E RID: 1918 RVA: 0x00012EC4 File Offset: 0x000110C4
	// (set) Token: 0x0600077D RID: 1917 RVA: 0x00012EB6 File Offset: 0x000110B6
	public string initialValue
	{
		get
		{
			return SDPCorePINVOKE.ReplyOption_initialValue_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOption_initialValue_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00012EE0 File Offset: 0x000110E0
	public ReplyOption(uint provider, uint optionID, SDPDataType dataType, string optionName, string initValue, string optionDesc, uint procID, uint optionAttr, uint category)
		: this(SDPCorePINVOKE.new_ReplyOption(provider, optionID, (int)dataType, optionName, initValue, optionDesc, procID, optionAttr, category), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0400016E RID: 366
	private HandleRef swigCPtr;
}
