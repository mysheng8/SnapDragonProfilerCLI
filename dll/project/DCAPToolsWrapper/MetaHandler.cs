using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x0200001D RID: 29
public class MetaHandler : IDisposable
{
	// Token: 0x06000906 RID: 2310 RVA: 0x000189F7 File Offset: 0x00016BF7
	internal MetaHandler(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00018A13 File Offset: 0x00016C13
	internal static HandleRef getCPtr(MetaHandler obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00018A2C File Offset: 0x00016C2C
	~MetaHandler()
	{
		this.Dispose();
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00018A58 File Offset: 0x00016C58
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_MetaHandler(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00018AD8 File Offset: 0x00016CD8
	public virtual void SetCurrentThread(uint id)
	{
		libDCAPPINVOKE.MetaHandler_SetCurrentThread(this.swigCPtr, id);
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x00018AE6 File Offset: 0x00016CE6
	public virtual void ProcessTextAscii(IntPtr pData, uint len)
	{
		libDCAPPINVOKE.MetaHandler_ProcessTextAscii(this.swigCPtr, pData, len);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x00018AF5 File Offset: 0x00016CF5
	public virtual void ProcessTextXML(IntPtr pData, uint len)
	{
		libDCAPPINVOKE.MetaHandler_ProcessTextXML(this.swigCPtr, pData, len);
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x00018B04 File Offset: 0x00016D04
	public virtual void ProcessTextJSON(IntPtr pData, uint len)
	{
		libDCAPPINVOKE.MetaHandler_ProcessTextJSON(this.swigCPtr, pData, len);
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00018B13 File Offset: 0x00016D13
	public virtual void ProcessDisplayMessageCommand(string message)
	{
		libDCAPPINVOKE.MetaHandler_ProcessDisplayMessageCommand(this.swigCPtr, message);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00018B2E File Offset: 0x00016D2E
	public virtual void ProcessSetWindowSizeCommand(uint displayId, uint surfaceId, uint width, uint height)
	{
		libDCAPPINVOKE.MetaHandler_ProcessSetWindowSizeCommand(this.swigCPtr, displayId, surfaceId, width, height);
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00018B40 File Offset: 0x00016D40
	public virtual void ProcessCreateEGLImageBufferCommand(uint displayId, uint imageId, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessCreateEGLImageBufferCommand(this.swigCPtr, displayId, imageId, pData);
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x00018B50 File Offset: 0x00016D50
	public virtual void ProcessSetEGLImageContentCommand(uint imageId, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessSetEGLImageContentCommand(this.swigCPtr, imageId, pData);
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00018B5F File Offset: 0x00016D5F
	public virtual void ProcessEGLConfigStateDesc(uint displayId, uint configId, IntPtr pAttribList, uint len)
	{
		libDCAPPINVOKE.MetaHandler_ProcessEGLConfigStateDesc(this.swigCPtr, displayId, configId, pAttribList, len);
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00018B71 File Offset: 0x00016D71
	public virtual void ProcessEGLSurfaceStateDesc(uint displayId, uint configId, IntPtr pAttribList, uint len)
	{
		libDCAPPINVOKE.MetaHandler_ProcessEGLSurfaceStateDesc(this.swigCPtr, displayId, configId, pAttribList, len);
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00018B83 File Offset: 0x00016D83
	public virtual void ProcessGLUniformsStateDesc(uint programId, uint numUniforms, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessGLUniformsStateDesc(this.swigCPtr, programId, numUniforms, pData);
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00018B93 File Offset: 0x00016D93
	public virtual void ProcessGLUniformBlocksStateDesc(uint programId, uint numUniformBlocks, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessGLUniformBlocksStateDesc(this.swigCPtr, programId, numUniformBlocks, pData);
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00018BA3 File Offset: 0x00016DA3
	public virtual void ProcessGLAttributesStateDesc(uint programId, uint numAttributes, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessGLAttributesStateDesc(this.swigCPtr, programId, numAttributes, pData);
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00018BB3 File Offset: 0x00016DB3
	public virtual void ProcessGLAtomicCounterBufferStateDesc(uint programId, uint numBuffers, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessGLAtomicCounterBufferStateDesc(this.swigCPtr, programId, numBuffers, pData);
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00018BC3 File Offset: 0x00016DC3
	public virtual void ProcessGLBufferVariablesStateDesc(uint programId, uint numBuffers, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessGLBufferVariablesStateDesc(this.swigCPtr, programId, numBuffers, pData);
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x00018BD3 File Offset: 0x00016DD3
	public virtual void ProcessGLStorageBlocksStateDesc(uint programId, uint numBuffers, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessGLStorageBlocksStateDesc(this.swigCPtr, programId, numBuffers, pData);
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00018BE3 File Offset: 0x00016DE3
	public virtual void ProcessProfilerData(uint subtype, IntPtr pData, uint len)
	{
		libDCAPPINVOKE.MetaHandler_ProcessProfilerData(this.swigCPtr, subtype, pData, len);
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00018BF3 File Offset: 0x00016DF3
	public virtual void ProcessGLLimits(uint numLimits, IntPtr pLimits)
	{
		libDCAPPINVOKE.MetaHandler_ProcessGLLimits(this.swigCPtr, numLimits, pLimits);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00018C04 File Offset: 0x00016E04
	public virtual void ProcessDefaultColorBuffer(uint displayId, uint surfaceId, uint width, uint height, uint format, uint type, uint buffSize, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessDefaultColorBuffer(this.swigCPtr, displayId, surfaceId, width, height, format, type, buffSize, pData);
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x00018C2C File Offset: 0x00016E2C
	public virtual void ProcessDefaultDepthBuffer(uint displayId, uint surfaceId, uint width, uint height, uint format, uint type, uint buffSize, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessDefaultDepthBuffer(this.swigCPtr, displayId, surfaceId, width, height, format, type, buffSize, pData);
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00018C54 File Offset: 0x00016E54
	public virtual void ProcessDefaultStencilBuffer(uint displayId, uint surfaceId, uint width, uint height, uint format, uint type, uint buffSize, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessDefaultStencilBuffer(this.swigCPtr, displayId, surfaceId, width, height, format, type, buffSize, pData);
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00018C7C File Offset: 0x00016E7C
	public virtual void ProcessMultiSampleTexture(uint target, uint texture, uint width, uint height, uint depth, uint internalFormat, uint format, uint type, uint layerSize, uint buffSize, IntPtr pData)
	{
		libDCAPPINVOKE.MetaHandler_ProcessMultiSampleTexture(this.swigCPtr, target, texture, width, height, depth, internalFormat, format, type, layerSize, buffSize, pData);
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00018CA7 File Offset: 0x00016EA7
	public MetaHandler()
		: this(libDCAPPINVOKE.new_MetaHandler(), true)
	{
		this.SwigDirectorConnect();
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x00018CBC File Offset: 0x00016EBC
	private void SwigDirectorConnect()
	{
		if (this.SwigDerivedClassHasMethod("SetCurrentThread", MetaHandler.swigMethodTypes0))
		{
			this.swigDelegate0 = new MetaHandler.SwigDelegateMetaHandler_0(this.SwigDirectorSetCurrentThread);
		}
		if (this.SwigDerivedClassHasMethod("ProcessTextAscii", MetaHandler.swigMethodTypes1))
		{
			this.swigDelegate1 = new MetaHandler.SwigDelegateMetaHandler_1(this.SwigDirectorProcessTextAscii);
		}
		if (this.SwigDerivedClassHasMethod("ProcessTextXML", MetaHandler.swigMethodTypes2))
		{
			this.swigDelegate2 = new MetaHandler.SwigDelegateMetaHandler_2(this.SwigDirectorProcessTextXML);
		}
		if (this.SwigDerivedClassHasMethod("ProcessTextJSON", MetaHandler.swigMethodTypes3))
		{
			this.swigDelegate3 = new MetaHandler.SwigDelegateMetaHandler_3(this.SwigDirectorProcessTextJSON);
		}
		if (this.SwigDerivedClassHasMethod("ProcessDisplayMessageCommand", MetaHandler.swigMethodTypes4))
		{
			this.swigDelegate4 = new MetaHandler.SwigDelegateMetaHandler_4(this.SwigDirectorProcessDisplayMessageCommand);
		}
		if (this.SwigDerivedClassHasMethod("ProcessSetWindowSizeCommand", MetaHandler.swigMethodTypes5))
		{
			this.swigDelegate5 = new MetaHandler.SwigDelegateMetaHandler_5(this.SwigDirectorProcessSetWindowSizeCommand);
		}
		if (this.SwigDerivedClassHasMethod("ProcessCreateEGLImageBufferCommand", MetaHandler.swigMethodTypes6))
		{
			this.swigDelegate6 = new MetaHandler.SwigDelegateMetaHandler_6(this.SwigDirectorProcessCreateEGLImageBufferCommand);
		}
		if (this.SwigDerivedClassHasMethod("ProcessSetEGLImageContentCommand", MetaHandler.swigMethodTypes7))
		{
			this.swigDelegate7 = new MetaHandler.SwigDelegateMetaHandler_7(this.SwigDirectorProcessSetEGLImageContentCommand);
		}
		if (this.SwigDerivedClassHasMethod("ProcessEGLConfigStateDesc", MetaHandler.swigMethodTypes8))
		{
			this.swigDelegate8 = new MetaHandler.SwigDelegateMetaHandler_8(this.SwigDirectorProcessEGLConfigStateDesc);
		}
		if (this.SwigDerivedClassHasMethod("ProcessEGLSurfaceStateDesc", MetaHandler.swigMethodTypes9))
		{
			this.swigDelegate9 = new MetaHandler.SwigDelegateMetaHandler_9(this.SwigDirectorProcessEGLSurfaceStateDesc);
		}
		if (this.SwigDerivedClassHasMethod("ProcessGLUniformsStateDesc", MetaHandler.swigMethodTypes10))
		{
			this.swigDelegate10 = new MetaHandler.SwigDelegateMetaHandler_10(this.SwigDirectorProcessGLUniformsStateDesc);
		}
		if (this.SwigDerivedClassHasMethod("ProcessGLUniformBlocksStateDesc", MetaHandler.swigMethodTypes11))
		{
			this.swigDelegate11 = new MetaHandler.SwigDelegateMetaHandler_11(this.SwigDirectorProcessGLUniformBlocksStateDesc);
		}
		if (this.SwigDerivedClassHasMethod("ProcessGLAttributesStateDesc", MetaHandler.swigMethodTypes12))
		{
			this.swigDelegate12 = new MetaHandler.SwigDelegateMetaHandler_12(this.SwigDirectorProcessGLAttributesStateDesc);
		}
		if (this.SwigDerivedClassHasMethod("ProcessGLAtomicCounterBufferStateDesc", MetaHandler.swigMethodTypes13))
		{
			this.swigDelegate13 = new MetaHandler.SwigDelegateMetaHandler_13(this.SwigDirectorProcessGLAtomicCounterBufferStateDesc);
		}
		if (this.SwigDerivedClassHasMethod("ProcessGLBufferVariablesStateDesc", MetaHandler.swigMethodTypes14))
		{
			this.swigDelegate14 = new MetaHandler.SwigDelegateMetaHandler_14(this.SwigDirectorProcessGLBufferVariablesStateDesc);
		}
		if (this.SwigDerivedClassHasMethod("ProcessGLStorageBlocksStateDesc", MetaHandler.swigMethodTypes15))
		{
			this.swigDelegate15 = new MetaHandler.SwigDelegateMetaHandler_15(this.SwigDirectorProcessGLStorageBlocksStateDesc);
		}
		if (this.SwigDerivedClassHasMethod("ProcessProfilerData", MetaHandler.swigMethodTypes16))
		{
			this.swigDelegate16 = new MetaHandler.SwigDelegateMetaHandler_16(this.SwigDirectorProcessProfilerData);
		}
		if (this.SwigDerivedClassHasMethod("ProcessGLLimits", MetaHandler.swigMethodTypes17))
		{
			this.swigDelegate17 = new MetaHandler.SwigDelegateMetaHandler_17(this.SwigDirectorProcessGLLimits);
		}
		if (this.SwigDerivedClassHasMethod("ProcessDefaultColorBuffer", MetaHandler.swigMethodTypes18))
		{
			this.swigDelegate18 = new MetaHandler.SwigDelegateMetaHandler_18(this.SwigDirectorProcessDefaultColorBuffer);
		}
		if (this.SwigDerivedClassHasMethod("ProcessDefaultDepthBuffer", MetaHandler.swigMethodTypes19))
		{
			this.swigDelegate19 = new MetaHandler.SwigDelegateMetaHandler_19(this.SwigDirectorProcessDefaultDepthBuffer);
		}
		if (this.SwigDerivedClassHasMethod("ProcessDefaultStencilBuffer", MetaHandler.swigMethodTypes20))
		{
			this.swigDelegate20 = new MetaHandler.SwigDelegateMetaHandler_20(this.SwigDirectorProcessDefaultStencilBuffer);
		}
		if (this.SwigDerivedClassHasMethod("ProcessMultiSampleTexture", MetaHandler.swigMethodTypes21))
		{
			this.swigDelegate21 = new MetaHandler.SwigDelegateMetaHandler_21(this.SwigDirectorProcessMultiSampleTexture);
		}
		libDCAPPINVOKE.MetaHandler_director_connect(this.swigCPtr, this.swigDelegate0, this.swigDelegate1, this.swigDelegate2, this.swigDelegate3, this.swigDelegate4, this.swigDelegate5, this.swigDelegate6, this.swigDelegate7, this.swigDelegate8, this.swigDelegate9, this.swigDelegate10, this.swigDelegate11, this.swigDelegate12, this.swigDelegate13, this.swigDelegate14, this.swigDelegate15, this.swigDelegate16, this.swigDelegate17, this.swigDelegate18, this.swigDelegate19, this.swigDelegate20, this.swigDelegate21);
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x00019070 File Offset: 0x00017270
	private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes)
	{
		return base.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, methodTypes, null).DeclaringType.IsSubclassOf(typeof(MetaHandler));
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00019097 File Offset: 0x00017297
	private void SwigDirectorSetCurrentThread(uint id)
	{
		this.SetCurrentThread(id);
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x000190A0 File Offset: 0x000172A0
	private void SwigDirectorProcessTextAscii(IntPtr pData, uint len)
	{
		this.ProcessTextAscii(pData, len);
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x000190AA File Offset: 0x000172AA
	private void SwigDirectorProcessTextXML(IntPtr pData, uint len)
	{
		this.ProcessTextXML(pData, len);
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x000190B4 File Offset: 0x000172B4
	private void SwigDirectorProcessTextJSON(IntPtr pData, uint len)
	{
		this.ProcessTextJSON(pData, len);
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x000190BE File Offset: 0x000172BE
	private void SwigDirectorProcessDisplayMessageCommand(string message)
	{
		this.ProcessDisplayMessageCommand(message);
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x000190C7 File Offset: 0x000172C7
	private void SwigDirectorProcessSetWindowSizeCommand(uint displayId, uint surfaceId, uint width, uint height)
	{
		this.ProcessSetWindowSizeCommand(displayId, surfaceId, width, height);
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x000190D4 File Offset: 0x000172D4
	private void SwigDirectorProcessCreateEGLImageBufferCommand(uint displayId, uint imageId, IntPtr pData)
	{
		this.ProcessCreateEGLImageBufferCommand(displayId, imageId, pData);
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x000190DF File Offset: 0x000172DF
	private void SwigDirectorProcessSetEGLImageContentCommand(uint imageId, IntPtr pData)
	{
		this.ProcessSetEGLImageContentCommand(imageId, pData);
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x000190E9 File Offset: 0x000172E9
	private void SwigDirectorProcessEGLConfigStateDesc(uint displayId, uint configId, IntPtr pAttribList, uint len)
	{
		this.ProcessEGLConfigStateDesc(displayId, configId, pAttribList, len);
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x000190F6 File Offset: 0x000172F6
	private void SwigDirectorProcessEGLSurfaceStateDesc(uint displayId, uint configId, IntPtr pAttribList, uint len)
	{
		this.ProcessEGLSurfaceStateDesc(displayId, configId, pAttribList, len);
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00019103 File Offset: 0x00017303
	private void SwigDirectorProcessGLUniformsStateDesc(uint programId, uint numUniforms, IntPtr pData)
	{
		this.ProcessGLUniformsStateDesc(programId, numUniforms, pData);
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0001910E File Offset: 0x0001730E
	private void SwigDirectorProcessGLUniformBlocksStateDesc(uint programId, uint numUniformBlocks, IntPtr pData)
	{
		this.ProcessGLUniformBlocksStateDesc(programId, numUniformBlocks, pData);
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00019119 File Offset: 0x00017319
	private void SwigDirectorProcessGLAttributesStateDesc(uint programId, uint numAttributes, IntPtr pData)
	{
		this.ProcessGLAttributesStateDesc(programId, numAttributes, pData);
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00019124 File Offset: 0x00017324
	private void SwigDirectorProcessGLAtomicCounterBufferStateDesc(uint programId, uint numBuffers, IntPtr pData)
	{
		this.ProcessGLAtomicCounterBufferStateDesc(programId, numBuffers, pData);
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0001912F File Offset: 0x0001732F
	private void SwigDirectorProcessGLBufferVariablesStateDesc(uint programId, uint numBuffers, IntPtr pData)
	{
		this.ProcessGLBufferVariablesStateDesc(programId, numBuffers, pData);
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0001913A File Offset: 0x0001733A
	private void SwigDirectorProcessGLStorageBlocksStateDesc(uint programId, uint numBuffers, IntPtr pData)
	{
		this.ProcessGLStorageBlocksStateDesc(programId, numBuffers, pData);
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00019145 File Offset: 0x00017345
	private void SwigDirectorProcessProfilerData(uint subtype, IntPtr pData, uint len)
	{
		this.ProcessProfilerData(subtype, pData, len);
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00019150 File Offset: 0x00017350
	private void SwigDirectorProcessGLLimits(uint numLimits, IntPtr pLimits)
	{
		this.ProcessGLLimits(numLimits, pLimits);
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0001915C File Offset: 0x0001735C
	private void SwigDirectorProcessDefaultColorBuffer(uint displayId, uint surfaceId, uint width, uint height, uint format, uint type, uint buffSize, IntPtr pData)
	{
		this.ProcessDefaultColorBuffer(displayId, surfaceId, width, height, format, type, buffSize, pData);
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0001917C File Offset: 0x0001737C
	private void SwigDirectorProcessDefaultDepthBuffer(uint displayId, uint surfaceId, uint width, uint height, uint format, uint type, uint buffSize, IntPtr pData)
	{
		this.ProcessDefaultDepthBuffer(displayId, surfaceId, width, height, format, type, buffSize, pData);
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0001919C File Offset: 0x0001739C
	private void SwigDirectorProcessDefaultStencilBuffer(uint displayId, uint surfaceId, uint width, uint height, uint format, uint type, uint buffSize, IntPtr pData)
	{
		this.ProcessDefaultStencilBuffer(displayId, surfaceId, width, height, format, type, buffSize, pData);
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x000191BC File Offset: 0x000173BC
	private void SwigDirectorProcessMultiSampleTexture(uint target, uint texture, uint width, uint height, uint depth, uint internalFormat, uint format, uint type, uint layerSize, uint buffSize, IntPtr pData)
	{
		this.ProcessMultiSampleTexture(target, texture, width, height, depth, internalFormat, format, type, layerSize, buffSize, pData);
	}

	// Token: 0x040009C2 RID: 2498
	private HandleRef swigCPtr;

	// Token: 0x040009C3 RID: 2499
	protected bool swigCMemOwn;

	// Token: 0x040009C4 RID: 2500
	private MetaHandler.SwigDelegateMetaHandler_0 swigDelegate0;

	// Token: 0x040009C5 RID: 2501
	private MetaHandler.SwigDelegateMetaHandler_1 swigDelegate1;

	// Token: 0x040009C6 RID: 2502
	private MetaHandler.SwigDelegateMetaHandler_2 swigDelegate2;

	// Token: 0x040009C7 RID: 2503
	private MetaHandler.SwigDelegateMetaHandler_3 swigDelegate3;

	// Token: 0x040009C8 RID: 2504
	private MetaHandler.SwigDelegateMetaHandler_4 swigDelegate4;

	// Token: 0x040009C9 RID: 2505
	private MetaHandler.SwigDelegateMetaHandler_5 swigDelegate5;

	// Token: 0x040009CA RID: 2506
	private MetaHandler.SwigDelegateMetaHandler_6 swigDelegate6;

	// Token: 0x040009CB RID: 2507
	private MetaHandler.SwigDelegateMetaHandler_7 swigDelegate7;

	// Token: 0x040009CC RID: 2508
	private MetaHandler.SwigDelegateMetaHandler_8 swigDelegate8;

	// Token: 0x040009CD RID: 2509
	private MetaHandler.SwigDelegateMetaHandler_9 swigDelegate9;

	// Token: 0x040009CE RID: 2510
	private MetaHandler.SwigDelegateMetaHandler_10 swigDelegate10;

	// Token: 0x040009CF RID: 2511
	private MetaHandler.SwigDelegateMetaHandler_11 swigDelegate11;

	// Token: 0x040009D0 RID: 2512
	private MetaHandler.SwigDelegateMetaHandler_12 swigDelegate12;

	// Token: 0x040009D1 RID: 2513
	private MetaHandler.SwigDelegateMetaHandler_13 swigDelegate13;

	// Token: 0x040009D2 RID: 2514
	private MetaHandler.SwigDelegateMetaHandler_14 swigDelegate14;

	// Token: 0x040009D3 RID: 2515
	private MetaHandler.SwigDelegateMetaHandler_15 swigDelegate15;

	// Token: 0x040009D4 RID: 2516
	private MetaHandler.SwigDelegateMetaHandler_16 swigDelegate16;

	// Token: 0x040009D5 RID: 2517
	private MetaHandler.SwigDelegateMetaHandler_17 swigDelegate17;

	// Token: 0x040009D6 RID: 2518
	private MetaHandler.SwigDelegateMetaHandler_18 swigDelegate18;

	// Token: 0x040009D7 RID: 2519
	private MetaHandler.SwigDelegateMetaHandler_19 swigDelegate19;

	// Token: 0x040009D8 RID: 2520
	private MetaHandler.SwigDelegateMetaHandler_20 swigDelegate20;

	// Token: 0x040009D9 RID: 2521
	private MetaHandler.SwigDelegateMetaHandler_21 swigDelegate21;

	// Token: 0x040009DA RID: 2522
	private static Type[] swigMethodTypes0 = new Type[] { typeof(uint) };

	// Token: 0x040009DB RID: 2523
	private static Type[] swigMethodTypes1 = new Type[]
	{
		typeof(IntPtr),
		typeof(uint)
	};

	// Token: 0x040009DC RID: 2524
	private static Type[] swigMethodTypes2 = new Type[]
	{
		typeof(IntPtr),
		typeof(uint)
	};

	// Token: 0x040009DD RID: 2525
	private static Type[] swigMethodTypes3 = new Type[]
	{
		typeof(IntPtr),
		typeof(uint)
	};

	// Token: 0x040009DE RID: 2526
	private static Type[] swigMethodTypes4 = new Type[] { typeof(string) };

	// Token: 0x040009DF RID: 2527
	private static Type[] swigMethodTypes5 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040009E0 RID: 2528
	private static Type[] swigMethodTypes6 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009E1 RID: 2529
	private static Type[] swigMethodTypes7 = new Type[]
	{
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009E2 RID: 2530
	private static Type[] swigMethodTypes8 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr),
		typeof(uint)
	};

	// Token: 0x040009E3 RID: 2531
	private static Type[] swigMethodTypes9 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr),
		typeof(uint)
	};

	// Token: 0x040009E4 RID: 2532
	private static Type[] swigMethodTypes10 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009E5 RID: 2533
	private static Type[] swigMethodTypes11 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009E6 RID: 2534
	private static Type[] swigMethodTypes12 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009E7 RID: 2535
	private static Type[] swigMethodTypes13 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009E8 RID: 2536
	private static Type[] swigMethodTypes14 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009E9 RID: 2537
	private static Type[] swigMethodTypes15 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009EA RID: 2538
	private static Type[] swigMethodTypes16 = new Type[]
	{
		typeof(uint),
		typeof(IntPtr),
		typeof(uint)
	};

	// Token: 0x040009EB RID: 2539
	private static Type[] swigMethodTypes17 = new Type[]
	{
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009EC RID: 2540
	private static Type[] swigMethodTypes18 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009ED RID: 2541
	private static Type[] swigMethodTypes19 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009EE RID: 2542
	private static Type[] swigMethodTypes20 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040009EF RID: 2543
	private static Type[] swigMethodTypes21 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x02000281 RID: 641
	// (Invoke) Token: 0x060012CE RID: 4814
	public delegate void SwigDelegateMetaHandler_0(uint id);

	// Token: 0x02000282 RID: 642
	// (Invoke) Token: 0x060012D2 RID: 4818
	public delegate void SwigDelegateMetaHandler_1(IntPtr pData, uint len);

	// Token: 0x02000283 RID: 643
	// (Invoke) Token: 0x060012D6 RID: 4822
	public delegate void SwigDelegateMetaHandler_2(IntPtr pData, uint len);

	// Token: 0x02000284 RID: 644
	// (Invoke) Token: 0x060012DA RID: 4826
	public delegate void SwigDelegateMetaHandler_3(IntPtr pData, uint len);

	// Token: 0x02000285 RID: 645
	// (Invoke) Token: 0x060012DE RID: 4830
	public delegate void SwigDelegateMetaHandler_4(string message);

	// Token: 0x02000286 RID: 646
	// (Invoke) Token: 0x060012E2 RID: 4834
	public delegate void SwigDelegateMetaHandler_5(uint displayId, uint surfaceId, uint width, uint height);

	// Token: 0x02000287 RID: 647
	// (Invoke) Token: 0x060012E6 RID: 4838
	public delegate void SwigDelegateMetaHandler_6(uint displayId, uint imageId, IntPtr pData);

	// Token: 0x02000288 RID: 648
	// (Invoke) Token: 0x060012EA RID: 4842
	public delegate void SwigDelegateMetaHandler_7(uint imageId, IntPtr pData);

	// Token: 0x02000289 RID: 649
	// (Invoke) Token: 0x060012EE RID: 4846
	public delegate void SwigDelegateMetaHandler_8(uint displayId, uint configId, IntPtr pAttribList, uint len);

	// Token: 0x0200028A RID: 650
	// (Invoke) Token: 0x060012F2 RID: 4850
	public delegate void SwigDelegateMetaHandler_9(uint displayId, uint configId, IntPtr pAttribList, uint len);

	// Token: 0x0200028B RID: 651
	// (Invoke) Token: 0x060012F6 RID: 4854
	public delegate void SwigDelegateMetaHandler_10(uint programId, uint numUniforms, IntPtr pData);

	// Token: 0x0200028C RID: 652
	// (Invoke) Token: 0x060012FA RID: 4858
	public delegate void SwigDelegateMetaHandler_11(uint programId, uint numUniformBlocks, IntPtr pData);

	// Token: 0x0200028D RID: 653
	// (Invoke) Token: 0x060012FE RID: 4862
	public delegate void SwigDelegateMetaHandler_12(uint programId, uint numAttributes, IntPtr pData);

	// Token: 0x0200028E RID: 654
	// (Invoke) Token: 0x06001302 RID: 4866
	public delegate void SwigDelegateMetaHandler_13(uint programId, uint numBuffers, IntPtr pData);

	// Token: 0x0200028F RID: 655
	// (Invoke) Token: 0x06001306 RID: 4870
	public delegate void SwigDelegateMetaHandler_14(uint programId, uint numBuffers, IntPtr pData);

	// Token: 0x02000290 RID: 656
	// (Invoke) Token: 0x0600130A RID: 4874
	public delegate void SwigDelegateMetaHandler_15(uint programId, uint numBuffers, IntPtr pData);

	// Token: 0x02000291 RID: 657
	// (Invoke) Token: 0x0600130E RID: 4878
	public delegate void SwigDelegateMetaHandler_16(uint subtype, IntPtr pData, uint len);

	// Token: 0x02000292 RID: 658
	// (Invoke) Token: 0x06001312 RID: 4882
	public delegate void SwigDelegateMetaHandler_17(uint numLimits, IntPtr pLimits);

	// Token: 0x02000293 RID: 659
	// (Invoke) Token: 0x06001316 RID: 4886
	public delegate void SwigDelegateMetaHandler_18(uint displayId, uint surfaceId, uint width, uint height, uint format, uint type, uint buffSize, IntPtr pData);

	// Token: 0x02000294 RID: 660
	// (Invoke) Token: 0x0600131A RID: 4890
	public delegate void SwigDelegateMetaHandler_19(uint displayId, uint surfaceId, uint width, uint height, uint format, uint type, uint buffSize, IntPtr pData);

	// Token: 0x02000295 RID: 661
	// (Invoke) Token: 0x0600131E RID: 4894
	public delegate void SwigDelegateMetaHandler_20(uint displayId, uint surfaceId, uint width, uint height, uint format, uint type, uint buffSize, IntPtr pData);

	// Token: 0x02000296 RID: 662
	// (Invoke) Token: 0x06001322 RID: 4898
	public delegate void SwigDelegateMetaHandler_21(uint target, uint texture, uint width, uint height, uint depth, uint internalFormat, uint format, uint type, uint layerSize, uint buffSize, IntPtr pData);
}
