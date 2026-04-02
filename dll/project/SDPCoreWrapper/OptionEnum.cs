using System;
using System.Runtime.InteropServices;

// Token: 0x02000052 RID: 82
public class OptionEnum : Option
{
	// Token: 0x06000549 RID: 1353 RVA: 0x0000DE77 File Offset: 0x0000C077
	internal OptionEnum(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionEnum_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0000DE93 File Offset: 0x0000C093
	internal static HandleRef getCPtr(OptionEnum obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0000DEAC File Offset: 0x0000C0AC
	~OptionEnum()
	{
		this.Dispose();
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x0000DED8 File Offset: 0x0000C0D8
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionEnum(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0000DF5C File Offset: 0x0000C15C
	public OptionEnum(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionEnum__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
	public OptionEnum(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionEnum__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0000DFE2 File Offset: 0x0000C1E2
	public OptionEnum(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionEnum__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0000E014 File Offset: 0x0000C214
	public override bool SetValue(int value, bool publishValue)
	{
		return SDPCorePINVOKE.OptionEnum_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0000E030 File Offset: 0x0000C230
	public override bool SetValue(int value)
	{
		return SDPCorePINVOKE.OptionEnum_SetValue__SWIG_1(this.swigCPtr, value);
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0000E04C File Offset: 0x0000C24C
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionEnum_SetValue__SWIG_2(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0000E078 File Offset: 0x0000C278
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionEnum_SetValue__SWIG_3(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0000E0A0 File Offset: 0x0000C2A0
	public override bool GetValue(SWIGTYPE_p_int value)
	{
		bool flag = SDPCorePINVOKE.OptionEnum_GetValue__SWIG_0(this.swigCPtr, SWIGTYPE_p_int.getCPtr(value));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0000E0D0 File Offset: 0x0000C2D0
	public override bool GetValue(SWIGTYPE_p_std__string value)
	{
		bool flag = SDPCorePINVOKE.OptionEnum_GetValue__SWIG_1(this.swigCPtr, SWIGTYPE_p_std__string.getCPtr(value));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0000E100 File Offset: 0x0000C300
	public override string GetValueStr()
	{
		return SDPCorePINVOKE.OptionEnum_GetValueStr(this.swigCPtr);
	}

	// Token: 0x04000135 RID: 309
	private HandleRef swigCPtr;
}
