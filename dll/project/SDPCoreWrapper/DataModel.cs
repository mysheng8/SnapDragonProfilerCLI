using System;
using System.Runtime.InteropServices;

// Token: 0x02000024 RID: 36
public class DataModel : IDisposable
{
	// Token: 0x0600018A RID: 394 RVA: 0x000055C6 File Offset: 0x000037C6
	internal DataModel(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600018B RID: 395 RVA: 0x000055E2 File Offset: 0x000037E2
	internal static HandleRef getCPtr(DataModel obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600018C RID: 396 RVA: 0x000055FC File Offset: 0x000037FC
	~DataModel()
	{
		this.Dispose();
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00005628 File Offset: 0x00003828
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DataModel(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600018E RID: 398 RVA: 0x000056A8 File Offset: 0x000038A8
	public static DataModel Get()
	{
		return new DataModel(SDPCorePINVOKE.DataModel_Get(), false);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x000056C2 File Offset: 0x000038C2
	public void ShutDown()
	{
		SDPCorePINVOKE.DataModel_ShutDown(this.swigCPtr);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x000056D0 File Offset: 0x000038D0
	public Model AddModel(string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataModel_AddModel(this.swigCPtr, name);
		Model model = ((intPtr == IntPtr.Zero) ? null : new Model(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return model;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x00005710 File Offset: 0x00003910
	public void DeleteModel(string name)
	{
		SDPCorePINVOKE.DataModel_DeleteModel(this.swigCPtr, name);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000572C File Offset: 0x0000392C
	public Model GetModel(string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataModel_GetModel(this.swigCPtr, name);
		Model model = ((intPtr == IntPtr.Zero) ? null : new Model(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return model;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000576C File Offset: 0x0000396C
	public ModelObject GetModelObject(Model model, string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.DataModel_GetModelObject(this.swigCPtr, Model.getCPtr(model), name);
		ModelObject modelObject = ((intPtr == IntPtr.Zero) ? null : new ModelObject(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObject;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x000057B4 File Offset: 0x000039B4
	public ModelObjectData GetModelObjectData(ModelObject modelObject, long id)
	{
		return new ModelObjectData(SDPCorePINVOKE.DataModel_GetModelObjectData__SWIG_0(this.swigCPtr, ModelObject.getCPtr(modelObject), id), true);
	}

	// Token: 0x06000195 RID: 405 RVA: 0x000057DC File Offset: 0x000039DC
	public ModelObjectDataList GetModelObjectData(ModelObject modelObject, string attribute, string value)
	{
		ModelObjectDataList modelObjectDataList = new ModelObjectDataList(SDPCorePINVOKE.DataModel_GetModelObjectData__SWIG_1(this.swigCPtr, ModelObject.getCPtr(modelObject), attribute, value), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObjectDataList;
	}

	// Token: 0x04000062 RID: 98
	private HandleRef swigCPtr;

	// Token: 0x04000063 RID: 99
	protected bool swigCMemOwn;
}
