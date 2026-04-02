using System;
using System.Runtime.InteropServices;

// Token: 0x0200005F RID: 95
public class ProcessIcon : IDisposable
{
	// Token: 0x06000625 RID: 1573 RVA: 0x00010200 File Offset: 0x0000E400
	internal ProcessIcon(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0001021C File Offset: 0x0000E41C
	internal static HandleRef getCPtr(ProcessIcon obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x00010234 File Offset: 0x0000E434
	~ProcessIcon()
	{
		this.Dispose();
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x00010260 File Offset: 0x0000E460
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ProcessIcon(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600062A RID: 1578 RVA: 0x000102F4 File Offset: 0x0000E4F4
	// (set) Token: 0x06000629 RID: 1577 RVA: 0x000102E0 File Offset: 0x0000E4E0
	public SWIGTYPE_p_uint8_t data
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.ProcessIcon_data_get(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_uint8_t(intPtr, false);
		}
		set
		{
			SDPCorePINVOKE.ProcessIcon_data_set(this.swigCPtr, SWIGTYPE_p_uint8_t.getCPtr(value));
		}
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00010326 File Offset: 0x0000E526
	public ProcessIcon()
		: this(SDPCorePINVOKE.new_ProcessIcon(), true)
	{
	}

	// Token: 0x04000146 RID: 326
	private HandleRef swigCPtr;

	// Token: 0x04000147 RID: 327
	protected bool swigCMemOwn;
}
