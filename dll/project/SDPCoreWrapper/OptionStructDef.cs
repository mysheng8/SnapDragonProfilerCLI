using System;
using System.Runtime.InteropServices;

// Token: 0x0200005A RID: 90
public class OptionStructDef : IDisposable
{
	// Token: 0x060005CC RID: 1484 RVA: 0x0000F424 File Offset: 0x0000D624
	internal OptionStructDef(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0000F440 File Offset: 0x0000D640
	internal static HandleRef getCPtr(OptionStructDef obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0000F458 File Offset: 0x0000D658
	~OptionStructDef()
	{
		this.Dispose();
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0000F484 File Offset: 0x0000D684
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_OptionStructDef(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0000F504 File Offset: 0x0000D704
	public OptionStructDef(string name)
		: this(SDPCorePINVOKE.new_OptionStructDef(name), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0000F520 File Offset: 0x0000D720
	public uint AddAttribute(string name, SDPDataType dataType)
	{
		uint num = SDPCorePINVOKE.OptionStructDef_AddAttribute__SWIG_0(this.swigCPtr, name, (int)dataType);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0000F54C File Offset: 0x0000D74C
	public uint AddAttribute(string name, SDPDataType dataType, uint offset)
	{
		uint num = SDPCorePINVOKE.OptionStructDef_AddAttribute__SWIG_1(this.swigCPtr, name, (int)dataType, offset);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0000F578 File Offset: 0x0000D778
	public uint GetNumberAttributes()
	{
		return SDPCorePINVOKE.OptionStructDef_GetNumberAttributes(this.swigCPtr);
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0000F594 File Offset: 0x0000D794
	public uint GetAttributeOffset(uint idx)
	{
		return SDPCorePINVOKE.OptionStructDef_GetAttributeOffset(this.swigCPtr, idx);
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0000F5B0 File Offset: 0x0000D7B0
	public SDPDataType GetAttributeType(uint idx)
	{
		return (SDPDataType)SDPCorePINVOKE.OptionStructDef_GetAttributeType(this.swigCPtr, idx);
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0000F5CC File Offset: 0x0000D7CC
	public string GetAttributeName(uint idx)
	{
		return SDPCorePINVOKE.OptionStructDef_GetAttributeName(this.swigCPtr, idx);
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0000F5E8 File Offset: 0x0000D7E8
	public OptionStructData NewData()
	{
		IntPtr intPtr = SDPCorePINVOKE.OptionStructDef_NewData(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new OptionStructData(intPtr, false);
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0000F61A File Offset: 0x0000D81A
	public void DeleteData(OptionStructData data)
	{
		SDPCorePINVOKE.OptionStructDef_DeleteData(this.swigCPtr, OptionStructData.getCPtr(data));
	}

	// Token: 0x0400013F RID: 319
	private HandleRef swigCPtr;

	// Token: 0x04000140 RID: 320
	protected bool swigCMemOwn;
}
