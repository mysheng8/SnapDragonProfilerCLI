using System;
using System.Runtime.InteropServices;

// Token: 0x0200007D RID: 125
public class ReportCustomDataBind : CommandMsg
{
	// Token: 0x060007E2 RID: 2018 RVA: 0x00013B3B File Offset: 0x00011D3B
	internal ReportCustomDataBind(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReportCustomDataBind_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00013B57 File Offset: 0x00011D57
	internal static HandleRef getCPtr(ReportCustomDataBind obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00013B70 File Offset: 0x00011D70
	~ReportCustomDataBind()
	{
		this.Dispose();
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00013B9C File Offset: 0x00011D9C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReportCustomDataBind(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00013C30 File Offset: 0x00011E30
	// (set) Token: 0x060007E6 RID: 2022 RVA: 0x00013C20 File Offset: 0x00011E20
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataBind_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataBind_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00013C58 File Offset: 0x00011E58
	// (set) Token: 0x060007E8 RID: 2024 RVA: 0x00013C4A File Offset: 0x00011E4A
	public uint uid
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataBind_uid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataBind_uid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x060007EB RID: 2027 RVA: 0x00013C80 File Offset: 0x00011E80
	// (set) Token: 0x060007EA RID: 2026 RVA: 0x00013C72 File Offset: 0x00011E72
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataBind_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataBind_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00013C9A File Offset: 0x00011E9A
	public ReportCustomDataBind(uint provider, uint id, uint metric)
		: this(SDPCorePINVOKE.new_ReportCustomDataBind(provider, id, metric), true)
	{
	}

	// Token: 0x04000176 RID: 374
	private HandleRef swigCPtr;
}
