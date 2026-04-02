using System;
using System.Runtime.InteropServices;

// Token: 0x02000059 RID: 89
public class OptionStructData : IDisposable
{
	// Token: 0x060005BC RID: 1468 RVA: 0x0000F1B8 File Offset: 0x0000D3B8
	internal OptionStructData(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0000F1D4 File Offset: 0x0000D3D4
	internal static HandleRef getCPtr(OptionStructData obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x0000F1EC File Offset: 0x0000D3EC
	~OptionStructData()
	{
		this.Dispose();
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x0000F218 File Offset: 0x0000D418
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionStructData(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x0000F298 File Offset: 0x0000D498
	public OptionStructData(OptionStructDef def)
		: this(SDPCorePINVOKE.new_OptionStructData__SWIG_0(OptionStructDef.getCPtr(def)), true)
	{
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0000F2AC File Offset: 0x0000D4AC
	public OptionStructData(Option option)
		: this(SDPCorePINVOKE.new_OptionStructData__SWIG_1(Option.getCPtr(option)), true)
	{
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x0000F2C0 File Offset: 0x0000D4C0
	public OptionStructData()
		: this(SDPCorePINVOKE.new_OptionStructData__SWIG_2(), true)
	{
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x0000F2D0 File Offset: 0x0000D4D0
	public bool SetValue(string name, IntPtr value)
	{
		bool flag = SDPCorePINVOKE.OptionStructData_SetValue__SWIG_0(this.swigCPtr, name, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x0000F2FC File Offset: 0x0000D4FC
	public bool SetValue(uint idx, IntPtr value)
	{
		return SDPCorePINVOKE.OptionStructData_SetValue__SWIG_1(this.swigCPtr, idx, value);
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x0000F318 File Offset: 0x0000D518
	public bool SetValue(string name, string value)
	{
		bool flag = SDPCorePINVOKE.OptionStructData_SetValue__SWIG_2(this.swigCPtr, name, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x0000F344 File Offset: 0x0000D544
	public string GetValue(string name)
	{
		string text = SDPCorePINVOKE.OptionStructData_GetValue__SWIG_0(this.swigCPtr, name);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x0000F36C File Offset: 0x0000D56C
	public bool GetValue(string name, IntPtr value)
	{
		bool flag = SDPCorePINVOKE.OptionStructData_GetValue__SWIG_1(this.swigCPtr, name, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0000F398 File Offset: 0x0000D598
	public bool GetValue(uint idx, IntPtr value)
	{
		return SDPCorePINVOKE.OptionStructData_GetValue__SWIG_2(this.swigCPtr, idx, value);
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0000F3B4 File Offset: 0x0000D5B4
	public uint GetSize()
	{
		return SDPCorePINVOKE.OptionStructData_GetSize(this.swigCPtr);
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0000F3D0 File Offset: 0x0000D5D0
	public OptionStructDef GetDefinition()
	{
		IntPtr intPtr = SDPCorePINVOKE.OptionStructData_GetDefinition(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new OptionStructDef(intPtr, false);
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0000F404 File Offset: 0x0000D604
	public bool Clone(OptionStructData outData)
	{
		return SDPCorePINVOKE.OptionStructData_Clone(this.swigCPtr, OptionStructData.getCPtr(outData));
	}

	// Token: 0x0400013D RID: 317
	private HandleRef swigCPtr;

	// Token: 0x0400013E RID: 318
	protected bool swigCMemOwn;
}
