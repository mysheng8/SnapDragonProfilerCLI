using System;
using System.Runtime.InteropServices;

// Token: 0x0200004D RID: 77
public class OptionBinary : Option
{
	// Token: 0x06000508 RID: 1288 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
	internal OptionBinary(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionBinary_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x0000D2E4 File Offset: 0x0000B4E4
	internal static HandleRef getCPtr(OptionBinary obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x0000D2FC File Offset: 0x0000B4FC
	~OptionBinary()
	{
		this.Dispose();
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0000D328 File Offset: 0x0000B528
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionBinary(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0000D3AC File Offset: 0x0000B5AC
	public OptionBinary(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, string connectionAddress, uint port, OptionCategory category, bool replicate)
		: this(SDPCorePINVOKE.new_OptionBinary__SWIG_0(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, connectionAddress, port, OptionCategory.getCPtr(category), replicate), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x0000D3F4 File Offset: 0x0000B5F4
	public OptionBinary(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, string connectionAddress, uint port, OptionCategory category)
		: this(SDPCorePINVOKE.new_OptionBinary__SWIG_1(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, connectionAddress, port, OptionCategory.getCPtr(category)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0000D43C File Offset: 0x0000B63C
	public OptionBinary(CoreObject coreParent, SWIGTYPE_p_SDP__NetCommand cmdNet, string name, string initialValue, string description, uint pid, uint attributes, string connectionAddress, uint port)
		: this(SDPCorePINVOKE.new_OptionBinary__SWIG_2(CoreObject.getCPtr(coreParent), SWIGTYPE_p_SDP__NetCommand.getCPtr(cmdNet), name, initialValue, description, pid, attributes, connectionAddress, port), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x0000D47C File Offset: 0x0000B67C
	public override bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.OptionBinary_SetValue__SWIG_0(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x0000D4A8 File Offset: 0x0000B6A8
	public override bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.OptionBinary_SetValue__SWIG_1(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x0000D4D0 File Offset: 0x0000B6D0
	public override bool SetValue(IntPtr data, uint size, bool publishValue)
	{
		return SDPCorePINVOKE.OptionBinary_SetValue__SWIG_2(this.swigCPtr, data, size, publishValue);
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0000D4F0 File Offset: 0x0000B6F0
	public override bool SetValue(IntPtr data, uint size)
	{
		return SDPCorePINVOKE.OptionBinary_SetValue__SWIG_3(this.swigCPtr, data, size);
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0000D50C File Offset: 0x0000B70C
	public override bool GetValue(IntPtr data, uint size)
	{
		return SDPCorePINVOKE.OptionBinary_GetValue(this.swigCPtr, data, size);
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x0000D528 File Offset: 0x0000B728
	public override uint GetValueSize()
	{
		return SDPCorePINVOKE.OptionBinary_GetValueSize(this.swigCPtr);
	}

	// Token: 0x04000130 RID: 304
	private HandleRef swigCPtr;
}
