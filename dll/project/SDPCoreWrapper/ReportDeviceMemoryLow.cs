using System;
using System.Runtime.InteropServices;

// Token: 0x0200007E RID: 126
public class ReportDeviceMemoryLow : CommandMsg
{
	// Token: 0x060007ED RID: 2029 RVA: 0x00013CAB File Offset: 0x00011EAB
	internal ReportDeviceMemoryLow(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReportDeviceMemoryLow_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00013CC7 File Offset: 0x00011EC7
	internal static HandleRef getCPtr(ReportDeviceMemoryLow obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00013CE0 File Offset: 0x00011EE0
	~ReportDeviceMemoryLow()
	{
		this.Dispose();
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x00013D0C File Offset: 0x00011F0C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReportDeviceMemoryLow(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00013D90 File Offset: 0x00011F90
	public ReportDeviceMemoryLow()
		: this(SDPCorePINVOKE.new_ReportDeviceMemoryLow(), true)
	{
	}

	// Token: 0x04000177 RID: 375
	private HandleRef swigCPtr;
}
