using System;
using System.Runtime.InteropServices;

// Token: 0x02000013 RID: 19
public class CancelCapture : CommandMsg
{
	// Token: 0x06000070 RID: 112 RVA: 0x000029D5 File Offset: 0x00000BD5
	internal CancelCapture(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.CancelCapture_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000071 RID: 113 RVA: 0x000029F1 File Offset: 0x00000BF1
	internal static HandleRef getCPtr(CancelCapture obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00002A08 File Offset: 0x00000C08
	~CancelCapture()
	{
		this.Dispose();
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00002A34 File Offset: 0x00000C34
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CancelCapture(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000075 RID: 117 RVA: 0x00002AC8 File Offset: 0x00000CC8
	// (set) Token: 0x06000074 RID: 116 RVA: 0x00002AB8 File Offset: 0x00000CB8
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.CancelCapture_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CancelCapture_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00002AE2 File Offset: 0x00000CE2
	public CancelCapture(uint capture)
		: this(SDPCorePINVOKE.new_CancelCapture(capture), true)
	{
	}

	// Token: 0x0400000E RID: 14
	private HandleRef swigCPtr;
}
