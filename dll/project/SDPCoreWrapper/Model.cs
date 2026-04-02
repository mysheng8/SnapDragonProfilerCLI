using System;
using System.Runtime.InteropServices;

// Token: 0x02000045 RID: 69
public class Model : IDisposable
{
	// Token: 0x06000456 RID: 1110 RVA: 0x0000BA23 File Offset: 0x00009C23
	internal Model(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0000BA3F File Offset: 0x00009C3F
	internal static HandleRef getCPtr(Model obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0000BA58 File Offset: 0x00009C58
	~Model()
	{
		this.Dispose();
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0000BA84 File Offset: 0x00009C84
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_Model(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0000BB04 File Offset: 0x00009D04
	public string GetName()
	{
		return SDPCorePINVOKE.Model_GetName(this.swigCPtr);
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0000BB20 File Offset: 0x00009D20
	public SWIGTYPE_p_std__recursive_mutex GetMutex()
	{
		return new SWIGTYPE_p_std__recursive_mutex(SDPCorePINVOKE.Model_GetMutex(this.swigCPtr), false);
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x0000BB40 File Offset: 0x00009D40
	public ModelObject AddObject(string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.Model_AddObject(this.swigCPtr, name);
		ModelObject modelObject = ((intPtr == IntPtr.Zero) ? null : new ModelObject(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObject;
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0000BB80 File Offset: 0x00009D80
	public ModelObject GetModelObject(string name)
	{
		IntPtr intPtr = SDPCorePINVOKE.Model_GetModelObject(this.swigCPtr, name);
		ModelObject modelObject = ((intPtr == IntPtr.Zero) ? null : new ModelObject(intPtr, false));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return modelObject;
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0000BBC0 File Offset: 0x00009DC0
	public void RegisterOnModelChanged()
	{
		SDPCorePINVOKE.Model_RegisterOnModelChanged(this.swigCPtr);
	}

	// Token: 0x04000120 RID: 288
	private HandleRef swigCPtr;

	// Token: 0x04000121 RID: 289
	protected bool swigCMemOwn;
}
