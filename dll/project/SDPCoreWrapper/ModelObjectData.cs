using System;
using System.Runtime.InteropServices;

// Token: 0x02000047 RID: 71
public class ModelObjectData : IDisposable
{
	// Token: 0x06000474 RID: 1140 RVA: 0x0000BF02 File Offset: 0x0000A102
	internal ModelObjectData(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0000BF1E File Offset: 0x0000A11E
	internal static HandleRef getCPtr(ModelObjectData obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0000BF38 File Offset: 0x0000A138
	~ModelObjectData()
	{
		this.Dispose();
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0000BF64 File Offset: 0x0000A164
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ModelObjectData(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0000BFE4 File Offset: 0x0000A1E4
	public ModelObjectData()
		: this(SDPCorePINVOKE.new_ModelObjectData__SWIG_0(), true)
	{
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0000BFF2 File Offset: 0x0000A1F2
	public ModelObjectData(ModelObjectData other)
		: this(SDPCorePINVOKE.new_ModelObjectData__SWIG_1(ModelObjectData.getCPtr(other)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0000C014 File Offset: 0x0000A214
	public ModelObjectData Equal(ModelObjectData rhs)
	{
		ModelObjectData modelObjectData = new ModelObjectData(SDPCorePINVOKE.ModelObjectData_Equal(this.swigCPtr, ModelObjectData.getCPtr(rhs)), false);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectData;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0000C048 File Offset: 0x0000A248
	public long GetID()
	{
		return SDPCorePINVOKE.ModelObjectData_GetID(this.swigCPtr);
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0000C064 File Offset: 0x0000A264
	public bool IsValid()
	{
		return SDPCorePINVOKE.ModelObjectData_IsValid(this.swigCPtr);
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0000C080 File Offset: 0x0000A280
	public long Save()
	{
		return SDPCorePINVOKE.ModelObjectData_Save(this.swigCPtr);
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0000C09C File Offset: 0x0000A29C
	public bool Update()
	{
		return SDPCorePINVOKE.ModelObjectData_Update__SWIG_0(this.swigCPtr);
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
	public bool Update(IntPtr data)
	{
		return SDPCorePINVOKE.ModelObjectData_Update__SWIG_1(this.swigCPtr, data);
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
	public bool SetAttributeValue(string name, string value)
	{
		bool flag = SDPCorePINVOKE.ModelObjectData_SetAttributeValue__SWIG_0(this.swigCPtr, name, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0000C100 File Offset: 0x0000A300
	public bool SetAttributeValue(string name, IntPtr value)
	{
		bool flag = SDPCorePINVOKE.ModelObjectData_SetAttributeValue__SWIG_1(this.swigCPtr, name, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0000C12C File Offset: 0x0000A32C
	public string GetValue(string name)
	{
		string text = SDPCorePINVOKE.ModelObjectData_GetValue(this.swigCPtr, name);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0000C154 File Offset: 0x0000A354
	public SWIGTYPE_p_std__mapT_std__string_std__string_t GetData()
	{
		return new SWIGTYPE_p_std__mapT_std__string_std__string_t(SDPCorePINVOKE.ModelObjectData_GetData(this.swigCPtr), true);
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0000C174 File Offset: 0x0000A374
	public BinaryDataPair GetValuePtrBinaryDataPair(string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.ModelObjectData_GetValuePtrBinaryDataPair(this.swigCPtr, name);
		BinaryDataPair binaryDataPair = ((intPtr == IntPtr.Zero) ? null : new BinaryDataPair(intPtr, true));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return binaryDataPair;
	}

	// Token: 0x04000124 RID: 292
	private HandleRef swigCPtr;

	// Token: 0x04000125 RID: 293
	protected bool swigCMemOwn;
}
