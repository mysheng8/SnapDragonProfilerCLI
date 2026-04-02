using System;
using System.Runtime.InteropServices;

// Token: 0x02000054 RID: 84
public class OptionInt32 : Option
{
	// Token: 0x06000563 RID: 1379 RVA: 0x0000E35F File Offset: 0x0000C55F
	internal OptionInt32(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionInt32_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0000E37B File Offset: 0x0000C57B
	internal static HandleRef getCPtr(OptionInt32 obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0000E394 File Offset: 0x0000C594
	~OptionInt32()
	{
		this.Dispose();
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionInt32(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0000E444 File Offset: 0x0000C644
	public OptionInt32(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionInt32__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0000E488 File Offset: 0x0000C688
	public OptionInt32(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionInt32__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0000E4CA File Offset: 0x0000C6CA
	public OptionInt32(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionInt32__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0000E4FC File Offset: 0x0000C6FC
	public override bool SetValue(int value, bool publishValue)
	{
		return SDPCorePINVOKE.OptionInt32_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0000E518 File Offset: 0x0000C718
	public override bool SetValue(int value)
	{
		return SDPCorePINVOKE.OptionInt32_SetValue__SWIG_1(this.swigCPtr, value);
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0000E534 File Offset: 0x0000C734
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionInt32_SetValue__SWIG_2(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x0000E560 File Offset: 0x0000C760
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionInt32_SetValue__SWIG_3(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0000E588 File Offset: 0x0000C788
	public override bool GetValue(SWIGTYPE_p_int value)
	{
		bool flag = SDPCorePINVOKE.OptionInt32_GetValue(this.swigCPtr, SWIGTYPE_p_int.getCPtr(value));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x04000137 RID: 311
	private HandleRef swigCPtr;
}
