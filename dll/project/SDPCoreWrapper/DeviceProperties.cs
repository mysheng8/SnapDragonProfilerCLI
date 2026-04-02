using System;
using System.Runtime.InteropServices;

// Token: 0x02000030 RID: 48
public class DeviceProperties : IDisposable
{
	// Token: 0x060002C5 RID: 709 RVA: 0x000082CB File Offset: 0x000064CB
	internal DeviceProperties(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x000082E7 File Offset: 0x000064E7
	internal static HandleRef getCPtr(DeviceProperties obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00008300 File Offset: 0x00006500
	~DeviceProperties()
	{
		this.Dispose();
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x0000832C File Offset: 0x0000652C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DeviceProperties(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x060002CA RID: 714 RVA: 0x000083BC File Offset: 0x000065BC
	// (set) Token: 0x060002C9 RID: 713 RVA: 0x000083AC File Offset: 0x000065AC
	public uint connectionType
	{
		get
		{
			return SDPCorePINVOKE.DeviceProperties_connectionType_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.DeviceProperties_connectionType_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x060002CC RID: 716 RVA: 0x000083F4 File Offset: 0x000065F4
	// (set) Token: 0x060002CB RID: 715 RVA: 0x000083D6 File Offset: 0x000065D6
	public string name
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceProperties_name_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceProperties_name_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x060002CE RID: 718 RVA: 0x00008438 File Offset: 0x00006638
	// (set) Token: 0x060002CD RID: 717 RVA: 0x0000841B File Offset: 0x0000661B
	public string ipAddress
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceProperties_ipAddress_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceProperties_ipAddress_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0000845F File Offset: 0x0000665F
	public DeviceProperties()
		: this(SDPCorePINVOKE.new_DeviceProperties(), true)
	{
	}

	// Token: 0x0400008E RID: 142
	private HandleRef swigCPtr;

	// Token: 0x0400008F RID: 143
	protected bool swigCMemOwn;
}
