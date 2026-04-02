using System;
using System.Runtime.InteropServices;

// Token: 0x02000032 RID: 50
public class ICPStringPairMessage : CommandMsg
{
	// Token: 0x060002D0 RID: 720 RVA: 0x0000846D File Offset: 0x0000666D
	internal ICPStringPairMessage(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ICPStringPairMessage_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00008489 File Offset: 0x00006689
	internal static HandleRef getCPtr(ICPStringPairMessage obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x000084A0 File Offset: 0x000066A0
	~ICPStringPairMessage()
	{
		this.Dispose();
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x000084CC File Offset: 0x000066CC
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ICPStringPairMessage(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x060002D5 RID: 725 RVA: 0x00008560 File Offset: 0x00006760
	// (set) Token: 0x060002D4 RID: 724 RVA: 0x00008550 File Offset: 0x00006750
	public uint uid
	{
		get
		{
			return SDPCorePINVOKE.ICPStringPairMessage_uid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ICPStringPairMessage_uid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x060002D7 RID: 727 RVA: 0x00008588 File Offset: 0x00006788
	// (set) Token: 0x060002D6 RID: 726 RVA: 0x0000857A File Offset: 0x0000677A
	public string value
	{
		get
		{
			return SDPCorePINVOKE.ICPStringPairMessage_value_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ICPStringPairMessage_value_set(this.swigCPtr, value);
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x000085A2 File Offset: 0x000067A2
	public ICPStringPairMessage(uint stringID, string stringValue)
		: this(SDPCorePINVOKE.new_ICPStringPairMessage(stringID, stringValue), true)
	{
	}

	// Token: 0x040000A1 RID: 161
	private HandleRef swigCPtr;
}
