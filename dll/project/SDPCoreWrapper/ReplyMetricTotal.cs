using System;
using System.Runtime.InteropServices;

// Token: 0x02000074 RID: 116
public class ReplyMetricTotal : CommandMsg
{
	// Token: 0x06000760 RID: 1888 RVA: 0x00012B49 File Offset: 0x00010D49
	internal ReplyMetricTotal(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyMetricTotal_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x00012B65 File Offset: 0x00010D65
	internal static HandleRef getCPtr(ReplyMetricTotal obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x00012B7C File Offset: 0x00010D7C
	~ReplyMetricTotal()
	{
		this.Dispose();
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00012BA8 File Offset: 0x00010DA8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyMetricTotal(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000765 RID: 1893 RVA: 0x00012C3C File Offset: 0x00010E3C
	// (set) Token: 0x06000764 RID: 1892 RVA: 0x00012C2C File Offset: 0x00010E2C
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricTotal_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricTotal_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000767 RID: 1895 RVA: 0x00012C64 File Offset: 0x00010E64
	// (set) Token: 0x06000766 RID: 1894 RVA: 0x00012C56 File Offset: 0x00010E56
	public uint numMetrics
	{
		get
		{
			return SDPCorePINVOKE.ReplyMetricTotal_numMetrics_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyMetricTotal_numMetrics_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x00012C7E File Offset: 0x00010E7E
	public ReplyMetricTotal(uint provider, uint total)
		: this(SDPCorePINVOKE.new_ReplyMetricTotal(provider, total), true)
	{
	}

	// Token: 0x0400016D RID: 365
	private HandleRef swigCPtr;
}
