using System;
using System.Runtime.InteropServices;

// Token: 0x02000051 RID: 81
public class OptionDouble : Option
{
	// Token: 0x0600053D RID: 1341 RVA: 0x0000DC32 File Offset: 0x0000BE32
	internal OptionDouble(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionDouble_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0000DC4E File Offset: 0x0000BE4E
	internal static HandleRef getCPtr(OptionDouble obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0000DC68 File Offset: 0x0000BE68
	~OptionDouble()
	{
		this.Dispose();
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x0000DC94 File Offset: 0x0000BE94
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionDouble(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0000DD18 File Offset: 0x0000BF18
	public OptionDouble(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionDouble__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0000DD5C File Offset: 0x0000BF5C
	public OptionDouble(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionDouble__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0000DD9E File Offset: 0x0000BF9E
	public OptionDouble(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionDouble__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0000DDD0 File Offset: 0x0000BFD0
	public override bool SetValue(double value, bool publishValue)
	{
		return SDPCorePINVOKE.OptionDouble_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0000DDEC File Offset: 0x0000BFEC
	public override bool SetValue(double value)
	{
		return SDPCorePINVOKE.OptionDouble_SetValue__SWIG_1(this.swigCPtr, value);
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0000DE08 File Offset: 0x0000C008
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionDouble_SetValue__SWIG_2(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0000DE34 File Offset: 0x0000C034
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionDouble_SetValue__SWIG_3(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0000DE5C File Offset: 0x0000C05C
	public override bool GetValue(out double value)
	{
		return SDPCorePINVOKE.OptionDouble_GetValue(this.swigCPtr, out value);
	}

	// Token: 0x04000134 RID: 308
	private HandleRef swigCPtr;
}
