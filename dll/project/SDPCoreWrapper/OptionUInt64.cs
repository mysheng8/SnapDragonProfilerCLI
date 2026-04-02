using System;
using System.Runtime.InteropServices;

// Token: 0x0200005C RID: 92
public class OptionUInt64 : Option
{
	// Token: 0x060005E5 RID: 1509 RVA: 0x0000F881 File Offset: 0x0000DA81
	internal OptionUInt64(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionUInt64_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0000F89D File Offset: 0x0000DA9D
	internal static HandleRef getCPtr(OptionUInt64 obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0000F8B4 File Offset: 0x0000DAB4
	~OptionUInt64()
	{
		this.Dispose();
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionUInt64(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0000F964 File Offset: 0x0000DB64
	public OptionUInt64(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionUInt64__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0000F9A8 File Offset: 0x0000DBA8
	public OptionUInt64(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionUInt64__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0000F9EA File Offset: 0x0000DBEA
	public OptionUInt64(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionUInt64__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0000FA1C File Offset: 0x0000DC1C
	public override bool SetValue(ulong value, bool publishValue)
	{
		return SDPCorePINVOKE.OptionUInt64_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0000FA38 File Offset: 0x0000DC38
	public override bool SetValue(ulong value)
	{
		return SDPCorePINVOKE.OptionUInt64_SetValue__SWIG_1(this.swigCPtr, value);
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0000FA54 File Offset: 0x0000DC54
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionUInt64_SetValue__SWIG_2(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0000FA80 File Offset: 0x0000DC80
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionUInt64_SetValue__SWIG_3(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0000FAA8 File Offset: 0x0000DCA8
	public override bool GetValue(SWIGTYPE_p_unsigned_long_long value)
	{
		bool flag = SDPCorePINVOKE.OptionUInt64_GetValue(this.swigCPtr, SWIGTYPE_p_unsigned_long_long.getCPtr(value));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x04000142 RID: 322
	private HandleRef swigCPtr;
}
