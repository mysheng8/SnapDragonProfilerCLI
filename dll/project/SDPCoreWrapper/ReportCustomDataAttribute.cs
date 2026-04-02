using System;
using System.Runtime.InteropServices;

// Token: 0x0200007C RID: 124
public class ReportCustomDataAttribute : CommandMsg
{
	// Token: 0x060007CF RID: 1999 RVA: 0x00013925 File Offset: 0x00011B25
	internal ReportCustomDataAttribute(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReportCustomDataAttribute_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00013941 File Offset: 0x00011B41
	internal static HandleRef getCPtr(ReportCustomDataAttribute obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00013958 File Offset: 0x00011B58
	~ReportCustomDataAttribute()
	{
		this.Dispose();
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00013984 File Offset: 0x00011B84
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReportCustomDataAttribute(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060007D4 RID: 2004 RVA: 0x00013A18 File Offset: 0x00011C18
	// (set) Token: 0x060007D3 RID: 2003 RVA: 0x00013A08 File Offset: 0x00011C08
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataAttribute_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataAttribute_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060007D6 RID: 2006 RVA: 0x00013A40 File Offset: 0x00011C40
	// (set) Token: 0x060007D5 RID: 2005 RVA: 0x00013A32 File Offset: 0x00011C32
	public uint uid
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataAttribute_uid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataAttribute_uid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00013A68 File Offset: 0x00011C68
	// (set) Token: 0x060007D7 RID: 2007 RVA: 0x00013A5A File Offset: 0x00011C5A
	public uint attrIdx
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataAttribute_attrIdx_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataAttribute_attrIdx_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x060007DA RID: 2010 RVA: 0x00013A90 File Offset: 0x00011C90
	// (set) Token: 0x060007D9 RID: 2009 RVA: 0x00013A82 File Offset: 0x00011C82
	public uint offset
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataAttribute_offset_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataAttribute_offset_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x060007DC RID: 2012 RVA: 0x00013AB8 File Offset: 0x00011CB8
	// (set) Token: 0x060007DB RID: 2011 RVA: 0x00013AAA File Offset: 0x00011CAA
	public uint type
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataAttribute_type_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataAttribute_type_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x060007DE RID: 2014 RVA: 0x00013AE0 File Offset: 0x00011CE0
	// (set) Token: 0x060007DD RID: 2013 RVA: 0x00013AD2 File Offset: 0x00011CD2
	public uint numAttr
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataAttribute_numAttr_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataAttribute_numAttr_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00013B08 File Offset: 0x00011D08
	// (set) Token: 0x060007DF RID: 2015 RVA: 0x00013AFA File Offset: 0x00011CFA
	public string strName
	{
		get
		{
			return SDPCorePINVOKE.ReportCustomDataAttribute_strName_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCustomDataAttribute_strName_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00013B22 File Offset: 0x00011D22
	public ReportCustomDataAttribute(uint provider, uint id, uint attributeIdx, uint byteOffset, uint dataType, string name, uint numAttributes)
		: this(SDPCorePINVOKE.new_ReportCustomDataAttribute(provider, id, attributeIdx, byteOffset, dataType, name, numAttributes), true)
	{
	}

	// Token: 0x04000175 RID: 373
	private HandleRef swigCPtr;
}
