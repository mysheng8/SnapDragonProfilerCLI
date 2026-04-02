using System;
using System.Runtime.InteropServices;

// Token: 0x02000050 RID: 80
public class OptionColor : Option
{
	// Token: 0x0600052D RID: 1325 RVA: 0x0000D965 File Offset: 0x0000BB65
	internal OptionColor(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionColor_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0000D981 File Offset: 0x0000BB81
	internal static HandleRef getCPtr(OptionColor obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0000D998 File Offset: 0x0000BB98
	~OptionColor()
	{
		this.Dispose();
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0000D9C4 File Offset: 0x0000BBC4
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionColor(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0000DA48 File Offset: 0x0000BC48
	public OptionColor(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionColor__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0000DA8C File Offset: 0x0000BC8C
	public OptionColor(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionColor__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x0000DACE File Offset: 0x0000BCCE
	public OptionColor(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes)
		: this(SDPCorePINVOKE.new_OptionColor__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x0000DB00 File Offset: 0x0000BD00
	public override bool SetValue(SWIGTYPE_p_float value, bool publishValue)
	{
		return SDPCorePINVOKE.OptionColor_SetValue__SWIG_0(this.swigCPtr, SWIGTYPE_p_float.getCPtr(value), publishValue);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x0000DB24 File Offset: 0x0000BD24
	public override bool SetValue(SWIGTYPE_p_float value)
	{
		return SDPCorePINVOKE.OptionColor_SetValue__SWIG_1(this.swigCPtr, SWIGTYPE_p_float.getCPtr(value));
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0000DB44 File Offset: 0x0000BD44
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionColor_SetValue__SWIG_2(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0000DB70 File Offset: 0x0000BD70
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionColor_SetValue__SWIG_3(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0000DB98 File Offset: 0x0000BD98
	public override bool SetValue(float r, float g, float b, float a, bool publishValue)
	{
		return SDPCorePINVOKE.OptionColor_SetValue__SWIG_4(this.swigCPtr, r, g, b, a, publishValue);
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0000DBBC File Offset: 0x0000BDBC
	public override bool SetValue(float r, float g, float b, float a)
	{
		return SDPCorePINVOKE.OptionColor_SetValue__SWIG_5(this.swigCPtr, r, g, b, a);
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x0000DBDC File Offset: 0x0000BDDC
	public override bool GetValue(out float value)
	{
		return SDPCorePINVOKE.OptionColor_GetValue__SWIG_0(this.swigCPtr, out value);
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0000DBF8 File Offset: 0x0000BDF8
	public override bool GetValue(out float r, out float g, out float b, out float a)
	{
		return SDPCorePINVOKE.OptionColor_GetValue__SWIG_1(this.swigCPtr, out r, out g, out b, out a);
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x0000DC18 File Offset: 0x0000BE18
	public override string GetValueStr()
	{
		return SDPCorePINVOKE.OptionColor_GetValueStr(this.swigCPtr);
	}

	// Token: 0x04000133 RID: 307
	private HandleRef swigCPtr;
}
