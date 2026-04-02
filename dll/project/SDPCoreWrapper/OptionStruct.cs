using System;
using System.Runtime.InteropServices;

// Token: 0x02000058 RID: 88
public class OptionStruct : Option
{
	// Token: 0x060005B1 RID: 1457 RVA: 0x0000EFC5 File Offset: 0x0000D1C5
	internal OptionStruct(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.OptionStruct_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0000EFE1 File Offset: 0x0000D1E1
	internal static HandleRef getCPtr(OptionStruct obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
	~OptionStruct()
	{
		this.Dispose();
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x0000F024 File Offset: 0x0000D224
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionStruct(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
	public override bool SetValue(OptionStructData data, bool publishValue)
	{
		return SDPCorePINVOKE.OptionStruct_SetValue__SWIG_0(this.swigCPtr, OptionStructData.getCPtr(data), publishValue);
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x0000F0CC File Offset: 0x0000D2CC
	public override bool SetValue(OptionStructData data)
	{
		return SDPCorePINVOKE.OptionStruct_SetValue__SWIG_1(this.swigCPtr, OptionStructData.getCPtr(data));
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0000F0EC File Offset: 0x0000D2EC
	public override bool GetValue(OptionStructData data)
	{
		return SDPCorePINVOKE.OptionStruct_GetValue(this.swigCPtr, OptionStructData.getCPtr(data));
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0000F10C File Offset: 0x0000D30C
	public override OptionStructData GetOptionStructData()
	{
		IntPtr intPtr = SDPCorePINVOKE.OptionStruct_GetOptionStructData(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new OptionStructData(intPtr, false);
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0000F140 File Offset: 0x0000D340
	public virtual OptionStructDef GetOptionStructDef()
	{
		IntPtr intPtr = SDPCorePINVOKE.OptionStruct_GetOptionStructDef(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new OptionStructDef(intPtr, false);
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x0000F174 File Offset: 0x0000D374
	public override bool SetValue(OptionStructDef definition, bool publishValue)
	{
		return SDPCorePINVOKE.OptionStruct_SetValue__SWIG_2(this.swigCPtr, OptionStructDef.getCPtr(definition), publishValue);
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0000F198 File Offset: 0x0000D398
	public override bool SetValue(OptionStructDef definition)
	{
		return SDPCorePINVOKE.OptionStruct_SetValue__SWIG_3(this.swigCPtr, OptionStructDef.getCPtr(definition));
	}

	// Token: 0x0400013C RID: 316
	private HandleRef swigCPtr;
}
