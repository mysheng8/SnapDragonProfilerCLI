using System;
using System.Runtime.InteropServices;

// Token: 0x0200007B RID: 123
public class ReportCustomData : CommandMsg
{
	// Token: 0x060007C2 RID: 1986 RVA: 0x0001378A File Offset: 0x0001198A
	internal ReportCustomData(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReportCustomData_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000137A6 File Offset: 0x000119A6
	internal static HandleRef getCPtr(ReportCustomData obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x000137C0 File Offset: 0x000119C0
	~ReportCustomData()
	{
		this.Dispose();
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x000137EC File Offset: 0x000119EC
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReportCustomData(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00013880 File Offset: 0x00011A80
	// (set) Token: 0x060007C6 RID: 1990 RVA: 0x00013870 File Offset: 0x00011A70
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomData_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomData_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x060007C9 RID: 1993 RVA: 0x000138A8 File Offset: 0x00011AA8
	// (set) Token: 0x060007C8 RID: 1992 RVA: 0x0001389A File Offset: 0x00011A9A
	public uint uid
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomData_uid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomData_uid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x060007CB RID: 1995 RVA: 0x000138D0 File Offset: 0x00011AD0
	// (set) Token: 0x060007CA RID: 1994 RVA: 0x000138C2 File Offset: 0x00011AC2
	public uint numCustomData
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomData_numCustomData_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomData_numCustomData_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x060007CD RID: 1997 RVA: 0x000138F8 File Offset: 0x00011AF8
	// (set) Token: 0x060007CC RID: 1996 RVA: 0x000138EA File Offset: 0x00011AEA
	public string name
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomData_name_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomData_name_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00013912 File Offset: 0x00011B12
	public ReportCustomData(uint provider, uint id, string customRefName, uint numCustomDataDef)
		: this(SDPCorePINVOKE.new_ReportCustomData(provider, id, customRefName, numCustomDataDef), true)
	{
	}

	// Token: 0x04000174 RID: 372
	private HandleRef swigCPtr;
}
