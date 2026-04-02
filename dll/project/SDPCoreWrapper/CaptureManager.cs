using System;
using System.Runtime.InteropServices;

// Token: 0x02000015 RID: 21
public class CaptureManager : IDisposable
{
	// Token: 0x06000082 RID: 130 RVA: 0x00002CAC File Offset: 0x00000EAC
	internal CaptureManager(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00002CC8 File Offset: 0x00000EC8
	internal static HandleRef getCPtr(CaptureManager obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00002CE0 File Offset: 0x00000EE0
	~CaptureManager()
	{
		this.Dispose();
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00002D0C File Offset: 0x00000F0C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CaptureManager(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00002D8C File Offset: 0x00000F8C
	public static CaptureManager Get()
	{
		return new CaptureManager(SDPCorePINVOKE.CaptureManager_Get(), false);
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00002DA8 File Offset: 0x00000FA8
	public uint CreateCapture(uint captureType)
	{
		return SDPCorePINVOKE.CaptureManager_CreateCapture(this.swigCPtr, captureType);
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00002DC4 File Offset: 0x00000FC4
	public Capture GetCapture(uint captureID)
	{
		return new Capture(SDPCorePINVOKE.CaptureManager_GetCapture__SWIG_0(this.swigCPtr, captureID), true);
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00002DE8 File Offset: 0x00000FE8
	public Capture GetCapture()
	{
		return new Capture(SDPCorePINVOKE.CaptureManager_GetCapture__SWIG_1(this.swigCPtr), true);
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00002E08 File Offset: 0x00001008
	public void SetCaptureName(uint captureID, string captureName)
	{
		SDPCorePINVOKE.CaptureManager_SetCaptureName(this.swigCPtr, captureID, captureName);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00002E24 File Offset: 0x00001024
	public void RegisterEventDelegate(SWIGTYPE_p_SDP__CaptureManagerDelegate arg0)
	{
		SDPCorePINVOKE.CaptureManager_RegisterEventDelegate(this.swigCPtr, SWIGTYPE_p_SDP__CaptureManagerDelegate.getCPtr(arg0));
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00002E37 File Offset: 0x00001037
	public void UnregisterEventDelegate(SWIGTYPE_p_SDP__CaptureManagerDelegate arg0)
	{
		SDPCorePINVOKE.CaptureManager_UnregisterEventDelegate(this.swigCPtr, SWIGTYPE_p_SDP__CaptureManagerDelegate.getCPtr(arg0));
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00002E4A File Offset: 0x0000104A
	public void ShutDown()
	{
		SDPCorePINVOKE.CaptureManager_ShutDown(this.swigCPtr);
	}

	// Token: 0x04000011 RID: 17
	private HandleRef swigCPtr;

	// Token: 0x04000012 RID: 18
	protected bool swigCMemOwn;
}
