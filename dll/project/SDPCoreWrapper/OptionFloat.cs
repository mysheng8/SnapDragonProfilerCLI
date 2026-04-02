using System;
using System.Runtime.InteropServices;

// Token: 0x02000053 RID: 83
public class OptionFloat : Option
{
	// Token: 0x06000557 RID: 1367 RVA: 0x0000E11A File Offset: 0x0000C31A
	internal OptionFloat(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionFloat_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0000E136 File Offset: 0x0000C336
	internal static HandleRef getCPtr(OptionFloat obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0000E150 File Offset: 0x0000C350
	~OptionFloat()
	{
		this.Dispose();
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0000E17C File Offset: 0x0000C37C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionFloat(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0000E200 File Offset: 0x0000C400
	public OptionFloat(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionFloat__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0000E244 File Offset: 0x0000C444
	public OptionFloat(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionFloat__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0000E286 File Offset: 0x0000C486
	public OptionFloat(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionFloat__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
	public override bool SetValue(float value, bool publishValue)
	{
		return SDPCorePINVOKE.OptionFloat_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0000E2D4 File Offset: 0x0000C4D4
	public override bool SetValue(float value)
	{
		return SDPCorePINVOKE.OptionFloat_SetValue__SWIG_1(this.swigCPtr, value);
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0000E2F0 File Offset: 0x0000C4F0
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionFloat_SetValue__SWIG_2(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0000E31C File Offset: 0x0000C51C
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionFloat_SetValue__SWIG_3(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0000E344 File Offset: 0x0000C544
	public override bool GetValue(out float value)
	{
		return SDPCorePINVOKE.OptionFloat_GetValue(this.swigCPtr, out value);
	}

	// Token: 0x04000136 RID: 310
	private HandleRef swigCPtr;
}
