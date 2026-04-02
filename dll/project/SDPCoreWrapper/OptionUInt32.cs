using System;
using System.Runtime.InteropServices;

// Token: 0x0200005B RID: 91
public class OptionUInt32 : Option
{
	// Token: 0x060005D9 RID: 1497 RVA: 0x0000F62D File Offset: 0x0000D82D
	internal OptionUInt32(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionUInt32_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0000F649 File Offset: 0x0000D849
	internal static HandleRef getCPtr(OptionUInt32 obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0000F660 File Offset: 0x0000D860
	~OptionUInt32()
	{
		this.Dispose();
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0000F68C File Offset: 0x0000D88C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionUInt32(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0000F710 File Offset: 0x0000D910
	public OptionUInt32(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionUInt32__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0000F754 File Offset: 0x0000D954
	public OptionUInt32(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionUInt32__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0000F796 File Offset: 0x0000D996
	public OptionUInt32(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionUInt32__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0000F7C8 File Offset: 0x0000D9C8
	public override bool SetValue(uint value, bool publishValue)
	{
		return SDPCorePINVOKE.OptionUInt32_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0000F7E4 File Offset: 0x0000D9E4
	public override bool SetValue(uint value)
	{
		return SDPCorePINVOKE.OptionUInt32_SetValue__SWIG_1(this.swigCPtr, value);
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0000F800 File Offset: 0x0000DA00
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionUInt32_SetValue__SWIG_2(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0000F82C File Offset: 0x0000DA2C
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionUInt32_SetValue__SWIG_3(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0000F854 File Offset: 0x0000DA54
	public override bool GetValue(SWIGTYPE_p_unsigned_int value)
	{
		bool flag = SDPCorePINVOKE.OptionUInt32_GetValue(this.swigCPtr, SWIGTYPE_p_unsigned_int.getCPtr(value));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x04000141 RID: 321
	private HandleRef swigCPtr;
}
