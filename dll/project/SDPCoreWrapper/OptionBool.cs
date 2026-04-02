using System;
using System.Runtime.InteropServices;

// Token: 0x0200004E RID: 78
public class OptionBool : Option
{
	// Token: 0x06000515 RID: 1301 RVA: 0x0000D542 File Offset: 0x0000B742
	internal OptionBool(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionBool_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x0000D55E File Offset: 0x0000B75E
	internal static HandleRef getCPtr(OptionBool obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x0000D578 File Offset: 0x0000B778
	~OptionBool()
	{
		this.Dispose();
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x0000D5A4 File Offset: 0x0000B7A4
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionBool(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x0000D628 File Offset: 0x0000B828
	public OptionBool(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionBool__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x0000D66C File Offset: 0x0000B86C
	public OptionBool(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionBool__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0000D6AE File Offset: 0x0000B8AE
	public OptionBool(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionBool__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x0000D6E0 File Offset: 0x0000B8E0
	public override bool SetValue(bool value, bool publishValue)
	{
		return SDPCorePINVOKE.OptionBool_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x0000D6FC File Offset: 0x0000B8FC
	public override bool SetValue(bool value)
	{
		return SDPCorePINVOKE.OptionBool_SetValue__SWIG_1(this.swigCPtr, value);
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0000D718 File Offset: 0x0000B918
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionBool_SetValue__SWIG_2(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0000D744 File Offset: 0x0000B944
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionBool_SetValue__SWIG_3(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0000D76C File Offset: 0x0000B96C
	public override bool GetValue(out bool value)
	{
		return SDPCorePINVOKE.OptionBool_GetValue(this.swigCPtr, out value);
	}

	// Token: 0x04000131 RID: 305
	private HandleRef swigCPtr;
}
