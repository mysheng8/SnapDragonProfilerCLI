using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x02000010 RID: 16
public class EGLAdapter : Adapter
{
	// Token: 0x060000AC RID: 172 RVA: 0x0000332B File Offset: 0x0000152B
	internal EGLAdapter(IntPtr cPtr, bool cMemoryOwn)
		: base(libDCAPPINVOKE.EGLAdapter_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00003347 File Offset: 0x00001547
	internal static HandleRef getCPtr(EGLAdapter obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00003360 File Offset: 0x00001560
	~EGLAdapter()
	{
		this.Dispose();
	}

	// Token: 0x060000AF RID: 175 RVA: 0x0000338C File Offset: 0x0000158C
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_EGLAdapter(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00003410 File Offset: 0x00001610
	public EGLAdapter()
		: this(libDCAPPINVOKE.new_EGLAdapter(), true)
	{
		this.SwigDirectorConnect();
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00003424 File Offset: 0x00001624
	public override void SetCurrentThread(uint id)
	{
		libDCAPPINVOKE.EGLAdapter_SetCurrentThread(this.swigCPtr, id);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00003432 File Offset: 0x00001632
	public virtual void Process_eglGetError(int returnVal)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetError(this.swigCPtr, returnVal);
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00003440 File Offset: 0x00001640
	public virtual void Process_eglGetDisplay(uint returnVal, uint nativeDisplay)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetDisplay(this.swigCPtr, returnVal, nativeDisplay);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x0000344F File Offset: 0x0000164F
	public virtual void Process_eglInitialize(int returnVal, uint display, PointerData pMajorPtrData, PointerData pMinorPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglInitialize(this.swigCPtr, returnVal, display, PointerData.getCPtr(pMajorPtrData), PointerData.getCPtr(pMinorPtrData));
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x0000346B File Offset: 0x0000166B
	public virtual void Process_eglTerminate(int returnVal, uint display)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglTerminate(this.swigCPtr, returnVal, display);
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000347A File Offset: 0x0000167A
	public virtual void Process_eglQueryString(PointerData pReturnPtrData, uint display, int name)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglQueryString(this.swigCPtr, PointerData.getCPtr(pReturnPtrData), display, name);
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x0000348F File Offset: 0x0000168F
	public virtual void Process_eglGetConfigs(int returnVal, uint display, PointerData pConfigsPtrData, int configSize, PointerData pNumConfigPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetConfigs(this.swigCPtr, returnVal, display, PointerData.getCPtr(pConfigsPtrData), configSize, PointerData.getCPtr(pNumConfigPtrData));
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000034AD File Offset: 0x000016AD
	public virtual void Process_eglChooseConfig(int returnVal, uint display, PointerData pAttribListPtrData, PointerData pConfigsPtrData, int configSize, PointerData pNumConfigPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglChooseConfig(this.swigCPtr, returnVal, display, PointerData.getCPtr(pAttribListPtrData), PointerData.getCPtr(pConfigsPtrData), configSize, PointerData.getCPtr(pNumConfigPtrData));
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000034D2 File Offset: 0x000016D2
	public virtual void Process_eglGetConfigAttrib(int returnVal, uint display, uint config, int attribute, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetConfigAttrib(this.swigCPtr, returnVal, display, config, attribute, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000034EB File Offset: 0x000016EB
	public virtual void Process_eglCreateWindowSurface(uint returnVal, uint display, uint config, uint nativeWindow, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreateWindowSurface(this.swigCPtr, returnVal, display, config, nativeWindow, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00003504 File Offset: 0x00001704
	public virtual void Process_eglCreatePbufferSurface(uint returnVal, uint display, uint config, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreatePbufferSurface(this.swigCPtr, returnVal, display, config, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000351B File Offset: 0x0000171B
	public virtual void Process_eglCreatePixmapSurface(uint returnVal, uint display, uint config, uint nativePixmap, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreatePixmapSurface(this.swigCPtr, returnVal, display, config, nativePixmap, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00003534 File Offset: 0x00001734
	public virtual void Process_eglDestroySurface(int returnVal, uint display, uint surface)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglDestroySurface(this.swigCPtr, returnVal, display, surface);
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00003544 File Offset: 0x00001744
	public virtual void Process_eglQuerySurface(int returnVal, uint display, uint surface, int attribute, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglQuerySurface(this.swigCPtr, returnVal, display, surface, attribute, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060000BF RID: 191 RVA: 0x0000355D File Offset: 0x0000175D
	public virtual void Process_eglBindAPI(int returnVal, uint api)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglBindAPI(this.swigCPtr, returnVal, api);
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000356C File Offset: 0x0000176C
	public virtual void Process_eglQueryAPI(uint returnVal)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglQueryAPI(this.swigCPtr, returnVal);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000357A File Offset: 0x0000177A
	public virtual void Process_eglWaitClient(int returnVal)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglWaitClient(this.swigCPtr, returnVal);
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00003588 File Offset: 0x00001788
	public virtual void Process_eglReleaseThread(int returnVal)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglReleaseThread(this.swigCPtr, returnVal);
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00003596 File Offset: 0x00001796
	public virtual void Process_eglCreatePbufferFromClientBuffer(uint returnVal, uint display, uint buftype, uint buffer, uint config, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreatePbufferFromClientBuffer(this.swigCPtr, returnVal, display, buftype, buffer, config, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x000035B1 File Offset: 0x000017B1
	public virtual void Process_eglSurfaceAttrib(int returnVal, uint display, uint surface, int attribute, int value)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglSurfaceAttrib(this.swigCPtr, returnVal, display, surface, attribute, value);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x000035C5 File Offset: 0x000017C5
	public virtual void Process_eglBindTexImage(int returnVal, uint display, uint surface, int buffer)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglBindTexImage(this.swigCPtr, returnVal, display, surface, buffer);
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000035D7 File Offset: 0x000017D7
	public virtual void Process_eglReleaseTexImage(int returnVal, uint display, uint surface, int buffer)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglReleaseTexImage(this.swigCPtr, returnVal, display, surface, buffer);
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x000035E9 File Offset: 0x000017E9
	public virtual void Process_eglSwapInterval(int returnVal, uint display, int interval)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglSwapInterval(this.swigCPtr, returnVal, display, interval);
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000035F9 File Offset: 0x000017F9
	public virtual void Process_eglCreateContext(uint returnVal, uint display, uint config, uint shareContext, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreateContext(this.swigCPtr, returnVal, display, config, shareContext, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00003612 File Offset: 0x00001812
	public virtual void Process_eglDestroyContext(int returnVal, uint display, uint context)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglDestroyContext(this.swigCPtr, returnVal, display, context);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00003622 File Offset: 0x00001822
	public virtual void Process_eglMakeCurrent(int returnVal, uint display, uint drawSurface, uint readSurface, uint context)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglMakeCurrent(this.swigCPtr, returnVal, display, drawSurface, readSurface, context);
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00003636 File Offset: 0x00001836
	public virtual void Process_eglGetCurrentContext(uint returnVal)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetCurrentContext(this.swigCPtr, returnVal);
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00003644 File Offset: 0x00001844
	public virtual void Process_eglGetCurrentSurface(uint returnVal, int readdraw)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetCurrentSurface(this.swigCPtr, returnVal, readdraw);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00003653 File Offset: 0x00001853
	public virtual void Process_eglGetCurrentDisplay(uint returnVal)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetCurrentDisplay(this.swigCPtr, returnVal);
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00003661 File Offset: 0x00001861
	public virtual void Process_eglQueryContext(int returnVal, uint display, uint context, int attribute, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglQueryContext(this.swigCPtr, returnVal, display, context, attribute, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0000367A File Offset: 0x0000187A
	public virtual void Process_eglWaitGL(int returnVal)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglWaitGL(this.swigCPtr, returnVal);
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00003688 File Offset: 0x00001888
	public virtual void Process_eglWaitNative(int returnVal, int engine)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglWaitNative(this.swigCPtr, returnVal, engine);
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00003697 File Offset: 0x00001897
	public virtual void Process_eglSwapBuffers(int returnVal, uint display, uint surface)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglSwapBuffers(this.swigCPtr, returnVal, display, surface);
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x000036A7 File Offset: 0x000018A7
	public virtual void Process_eglCopyBuffers(int returnVal, uint display, uint surface, uint nativePixmap)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCopyBuffers(this.swigCPtr, returnVal, display, surface, nativePixmap);
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x000036B9 File Offset: 0x000018B9
	public virtual void Process_eglGetProcAddress(PointerData pReturnPtrData, PointerData pProcnamePtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetProcAddress(this.swigCPtr, PointerData.getCPtr(pReturnPtrData), PointerData.getCPtr(pProcnamePtrData));
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x000036D2 File Offset: 0x000018D2
	public virtual void Process_eglCreateImageKHR(uint returnVal, uint display, uint context, uint target, uint buffer, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreateImageKHR(this.swigCPtr, returnVal, display, context, target, buffer, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x000036ED File Offset: 0x000018ED
	public virtual void Process_eglDestroyImageKHR(int returnVal, uint display, uint image)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglDestroyImageKHR(this.swigCPtr, returnVal, display, image);
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x000036FD File Offset: 0x000018FD
	public virtual void Process_eglLockImageQCOM(int returnVal, uint display, uint image, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglLockImageQCOM(this.swigCPtr, returnVal, display, image, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00003714 File Offset: 0x00001914
	public virtual void Process_eglUnlockImageQCOM(int returnVal, uint display, uint image)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglUnlockImageQCOM(this.swigCPtr, returnVal, display, image);
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00003724 File Offset: 0x00001924
	public virtual void Process_eglQueryImageQCOM(int returnVal, uint display, uint image, int attribute, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglQueryImageQCOM(this.swigCPtr, returnVal, display, image, attribute, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x0000373D File Offset: 0x0000193D
	public virtual void Process_eglQueryImage64QCOM(int returnVal, uint display, uint image, int attribute, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglQueryImage64QCOM(this.swigCPtr, returnVal, display, image, attribute, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00003756 File Offset: 0x00001956
	public virtual void Process_eglSetBlobCacheFuncsANDROID(uint display, PointerData pSetFuncPtrData, PointerData pGetFuncPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglSetBlobCacheFuncsANDROID(this.swigCPtr, display, PointerData.getCPtr(pSetFuncPtrData), PointerData.getCPtr(pGetFuncPtrData));
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00003770 File Offset: 0x00001970
	public virtual void Process_eglCreateSync(uint returnVal, uint display, uint synctype, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreateSync(this.swigCPtr, returnVal, display, synctype, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00003787 File Offset: 0x00001987
	public virtual void Process_eglCreateSyncKHR(uint returnVal, uint display, uint synctype, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreateSyncKHR(this.swigCPtr, returnVal, display, synctype, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000379E File Offset: 0x0000199E
	public virtual void Process_eglCreateSync64KHR(uint returnVal, uint display, uint synctype, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreateSync64KHR(this.swigCPtr, returnVal, display, synctype, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000DE RID: 222 RVA: 0x000037B5 File Offset: 0x000019B5
	public virtual void Process_eglDestroySyncKHR(int returnVal, uint display, uint sync)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglDestroySyncKHR(this.swigCPtr, returnVal, display, sync);
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000037C5 File Offset: 0x000019C5
	public virtual void Process_eglClientWaitSyncKHR(int returnVal, uint display, uint sync, int flags, ulong timeout)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglClientWaitSyncKHR(this.swigCPtr, returnVal, display, sync, flags, timeout);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000037D9 File Offset: 0x000019D9
	public virtual void Process_eglWaitSyncKHR(int returnVal, uint display, uint sync, int flags)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglWaitSyncKHR(this.swigCPtr, returnVal, display, sync, flags);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x000037EB File Offset: 0x000019EB
	public virtual void Process_eglSignalSyncKHR(int returnVal, uint display, uint sync, uint mode)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglSignalSyncKHR(this.swigCPtr, returnVal, display, sync, mode);
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x000037FD File Offset: 0x000019FD
	public virtual void Process_eglGetSyncAttrib(int returnVal, uint display, uint sync, int attribute, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetSyncAttrib(this.swigCPtr, returnVal, display, sync, attribute, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00003816 File Offset: 0x00001A16
	public virtual void Process_eglDupNativeFenceFDANDROID(int returnVal, uint display, uint sync)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglDupNativeFenceFDANDROID(this.swigCPtr, returnVal, display, sync);
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00003826 File Offset: 0x00001A26
	public virtual void Process_eglGetSyncObjFromEglSyncQCOM(int returnVal, uint display, uint sync, uint syncObj)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetSyncObjFromEglSyncQCOM(this.swigCPtr, returnVal, display, sync, syncObj);
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00003838 File Offset: 0x00001A38
	public virtual void Process_eglLockSurfaceKHR(int returnVal, uint display, uint surface, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglLockSurfaceKHR(this.swigCPtr, returnVal, display, surface, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0000384F File Offset: 0x00001A4F
	public virtual void Process_eglUnlockSurfaceKHR(int returnVal, uint display, uint surface)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglUnlockSurfaceKHR(this.swigCPtr, returnVal, display, surface);
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x0000385F File Offset: 0x00001A5F
	public virtual void Process_eglQuerySurface64KHR(int returnVal, uint display, uint surface, int attribute, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglQuerySurface64KHR(this.swigCPtr, returnVal, display, surface, attribute, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00003878 File Offset: 0x00001A78
	public virtual void Process_eglGpuPerfHintQCOM(int returnVal, uint display, uint context, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGpuPerfHintQCOM(this.swigCPtr, returnVal, display, context, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0000388F File Offset: 0x00001A8F
	public virtual void Process_eglSetDamageRegionKHR(int returnVal, uint display, uint surface, PointerData pRectsPtrData, int numRects)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglSetDamageRegionKHR(this.swigCPtr, returnVal, display, surface, PointerData.getCPtr(pRectsPtrData), numRects);
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000038A8 File Offset: 0x00001AA8
	public virtual void Process_eglGetPlatformDisplay(uint returnVal, uint platform, uint nativeDisplay, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetPlatformDisplay(this.swigCPtr, returnVal, platform, nativeDisplay, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000038BF File Offset: 0x00001ABF
	public virtual void Process_eglCreatePlatformWindowSurface(uint returnVal, uint display, uint config, uint nativeWindow, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreatePlatformWindowSurface(this.swigCPtr, returnVal, display, config, nativeWindow, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000EC RID: 236 RVA: 0x000038D8 File Offset: 0x00001AD8
	public virtual void Process_eglCreatePlatformPixmapSurface(uint returnVal, uint display, uint config, uint nativePixmap, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreatePlatformPixmapSurface(this.swigCPtr, returnVal, display, config, nativePixmap, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000ED RID: 237 RVA: 0x000038F1 File Offset: 0x00001AF1
	public virtual void Process_eglGetSyncAttribKHR(int returnVal, uint display, uint sync, int attribute, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglGetSyncAttribKHR(this.swigCPtr, returnVal, display, sync, attribute, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0000390A File Offset: 0x00001B0A
	public virtual void Process_eglQueryDmaBufFormatsEXT(int returnVal, uint display, int maxFormats, PointerData pFormatsPtrData, PointerData pNumFormatsPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglQueryDmaBufFormatsEXT(this.swigCPtr, returnVal, display, maxFormats, PointerData.getCPtr(pFormatsPtrData), PointerData.getCPtr(pNumFormatsPtrData));
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00003928 File Offset: 0x00001B28
	public virtual void Process_eglQueryDmaBufModifiersEXT(int returnVal, uint display, int format, int maxModifiers, PointerData pModifiersPtrData, PointerData pExternalOnlyPtrData, PointerData pNumModifiersPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglQueryDmaBufModifiersEXT(this.swigCPtr, returnVal, display, format, maxModifiers, PointerData.getCPtr(pModifiersPtrData), PointerData.getCPtr(pExternalOnlyPtrData), PointerData.getCPtr(pNumModifiersPtrData));
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0000394F File Offset: 0x00001B4F
	public virtual void Process_eglExportDMABUFImageQueryMESA(int returnVal, uint display, uint image, PointerData pFourccPtrData, PointerData pNumPlanesPtrData, PointerData pModifiersPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglExportDMABUFImageQueryMESA(this.swigCPtr, returnVal, display, image, PointerData.getCPtr(pFourccPtrData), PointerData.getCPtr(pNumPlanesPtrData), PointerData.getCPtr(pModifiersPtrData));
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00003974 File Offset: 0x00001B74
	public virtual void Process_eglExportDMABUFImageMESA(int returnVal, uint display, uint image, PointerData pFdsPtrData, PointerData pStridesPtrData, PointerData pOffsetsPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglExportDMABUFImageMESA(this.swigCPtr, returnVal, display, image, PointerData.getCPtr(pFdsPtrData), PointerData.getCPtr(pStridesPtrData), PointerData.getCPtr(pOffsetsPtrData));
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00003999 File Offset: 0x00001B99
	public virtual void Process_eglCreateImageKHRv1(uint returnVal, uint display, uint context, uint target, uint buffer, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.EGLAdapter_Process_eglCreateImageKHRv1(this.swigCPtr, returnVal, display, context, target, buffer, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x000039B4 File Offset: 0x00001BB4
	private void SwigDirectorConnect()
	{
		if (this.SwigDerivedClassHasMethod("SetCurrentThread", EGLAdapter.swigMethodTypes0))
		{
			this.swigDelegate0 = new EGLAdapter.SwigDelegateEGLAdapter_0(this.SwigDirectorSetCurrentThread);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetError", EGLAdapter.swigMethodTypes1))
		{
			this.swigDelegate1 = new EGLAdapter.SwigDelegateEGLAdapter_1(this.SwigDirectorProcess_eglGetError);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetDisplay", EGLAdapter.swigMethodTypes2))
		{
			this.swigDelegate2 = new EGLAdapter.SwigDelegateEGLAdapter_2(this.SwigDirectorProcess_eglGetDisplay);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglInitialize", EGLAdapter.swigMethodTypes3))
		{
			this.swigDelegate3 = new EGLAdapter.SwigDelegateEGLAdapter_3(this.SwigDirectorProcess_eglInitialize);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglTerminate", EGLAdapter.swigMethodTypes4))
		{
			this.swigDelegate4 = new EGLAdapter.SwigDelegateEGLAdapter_4(this.SwigDirectorProcess_eglTerminate);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglQueryString", EGLAdapter.swigMethodTypes5))
		{
			this.swigDelegate5 = new EGLAdapter.SwigDelegateEGLAdapter_5(this.SwigDirectorProcess_eglQueryString);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetConfigs", EGLAdapter.swigMethodTypes6))
		{
			this.swigDelegate6 = new EGLAdapter.SwigDelegateEGLAdapter_6(this.SwigDirectorProcess_eglGetConfigs);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglChooseConfig", EGLAdapter.swigMethodTypes7))
		{
			this.swigDelegate7 = new EGLAdapter.SwigDelegateEGLAdapter_7(this.SwigDirectorProcess_eglChooseConfig);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetConfigAttrib", EGLAdapter.swigMethodTypes8))
		{
			this.swigDelegate8 = new EGLAdapter.SwigDelegateEGLAdapter_8(this.SwigDirectorProcess_eglGetConfigAttrib);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreateWindowSurface", EGLAdapter.swigMethodTypes9))
		{
			this.swigDelegate9 = new EGLAdapter.SwigDelegateEGLAdapter_9(this.SwigDirectorProcess_eglCreateWindowSurface);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreatePbufferSurface", EGLAdapter.swigMethodTypes10))
		{
			this.swigDelegate10 = new EGLAdapter.SwigDelegateEGLAdapter_10(this.SwigDirectorProcess_eglCreatePbufferSurface);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreatePixmapSurface", EGLAdapter.swigMethodTypes11))
		{
			this.swigDelegate11 = new EGLAdapter.SwigDelegateEGLAdapter_11(this.SwigDirectorProcess_eglCreatePixmapSurface);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglDestroySurface", EGLAdapter.swigMethodTypes12))
		{
			this.swigDelegate12 = new EGLAdapter.SwigDelegateEGLAdapter_12(this.SwigDirectorProcess_eglDestroySurface);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglQuerySurface", EGLAdapter.swigMethodTypes13))
		{
			this.swigDelegate13 = new EGLAdapter.SwigDelegateEGLAdapter_13(this.SwigDirectorProcess_eglQuerySurface);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglBindAPI", EGLAdapter.swigMethodTypes14))
		{
			this.swigDelegate14 = new EGLAdapter.SwigDelegateEGLAdapter_14(this.SwigDirectorProcess_eglBindAPI);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglQueryAPI", EGLAdapter.swigMethodTypes15))
		{
			this.swigDelegate15 = new EGLAdapter.SwigDelegateEGLAdapter_15(this.SwigDirectorProcess_eglQueryAPI);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglWaitClient", EGLAdapter.swigMethodTypes16))
		{
			this.swigDelegate16 = new EGLAdapter.SwigDelegateEGLAdapter_16(this.SwigDirectorProcess_eglWaitClient);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglReleaseThread", EGLAdapter.swigMethodTypes17))
		{
			this.swigDelegate17 = new EGLAdapter.SwigDelegateEGLAdapter_17(this.SwigDirectorProcess_eglReleaseThread);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreatePbufferFromClientBuffer", EGLAdapter.swigMethodTypes18))
		{
			this.swigDelegate18 = new EGLAdapter.SwigDelegateEGLAdapter_18(this.SwigDirectorProcess_eglCreatePbufferFromClientBuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglSurfaceAttrib", EGLAdapter.swigMethodTypes19))
		{
			this.swigDelegate19 = new EGLAdapter.SwigDelegateEGLAdapter_19(this.SwigDirectorProcess_eglSurfaceAttrib);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglBindTexImage", EGLAdapter.swigMethodTypes20))
		{
			this.swigDelegate20 = new EGLAdapter.SwigDelegateEGLAdapter_20(this.SwigDirectorProcess_eglBindTexImage);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglReleaseTexImage", EGLAdapter.swigMethodTypes21))
		{
			this.swigDelegate21 = new EGLAdapter.SwigDelegateEGLAdapter_21(this.SwigDirectorProcess_eglReleaseTexImage);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglSwapInterval", EGLAdapter.swigMethodTypes22))
		{
			this.swigDelegate22 = new EGLAdapter.SwigDelegateEGLAdapter_22(this.SwigDirectorProcess_eglSwapInterval);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreateContext", EGLAdapter.swigMethodTypes23))
		{
			this.swigDelegate23 = new EGLAdapter.SwigDelegateEGLAdapter_23(this.SwigDirectorProcess_eglCreateContext);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglDestroyContext", EGLAdapter.swigMethodTypes24))
		{
			this.swigDelegate24 = new EGLAdapter.SwigDelegateEGLAdapter_24(this.SwigDirectorProcess_eglDestroyContext);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglMakeCurrent", EGLAdapter.swigMethodTypes25))
		{
			this.swigDelegate25 = new EGLAdapter.SwigDelegateEGLAdapter_25(this.SwigDirectorProcess_eglMakeCurrent);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetCurrentContext", EGLAdapter.swigMethodTypes26))
		{
			this.swigDelegate26 = new EGLAdapter.SwigDelegateEGLAdapter_26(this.SwigDirectorProcess_eglGetCurrentContext);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetCurrentSurface", EGLAdapter.swigMethodTypes27))
		{
			this.swigDelegate27 = new EGLAdapter.SwigDelegateEGLAdapter_27(this.SwigDirectorProcess_eglGetCurrentSurface);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetCurrentDisplay", EGLAdapter.swigMethodTypes28))
		{
			this.swigDelegate28 = new EGLAdapter.SwigDelegateEGLAdapter_28(this.SwigDirectorProcess_eglGetCurrentDisplay);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglQueryContext", EGLAdapter.swigMethodTypes29))
		{
			this.swigDelegate29 = new EGLAdapter.SwigDelegateEGLAdapter_29(this.SwigDirectorProcess_eglQueryContext);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglWaitGL", EGLAdapter.swigMethodTypes30))
		{
			this.swigDelegate30 = new EGLAdapter.SwigDelegateEGLAdapter_30(this.SwigDirectorProcess_eglWaitGL);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglWaitNative", EGLAdapter.swigMethodTypes31))
		{
			this.swigDelegate31 = new EGLAdapter.SwigDelegateEGLAdapter_31(this.SwigDirectorProcess_eglWaitNative);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglSwapBuffers", EGLAdapter.swigMethodTypes32))
		{
			this.swigDelegate32 = new EGLAdapter.SwigDelegateEGLAdapter_32(this.SwigDirectorProcess_eglSwapBuffers);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCopyBuffers", EGLAdapter.swigMethodTypes33))
		{
			this.swigDelegate33 = new EGLAdapter.SwigDelegateEGLAdapter_33(this.SwigDirectorProcess_eglCopyBuffers);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetProcAddress", EGLAdapter.swigMethodTypes34))
		{
			this.swigDelegate34 = new EGLAdapter.SwigDelegateEGLAdapter_34(this.SwigDirectorProcess_eglGetProcAddress);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreateImageKHR", EGLAdapter.swigMethodTypes35))
		{
			this.swigDelegate35 = new EGLAdapter.SwigDelegateEGLAdapter_35(this.SwigDirectorProcess_eglCreateImageKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglDestroyImageKHR", EGLAdapter.swigMethodTypes36))
		{
			this.swigDelegate36 = new EGLAdapter.SwigDelegateEGLAdapter_36(this.SwigDirectorProcess_eglDestroyImageKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglLockImageQCOM", EGLAdapter.swigMethodTypes37))
		{
			this.swigDelegate37 = new EGLAdapter.SwigDelegateEGLAdapter_37(this.SwigDirectorProcess_eglLockImageQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglUnlockImageQCOM", EGLAdapter.swigMethodTypes38))
		{
			this.swigDelegate38 = new EGLAdapter.SwigDelegateEGLAdapter_38(this.SwigDirectorProcess_eglUnlockImageQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglQueryImageQCOM", EGLAdapter.swigMethodTypes39))
		{
			this.swigDelegate39 = new EGLAdapter.SwigDelegateEGLAdapter_39(this.SwigDirectorProcess_eglQueryImageQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglQueryImage64QCOM", EGLAdapter.swigMethodTypes40))
		{
			this.swigDelegate40 = new EGLAdapter.SwigDelegateEGLAdapter_40(this.SwigDirectorProcess_eglQueryImage64QCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglSetBlobCacheFuncsANDROID", EGLAdapter.swigMethodTypes41))
		{
			this.swigDelegate41 = new EGLAdapter.SwigDelegateEGLAdapter_41(this.SwigDirectorProcess_eglSetBlobCacheFuncsANDROID);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreateSync", EGLAdapter.swigMethodTypes42))
		{
			this.swigDelegate42 = new EGLAdapter.SwigDelegateEGLAdapter_42(this.SwigDirectorProcess_eglCreateSync);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreateSyncKHR", EGLAdapter.swigMethodTypes43))
		{
			this.swigDelegate43 = new EGLAdapter.SwigDelegateEGLAdapter_43(this.SwigDirectorProcess_eglCreateSyncKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreateSync64KHR", EGLAdapter.swigMethodTypes44))
		{
			this.swigDelegate44 = new EGLAdapter.SwigDelegateEGLAdapter_44(this.SwigDirectorProcess_eglCreateSync64KHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglDestroySyncKHR", EGLAdapter.swigMethodTypes45))
		{
			this.swigDelegate45 = new EGLAdapter.SwigDelegateEGLAdapter_45(this.SwigDirectorProcess_eglDestroySyncKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglClientWaitSyncKHR", EGLAdapter.swigMethodTypes46))
		{
			this.swigDelegate46 = new EGLAdapter.SwigDelegateEGLAdapter_46(this.SwigDirectorProcess_eglClientWaitSyncKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglWaitSyncKHR", EGLAdapter.swigMethodTypes47))
		{
			this.swigDelegate47 = new EGLAdapter.SwigDelegateEGLAdapter_47(this.SwigDirectorProcess_eglWaitSyncKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglSignalSyncKHR", EGLAdapter.swigMethodTypes48))
		{
			this.swigDelegate48 = new EGLAdapter.SwigDelegateEGLAdapter_48(this.SwigDirectorProcess_eglSignalSyncKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetSyncAttrib", EGLAdapter.swigMethodTypes49))
		{
			this.swigDelegate49 = new EGLAdapter.SwigDelegateEGLAdapter_49(this.SwigDirectorProcess_eglGetSyncAttrib);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglDupNativeFenceFDANDROID", EGLAdapter.swigMethodTypes50))
		{
			this.swigDelegate50 = new EGLAdapter.SwigDelegateEGLAdapter_50(this.SwigDirectorProcess_eglDupNativeFenceFDANDROID);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetSyncObjFromEglSyncQCOM", EGLAdapter.swigMethodTypes51))
		{
			this.swigDelegate51 = new EGLAdapter.SwigDelegateEGLAdapter_51(this.SwigDirectorProcess_eglGetSyncObjFromEglSyncQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglLockSurfaceKHR", EGLAdapter.swigMethodTypes52))
		{
			this.swigDelegate52 = new EGLAdapter.SwigDelegateEGLAdapter_52(this.SwigDirectorProcess_eglLockSurfaceKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglUnlockSurfaceKHR", EGLAdapter.swigMethodTypes53))
		{
			this.swigDelegate53 = new EGLAdapter.SwigDelegateEGLAdapter_53(this.SwigDirectorProcess_eglUnlockSurfaceKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglQuerySurface64KHR", EGLAdapter.swigMethodTypes54))
		{
			this.swigDelegate54 = new EGLAdapter.SwigDelegateEGLAdapter_54(this.SwigDirectorProcess_eglQuerySurface64KHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGpuPerfHintQCOM", EGLAdapter.swigMethodTypes55))
		{
			this.swigDelegate55 = new EGLAdapter.SwigDelegateEGLAdapter_55(this.SwigDirectorProcess_eglGpuPerfHintQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglSetDamageRegionKHR", EGLAdapter.swigMethodTypes56))
		{
			this.swigDelegate56 = new EGLAdapter.SwigDelegateEGLAdapter_56(this.SwigDirectorProcess_eglSetDamageRegionKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetPlatformDisplay", EGLAdapter.swigMethodTypes57))
		{
			this.swigDelegate57 = new EGLAdapter.SwigDelegateEGLAdapter_57(this.SwigDirectorProcess_eglGetPlatformDisplay);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreatePlatformWindowSurface", EGLAdapter.swigMethodTypes58))
		{
			this.swigDelegate58 = new EGLAdapter.SwigDelegateEGLAdapter_58(this.SwigDirectorProcess_eglCreatePlatformWindowSurface);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreatePlatformPixmapSurface", EGLAdapter.swigMethodTypes59))
		{
			this.swigDelegate59 = new EGLAdapter.SwigDelegateEGLAdapter_59(this.SwigDirectorProcess_eglCreatePlatformPixmapSurface);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglGetSyncAttribKHR", EGLAdapter.swigMethodTypes60))
		{
			this.swigDelegate60 = new EGLAdapter.SwigDelegateEGLAdapter_60(this.SwigDirectorProcess_eglGetSyncAttribKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglQueryDmaBufFormatsEXT", EGLAdapter.swigMethodTypes61))
		{
			this.swigDelegate61 = new EGLAdapter.SwigDelegateEGLAdapter_61(this.SwigDirectorProcess_eglQueryDmaBufFormatsEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglQueryDmaBufModifiersEXT", EGLAdapter.swigMethodTypes62))
		{
			this.swigDelegate62 = new EGLAdapter.SwigDelegateEGLAdapter_62(this.SwigDirectorProcess_eglQueryDmaBufModifiersEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglExportDMABUFImageQueryMESA", EGLAdapter.swigMethodTypes63))
		{
			this.swigDelegate63 = new EGLAdapter.SwigDelegateEGLAdapter_63(this.SwigDirectorProcess_eglExportDMABUFImageQueryMESA);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglExportDMABUFImageMESA", EGLAdapter.swigMethodTypes64))
		{
			this.swigDelegate64 = new EGLAdapter.SwigDelegateEGLAdapter_64(this.SwigDirectorProcess_eglExportDMABUFImageMESA);
		}
		if (this.SwigDerivedClassHasMethod("Process_eglCreateImageKHRv1", EGLAdapter.swigMethodTypes65))
		{
			this.swigDelegate65 = new EGLAdapter.SwigDelegateEGLAdapter_65(this.SwigDirectorProcess_eglCreateImageKHRv1);
		}
		libDCAPPINVOKE.EGLAdapter_director_connect(this.swigCPtr, this.swigDelegate0, this.swigDelegate1, this.swigDelegate2, this.swigDelegate3, this.swigDelegate4, this.swigDelegate5, this.swigDelegate6, this.swigDelegate7, this.swigDelegate8, this.swigDelegate9, this.swigDelegate10, this.swigDelegate11, this.swigDelegate12, this.swigDelegate13, this.swigDelegate14, this.swigDelegate15, this.swigDelegate16, this.swigDelegate17, this.swigDelegate18, this.swigDelegate19, this.swigDelegate20, this.swigDelegate21, this.swigDelegate22, this.swigDelegate23, this.swigDelegate24, this.swigDelegate25, this.swigDelegate26, this.swigDelegate27, this.swigDelegate28, this.swigDelegate29, this.swigDelegate30, this.swigDelegate31, this.swigDelegate32, this.swigDelegate33, this.swigDelegate34, this.swigDelegate35, this.swigDelegate36, this.swigDelegate37, this.swigDelegate38, this.swigDelegate39, this.swigDelegate40, this.swigDelegate41, this.swigDelegate42, this.swigDelegate43, this.swigDelegate44, this.swigDelegate45, this.swigDelegate46, this.swigDelegate47, this.swigDelegate48, this.swigDelegate49, this.swigDelegate50, this.swigDelegate51, this.swigDelegate52, this.swigDelegate53, this.swigDelegate54, this.swigDelegate55, this.swigDelegate56, this.swigDelegate57, this.swigDelegate58, this.swigDelegate59, this.swigDelegate60, this.swigDelegate61, this.swigDelegate62, this.swigDelegate63, this.swigDelegate64, this.swigDelegate65);
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000044A0 File Offset: 0x000026A0
	private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes)
	{
		return base.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, methodTypes, null).DeclaringType.IsSubclassOf(typeof(EGLAdapter));
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x000044C7 File Offset: 0x000026C7
	private void SwigDirectorSetCurrentThread(uint id)
	{
		this.SetCurrentThread(id);
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000044D0 File Offset: 0x000026D0
	private void SwigDirectorProcess_eglGetError(int returnVal)
	{
		this.Process_eglGetError(returnVal);
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000044D9 File Offset: 0x000026D9
	private void SwigDirectorProcess_eglGetDisplay(uint returnVal, uint nativeDisplay)
	{
		this.Process_eglGetDisplay(returnVal, nativeDisplay);
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x000044E3 File Offset: 0x000026E3
	private void SwigDirectorProcess_eglInitialize(int returnVal, uint display, IntPtr pMajorPtrData, IntPtr pMinorPtrData)
	{
		this.Process_eglInitialize(returnVal, display, (pMajorPtrData == IntPtr.Zero) ? null : new PointerData(pMajorPtrData, false), (pMinorPtrData == IntPtr.Zero) ? null : new PointerData(pMinorPtrData, false));
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x0000451D File Offset: 0x0000271D
	private void SwigDirectorProcess_eglTerminate(int returnVal, uint display)
	{
		this.Process_eglTerminate(returnVal, display);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00004527 File Offset: 0x00002727
	private void SwigDirectorProcess_eglQueryString(IntPtr pReturnPtrData, uint display, int name)
	{
		this.Process_eglQueryString((pReturnPtrData == IntPtr.Zero) ? null : new PointerData(pReturnPtrData, false), display, name);
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00004548 File Offset: 0x00002748
	private void SwigDirectorProcess_eglGetConfigs(int returnVal, uint display, IntPtr pConfigsPtrData, int configSize, IntPtr pNumConfigPtrData)
	{
		this.Process_eglGetConfigs(returnVal, display, (pConfigsPtrData == IntPtr.Zero) ? null : new PointerData(pConfigsPtrData, false), configSize, (pNumConfigPtrData == IntPtr.Zero) ? null : new PointerData(pNumConfigPtrData, false));
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00004584 File Offset: 0x00002784
	private void SwigDirectorProcess_eglChooseConfig(int returnVal, uint display, IntPtr pAttribListPtrData, IntPtr pConfigsPtrData, int configSize, IntPtr pNumConfigPtrData)
	{
		this.Process_eglChooseConfig(returnVal, display, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false), (pConfigsPtrData == IntPtr.Zero) ? null : new PointerData(pConfigsPtrData, false), configSize, (pNumConfigPtrData == IntPtr.Zero) ? null : new PointerData(pNumConfigPtrData, false));
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000045E4 File Offset: 0x000027E4
	private void SwigDirectorProcess_eglGetConfigAttrib(int returnVal, uint display, uint config, int attribute, IntPtr pValuePtrData)
	{
		this.Process_eglGetConfigAttrib(returnVal, display, config, attribute, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060000FE RID: 254 RVA: 0x0000460A File Offset: 0x0000280A
	private void SwigDirectorProcess_eglCreateWindowSurface(uint returnVal, uint display, uint config, uint nativeWindow, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreateWindowSurface(returnVal, display, config, nativeWindow, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00004630 File Offset: 0x00002830
	private void SwigDirectorProcess_eglCreatePbufferSurface(uint returnVal, uint display, uint config, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreatePbufferSurface(returnVal, display, config, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00004654 File Offset: 0x00002854
	private void SwigDirectorProcess_eglCreatePixmapSurface(uint returnVal, uint display, uint config, uint nativePixmap, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreatePixmapSurface(returnVal, display, config, nativePixmap, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x06000101 RID: 257 RVA: 0x0000467A File Offset: 0x0000287A
	private void SwigDirectorProcess_eglDestroySurface(int returnVal, uint display, uint surface)
	{
		this.Process_eglDestroySurface(returnVal, display, surface);
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00004685 File Offset: 0x00002885
	private void SwigDirectorProcess_eglQuerySurface(int returnVal, uint display, uint surface, int attribute, IntPtr pValuePtrData)
	{
		this.Process_eglQuerySurface(returnVal, display, surface, attribute, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000103 RID: 259 RVA: 0x000046AB File Offset: 0x000028AB
	private void SwigDirectorProcess_eglBindAPI(int returnVal, uint api)
	{
		this.Process_eglBindAPI(returnVal, api);
	}

	// Token: 0x06000104 RID: 260 RVA: 0x000046B5 File Offset: 0x000028B5
	private void SwigDirectorProcess_eglQueryAPI(uint returnVal)
	{
		this.Process_eglQueryAPI(returnVal);
	}

	// Token: 0x06000105 RID: 261 RVA: 0x000046BE File Offset: 0x000028BE
	private void SwigDirectorProcess_eglWaitClient(int returnVal)
	{
		this.Process_eglWaitClient(returnVal);
	}

	// Token: 0x06000106 RID: 262 RVA: 0x000046C7 File Offset: 0x000028C7
	private void SwigDirectorProcess_eglReleaseThread(int returnVal)
	{
		this.Process_eglReleaseThread(returnVal);
	}

	// Token: 0x06000107 RID: 263 RVA: 0x000046D0 File Offset: 0x000028D0
	private void SwigDirectorProcess_eglCreatePbufferFromClientBuffer(uint returnVal, uint display, uint buftype, uint buffer, uint config, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreatePbufferFromClientBuffer(returnVal, display, buftype, buffer, config, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000046F8 File Offset: 0x000028F8
	private void SwigDirectorProcess_eglSurfaceAttrib(int returnVal, uint display, uint surface, int attribute, int value)
	{
		this.Process_eglSurfaceAttrib(returnVal, display, surface, attribute, value);
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00004707 File Offset: 0x00002907
	private void SwigDirectorProcess_eglBindTexImage(int returnVal, uint display, uint surface, int buffer)
	{
		this.Process_eglBindTexImage(returnVal, display, surface, buffer);
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00004714 File Offset: 0x00002914
	private void SwigDirectorProcess_eglReleaseTexImage(int returnVal, uint display, uint surface, int buffer)
	{
		this.Process_eglReleaseTexImage(returnVal, display, surface, buffer);
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00004721 File Offset: 0x00002921
	private void SwigDirectorProcess_eglSwapInterval(int returnVal, uint display, int interval)
	{
		this.Process_eglSwapInterval(returnVal, display, interval);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x0000472C File Offset: 0x0000292C
	private void SwigDirectorProcess_eglCreateContext(uint returnVal, uint display, uint config, uint shareContext, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreateContext(returnVal, display, config, shareContext, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00004752 File Offset: 0x00002952
	private void SwigDirectorProcess_eglDestroyContext(int returnVal, uint display, uint context)
	{
		this.Process_eglDestroyContext(returnVal, display, context);
	}

	// Token: 0x0600010E RID: 270 RVA: 0x0000475D File Offset: 0x0000295D
	private void SwigDirectorProcess_eglMakeCurrent(int returnVal, uint display, uint drawSurface, uint readSurface, uint context)
	{
		this.Process_eglMakeCurrent(returnVal, display, drawSurface, readSurface, context);
	}

	// Token: 0x0600010F RID: 271 RVA: 0x0000476C File Offset: 0x0000296C
	private void SwigDirectorProcess_eglGetCurrentContext(uint returnVal)
	{
		this.Process_eglGetCurrentContext(returnVal);
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00004775 File Offset: 0x00002975
	private void SwigDirectorProcess_eglGetCurrentSurface(uint returnVal, int readdraw)
	{
		this.Process_eglGetCurrentSurface(returnVal, readdraw);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x0000477F File Offset: 0x0000297F
	private void SwigDirectorProcess_eglGetCurrentDisplay(uint returnVal)
	{
		this.Process_eglGetCurrentDisplay(returnVal);
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00004788 File Offset: 0x00002988
	private void SwigDirectorProcess_eglQueryContext(int returnVal, uint display, uint context, int attribute, IntPtr pValuePtrData)
	{
		this.Process_eglQueryContext(returnVal, display, context, attribute, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000113 RID: 275 RVA: 0x000047AE File Offset: 0x000029AE
	private void SwigDirectorProcess_eglWaitGL(int returnVal)
	{
		this.Process_eglWaitGL(returnVal);
	}

	// Token: 0x06000114 RID: 276 RVA: 0x000047B7 File Offset: 0x000029B7
	private void SwigDirectorProcess_eglWaitNative(int returnVal, int engine)
	{
		this.Process_eglWaitNative(returnVal, engine);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000047C1 File Offset: 0x000029C1
	private void SwigDirectorProcess_eglSwapBuffers(int returnVal, uint display, uint surface)
	{
		this.Process_eglSwapBuffers(returnVal, display, surface);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x000047CC File Offset: 0x000029CC
	private void SwigDirectorProcess_eglCopyBuffers(int returnVal, uint display, uint surface, uint nativePixmap)
	{
		this.Process_eglCopyBuffers(returnVal, display, surface, nativePixmap);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x000047D9 File Offset: 0x000029D9
	private void SwigDirectorProcess_eglGetProcAddress(IntPtr pReturnPtrData, IntPtr pProcnamePtrData)
	{
		this.Process_eglGetProcAddress((pReturnPtrData == IntPtr.Zero) ? null : new PointerData(pReturnPtrData, false), (pProcnamePtrData == IntPtr.Zero) ? null : new PointerData(pProcnamePtrData, false));
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000480F File Offset: 0x00002A0F
	private void SwigDirectorProcess_eglCreateImageKHR(uint returnVal, uint display, uint context, uint target, uint buffer, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreateImageKHR(returnVal, display, context, target, buffer, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00004837 File Offset: 0x00002A37
	private void SwigDirectorProcess_eglDestroyImageKHR(int returnVal, uint display, uint image)
	{
		this.Process_eglDestroyImageKHR(returnVal, display, image);
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00004842 File Offset: 0x00002A42
	private void SwigDirectorProcess_eglLockImageQCOM(int returnVal, uint display, uint image, IntPtr pAttribListPtrData)
	{
		this.Process_eglLockImageQCOM(returnVal, display, image, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00004866 File Offset: 0x00002A66
	private void SwigDirectorProcess_eglUnlockImageQCOM(int returnVal, uint display, uint image)
	{
		this.Process_eglUnlockImageQCOM(returnVal, display, image);
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00004871 File Offset: 0x00002A71
	private void SwigDirectorProcess_eglQueryImageQCOM(int returnVal, uint display, uint image, int attribute, IntPtr pValuePtrData)
	{
		this.Process_eglQueryImageQCOM(returnVal, display, image, attribute, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00004897 File Offset: 0x00002A97
	private void SwigDirectorProcess_eglQueryImage64QCOM(int returnVal, uint display, uint image, int attribute, IntPtr pValuePtrData)
	{
		this.Process_eglQueryImage64QCOM(returnVal, display, image, attribute, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600011E RID: 286 RVA: 0x000048BD File Offset: 0x00002ABD
	private void SwigDirectorProcess_eglSetBlobCacheFuncsANDROID(uint display, IntPtr pSetFuncPtrData, IntPtr pGetFuncPtrData)
	{
		this.Process_eglSetBlobCacheFuncsANDROID(display, (pSetFuncPtrData == IntPtr.Zero) ? null : new PointerData(pSetFuncPtrData, false), (pGetFuncPtrData == IntPtr.Zero) ? null : new PointerData(pGetFuncPtrData, false));
	}

	// Token: 0x0600011F RID: 287 RVA: 0x000048F4 File Offset: 0x00002AF4
	private void SwigDirectorProcess_eglCreateSync(uint returnVal, uint display, uint synctype, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreateSync(returnVal, display, synctype, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00004918 File Offset: 0x00002B18
	private void SwigDirectorProcess_eglCreateSyncKHR(uint returnVal, uint display, uint synctype, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreateSyncKHR(returnVal, display, synctype, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0000493C File Offset: 0x00002B3C
	private void SwigDirectorProcess_eglCreateSync64KHR(uint returnVal, uint display, uint synctype, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreateSync64KHR(returnVal, display, synctype, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00004960 File Offset: 0x00002B60
	private void SwigDirectorProcess_eglDestroySyncKHR(int returnVal, uint display, uint sync)
	{
		this.Process_eglDestroySyncKHR(returnVal, display, sync);
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0000496B File Offset: 0x00002B6B
	private void SwigDirectorProcess_eglClientWaitSyncKHR(int returnVal, uint display, uint sync, int flags, ulong timeout)
	{
		this.Process_eglClientWaitSyncKHR(returnVal, display, sync, flags, timeout);
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000497A File Offset: 0x00002B7A
	private void SwigDirectorProcess_eglWaitSyncKHR(int returnVal, uint display, uint sync, int flags)
	{
		this.Process_eglWaitSyncKHR(returnVal, display, sync, flags);
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00004987 File Offset: 0x00002B87
	private void SwigDirectorProcess_eglSignalSyncKHR(int returnVal, uint display, uint sync, uint mode)
	{
		this.Process_eglSignalSyncKHR(returnVal, display, sync, mode);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00004994 File Offset: 0x00002B94
	private void SwigDirectorProcess_eglGetSyncAttrib(int returnVal, uint display, uint sync, int attribute, IntPtr pValuePtrData)
	{
		this.Process_eglGetSyncAttrib(returnVal, display, sync, attribute, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000127 RID: 295 RVA: 0x000049BA File Offset: 0x00002BBA
	private void SwigDirectorProcess_eglDupNativeFenceFDANDROID(int returnVal, uint display, uint sync)
	{
		this.Process_eglDupNativeFenceFDANDROID(returnVal, display, sync);
	}

	// Token: 0x06000128 RID: 296 RVA: 0x000049C5 File Offset: 0x00002BC5
	private void SwigDirectorProcess_eglGetSyncObjFromEglSyncQCOM(int returnVal, uint display, uint sync, uint syncObj)
	{
		this.Process_eglGetSyncObjFromEglSyncQCOM(returnVal, display, sync, syncObj);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000049D2 File Offset: 0x00002BD2
	private void SwigDirectorProcess_eglLockSurfaceKHR(int returnVal, uint display, uint surface, IntPtr pAttribListPtrData)
	{
		this.Process_eglLockSurfaceKHR(returnVal, display, surface, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x0600012A RID: 298 RVA: 0x000049F6 File Offset: 0x00002BF6
	private void SwigDirectorProcess_eglUnlockSurfaceKHR(int returnVal, uint display, uint surface)
	{
		this.Process_eglUnlockSurfaceKHR(returnVal, display, surface);
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00004A01 File Offset: 0x00002C01
	private void SwigDirectorProcess_eglQuerySurface64KHR(int returnVal, uint display, uint surface, int attribute, IntPtr pValuePtrData)
	{
		this.Process_eglQuerySurface64KHR(returnVal, display, surface, attribute, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00004A27 File Offset: 0x00002C27
	private void SwigDirectorProcess_eglGpuPerfHintQCOM(int returnVal, uint display, uint context, IntPtr pAttribListPtrData)
	{
		this.Process_eglGpuPerfHintQCOM(returnVal, display, context, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00004A4B File Offset: 0x00002C4B
	private void SwigDirectorProcess_eglSetDamageRegionKHR(int returnVal, uint display, uint surface, IntPtr pRectsPtrData, int numRects)
	{
		this.Process_eglSetDamageRegionKHR(returnVal, display, surface, (pRectsPtrData == IntPtr.Zero) ? null : new PointerData(pRectsPtrData, false), numRects);
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00004A71 File Offset: 0x00002C71
	private void SwigDirectorProcess_eglGetPlatformDisplay(uint returnVal, uint platform, uint nativeDisplay, IntPtr pAttribListPtrData)
	{
		this.Process_eglGetPlatformDisplay(returnVal, platform, nativeDisplay, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00004A95 File Offset: 0x00002C95
	private void SwigDirectorProcess_eglCreatePlatformWindowSurface(uint returnVal, uint display, uint config, uint nativeWindow, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreatePlatformWindowSurface(returnVal, display, config, nativeWindow, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00004ABB File Offset: 0x00002CBB
	private void SwigDirectorProcess_eglCreatePlatformPixmapSurface(uint returnVal, uint display, uint config, uint nativePixmap, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreatePlatformPixmapSurface(returnVal, display, config, nativePixmap, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00004AE1 File Offset: 0x00002CE1
	private void SwigDirectorProcess_eglGetSyncAttribKHR(int returnVal, uint display, uint sync, int attribute, IntPtr pValuePtrData)
	{
		this.Process_eglGetSyncAttribKHR(returnVal, display, sync, attribute, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00004B07 File Offset: 0x00002D07
	private void SwigDirectorProcess_eglQueryDmaBufFormatsEXT(int returnVal, uint display, int maxFormats, IntPtr pFormatsPtrData, IntPtr pNumFormatsPtrData)
	{
		this.Process_eglQueryDmaBufFormatsEXT(returnVal, display, maxFormats, (pFormatsPtrData == IntPtr.Zero) ? null : new PointerData(pFormatsPtrData, false), (pNumFormatsPtrData == IntPtr.Zero) ? null : new PointerData(pNumFormatsPtrData, false));
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00004B44 File Offset: 0x00002D44
	private void SwigDirectorProcess_eglQueryDmaBufModifiersEXT(int returnVal, uint display, int format, int maxModifiers, IntPtr pModifiersPtrData, IntPtr pExternalOnlyPtrData, IntPtr pNumModifiersPtrData)
	{
		this.Process_eglQueryDmaBufModifiersEXT(returnVal, display, format, maxModifiers, (pModifiersPtrData == IntPtr.Zero) ? null : new PointerData(pModifiersPtrData, false), (pExternalOnlyPtrData == IntPtr.Zero) ? null : new PointerData(pExternalOnlyPtrData, false), (pNumModifiersPtrData == IntPtr.Zero) ? null : new PointerData(pNumModifiersPtrData, false));
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00004BA8 File Offset: 0x00002DA8
	private void SwigDirectorProcess_eglExportDMABUFImageQueryMESA(int returnVal, uint display, uint image, IntPtr pFourccPtrData, IntPtr pNumPlanesPtrData, IntPtr pModifiersPtrData)
	{
		this.Process_eglExportDMABUFImageQueryMESA(returnVal, display, image, (pFourccPtrData == IntPtr.Zero) ? null : new PointerData(pFourccPtrData, false), (pNumPlanesPtrData == IntPtr.Zero) ? null : new PointerData(pNumPlanesPtrData, false), (pModifiersPtrData == IntPtr.Zero) ? null : new PointerData(pModifiersPtrData, false));
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00004C0C File Offset: 0x00002E0C
	private void SwigDirectorProcess_eglExportDMABUFImageMESA(int returnVal, uint display, uint image, IntPtr pFdsPtrData, IntPtr pStridesPtrData, IntPtr pOffsetsPtrData)
	{
		this.Process_eglExportDMABUFImageMESA(returnVal, display, image, (pFdsPtrData == IntPtr.Zero) ? null : new PointerData(pFdsPtrData, false), (pStridesPtrData == IntPtr.Zero) ? null : new PointerData(pStridesPtrData, false), (pOffsetsPtrData == IntPtr.Zero) ? null : new PointerData(pOffsetsPtrData, false));
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00004C6D File Offset: 0x00002E6D
	private void SwigDirectorProcess_eglCreateImageKHRv1(uint returnVal, uint display, uint context, uint target, uint buffer, IntPtr pAttribListPtrData)
	{
		this.Process_eglCreateImageKHRv1(returnVal, display, context, target, buffer, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x040004FB RID: 1275
	private HandleRef swigCPtr;

	// Token: 0x040004FC RID: 1276
	private EGLAdapter.SwigDelegateEGLAdapter_0 swigDelegate0;

	// Token: 0x040004FD RID: 1277
	private EGLAdapter.SwigDelegateEGLAdapter_1 swigDelegate1;

	// Token: 0x040004FE RID: 1278
	private EGLAdapter.SwigDelegateEGLAdapter_2 swigDelegate2;

	// Token: 0x040004FF RID: 1279
	private EGLAdapter.SwigDelegateEGLAdapter_3 swigDelegate3;

	// Token: 0x04000500 RID: 1280
	private EGLAdapter.SwigDelegateEGLAdapter_4 swigDelegate4;

	// Token: 0x04000501 RID: 1281
	private EGLAdapter.SwigDelegateEGLAdapter_5 swigDelegate5;

	// Token: 0x04000502 RID: 1282
	private EGLAdapter.SwigDelegateEGLAdapter_6 swigDelegate6;

	// Token: 0x04000503 RID: 1283
	private EGLAdapter.SwigDelegateEGLAdapter_7 swigDelegate7;

	// Token: 0x04000504 RID: 1284
	private EGLAdapter.SwigDelegateEGLAdapter_8 swigDelegate8;

	// Token: 0x04000505 RID: 1285
	private EGLAdapter.SwigDelegateEGLAdapter_9 swigDelegate9;

	// Token: 0x04000506 RID: 1286
	private EGLAdapter.SwigDelegateEGLAdapter_10 swigDelegate10;

	// Token: 0x04000507 RID: 1287
	private EGLAdapter.SwigDelegateEGLAdapter_11 swigDelegate11;

	// Token: 0x04000508 RID: 1288
	private EGLAdapter.SwigDelegateEGLAdapter_12 swigDelegate12;

	// Token: 0x04000509 RID: 1289
	private EGLAdapter.SwigDelegateEGLAdapter_13 swigDelegate13;

	// Token: 0x0400050A RID: 1290
	private EGLAdapter.SwigDelegateEGLAdapter_14 swigDelegate14;

	// Token: 0x0400050B RID: 1291
	private EGLAdapter.SwigDelegateEGLAdapter_15 swigDelegate15;

	// Token: 0x0400050C RID: 1292
	private EGLAdapter.SwigDelegateEGLAdapter_16 swigDelegate16;

	// Token: 0x0400050D RID: 1293
	private EGLAdapter.SwigDelegateEGLAdapter_17 swigDelegate17;

	// Token: 0x0400050E RID: 1294
	private EGLAdapter.SwigDelegateEGLAdapter_18 swigDelegate18;

	// Token: 0x0400050F RID: 1295
	private EGLAdapter.SwigDelegateEGLAdapter_19 swigDelegate19;

	// Token: 0x04000510 RID: 1296
	private EGLAdapter.SwigDelegateEGLAdapter_20 swigDelegate20;

	// Token: 0x04000511 RID: 1297
	private EGLAdapter.SwigDelegateEGLAdapter_21 swigDelegate21;

	// Token: 0x04000512 RID: 1298
	private EGLAdapter.SwigDelegateEGLAdapter_22 swigDelegate22;

	// Token: 0x04000513 RID: 1299
	private EGLAdapter.SwigDelegateEGLAdapter_23 swigDelegate23;

	// Token: 0x04000514 RID: 1300
	private EGLAdapter.SwigDelegateEGLAdapter_24 swigDelegate24;

	// Token: 0x04000515 RID: 1301
	private EGLAdapter.SwigDelegateEGLAdapter_25 swigDelegate25;

	// Token: 0x04000516 RID: 1302
	private EGLAdapter.SwigDelegateEGLAdapter_26 swigDelegate26;

	// Token: 0x04000517 RID: 1303
	private EGLAdapter.SwigDelegateEGLAdapter_27 swigDelegate27;

	// Token: 0x04000518 RID: 1304
	private EGLAdapter.SwigDelegateEGLAdapter_28 swigDelegate28;

	// Token: 0x04000519 RID: 1305
	private EGLAdapter.SwigDelegateEGLAdapter_29 swigDelegate29;

	// Token: 0x0400051A RID: 1306
	private EGLAdapter.SwigDelegateEGLAdapter_30 swigDelegate30;

	// Token: 0x0400051B RID: 1307
	private EGLAdapter.SwigDelegateEGLAdapter_31 swigDelegate31;

	// Token: 0x0400051C RID: 1308
	private EGLAdapter.SwigDelegateEGLAdapter_32 swigDelegate32;

	// Token: 0x0400051D RID: 1309
	private EGLAdapter.SwigDelegateEGLAdapter_33 swigDelegate33;

	// Token: 0x0400051E RID: 1310
	private EGLAdapter.SwigDelegateEGLAdapter_34 swigDelegate34;

	// Token: 0x0400051F RID: 1311
	private EGLAdapter.SwigDelegateEGLAdapter_35 swigDelegate35;

	// Token: 0x04000520 RID: 1312
	private EGLAdapter.SwigDelegateEGLAdapter_36 swigDelegate36;

	// Token: 0x04000521 RID: 1313
	private EGLAdapter.SwigDelegateEGLAdapter_37 swigDelegate37;

	// Token: 0x04000522 RID: 1314
	private EGLAdapter.SwigDelegateEGLAdapter_38 swigDelegate38;

	// Token: 0x04000523 RID: 1315
	private EGLAdapter.SwigDelegateEGLAdapter_39 swigDelegate39;

	// Token: 0x04000524 RID: 1316
	private EGLAdapter.SwigDelegateEGLAdapter_40 swigDelegate40;

	// Token: 0x04000525 RID: 1317
	private EGLAdapter.SwigDelegateEGLAdapter_41 swigDelegate41;

	// Token: 0x04000526 RID: 1318
	private EGLAdapter.SwigDelegateEGLAdapter_42 swigDelegate42;

	// Token: 0x04000527 RID: 1319
	private EGLAdapter.SwigDelegateEGLAdapter_43 swigDelegate43;

	// Token: 0x04000528 RID: 1320
	private EGLAdapter.SwigDelegateEGLAdapter_44 swigDelegate44;

	// Token: 0x04000529 RID: 1321
	private EGLAdapter.SwigDelegateEGLAdapter_45 swigDelegate45;

	// Token: 0x0400052A RID: 1322
	private EGLAdapter.SwigDelegateEGLAdapter_46 swigDelegate46;

	// Token: 0x0400052B RID: 1323
	private EGLAdapter.SwigDelegateEGLAdapter_47 swigDelegate47;

	// Token: 0x0400052C RID: 1324
	private EGLAdapter.SwigDelegateEGLAdapter_48 swigDelegate48;

	// Token: 0x0400052D RID: 1325
	private EGLAdapter.SwigDelegateEGLAdapter_49 swigDelegate49;

	// Token: 0x0400052E RID: 1326
	private EGLAdapter.SwigDelegateEGLAdapter_50 swigDelegate50;

	// Token: 0x0400052F RID: 1327
	private EGLAdapter.SwigDelegateEGLAdapter_51 swigDelegate51;

	// Token: 0x04000530 RID: 1328
	private EGLAdapter.SwigDelegateEGLAdapter_52 swigDelegate52;

	// Token: 0x04000531 RID: 1329
	private EGLAdapter.SwigDelegateEGLAdapter_53 swigDelegate53;

	// Token: 0x04000532 RID: 1330
	private EGLAdapter.SwigDelegateEGLAdapter_54 swigDelegate54;

	// Token: 0x04000533 RID: 1331
	private EGLAdapter.SwigDelegateEGLAdapter_55 swigDelegate55;

	// Token: 0x04000534 RID: 1332
	private EGLAdapter.SwigDelegateEGLAdapter_56 swigDelegate56;

	// Token: 0x04000535 RID: 1333
	private EGLAdapter.SwigDelegateEGLAdapter_57 swigDelegate57;

	// Token: 0x04000536 RID: 1334
	private EGLAdapter.SwigDelegateEGLAdapter_58 swigDelegate58;

	// Token: 0x04000537 RID: 1335
	private EGLAdapter.SwigDelegateEGLAdapter_59 swigDelegate59;

	// Token: 0x04000538 RID: 1336
	private EGLAdapter.SwigDelegateEGLAdapter_60 swigDelegate60;

	// Token: 0x04000539 RID: 1337
	private EGLAdapter.SwigDelegateEGLAdapter_61 swigDelegate61;

	// Token: 0x0400053A RID: 1338
	private EGLAdapter.SwigDelegateEGLAdapter_62 swigDelegate62;

	// Token: 0x0400053B RID: 1339
	private EGLAdapter.SwigDelegateEGLAdapter_63 swigDelegate63;

	// Token: 0x0400053C RID: 1340
	private EGLAdapter.SwigDelegateEGLAdapter_64 swigDelegate64;

	// Token: 0x0400053D RID: 1341
	private EGLAdapter.SwigDelegateEGLAdapter_65 swigDelegate65;

	// Token: 0x0400053E RID: 1342
	private static Type[] swigMethodTypes0 = new Type[] { typeof(uint) };

	// Token: 0x0400053F RID: 1343
	private static Type[] swigMethodTypes1 = new Type[] { typeof(int) };

	// Token: 0x04000540 RID: 1344
	private static Type[] swigMethodTypes2 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000541 RID: 1345
	private static Type[] swigMethodTypes3 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000542 RID: 1346
	private static Type[] swigMethodTypes4 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x04000543 RID: 1347
	private static Type[] swigMethodTypes5 = new Type[]
	{
		typeof(PointerData),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000544 RID: 1348
	private static Type[] swigMethodTypes6 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(PointerData),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000545 RID: 1349
	private static Type[] swigMethodTypes7 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(PointerData),
		typeof(PointerData),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000546 RID: 1350
	private static Type[] swigMethodTypes8 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000547 RID: 1351
	private static Type[] swigMethodTypes9 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000548 RID: 1352
	private static Type[] swigMethodTypes10 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000549 RID: 1353
	private static Type[] swigMethodTypes11 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400054A RID: 1354
	private static Type[] swigMethodTypes12 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400054B RID: 1355
	private static Type[] swigMethodTypes13 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400054C RID: 1356
	private static Type[] swigMethodTypes14 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x0400054D RID: 1357
	private static Type[] swigMethodTypes15 = new Type[] { typeof(uint) };

	// Token: 0x0400054E RID: 1358
	private static Type[] swigMethodTypes16 = new Type[] { typeof(int) };

	// Token: 0x0400054F RID: 1359
	private static Type[] swigMethodTypes17 = new Type[] { typeof(int) };

	// Token: 0x04000550 RID: 1360
	private static Type[] swigMethodTypes18 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000551 RID: 1361
	private static Type[] swigMethodTypes19 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000552 RID: 1362
	private static Type[] swigMethodTypes20 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000553 RID: 1363
	private static Type[] swigMethodTypes21 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000554 RID: 1364
	private static Type[] swigMethodTypes22 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000555 RID: 1365
	private static Type[] swigMethodTypes23 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000556 RID: 1366
	private static Type[] swigMethodTypes24 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000557 RID: 1367
	private static Type[] swigMethodTypes25 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000558 RID: 1368
	private static Type[] swigMethodTypes26 = new Type[] { typeof(uint) };

	// Token: 0x04000559 RID: 1369
	private static Type[] swigMethodTypes27 = new Type[]
	{
		typeof(uint),
		typeof(int)
	};

	// Token: 0x0400055A RID: 1370
	private static Type[] swigMethodTypes28 = new Type[] { typeof(uint) };

	// Token: 0x0400055B RID: 1371
	private static Type[] swigMethodTypes29 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400055C RID: 1372
	private static Type[] swigMethodTypes30 = new Type[] { typeof(int) };

	// Token: 0x0400055D RID: 1373
	private static Type[] swigMethodTypes31 = new Type[]
	{
		typeof(int),
		typeof(int)
	};

	// Token: 0x0400055E RID: 1374
	private static Type[] swigMethodTypes32 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400055F RID: 1375
	private static Type[] swigMethodTypes33 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000560 RID: 1376
	private static Type[] swigMethodTypes34 = new Type[]
	{
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000561 RID: 1377
	private static Type[] swigMethodTypes35 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000562 RID: 1378
	private static Type[] swigMethodTypes36 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000563 RID: 1379
	private static Type[] swigMethodTypes37 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000564 RID: 1380
	private static Type[] swigMethodTypes38 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000565 RID: 1381
	private static Type[] swigMethodTypes39 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000566 RID: 1382
	private static Type[] swigMethodTypes40 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000567 RID: 1383
	private static Type[] swigMethodTypes41 = new Type[]
	{
		typeof(uint),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000568 RID: 1384
	private static Type[] swigMethodTypes42 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000569 RID: 1385
	private static Type[] swigMethodTypes43 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400056A RID: 1386
	private static Type[] swigMethodTypes44 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400056B RID: 1387
	private static Type[] swigMethodTypes45 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400056C RID: 1388
	private static Type[] swigMethodTypes46 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(ulong)
	};

	// Token: 0x0400056D RID: 1389
	private static Type[] swigMethodTypes47 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x0400056E RID: 1390
	private static Type[] swigMethodTypes48 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400056F RID: 1391
	private static Type[] swigMethodTypes49 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000570 RID: 1392
	private static Type[] swigMethodTypes50 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000571 RID: 1393
	private static Type[] swigMethodTypes51 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000572 RID: 1394
	private static Type[] swigMethodTypes52 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000573 RID: 1395
	private static Type[] swigMethodTypes53 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000574 RID: 1396
	private static Type[] swigMethodTypes54 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000575 RID: 1397
	private static Type[] swigMethodTypes55 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000576 RID: 1398
	private static Type[] swigMethodTypes56 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData),
		typeof(int)
	};

	// Token: 0x04000577 RID: 1399
	private static Type[] swigMethodTypes57 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000578 RID: 1400
	private static Type[] swigMethodTypes58 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000579 RID: 1401
	private static Type[] swigMethodTypes59 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400057A RID: 1402
	private static Type[] swigMethodTypes60 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400057B RID: 1403
	private static Type[] swigMethodTypes61 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x0400057C RID: 1404
	private static Type[] swigMethodTypes62 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x0400057D RID: 1405
	private static Type[] swigMethodTypes63 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x0400057E RID: 1406
	private static Type[] swigMethodTypes64 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x0400057F RID: 1407
	private static Type[] swigMethodTypes65 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0200003D RID: 61
	// (Invoke) Token: 0x060009B0 RID: 2480
	public delegate void SwigDelegateEGLAdapter_0(uint id);

	// Token: 0x0200003E RID: 62
	// (Invoke) Token: 0x060009B4 RID: 2484
	public delegate void SwigDelegateEGLAdapter_1(int returnVal);

	// Token: 0x0200003F RID: 63
	// (Invoke) Token: 0x060009B8 RID: 2488
	public delegate void SwigDelegateEGLAdapter_2(uint returnVal, uint nativeDisplay);

	// Token: 0x02000040 RID: 64
	// (Invoke) Token: 0x060009BC RID: 2492
	public delegate void SwigDelegateEGLAdapter_3(int returnVal, uint display, IntPtr pMajorPtrData, IntPtr pMinorPtrData);

	// Token: 0x02000041 RID: 65
	// (Invoke) Token: 0x060009C0 RID: 2496
	public delegate void SwigDelegateEGLAdapter_4(int returnVal, uint display);

	// Token: 0x02000042 RID: 66
	// (Invoke) Token: 0x060009C4 RID: 2500
	public delegate void SwigDelegateEGLAdapter_5(IntPtr pReturnPtrData, uint display, int name);

	// Token: 0x02000043 RID: 67
	// (Invoke) Token: 0x060009C8 RID: 2504
	public delegate void SwigDelegateEGLAdapter_6(int returnVal, uint display, IntPtr pConfigsPtrData, int configSize, IntPtr pNumConfigPtrData);

	// Token: 0x02000044 RID: 68
	// (Invoke) Token: 0x060009CC RID: 2508
	public delegate void SwigDelegateEGLAdapter_7(int returnVal, uint display, IntPtr pAttribListPtrData, IntPtr pConfigsPtrData, int configSize, IntPtr pNumConfigPtrData);

	// Token: 0x02000045 RID: 69
	// (Invoke) Token: 0x060009D0 RID: 2512
	public delegate void SwigDelegateEGLAdapter_8(int returnVal, uint display, uint config, int attribute, IntPtr pValuePtrData);

	// Token: 0x02000046 RID: 70
	// (Invoke) Token: 0x060009D4 RID: 2516
	public delegate void SwigDelegateEGLAdapter_9(uint returnVal, uint display, uint config, uint nativeWindow, IntPtr pAttribListPtrData);

	// Token: 0x02000047 RID: 71
	// (Invoke) Token: 0x060009D8 RID: 2520
	public delegate void SwigDelegateEGLAdapter_10(uint returnVal, uint display, uint config, IntPtr pAttribListPtrData);

	// Token: 0x02000048 RID: 72
	// (Invoke) Token: 0x060009DC RID: 2524
	public delegate void SwigDelegateEGLAdapter_11(uint returnVal, uint display, uint config, uint nativePixmap, IntPtr pAttribListPtrData);

	// Token: 0x02000049 RID: 73
	// (Invoke) Token: 0x060009E0 RID: 2528
	public delegate void SwigDelegateEGLAdapter_12(int returnVal, uint display, uint surface);

	// Token: 0x0200004A RID: 74
	// (Invoke) Token: 0x060009E4 RID: 2532
	public delegate void SwigDelegateEGLAdapter_13(int returnVal, uint display, uint surface, int attribute, IntPtr pValuePtrData);

	// Token: 0x0200004B RID: 75
	// (Invoke) Token: 0x060009E8 RID: 2536
	public delegate void SwigDelegateEGLAdapter_14(int returnVal, uint api);

	// Token: 0x0200004C RID: 76
	// (Invoke) Token: 0x060009EC RID: 2540
	public delegate void SwigDelegateEGLAdapter_15(uint returnVal);

	// Token: 0x0200004D RID: 77
	// (Invoke) Token: 0x060009F0 RID: 2544
	public delegate void SwigDelegateEGLAdapter_16(int returnVal);

	// Token: 0x0200004E RID: 78
	// (Invoke) Token: 0x060009F4 RID: 2548
	public delegate void SwigDelegateEGLAdapter_17(int returnVal);

	// Token: 0x0200004F RID: 79
	// (Invoke) Token: 0x060009F8 RID: 2552
	public delegate void SwigDelegateEGLAdapter_18(uint returnVal, uint display, uint buftype, uint buffer, uint config, IntPtr pAttribListPtrData);

	// Token: 0x02000050 RID: 80
	// (Invoke) Token: 0x060009FC RID: 2556
	public delegate void SwigDelegateEGLAdapter_19(int returnVal, uint display, uint surface, int attribute, int value);

	// Token: 0x02000051 RID: 81
	// (Invoke) Token: 0x06000A00 RID: 2560
	public delegate void SwigDelegateEGLAdapter_20(int returnVal, uint display, uint surface, int buffer);

	// Token: 0x02000052 RID: 82
	// (Invoke) Token: 0x06000A04 RID: 2564
	public delegate void SwigDelegateEGLAdapter_21(int returnVal, uint display, uint surface, int buffer);

	// Token: 0x02000053 RID: 83
	// (Invoke) Token: 0x06000A08 RID: 2568
	public delegate void SwigDelegateEGLAdapter_22(int returnVal, uint display, int interval);

	// Token: 0x02000054 RID: 84
	// (Invoke) Token: 0x06000A0C RID: 2572
	public delegate void SwigDelegateEGLAdapter_23(uint returnVal, uint display, uint config, uint shareContext, IntPtr pAttribListPtrData);

	// Token: 0x02000055 RID: 85
	// (Invoke) Token: 0x06000A10 RID: 2576
	public delegate void SwigDelegateEGLAdapter_24(int returnVal, uint display, uint context);

	// Token: 0x02000056 RID: 86
	// (Invoke) Token: 0x06000A14 RID: 2580
	public delegate void SwigDelegateEGLAdapter_25(int returnVal, uint display, uint drawSurface, uint readSurface, uint context);

	// Token: 0x02000057 RID: 87
	// (Invoke) Token: 0x06000A18 RID: 2584
	public delegate void SwigDelegateEGLAdapter_26(uint returnVal);

	// Token: 0x02000058 RID: 88
	// (Invoke) Token: 0x06000A1C RID: 2588
	public delegate void SwigDelegateEGLAdapter_27(uint returnVal, int readdraw);

	// Token: 0x02000059 RID: 89
	// (Invoke) Token: 0x06000A20 RID: 2592
	public delegate void SwigDelegateEGLAdapter_28(uint returnVal);

	// Token: 0x0200005A RID: 90
	// (Invoke) Token: 0x06000A24 RID: 2596
	public delegate void SwigDelegateEGLAdapter_29(int returnVal, uint display, uint context, int attribute, IntPtr pValuePtrData);

	// Token: 0x0200005B RID: 91
	// (Invoke) Token: 0x06000A28 RID: 2600
	public delegate void SwigDelegateEGLAdapter_30(int returnVal);

	// Token: 0x0200005C RID: 92
	// (Invoke) Token: 0x06000A2C RID: 2604
	public delegate void SwigDelegateEGLAdapter_31(int returnVal, int engine);

	// Token: 0x0200005D RID: 93
	// (Invoke) Token: 0x06000A30 RID: 2608
	public delegate void SwigDelegateEGLAdapter_32(int returnVal, uint display, uint surface);

	// Token: 0x0200005E RID: 94
	// (Invoke) Token: 0x06000A34 RID: 2612
	public delegate void SwigDelegateEGLAdapter_33(int returnVal, uint display, uint surface, uint nativePixmap);

	// Token: 0x0200005F RID: 95
	// (Invoke) Token: 0x06000A38 RID: 2616
	public delegate void SwigDelegateEGLAdapter_34(IntPtr pReturnPtrData, IntPtr pProcnamePtrData);

	// Token: 0x02000060 RID: 96
	// (Invoke) Token: 0x06000A3C RID: 2620
	public delegate void SwigDelegateEGLAdapter_35(uint returnVal, uint display, uint context, uint target, uint buffer, IntPtr pAttribListPtrData);

	// Token: 0x02000061 RID: 97
	// (Invoke) Token: 0x06000A40 RID: 2624
	public delegate void SwigDelegateEGLAdapter_36(int returnVal, uint display, uint image);

	// Token: 0x02000062 RID: 98
	// (Invoke) Token: 0x06000A44 RID: 2628
	public delegate void SwigDelegateEGLAdapter_37(int returnVal, uint display, uint image, IntPtr pAttribListPtrData);

	// Token: 0x02000063 RID: 99
	// (Invoke) Token: 0x06000A48 RID: 2632
	public delegate void SwigDelegateEGLAdapter_38(int returnVal, uint display, uint image);

	// Token: 0x02000064 RID: 100
	// (Invoke) Token: 0x06000A4C RID: 2636
	public delegate void SwigDelegateEGLAdapter_39(int returnVal, uint display, uint image, int attribute, IntPtr pValuePtrData);

	// Token: 0x02000065 RID: 101
	// (Invoke) Token: 0x06000A50 RID: 2640
	public delegate void SwigDelegateEGLAdapter_40(int returnVal, uint display, uint image, int attribute, IntPtr pValuePtrData);

	// Token: 0x02000066 RID: 102
	// (Invoke) Token: 0x06000A54 RID: 2644
	public delegate void SwigDelegateEGLAdapter_41(uint display, IntPtr pSetFuncPtrData, IntPtr pGetFuncPtrData);

	// Token: 0x02000067 RID: 103
	// (Invoke) Token: 0x06000A58 RID: 2648
	public delegate void SwigDelegateEGLAdapter_42(uint returnVal, uint display, uint synctype, IntPtr pAttribListPtrData);

	// Token: 0x02000068 RID: 104
	// (Invoke) Token: 0x06000A5C RID: 2652
	public delegate void SwigDelegateEGLAdapter_43(uint returnVal, uint display, uint synctype, IntPtr pAttribListPtrData);

	// Token: 0x02000069 RID: 105
	// (Invoke) Token: 0x06000A60 RID: 2656
	public delegate void SwigDelegateEGLAdapter_44(uint returnVal, uint display, uint synctype, IntPtr pAttribListPtrData);

	// Token: 0x0200006A RID: 106
	// (Invoke) Token: 0x06000A64 RID: 2660
	public delegate void SwigDelegateEGLAdapter_45(int returnVal, uint display, uint sync);

	// Token: 0x0200006B RID: 107
	// (Invoke) Token: 0x06000A68 RID: 2664
	public delegate void SwigDelegateEGLAdapter_46(int returnVal, uint display, uint sync, int flags, ulong timeout);

	// Token: 0x0200006C RID: 108
	// (Invoke) Token: 0x06000A6C RID: 2668
	public delegate void SwigDelegateEGLAdapter_47(int returnVal, uint display, uint sync, int flags);

	// Token: 0x0200006D RID: 109
	// (Invoke) Token: 0x06000A70 RID: 2672
	public delegate void SwigDelegateEGLAdapter_48(int returnVal, uint display, uint sync, uint mode);

	// Token: 0x0200006E RID: 110
	// (Invoke) Token: 0x06000A74 RID: 2676
	public delegate void SwigDelegateEGLAdapter_49(int returnVal, uint display, uint sync, int attribute, IntPtr pValuePtrData);

	// Token: 0x0200006F RID: 111
	// (Invoke) Token: 0x06000A78 RID: 2680
	public delegate void SwigDelegateEGLAdapter_50(int returnVal, uint display, uint sync);

	// Token: 0x02000070 RID: 112
	// (Invoke) Token: 0x06000A7C RID: 2684
	public delegate void SwigDelegateEGLAdapter_51(int returnVal, uint display, uint sync, uint syncObj);

	// Token: 0x02000071 RID: 113
	// (Invoke) Token: 0x06000A80 RID: 2688
	public delegate void SwigDelegateEGLAdapter_52(int returnVal, uint display, uint surface, IntPtr pAttribListPtrData);

	// Token: 0x02000072 RID: 114
	// (Invoke) Token: 0x06000A84 RID: 2692
	public delegate void SwigDelegateEGLAdapter_53(int returnVal, uint display, uint surface);

	// Token: 0x02000073 RID: 115
	// (Invoke) Token: 0x06000A88 RID: 2696
	public delegate void SwigDelegateEGLAdapter_54(int returnVal, uint display, uint surface, int attribute, IntPtr pValuePtrData);

	// Token: 0x02000074 RID: 116
	// (Invoke) Token: 0x06000A8C RID: 2700
	public delegate void SwigDelegateEGLAdapter_55(int returnVal, uint display, uint context, IntPtr pAttribListPtrData);

	// Token: 0x02000075 RID: 117
	// (Invoke) Token: 0x06000A90 RID: 2704
	public delegate void SwigDelegateEGLAdapter_56(int returnVal, uint display, uint surface, IntPtr pRectsPtrData, int numRects);

	// Token: 0x02000076 RID: 118
	// (Invoke) Token: 0x06000A94 RID: 2708
	public delegate void SwigDelegateEGLAdapter_57(uint returnVal, uint platform, uint nativeDisplay, IntPtr pAttribListPtrData);

	// Token: 0x02000077 RID: 119
	// (Invoke) Token: 0x06000A98 RID: 2712
	public delegate void SwigDelegateEGLAdapter_58(uint returnVal, uint display, uint config, uint nativeWindow, IntPtr pAttribListPtrData);

	// Token: 0x02000078 RID: 120
	// (Invoke) Token: 0x06000A9C RID: 2716
	public delegate void SwigDelegateEGLAdapter_59(uint returnVal, uint display, uint config, uint nativePixmap, IntPtr pAttribListPtrData);

	// Token: 0x02000079 RID: 121
	// (Invoke) Token: 0x06000AA0 RID: 2720
	public delegate void SwigDelegateEGLAdapter_60(int returnVal, uint display, uint sync, int attribute, IntPtr pValuePtrData);

	// Token: 0x0200007A RID: 122
	// (Invoke) Token: 0x06000AA4 RID: 2724
	public delegate void SwigDelegateEGLAdapter_61(int returnVal, uint display, int maxFormats, IntPtr pFormatsPtrData, IntPtr pNumFormatsPtrData);

	// Token: 0x0200007B RID: 123
	// (Invoke) Token: 0x06000AA8 RID: 2728
	public delegate void SwigDelegateEGLAdapter_62(int returnVal, uint display, int format, int maxModifiers, IntPtr pModifiersPtrData, IntPtr pExternalOnlyPtrData, IntPtr pNumModifiersPtrData);

	// Token: 0x0200007C RID: 124
	// (Invoke) Token: 0x06000AAC RID: 2732
	public delegate void SwigDelegateEGLAdapter_63(int returnVal, uint display, uint image, IntPtr pFourccPtrData, IntPtr pNumPlanesPtrData, IntPtr pModifiersPtrData);

	// Token: 0x0200007D RID: 125
	// (Invoke) Token: 0x06000AB0 RID: 2736
	public delegate void SwigDelegateEGLAdapter_64(int returnVal, uint display, uint image, IntPtr pFdsPtrData, IntPtr pStridesPtrData, IntPtr pOffsetsPtrData);

	// Token: 0x0200007E RID: 126
	// (Invoke) Token: 0x06000AB4 RID: 2740
	public delegate void SwigDelegateEGLAdapter_65(uint returnVal, uint display, uint context, uint target, uint buffer, IntPtr pAttribListPtrData);
}
