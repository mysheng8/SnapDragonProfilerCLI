using System;
using System.Runtime.InteropServices;

// Token: 0x02000076 RID: 118
public class ReplyOptionAttribute : CommandMsg
{
	// Token: 0x06000780 RID: 1920 RVA: 0x00012F15 File Offset: 0x00011115
	internal ReplyOptionAttribute(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyOptionAttribute_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00012F31 File Offset: 0x00011131
	internal static HandleRef getCPtr(ReplyOptionAttribute obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00012F48 File Offset: 0x00011148
	~ReplyOptionAttribute()
	{
		this.Dispose();
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00012F74 File Offset: 0x00011174
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyOptionAttribute(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000785 RID: 1925 RVA: 0x00013008 File Offset: 0x00011208
	// (set) Token: 0x06000784 RID: 1924 RVA: 0x00012FF8 File Offset: 0x000111F8
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionAttribute_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionAttribute_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000787 RID: 1927 RVA: 0x00013030 File Offset: 0x00011230
	// (set) Token: 0x06000786 RID: 1926 RVA: 0x00013022 File Offset: 0x00011222
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionAttribute_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionAttribute_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000789 RID: 1929 RVA: 0x00013058 File Offset: 0x00011258
	// (set) Token: 0x06000788 RID: 1928 RVA: 0x0001304A File Offset: 0x0001124A
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionAttribute_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionAttribute_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x0600078B RID: 1931 RVA: 0x00013080 File Offset: 0x00011280
	// (set) Token: 0x0600078A RID: 1930 RVA: 0x00013072 File Offset: 0x00011272
	public uint attrIdx
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionAttribute_attrIdx_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionAttribute_attrIdx_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x0600078D RID: 1933 RVA: 0x000130A8 File Offset: 0x000112A8
	// (set) Token: 0x0600078C RID: 1932 RVA: 0x0001309A File Offset: 0x0001129A
	public uint offset
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionAttribute_offset_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionAttribute_offset_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x000130D0 File Offset: 0x000112D0
	// (set) Token: 0x0600078E RID: 1934 RVA: 0x000130C2 File Offset: 0x000112C2
	public uint type
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionAttribute_type_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionAttribute_type_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x000130F8 File Offset: 0x000112F8
	// (set) Token: 0x06000790 RID: 1936 RVA: 0x000130EA File Offset: 0x000112EA
	public string strName
	{
		get
		{
			return SDPCorePINVOKE.ReplyOptionAttribute_strName_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyOptionAttribute_strName_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00013112 File Offset: 0x00011312
	public ReplyOptionAttribute(uint provider, uint optionID, uint processId, uint idx, uint ofs, uint attrType, string name)
		: this(SDPCorePINVOKE.new_ReplyOptionAttribute(provider, optionID, processId, idx, ofs, attrType, name), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0400016F RID: 367
	private HandleRef swigCPtr;
}
