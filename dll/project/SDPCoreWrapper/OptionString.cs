using System;
using System.Runtime.InteropServices;

// Token: 0x02000057 RID: 87
public class OptionString : Option
{
	// Token: 0x060005A7 RID: 1447 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
	internal OptionString(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionString_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
	internal static HandleRef getCPtr(OptionString obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x0000EDDC File Offset: 0x0000CFDC
	~OptionString()
	{
		this.Dispose();
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0000EE08 File Offset: 0x0000D008
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionString(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0000EE8C File Offset: 0x0000D08C
	public OptionString(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionString__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0000EED0 File Offset: 0x0000D0D0
	public OptionString(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionString__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0000EF12 File Offset: 0x0000D112
	public OptionString(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionString__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0000EF44 File Offset: 0x0000D144
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionString_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x0000EF70 File Offset: 0x0000D170
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionString_SetValue__SWIG_1(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0000EF98 File Offset: 0x0000D198
	public override bool GetValue(SWIGTYPE_p_std__string value)
	{
		bool flag = SDPCorePINVOKE.OptionString_GetValue(this.swigCPtr, SWIGTYPE_p_std__string.getCPtr(value));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0400013B RID: 315
	private HandleRef swigCPtr;
}
