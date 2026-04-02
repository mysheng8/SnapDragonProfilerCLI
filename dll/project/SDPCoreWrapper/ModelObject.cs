using System;
using System.Runtime.InteropServices;

// Token: 0x02000046 RID: 70
public class ModelObject : IDisposable
{
	// Token: 0x0600045F RID: 1119 RVA: 0x0000BBCD File Offset: 0x00009DCD
	internal ModelObject(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0000BBE9 File Offset: 0x00009DE9
	internal static HandleRef getCPtr(ModelObject obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0000BC00 File Offset: 0x00009E00
	~ModelObject()
	{
		this.Dispose();
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0000BC2C File Offset: 0x00009E2C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ModelObject(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0000BCAC File Offset: 0x00009EAC
	public static long Error_NoDatabase()
	{
		return SDPCorePINVOKE.ModelObject_Error_NoDatabase();
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0000BCC0 File Offset: 0x00009EC0
	public ModelObjectData NewData()
	{
		return new ModelObjectData(SDPCorePINVOKE.ModelObject_NewData(this.swigCPtr), true);
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0000BCE0 File Offset: 0x00009EE0
	public long Save()
	{
		return SDPCorePINVOKE.ModelObject_Save(this.swigCPtr);
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0000BCFC File Offset: 0x00009EFC
	public bool AddAttribute(string name, SDPDataType dataType, uint dataSize, uint offset)
	{
		bool flag = SDPCorePINVOKE.ModelObject_AddAttribute(this.swigCPtr, name, (int)dataType, dataSize, offset);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0000BD28 File Offset: 0x00009F28
	public bool LinkAttribute(string name, ModelObject modelObj)
	{
		bool flag = SDPCorePINVOKE.ModelObject_LinkAttribute(this.swigCPtr, name, ModelObject.getCPtr(modelObj));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0000BD58 File Offset: 0x00009F58
	public long Insert(IntPtr data)
	{
		return SDPCorePINVOKE.ModelObject_Insert(this.swigCPtr, data);
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0000BD74 File Offset: 0x00009F74
	public ModelObjectDataList GetData(SWIGTYPE_p_std__vectorT_long_long_t idQuery)
	{
		return new ModelObjectDataList(SDPCorePINVOKE.ModelObject_GetData__SWIG_0(this.swigCPtr, SWIGTYPE_p_std__vectorT_long_long_t.getCPtr(idQuery)), true);
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0000BD9C File Offset: 0x00009F9C
	public ModelObjectDataList GetData()
	{
		return new ModelObjectDataList(SDPCorePINVOKE.ModelObject_GetData__SWIG_1(this.swigCPtr), true);
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0000BDBC File Offset: 0x00009FBC
	public ModelObjectData GetDataByID(long id)
	{
		return new ModelObjectData(SDPCorePINVOKE.ModelObject_GetDataByID(this.swigCPtr, id), true);
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0000BDE0 File Offset: 0x00009FE0
	public ModelObjectDataList GetData(string attribute, string value)
	{
		ModelObjectDataList modelObjectDataList = new ModelObjectDataList(SDPCorePINVOKE.ModelObject_GetData__SWIG_2(this.swigCPtr, attribute, value), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectDataList;
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0000BE10 File Offset: 0x0000A010
	public ModelObjectDataList GetData(string attribute)
	{
		ModelObjectDataList modelObjectDataList = new ModelObjectDataList(SDPCorePINVOKE.ModelObject_GetData__SWIG_3(this.swigCPtr, attribute), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectDataList;
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0000BE40 File Offset: 0x0000A040
	public ModelObjectDataList GetData(StringList attributeValuePairs)
	{
		ModelObjectDataList modelObjectDataList = new ModelObjectDataList(SDPCorePINVOKE.ModelObject_GetData__SWIG_4(this.swigCPtr, StringList.getCPtr(attributeValuePairs)), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectDataList;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0000BE74 File Offset: 0x0000A074
	public ModelObjectData GetDataByAttribute(string attribute, string value)
	{
		ModelObjectData modelObjectData = new ModelObjectData(SDPCorePINVOKE.ModelObject_GetDataByAttribute__SWIG_0(this.swigCPtr, attribute, value), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectData;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0000BEA4 File Offset: 0x0000A0A4
	public ModelObjectData GetDataByAttribute(string attribute)
	{
		ModelObjectData modelObjectData = new ModelObjectData(SDPCorePINVOKE.ModelObject_GetDataByAttribute__SWIG_1(this.swigCPtr, attribute), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectData;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0000BED4 File Offset: 0x0000A0D4
	public ModelObjectDataList GetAll()
	{
		return new ModelObjectDataList(SDPCorePINVOKE.ModelObject_GetAll(this.swigCPtr), true);
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0000BEF4 File Offset: 0x0000A0F4
	public static void BeginBatch()
	{
		SDPCorePINVOKE.ModelObject_BeginBatch();
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0000BEFB File Offset: 0x0000A0FB
	public static void CommitBatch()
	{
		SDPCorePINVOKE.ModelObject_CommitBatch();
	}

	// Token: 0x04000122 RID: 290
	private HandleRef swigCPtr;

	// Token: 0x04000123 RID: 291
	protected bool swigCMemOwn;
}
