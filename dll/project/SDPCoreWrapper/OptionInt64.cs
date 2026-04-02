using System;
using System.Runtime.InteropServices;

// Token: 0x02000055 RID: 85
public class OptionInt64 : Option
{
	// Token: 0x0600056F RID: 1391 RVA: 0x0000E5B5 File Offset: 0x0000C7B5
	internal OptionInt64(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionInt64_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0000E5D1 File Offset: 0x0000C7D1
	internal static HandleRef getCPtr(OptionInt64 obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
	~OptionInt64()
	{
		this.Dispose();
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0000E614 File Offset: 0x0000C814
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionInt64(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0000E698 File Offset: 0x0000C898
	public OptionInt64(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionInt64__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0000E6DC File Offset: 0x0000C8DC
	public OptionInt64(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionInt64__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0000E71E File Offset: 0x0000C91E
	public OptionInt64(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionInt64__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0000E750 File Offset: 0x0000C950
	public override bool SetValue(long value, bool publishValue)
	{
		return SDPCorePINVOKE.OptionInt64_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0000E76C File Offset: 0x0000C96C
	public override bool SetValue(long value)
	{
		return SDPCorePINVOKE.OptionInt64_SetValue__SWIG_1(this.swigCPtr, value);
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0000E788 File Offset: 0x0000C988
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionInt64_SetValue__SWIG_2(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0000E7B4 File Offset: 0x0000C9B4
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionInt64_SetValue__SWIG_3(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0000E7DC File Offset: 0x0000C9DC
	public override bool GetValue(SWIGTYPE_p_long_long value)
	{
		bool flag = SDPCorePINVOKE.OptionInt64_GetValue(this.swigCPtr, SWIGTYPE_p_long_long.getCPtr(value));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x04000138 RID: 312
	private HandleRef swigCPtr;
}
