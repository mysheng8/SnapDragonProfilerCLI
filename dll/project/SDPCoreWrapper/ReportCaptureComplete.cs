using System;
using System.Runtime.InteropServices;

// Token: 0x0200007A RID: 122
public class ReportCaptureComplete : CommandMsg
{
	// Token: 0x060007B9 RID: 1977 RVA: 0x00013645 File Offset: 0x00011845
	internal ReportCaptureComplete(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReportCaptureComplete_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00013661 File Offset: 0x00011861
	internal static HandleRef getCPtr(ReportCaptureComplete obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00013678 File Offset: 0x00011878
	~ReportCaptureComplete()
	{
		this.Dispose();
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x000136A4 File Offset: 0x000118A4
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReportCaptureComplete(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x060007BE RID: 1982 RVA: 0x00013738 File Offset: 0x00011938
	// (set) Token: 0x060007BD RID: 1981 RVA: 0x00013728 File Offset: 0x00011928
	public uint providerID
	{
		get
		{
			return SDPCorePINVOKE.ReportCaptureComplete_providerID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCaptureComplete_providerID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00013760 File Offset: 0x00011960
	// (set) Token: 0x060007BF RID: 1983 RVA: 0x00013752 File Offset: 0x00011952
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.ReportCaptureComplete_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReportCaptureComplete_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0001377A File Offset: 0x0001197A
	public ReportCaptureComplete(uint provider, uint captureId)
		: this(SDPCorePINVOKE.new_ReportCaptureComplete(provider, captureId), true)
	{
	}

	// Token: 0x04000173 RID: 371
	private HandleRef swigCPtr;
}
