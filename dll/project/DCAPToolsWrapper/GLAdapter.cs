using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x02000015 RID: 21
public class GLAdapter : Adapter
{
	// Token: 0x0600014B RID: 331 RVA: 0x00005EA8 File Offset: 0x000040A8
	internal GLAdapter(IntPtr cPtr, bool cMemoryOwn)
		: base(libDCAPPINVOKE.GLAdapter_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00005EC4 File Offset: 0x000040C4
	internal static HandleRef getCPtr(GLAdapter obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00005EDC File Offset: 0x000040DC
	~GLAdapter()
	{
		this.Dispose();
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00005F08 File Offset: 0x00004108
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_GLAdapter(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00005F8C File Offset: 0x0000418C
	public GLAdapter()
		: this(libDCAPPINVOKE.new_GLAdapter(), true)
	{
		this.SwigDirectorConnect();
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00005FA0 File Offset: 0x000041A0
	public override void SetCurrentThread(uint id)
	{
		libDCAPPINVOKE.GLAdapter_SetCurrentThread(this.swigCPtr, id);
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00005FB0 File Offset: 0x000041B0
	public virtual void ProcessVertexAttribData(uint index, int size, uint type, uint normalized, int stride, IntPtr pData, uint dataSize, uint dataOffset)
	{
		libDCAPPINVOKE.GLAdapter_ProcessVertexAttribData(this.swigCPtr, index, size, type, normalized, stride, pData, dataSize, dataOffset);
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00005FD5 File Offset: 0x000041D5
	public virtual void ProcessVertexAttribIData(uint index, int size, uint type, int stride, IntPtr pData, uint dataSize, uint dataOffset)
	{
		libDCAPPINVOKE.GLAdapter_ProcessVertexAttribIData(this.swigCPtr, index, size, type, stride, pData, dataSize, dataOffset);
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00005FED File Offset: 0x000041ED
	public virtual void ProcessFlushMappedBufferRange(uint target, uint offset, uint length, IntPtr pData)
	{
		libDCAPPINVOKE.GLAdapter_ProcessFlushMappedBufferRange(this.swigCPtr, target, offset, length, pData);
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00005FFF File Offset: 0x000041FF
	public virtual void ProcessUnmapBuffer(uint target, uint length, IntPtr pData)
	{
		libDCAPPINVOKE.GLAdapter_ProcessUnmapBuffer(this.swigCPtr, target, length, pData);
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0000600F File Offset: 0x0000420F
	public virtual void Process_glActiveTexture(uint texture)
	{
		libDCAPPINVOKE.GLAdapter_Process_glActiveTexture(this.swigCPtr, texture);
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000601D File Offset: 0x0000421D
	public virtual void Process_glAttachShader(uint program, uint shader)
	{
		libDCAPPINVOKE.GLAdapter_Process_glAttachShader(this.swigCPtr, program, shader);
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000602C File Offset: 0x0000422C
	public virtual void Process_glBindAttribLocation(uint program, uint index, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindAttribLocation(this.swigCPtr, program, index, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00006041 File Offset: 0x00004241
	public virtual void Process_glBindBuffer(uint target, uint buffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindBuffer(this.swigCPtr, target, buffer);
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00006050 File Offset: 0x00004250
	public virtual void Process_glBindFramebuffer(uint target, uint framebuffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindFramebuffer(this.swigCPtr, target, framebuffer);
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000605F File Offset: 0x0000425F
	public virtual void Process_glBindRenderbuffer(uint target, uint renderbuffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindRenderbuffer(this.swigCPtr, target, renderbuffer);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000606E File Offset: 0x0000426E
	public virtual void Process_glBindTexture(uint target, uint texture)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindTexture(this.swigCPtr, target, texture);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000607D File Offset: 0x0000427D
	public virtual void Process_glBlendColor(float red, float green, float blue, float alpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendColor(this.swigCPtr, red, green, blue, alpha);
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000608F File Offset: 0x0000428F
	public virtual void Process_glBlendEquation(uint mode)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendEquation(this.swigCPtr, mode);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000609D File Offset: 0x0000429D
	public virtual void Process_glBlendEquationSeparate(uint modeRGB, uint modeAlpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendEquationSeparate(this.swigCPtr, modeRGB, modeAlpha);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x000060AC File Offset: 0x000042AC
	public virtual void Process_glBlendFunc(uint sfactor, uint dfactor)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendFunc(this.swigCPtr, sfactor, dfactor);
	}

	// Token: 0x06000160 RID: 352 RVA: 0x000060BB File Offset: 0x000042BB
	public virtual void Process_glBlendFuncSeparate(uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendFuncSeparate(this.swigCPtr, srcRGB, dstRGB, srcAlpha, dstAlpha);
	}

	// Token: 0x06000161 RID: 353 RVA: 0x000060CD File Offset: 0x000042CD
	public virtual void Process_glBufferData(uint target, int size, PointerData pDataPtrData, uint usage)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBufferData(this.swigCPtr, target, size, PointerData.getCPtr(pDataPtrData), usage);
	}

	// Token: 0x06000162 RID: 354 RVA: 0x000060E4 File Offset: 0x000042E4
	public virtual void Process_glBufferSubData(uint target, int offset, int size, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBufferSubData(this.swigCPtr, target, offset, size, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x06000163 RID: 355 RVA: 0x000060FB File Offset: 0x000042FB
	public virtual void Process_glCheckFramebufferStatus(uint returnVal, uint target)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCheckFramebufferStatus(this.swigCPtr, returnVal, target);
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000610A File Offset: 0x0000430A
	public virtual void Process_glClear(uint mask)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClear(this.swigCPtr, mask);
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00006118 File Offset: 0x00004318
	public virtual void Process_glClearColor(float red, float green, float blue, float alpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClearColor(this.swigCPtr, red, green, blue, alpha);
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000612A File Offset: 0x0000432A
	public virtual void Process_glClearDepthf(float depth)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClearDepthf(this.swigCPtr, depth);
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00006138 File Offset: 0x00004338
	public virtual void Process_glClearStencil(int s)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClearStencil(this.swigCPtr, s);
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00006146 File Offset: 0x00004346
	public virtual void Process_glColorMask(int red, int green, int blue, int alpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glColorMask(this.swigCPtr, red, green, blue, alpha);
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00006158 File Offset: 0x00004358
	public virtual void Process_glCompileShader(uint shader)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCompileShader(this.swigCPtr, shader);
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00006168 File Offset: 0x00004368
	public virtual void Process_glCompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCompressedTexImage2D(this.swigCPtr, target, level, internalformat, width, height, border, imageSize, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00006194 File Offset: 0x00004394
	public virtual void Process_glCompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCompressedTexSubImage2D(this.swigCPtr, target, level, xoffset, yoffset, width, height, format, imageSize, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x0600016C RID: 364 RVA: 0x000061C0 File Offset: 0x000043C0
	public virtual void Process_glCopyTexImage2D(uint target, int level, uint internalformat, int x, int y, int width, int height, int border)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCopyTexImage2D(this.swigCPtr, target, level, internalformat, x, y, width, height, border);
	}

	// Token: 0x0600016D RID: 365 RVA: 0x000061E8 File Offset: 0x000043E8
	public virtual void Process_glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCopyTexSubImage2D(this.swigCPtr, target, level, xoffset, yoffset, x, y, width, height);
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000620D File Offset: 0x0000440D
	public virtual void Process_glCreateProgram(uint returnVal)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCreateProgram(this.swigCPtr, returnVal);
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000621B File Offset: 0x0000441B
	public virtual void Process_glCreateShader(uint returnVal, uint type)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCreateShader(this.swigCPtr, returnVal, type);
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000622A File Offset: 0x0000442A
	public virtual void Process_glCullFace(uint mode)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCullFace(this.swigCPtr, mode);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00006238 File Offset: 0x00004438
	public virtual void Process_glDeleteBuffers(int n, PointerData pBuffersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteBuffers(this.swigCPtr, n, PointerData.getCPtr(pBuffersPtrData));
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000624C File Offset: 0x0000444C
	public virtual void Process_glDeleteFramebuffers(int n, PointerData pFramebuffersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteFramebuffers(this.swigCPtr, n, PointerData.getCPtr(pFramebuffersPtrData));
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00006260 File Offset: 0x00004460
	public virtual void Process_glDeleteProgram(uint program)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteProgram(this.swigCPtr, program);
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000626E File Offset: 0x0000446E
	public virtual void Process_glDeleteRenderbuffers(int n, PointerData pRenderbuffersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteRenderbuffers(this.swigCPtr, n, PointerData.getCPtr(pRenderbuffersPtrData));
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00006282 File Offset: 0x00004482
	public virtual void Process_glDeleteShader(uint shader)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteShader(this.swigCPtr, shader);
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00006290 File Offset: 0x00004490
	public virtual void Process_glDeleteTextures(int n, PointerData pTexturesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteTextures(this.swigCPtr, n, PointerData.getCPtr(pTexturesPtrData));
	}

	// Token: 0x06000177 RID: 375 RVA: 0x000062A4 File Offset: 0x000044A4
	public virtual void Process_glDepthFunc(uint func)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDepthFunc(this.swigCPtr, func);
	}

	// Token: 0x06000178 RID: 376 RVA: 0x000062B2 File Offset: 0x000044B2
	public virtual void Process_glDepthMask(int flag)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDepthMask(this.swigCPtr, flag);
	}

	// Token: 0x06000179 RID: 377 RVA: 0x000062C0 File Offset: 0x000044C0
	public virtual void Process_glDepthRangef(float n, float f)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDepthRangef(this.swigCPtr, n, f);
	}

	// Token: 0x0600017A RID: 378 RVA: 0x000062CF File Offset: 0x000044CF
	public virtual void Process_glDetachShader(uint program, uint shader)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDetachShader(this.swigCPtr, program, shader);
	}

	// Token: 0x0600017B RID: 379 RVA: 0x000062DE File Offset: 0x000044DE
	public virtual void Process_glDisable(uint cap)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDisable(this.swigCPtr, cap);
	}

	// Token: 0x0600017C RID: 380 RVA: 0x000062EC File Offset: 0x000044EC
	public virtual void Process_glDisableVertexAttribArray(uint index)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDisableVertexAttribArray(this.swigCPtr, index);
	}

	// Token: 0x0600017D RID: 381 RVA: 0x000062FA File Offset: 0x000044FA
	public virtual void Process_glDrawArrays(uint mode, int first, int count)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawArrays(this.swigCPtr, mode, first, count);
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000630A File Offset: 0x0000450A
	public virtual void Process_glDrawElements(uint mode, int count, uint type, PointerData pIndicesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawElements(this.swigCPtr, mode, count, type, PointerData.getCPtr(pIndicesPtrData));
	}

	// Token: 0x0600017F RID: 383 RVA: 0x00006321 File Offset: 0x00004521
	public virtual void Process_glEnable(uint cap)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEnable(this.swigCPtr, cap);
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000632F File Offset: 0x0000452F
	public virtual void Process_glEnableVertexAttribArray(uint index)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEnableVertexAttribArray(this.swigCPtr, index);
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000633D File Offset: 0x0000453D
	public virtual void Process_glFinish()
	{
		libDCAPPINVOKE.GLAdapter_Process_glFinish(this.swigCPtr);
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000634A File Offset: 0x0000454A
	public virtual void Process_glFlush()
	{
		libDCAPPINVOKE.GLAdapter_Process_glFlush(this.swigCPtr);
	}

	// Token: 0x06000183 RID: 387 RVA: 0x00006357 File Offset: 0x00004557
	public virtual void Process_glFramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferRenderbuffer(this.swigCPtr, target, attachment, renderbuffertarget, renderbuffer);
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00006369 File Offset: 0x00004569
	public virtual void Process_glFramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferTexture2D(this.swigCPtr, target, attachment, textarget, texture, level);
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000637D File Offset: 0x0000457D
	public virtual void Process_glFrontFace(uint mode)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFrontFace(this.swigCPtr, mode);
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000638B File Offset: 0x0000458B
	public virtual void Process_glGenBuffers(int n, PointerData pBuffersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenBuffers(this.swigCPtr, n, PointerData.getCPtr(pBuffersPtrData));
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000639F File Offset: 0x0000459F
	public virtual void Process_glGenerateMipmap(uint target)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenerateMipmap(this.swigCPtr, target);
	}

	// Token: 0x06000188 RID: 392 RVA: 0x000063AD File Offset: 0x000045AD
	public virtual void Process_glGenFramebuffers(int n, PointerData pFramebuffersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenFramebuffers(this.swigCPtr, n, PointerData.getCPtr(pFramebuffersPtrData));
	}

	// Token: 0x06000189 RID: 393 RVA: 0x000063C1 File Offset: 0x000045C1
	public virtual void Process_glGenRenderbuffers(int n, PointerData pRenderbuffersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenRenderbuffers(this.swigCPtr, n, PointerData.getCPtr(pRenderbuffersPtrData));
	}

	// Token: 0x0600018A RID: 394 RVA: 0x000063D5 File Offset: 0x000045D5
	public virtual void Process_glGenTextures(int n, PointerData pTexturesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenTextures(this.swigCPtr, n, PointerData.getCPtr(pTexturesPtrData));
	}

	// Token: 0x0600018B RID: 395 RVA: 0x000063E9 File Offset: 0x000045E9
	public virtual void Process_glGetActiveAttrib(uint program, uint index, int bufsize, PointerData pLengthPtrData, PointerData pSizePtrData, PointerData pTypePtrData, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetActiveAttrib(this.swigCPtr, program, index, bufsize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pSizePtrData), PointerData.getCPtr(pTypePtrData), PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00006415 File Offset: 0x00004615
	public virtual void Process_glGetActiveUniform(uint program, uint index, int bufsize, PointerData pLengthPtrData, PointerData pSizePtrData, PointerData pTypePtrData, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetActiveUniform(this.swigCPtr, program, index, bufsize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pSizePtrData), PointerData.getCPtr(pTypePtrData), PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00006441 File Offset: 0x00004641
	public virtual void Process_glGetAttachedShaders(uint program, int maxcount, PointerData pCountPtrData, PointerData pShadersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetAttachedShaders(this.swigCPtr, program, maxcount, PointerData.getCPtr(pCountPtrData), PointerData.getCPtr(pShadersPtrData));
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000645D File Offset: 0x0000465D
	public virtual void Process_glGetAttribLocation(uint returnVal, uint program, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetAttribLocation(this.swigCPtr, returnVal, program, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00006472 File Offset: 0x00004672
	public virtual void Process_glGetBooleanv(uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetBooleanv(this.swigCPtr, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00006486 File Offset: 0x00004686
	public virtual void Process_glGetBufferParameteriv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetBufferParameteriv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000649B File Offset: 0x0000469B
	public virtual void Process_glGetError(uint returnVal)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetError(this.swigCPtr, returnVal);
	}

	// Token: 0x06000192 RID: 402 RVA: 0x000064A9 File Offset: 0x000046A9
	public virtual void Process_glGetFloatv(uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetFloatv(this.swigCPtr, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000193 RID: 403 RVA: 0x000064BD File Offset: 0x000046BD
	public virtual void Process_glGetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetFramebufferAttachmentParameteriv(this.swigCPtr, target, attachment, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000194 RID: 404 RVA: 0x000064D4 File Offset: 0x000046D4
	public virtual void Process_glGetIntegerv(uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetIntegerv(this.swigCPtr, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000195 RID: 405 RVA: 0x000064E8 File Offset: 0x000046E8
	public virtual void Process_glGetProgramiv(uint program, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramiv(this.swigCPtr, program, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000196 RID: 406 RVA: 0x000064FD File Offset: 0x000046FD
	public virtual void Process_glGetProgramInfoLog(uint program, int bufsize, PointerData pLengthPtrData, PointerData pInfologPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramInfoLog(this.swigCPtr, program, bufsize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pInfologPtrData));
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00006519 File Offset: 0x00004719
	public virtual void Process_glGetRenderbufferParameteriv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetRenderbufferParameteriv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000652E File Offset: 0x0000472E
	public virtual void Process_glGetShaderiv(uint shader, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetShaderiv(this.swigCPtr, shader, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00006543 File Offset: 0x00004743
	public virtual void Process_glGetShaderInfoLog(uint shader, int bufsize, PointerData pLengthPtrData, PointerData pInfologPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetShaderInfoLog(this.swigCPtr, shader, bufsize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pInfologPtrData));
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000655F File Offset: 0x0000475F
	public virtual void Process_glGetShaderPrecisionFormat(uint shadertype, uint precisiontype, PointerData pRangePtrData, PointerData pPrecisionPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetShaderPrecisionFormat(this.swigCPtr, shadertype, precisiontype, PointerData.getCPtr(pRangePtrData), PointerData.getCPtr(pPrecisionPtrData));
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000657B File Offset: 0x0000477B
	public virtual void Process_glGetShaderSource(uint shader, int bufsize, PointerData pLengthPtrData, PointerData pSourcePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetShaderSource(this.swigCPtr, shader, bufsize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pSourcePtrData));
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00006597 File Offset: 0x00004797
	public virtual void Process_glGetString(PointerData pReturnPtrData, uint name)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetString(this.swigCPtr, PointerData.getCPtr(pReturnPtrData), name);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x000065AB File Offset: 0x000047AB
	public virtual void Process_glGetTexParameterfv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetTexParameterfv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600019E RID: 414 RVA: 0x000065C0 File Offset: 0x000047C0
	public virtual void Process_glGetTexParameteriv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetTexParameteriv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600019F RID: 415 RVA: 0x000065D5 File Offset: 0x000047D5
	public virtual void Process_glGetUniformfv(uint program, uint location, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetUniformfv(this.swigCPtr, program, location, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000065EA File Offset: 0x000047EA
	public virtual void Process_glGetUniformiv(uint program, uint location, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetUniformiv(this.swigCPtr, program, location, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x000065FF File Offset: 0x000047FF
	public virtual void Process_glGetUniformLocation(uint returnVal, uint program, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetUniformLocation(this.swigCPtr, returnVal, program, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00006614 File Offset: 0x00004814
	public virtual void Process_glGetVertexAttribfv(uint index, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetVertexAttribfv(this.swigCPtr, index, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00006629 File Offset: 0x00004829
	public virtual void Process_glGetVertexAttribiv(uint index, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetVertexAttribiv(this.swigCPtr, index, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000663E File Offset: 0x0000483E
	public virtual void Process_glGetVertexAttribPointerv(uint index, uint pname, PointerData pPointerPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetVertexAttribPointerv(this.swigCPtr, index, pname, PointerData.getCPtr(pPointerPtrData));
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00006653 File Offset: 0x00004853
	public virtual void Process_glHint(uint target, uint mode)
	{
		libDCAPPINVOKE.GLAdapter_Process_glHint(this.swigCPtr, target, mode);
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00006662 File Offset: 0x00004862
	public virtual void Process_glIsBuffer(int returnVal, uint buffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsBuffer(this.swigCPtr, returnVal, buffer);
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00006671 File Offset: 0x00004871
	public virtual void Process_glIsEnabled(int returnVal, uint cap)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsEnabled(this.swigCPtr, returnVal, cap);
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00006680 File Offset: 0x00004880
	public virtual void Process_glIsFramebuffer(int returnVal, uint framebuffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsFramebuffer(this.swigCPtr, returnVal, framebuffer);
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000668F File Offset: 0x0000488F
	public virtual void Process_glIsProgram(int returnVal, uint program)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsProgram(this.swigCPtr, returnVal, program);
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000669E File Offset: 0x0000489E
	public virtual void Process_glIsRenderbuffer(int returnVal, uint renderbuffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsRenderbuffer(this.swigCPtr, returnVal, renderbuffer);
	}

	// Token: 0x060001AB RID: 427 RVA: 0x000066AD File Offset: 0x000048AD
	public virtual void Process_glIsShader(int returnVal, uint shader)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsShader(this.swigCPtr, returnVal, shader);
	}

	// Token: 0x060001AC RID: 428 RVA: 0x000066BC File Offset: 0x000048BC
	public virtual void Process_glIsTexture(int returnVal, uint texture)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsTexture(this.swigCPtr, returnVal, texture);
	}

	// Token: 0x060001AD RID: 429 RVA: 0x000066CB File Offset: 0x000048CB
	public virtual void Process_glLineWidth(float width)
	{
		libDCAPPINVOKE.GLAdapter_Process_glLineWidth(this.swigCPtr, width);
	}

	// Token: 0x060001AE RID: 430 RVA: 0x000066D9 File Offset: 0x000048D9
	public virtual void Process_glLinkProgram(uint program)
	{
		libDCAPPINVOKE.GLAdapter_Process_glLinkProgram(this.swigCPtr, program);
	}

	// Token: 0x060001AF RID: 431 RVA: 0x000066E7 File Offset: 0x000048E7
	public virtual void Process_glPixelStorei(uint pname, int param)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPixelStorei(this.swigCPtr, pname, param);
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x000066F6 File Offset: 0x000048F6
	public virtual void Process_glPolygonOffset(float factor, float units)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPolygonOffset(this.swigCPtr, factor, units);
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00006705 File Offset: 0x00004905
	public virtual void Process_glReadPixels(int x, int y, int width, int height, uint format, uint type, PointerData pPixelsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glReadPixels(this.swigCPtr, x, y, width, height, format, type, PointerData.getCPtr(pPixelsPtrData));
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00006722 File Offset: 0x00004922
	public virtual void Process_glReleaseShaderCompiler()
	{
		libDCAPPINVOKE.GLAdapter_Process_glReleaseShaderCompiler(this.swigCPtr);
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000672F File Offset: 0x0000492F
	public virtual void Process_glRenderbufferStorage(uint target, uint internalformat, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glRenderbufferStorage(this.swigCPtr, target, internalformat, width, height);
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00006741 File Offset: 0x00004941
	public virtual void Process_glSampleCoverage(float value, int invert)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSampleCoverage(this.swigCPtr, value, invert);
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00006750 File Offset: 0x00004950
	public virtual void Process_glScissor(int x, int y, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glScissor(this.swigCPtr, x, y, width, height);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00006762 File Offset: 0x00004962
	public virtual void Process_glShaderBinary(int n, PointerData pShadersPtrData, uint binaryformat, PointerData pBinaryPtrData, int length)
	{
		libDCAPPINVOKE.GLAdapter_Process_glShaderBinary(this.swigCPtr, n, PointerData.getCPtr(pShadersPtrData), binaryformat, PointerData.getCPtr(pBinaryPtrData), length);
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00006780 File Offset: 0x00004980
	public virtual void Process_glShaderSource(uint shader, int count, PointerData pStrPtrData, PointerData pLengthPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glShaderSource(this.swigCPtr, shader, count, PointerData.getCPtr(pStrPtrData), PointerData.getCPtr(pLengthPtrData));
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000679C File Offset: 0x0000499C
	public virtual void Process_glStencilFunc(uint func, int arg1, uint mask)
	{
		libDCAPPINVOKE.GLAdapter_Process_glStencilFunc(this.swigCPtr, func, arg1, mask);
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x000067AC File Offset: 0x000049AC
	public virtual void Process_glStencilFuncSeparate(uint face, uint func, int arg2, uint mask)
	{
		libDCAPPINVOKE.GLAdapter_Process_glStencilFuncSeparate(this.swigCPtr, face, func, arg2, mask);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x000067BE File Offset: 0x000049BE
	public virtual void Process_glStencilMask(uint mask)
	{
		libDCAPPINVOKE.GLAdapter_Process_glStencilMask(this.swigCPtr, mask);
	}

	// Token: 0x060001BB RID: 443 RVA: 0x000067CC File Offset: 0x000049CC
	public virtual void Process_glStencilMaskSeparate(uint face, uint mask)
	{
		libDCAPPINVOKE.GLAdapter_Process_glStencilMaskSeparate(this.swigCPtr, face, mask);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x000067DB File Offset: 0x000049DB
	public virtual void Process_glStencilOp(uint fail, uint zfail, uint zpass)
	{
		libDCAPPINVOKE.GLAdapter_Process_glStencilOp(this.swigCPtr, fail, zfail, zpass);
	}

	// Token: 0x060001BD RID: 445 RVA: 0x000067EB File Offset: 0x000049EB
	public virtual void Process_glStencilOpSeparate(uint face, uint fail, uint zfail, uint zpass)
	{
		libDCAPPINVOKE.GLAdapter_Process_glStencilOpSeparate(this.swigCPtr, face, fail, zfail, zpass);
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00006800 File Offset: 0x00004A00
	public virtual void Process_glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, PointerData pPixelsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexImage2D(this.swigCPtr, target, level, internalformat, width, height, border, format, type, PointerData.getCPtr(pPixelsPtrData));
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000682C File Offset: 0x00004A2C
	public virtual void Process_glTexParameterf(uint target, uint pname, float param)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexParameterf(this.swigCPtr, target, pname, param);
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000683C File Offset: 0x00004A3C
	public virtual void Process_glTexParameterfv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexParameterfv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00006851 File Offset: 0x00004A51
	public virtual void Process_glTexParameteri(uint target, uint pname, int param)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexParameteri(this.swigCPtr, target, pname, param);
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00006861 File Offset: 0x00004A61
	public virtual void Process_glTexParameteriv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexParameteriv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00006878 File Offset: 0x00004A78
	public virtual void Process_glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, PointerData pPixelsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexSubImage2D(this.swigCPtr, target, level, xoffset, yoffset, width, height, format, type, PointerData.getCPtr(pPixelsPtrData));
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x000068A4 File Offset: 0x00004AA4
	public virtual void Process_glUniform1f(uint location, float x)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform1f(this.swigCPtr, location, x);
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x000068B3 File Offset: 0x00004AB3
	public virtual void Process_glUniform1fv(uint location, int count, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform1fv(this.swigCPtr, location, count, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x000068C8 File Offset: 0x00004AC8
	public virtual void Process_glUniform1i(uint location, int x)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform1i(this.swigCPtr, location, x);
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x000068D7 File Offset: 0x00004AD7
	public virtual void Process_glUniform1iv(uint location, int count, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform1iv(this.swigCPtr, location, count, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x000068EC File Offset: 0x00004AEC
	public virtual void Process_glUniform2f(uint location, float x, float y)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform2f(this.swigCPtr, location, x, y);
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x000068FC File Offset: 0x00004AFC
	public virtual void Process_glUniform2fv(uint location, int count, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform2fv(this.swigCPtr, location, count, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x060001CA RID: 458 RVA: 0x00006911 File Offset: 0x00004B11
	public virtual void Process_glUniform2i(uint location, int x, int y)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform2i(this.swigCPtr, location, x, y);
	}

	// Token: 0x060001CB RID: 459 RVA: 0x00006921 File Offset: 0x00004B21
	public virtual void Process_glUniform2iv(uint location, int count, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform2iv(this.swigCPtr, location, count, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x060001CC RID: 460 RVA: 0x00006936 File Offset: 0x00004B36
	public virtual void Process_glUniform3f(uint location, float x, float y, float z)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform3f(this.swigCPtr, location, x, y, z);
	}

	// Token: 0x060001CD RID: 461 RVA: 0x00006948 File Offset: 0x00004B48
	public virtual void Process_glUniform3fv(uint location, int count, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform3fv(this.swigCPtr, location, count, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000695D File Offset: 0x00004B5D
	public virtual void Process_glUniform3i(uint location, int x, int y, int z)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform3i(this.swigCPtr, location, x, y, z);
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000696F File Offset: 0x00004B6F
	public virtual void Process_glUniform3iv(uint location, int count, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform3iv(this.swigCPtr, location, count, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x00006984 File Offset: 0x00004B84
	public virtual void Process_glUniform4f(uint location, float x, float y, float z, float w)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform4f(this.swigCPtr, location, x, y, z, w);
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00006998 File Offset: 0x00004B98
	public virtual void Process_glUniform4fv(uint location, int count, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform4fv(this.swigCPtr, location, count, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x000069AD File Offset: 0x00004BAD
	public virtual void Process_glUniform4i(uint location, int x, int y, int z, int w)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform4i(this.swigCPtr, location, x, y, z, w);
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x000069C1 File Offset: 0x00004BC1
	public virtual void Process_glUniform4iv(uint location, int count, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform4iv(this.swigCPtr, location, count, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x000069D6 File Offset: 0x00004BD6
	public virtual void Process_glUniformMatrix2fv(uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformMatrix2fv(this.swigCPtr, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x000069ED File Offset: 0x00004BED
	public virtual void Process_glUniformMatrix3fv(uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformMatrix3fv(this.swigCPtr, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00006A04 File Offset: 0x00004C04
	public virtual void Process_glUniformMatrix4fv(uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformMatrix4fv(this.swigCPtr, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00006A1B File Offset: 0x00004C1B
	public virtual void Process_glUseProgram(uint program)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUseProgram(this.swigCPtr, program);
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00006A29 File Offset: 0x00004C29
	public virtual void Process_glValidateProgram(uint program)
	{
		libDCAPPINVOKE.GLAdapter_Process_glValidateProgram(this.swigCPtr, program);
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00006A37 File Offset: 0x00004C37
	public virtual void Process_glVertexAttrib1f(uint indx, float x)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttrib1f(this.swigCPtr, indx, x);
	}

	// Token: 0x060001DA RID: 474 RVA: 0x00006A46 File Offset: 0x00004C46
	public virtual void Process_glVertexAttrib1fv(uint indx, PointerData pValuesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttrib1fv(this.swigCPtr, indx, PointerData.getCPtr(pValuesPtrData));
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00006A5A File Offset: 0x00004C5A
	public virtual void Process_glVertexAttrib2f(uint indx, float x, float y)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttrib2f(this.swigCPtr, indx, x, y);
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00006A6A File Offset: 0x00004C6A
	public virtual void Process_glVertexAttrib2fv(uint indx, PointerData pValuesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttrib2fv(this.swigCPtr, indx, PointerData.getCPtr(pValuesPtrData));
	}

	// Token: 0x060001DD RID: 477 RVA: 0x00006A7E File Offset: 0x00004C7E
	public virtual void Process_glVertexAttrib3f(uint indx, float x, float y, float z)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttrib3f(this.swigCPtr, indx, x, y, z);
	}

	// Token: 0x060001DE RID: 478 RVA: 0x00006A90 File Offset: 0x00004C90
	public virtual void Process_glVertexAttrib3fv(uint indx, PointerData pValuesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttrib3fv(this.swigCPtr, indx, PointerData.getCPtr(pValuesPtrData));
	}

	// Token: 0x060001DF RID: 479 RVA: 0x00006AA4 File Offset: 0x00004CA4
	public virtual void Process_glVertexAttrib4f(uint indx, float x, float y, float z, float w)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttrib4f(this.swigCPtr, indx, x, y, z, w);
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00006AB8 File Offset: 0x00004CB8
	public virtual void Process_glVertexAttrib4fv(uint indx, PointerData pValuesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttrib4fv(this.swigCPtr, indx, PointerData.getCPtr(pValuesPtrData));
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00006ACC File Offset: 0x00004CCC
	public virtual void Process_glVertexAttribPointer(uint indx, int size, uint type, int normalized, int stride, PointerData pPtrPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribPointer(this.swigCPtr, indx, size, type, normalized, stride, PointerData.getCPtr(pPtrPtrData));
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x00006AE7 File Offset: 0x00004CE7
	public virtual void Process_glViewport(int x, int y, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glViewport(this.swigCPtr, x, y, width, height);
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x00006AF9 File Offset: 0x00004CF9
	public virtual void Process_glReadBuffer(uint mode)
	{
		libDCAPPINVOKE.GLAdapter_Process_glReadBuffer(this.swigCPtr, mode);
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00006B07 File Offset: 0x00004D07
	public virtual void Process_glDrawRangeElements(uint mode, uint start, uint end, int count, uint type, PointerData pIndicesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawRangeElements(this.swigCPtr, mode, start, end, count, type, PointerData.getCPtr(pIndicesPtrData));
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x00006B24 File Offset: 0x00004D24
	public virtual void Process_glTexImage3D(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, PointerData pPixelsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexImage3D(this.swigCPtr, target, level, internalformat, width, height, depth, border, format, type, PointerData.getCPtr(pPixelsPtrData));
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00006B54 File Offset: 0x00004D54
	public virtual void Process_glTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, PointerData pPixelsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexSubImage3D(this.swigCPtr, target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, PointerData.getCPtr(pPixelsPtrData));
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x00006B84 File Offset: 0x00004D84
	public virtual void Process_glCopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCopyTexSubImage3D(this.swigCPtr, target, level, xoffset, yoffset, zoffset, x, y, width, height);
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x00006BAC File Offset: 0x00004DAC
	public virtual void Process_glCompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCompressedTexImage3D(this.swigCPtr, target, level, internalformat, width, height, depth, border, imageSize, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00006BD8 File Offset: 0x00004DD8
	public virtual void Process_glCompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCompressedTexSubImage3D(this.swigCPtr, target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00006C08 File Offset: 0x00004E08
	public virtual void Process_glGenQueries(int n, PointerData pIdsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenQueries(this.swigCPtr, n, PointerData.getCPtr(pIdsPtrData));
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00006C1C File Offset: 0x00004E1C
	public virtual void Process_glDeleteQueries(int n, PointerData pIdsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteQueries(this.swigCPtr, n, PointerData.getCPtr(pIdsPtrData));
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00006C30 File Offset: 0x00004E30
	public virtual void Process_glIsQuery(int returnVal, uint id)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsQuery(this.swigCPtr, returnVal, id);
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00006C3F File Offset: 0x00004E3F
	public virtual void Process_glBeginQuery(uint target, uint id)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBeginQuery(this.swigCPtr, target, id);
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00006C4E File Offset: 0x00004E4E
	public virtual void Process_glEndQuery(uint target)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEndQuery(this.swigCPtr, target);
	}

	// Token: 0x060001EF RID: 495 RVA: 0x00006C5C File Offset: 0x00004E5C
	public virtual void Process_glGetQueryiv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetQueryiv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00006C71 File Offset: 0x00004E71
	public virtual void Process_glGetQueryObjectuiv(uint id, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetQueryObjectuiv(this.swigCPtr, id, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x00006C86 File Offset: 0x00004E86
	public virtual void Process_glUnmapBuffer(int returnVal, uint target)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUnmapBuffer(this.swigCPtr, returnVal, target);
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00006C95 File Offset: 0x00004E95
	public virtual void Process_glGetBufferPointerv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetBufferPointerv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00006CAA File Offset: 0x00004EAA
	public virtual void Process_glDrawBuffers(int n, PointerData pBufsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawBuffers(this.swigCPtr, n, PointerData.getCPtr(pBufsPtrData));
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x00006CBE File Offset: 0x00004EBE
	public virtual void Process_glUniformMatrix2x3fv(uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformMatrix2x3fv(this.swigCPtr, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00006CD5 File Offset: 0x00004ED5
	public virtual void Process_glUniformMatrix3x2fv(uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformMatrix3x2fv(this.swigCPtr, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00006CEC File Offset: 0x00004EEC
	public virtual void Process_glUniformMatrix2x4fv(uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformMatrix2x4fv(this.swigCPtr, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x00006D03 File Offset: 0x00004F03
	public virtual void Process_glUniformMatrix4x2fv(uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformMatrix4x2fv(this.swigCPtr, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x00006D1A File Offset: 0x00004F1A
	public virtual void Process_glUniformMatrix3x4fv(uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformMatrix3x4fv(this.swigCPtr, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x00006D31 File Offset: 0x00004F31
	public virtual void Process_glUniformMatrix4x3fv(uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformMatrix4x3fv(this.swigCPtr, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x060001FA RID: 506 RVA: 0x00006D48 File Offset: 0x00004F48
	public virtual void Process_glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlitFramebuffer(this.swigCPtr, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
	}

	// Token: 0x060001FB RID: 507 RVA: 0x00006D71 File Offset: 0x00004F71
	public virtual void Process_glRenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glRenderbufferStorageMultisample(this.swigCPtr, target, samples, internalformat, width, height);
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00006D85 File Offset: 0x00004F85
	public virtual void Process_glFramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferTextureLayer(this.swigCPtr, target, attachment, texture, level, layer);
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00006D99 File Offset: 0x00004F99
	public virtual void Process_glMapBufferRange(PointerData pReturnPtrData, uint target, int offset, int length, uint access)
	{
		libDCAPPINVOKE.GLAdapter_Process_glMapBufferRange(this.swigCPtr, PointerData.getCPtr(pReturnPtrData), target, offset, length, access);
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00006DB2 File Offset: 0x00004FB2
	public virtual void Process_glFlushMappedBufferRange(uint target, int offset, int length)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFlushMappedBufferRange(this.swigCPtr, target, offset, length);
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00006DC2 File Offset: 0x00004FC2
	public virtual void Process_glBindVertexArray(uint array)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindVertexArray(this.swigCPtr, array);
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00006DD0 File Offset: 0x00004FD0
	public virtual void Process_glDeleteVertexArrays(int n, PointerData pArraysPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteVertexArrays(this.swigCPtr, n, PointerData.getCPtr(pArraysPtrData));
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00006DE4 File Offset: 0x00004FE4
	public virtual void Process_glGenVertexArrays(int n, PointerData pArraysPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenVertexArrays(this.swigCPtr, n, PointerData.getCPtr(pArraysPtrData));
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00006DF8 File Offset: 0x00004FF8
	public virtual void Process_glIsVertexArray(int returnVal, uint array)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsVertexArray(this.swigCPtr, returnVal, array);
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00006E07 File Offset: 0x00005007
	public virtual void Process_glGetIntegeri_v(uint target, uint index, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetIntegeri_v(this.swigCPtr, target, index, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00006E1C File Offset: 0x0000501C
	public virtual void Process_glGetBooleani_v(uint target, uint index, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetBooleani_v(this.swigCPtr, target, index, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00006E31 File Offset: 0x00005031
	public virtual void Process_glBeginTransformFeedback(uint primitiveMode)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBeginTransformFeedback(this.swigCPtr, primitiveMode);
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00006E3F File Offset: 0x0000503F
	public virtual void Process_glEndTransformFeedback()
	{
		libDCAPPINVOKE.GLAdapter_Process_glEndTransformFeedback(this.swigCPtr);
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00006E4C File Offset: 0x0000504C
	public virtual void Process_glBindBufferRange(uint target, uint index, uint buffer, int offset, int size)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindBufferRange(this.swigCPtr, target, index, buffer, offset, size);
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00006E60 File Offset: 0x00005060
	public virtual void Process_glBindBufferBase(uint target, uint index, uint buffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindBufferBase(this.swigCPtr, target, index, buffer);
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00006E70 File Offset: 0x00005070
	public virtual void Process_glTransformFeedbackVaryings(uint program, int count, PointerData pVaryingsPtrData, uint bufferMode)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTransformFeedbackVaryings(this.swigCPtr, program, count, PointerData.getCPtr(pVaryingsPtrData), bufferMode);
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00006E87 File Offset: 0x00005087
	public virtual void Process_glGetTransformFeedbackVarying(uint program, uint index, int bufSize, PointerData pLengthPtrData, PointerData pSizePtrData, PointerData pTypePtrData, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetTransformFeedbackVarying(this.swigCPtr, program, index, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pSizePtrData), PointerData.getCPtr(pTypePtrData), PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x0600020B RID: 523 RVA: 0x00006EB3 File Offset: 0x000050B3
	public virtual void Process_glVertexAttribIPointer(uint index, int size, uint type, int stride, PointerData pPointerPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribIPointer(this.swigCPtr, index, size, type, stride, PointerData.getCPtr(pPointerPtrData));
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00006ECC File Offset: 0x000050CC
	public virtual void Process_glGetVertexAttribIiv(uint index, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetVertexAttribIiv(this.swigCPtr, index, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00006EE1 File Offset: 0x000050E1
	public virtual void Process_glGetVertexAttribIuiv(uint index, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetVertexAttribIuiv(this.swigCPtr, index, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600020E RID: 526 RVA: 0x00006EF6 File Offset: 0x000050F6
	public virtual void Process_glVertexAttribI4i(uint index, int x, int y, int z, int w)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribI4i(this.swigCPtr, index, x, y, z, w);
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00006F0A File Offset: 0x0000510A
	public virtual void Process_glVertexAttribI4ui(uint index, uint x, uint y, uint z, uint w)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribI4ui(this.swigCPtr, index, x, y, z, w);
	}

	// Token: 0x06000210 RID: 528 RVA: 0x00006F1E File Offset: 0x0000511E
	public virtual void Process_glVertexAttribI4iv(uint index, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribI4iv(this.swigCPtr, index, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x06000211 RID: 529 RVA: 0x00006F32 File Offset: 0x00005132
	public virtual void Process_glVertexAttribI4uiv(uint index, PointerData pVPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribI4uiv(this.swigCPtr, index, PointerData.getCPtr(pVPtrData));
	}

	// Token: 0x06000212 RID: 530 RVA: 0x00006F46 File Offset: 0x00005146
	public virtual void Process_glGetUniformuiv(uint program, uint location, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetUniformuiv(this.swigCPtr, program, location, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000213 RID: 531 RVA: 0x00006F5B File Offset: 0x0000515B
	public virtual void Process_glGetFragDataLocation(uint returnVal, uint program, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetFragDataLocation(this.swigCPtr, returnVal, program, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00006F70 File Offset: 0x00005170
	public virtual void Process_glUniform1ui(uint location, uint v0)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform1ui(this.swigCPtr, location, v0);
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00006F7F File Offset: 0x0000517F
	public virtual void Process_glUniform2ui(uint location, uint v0, uint v1)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform2ui(this.swigCPtr, location, v0, v1);
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00006F8F File Offset: 0x0000518F
	public virtual void Process_glUniform3ui(uint location, uint v0, uint v1, uint v2)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform3ui(this.swigCPtr, location, v0, v1, v2);
	}

	// Token: 0x06000217 RID: 535 RVA: 0x00006FA1 File Offset: 0x000051A1
	public virtual void Process_glUniform4ui(uint location, uint v0, uint v1, uint v2, uint v3)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform4ui(this.swigCPtr, location, v0, v1, v2, v3);
	}

	// Token: 0x06000218 RID: 536 RVA: 0x00006FB5 File Offset: 0x000051B5
	public virtual void Process_glUniform1uiv(uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform1uiv(this.swigCPtr, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000219 RID: 537 RVA: 0x00006FCA File Offset: 0x000051CA
	public virtual void Process_glUniform2uiv(uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform2uiv(this.swigCPtr, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600021A RID: 538 RVA: 0x00006FDF File Offset: 0x000051DF
	public virtual void Process_glUniform3uiv(uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform3uiv(this.swigCPtr, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600021B RID: 539 RVA: 0x00006FF4 File Offset: 0x000051F4
	public virtual void Process_glUniform4uiv(uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniform4uiv(this.swigCPtr, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600021C RID: 540 RVA: 0x00007009 File Offset: 0x00005209
	public virtual void Process_glClearBufferiv(uint buffer, int drawbuffer, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClearBufferiv(this.swigCPtr, buffer, drawbuffer, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000701E File Offset: 0x0000521E
	public virtual void Process_glClearBufferuiv(uint buffer, int drawbuffer, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClearBufferuiv(this.swigCPtr, buffer, drawbuffer, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600021E RID: 542 RVA: 0x00007033 File Offset: 0x00005233
	public virtual void Process_glClearBufferfv(uint buffer, int drawbuffer, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClearBufferfv(this.swigCPtr, buffer, drawbuffer, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00007048 File Offset: 0x00005248
	public virtual void Process_glClearBufferfi(uint buffer, int drawbuffer, float depth, int stencil)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClearBufferfi(this.swigCPtr, buffer, drawbuffer, depth, stencil);
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000705A File Offset: 0x0000525A
	public virtual void Process_glGetStringi(PointerData pReturnPtrData, uint name, uint index)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetStringi(this.swigCPtr, PointerData.getCPtr(pReturnPtrData), name, index);
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000706F File Offset: 0x0000526F
	public virtual void Process_glCopyBufferSubData(uint readTarget, uint writeTarget, int readOffset, int writeOffset, int size)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCopyBufferSubData(this.swigCPtr, readTarget, writeTarget, readOffset, writeOffset, size);
	}

	// Token: 0x06000222 RID: 546 RVA: 0x00007083 File Offset: 0x00005283
	public virtual void Process_glGetUniformIndices(uint program, int uniformCount, PointerData pUniformNamesPtrData, PointerData pUniformIndicesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetUniformIndices(this.swigCPtr, program, uniformCount, PointerData.getCPtr(pUniformNamesPtrData), PointerData.getCPtr(pUniformIndicesPtrData));
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000709F File Offset: 0x0000529F
	public virtual void Process_glGetActiveUniformsiv(uint program, int uniformCount, PointerData pUniformIndicesPtrData, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetActiveUniformsiv(this.swigCPtr, program, uniformCount, PointerData.getCPtr(pUniformIndicesPtrData), pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000224 RID: 548 RVA: 0x000070BD File Offset: 0x000052BD
	public virtual void Process_glGetUniformBlockIndex(uint returnVal, uint program, PointerData pUniformBlockNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetUniformBlockIndex(this.swigCPtr, returnVal, program, PointerData.getCPtr(pUniformBlockNamePtrData));
	}

	// Token: 0x06000225 RID: 549 RVA: 0x000070D2 File Offset: 0x000052D2
	public virtual void Process_glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetActiveUniformBlockiv(this.swigCPtr, program, uniformBlockIndex, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000226 RID: 550 RVA: 0x000070E9 File Offset: 0x000052E9
	public virtual void Process_glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, PointerData pLengthPtrData, PointerData pUniformBlockNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetActiveUniformBlockName(this.swigCPtr, program, uniformBlockIndex, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pUniformBlockNamePtrData));
	}

	// Token: 0x06000227 RID: 551 RVA: 0x00007107 File Offset: 0x00005307
	public virtual void Process_glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUniformBlockBinding(this.swigCPtr, program, uniformBlockIndex, uniformBlockBinding);
	}

	// Token: 0x06000228 RID: 552 RVA: 0x00007117 File Offset: 0x00005317
	public virtual void Process_glDrawArraysInstanced(uint mode, int first, int count, int instanceCount)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawArraysInstanced(this.swigCPtr, mode, first, count, instanceCount);
	}

	// Token: 0x06000229 RID: 553 RVA: 0x00007129 File Offset: 0x00005329
	public virtual void Process_glDrawElementsInstanced(uint mode, int count, uint type, PointerData pIndicesPtrData, int instanceCount)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawElementsInstanced(this.swigCPtr, mode, count, type, PointerData.getCPtr(pIndicesPtrData), instanceCount);
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00007142 File Offset: 0x00005342
	public virtual void Process_glFenceSync(uint returnVal, uint condition, uint flags)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFenceSync(this.swigCPtr, returnVal, condition, flags);
	}

	// Token: 0x0600022B RID: 555 RVA: 0x00007152 File Offset: 0x00005352
	public virtual void Process_glIsSync(int returnVal, uint sync)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsSync(this.swigCPtr, returnVal, sync);
	}

	// Token: 0x0600022C RID: 556 RVA: 0x00007161 File Offset: 0x00005361
	public virtual void Process_glDeleteSync(uint sync)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteSync(this.swigCPtr, sync);
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000716F File Offset: 0x0000536F
	public virtual void Process_glClientWaitSync(uint returnVal, uint sync, uint flags, ulong timeout)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClientWaitSync(this.swigCPtr, returnVal, sync, flags, timeout);
	}

	// Token: 0x0600022E RID: 558 RVA: 0x00007181 File Offset: 0x00005381
	public virtual void Process_glWaitSync(uint sync, uint flags, ulong timeout)
	{
		libDCAPPINVOKE.GLAdapter_Process_glWaitSync(this.swigCPtr, sync, flags, timeout);
	}

	// Token: 0x0600022F RID: 559 RVA: 0x00007191 File Offset: 0x00005391
	public virtual void Process_glGetInteger64v(uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetInteger64v(this.swigCPtr, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000230 RID: 560 RVA: 0x000071A5 File Offset: 0x000053A5
	public virtual void Process_glGetSynciv(uint sync, uint pname, int bufSize, PointerData pLengthPtrData, PointerData pValuesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetSynciv(this.swigCPtr, sync, pname, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pValuesPtrData));
	}

	// Token: 0x06000231 RID: 561 RVA: 0x000071C3 File Offset: 0x000053C3
	public virtual void Process_glGetInteger64i_v(uint target, uint index, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetInteger64i_v(this.swigCPtr, target, index, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x06000232 RID: 562 RVA: 0x000071D8 File Offset: 0x000053D8
	public virtual void Process_glGetBufferParameteri64v(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetBufferParameteri64v(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000233 RID: 563 RVA: 0x000071ED File Offset: 0x000053ED
	public virtual void Process_glGenSamplers(int count, PointerData pSamplersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenSamplers(this.swigCPtr, count, PointerData.getCPtr(pSamplersPtrData));
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00007201 File Offset: 0x00005401
	public virtual void Process_glDeleteSamplers(int count, PointerData pSamplersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteSamplers(this.swigCPtr, count, PointerData.getCPtr(pSamplersPtrData));
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00007215 File Offset: 0x00005415
	public virtual void Process_glIsSampler(int returnVal, uint sampler)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsSampler(this.swigCPtr, returnVal, sampler);
	}

	// Token: 0x06000236 RID: 566 RVA: 0x00007224 File Offset: 0x00005424
	public virtual void Process_glBindSampler(uint unit, uint sampler)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindSampler(this.swigCPtr, unit, sampler);
	}

	// Token: 0x06000237 RID: 567 RVA: 0x00007233 File Offset: 0x00005433
	public virtual void Process_glSamplerParameteri(uint sampler, uint pname, int param)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSamplerParameteri(this.swigCPtr, sampler, pname, param);
	}

	// Token: 0x06000238 RID: 568 RVA: 0x00007243 File Offset: 0x00005443
	public virtual void Process_glSamplerParameteriv(uint sampler, uint pname, PointerData pParamPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSamplerParameteriv(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamPtrData));
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00007258 File Offset: 0x00005458
	public virtual void Process_glSamplerParameterf(uint sampler, uint pname, float param)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSamplerParameterf(this.swigCPtr, sampler, pname, param);
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00007268 File Offset: 0x00005468
	public virtual void Process_glSamplerParameterfv(uint sampler, uint pname, PointerData pParamPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSamplerParameterfv(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamPtrData));
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000727D File Offset: 0x0000547D
	public virtual void Process_glGetSamplerParameteriv(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetSamplerParameteriv(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00007292 File Offset: 0x00005492
	public virtual void Process_glGetSamplerParameterfv(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetSamplerParameterfv(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600023D RID: 573 RVA: 0x000072A7 File Offset: 0x000054A7
	public virtual void Process_glVertexAttribDivisor(uint index, uint divisor)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribDivisor(this.swigCPtr, index, divisor);
	}

	// Token: 0x0600023E RID: 574 RVA: 0x000072B6 File Offset: 0x000054B6
	public virtual void Process_glBindTransformFeedback(uint target, uint id)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindTransformFeedback(this.swigCPtr, target, id);
	}

	// Token: 0x0600023F RID: 575 RVA: 0x000072C5 File Offset: 0x000054C5
	public virtual void Process_glDeleteTransformFeedbacks(int n, PointerData pIdsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteTransformFeedbacks(this.swigCPtr, n, PointerData.getCPtr(pIdsPtrData));
	}

	// Token: 0x06000240 RID: 576 RVA: 0x000072D9 File Offset: 0x000054D9
	public virtual void Process_glGenTransformFeedbacks(int n, PointerData pIdsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenTransformFeedbacks(this.swigCPtr, n, PointerData.getCPtr(pIdsPtrData));
	}

	// Token: 0x06000241 RID: 577 RVA: 0x000072ED File Offset: 0x000054ED
	public virtual void Process_glIsTransformFeedback(int returnVal, uint id)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsTransformFeedback(this.swigCPtr, returnVal, id);
	}

	// Token: 0x06000242 RID: 578 RVA: 0x000072FC File Offset: 0x000054FC
	public virtual void Process_glPauseTransformFeedback()
	{
		libDCAPPINVOKE.GLAdapter_Process_glPauseTransformFeedback(this.swigCPtr);
	}

	// Token: 0x06000243 RID: 579 RVA: 0x00007309 File Offset: 0x00005509
	public virtual void Process_glResumeTransformFeedback()
	{
		libDCAPPINVOKE.GLAdapter_Process_glResumeTransformFeedback(this.swigCPtr);
	}

	// Token: 0x06000244 RID: 580 RVA: 0x00007316 File Offset: 0x00005516
	public virtual void Process_glGetProgramBinary(uint program, int bufSize, PointerData pLengthPtrData, PointerData pBinaryFormatPtrData, PointerData pBinaryPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramBinary(this.swigCPtr, program, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pBinaryFormatPtrData), PointerData.getCPtr(pBinaryPtrData));
	}

	// Token: 0x06000245 RID: 581 RVA: 0x00007339 File Offset: 0x00005539
	public virtual void Process_glProgramBinary(uint program, uint binaryFormat, PointerData pBinaryPtrData, int length)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramBinary(this.swigCPtr, program, binaryFormat, PointerData.getCPtr(pBinaryPtrData), length);
	}

	// Token: 0x06000246 RID: 582 RVA: 0x00007350 File Offset: 0x00005550
	public virtual void Process_glProgramParameteri(uint program, uint pname, int value)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramParameteri(this.swigCPtr, program, pname, value);
	}

	// Token: 0x06000247 RID: 583 RVA: 0x00007360 File Offset: 0x00005560
	public virtual void Process_glInvalidateFramebuffer(uint target, int numAttachments, PointerData pAttachmentsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glInvalidateFramebuffer(this.swigCPtr, target, numAttachments, PointerData.getCPtr(pAttachmentsPtrData));
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00007375 File Offset: 0x00005575
	public virtual void Process_glInvalidateSubFramebuffer(uint target, int numAttachments, PointerData pAttachmentsPtrData, int x, int y, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glInvalidateSubFramebuffer(this.swigCPtr, target, numAttachments, PointerData.getCPtr(pAttachmentsPtrData), x, y, width, height);
	}

	// Token: 0x06000249 RID: 585 RVA: 0x00007392 File Offset: 0x00005592
	public virtual void Process_glTexStorage2D(uint target, int levels, uint internalformat, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexStorage2D(this.swigCPtr, target, levels, internalformat, width, height);
	}

	// Token: 0x0600024A RID: 586 RVA: 0x000073A6 File Offset: 0x000055A6
	public virtual void Process_glTexStorage3D(uint target, int levels, uint internalformat, int width, int height, int depth)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexStorage3D(this.swigCPtr, target, levels, internalformat, width, height, depth);
	}

	// Token: 0x0600024B RID: 587 RVA: 0x000073BC File Offset: 0x000055BC
	public virtual void Process_glGetInternalformativ(uint target, uint internalformat, uint pname, int bufSize, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetInternalformativ(this.swigCPtr, target, internalformat, pname, bufSize, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600024C RID: 588 RVA: 0x000073D5 File Offset: 0x000055D5
	public virtual void Process_glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDispatchCompute(this.swigCPtr, num_groups_x, num_groups_y, num_groups_z);
	}

	// Token: 0x0600024D RID: 589 RVA: 0x000073E5 File Offset: 0x000055E5
	public virtual void Process_glDispatchComputeIndirect(int indirect)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDispatchComputeIndirect(this.swigCPtr, indirect);
	}

	// Token: 0x0600024E RID: 590 RVA: 0x000073F3 File Offset: 0x000055F3
	public virtual void Process_glDrawArraysIndirect(uint mode, PointerData pIndirectPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawArraysIndirect(this.swigCPtr, mode, PointerData.getCPtr(pIndirectPtrData));
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00007407 File Offset: 0x00005607
	public virtual void Process_glDrawElementsIndirect(uint mode, uint type, PointerData pIndirectPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawElementsIndirect(this.swigCPtr, mode, type, PointerData.getCPtr(pIndirectPtrData));
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000741C File Offset: 0x0000561C
	public virtual void Process_glFramebufferParameteri(uint target, uint pname, int param)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferParameteri(this.swigCPtr, target, pname, param);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000742C File Offset: 0x0000562C
	public virtual void Process_glGetFramebufferParameteriv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetFramebufferParameteriv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00007441 File Offset: 0x00005641
	public virtual void Process_glGetProgramInterfaceiv(uint program, uint programInterface, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramInterfaceiv(this.swigCPtr, program, programInterface, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00007458 File Offset: 0x00005658
	public virtual void Process_glGetProgramResourceIndex(uint returnVal, uint program, uint programInterface, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramResourceIndex(this.swigCPtr, returnVal, program, programInterface, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000746F File Offset: 0x0000566F
	public virtual void Process_glGetProgramResourceName(uint program, uint programInterface, uint index, int bufSize, PointerData pLengthPtrData, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramResourceName(this.swigCPtr, program, programInterface, index, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00007490 File Offset: 0x00005690
	public virtual void Process_glGetProgramResourceiv(uint program, uint programInterface, uint index, int propCount, PointerData pPropsPtrData, int bufSize, PointerData pLengthPtrData, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramResourceiv(this.swigCPtr, program, programInterface, index, propCount, PointerData.getCPtr(pPropsPtrData), bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000256 RID: 598 RVA: 0x000074C4 File Offset: 0x000056C4
	public virtual void Process_glGetProgramResourceLocation(uint returnVal, uint program, uint programInterface, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramResourceLocation(this.swigCPtr, returnVal, program, programInterface, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x06000257 RID: 599 RVA: 0x000074DB File Offset: 0x000056DB
	public virtual void Process_glUseProgramStages(uint pipeline, uint stages, uint program)
	{
		libDCAPPINVOKE.GLAdapter_Process_glUseProgramStages(this.swigCPtr, pipeline, stages, program);
	}

	// Token: 0x06000258 RID: 600 RVA: 0x000074EB File Offset: 0x000056EB
	public virtual void Process_glActiveShaderProgram(uint pipeline, uint program)
	{
		libDCAPPINVOKE.GLAdapter_Process_glActiveShaderProgram(this.swigCPtr, pipeline, program);
	}

	// Token: 0x06000259 RID: 601 RVA: 0x000074FA File Offset: 0x000056FA
	public virtual void Process_glCreateShaderProgramv(uint returnVal, uint type, int count, PointerData pAPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCreateShaderProgramv(this.swigCPtr, returnVal, type, count, PointerData.getCPtr(pAPtrData));
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00007511 File Offset: 0x00005711
	public virtual void Process_glBindProgramPipeline(uint pipeline)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindProgramPipeline(this.swigCPtr, pipeline);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000751F File Offset: 0x0000571F
	public virtual void Process_glDeleteProgramPipelines(int n, PointerData pPipelinesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteProgramPipelines(this.swigCPtr, n, PointerData.getCPtr(pPipelinesPtrData));
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00007533 File Offset: 0x00005733
	public virtual void Process_glGenProgramPipelines(int n, PointerData pPipelinesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenProgramPipelines(this.swigCPtr, n, PointerData.getCPtr(pPipelinesPtrData));
	}

	// Token: 0x0600025D RID: 605 RVA: 0x00007547 File Offset: 0x00005747
	public virtual void Process_glIsProgramPipeline(int returnVal, uint pipeline)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsProgramPipeline(this.swigCPtr, returnVal, pipeline);
	}

	// Token: 0x0600025E RID: 606 RVA: 0x00007556 File Offset: 0x00005756
	public virtual void Process_glGetProgramPipelineiv(uint pipeline, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramPipelineiv(this.swigCPtr, pipeline, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000756B File Offset: 0x0000576B
	public virtual void Process_glProgramUniform1i(uint program, uint location, int v0)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform1i(this.swigCPtr, program, location, v0);
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0000757B File Offset: 0x0000577B
	public virtual void Process_glProgramUniform2i(uint program, uint location, int v0, int v1)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform2i(this.swigCPtr, program, location, v0, v1);
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000758D File Offset: 0x0000578D
	public virtual void Process_glProgramUniform3i(uint program, uint location, int v0, int v1, int v2)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform3i(this.swigCPtr, program, location, v0, v1, v2);
	}

	// Token: 0x06000262 RID: 610 RVA: 0x000075A1 File Offset: 0x000057A1
	public virtual void Process_glProgramUniform4i(uint program, uint location, int v0, int v1, int v2, int v3)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform4i(this.swigCPtr, program, location, v0, v1, v2, v3);
	}

	// Token: 0x06000263 RID: 611 RVA: 0x000075B7 File Offset: 0x000057B7
	public virtual void Process_glProgramUniform1ui(uint program, uint location, uint v0)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform1ui(this.swigCPtr, program, location, v0);
	}

	// Token: 0x06000264 RID: 612 RVA: 0x000075C7 File Offset: 0x000057C7
	public virtual void Process_glProgramUniform2ui(uint program, uint location, uint v0, uint v1)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform2ui(this.swigCPtr, program, location, v0, v1);
	}

	// Token: 0x06000265 RID: 613 RVA: 0x000075D9 File Offset: 0x000057D9
	public virtual void Process_glProgramUniform3ui(uint program, uint location, uint v0, uint v1, uint v2)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform3ui(this.swigCPtr, program, location, v0, v1, v2);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x000075ED File Offset: 0x000057ED
	public virtual void Process_glProgramUniform4ui(uint program, uint location, uint v0, uint v1, uint v2, uint v3)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform4ui(this.swigCPtr, program, location, v0, v1, v2, v3);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00007603 File Offset: 0x00005803
	public virtual void Process_glProgramUniform1f(uint program, uint location, float v0)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform1f(this.swigCPtr, program, location, v0);
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00007613 File Offset: 0x00005813
	public virtual void Process_glProgramUniform2f(uint program, uint location, float v0, float v1)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform2f(this.swigCPtr, program, location, v0, v1);
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00007625 File Offset: 0x00005825
	public virtual void Process_glProgramUniform3f(uint program, uint location, float v0, float v1, float v2)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform3f(this.swigCPtr, program, location, v0, v1, v2);
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00007639 File Offset: 0x00005839
	public virtual void Process_glProgramUniform4f(uint program, uint location, float v0, float v1, float v2, float v3)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform4f(this.swigCPtr, program, location, v0, v1, v2, v3);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000764F File Offset: 0x0000584F
	public virtual void Process_glProgramUniform1iv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform1iv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00007666 File Offset: 0x00005866
	public virtual void Process_glProgramUniform2iv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform2iv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000767D File Offset: 0x0000587D
	public virtual void Process_glProgramUniform3iv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform3iv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00007694 File Offset: 0x00005894
	public virtual void Process_glProgramUniform4iv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform4iv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600026F RID: 623 RVA: 0x000076AB File Offset: 0x000058AB
	public virtual void Process_glProgramUniform1uiv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform1uiv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000270 RID: 624 RVA: 0x000076C2 File Offset: 0x000058C2
	public virtual void Process_glProgramUniform2uiv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform2uiv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000271 RID: 625 RVA: 0x000076D9 File Offset: 0x000058D9
	public virtual void Process_glProgramUniform3uiv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform3uiv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000272 RID: 626 RVA: 0x000076F0 File Offset: 0x000058F0
	public virtual void Process_glProgramUniform4uiv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform4uiv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00007707 File Offset: 0x00005907
	public virtual void Process_glProgramUniform1fv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform1fv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000771E File Offset: 0x0000591E
	public virtual void Process_glProgramUniform2fv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform2fv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00007735 File Offset: 0x00005935
	public virtual void Process_glProgramUniform3fv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform3fv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000774C File Offset: 0x0000594C
	public virtual void Process_glProgramUniform4fv(uint program, uint location, int count, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniform4fv(this.swigCPtr, program, location, count, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00007763 File Offset: 0x00005963
	public virtual void Process_glProgramUniformMatrix2fv(uint program, uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniformMatrix2fv(this.swigCPtr, program, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000777C File Offset: 0x0000597C
	public virtual void Process_glProgramUniformMatrix3fv(uint program, uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniformMatrix3fv(this.swigCPtr, program, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00007795 File Offset: 0x00005995
	public virtual void Process_glProgramUniformMatrix4fv(uint program, uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniformMatrix4fv(this.swigCPtr, program, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600027A RID: 634 RVA: 0x000077AE File Offset: 0x000059AE
	public virtual void Process_glProgramUniformMatrix2x3fv(uint program, uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniformMatrix2x3fv(this.swigCPtr, program, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600027B RID: 635 RVA: 0x000077C7 File Offset: 0x000059C7
	public virtual void Process_glProgramUniformMatrix3x2fv(uint program, uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniformMatrix3x2fv(this.swigCPtr, program, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600027C RID: 636 RVA: 0x000077E0 File Offset: 0x000059E0
	public virtual void Process_glProgramUniformMatrix2x4fv(uint program, uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniformMatrix2x4fv(this.swigCPtr, program, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600027D RID: 637 RVA: 0x000077F9 File Offset: 0x000059F9
	public virtual void Process_glProgramUniformMatrix4x2fv(uint program, uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniformMatrix4x2fv(this.swigCPtr, program, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00007812 File Offset: 0x00005A12
	public virtual void Process_glProgramUniformMatrix3x4fv(uint program, uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniformMatrix3x4fv(this.swigCPtr, program, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0000782B File Offset: 0x00005A2B
	public virtual void Process_glProgramUniformMatrix4x3fv(uint program, uint location, int count, int transpose, PointerData pValuePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramUniformMatrix4x3fv(this.swigCPtr, program, location, count, transpose, PointerData.getCPtr(pValuePtrData));
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00007844 File Offset: 0x00005A44
	public virtual void Process_glValidateProgramPipeline(uint pipeline)
	{
		libDCAPPINVOKE.GLAdapter_Process_glValidateProgramPipeline(this.swigCPtr, pipeline);
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00007852 File Offset: 0x00005A52
	public virtual void Process_glGetProgramPipelineInfoLog(uint pipeline, int bufSize, PointerData pLengthPtrData, PointerData pInfoLogPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramPipelineInfoLog(this.swigCPtr, pipeline, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pInfoLogPtrData));
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000786E File Offset: 0x00005A6E
	public virtual void Process_glGetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetActiveAtomicCounterBufferiv(this.swigCPtr, program, bufferIndex, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00007885 File Offset: 0x00005A85
	public virtual void Process_glBindImageTexture(uint unit, uint texture, int level, int layered, int layer, uint access, uint format)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindImageTexture(this.swigCPtr, unit, texture, level, layered, layer, access, format);
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0000789D File Offset: 0x00005A9D
	public virtual void Process_glMemoryBarrier(uint barriers)
	{
		libDCAPPINVOKE.GLAdapter_Process_glMemoryBarrier(this.swigCPtr, barriers);
	}

	// Token: 0x06000285 RID: 645 RVA: 0x000078AB File Offset: 0x00005AAB
	public virtual void Process_glMemoryBarrierByRegion(uint barriers)
	{
		libDCAPPINVOKE.GLAdapter_Process_glMemoryBarrierByRegion(this.swigCPtr, barriers);
	}

	// Token: 0x06000286 RID: 646 RVA: 0x000078B9 File Offset: 0x00005AB9
	public virtual void Process_glTexStorage2DMultisample(uint target, int samples, uint internalformat, int width, int height, int fixedsamplelocations)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexStorage2DMultisample(this.swigCPtr, target, samples, internalformat, width, height, fixedsamplelocations);
	}

	// Token: 0x06000287 RID: 647 RVA: 0x000078CF File Offset: 0x00005ACF
	public virtual void Process_glTexStorage3DMultisampleOES(uint target, int samples, uint internalformat, int width, int height, int depth, int fixedsamplelocations)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexStorage3DMultisampleOES(this.swigCPtr, target, samples, internalformat, width, height, depth, fixedsamplelocations);
	}

	// Token: 0x06000288 RID: 648 RVA: 0x000078E7 File Offset: 0x00005AE7
	public virtual void Process_glGetMultisamplefv(uint pname, uint index, PointerData pValPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetMultisamplefv(this.swigCPtr, pname, index, PointerData.getCPtr(pValPtrData));
	}

	// Token: 0x06000289 RID: 649 RVA: 0x000078FC File Offset: 0x00005AFC
	public virtual void Process_glSampleMaski(uint index, uint mask)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSampleMaski(this.swigCPtr, index, mask);
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000790B File Offset: 0x00005B0B
	public virtual void Process_glGetTexLevelParameteriv(uint target, int level, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetTexLevelParameteriv(this.swigCPtr, target, level, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00007922 File Offset: 0x00005B22
	public virtual void Process_glGetTexLevelParameterfv(uint target, int level, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetTexLevelParameterfv(this.swigCPtr, target, level, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00007939 File Offset: 0x00005B39
	public virtual void Process_glBindVertexBuffer(uint bindingindex, uint buffer, int offset, int stride)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindVertexBuffer(this.swigCPtr, bindingindex, buffer, offset, stride);
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0000794B File Offset: 0x00005B4B
	public virtual void Process_glVertexAttribFormat(uint attribindex, int size, uint type, int normalized, uint relativeoffset)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribFormat(this.swigCPtr, attribindex, size, type, normalized, relativeoffset);
	}

	// Token: 0x0600028E RID: 654 RVA: 0x0000795F File Offset: 0x00005B5F
	public virtual void Process_glVertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribIFormat(this.swigCPtr, attribindex, size, type, relativeoffset);
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00007971 File Offset: 0x00005B71
	public virtual void Process_glVertexAttribBinding(uint attribindex, uint bindingindex)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexAttribBinding(this.swigCPtr, attribindex, bindingindex);
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00007980 File Offset: 0x00005B80
	public virtual void Process_glVertexBindingDivisor(uint bindingindex, uint divisor)
	{
		libDCAPPINVOKE.GLAdapter_Process_glVertexBindingDivisor(this.swigCPtr, bindingindex, divisor);
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000798F File Offset: 0x00005B8F
	public virtual void Process_glPatchParameteri(uint pname, int value)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPatchParameteri(this.swigCPtr, pname, value);
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000799E File Offset: 0x00005B9E
	public virtual void Process_glGetFixedvAMD(uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetFixedvAMD(this.swigCPtr, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000293 RID: 659 RVA: 0x000079B2 File Offset: 0x00005BB2
	public virtual void Process_glLogicOpAMD(uint op)
	{
		libDCAPPINVOKE.GLAdapter_Process_glLogicOpAMD(this.swigCPtr, op);
	}

	// Token: 0x06000294 RID: 660 RVA: 0x000079C0 File Offset: 0x00005BC0
	public virtual void Process_glFogfvAMD(uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFogfvAMD(this.swigCPtr, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000295 RID: 661 RVA: 0x000079D4 File Offset: 0x00005BD4
	public virtual void Process_glGetMemoryStatsQCOM(uint pname, uint usage, PointerData pParamPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetMemoryStatsQCOM(this.swigCPtr, pname, usage, PointerData.getCPtr(pParamPtrData));
	}

	// Token: 0x06000296 RID: 662 RVA: 0x000079E9 File Offset: 0x00005BE9
	public virtual void Process_glGetSizedMemoryStatsQCOM(int maxcount, PointerData pCountPtrData, PointerData pBufPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetSizedMemoryStatsQCOM(this.swigCPtr, maxcount, PointerData.getCPtr(pCountPtrData), PointerData.getCPtr(pBufPtrData));
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00007A03 File Offset: 0x00005C03
	public virtual void Process_glBlitOverlapQCOM(int dest_x, int dest_y, int src_x, int src_y, int src_width, int src_height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlitOverlapQCOM(this.swigCPtr, dest_x, dest_y, src_x, src_y, src_width, src_height);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00007A19 File Offset: 0x00005C19
	public virtual void Process_glGetShaderStatsQCOM(uint shader, int maxLength, PointerData pLengthPtrData, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetShaderStatsQCOM(this.swigCPtr, shader, maxLength, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00007A35 File Offset: 0x00005C35
	public virtual void Process_glExtGetSamplersQCOM(PointerData pSamplersPtrData, int maxSamplers, PointerData pNumSamplersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glExtGetSamplersQCOM(this.swigCPtr, PointerData.getCPtr(pSamplersPtrData), maxSamplers, PointerData.getCPtr(pNumSamplersPtrData));
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00007A4F File Offset: 0x00005C4F
	public virtual void Process_glClipPlanefQCOM(uint p, PointerData pEquationPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glClipPlanefQCOM(this.swigCPtr, p, PointerData.getCPtr(pEquationPtrData));
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00007A63 File Offset: 0x00005C63
	public virtual void Process_glFramebufferTexture2DExternalQCOM(uint target, uint attachment, uint textarget, uint texture, int level)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferTexture2DExternalQCOM(this.swigCPtr, target, attachment, textarget, texture, level);
	}

	// Token: 0x0600029C RID: 668 RVA: 0x00007A77 File Offset: 0x00005C77
	public virtual void Process_glFramebufferRenderbufferExternalQCOM(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferRenderbufferExternalQCOM(this.swigCPtr, target, attachment, renderbuffertarget, renderbuffer);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00007A89 File Offset: 0x00005C89
	public virtual void Process_glEGLImageTargetTexture2DOES(uint target, uint image)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEGLImageTargetTexture2DOES(this.swigCPtr, target, image);
	}

	// Token: 0x0600029E RID: 670 RVA: 0x00007A98 File Offset: 0x00005C98
	public virtual void Process_glEGLImageTargetRenderbufferStorageOES(uint target, uint image)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEGLImageTargetRenderbufferStorageOES(this.swigCPtr, target, image);
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00007AA7 File Offset: 0x00005CA7
	public virtual void Process_glGetProgramBinaryOES(uint program, int bufSize, PointerData pLengthPtrData, PointerData pBinaryFormatPtrData, PointerData pBinaryPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramBinaryOES(this.swigCPtr, program, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pBinaryFormatPtrData), PointerData.getCPtr(pBinaryPtrData));
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00007ACA File Offset: 0x00005CCA
	public virtual void Process_glProgramBinaryOES(uint program, uint binaryFormat, PointerData pBinaryPtrData, int length)
	{
		libDCAPPINVOKE.GLAdapter_Process_glProgramBinaryOES(this.swigCPtr, program, binaryFormat, PointerData.getCPtr(pBinaryPtrData), length);
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00007AE4 File Offset: 0x00005CE4
	public virtual void Process_glTexImage3DOES(uint target, int level, uint internalformat, int width, int height, int depth, int border, uint format, uint type, PointerData pPixelsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexImage3DOES(this.swigCPtr, target, level, internalformat, width, height, depth, border, format, type, PointerData.getCPtr(pPixelsPtrData));
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00007B14 File Offset: 0x00005D14
	public virtual void Process_glTexSubImage3DOES(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, PointerData pPixelsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexSubImage3DOES(this.swigCPtr, target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, PointerData.getCPtr(pPixelsPtrData));
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00007B44 File Offset: 0x00005D44
	public virtual void Process_glCopyTexSubImage3DOES(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCopyTexSubImage3DOES(this.swigCPtr, target, level, xoffset, yoffset, zoffset, x, y, width, height);
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00007B6C File Offset: 0x00005D6C
	public virtual void Process_glCompressedTexImage3DOES(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCompressedTexImage3DOES(this.swigCPtr, target, level, internalformat, width, height, depth, border, imageSize, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00007B98 File Offset: 0x00005D98
	public virtual void Process_glCompressedTexSubImage3DOES(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCompressedTexSubImage3DOES(this.swigCPtr, target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00007BC8 File Offset: 0x00005DC8
	public virtual void Process_glFramebufferTexture3DOES(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferTexture3DOES(this.swigCPtr, target, attachment, textarget, texture, level, zoffset);
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00007BDE File Offset: 0x00005DDE
	public virtual void Process_glBindVertexArrayOES(uint array)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindVertexArrayOES(this.swigCPtr, array);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00007BEC File Offset: 0x00005DEC
	public virtual void Process_glDeleteVertexArraysOES(int n, PointerData pArraysPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteVertexArraysOES(this.swigCPtr, n, PointerData.getCPtr(pArraysPtrData));
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00007C00 File Offset: 0x00005E00
	public virtual void Process_glGenVertexArraysOES(int n, PointerData pArraysPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenVertexArraysOES(this.swigCPtr, n, PointerData.getCPtr(pArraysPtrData));
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00007C14 File Offset: 0x00005E14
	public virtual void Process_glIsVertexArrayOES(int returnVal, uint array)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsVertexArrayOES(this.swigCPtr, returnVal, array);
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00007C23 File Offset: 0x00005E23
	public virtual void Process_glGetPerfMonitorGroupsAMD(PointerData pNumGroupsPtrData, int groupsSize, PointerData pGroupsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetPerfMonitorGroupsAMD(this.swigCPtr, PointerData.getCPtr(pNumGroupsPtrData), groupsSize, PointerData.getCPtr(pGroupsPtrData));
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00007C3D File Offset: 0x00005E3D
	public virtual void Process_glGetPerfMonitorCountersAMD(uint group, PointerData pNumCountersPtrData, PointerData pMaxActiveCountersPtrData, int counterSize, PointerData pCountersPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetPerfMonitorCountersAMD(this.swigCPtr, group, PointerData.getCPtr(pNumCountersPtrData), PointerData.getCPtr(pMaxActiveCountersPtrData), counterSize, PointerData.getCPtr(pCountersPtrData));
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00007C60 File Offset: 0x00005E60
	public virtual void Process_glGetPerfMonitorGroupStringAMD(uint group, int bufSize, PointerData pLengthPtrData, PointerData pGroupStringPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetPerfMonitorGroupStringAMD(this.swigCPtr, group, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pGroupStringPtrData));
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00007C7C File Offset: 0x00005E7C
	public virtual void Process_glGetPerfMonitorCounterStringAMD(uint group, uint counter, int bufSize, PointerData pLengthPtrData, PointerData pCounterStringPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetPerfMonitorCounterStringAMD(this.swigCPtr, group, counter, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pCounterStringPtrData));
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00007C9A File Offset: 0x00005E9A
	public virtual void Process_glGetPerfMonitorCounterInfoAMD(uint group, uint counter, uint pname, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetPerfMonitorCounterInfoAMD(this.swigCPtr, group, counter, pname, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00007CB1 File Offset: 0x00005EB1
	public virtual void Process_glGenPerfMonitorsAMD(int n, PointerData pMonitorsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenPerfMonitorsAMD(this.swigCPtr, n, PointerData.getCPtr(pMonitorsPtrData));
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00007CC5 File Offset: 0x00005EC5
	public virtual void Process_glDeletePerfMonitorsAMD(int n, PointerData pMonitorsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeletePerfMonitorsAMD(this.swigCPtr, n, PointerData.getCPtr(pMonitorsPtrData));
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00007CD9 File Offset: 0x00005ED9
	public virtual void Process_glSelectPerfMonitorCountersAMD(uint monitor, int enable, uint group, int numCounters, PointerData pCountersListPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSelectPerfMonitorCountersAMD(this.swigCPtr, monitor, enable, group, numCounters, PointerData.getCPtr(pCountersListPtrData));
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00007CF2 File Offset: 0x00005EF2
	public virtual void Process_glBeginPerfMonitorAMD(uint monitor)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBeginPerfMonitorAMD(this.swigCPtr, monitor);
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00007D00 File Offset: 0x00005F00
	public virtual void Process_glEndPerfMonitorAMD(uint monitor)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEndPerfMonitorAMD(this.swigCPtr, monitor);
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00007D0E File Offset: 0x00005F0E
	public virtual void Process_glGetPerfMonitorCounterDataAMD(uint monitor, uint pname, int dataSize, PointerData pDataPtrData, PointerData pBytesWrittenPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetPerfMonitorCounterDataAMD(this.swigCPtr, monitor, pname, dataSize, PointerData.getCPtr(pDataPtrData), PointerData.getCPtr(pBytesWrittenPtrData));
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00007D2C File Offset: 0x00005F2C
	public virtual void Process_glLabelObjectEXT(uint type, uint arg1, int length, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glLabelObjectEXT(this.swigCPtr, type, arg1, length, PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00007D43 File Offset: 0x00005F43
	public virtual void Process_glGetObjectLabelEXT(uint type, uint arg1, int bufSize, PointerData pLengthPtrData, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetObjectLabelEXT(this.swigCPtr, type, arg1, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00007D61 File Offset: 0x00005F61
	public virtual void Process_glInsertEventMarkerEXT(int length, PointerData pMarkerPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glInsertEventMarkerEXT(this.swigCPtr, length, PointerData.getCPtr(pMarkerPtrData));
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00007D75 File Offset: 0x00005F75
	public virtual void Process_glPushGroupMarkerEXT(int length, PointerData pMarkerPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPushGroupMarkerEXT(this.swigCPtr, length, PointerData.getCPtr(pMarkerPtrData));
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00007D89 File Offset: 0x00005F89
	public virtual void Process_glPopGroupMarkerEXT()
	{
		libDCAPPINVOKE.GLAdapter_Process_glPopGroupMarkerEXT(this.swigCPtr);
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00007D96 File Offset: 0x00005F96
	public virtual void Process_glDiscardFramebufferEXT(uint target, int numAttachments, PointerData pAttachmentsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDiscardFramebufferEXT(this.swigCPtr, target, numAttachments, PointerData.getCPtr(pAttachmentsPtrData));
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00007DAB File Offset: 0x00005FAB
	public virtual void Process_glGenQueriesEXT(int n, PointerData pIdsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenQueriesEXT(this.swigCPtr, n, PointerData.getCPtr(pIdsPtrData));
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00007DBF File Offset: 0x00005FBF
	public virtual void Process_glDeleteQueriesEXT(int n, PointerData pIdsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteQueriesEXT(this.swigCPtr, n, PointerData.getCPtr(pIdsPtrData));
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00007DD3 File Offset: 0x00005FD3
	public virtual void Process_glIsQueryEXT(int returnVal, uint id)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsQueryEXT(this.swigCPtr, returnVal, id);
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00007DE2 File Offset: 0x00005FE2
	public virtual void Process_glBeginQueryEXT(uint target, uint id)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBeginQueryEXT(this.swigCPtr, target, id);
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00007DF1 File Offset: 0x00005FF1
	public virtual void Process_glEndQueryEXT(uint target)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEndQueryEXT(this.swigCPtr, target);
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00007DFF File Offset: 0x00005FFF
	public virtual void Process_glQueryCounterEXT(uint id, uint target)
	{
		libDCAPPINVOKE.GLAdapter_Process_glQueryCounterEXT(this.swigCPtr, id, target);
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00007E0E File Offset: 0x0000600E
	public virtual void Process_glGetQueryivEXT(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetQueryivEXT(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00007E23 File Offset: 0x00006023
	public virtual void Process_glGetQueryObjectivEXT(uint id, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetQueryObjectivEXT(this.swigCPtr, id, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00007E38 File Offset: 0x00006038
	public virtual void Process_glGetQueryObjectuivEXT(uint id, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetQueryObjectuivEXT(this.swigCPtr, id, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00007E4D File Offset: 0x0000604D
	public virtual void Process_glGetQueryObjecti64vEXT(uint id, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetQueryObjecti64vEXT(this.swigCPtr, id, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00007E62 File Offset: 0x00006062
	public virtual void Process_glGetQueryObjectui64vEXT(uint id, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetQueryObjectui64vEXT(this.swigCPtr, id, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00007E77 File Offset: 0x00006077
	public virtual void Process_glGetGraphicsResetStatusEXT(uint returnVal)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetGraphicsResetStatusEXT(this.swigCPtr, returnVal);
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00007E88 File Offset: 0x00006088
	public virtual void Process_glReadnPixelsEXT(int x, int y, int width, int height, uint format, uint type, int bufSize, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glReadnPixelsEXT(this.swigCPtr, x, y, width, height, format, type, bufSize, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00007EB2 File Offset: 0x000060B2
	public virtual void Process_glGetnUniformfvEXT(uint program, uint location, int bufSize, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetnUniformfvEXT(this.swigCPtr, program, location, bufSize, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00007EC9 File Offset: 0x000060C9
	public virtual void Process_glGetnUniformivEXT(uint program, uint location, int bufSize, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetnUniformivEXT(this.swigCPtr, program, location, bufSize, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00007EE0 File Offset: 0x000060E0
	public virtual void Process_glTexParameterIivEXT(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexParameterIivEXT(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00007EF5 File Offset: 0x000060F5
	public virtual void Process_glTexParameterIuivEXT(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexParameterIuivEXT(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00007F0A File Offset: 0x0000610A
	public virtual void Process_glGetTexParameterIivEXT(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetTexParameterIivEXT(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00007F1F File Offset: 0x0000611F
	public virtual void Process_glGetTexParameterIuivEXT(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetTexParameterIuivEXT(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002CF RID: 719 RVA: 0x00007F34 File Offset: 0x00006134
	public virtual void Process_glSamplerParameterIivEXT(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSamplerParameterIivEXT(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00007F49 File Offset: 0x00006149
	public virtual void Process_glSamplerParameterIuivEXT(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSamplerParameterIuivEXT(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00007F5E File Offset: 0x0000615E
	public virtual void Process_glGetSamplerParameterIivEXT(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetSamplerParameterIivEXT(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00007F73 File Offset: 0x00006173
	public virtual void Process_glGetSamplerParameterIuivEXT(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetSamplerParameterIuivEXT(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00007F88 File Offset: 0x00006188
	public virtual void Process_glRenderbufferStorageMultisampleEXT(uint target, int samples, uint internalformat, int width, int height)
	{
		libDCAPPINVOKE.GLAdapter_Process_glRenderbufferStorageMultisampleEXT(this.swigCPtr, target, samples, internalformat, width, height);
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00007F9C File Offset: 0x0000619C
	public virtual void Process_glFramebufferTexture2DMultisampleEXT(uint target, uint attachment, uint textarget, uint texture, int level, int samples)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferTexture2DMultisampleEXT(this.swigCPtr, target, attachment, textarget, texture, level, samples);
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00007FB2 File Offset: 0x000061B2
	public virtual void Process_glAlphaFuncQCOM(uint func, float arg1)
	{
		libDCAPPINVOKE.GLAdapter_Process_glAlphaFuncQCOM(this.swigCPtr, func, arg1);
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00007FC1 File Offset: 0x000061C1
	public virtual void Process_glStartTilingQCOM(uint x, uint y, uint width, uint height, uint preserveMask)
	{
		libDCAPPINVOKE.GLAdapter_Process_glStartTilingQCOM(this.swigCPtr, x, y, width, height, preserveMask);
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00007FD5 File Offset: 0x000061D5
	public virtual void Process_glEndTilingQCOM(uint preserveMask)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEndTilingQCOM(this.swigCPtr, preserveMask);
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00007FE4 File Offset: 0x000061E4
	public virtual void Process_glCopyImageSubDataEXT(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCopyImageSubDataEXT(this.swigCPtr, srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00008017 File Offset: 0x00006217
	public virtual void Process_glBlendBarrierKHR()
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendBarrierKHR(this.swigCPtr);
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00008024 File Offset: 0x00006224
	public virtual void Process_glMinSampleShadingOES(float value)
	{
		libDCAPPINVOKE.GLAdapter_Process_glMinSampleShadingOES(this.swigCPtr, value);
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00008032 File Offset: 0x00006232
	public virtual void Process_glEnableiEXT(uint target, uint index)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEnableiEXT(this.swigCPtr, target, index);
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00008041 File Offset: 0x00006241
	public virtual void Process_glDisableiEXT(uint target, uint index)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDisableiEXT(this.swigCPtr, target, index);
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00008050 File Offset: 0x00006250
	public virtual void Process_glBlendEquationiEXT(uint buf, uint mode)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendEquationiEXT(this.swigCPtr, buf, mode);
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0000805F File Offset: 0x0000625F
	public virtual void Process_glBlendEquationSeparateiEXT(uint buf, uint modeRGB, uint modeAlpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendEquationSeparateiEXT(this.swigCPtr, buf, modeRGB, modeAlpha);
	}

	// Token: 0x060002DF RID: 735 RVA: 0x0000806F File Offset: 0x0000626F
	public virtual void Process_glBlendFunciEXT(uint buf, uint src, uint dst)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendFunciEXT(this.swigCPtr, buf, src, dst);
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0000807F File Offset: 0x0000627F
	public virtual void Process_glBlendFuncSeparateiEXT(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendFuncSeparateiEXT(this.swigCPtr, buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00008093 File Offset: 0x00006293
	public virtual void Process_glColorMaskiEXT(uint buf, int r, int g, int b, int a)
	{
		libDCAPPINVOKE.GLAdapter_Process_glColorMaskiEXT(this.swigCPtr, buf, r, g, b, a);
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000080A7 File Offset: 0x000062A7
	public virtual void Process_glIsEnablediEXT(int returnVal, uint target, uint index)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsEnablediEXT(this.swigCPtr, returnVal, target, index);
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000080B7 File Offset: 0x000062B7
	public virtual void Process_glTexBufferEXT(uint target, uint internalFormat, uint buffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexBufferEXT(this.swigCPtr, target, internalFormat, buffer);
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x000080C7 File Offset: 0x000062C7
	public virtual void Process_glTexBufferRangeEXT(uint target, uint internalFormat, uint buffer, int offset, int size)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexBufferRangeEXT(this.swigCPtr, target, internalFormat, buffer, offset, size);
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x000080DB File Offset: 0x000062DB
	public virtual void Process_glDebugMessageControlKHR(uint source, uint type, uint severity, int count, PointerData pIdsPtrData, int enabled)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDebugMessageControlKHR(this.swigCPtr, source, type, severity, count, PointerData.getCPtr(pIdsPtrData), enabled);
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x000080F6 File Offset: 0x000062F6
	public virtual void Process_glDebugMessageInsertKHR(uint source, uint type, uint id, uint severity, int length, PointerData pBufPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDebugMessageInsertKHR(this.swigCPtr, source, type, id, severity, length, PointerData.getCPtr(pBufPtrData));
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00008111 File Offset: 0x00006311
	public virtual void Process_glDebugMessageCallbackKHR(PointerData pCallbackPtrData, PointerData pUserParamPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDebugMessageCallbackKHR(this.swigCPtr, PointerData.getCPtr(pCallbackPtrData), PointerData.getCPtr(pUserParamPtrData));
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0000812C File Offset: 0x0000632C
	public virtual void Process_glGetDebugMessageLogKHR(uint returnVal, uint count, int bufSize, PointerData pSourcesPtrData, PointerData pTypesPtrData, PointerData pIdsPtrData, PointerData pSeveritiesPtrData, PointerData pLengthsPtrData, PointerData pMessageLogPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetDebugMessageLogKHR(this.swigCPtr, returnVal, count, bufSize, PointerData.getCPtr(pSourcesPtrData), PointerData.getCPtr(pTypesPtrData), PointerData.getCPtr(pIdsPtrData), PointerData.getCPtr(pSeveritiesPtrData), PointerData.getCPtr(pLengthsPtrData), PointerData.getCPtr(pMessageLogPtrData));
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00008171 File Offset: 0x00006371
	public virtual void Process_glPushDebugGroupKHR(uint source, uint id, int length, PointerData pMessagePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPushDebugGroupKHR(this.swigCPtr, source, id, length, PointerData.getCPtr(pMessagePtrData));
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00008188 File Offset: 0x00006388
	public virtual void Process_glPopDebugGroupKHR()
	{
		libDCAPPINVOKE.GLAdapter_Process_glPopDebugGroupKHR(this.swigCPtr);
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00008195 File Offset: 0x00006395
	public virtual void Process_glObjectLabelKHR(uint identifier, uint name, int length, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glObjectLabelKHR(this.swigCPtr, identifier, name, length, PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x060002EC RID: 748 RVA: 0x000081AC File Offset: 0x000063AC
	public virtual void Process_glGetObjectLabelKHR(uint identifier, uint name, int bufSize, PointerData pLengthPtrData, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetObjectLabelKHR(this.swigCPtr, identifier, name, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x060002ED RID: 749 RVA: 0x000081CA File Offset: 0x000063CA
	public virtual void Process_glObjectPtrLabelKHR(uint ptr, int length, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glObjectPtrLabelKHR(this.swigCPtr, ptr, length, PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x060002EE RID: 750 RVA: 0x000081DF File Offset: 0x000063DF
	public virtual void Process_glGetObjectPtrLabelKHR(uint ptr, int bufSize, PointerData pLengthPtrData, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetObjectPtrLabelKHR(this.swigCPtr, ptr, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000081FB File Offset: 0x000063FB
	public virtual void Process_glGetPointervKHR(uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetPointervKHR(this.swigCPtr, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00008210 File Offset: 0x00006410
	public virtual void Process_glPrimitiveBoundingBoxEXT(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPrimitiveBoundingBoxEXT(this.swigCPtr, minX, minY, minZ, minW, maxX, maxY, maxZ, maxW);
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00008235 File Offset: 0x00006435
	public virtual void Process_glPatchParameteriEXT(uint pname, int value)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPatchParameteriEXT(this.swigCPtr, pname, value);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00008244 File Offset: 0x00006444
	public virtual void Process_glDrawElementsBaseVertex(uint mode, int count, uint type, PointerData pIndicesPtrData, int basevertex)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawElementsBaseVertex(this.swigCPtr, mode, count, type, PointerData.getCPtr(pIndicesPtrData), basevertex);
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0000825D File Offset: 0x0000645D
	public virtual void Process_glDrawRangeElementsBaseVertex(uint mode, uint start, uint end, int count, uint type, PointerData pIndicesPtrData, int basevertex)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawRangeElementsBaseVertex(this.swigCPtr, mode, start, end, count, type, PointerData.getCPtr(pIndicesPtrData), basevertex);
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0000827A File Offset: 0x0000647A
	public virtual void Process_glDrawElementsInstancedBaseVertex(uint mode, int count, uint type, PointerData pIndicesPtrData, int instanceCount, int basevertex)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDrawElementsInstancedBaseVertex(this.swigCPtr, mode, count, type, PointerData.getCPtr(pIndicesPtrData), instanceCount, basevertex);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00008295 File Offset: 0x00006495
	public virtual void Process_glFramebufferTextureEXT(uint target, uint attachment, uint texture, int level)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferTextureEXT(this.swigCPtr, target, attachment, texture, level);
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x000082A7 File Offset: 0x000064A7
	public virtual void Process_glFramebufferTextureMultiviewOVR(uint target, uint attachment, uint texture, int level, int baseViewIndex, int numViews)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferTextureMultiviewOVR(this.swigCPtr, target, attachment, texture, level, baseViewIndex, numViews);
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x000082BD File Offset: 0x000064BD
	public virtual void Process_glFramebufferTextureMultisampleMultiviewOVR(uint target, uint attachment, uint texture, int level, int samples, int baseView, int numViews)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferTextureMultisampleMultiviewOVR(this.swigCPtr, target, attachment, texture, level, samples, baseView, numViews);
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x000082D5 File Offset: 0x000064D5
	public virtual void Process_glBufferStorageEXT(uint target, int size, PointerData pDataPtrData, uint flags)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBufferStorageEXT(this.swigCPtr, target, size, PointerData.getCPtr(pDataPtrData), flags);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x000082EC File Offset: 0x000064EC
	public virtual void Process_glGetGraphicsResetStatus(uint returnVal)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetGraphicsResetStatus(this.swigCPtr, returnVal);
	}

	// Token: 0x060002FA RID: 762 RVA: 0x000082FC File Offset: 0x000064FC
	public virtual void Process_glReadnPixels(int x, int y, int width, int height, uint format, uint type, int bufSize, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glReadnPixels(this.swigCPtr, x, y, width, height, format, type, bufSize, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00008326 File Offset: 0x00006526
	public virtual void Process_glGetnUniformfv(uint program, uint location, int bufSize, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetnUniformfv(this.swigCPtr, program, location, bufSize, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002FC RID: 764 RVA: 0x0000833D File Offset: 0x0000653D
	public virtual void Process_glGetnUniformiv(uint program, uint location, int bufSize, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetnUniformiv(this.swigCPtr, program, location, bufSize, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00008354 File Offset: 0x00006554
	public virtual void Process_glGetnUniformuiv(uint program, uint location, int bufSize, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetnUniformuiv(this.swigCPtr, program, location, bufSize, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0000836B File Offset: 0x0000656B
	public virtual void Process_glTexParameterIiv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexParameterIiv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00008380 File Offset: 0x00006580
	public virtual void Process_glTexParameterIuiv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexParameterIuiv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00008395 File Offset: 0x00006595
	public virtual void Process_glGetTexParameterIiv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetTexParameterIiv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000301 RID: 769 RVA: 0x000083AA File Offset: 0x000065AA
	public virtual void Process_glGetTexParameterIuiv(uint target, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetTexParameterIuiv(this.swigCPtr, target, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000302 RID: 770 RVA: 0x000083BF File Offset: 0x000065BF
	public virtual void Process_glSamplerParameterIiv(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSamplerParameterIiv(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000303 RID: 771 RVA: 0x000083D4 File Offset: 0x000065D4
	public virtual void Process_glSamplerParameterIuiv(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSamplerParameterIuiv(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000304 RID: 772 RVA: 0x000083E9 File Offset: 0x000065E9
	public virtual void Process_glGetSamplerParameterIiv(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetSamplerParameterIiv(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000305 RID: 773 RVA: 0x000083FE File Offset: 0x000065FE
	public virtual void Process_glGetSamplerParameterIuiv(uint sampler, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetSamplerParameterIuiv(this.swigCPtr, sampler, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00008413 File Offset: 0x00006613
	public virtual void Process_glNumBinsPerSubmitQCOM(uint numBins, int separateBinningPass)
	{
		libDCAPPINVOKE.GLAdapter_Process_glNumBinsPerSubmitQCOM(this.swigCPtr, numBins, separateBinningPass);
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00008424 File Offset: 0x00006624
	public virtual void Process_glCopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCopyImageSubData(this.swigCPtr, srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00008457 File Offset: 0x00006657
	public virtual void Process_glBlendBarrier()
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendBarrier(this.swigCPtr);
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00008464 File Offset: 0x00006664
	public virtual void Process_glMinSampleShading(float value)
	{
		libDCAPPINVOKE.GLAdapter_Process_glMinSampleShading(this.swigCPtr, value);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00008472 File Offset: 0x00006672
	public virtual void Process_glEnablei(uint target, uint index)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEnablei(this.swigCPtr, target, index);
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00008481 File Offset: 0x00006681
	public virtual void Process_glDisablei(uint target, uint index)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDisablei(this.swigCPtr, target, index);
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00008490 File Offset: 0x00006690
	public virtual void Process_glBlendEquationi(uint buf, uint mode)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendEquationi(this.swigCPtr, buf, mode);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0000849F File Offset: 0x0000669F
	public virtual void Process_glBlendEquationSeparatei(uint buf, uint modeRGB, uint modeAlpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendEquationSeparatei(this.swigCPtr, buf, modeRGB, modeAlpha);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x000084AF File Offset: 0x000066AF
	public virtual void Process_glBlendFunci(uint buf, uint src, uint dst)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendFunci(this.swigCPtr, buf, src, dst);
	}

	// Token: 0x0600030F RID: 783 RVA: 0x000084BF File Offset: 0x000066BF
	public virtual void Process_glBlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlendFuncSeparatei(this.swigCPtr, buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
	}

	// Token: 0x06000310 RID: 784 RVA: 0x000084D3 File Offset: 0x000066D3
	public virtual void Process_glColorMaski(uint buf, int r, int g, int b, int a)
	{
		libDCAPPINVOKE.GLAdapter_Process_glColorMaski(this.swigCPtr, buf, r, g, b, a);
	}

	// Token: 0x06000311 RID: 785 RVA: 0x000084E7 File Offset: 0x000066E7
	public virtual void Process_glIsEnabledi(int returnVal, uint target, uint index)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsEnabledi(this.swigCPtr, returnVal, target, index);
	}

	// Token: 0x06000312 RID: 786 RVA: 0x000084F7 File Offset: 0x000066F7
	public virtual void Process_glTexBuffer(uint target, uint internalFormat, uint buffer)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexBuffer(this.swigCPtr, target, internalFormat, buffer);
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00008507 File Offset: 0x00006707
	public virtual void Process_glTexBufferRange(uint target, uint internalFormat, uint buffer, int offset, int size)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexBufferRange(this.swigCPtr, target, internalFormat, buffer, offset, size);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0000851B File Offset: 0x0000671B
	public virtual void Process_glDebugMessageControl(uint source, uint type, uint severity, int count, PointerData pIdsPtrData, int enabled)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDebugMessageControl(this.swigCPtr, source, type, severity, count, PointerData.getCPtr(pIdsPtrData), enabled);
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00008536 File Offset: 0x00006736
	public virtual void Process_glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, PointerData pBufPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDebugMessageInsert(this.swigCPtr, source, type, id, severity, length, PointerData.getCPtr(pBufPtrData));
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00008551 File Offset: 0x00006751
	public virtual void Process_glDebugMessageCallback(PointerData pCallbackPtrData, PointerData pUserParamPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDebugMessageCallback(this.swigCPtr, PointerData.getCPtr(pCallbackPtrData), PointerData.getCPtr(pUserParamPtrData));
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0000856C File Offset: 0x0000676C
	public virtual void Process_glGetDebugMessageLog(uint returnVal, uint count, int bufSize, PointerData pSourcesPtrData, PointerData pTypesPtrData, PointerData pIdsPtrData, PointerData pSeveritiesPtrData, PointerData pLengthsPtrData, PointerData pMessageLogPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetDebugMessageLog(this.swigCPtr, returnVal, count, bufSize, PointerData.getCPtr(pSourcesPtrData), PointerData.getCPtr(pTypesPtrData), PointerData.getCPtr(pIdsPtrData), PointerData.getCPtr(pSeveritiesPtrData), PointerData.getCPtr(pLengthsPtrData), PointerData.getCPtr(pMessageLogPtrData));
	}

	// Token: 0x06000318 RID: 792 RVA: 0x000085B1 File Offset: 0x000067B1
	public virtual void Process_glPushDebugGroup(uint source, uint id, int length, PointerData pMessagePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPushDebugGroup(this.swigCPtr, source, id, length, PointerData.getCPtr(pMessagePtrData));
	}

	// Token: 0x06000319 RID: 793 RVA: 0x000085C8 File Offset: 0x000067C8
	public virtual void Process_glPopDebugGroup()
	{
		libDCAPPINVOKE.GLAdapter_Process_glPopDebugGroup(this.swigCPtr);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x000085D5 File Offset: 0x000067D5
	public virtual void Process_glObjectLabel(uint identifier, uint name, int length, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glObjectLabel(this.swigCPtr, identifier, name, length, PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000085EC File Offset: 0x000067EC
	public virtual void Process_glGetObjectLabel(uint identifier, uint name, int bufSize, PointerData pLengthPtrData, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetObjectLabel(this.swigCPtr, identifier, name, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0000860A File Offset: 0x0000680A
	public virtual void Process_glObjectPtrLabel(uint ptr, int length, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glObjectPtrLabel(this.swigCPtr, ptr, length, PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0000861F File Offset: 0x0000681F
	public virtual void Process_glGetObjectPtrLabel(uint ptr, int bufSize, PointerData pLengthPtrData, PointerData pLabelPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetObjectPtrLabel(this.swigCPtr, ptr, bufSize, PointerData.getCPtr(pLengthPtrData), PointerData.getCPtr(pLabelPtrData));
	}

	// Token: 0x0600031E RID: 798 RVA: 0x0000863B File Offset: 0x0000683B
	public virtual void Process_glGetPointerv(uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetPointerv(this.swigCPtr, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00008650 File Offset: 0x00006850
	public virtual void Process_glPrimitiveBoundingBox(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPrimitiveBoundingBox(this.swigCPtr, minX, minY, minZ, minW, maxX, maxY, maxZ, maxW);
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00008675 File Offset: 0x00006875
	public virtual void Process_glBlitBlendColor(float red, float green, float blue, float alpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlitBlendColor(this.swigCPtr, red, green, blue, alpha);
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00008687 File Offset: 0x00006887
	public virtual void Process_glBlitBlendEquationSeparate(uint modeRGB, uint modeAlpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlitBlendEquationSeparate(this.swigCPtr, modeRGB, modeAlpha);
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00008696 File Offset: 0x00006896
	public virtual void Process_glBlitBlendFuncSeparate(uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlitBlendFuncSeparate(this.swigCPtr, srcRGB, dstRGB, srcAlpha, dstAlpha);
	}

	// Token: 0x06000323 RID: 803 RVA: 0x000086A8 File Offset: 0x000068A8
	public virtual void Process_glBlitRotation(uint rot)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBlitRotation(this.swigCPtr, rot);
	}

	// Token: 0x06000324 RID: 804 RVA: 0x000086B6 File Offset: 0x000068B6
	public virtual void Process_glBindSharedBufferQCOM(uint target, int sizeInBytes, int fd)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindSharedBufferQCOM(this.swigCPtr, target, sizeInBytes, fd);
	}

	// Token: 0x06000325 RID: 805 RVA: 0x000086C6 File Offset: 0x000068C6
	public virtual void Process_glCreateSharedBufferQCOM(int sizeInBytes, uint cacheMode, uint sharedList, PointerData pOutFdPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCreateSharedBufferQCOM(this.swigCPtr, sizeInBytes, cacheMode, sharedList, PointerData.getCPtr(pOutFdPtrData));
	}

	// Token: 0x06000326 RID: 806 RVA: 0x000086DD File Offset: 0x000068DD
	public virtual void Process_glDestroySharedBufferQCOM(int fd)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDestroySharedBufferQCOM(this.swigCPtr, fd);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000086EB File Offset: 0x000068EB
	public virtual void Process_glTextureBarrier()
	{
		libDCAPPINVOKE.GLAdapter_Process_glTextureBarrier(this.swigCPtr);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x000086F8 File Offset: 0x000068F8
	public virtual void Process_glFramebufferFoveationConfigQCOM(uint framebuffer, uint numLayers, uint focalPointsPerLayer, uint requestedFeatures, PointerData pProvidedFeaturesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferFoveationConfigQCOM(this.swigCPtr, framebuffer, numLayers, focalPointsPerLayer, requestedFeatures, PointerData.getCPtr(pProvidedFeaturesPtrData));
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00008714 File Offset: 0x00006914
	public virtual void Process_glFramebufferFoveationParametersQCOM(uint framebuffer, uint layer, uint focalPoint, float focalX, float focalY, float gainX, float gainY, float foveaArea)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferFoveationParametersQCOM(this.swigCPtr, framebuffer, layer, focalPoint, focalX, focalY, gainX, gainY, foveaArea);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00008739 File Offset: 0x00006939
	public virtual void Process_glBufferStorageExternalEXT(uint target, int offset, int size, uint clientBuffer, uint flags)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBufferStorageExternalEXT(this.swigCPtr, target, offset, size, clientBuffer, flags);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0000874D File Offset: 0x0000694D
	public virtual void Process_glFramebufferFetchBarrierQCOM()
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferFetchBarrierQCOM(this.swigCPtr);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0000875A File Offset: 0x0000695A
	public virtual void Process_glCreateMemoryObjectsEXT(int n, PointerData pMemoryObjectsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glCreateMemoryObjectsEXT(this.swigCPtr, n, PointerData.getCPtr(pMemoryObjectsPtrData));
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0000876E File Offset: 0x0000696E
	public virtual void Process_glDeleteMemoryObjectsEXT(int n, PointerData pMemoryObjectsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteMemoryObjectsEXT(this.swigCPtr, n, PointerData.getCPtr(pMemoryObjectsPtrData));
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00008782 File Offset: 0x00006982
	public virtual void Process_glIsMemoryObjectEXT(int returnVal, uint memoryObject)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsMemoryObjectEXT(this.swigCPtr, returnVal, memoryObject);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00008791 File Offset: 0x00006991
	public virtual void Process_glMemoryObjectParameterivEXT(uint memoryObject, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glMemoryObjectParameterivEXT(this.swigCPtr, memoryObject, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000330 RID: 816 RVA: 0x000087A6 File Offset: 0x000069A6
	public virtual void Process_glGetMemoryObjectParameterivEXT(uint memoryObject, uint pname, PointerData pParamsPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetMemoryObjectParameterivEXT(this.swigCPtr, memoryObject, pname, PointerData.getCPtr(pParamsPtrData));
	}

	// Token: 0x06000331 RID: 817 RVA: 0x000087BB File Offset: 0x000069BB
	public virtual void Process_glTexStorageMem2DEXT(uint target, int levels, uint internalFormat, int width, int height, uint memory, ulong offset)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexStorageMem2DEXT(this.swigCPtr, target, levels, internalFormat, width, height, memory, offset);
	}

	// Token: 0x06000332 RID: 818 RVA: 0x000087D4 File Offset: 0x000069D4
	public virtual void Process_glTexStorageMem2DMultisampleEXT(uint target, int samples, uint internalFormat, int width, int height, int fixedSampleLocations, uint memory, ulong offset)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexStorageMem2DMultisampleEXT(this.swigCPtr, target, samples, internalFormat, width, height, fixedSampleLocations, memory, offset);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x000087FC File Offset: 0x000069FC
	public virtual void Process_glTexStorageMem3DEXT(uint target, int levels, uint internalFormat, int width, int height, int depth, uint memory, ulong offset)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexStorageMem3DEXT(this.swigCPtr, target, levels, internalFormat, width, height, depth, memory, offset);
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00008824 File Offset: 0x00006A24
	public virtual void Process_glTexStorageMem3DMultisampleEXT(uint target, int samples, uint internalFormat, int width, int height, int depth, int fixedSampleLocations, uint memory, ulong offset)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexStorageMem3DMultisampleEXT(this.swigCPtr, target, samples, internalFormat, width, height, depth, fixedSampleLocations, memory, offset);
	}

	// Token: 0x06000335 RID: 821 RVA: 0x0000884B File Offset: 0x00006A4B
	public virtual void Process_glBufferStorageMemEXT(uint target, int size, uint memory, ulong offset)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBufferStorageMemEXT(this.swigCPtr, target, size, memory, offset);
	}

	// Token: 0x06000336 RID: 822 RVA: 0x0000885D File Offset: 0x00006A5D
	public virtual void Process_glGenSemaphoresKHR(int n, PointerData pSemaphoresPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGenSemaphoresKHR(this.swigCPtr, n, PointerData.getCPtr(pSemaphoresPtrData));
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00008871 File Offset: 0x00006A71
	public virtual void Process_glDeleteSemaphoresKHR(int n, PointerData pSemaphoresPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glDeleteSemaphoresKHR(this.swigCPtr, n, PointerData.getCPtr(pSemaphoresPtrData));
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00008885 File Offset: 0x00006A85
	public virtual void Process_glIsSemaphoreKHR(int returnVal, uint semaphore)
	{
		libDCAPPINVOKE.GLAdapter_Process_glIsSemaphoreKHR(this.swigCPtr, returnVal, semaphore);
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00008894 File Offset: 0x00006A94
	public virtual void Process_glWaitSemaphoreKHR(uint semaphore, PointerData pSrcExternalUasgePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glWaitSemaphoreKHR(this.swigCPtr, semaphore, PointerData.getCPtr(pSrcExternalUasgePtrData));
	}

	// Token: 0x0600033A RID: 826 RVA: 0x000088A8 File Offset: 0x00006AA8
	public virtual void Process_glSignalSemaphoreKHR(PointerData pDstExternalUsagePtrData, uint semaphore)
	{
		libDCAPPINVOKE.GLAdapter_Process_glSignalSemaphoreKHR(this.swigCPtr, PointerData.getCPtr(pDstExternalUsagePtrData), semaphore);
	}

	// Token: 0x0600033B RID: 827 RVA: 0x000088BC File Offset: 0x00006ABC
	public virtual void Process_glImportMemoryFdEXT(uint memory, ulong size, uint handleType, int fd)
	{
		libDCAPPINVOKE.GLAdapter_Process_glImportMemoryFdEXT(this.swigCPtr, memory, size, handleType, fd);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x000088CE File Offset: 0x00006ACE
	public virtual void Process_glImportSemaphoreFdEXT(uint semaphore, uint handleType, int fd)
	{
		libDCAPPINVOKE.GLAdapter_Process_glImportSemaphoreFdEXT(this.swigCPtr, semaphore, handleType, fd);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x000088DE File Offset: 0x00006ADE
	public virtual void Process_glGetUnsignedBytevEXT(uint pname, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetUnsignedBytevEXT(this.swigCPtr, pname, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x0600033E RID: 830 RVA: 0x000088F2 File Offset: 0x00006AF2
	public virtual void Process_glGetUnsignedBytei_vEXT(uint target, uint index, PointerData pDataPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetUnsignedBytei_vEXT(this.swigCPtr, target, index, PointerData.getCPtr(pDataPtrData));
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00008908 File Offset: 0x00006B08
	public virtual void Process_glTextureFoveationParametersQCOM(uint texture, uint layer, uint focalPoint, float focalX, float focalY, float gainX, float gainY, float foveaArea)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTextureFoveationParametersQCOM(this.swigCPtr, texture, layer, focalPoint, focalX, focalY, gainX, gainY, foveaArea);
	}

	// Token: 0x06000340 RID: 832 RVA: 0x0000892D File Offset: 0x00006B2D
	public virtual void Process_glBindFragDataLocationIndexedEXT(uint program, uint colorNumber, uint index, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindFragDataLocationIndexedEXT(this.swigCPtr, program, colorNumber, index, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00008944 File Offset: 0x00006B44
	public virtual void Process_glBindFragDataLocationEXT(uint program, uint color, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glBindFragDataLocationEXT(this.swigCPtr, program, color, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00008959 File Offset: 0x00006B59
	public virtual void Process_glGetProgramResourceLocationIndexEXT(uint returnVal, uint program, uint programInterface, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetProgramResourceLocationIndexEXT(this.swigCPtr, returnVal, program, programInterface, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00008970 File Offset: 0x00006B70
	public virtual void Process_glGetFragDataIndexEXT(uint returnVal, uint program, PointerData pNamePtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetFragDataIndexEXT(this.swigCPtr, returnVal, program, PointerData.getCPtr(pNamePtrData));
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00008985 File Offset: 0x00006B85
	public virtual void Process_glShadingRateQCOM(uint rate)
	{
		libDCAPPINVOKE.GLAdapter_Process_glShadingRateQCOM(this.swigCPtr, rate);
	}

	// Token: 0x06000345 RID: 837 RVA: 0x00008993 File Offset: 0x00006B93
	public virtual void Process_glExtrapolateTex2DQCOM(uint src1, uint src2, uint output, float scaleFactor)
	{
		libDCAPPINVOKE.GLAdapter_Process_glExtrapolateTex2DQCOM(this.swigCPtr, src1, src2, output, scaleFactor);
	}

	// Token: 0x06000346 RID: 838 RVA: 0x000089A8 File Offset: 0x00006BA8
	public virtual void Process_glTextureViewOES(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTextureViewOES(this.swigCPtr, texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers);
	}

	// Token: 0x06000347 RID: 839 RVA: 0x000089CD File Offset: 0x00006BCD
	public virtual void Process_glTexEstimateMotionQCOM(uint arg0, uint target, uint output)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexEstimateMotionQCOM(this.swigCPtr, arg0, target, output);
	}

	// Token: 0x06000348 RID: 840 RVA: 0x000089DD File Offset: 0x00006BDD
	public virtual void Process_glTexEstimateMotionRegionsQCOM(uint arg0, uint target, uint output, uint mask)
	{
		libDCAPPINVOKE.GLAdapter_Process_glTexEstimateMotionRegionsQCOM(this.swigCPtr, arg0, target, output, mask);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x000089EF File Offset: 0x00006BEF
	public virtual void Process_glEGLImageTargetTexStorageEXT(uint target, uint image, PointerData pAttribListPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glEGLImageTargetTexStorageEXT(this.swigCPtr, target, image, PointerData.getCPtr(pAttribListPtrData));
	}

	// Token: 0x0600034A RID: 842 RVA: 0x00008A04 File Offset: 0x00006C04
	public virtual void Process_glPolygonOffsetClampEXT(float factor, float units, float clamp)
	{
		libDCAPPINVOKE.GLAdapter_Process_glPolygonOffsetClampEXT(this.swigCPtr, factor, units, clamp);
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00008A14 File Offset: 0x00006C14
	public virtual void Process_glGetFragmentShadingRatesEXT(int samples, int maxCount, PointerData pCountPtrData, PointerData pShadingRatesPtrData)
	{
		libDCAPPINVOKE.GLAdapter_Process_glGetFragmentShadingRatesEXT(this.swigCPtr, samples, maxCount, PointerData.getCPtr(pCountPtrData), PointerData.getCPtr(pShadingRatesPtrData));
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00008A30 File Offset: 0x00006C30
	public virtual void Process_glShadingRateEXT(uint rate)
	{
		libDCAPPINVOKE.GLAdapter_Process_glShadingRateEXT(this.swigCPtr, rate);
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00008A3E File Offset: 0x00006C3E
	public virtual void Process_glShadingRateCombinerOpsEXT(uint combinerOp0, uint combinerOp1)
	{
		libDCAPPINVOKE.GLAdapter_Process_glShadingRateCombinerOpsEXT(this.swigCPtr, combinerOp0, combinerOp1);
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00008A4D File Offset: 0x00006C4D
	public virtual void Process_glFramebufferShadingRateEXT(uint target, uint attachment, uint texture, int baseLayer, int numLayers, int texelWidth, int texelHeight)
	{
		libDCAPPINVOKE.GLAdapter_Process_glFramebufferShadingRateEXT(this.swigCPtr, target, attachment, texture, baseLayer, numLayers, texelWidth, texelHeight);
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00008A68 File Offset: 0x00006C68
	private void SwigDirectorConnect()
	{
		if (this.SwigDerivedClassHasMethod("SetCurrentThread", GLAdapter.swigMethodTypes0))
		{
			this.swigDelegate0 = new GLAdapter.SwigDelegateGLAdapter_0(this.SwigDirectorSetCurrentThread);
		}
		if (this.SwigDerivedClassHasMethod("ProcessVertexAttribData", GLAdapter.swigMethodTypes1))
		{
			this.swigDelegate1 = new GLAdapter.SwigDelegateGLAdapter_1(this.SwigDirectorProcessVertexAttribData);
		}
		if (this.SwigDerivedClassHasMethod("ProcessVertexAttribIData", GLAdapter.swigMethodTypes2))
		{
			this.swigDelegate2 = new GLAdapter.SwigDelegateGLAdapter_2(this.SwigDirectorProcessVertexAttribIData);
		}
		if (this.SwigDerivedClassHasMethod("ProcessFlushMappedBufferRange", GLAdapter.swigMethodTypes3))
		{
			this.swigDelegate3 = new GLAdapter.SwigDelegateGLAdapter_3(this.SwigDirectorProcessFlushMappedBufferRange);
		}
		if (this.SwigDerivedClassHasMethod("ProcessUnmapBuffer", GLAdapter.swigMethodTypes4))
		{
			this.swigDelegate4 = new GLAdapter.SwigDelegateGLAdapter_4(this.SwigDirectorProcessUnmapBuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glActiveTexture", GLAdapter.swigMethodTypes5))
		{
			this.swigDelegate5 = new GLAdapter.SwigDelegateGLAdapter_5(this.SwigDirectorProcess_glActiveTexture);
		}
		if (this.SwigDerivedClassHasMethod("Process_glAttachShader", GLAdapter.swigMethodTypes6))
		{
			this.swigDelegate6 = new GLAdapter.SwigDelegateGLAdapter_6(this.SwigDirectorProcess_glAttachShader);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindAttribLocation", GLAdapter.swigMethodTypes7))
		{
			this.swigDelegate7 = new GLAdapter.SwigDelegateGLAdapter_7(this.SwigDirectorProcess_glBindAttribLocation);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindBuffer", GLAdapter.swigMethodTypes8))
		{
			this.swigDelegate8 = new GLAdapter.SwigDelegateGLAdapter_8(this.SwigDirectorProcess_glBindBuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindFramebuffer", GLAdapter.swigMethodTypes9))
		{
			this.swigDelegate9 = new GLAdapter.SwigDelegateGLAdapter_9(this.SwigDirectorProcess_glBindFramebuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindRenderbuffer", GLAdapter.swigMethodTypes10))
		{
			this.swigDelegate10 = new GLAdapter.SwigDelegateGLAdapter_10(this.SwigDirectorProcess_glBindRenderbuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindTexture", GLAdapter.swigMethodTypes11))
		{
			this.swigDelegate11 = new GLAdapter.SwigDelegateGLAdapter_11(this.SwigDirectorProcess_glBindTexture);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendColor", GLAdapter.swigMethodTypes12))
		{
			this.swigDelegate12 = new GLAdapter.SwigDelegateGLAdapter_12(this.SwigDirectorProcess_glBlendColor);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendEquation", GLAdapter.swigMethodTypes13))
		{
			this.swigDelegate13 = new GLAdapter.SwigDelegateGLAdapter_13(this.SwigDirectorProcess_glBlendEquation);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendEquationSeparate", GLAdapter.swigMethodTypes14))
		{
			this.swigDelegate14 = new GLAdapter.SwigDelegateGLAdapter_14(this.SwigDirectorProcess_glBlendEquationSeparate);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendFunc", GLAdapter.swigMethodTypes15))
		{
			this.swigDelegate15 = new GLAdapter.SwigDelegateGLAdapter_15(this.SwigDirectorProcess_glBlendFunc);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendFuncSeparate", GLAdapter.swigMethodTypes16))
		{
			this.swigDelegate16 = new GLAdapter.SwigDelegateGLAdapter_16(this.SwigDirectorProcess_glBlendFuncSeparate);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBufferData", GLAdapter.swigMethodTypes17))
		{
			this.swigDelegate17 = new GLAdapter.SwigDelegateGLAdapter_17(this.SwigDirectorProcess_glBufferData);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBufferSubData", GLAdapter.swigMethodTypes18))
		{
			this.swigDelegate18 = new GLAdapter.SwigDelegateGLAdapter_18(this.SwigDirectorProcess_glBufferSubData);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCheckFramebufferStatus", GLAdapter.swigMethodTypes19))
		{
			this.swigDelegate19 = new GLAdapter.SwigDelegateGLAdapter_19(this.SwigDirectorProcess_glCheckFramebufferStatus);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClear", GLAdapter.swigMethodTypes20))
		{
			this.swigDelegate20 = new GLAdapter.SwigDelegateGLAdapter_20(this.SwigDirectorProcess_glClear);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClearColor", GLAdapter.swigMethodTypes21))
		{
			this.swigDelegate21 = new GLAdapter.SwigDelegateGLAdapter_21(this.SwigDirectorProcess_glClearColor);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClearDepthf", GLAdapter.swigMethodTypes22))
		{
			this.swigDelegate22 = new GLAdapter.SwigDelegateGLAdapter_22(this.SwigDirectorProcess_glClearDepthf);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClearStencil", GLAdapter.swigMethodTypes23))
		{
			this.swigDelegate23 = new GLAdapter.SwigDelegateGLAdapter_23(this.SwigDirectorProcess_glClearStencil);
		}
		if (this.SwigDerivedClassHasMethod("Process_glColorMask", GLAdapter.swigMethodTypes24))
		{
			this.swigDelegate24 = new GLAdapter.SwigDelegateGLAdapter_24(this.SwigDirectorProcess_glColorMask);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCompileShader", GLAdapter.swigMethodTypes25))
		{
			this.swigDelegate25 = new GLAdapter.SwigDelegateGLAdapter_25(this.SwigDirectorProcess_glCompileShader);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCompressedTexImage2D", GLAdapter.swigMethodTypes26))
		{
			this.swigDelegate26 = new GLAdapter.SwigDelegateGLAdapter_26(this.SwigDirectorProcess_glCompressedTexImage2D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCompressedTexSubImage2D", GLAdapter.swigMethodTypes27))
		{
			this.swigDelegate27 = new GLAdapter.SwigDelegateGLAdapter_27(this.SwigDirectorProcess_glCompressedTexSubImage2D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCopyTexImage2D", GLAdapter.swigMethodTypes28))
		{
			this.swigDelegate28 = new GLAdapter.SwigDelegateGLAdapter_28(this.SwigDirectorProcess_glCopyTexImage2D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCopyTexSubImage2D", GLAdapter.swigMethodTypes29))
		{
			this.swigDelegate29 = new GLAdapter.SwigDelegateGLAdapter_29(this.SwigDirectorProcess_glCopyTexSubImage2D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCreateProgram", GLAdapter.swigMethodTypes30))
		{
			this.swigDelegate30 = new GLAdapter.SwigDelegateGLAdapter_30(this.SwigDirectorProcess_glCreateProgram);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCreateShader", GLAdapter.swigMethodTypes31))
		{
			this.swigDelegate31 = new GLAdapter.SwigDelegateGLAdapter_31(this.SwigDirectorProcess_glCreateShader);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCullFace", GLAdapter.swigMethodTypes32))
		{
			this.swigDelegate32 = new GLAdapter.SwigDelegateGLAdapter_32(this.SwigDirectorProcess_glCullFace);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteBuffers", GLAdapter.swigMethodTypes33))
		{
			this.swigDelegate33 = new GLAdapter.SwigDelegateGLAdapter_33(this.SwigDirectorProcess_glDeleteBuffers);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteFramebuffers", GLAdapter.swigMethodTypes34))
		{
			this.swigDelegate34 = new GLAdapter.SwigDelegateGLAdapter_34(this.SwigDirectorProcess_glDeleteFramebuffers);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteProgram", GLAdapter.swigMethodTypes35))
		{
			this.swigDelegate35 = new GLAdapter.SwigDelegateGLAdapter_35(this.SwigDirectorProcess_glDeleteProgram);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteRenderbuffers", GLAdapter.swigMethodTypes36))
		{
			this.swigDelegate36 = new GLAdapter.SwigDelegateGLAdapter_36(this.SwigDirectorProcess_glDeleteRenderbuffers);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteShader", GLAdapter.swigMethodTypes37))
		{
			this.swigDelegate37 = new GLAdapter.SwigDelegateGLAdapter_37(this.SwigDirectorProcess_glDeleteShader);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteTextures", GLAdapter.swigMethodTypes38))
		{
			this.swigDelegate38 = new GLAdapter.SwigDelegateGLAdapter_38(this.SwigDirectorProcess_glDeleteTextures);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDepthFunc", GLAdapter.swigMethodTypes39))
		{
			this.swigDelegate39 = new GLAdapter.SwigDelegateGLAdapter_39(this.SwigDirectorProcess_glDepthFunc);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDepthMask", GLAdapter.swigMethodTypes40))
		{
			this.swigDelegate40 = new GLAdapter.SwigDelegateGLAdapter_40(this.SwigDirectorProcess_glDepthMask);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDepthRangef", GLAdapter.swigMethodTypes41))
		{
			this.swigDelegate41 = new GLAdapter.SwigDelegateGLAdapter_41(this.SwigDirectorProcess_glDepthRangef);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDetachShader", GLAdapter.swigMethodTypes42))
		{
			this.swigDelegate42 = new GLAdapter.SwigDelegateGLAdapter_42(this.SwigDirectorProcess_glDetachShader);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDisable", GLAdapter.swigMethodTypes43))
		{
			this.swigDelegate43 = new GLAdapter.SwigDelegateGLAdapter_43(this.SwigDirectorProcess_glDisable);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDisableVertexAttribArray", GLAdapter.swigMethodTypes44))
		{
			this.swigDelegate44 = new GLAdapter.SwigDelegateGLAdapter_44(this.SwigDirectorProcess_glDisableVertexAttribArray);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawArrays", GLAdapter.swigMethodTypes45))
		{
			this.swigDelegate45 = new GLAdapter.SwigDelegateGLAdapter_45(this.SwigDirectorProcess_glDrawArrays);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawElements", GLAdapter.swigMethodTypes46))
		{
			this.swigDelegate46 = new GLAdapter.SwigDelegateGLAdapter_46(this.SwigDirectorProcess_glDrawElements);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEnable", GLAdapter.swigMethodTypes47))
		{
			this.swigDelegate47 = new GLAdapter.SwigDelegateGLAdapter_47(this.SwigDirectorProcess_glEnable);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEnableVertexAttribArray", GLAdapter.swigMethodTypes48))
		{
			this.swigDelegate48 = new GLAdapter.SwigDelegateGLAdapter_48(this.SwigDirectorProcess_glEnableVertexAttribArray);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFinish", GLAdapter.swigMethodTypes49))
		{
			this.swigDelegate49 = new GLAdapter.SwigDelegateGLAdapter_49(this.SwigDirectorProcess_glFinish);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFlush", GLAdapter.swigMethodTypes50))
		{
			this.swigDelegate50 = new GLAdapter.SwigDelegateGLAdapter_50(this.SwigDirectorProcess_glFlush);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferRenderbuffer", GLAdapter.swigMethodTypes51))
		{
			this.swigDelegate51 = new GLAdapter.SwigDelegateGLAdapter_51(this.SwigDirectorProcess_glFramebufferRenderbuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferTexture2D", GLAdapter.swigMethodTypes52))
		{
			this.swigDelegate52 = new GLAdapter.SwigDelegateGLAdapter_52(this.SwigDirectorProcess_glFramebufferTexture2D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFrontFace", GLAdapter.swigMethodTypes53))
		{
			this.swigDelegate53 = new GLAdapter.SwigDelegateGLAdapter_53(this.SwigDirectorProcess_glFrontFace);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenBuffers", GLAdapter.swigMethodTypes54))
		{
			this.swigDelegate54 = new GLAdapter.SwigDelegateGLAdapter_54(this.SwigDirectorProcess_glGenBuffers);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenerateMipmap", GLAdapter.swigMethodTypes55))
		{
			this.swigDelegate55 = new GLAdapter.SwigDelegateGLAdapter_55(this.SwigDirectorProcess_glGenerateMipmap);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenFramebuffers", GLAdapter.swigMethodTypes56))
		{
			this.swigDelegate56 = new GLAdapter.SwigDelegateGLAdapter_56(this.SwigDirectorProcess_glGenFramebuffers);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenRenderbuffers", GLAdapter.swigMethodTypes57))
		{
			this.swigDelegate57 = new GLAdapter.SwigDelegateGLAdapter_57(this.SwigDirectorProcess_glGenRenderbuffers);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenTextures", GLAdapter.swigMethodTypes58))
		{
			this.swigDelegate58 = new GLAdapter.SwigDelegateGLAdapter_58(this.SwigDirectorProcess_glGenTextures);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetActiveAttrib", GLAdapter.swigMethodTypes59))
		{
			this.swigDelegate59 = new GLAdapter.SwigDelegateGLAdapter_59(this.SwigDirectorProcess_glGetActiveAttrib);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetActiveUniform", GLAdapter.swigMethodTypes60))
		{
			this.swigDelegate60 = new GLAdapter.SwigDelegateGLAdapter_60(this.SwigDirectorProcess_glGetActiveUniform);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetAttachedShaders", GLAdapter.swigMethodTypes61))
		{
			this.swigDelegate61 = new GLAdapter.SwigDelegateGLAdapter_61(this.SwigDirectorProcess_glGetAttachedShaders);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetAttribLocation", GLAdapter.swigMethodTypes62))
		{
			this.swigDelegate62 = new GLAdapter.SwigDelegateGLAdapter_62(this.SwigDirectorProcess_glGetAttribLocation);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetBooleanv", GLAdapter.swigMethodTypes63))
		{
			this.swigDelegate63 = new GLAdapter.SwigDelegateGLAdapter_63(this.SwigDirectorProcess_glGetBooleanv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetBufferParameteriv", GLAdapter.swigMethodTypes64))
		{
			this.swigDelegate64 = new GLAdapter.SwigDelegateGLAdapter_64(this.SwigDirectorProcess_glGetBufferParameteriv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetError", GLAdapter.swigMethodTypes65))
		{
			this.swigDelegate65 = new GLAdapter.SwigDelegateGLAdapter_65(this.SwigDirectorProcess_glGetError);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetFloatv", GLAdapter.swigMethodTypes66))
		{
			this.swigDelegate66 = new GLAdapter.SwigDelegateGLAdapter_66(this.SwigDirectorProcess_glGetFloatv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetFramebufferAttachmentParameteriv", GLAdapter.swigMethodTypes67))
		{
			this.swigDelegate67 = new GLAdapter.SwigDelegateGLAdapter_67(this.SwigDirectorProcess_glGetFramebufferAttachmentParameteriv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetIntegerv", GLAdapter.swigMethodTypes68))
		{
			this.swigDelegate68 = new GLAdapter.SwigDelegateGLAdapter_68(this.SwigDirectorProcess_glGetIntegerv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramiv", GLAdapter.swigMethodTypes69))
		{
			this.swigDelegate69 = new GLAdapter.SwigDelegateGLAdapter_69(this.SwigDirectorProcess_glGetProgramiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramInfoLog", GLAdapter.swigMethodTypes70))
		{
			this.swigDelegate70 = new GLAdapter.SwigDelegateGLAdapter_70(this.SwigDirectorProcess_glGetProgramInfoLog);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetRenderbufferParameteriv", GLAdapter.swigMethodTypes71))
		{
			this.swigDelegate71 = new GLAdapter.SwigDelegateGLAdapter_71(this.SwigDirectorProcess_glGetRenderbufferParameteriv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetShaderiv", GLAdapter.swigMethodTypes72))
		{
			this.swigDelegate72 = new GLAdapter.SwigDelegateGLAdapter_72(this.SwigDirectorProcess_glGetShaderiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetShaderInfoLog", GLAdapter.swigMethodTypes73))
		{
			this.swigDelegate73 = new GLAdapter.SwigDelegateGLAdapter_73(this.SwigDirectorProcess_glGetShaderInfoLog);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetShaderPrecisionFormat", GLAdapter.swigMethodTypes74))
		{
			this.swigDelegate74 = new GLAdapter.SwigDelegateGLAdapter_74(this.SwigDirectorProcess_glGetShaderPrecisionFormat);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetShaderSource", GLAdapter.swigMethodTypes75))
		{
			this.swigDelegate75 = new GLAdapter.SwigDelegateGLAdapter_75(this.SwigDirectorProcess_glGetShaderSource);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetString", GLAdapter.swigMethodTypes76))
		{
			this.swigDelegate76 = new GLAdapter.SwigDelegateGLAdapter_76(this.SwigDirectorProcess_glGetString);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetTexParameterfv", GLAdapter.swigMethodTypes77))
		{
			this.swigDelegate77 = new GLAdapter.SwigDelegateGLAdapter_77(this.SwigDirectorProcess_glGetTexParameterfv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetTexParameteriv", GLAdapter.swigMethodTypes78))
		{
			this.swigDelegate78 = new GLAdapter.SwigDelegateGLAdapter_78(this.SwigDirectorProcess_glGetTexParameteriv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetUniformfv", GLAdapter.swigMethodTypes79))
		{
			this.swigDelegate79 = new GLAdapter.SwigDelegateGLAdapter_79(this.SwigDirectorProcess_glGetUniformfv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetUniformiv", GLAdapter.swigMethodTypes80))
		{
			this.swigDelegate80 = new GLAdapter.SwigDelegateGLAdapter_80(this.SwigDirectorProcess_glGetUniformiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetUniformLocation", GLAdapter.swigMethodTypes81))
		{
			this.swigDelegate81 = new GLAdapter.SwigDelegateGLAdapter_81(this.SwigDirectorProcess_glGetUniformLocation);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetVertexAttribfv", GLAdapter.swigMethodTypes82))
		{
			this.swigDelegate82 = new GLAdapter.SwigDelegateGLAdapter_82(this.SwigDirectorProcess_glGetVertexAttribfv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetVertexAttribiv", GLAdapter.swigMethodTypes83))
		{
			this.swigDelegate83 = new GLAdapter.SwigDelegateGLAdapter_83(this.SwigDirectorProcess_glGetVertexAttribiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetVertexAttribPointerv", GLAdapter.swigMethodTypes84))
		{
			this.swigDelegate84 = new GLAdapter.SwigDelegateGLAdapter_84(this.SwigDirectorProcess_glGetVertexAttribPointerv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glHint", GLAdapter.swigMethodTypes85))
		{
			this.swigDelegate85 = new GLAdapter.SwigDelegateGLAdapter_85(this.SwigDirectorProcess_glHint);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsBuffer", GLAdapter.swigMethodTypes86))
		{
			this.swigDelegate86 = new GLAdapter.SwigDelegateGLAdapter_86(this.SwigDirectorProcess_glIsBuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsEnabled", GLAdapter.swigMethodTypes87))
		{
			this.swigDelegate87 = new GLAdapter.SwigDelegateGLAdapter_87(this.SwigDirectorProcess_glIsEnabled);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsFramebuffer", GLAdapter.swigMethodTypes88))
		{
			this.swigDelegate88 = new GLAdapter.SwigDelegateGLAdapter_88(this.SwigDirectorProcess_glIsFramebuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsProgram", GLAdapter.swigMethodTypes89))
		{
			this.swigDelegate89 = new GLAdapter.SwigDelegateGLAdapter_89(this.SwigDirectorProcess_glIsProgram);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsRenderbuffer", GLAdapter.swigMethodTypes90))
		{
			this.swigDelegate90 = new GLAdapter.SwigDelegateGLAdapter_90(this.SwigDirectorProcess_glIsRenderbuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsShader", GLAdapter.swigMethodTypes91))
		{
			this.swigDelegate91 = new GLAdapter.SwigDelegateGLAdapter_91(this.SwigDirectorProcess_glIsShader);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsTexture", GLAdapter.swigMethodTypes92))
		{
			this.swigDelegate92 = new GLAdapter.SwigDelegateGLAdapter_92(this.SwigDirectorProcess_glIsTexture);
		}
		if (this.SwigDerivedClassHasMethod("Process_glLineWidth", GLAdapter.swigMethodTypes93))
		{
			this.swigDelegate93 = new GLAdapter.SwigDelegateGLAdapter_93(this.SwigDirectorProcess_glLineWidth);
		}
		if (this.SwigDerivedClassHasMethod("Process_glLinkProgram", GLAdapter.swigMethodTypes94))
		{
			this.swigDelegate94 = new GLAdapter.SwigDelegateGLAdapter_94(this.SwigDirectorProcess_glLinkProgram);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPixelStorei", GLAdapter.swigMethodTypes95))
		{
			this.swigDelegate95 = new GLAdapter.SwigDelegateGLAdapter_95(this.SwigDirectorProcess_glPixelStorei);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPolygonOffset", GLAdapter.swigMethodTypes96))
		{
			this.swigDelegate96 = new GLAdapter.SwigDelegateGLAdapter_96(this.SwigDirectorProcess_glPolygonOffset);
		}
		if (this.SwigDerivedClassHasMethod("Process_glReadPixels", GLAdapter.swigMethodTypes97))
		{
			this.swigDelegate97 = new GLAdapter.SwigDelegateGLAdapter_97(this.SwigDirectorProcess_glReadPixels);
		}
		if (this.SwigDerivedClassHasMethod("Process_glReleaseShaderCompiler", GLAdapter.swigMethodTypes98))
		{
			this.swigDelegate98 = new GLAdapter.SwigDelegateGLAdapter_98(this.SwigDirectorProcess_glReleaseShaderCompiler);
		}
		if (this.SwigDerivedClassHasMethod("Process_glRenderbufferStorage", GLAdapter.swigMethodTypes99))
		{
			this.swigDelegate99 = new GLAdapter.SwigDelegateGLAdapter_99(this.SwigDirectorProcess_glRenderbufferStorage);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSampleCoverage", GLAdapter.swigMethodTypes100))
		{
			this.swigDelegate100 = new GLAdapter.SwigDelegateGLAdapter_100(this.SwigDirectorProcess_glSampleCoverage);
		}
		if (this.SwigDerivedClassHasMethod("Process_glScissor", GLAdapter.swigMethodTypes101))
		{
			this.swigDelegate101 = new GLAdapter.SwigDelegateGLAdapter_101(this.SwigDirectorProcess_glScissor);
		}
		if (this.SwigDerivedClassHasMethod("Process_glShaderBinary", GLAdapter.swigMethodTypes102))
		{
			this.swigDelegate102 = new GLAdapter.SwigDelegateGLAdapter_102(this.SwigDirectorProcess_glShaderBinary);
		}
		if (this.SwigDerivedClassHasMethod("Process_glShaderSource", GLAdapter.swigMethodTypes103))
		{
			this.swigDelegate103 = new GLAdapter.SwigDelegateGLAdapter_103(this.SwigDirectorProcess_glShaderSource);
		}
		if (this.SwigDerivedClassHasMethod("Process_glStencilFunc", GLAdapter.swigMethodTypes104))
		{
			this.swigDelegate104 = new GLAdapter.SwigDelegateGLAdapter_104(this.SwigDirectorProcess_glStencilFunc);
		}
		if (this.SwigDerivedClassHasMethod("Process_glStencilFuncSeparate", GLAdapter.swigMethodTypes105))
		{
			this.swigDelegate105 = new GLAdapter.SwigDelegateGLAdapter_105(this.SwigDirectorProcess_glStencilFuncSeparate);
		}
		if (this.SwigDerivedClassHasMethod("Process_glStencilMask", GLAdapter.swigMethodTypes106))
		{
			this.swigDelegate106 = new GLAdapter.SwigDelegateGLAdapter_106(this.SwigDirectorProcess_glStencilMask);
		}
		if (this.SwigDerivedClassHasMethod("Process_glStencilMaskSeparate", GLAdapter.swigMethodTypes107))
		{
			this.swigDelegate107 = new GLAdapter.SwigDelegateGLAdapter_107(this.SwigDirectorProcess_glStencilMaskSeparate);
		}
		if (this.SwigDerivedClassHasMethod("Process_glStencilOp", GLAdapter.swigMethodTypes108))
		{
			this.swigDelegate108 = new GLAdapter.SwigDelegateGLAdapter_108(this.SwigDirectorProcess_glStencilOp);
		}
		if (this.SwigDerivedClassHasMethod("Process_glStencilOpSeparate", GLAdapter.swigMethodTypes109))
		{
			this.swigDelegate109 = new GLAdapter.SwigDelegateGLAdapter_109(this.SwigDirectorProcess_glStencilOpSeparate);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexImage2D", GLAdapter.swigMethodTypes110))
		{
			this.swigDelegate110 = new GLAdapter.SwigDelegateGLAdapter_110(this.SwigDirectorProcess_glTexImage2D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexParameterf", GLAdapter.swigMethodTypes111))
		{
			this.swigDelegate111 = new GLAdapter.SwigDelegateGLAdapter_111(this.SwigDirectorProcess_glTexParameterf);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexParameterfv", GLAdapter.swigMethodTypes112))
		{
			this.swigDelegate112 = new GLAdapter.SwigDelegateGLAdapter_112(this.SwigDirectorProcess_glTexParameterfv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexParameteri", GLAdapter.swigMethodTypes113))
		{
			this.swigDelegate113 = new GLAdapter.SwigDelegateGLAdapter_113(this.SwigDirectorProcess_glTexParameteri);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexParameteriv", GLAdapter.swigMethodTypes114))
		{
			this.swigDelegate114 = new GLAdapter.SwigDelegateGLAdapter_114(this.SwigDirectorProcess_glTexParameteriv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexSubImage2D", GLAdapter.swigMethodTypes115))
		{
			this.swigDelegate115 = new GLAdapter.SwigDelegateGLAdapter_115(this.SwigDirectorProcess_glTexSubImage2D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform1f", GLAdapter.swigMethodTypes116))
		{
			this.swigDelegate116 = new GLAdapter.SwigDelegateGLAdapter_116(this.SwigDirectorProcess_glUniform1f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform1fv", GLAdapter.swigMethodTypes117))
		{
			this.swigDelegate117 = new GLAdapter.SwigDelegateGLAdapter_117(this.SwigDirectorProcess_glUniform1fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform1i", GLAdapter.swigMethodTypes118))
		{
			this.swigDelegate118 = new GLAdapter.SwigDelegateGLAdapter_118(this.SwigDirectorProcess_glUniform1i);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform1iv", GLAdapter.swigMethodTypes119))
		{
			this.swigDelegate119 = new GLAdapter.SwigDelegateGLAdapter_119(this.SwigDirectorProcess_glUniform1iv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform2f", GLAdapter.swigMethodTypes120))
		{
			this.swigDelegate120 = new GLAdapter.SwigDelegateGLAdapter_120(this.SwigDirectorProcess_glUniform2f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform2fv", GLAdapter.swigMethodTypes121))
		{
			this.swigDelegate121 = new GLAdapter.SwigDelegateGLAdapter_121(this.SwigDirectorProcess_glUniform2fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform2i", GLAdapter.swigMethodTypes122))
		{
			this.swigDelegate122 = new GLAdapter.SwigDelegateGLAdapter_122(this.SwigDirectorProcess_glUniform2i);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform2iv", GLAdapter.swigMethodTypes123))
		{
			this.swigDelegate123 = new GLAdapter.SwigDelegateGLAdapter_123(this.SwigDirectorProcess_glUniform2iv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform3f", GLAdapter.swigMethodTypes124))
		{
			this.swigDelegate124 = new GLAdapter.SwigDelegateGLAdapter_124(this.SwigDirectorProcess_glUniform3f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform3fv", GLAdapter.swigMethodTypes125))
		{
			this.swigDelegate125 = new GLAdapter.SwigDelegateGLAdapter_125(this.SwigDirectorProcess_glUniform3fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform3i", GLAdapter.swigMethodTypes126))
		{
			this.swigDelegate126 = new GLAdapter.SwigDelegateGLAdapter_126(this.SwigDirectorProcess_glUniform3i);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform3iv", GLAdapter.swigMethodTypes127))
		{
			this.swigDelegate127 = new GLAdapter.SwigDelegateGLAdapter_127(this.SwigDirectorProcess_glUniform3iv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform4f", GLAdapter.swigMethodTypes128))
		{
			this.swigDelegate128 = new GLAdapter.SwigDelegateGLAdapter_128(this.SwigDirectorProcess_glUniform4f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform4fv", GLAdapter.swigMethodTypes129))
		{
			this.swigDelegate129 = new GLAdapter.SwigDelegateGLAdapter_129(this.SwigDirectorProcess_glUniform4fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform4i", GLAdapter.swigMethodTypes130))
		{
			this.swigDelegate130 = new GLAdapter.SwigDelegateGLAdapter_130(this.SwigDirectorProcess_glUniform4i);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform4iv", GLAdapter.swigMethodTypes131))
		{
			this.swigDelegate131 = new GLAdapter.SwigDelegateGLAdapter_131(this.SwigDirectorProcess_glUniform4iv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformMatrix2fv", GLAdapter.swigMethodTypes132))
		{
			this.swigDelegate132 = new GLAdapter.SwigDelegateGLAdapter_132(this.SwigDirectorProcess_glUniformMatrix2fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformMatrix3fv", GLAdapter.swigMethodTypes133))
		{
			this.swigDelegate133 = new GLAdapter.SwigDelegateGLAdapter_133(this.SwigDirectorProcess_glUniformMatrix3fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformMatrix4fv", GLAdapter.swigMethodTypes134))
		{
			this.swigDelegate134 = new GLAdapter.SwigDelegateGLAdapter_134(this.SwigDirectorProcess_glUniformMatrix4fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUseProgram", GLAdapter.swigMethodTypes135))
		{
			this.swigDelegate135 = new GLAdapter.SwigDelegateGLAdapter_135(this.SwigDirectorProcess_glUseProgram);
		}
		if (this.SwigDerivedClassHasMethod("Process_glValidateProgram", GLAdapter.swigMethodTypes136))
		{
			this.swigDelegate136 = new GLAdapter.SwigDelegateGLAdapter_136(this.SwigDirectorProcess_glValidateProgram);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttrib1f", GLAdapter.swigMethodTypes137))
		{
			this.swigDelegate137 = new GLAdapter.SwigDelegateGLAdapter_137(this.SwigDirectorProcess_glVertexAttrib1f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttrib1fv", GLAdapter.swigMethodTypes138))
		{
			this.swigDelegate138 = new GLAdapter.SwigDelegateGLAdapter_138(this.SwigDirectorProcess_glVertexAttrib1fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttrib2f", GLAdapter.swigMethodTypes139))
		{
			this.swigDelegate139 = new GLAdapter.SwigDelegateGLAdapter_139(this.SwigDirectorProcess_glVertexAttrib2f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttrib2fv", GLAdapter.swigMethodTypes140))
		{
			this.swigDelegate140 = new GLAdapter.SwigDelegateGLAdapter_140(this.SwigDirectorProcess_glVertexAttrib2fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttrib3f", GLAdapter.swigMethodTypes141))
		{
			this.swigDelegate141 = new GLAdapter.SwigDelegateGLAdapter_141(this.SwigDirectorProcess_glVertexAttrib3f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttrib3fv", GLAdapter.swigMethodTypes142))
		{
			this.swigDelegate142 = new GLAdapter.SwigDelegateGLAdapter_142(this.SwigDirectorProcess_glVertexAttrib3fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttrib4f", GLAdapter.swigMethodTypes143))
		{
			this.swigDelegate143 = new GLAdapter.SwigDelegateGLAdapter_143(this.SwigDirectorProcess_glVertexAttrib4f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttrib4fv", GLAdapter.swigMethodTypes144))
		{
			this.swigDelegate144 = new GLAdapter.SwigDelegateGLAdapter_144(this.SwigDirectorProcess_glVertexAttrib4fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribPointer", GLAdapter.swigMethodTypes145))
		{
			this.swigDelegate145 = new GLAdapter.SwigDelegateGLAdapter_145(this.SwigDirectorProcess_glVertexAttribPointer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glViewport", GLAdapter.swigMethodTypes146))
		{
			this.swigDelegate146 = new GLAdapter.SwigDelegateGLAdapter_146(this.SwigDirectorProcess_glViewport);
		}
		if (this.SwigDerivedClassHasMethod("Process_glReadBuffer", GLAdapter.swigMethodTypes147))
		{
			this.swigDelegate147 = new GLAdapter.SwigDelegateGLAdapter_147(this.SwigDirectorProcess_glReadBuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawRangeElements", GLAdapter.swigMethodTypes148))
		{
			this.swigDelegate148 = new GLAdapter.SwigDelegateGLAdapter_148(this.SwigDirectorProcess_glDrawRangeElements);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexImage3D", GLAdapter.swigMethodTypes149))
		{
			this.swigDelegate149 = new GLAdapter.SwigDelegateGLAdapter_149(this.SwigDirectorProcess_glTexImage3D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexSubImage3D", GLAdapter.swigMethodTypes150))
		{
			this.swigDelegate150 = new GLAdapter.SwigDelegateGLAdapter_150(this.SwigDirectorProcess_glTexSubImage3D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCopyTexSubImage3D", GLAdapter.swigMethodTypes151))
		{
			this.swigDelegate151 = new GLAdapter.SwigDelegateGLAdapter_151(this.SwigDirectorProcess_glCopyTexSubImage3D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCompressedTexImage3D", GLAdapter.swigMethodTypes152))
		{
			this.swigDelegate152 = new GLAdapter.SwigDelegateGLAdapter_152(this.SwigDirectorProcess_glCompressedTexImage3D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCompressedTexSubImage3D", GLAdapter.swigMethodTypes153))
		{
			this.swigDelegate153 = new GLAdapter.SwigDelegateGLAdapter_153(this.SwigDirectorProcess_glCompressedTexSubImage3D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenQueries", GLAdapter.swigMethodTypes154))
		{
			this.swigDelegate154 = new GLAdapter.SwigDelegateGLAdapter_154(this.SwigDirectorProcess_glGenQueries);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteQueries", GLAdapter.swigMethodTypes155))
		{
			this.swigDelegate155 = new GLAdapter.SwigDelegateGLAdapter_155(this.SwigDirectorProcess_glDeleteQueries);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsQuery", GLAdapter.swigMethodTypes156))
		{
			this.swigDelegate156 = new GLAdapter.SwigDelegateGLAdapter_156(this.SwigDirectorProcess_glIsQuery);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBeginQuery", GLAdapter.swigMethodTypes157))
		{
			this.swigDelegate157 = new GLAdapter.SwigDelegateGLAdapter_157(this.SwigDirectorProcess_glBeginQuery);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEndQuery", GLAdapter.swigMethodTypes158))
		{
			this.swigDelegate158 = new GLAdapter.SwigDelegateGLAdapter_158(this.SwigDirectorProcess_glEndQuery);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetQueryiv", GLAdapter.swigMethodTypes159))
		{
			this.swigDelegate159 = new GLAdapter.SwigDelegateGLAdapter_159(this.SwigDirectorProcess_glGetQueryiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetQueryObjectuiv", GLAdapter.swigMethodTypes160))
		{
			this.swigDelegate160 = new GLAdapter.SwigDelegateGLAdapter_160(this.SwigDirectorProcess_glGetQueryObjectuiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUnmapBuffer", GLAdapter.swigMethodTypes161))
		{
			this.swigDelegate161 = new GLAdapter.SwigDelegateGLAdapter_161(this.SwigDirectorProcess_glUnmapBuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetBufferPointerv", GLAdapter.swigMethodTypes162))
		{
			this.swigDelegate162 = new GLAdapter.SwigDelegateGLAdapter_162(this.SwigDirectorProcess_glGetBufferPointerv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawBuffers", GLAdapter.swigMethodTypes163))
		{
			this.swigDelegate163 = new GLAdapter.SwigDelegateGLAdapter_163(this.SwigDirectorProcess_glDrawBuffers);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformMatrix2x3fv", GLAdapter.swigMethodTypes164))
		{
			this.swigDelegate164 = new GLAdapter.SwigDelegateGLAdapter_164(this.SwigDirectorProcess_glUniformMatrix2x3fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformMatrix3x2fv", GLAdapter.swigMethodTypes165))
		{
			this.swigDelegate165 = new GLAdapter.SwigDelegateGLAdapter_165(this.SwigDirectorProcess_glUniformMatrix3x2fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformMatrix2x4fv", GLAdapter.swigMethodTypes166))
		{
			this.swigDelegate166 = new GLAdapter.SwigDelegateGLAdapter_166(this.SwigDirectorProcess_glUniformMatrix2x4fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformMatrix4x2fv", GLAdapter.swigMethodTypes167))
		{
			this.swigDelegate167 = new GLAdapter.SwigDelegateGLAdapter_167(this.SwigDirectorProcess_glUniformMatrix4x2fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformMatrix3x4fv", GLAdapter.swigMethodTypes168))
		{
			this.swigDelegate168 = new GLAdapter.SwigDelegateGLAdapter_168(this.SwigDirectorProcess_glUniformMatrix3x4fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformMatrix4x3fv", GLAdapter.swigMethodTypes169))
		{
			this.swigDelegate169 = new GLAdapter.SwigDelegateGLAdapter_169(this.SwigDirectorProcess_glUniformMatrix4x3fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlitFramebuffer", GLAdapter.swigMethodTypes170))
		{
			this.swigDelegate170 = new GLAdapter.SwigDelegateGLAdapter_170(this.SwigDirectorProcess_glBlitFramebuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glRenderbufferStorageMultisample", GLAdapter.swigMethodTypes171))
		{
			this.swigDelegate171 = new GLAdapter.SwigDelegateGLAdapter_171(this.SwigDirectorProcess_glRenderbufferStorageMultisample);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferTextureLayer", GLAdapter.swigMethodTypes172))
		{
			this.swigDelegate172 = new GLAdapter.SwigDelegateGLAdapter_172(this.SwigDirectorProcess_glFramebufferTextureLayer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glMapBufferRange", GLAdapter.swigMethodTypes173))
		{
			this.swigDelegate173 = new GLAdapter.SwigDelegateGLAdapter_173(this.SwigDirectorProcess_glMapBufferRange);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFlushMappedBufferRange", GLAdapter.swigMethodTypes174))
		{
			this.swigDelegate174 = new GLAdapter.SwigDelegateGLAdapter_174(this.SwigDirectorProcess_glFlushMappedBufferRange);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindVertexArray", GLAdapter.swigMethodTypes175))
		{
			this.swigDelegate175 = new GLAdapter.SwigDelegateGLAdapter_175(this.SwigDirectorProcess_glBindVertexArray);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteVertexArrays", GLAdapter.swigMethodTypes176))
		{
			this.swigDelegate176 = new GLAdapter.SwigDelegateGLAdapter_176(this.SwigDirectorProcess_glDeleteVertexArrays);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenVertexArrays", GLAdapter.swigMethodTypes177))
		{
			this.swigDelegate177 = new GLAdapter.SwigDelegateGLAdapter_177(this.SwigDirectorProcess_glGenVertexArrays);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsVertexArray", GLAdapter.swigMethodTypes178))
		{
			this.swigDelegate178 = new GLAdapter.SwigDelegateGLAdapter_178(this.SwigDirectorProcess_glIsVertexArray);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetIntegeri_v", GLAdapter.swigMethodTypes179))
		{
			this.swigDelegate179 = new GLAdapter.SwigDelegateGLAdapter_179(this.SwigDirectorProcess_glGetIntegeri_v);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetBooleani_v", GLAdapter.swigMethodTypes180))
		{
			this.swigDelegate180 = new GLAdapter.SwigDelegateGLAdapter_180(this.SwigDirectorProcess_glGetBooleani_v);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBeginTransformFeedback", GLAdapter.swigMethodTypes181))
		{
			this.swigDelegate181 = new GLAdapter.SwigDelegateGLAdapter_181(this.SwigDirectorProcess_glBeginTransformFeedback);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEndTransformFeedback", GLAdapter.swigMethodTypes182))
		{
			this.swigDelegate182 = new GLAdapter.SwigDelegateGLAdapter_182(this.SwigDirectorProcess_glEndTransformFeedback);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindBufferRange", GLAdapter.swigMethodTypes183))
		{
			this.swigDelegate183 = new GLAdapter.SwigDelegateGLAdapter_183(this.SwigDirectorProcess_glBindBufferRange);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindBufferBase", GLAdapter.swigMethodTypes184))
		{
			this.swigDelegate184 = new GLAdapter.SwigDelegateGLAdapter_184(this.SwigDirectorProcess_glBindBufferBase);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTransformFeedbackVaryings", GLAdapter.swigMethodTypes185))
		{
			this.swigDelegate185 = new GLAdapter.SwigDelegateGLAdapter_185(this.SwigDirectorProcess_glTransformFeedbackVaryings);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetTransformFeedbackVarying", GLAdapter.swigMethodTypes186))
		{
			this.swigDelegate186 = new GLAdapter.SwigDelegateGLAdapter_186(this.SwigDirectorProcess_glGetTransformFeedbackVarying);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribIPointer", GLAdapter.swigMethodTypes187))
		{
			this.swigDelegate187 = new GLAdapter.SwigDelegateGLAdapter_187(this.SwigDirectorProcess_glVertexAttribIPointer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetVertexAttribIiv", GLAdapter.swigMethodTypes188))
		{
			this.swigDelegate188 = new GLAdapter.SwigDelegateGLAdapter_188(this.SwigDirectorProcess_glGetVertexAttribIiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetVertexAttribIuiv", GLAdapter.swigMethodTypes189))
		{
			this.swigDelegate189 = new GLAdapter.SwigDelegateGLAdapter_189(this.SwigDirectorProcess_glGetVertexAttribIuiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribI4i", GLAdapter.swigMethodTypes190))
		{
			this.swigDelegate190 = new GLAdapter.SwigDelegateGLAdapter_190(this.SwigDirectorProcess_glVertexAttribI4i);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribI4ui", GLAdapter.swigMethodTypes191))
		{
			this.swigDelegate191 = new GLAdapter.SwigDelegateGLAdapter_191(this.SwigDirectorProcess_glVertexAttribI4ui);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribI4iv", GLAdapter.swigMethodTypes192))
		{
			this.swigDelegate192 = new GLAdapter.SwigDelegateGLAdapter_192(this.SwigDirectorProcess_glVertexAttribI4iv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribI4uiv", GLAdapter.swigMethodTypes193))
		{
			this.swigDelegate193 = new GLAdapter.SwigDelegateGLAdapter_193(this.SwigDirectorProcess_glVertexAttribI4uiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetUniformuiv", GLAdapter.swigMethodTypes194))
		{
			this.swigDelegate194 = new GLAdapter.SwigDelegateGLAdapter_194(this.SwigDirectorProcess_glGetUniformuiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetFragDataLocation", GLAdapter.swigMethodTypes195))
		{
			this.swigDelegate195 = new GLAdapter.SwigDelegateGLAdapter_195(this.SwigDirectorProcess_glGetFragDataLocation);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform1ui", GLAdapter.swigMethodTypes196))
		{
			this.swigDelegate196 = new GLAdapter.SwigDelegateGLAdapter_196(this.SwigDirectorProcess_glUniform1ui);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform2ui", GLAdapter.swigMethodTypes197))
		{
			this.swigDelegate197 = new GLAdapter.SwigDelegateGLAdapter_197(this.SwigDirectorProcess_glUniform2ui);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform3ui", GLAdapter.swigMethodTypes198))
		{
			this.swigDelegate198 = new GLAdapter.SwigDelegateGLAdapter_198(this.SwigDirectorProcess_glUniform3ui);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform4ui", GLAdapter.swigMethodTypes199))
		{
			this.swigDelegate199 = new GLAdapter.SwigDelegateGLAdapter_199(this.SwigDirectorProcess_glUniform4ui);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform1uiv", GLAdapter.swigMethodTypes200))
		{
			this.swigDelegate200 = new GLAdapter.SwigDelegateGLAdapter_200(this.SwigDirectorProcess_glUniform1uiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform2uiv", GLAdapter.swigMethodTypes201))
		{
			this.swigDelegate201 = new GLAdapter.SwigDelegateGLAdapter_201(this.SwigDirectorProcess_glUniform2uiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform3uiv", GLAdapter.swigMethodTypes202))
		{
			this.swigDelegate202 = new GLAdapter.SwigDelegateGLAdapter_202(this.SwigDirectorProcess_glUniform3uiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniform4uiv", GLAdapter.swigMethodTypes203))
		{
			this.swigDelegate203 = new GLAdapter.SwigDelegateGLAdapter_203(this.SwigDirectorProcess_glUniform4uiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClearBufferiv", GLAdapter.swigMethodTypes204))
		{
			this.swigDelegate204 = new GLAdapter.SwigDelegateGLAdapter_204(this.SwigDirectorProcess_glClearBufferiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClearBufferuiv", GLAdapter.swigMethodTypes205))
		{
			this.swigDelegate205 = new GLAdapter.SwigDelegateGLAdapter_205(this.SwigDirectorProcess_glClearBufferuiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClearBufferfv", GLAdapter.swigMethodTypes206))
		{
			this.swigDelegate206 = new GLAdapter.SwigDelegateGLAdapter_206(this.SwigDirectorProcess_glClearBufferfv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClearBufferfi", GLAdapter.swigMethodTypes207))
		{
			this.swigDelegate207 = new GLAdapter.SwigDelegateGLAdapter_207(this.SwigDirectorProcess_glClearBufferfi);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetStringi", GLAdapter.swigMethodTypes208))
		{
			this.swigDelegate208 = new GLAdapter.SwigDelegateGLAdapter_208(this.SwigDirectorProcess_glGetStringi);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCopyBufferSubData", GLAdapter.swigMethodTypes209))
		{
			this.swigDelegate209 = new GLAdapter.SwigDelegateGLAdapter_209(this.SwigDirectorProcess_glCopyBufferSubData);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetUniformIndices", GLAdapter.swigMethodTypes210))
		{
			this.swigDelegate210 = new GLAdapter.SwigDelegateGLAdapter_210(this.SwigDirectorProcess_glGetUniformIndices);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetActiveUniformsiv", GLAdapter.swigMethodTypes211))
		{
			this.swigDelegate211 = new GLAdapter.SwigDelegateGLAdapter_211(this.SwigDirectorProcess_glGetActiveUniformsiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetUniformBlockIndex", GLAdapter.swigMethodTypes212))
		{
			this.swigDelegate212 = new GLAdapter.SwigDelegateGLAdapter_212(this.SwigDirectorProcess_glGetUniformBlockIndex);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetActiveUniformBlockiv", GLAdapter.swigMethodTypes213))
		{
			this.swigDelegate213 = new GLAdapter.SwigDelegateGLAdapter_213(this.SwigDirectorProcess_glGetActiveUniformBlockiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetActiveUniformBlockName", GLAdapter.swigMethodTypes214))
		{
			this.swigDelegate214 = new GLAdapter.SwigDelegateGLAdapter_214(this.SwigDirectorProcess_glGetActiveUniformBlockName);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUniformBlockBinding", GLAdapter.swigMethodTypes215))
		{
			this.swigDelegate215 = new GLAdapter.SwigDelegateGLAdapter_215(this.SwigDirectorProcess_glUniformBlockBinding);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawArraysInstanced", GLAdapter.swigMethodTypes216))
		{
			this.swigDelegate216 = new GLAdapter.SwigDelegateGLAdapter_216(this.SwigDirectorProcess_glDrawArraysInstanced);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawElementsInstanced", GLAdapter.swigMethodTypes217))
		{
			this.swigDelegate217 = new GLAdapter.SwigDelegateGLAdapter_217(this.SwigDirectorProcess_glDrawElementsInstanced);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFenceSync", GLAdapter.swigMethodTypes218))
		{
			this.swigDelegate218 = new GLAdapter.SwigDelegateGLAdapter_218(this.SwigDirectorProcess_glFenceSync);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsSync", GLAdapter.swigMethodTypes219))
		{
			this.swigDelegate219 = new GLAdapter.SwigDelegateGLAdapter_219(this.SwigDirectorProcess_glIsSync);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteSync", GLAdapter.swigMethodTypes220))
		{
			this.swigDelegate220 = new GLAdapter.SwigDelegateGLAdapter_220(this.SwigDirectorProcess_glDeleteSync);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClientWaitSync", GLAdapter.swigMethodTypes221))
		{
			this.swigDelegate221 = new GLAdapter.SwigDelegateGLAdapter_221(this.SwigDirectorProcess_glClientWaitSync);
		}
		if (this.SwigDerivedClassHasMethod("Process_glWaitSync", GLAdapter.swigMethodTypes222))
		{
			this.swigDelegate222 = new GLAdapter.SwigDelegateGLAdapter_222(this.SwigDirectorProcess_glWaitSync);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetInteger64v", GLAdapter.swigMethodTypes223))
		{
			this.swigDelegate223 = new GLAdapter.SwigDelegateGLAdapter_223(this.SwigDirectorProcess_glGetInteger64v);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetSynciv", GLAdapter.swigMethodTypes224))
		{
			this.swigDelegate224 = new GLAdapter.SwigDelegateGLAdapter_224(this.SwigDirectorProcess_glGetSynciv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetInteger64i_v", GLAdapter.swigMethodTypes225))
		{
			this.swigDelegate225 = new GLAdapter.SwigDelegateGLAdapter_225(this.SwigDirectorProcess_glGetInteger64i_v);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetBufferParameteri64v", GLAdapter.swigMethodTypes226))
		{
			this.swigDelegate226 = new GLAdapter.SwigDelegateGLAdapter_226(this.SwigDirectorProcess_glGetBufferParameteri64v);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenSamplers", GLAdapter.swigMethodTypes227))
		{
			this.swigDelegate227 = new GLAdapter.SwigDelegateGLAdapter_227(this.SwigDirectorProcess_glGenSamplers);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteSamplers", GLAdapter.swigMethodTypes228))
		{
			this.swigDelegate228 = new GLAdapter.SwigDelegateGLAdapter_228(this.SwigDirectorProcess_glDeleteSamplers);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsSampler", GLAdapter.swigMethodTypes229))
		{
			this.swigDelegate229 = new GLAdapter.SwigDelegateGLAdapter_229(this.SwigDirectorProcess_glIsSampler);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindSampler", GLAdapter.swigMethodTypes230))
		{
			this.swigDelegate230 = new GLAdapter.SwigDelegateGLAdapter_230(this.SwigDirectorProcess_glBindSampler);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSamplerParameteri", GLAdapter.swigMethodTypes231))
		{
			this.swigDelegate231 = new GLAdapter.SwigDelegateGLAdapter_231(this.SwigDirectorProcess_glSamplerParameteri);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSamplerParameteriv", GLAdapter.swigMethodTypes232))
		{
			this.swigDelegate232 = new GLAdapter.SwigDelegateGLAdapter_232(this.SwigDirectorProcess_glSamplerParameteriv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSamplerParameterf", GLAdapter.swigMethodTypes233))
		{
			this.swigDelegate233 = new GLAdapter.SwigDelegateGLAdapter_233(this.SwigDirectorProcess_glSamplerParameterf);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSamplerParameterfv", GLAdapter.swigMethodTypes234))
		{
			this.swigDelegate234 = new GLAdapter.SwigDelegateGLAdapter_234(this.SwigDirectorProcess_glSamplerParameterfv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetSamplerParameteriv", GLAdapter.swigMethodTypes235))
		{
			this.swigDelegate235 = new GLAdapter.SwigDelegateGLAdapter_235(this.SwigDirectorProcess_glGetSamplerParameteriv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetSamplerParameterfv", GLAdapter.swigMethodTypes236))
		{
			this.swigDelegate236 = new GLAdapter.SwigDelegateGLAdapter_236(this.SwigDirectorProcess_glGetSamplerParameterfv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribDivisor", GLAdapter.swigMethodTypes237))
		{
			this.swigDelegate237 = new GLAdapter.SwigDelegateGLAdapter_237(this.SwigDirectorProcess_glVertexAttribDivisor);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindTransformFeedback", GLAdapter.swigMethodTypes238))
		{
			this.swigDelegate238 = new GLAdapter.SwigDelegateGLAdapter_238(this.SwigDirectorProcess_glBindTransformFeedback);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteTransformFeedbacks", GLAdapter.swigMethodTypes239))
		{
			this.swigDelegate239 = new GLAdapter.SwigDelegateGLAdapter_239(this.SwigDirectorProcess_glDeleteTransformFeedbacks);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenTransformFeedbacks", GLAdapter.swigMethodTypes240))
		{
			this.swigDelegate240 = new GLAdapter.SwigDelegateGLAdapter_240(this.SwigDirectorProcess_glGenTransformFeedbacks);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsTransformFeedback", GLAdapter.swigMethodTypes241))
		{
			this.swigDelegate241 = new GLAdapter.SwigDelegateGLAdapter_241(this.SwigDirectorProcess_glIsTransformFeedback);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPauseTransformFeedback", GLAdapter.swigMethodTypes242))
		{
			this.swigDelegate242 = new GLAdapter.SwigDelegateGLAdapter_242(this.SwigDirectorProcess_glPauseTransformFeedback);
		}
		if (this.SwigDerivedClassHasMethod("Process_glResumeTransformFeedback", GLAdapter.swigMethodTypes243))
		{
			this.swigDelegate243 = new GLAdapter.SwigDelegateGLAdapter_243(this.SwigDirectorProcess_glResumeTransformFeedback);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramBinary", GLAdapter.swigMethodTypes244))
		{
			this.swigDelegate244 = new GLAdapter.SwigDelegateGLAdapter_244(this.SwigDirectorProcess_glGetProgramBinary);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramBinary", GLAdapter.swigMethodTypes245))
		{
			this.swigDelegate245 = new GLAdapter.SwigDelegateGLAdapter_245(this.SwigDirectorProcess_glProgramBinary);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramParameteri", GLAdapter.swigMethodTypes246))
		{
			this.swigDelegate246 = new GLAdapter.SwigDelegateGLAdapter_246(this.SwigDirectorProcess_glProgramParameteri);
		}
		if (this.SwigDerivedClassHasMethod("Process_glInvalidateFramebuffer", GLAdapter.swigMethodTypes247))
		{
			this.swigDelegate247 = new GLAdapter.SwigDelegateGLAdapter_247(this.SwigDirectorProcess_glInvalidateFramebuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glInvalidateSubFramebuffer", GLAdapter.swigMethodTypes248))
		{
			this.swigDelegate248 = new GLAdapter.SwigDelegateGLAdapter_248(this.SwigDirectorProcess_glInvalidateSubFramebuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexStorage2D", GLAdapter.swigMethodTypes249))
		{
			this.swigDelegate249 = new GLAdapter.SwigDelegateGLAdapter_249(this.SwigDirectorProcess_glTexStorage2D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexStorage3D", GLAdapter.swigMethodTypes250))
		{
			this.swigDelegate250 = new GLAdapter.SwigDelegateGLAdapter_250(this.SwigDirectorProcess_glTexStorage3D);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetInternalformativ", GLAdapter.swigMethodTypes251))
		{
			this.swigDelegate251 = new GLAdapter.SwigDelegateGLAdapter_251(this.SwigDirectorProcess_glGetInternalformativ);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDispatchCompute", GLAdapter.swigMethodTypes252))
		{
			this.swigDelegate252 = new GLAdapter.SwigDelegateGLAdapter_252(this.SwigDirectorProcess_glDispatchCompute);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDispatchComputeIndirect", GLAdapter.swigMethodTypes253))
		{
			this.swigDelegate253 = new GLAdapter.SwigDelegateGLAdapter_253(this.SwigDirectorProcess_glDispatchComputeIndirect);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawArraysIndirect", GLAdapter.swigMethodTypes254))
		{
			this.swigDelegate254 = new GLAdapter.SwigDelegateGLAdapter_254(this.SwigDirectorProcess_glDrawArraysIndirect);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawElementsIndirect", GLAdapter.swigMethodTypes255))
		{
			this.swigDelegate255 = new GLAdapter.SwigDelegateGLAdapter_255(this.SwigDirectorProcess_glDrawElementsIndirect);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferParameteri", GLAdapter.swigMethodTypes256))
		{
			this.swigDelegate256 = new GLAdapter.SwigDelegateGLAdapter_256(this.SwigDirectorProcess_glFramebufferParameteri);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetFramebufferParameteriv", GLAdapter.swigMethodTypes257))
		{
			this.swigDelegate257 = new GLAdapter.SwigDelegateGLAdapter_257(this.SwigDirectorProcess_glGetFramebufferParameteriv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramInterfaceiv", GLAdapter.swigMethodTypes258))
		{
			this.swigDelegate258 = new GLAdapter.SwigDelegateGLAdapter_258(this.SwigDirectorProcess_glGetProgramInterfaceiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramResourceIndex", GLAdapter.swigMethodTypes259))
		{
			this.swigDelegate259 = new GLAdapter.SwigDelegateGLAdapter_259(this.SwigDirectorProcess_glGetProgramResourceIndex);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramResourceName", GLAdapter.swigMethodTypes260))
		{
			this.swigDelegate260 = new GLAdapter.SwigDelegateGLAdapter_260(this.SwigDirectorProcess_glGetProgramResourceName);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramResourceiv", GLAdapter.swigMethodTypes261))
		{
			this.swigDelegate261 = new GLAdapter.SwigDelegateGLAdapter_261(this.SwigDirectorProcess_glGetProgramResourceiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramResourceLocation", GLAdapter.swigMethodTypes262))
		{
			this.swigDelegate262 = new GLAdapter.SwigDelegateGLAdapter_262(this.SwigDirectorProcess_glGetProgramResourceLocation);
		}
		if (this.SwigDerivedClassHasMethod("Process_glUseProgramStages", GLAdapter.swigMethodTypes263))
		{
			this.swigDelegate263 = new GLAdapter.SwigDelegateGLAdapter_263(this.SwigDirectorProcess_glUseProgramStages);
		}
		if (this.SwigDerivedClassHasMethod("Process_glActiveShaderProgram", GLAdapter.swigMethodTypes264))
		{
			this.swigDelegate264 = new GLAdapter.SwigDelegateGLAdapter_264(this.SwigDirectorProcess_glActiveShaderProgram);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCreateShaderProgramv", GLAdapter.swigMethodTypes265))
		{
			this.swigDelegate265 = new GLAdapter.SwigDelegateGLAdapter_265(this.SwigDirectorProcess_glCreateShaderProgramv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindProgramPipeline", GLAdapter.swigMethodTypes266))
		{
			this.swigDelegate266 = new GLAdapter.SwigDelegateGLAdapter_266(this.SwigDirectorProcess_glBindProgramPipeline);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteProgramPipelines", GLAdapter.swigMethodTypes267))
		{
			this.swigDelegate267 = new GLAdapter.SwigDelegateGLAdapter_267(this.SwigDirectorProcess_glDeleteProgramPipelines);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenProgramPipelines", GLAdapter.swigMethodTypes268))
		{
			this.swigDelegate268 = new GLAdapter.SwigDelegateGLAdapter_268(this.SwigDirectorProcess_glGenProgramPipelines);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsProgramPipeline", GLAdapter.swigMethodTypes269))
		{
			this.swigDelegate269 = new GLAdapter.SwigDelegateGLAdapter_269(this.SwigDirectorProcess_glIsProgramPipeline);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramPipelineiv", GLAdapter.swigMethodTypes270))
		{
			this.swigDelegate270 = new GLAdapter.SwigDelegateGLAdapter_270(this.SwigDirectorProcess_glGetProgramPipelineiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform1i", GLAdapter.swigMethodTypes271))
		{
			this.swigDelegate271 = new GLAdapter.SwigDelegateGLAdapter_271(this.SwigDirectorProcess_glProgramUniform1i);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform2i", GLAdapter.swigMethodTypes272))
		{
			this.swigDelegate272 = new GLAdapter.SwigDelegateGLAdapter_272(this.SwigDirectorProcess_glProgramUniform2i);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform3i", GLAdapter.swigMethodTypes273))
		{
			this.swigDelegate273 = new GLAdapter.SwigDelegateGLAdapter_273(this.SwigDirectorProcess_glProgramUniform3i);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform4i", GLAdapter.swigMethodTypes274))
		{
			this.swigDelegate274 = new GLAdapter.SwigDelegateGLAdapter_274(this.SwigDirectorProcess_glProgramUniform4i);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform1ui", GLAdapter.swigMethodTypes275))
		{
			this.swigDelegate275 = new GLAdapter.SwigDelegateGLAdapter_275(this.SwigDirectorProcess_glProgramUniform1ui);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform2ui", GLAdapter.swigMethodTypes276))
		{
			this.swigDelegate276 = new GLAdapter.SwigDelegateGLAdapter_276(this.SwigDirectorProcess_glProgramUniform2ui);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform3ui", GLAdapter.swigMethodTypes277))
		{
			this.swigDelegate277 = new GLAdapter.SwigDelegateGLAdapter_277(this.SwigDirectorProcess_glProgramUniform3ui);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform4ui", GLAdapter.swigMethodTypes278))
		{
			this.swigDelegate278 = new GLAdapter.SwigDelegateGLAdapter_278(this.SwigDirectorProcess_glProgramUniform4ui);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform1f", GLAdapter.swigMethodTypes279))
		{
			this.swigDelegate279 = new GLAdapter.SwigDelegateGLAdapter_279(this.SwigDirectorProcess_glProgramUniform1f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform2f", GLAdapter.swigMethodTypes280))
		{
			this.swigDelegate280 = new GLAdapter.SwigDelegateGLAdapter_280(this.SwigDirectorProcess_glProgramUniform2f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform3f", GLAdapter.swigMethodTypes281))
		{
			this.swigDelegate281 = new GLAdapter.SwigDelegateGLAdapter_281(this.SwigDirectorProcess_glProgramUniform3f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform4f", GLAdapter.swigMethodTypes282))
		{
			this.swigDelegate282 = new GLAdapter.SwigDelegateGLAdapter_282(this.SwigDirectorProcess_glProgramUniform4f);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform1iv", GLAdapter.swigMethodTypes283))
		{
			this.swigDelegate283 = new GLAdapter.SwigDelegateGLAdapter_283(this.SwigDirectorProcess_glProgramUniform1iv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform2iv", GLAdapter.swigMethodTypes284))
		{
			this.swigDelegate284 = new GLAdapter.SwigDelegateGLAdapter_284(this.SwigDirectorProcess_glProgramUniform2iv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform3iv", GLAdapter.swigMethodTypes285))
		{
			this.swigDelegate285 = new GLAdapter.SwigDelegateGLAdapter_285(this.SwigDirectorProcess_glProgramUniform3iv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform4iv", GLAdapter.swigMethodTypes286))
		{
			this.swigDelegate286 = new GLAdapter.SwigDelegateGLAdapter_286(this.SwigDirectorProcess_glProgramUniform4iv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform1uiv", GLAdapter.swigMethodTypes287))
		{
			this.swigDelegate287 = new GLAdapter.SwigDelegateGLAdapter_287(this.SwigDirectorProcess_glProgramUniform1uiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform2uiv", GLAdapter.swigMethodTypes288))
		{
			this.swigDelegate288 = new GLAdapter.SwigDelegateGLAdapter_288(this.SwigDirectorProcess_glProgramUniform2uiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform3uiv", GLAdapter.swigMethodTypes289))
		{
			this.swigDelegate289 = new GLAdapter.SwigDelegateGLAdapter_289(this.SwigDirectorProcess_glProgramUniform3uiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform4uiv", GLAdapter.swigMethodTypes290))
		{
			this.swigDelegate290 = new GLAdapter.SwigDelegateGLAdapter_290(this.SwigDirectorProcess_glProgramUniform4uiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform1fv", GLAdapter.swigMethodTypes291))
		{
			this.swigDelegate291 = new GLAdapter.SwigDelegateGLAdapter_291(this.SwigDirectorProcess_glProgramUniform1fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform2fv", GLAdapter.swigMethodTypes292))
		{
			this.swigDelegate292 = new GLAdapter.SwigDelegateGLAdapter_292(this.SwigDirectorProcess_glProgramUniform2fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform3fv", GLAdapter.swigMethodTypes293))
		{
			this.swigDelegate293 = new GLAdapter.SwigDelegateGLAdapter_293(this.SwigDirectorProcess_glProgramUniform3fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniform4fv", GLAdapter.swigMethodTypes294))
		{
			this.swigDelegate294 = new GLAdapter.SwigDelegateGLAdapter_294(this.SwigDirectorProcess_glProgramUniform4fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniformMatrix2fv", GLAdapter.swigMethodTypes295))
		{
			this.swigDelegate295 = new GLAdapter.SwigDelegateGLAdapter_295(this.SwigDirectorProcess_glProgramUniformMatrix2fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniformMatrix3fv", GLAdapter.swigMethodTypes296))
		{
			this.swigDelegate296 = new GLAdapter.SwigDelegateGLAdapter_296(this.SwigDirectorProcess_glProgramUniformMatrix3fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniformMatrix4fv", GLAdapter.swigMethodTypes297))
		{
			this.swigDelegate297 = new GLAdapter.SwigDelegateGLAdapter_297(this.SwigDirectorProcess_glProgramUniformMatrix4fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniformMatrix2x3fv", GLAdapter.swigMethodTypes298))
		{
			this.swigDelegate298 = new GLAdapter.SwigDelegateGLAdapter_298(this.SwigDirectorProcess_glProgramUniformMatrix2x3fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniformMatrix3x2fv", GLAdapter.swigMethodTypes299))
		{
			this.swigDelegate299 = new GLAdapter.SwigDelegateGLAdapter_299(this.SwigDirectorProcess_glProgramUniformMatrix3x2fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniformMatrix2x4fv", GLAdapter.swigMethodTypes300))
		{
			this.swigDelegate300 = new GLAdapter.SwigDelegateGLAdapter_300(this.SwigDirectorProcess_glProgramUniformMatrix2x4fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniformMatrix4x2fv", GLAdapter.swigMethodTypes301))
		{
			this.swigDelegate301 = new GLAdapter.SwigDelegateGLAdapter_301(this.SwigDirectorProcess_glProgramUniformMatrix4x2fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniformMatrix3x4fv", GLAdapter.swigMethodTypes302))
		{
			this.swigDelegate302 = new GLAdapter.SwigDelegateGLAdapter_302(this.SwigDirectorProcess_glProgramUniformMatrix3x4fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramUniformMatrix4x3fv", GLAdapter.swigMethodTypes303))
		{
			this.swigDelegate303 = new GLAdapter.SwigDelegateGLAdapter_303(this.SwigDirectorProcess_glProgramUniformMatrix4x3fv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glValidateProgramPipeline", GLAdapter.swigMethodTypes304))
		{
			this.swigDelegate304 = new GLAdapter.SwigDelegateGLAdapter_304(this.SwigDirectorProcess_glValidateProgramPipeline);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramPipelineInfoLog", GLAdapter.swigMethodTypes305))
		{
			this.swigDelegate305 = new GLAdapter.SwigDelegateGLAdapter_305(this.SwigDirectorProcess_glGetProgramPipelineInfoLog);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetActiveAtomicCounterBufferiv", GLAdapter.swigMethodTypes306))
		{
			this.swigDelegate306 = new GLAdapter.SwigDelegateGLAdapter_306(this.SwigDirectorProcess_glGetActiveAtomicCounterBufferiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindImageTexture", GLAdapter.swigMethodTypes307))
		{
			this.swigDelegate307 = new GLAdapter.SwigDelegateGLAdapter_307(this.SwigDirectorProcess_glBindImageTexture);
		}
		if (this.SwigDerivedClassHasMethod("Process_glMemoryBarrier", GLAdapter.swigMethodTypes308))
		{
			this.swigDelegate308 = new GLAdapter.SwigDelegateGLAdapter_308(this.SwigDirectorProcess_glMemoryBarrier);
		}
		if (this.SwigDerivedClassHasMethod("Process_glMemoryBarrierByRegion", GLAdapter.swigMethodTypes309))
		{
			this.swigDelegate309 = new GLAdapter.SwigDelegateGLAdapter_309(this.SwigDirectorProcess_glMemoryBarrierByRegion);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexStorage2DMultisample", GLAdapter.swigMethodTypes310))
		{
			this.swigDelegate310 = new GLAdapter.SwigDelegateGLAdapter_310(this.SwigDirectorProcess_glTexStorage2DMultisample);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexStorage3DMultisampleOES", GLAdapter.swigMethodTypes311))
		{
			this.swigDelegate311 = new GLAdapter.SwigDelegateGLAdapter_311(this.SwigDirectorProcess_glTexStorage3DMultisampleOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetMultisamplefv", GLAdapter.swigMethodTypes312))
		{
			this.swigDelegate312 = new GLAdapter.SwigDelegateGLAdapter_312(this.SwigDirectorProcess_glGetMultisamplefv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSampleMaski", GLAdapter.swigMethodTypes313))
		{
			this.swigDelegate313 = new GLAdapter.SwigDelegateGLAdapter_313(this.SwigDirectorProcess_glSampleMaski);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetTexLevelParameteriv", GLAdapter.swigMethodTypes314))
		{
			this.swigDelegate314 = new GLAdapter.SwigDelegateGLAdapter_314(this.SwigDirectorProcess_glGetTexLevelParameteriv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetTexLevelParameterfv", GLAdapter.swigMethodTypes315))
		{
			this.swigDelegate315 = new GLAdapter.SwigDelegateGLAdapter_315(this.SwigDirectorProcess_glGetTexLevelParameterfv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindVertexBuffer", GLAdapter.swigMethodTypes316))
		{
			this.swigDelegate316 = new GLAdapter.SwigDelegateGLAdapter_316(this.SwigDirectorProcess_glBindVertexBuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribFormat", GLAdapter.swigMethodTypes317))
		{
			this.swigDelegate317 = new GLAdapter.SwigDelegateGLAdapter_317(this.SwigDirectorProcess_glVertexAttribFormat);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribIFormat", GLAdapter.swigMethodTypes318))
		{
			this.swigDelegate318 = new GLAdapter.SwigDelegateGLAdapter_318(this.SwigDirectorProcess_glVertexAttribIFormat);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexAttribBinding", GLAdapter.swigMethodTypes319))
		{
			this.swigDelegate319 = new GLAdapter.SwigDelegateGLAdapter_319(this.SwigDirectorProcess_glVertexAttribBinding);
		}
		if (this.SwigDerivedClassHasMethod("Process_glVertexBindingDivisor", GLAdapter.swigMethodTypes320))
		{
			this.swigDelegate320 = new GLAdapter.SwigDelegateGLAdapter_320(this.SwigDirectorProcess_glVertexBindingDivisor);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPatchParameteri", GLAdapter.swigMethodTypes321))
		{
			this.swigDelegate321 = new GLAdapter.SwigDelegateGLAdapter_321(this.SwigDirectorProcess_glPatchParameteri);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetFixedvAMD", GLAdapter.swigMethodTypes322))
		{
			this.swigDelegate322 = new GLAdapter.SwigDelegateGLAdapter_322(this.SwigDirectorProcess_glGetFixedvAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glLogicOpAMD", GLAdapter.swigMethodTypes323))
		{
			this.swigDelegate323 = new GLAdapter.SwigDelegateGLAdapter_323(this.SwigDirectorProcess_glLogicOpAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFogfvAMD", GLAdapter.swigMethodTypes324))
		{
			this.swigDelegate324 = new GLAdapter.SwigDelegateGLAdapter_324(this.SwigDirectorProcess_glFogfvAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetMemoryStatsQCOM", GLAdapter.swigMethodTypes325))
		{
			this.swigDelegate325 = new GLAdapter.SwigDelegateGLAdapter_325(this.SwigDirectorProcess_glGetMemoryStatsQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetSizedMemoryStatsQCOM", GLAdapter.swigMethodTypes326))
		{
			this.swigDelegate326 = new GLAdapter.SwigDelegateGLAdapter_326(this.SwigDirectorProcess_glGetSizedMemoryStatsQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlitOverlapQCOM", GLAdapter.swigMethodTypes327))
		{
			this.swigDelegate327 = new GLAdapter.SwigDelegateGLAdapter_327(this.SwigDirectorProcess_glBlitOverlapQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetShaderStatsQCOM", GLAdapter.swigMethodTypes328))
		{
			this.swigDelegate328 = new GLAdapter.SwigDelegateGLAdapter_328(this.SwigDirectorProcess_glGetShaderStatsQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glExtGetSamplersQCOM", GLAdapter.swigMethodTypes329))
		{
			this.swigDelegate329 = new GLAdapter.SwigDelegateGLAdapter_329(this.SwigDirectorProcess_glExtGetSamplersQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glClipPlanefQCOM", GLAdapter.swigMethodTypes330))
		{
			this.swigDelegate330 = new GLAdapter.SwigDelegateGLAdapter_330(this.SwigDirectorProcess_glClipPlanefQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferTexture2DExternalQCOM", GLAdapter.swigMethodTypes331))
		{
			this.swigDelegate331 = new GLAdapter.SwigDelegateGLAdapter_331(this.SwigDirectorProcess_glFramebufferTexture2DExternalQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferRenderbufferExternalQCOM", GLAdapter.swigMethodTypes332))
		{
			this.swigDelegate332 = new GLAdapter.SwigDelegateGLAdapter_332(this.SwigDirectorProcess_glFramebufferRenderbufferExternalQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEGLImageTargetTexture2DOES", GLAdapter.swigMethodTypes333))
		{
			this.swigDelegate333 = new GLAdapter.SwigDelegateGLAdapter_333(this.SwigDirectorProcess_glEGLImageTargetTexture2DOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEGLImageTargetRenderbufferStorageOES", GLAdapter.swigMethodTypes334))
		{
			this.swigDelegate334 = new GLAdapter.SwigDelegateGLAdapter_334(this.SwigDirectorProcess_glEGLImageTargetRenderbufferStorageOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramBinaryOES", GLAdapter.swigMethodTypes335))
		{
			this.swigDelegate335 = new GLAdapter.SwigDelegateGLAdapter_335(this.SwigDirectorProcess_glGetProgramBinaryOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glProgramBinaryOES", GLAdapter.swigMethodTypes336))
		{
			this.swigDelegate336 = new GLAdapter.SwigDelegateGLAdapter_336(this.SwigDirectorProcess_glProgramBinaryOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexImage3DOES", GLAdapter.swigMethodTypes337))
		{
			this.swigDelegate337 = new GLAdapter.SwigDelegateGLAdapter_337(this.SwigDirectorProcess_glTexImage3DOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexSubImage3DOES", GLAdapter.swigMethodTypes338))
		{
			this.swigDelegate338 = new GLAdapter.SwigDelegateGLAdapter_338(this.SwigDirectorProcess_glTexSubImage3DOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCopyTexSubImage3DOES", GLAdapter.swigMethodTypes339))
		{
			this.swigDelegate339 = new GLAdapter.SwigDelegateGLAdapter_339(this.SwigDirectorProcess_glCopyTexSubImage3DOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCompressedTexImage3DOES", GLAdapter.swigMethodTypes340))
		{
			this.swigDelegate340 = new GLAdapter.SwigDelegateGLAdapter_340(this.SwigDirectorProcess_glCompressedTexImage3DOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCompressedTexSubImage3DOES", GLAdapter.swigMethodTypes341))
		{
			this.swigDelegate341 = new GLAdapter.SwigDelegateGLAdapter_341(this.SwigDirectorProcess_glCompressedTexSubImage3DOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferTexture3DOES", GLAdapter.swigMethodTypes342))
		{
			this.swigDelegate342 = new GLAdapter.SwigDelegateGLAdapter_342(this.SwigDirectorProcess_glFramebufferTexture3DOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindVertexArrayOES", GLAdapter.swigMethodTypes343))
		{
			this.swigDelegate343 = new GLAdapter.SwigDelegateGLAdapter_343(this.SwigDirectorProcess_glBindVertexArrayOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteVertexArraysOES", GLAdapter.swigMethodTypes344))
		{
			this.swigDelegate344 = new GLAdapter.SwigDelegateGLAdapter_344(this.SwigDirectorProcess_glDeleteVertexArraysOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenVertexArraysOES", GLAdapter.swigMethodTypes345))
		{
			this.swigDelegate345 = new GLAdapter.SwigDelegateGLAdapter_345(this.SwigDirectorProcess_glGenVertexArraysOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsVertexArrayOES", GLAdapter.swigMethodTypes346))
		{
			this.swigDelegate346 = new GLAdapter.SwigDelegateGLAdapter_346(this.SwigDirectorProcess_glIsVertexArrayOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetPerfMonitorGroupsAMD", GLAdapter.swigMethodTypes347))
		{
			this.swigDelegate347 = new GLAdapter.SwigDelegateGLAdapter_347(this.SwigDirectorProcess_glGetPerfMonitorGroupsAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetPerfMonitorCountersAMD", GLAdapter.swigMethodTypes348))
		{
			this.swigDelegate348 = new GLAdapter.SwigDelegateGLAdapter_348(this.SwigDirectorProcess_glGetPerfMonitorCountersAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetPerfMonitorGroupStringAMD", GLAdapter.swigMethodTypes349))
		{
			this.swigDelegate349 = new GLAdapter.SwigDelegateGLAdapter_349(this.SwigDirectorProcess_glGetPerfMonitorGroupStringAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetPerfMonitorCounterStringAMD", GLAdapter.swigMethodTypes350))
		{
			this.swigDelegate350 = new GLAdapter.SwigDelegateGLAdapter_350(this.SwigDirectorProcess_glGetPerfMonitorCounterStringAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetPerfMonitorCounterInfoAMD", GLAdapter.swigMethodTypes351))
		{
			this.swigDelegate351 = new GLAdapter.SwigDelegateGLAdapter_351(this.SwigDirectorProcess_glGetPerfMonitorCounterInfoAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenPerfMonitorsAMD", GLAdapter.swigMethodTypes352))
		{
			this.swigDelegate352 = new GLAdapter.SwigDelegateGLAdapter_352(this.SwigDirectorProcess_glGenPerfMonitorsAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeletePerfMonitorsAMD", GLAdapter.swigMethodTypes353))
		{
			this.swigDelegate353 = new GLAdapter.SwigDelegateGLAdapter_353(this.SwigDirectorProcess_glDeletePerfMonitorsAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSelectPerfMonitorCountersAMD", GLAdapter.swigMethodTypes354))
		{
			this.swigDelegate354 = new GLAdapter.SwigDelegateGLAdapter_354(this.SwigDirectorProcess_glSelectPerfMonitorCountersAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBeginPerfMonitorAMD", GLAdapter.swigMethodTypes355))
		{
			this.swigDelegate355 = new GLAdapter.SwigDelegateGLAdapter_355(this.SwigDirectorProcess_glBeginPerfMonitorAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEndPerfMonitorAMD", GLAdapter.swigMethodTypes356))
		{
			this.swigDelegate356 = new GLAdapter.SwigDelegateGLAdapter_356(this.SwigDirectorProcess_glEndPerfMonitorAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetPerfMonitorCounterDataAMD", GLAdapter.swigMethodTypes357))
		{
			this.swigDelegate357 = new GLAdapter.SwigDelegateGLAdapter_357(this.SwigDirectorProcess_glGetPerfMonitorCounterDataAMD);
		}
		if (this.SwigDerivedClassHasMethod("Process_glLabelObjectEXT", GLAdapter.swigMethodTypes358))
		{
			this.swigDelegate358 = new GLAdapter.SwigDelegateGLAdapter_358(this.SwigDirectorProcess_glLabelObjectEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetObjectLabelEXT", GLAdapter.swigMethodTypes359))
		{
			this.swigDelegate359 = new GLAdapter.SwigDelegateGLAdapter_359(this.SwigDirectorProcess_glGetObjectLabelEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glInsertEventMarkerEXT", GLAdapter.swigMethodTypes360))
		{
			this.swigDelegate360 = new GLAdapter.SwigDelegateGLAdapter_360(this.SwigDirectorProcess_glInsertEventMarkerEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPushGroupMarkerEXT", GLAdapter.swigMethodTypes361))
		{
			this.swigDelegate361 = new GLAdapter.SwigDelegateGLAdapter_361(this.SwigDirectorProcess_glPushGroupMarkerEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPopGroupMarkerEXT", GLAdapter.swigMethodTypes362))
		{
			this.swigDelegate362 = new GLAdapter.SwigDelegateGLAdapter_362(this.SwigDirectorProcess_glPopGroupMarkerEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDiscardFramebufferEXT", GLAdapter.swigMethodTypes363))
		{
			this.swigDelegate363 = new GLAdapter.SwigDelegateGLAdapter_363(this.SwigDirectorProcess_glDiscardFramebufferEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenQueriesEXT", GLAdapter.swigMethodTypes364))
		{
			this.swigDelegate364 = new GLAdapter.SwigDelegateGLAdapter_364(this.SwigDirectorProcess_glGenQueriesEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteQueriesEXT", GLAdapter.swigMethodTypes365))
		{
			this.swigDelegate365 = new GLAdapter.SwigDelegateGLAdapter_365(this.SwigDirectorProcess_glDeleteQueriesEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsQueryEXT", GLAdapter.swigMethodTypes366))
		{
			this.swigDelegate366 = new GLAdapter.SwigDelegateGLAdapter_366(this.SwigDirectorProcess_glIsQueryEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBeginQueryEXT", GLAdapter.swigMethodTypes367))
		{
			this.swigDelegate367 = new GLAdapter.SwigDelegateGLAdapter_367(this.SwigDirectorProcess_glBeginQueryEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEndQueryEXT", GLAdapter.swigMethodTypes368))
		{
			this.swigDelegate368 = new GLAdapter.SwigDelegateGLAdapter_368(this.SwigDirectorProcess_glEndQueryEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glQueryCounterEXT", GLAdapter.swigMethodTypes369))
		{
			this.swigDelegate369 = new GLAdapter.SwigDelegateGLAdapter_369(this.SwigDirectorProcess_glQueryCounterEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetQueryivEXT", GLAdapter.swigMethodTypes370))
		{
			this.swigDelegate370 = new GLAdapter.SwigDelegateGLAdapter_370(this.SwigDirectorProcess_glGetQueryivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetQueryObjectivEXT", GLAdapter.swigMethodTypes371))
		{
			this.swigDelegate371 = new GLAdapter.SwigDelegateGLAdapter_371(this.SwigDirectorProcess_glGetQueryObjectivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetQueryObjectuivEXT", GLAdapter.swigMethodTypes372))
		{
			this.swigDelegate372 = new GLAdapter.SwigDelegateGLAdapter_372(this.SwigDirectorProcess_glGetQueryObjectuivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetQueryObjecti64vEXT", GLAdapter.swigMethodTypes373))
		{
			this.swigDelegate373 = new GLAdapter.SwigDelegateGLAdapter_373(this.SwigDirectorProcess_glGetQueryObjecti64vEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetQueryObjectui64vEXT", GLAdapter.swigMethodTypes374))
		{
			this.swigDelegate374 = new GLAdapter.SwigDelegateGLAdapter_374(this.SwigDirectorProcess_glGetQueryObjectui64vEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetGraphicsResetStatusEXT", GLAdapter.swigMethodTypes375))
		{
			this.swigDelegate375 = new GLAdapter.SwigDelegateGLAdapter_375(this.SwigDirectorProcess_glGetGraphicsResetStatusEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glReadnPixelsEXT", GLAdapter.swigMethodTypes376))
		{
			this.swigDelegate376 = new GLAdapter.SwigDelegateGLAdapter_376(this.SwigDirectorProcess_glReadnPixelsEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetnUniformfvEXT", GLAdapter.swigMethodTypes377))
		{
			this.swigDelegate377 = new GLAdapter.SwigDelegateGLAdapter_377(this.SwigDirectorProcess_glGetnUniformfvEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetnUniformivEXT", GLAdapter.swigMethodTypes378))
		{
			this.swigDelegate378 = new GLAdapter.SwigDelegateGLAdapter_378(this.SwigDirectorProcess_glGetnUniformivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexParameterIivEXT", GLAdapter.swigMethodTypes379))
		{
			this.swigDelegate379 = new GLAdapter.SwigDelegateGLAdapter_379(this.SwigDirectorProcess_glTexParameterIivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexParameterIuivEXT", GLAdapter.swigMethodTypes380))
		{
			this.swigDelegate380 = new GLAdapter.SwigDelegateGLAdapter_380(this.SwigDirectorProcess_glTexParameterIuivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetTexParameterIivEXT", GLAdapter.swigMethodTypes381))
		{
			this.swigDelegate381 = new GLAdapter.SwigDelegateGLAdapter_381(this.SwigDirectorProcess_glGetTexParameterIivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetTexParameterIuivEXT", GLAdapter.swigMethodTypes382))
		{
			this.swigDelegate382 = new GLAdapter.SwigDelegateGLAdapter_382(this.SwigDirectorProcess_glGetTexParameterIuivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSamplerParameterIivEXT", GLAdapter.swigMethodTypes383))
		{
			this.swigDelegate383 = new GLAdapter.SwigDelegateGLAdapter_383(this.SwigDirectorProcess_glSamplerParameterIivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSamplerParameterIuivEXT", GLAdapter.swigMethodTypes384))
		{
			this.swigDelegate384 = new GLAdapter.SwigDelegateGLAdapter_384(this.SwigDirectorProcess_glSamplerParameterIuivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetSamplerParameterIivEXT", GLAdapter.swigMethodTypes385))
		{
			this.swigDelegate385 = new GLAdapter.SwigDelegateGLAdapter_385(this.SwigDirectorProcess_glGetSamplerParameterIivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetSamplerParameterIuivEXT", GLAdapter.swigMethodTypes386))
		{
			this.swigDelegate386 = new GLAdapter.SwigDelegateGLAdapter_386(this.SwigDirectorProcess_glGetSamplerParameterIuivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glRenderbufferStorageMultisampleEXT", GLAdapter.swigMethodTypes387))
		{
			this.swigDelegate387 = new GLAdapter.SwigDelegateGLAdapter_387(this.SwigDirectorProcess_glRenderbufferStorageMultisampleEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferTexture2DMultisampleEXT", GLAdapter.swigMethodTypes388))
		{
			this.swigDelegate388 = new GLAdapter.SwigDelegateGLAdapter_388(this.SwigDirectorProcess_glFramebufferTexture2DMultisampleEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glAlphaFuncQCOM", GLAdapter.swigMethodTypes389))
		{
			this.swigDelegate389 = new GLAdapter.SwigDelegateGLAdapter_389(this.SwigDirectorProcess_glAlphaFuncQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glStartTilingQCOM", GLAdapter.swigMethodTypes390))
		{
			this.swigDelegate390 = new GLAdapter.SwigDelegateGLAdapter_390(this.SwigDirectorProcess_glStartTilingQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEndTilingQCOM", GLAdapter.swigMethodTypes391))
		{
			this.swigDelegate391 = new GLAdapter.SwigDelegateGLAdapter_391(this.SwigDirectorProcess_glEndTilingQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCopyImageSubDataEXT", GLAdapter.swigMethodTypes392))
		{
			this.swigDelegate392 = new GLAdapter.SwigDelegateGLAdapter_392(this.SwigDirectorProcess_glCopyImageSubDataEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendBarrierKHR", GLAdapter.swigMethodTypes393))
		{
			this.swigDelegate393 = new GLAdapter.SwigDelegateGLAdapter_393(this.SwigDirectorProcess_glBlendBarrierKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glMinSampleShadingOES", GLAdapter.swigMethodTypes394))
		{
			this.swigDelegate394 = new GLAdapter.SwigDelegateGLAdapter_394(this.SwigDirectorProcess_glMinSampleShadingOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEnableiEXT", GLAdapter.swigMethodTypes395))
		{
			this.swigDelegate395 = new GLAdapter.SwigDelegateGLAdapter_395(this.SwigDirectorProcess_glEnableiEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDisableiEXT", GLAdapter.swigMethodTypes396))
		{
			this.swigDelegate396 = new GLAdapter.SwigDelegateGLAdapter_396(this.SwigDirectorProcess_glDisableiEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendEquationiEXT", GLAdapter.swigMethodTypes397))
		{
			this.swigDelegate397 = new GLAdapter.SwigDelegateGLAdapter_397(this.SwigDirectorProcess_glBlendEquationiEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendEquationSeparateiEXT", GLAdapter.swigMethodTypes398))
		{
			this.swigDelegate398 = new GLAdapter.SwigDelegateGLAdapter_398(this.SwigDirectorProcess_glBlendEquationSeparateiEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendFunciEXT", GLAdapter.swigMethodTypes399))
		{
			this.swigDelegate399 = new GLAdapter.SwigDelegateGLAdapter_399(this.SwigDirectorProcess_glBlendFunciEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendFuncSeparateiEXT", GLAdapter.swigMethodTypes400))
		{
			this.swigDelegate400 = new GLAdapter.SwigDelegateGLAdapter_400(this.SwigDirectorProcess_glBlendFuncSeparateiEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glColorMaskiEXT", GLAdapter.swigMethodTypes401))
		{
			this.swigDelegate401 = new GLAdapter.SwigDelegateGLAdapter_401(this.SwigDirectorProcess_glColorMaskiEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsEnablediEXT", GLAdapter.swigMethodTypes402))
		{
			this.swigDelegate402 = new GLAdapter.SwigDelegateGLAdapter_402(this.SwigDirectorProcess_glIsEnablediEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexBufferEXT", GLAdapter.swigMethodTypes403))
		{
			this.swigDelegate403 = new GLAdapter.SwigDelegateGLAdapter_403(this.SwigDirectorProcess_glTexBufferEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexBufferRangeEXT", GLAdapter.swigMethodTypes404))
		{
			this.swigDelegate404 = new GLAdapter.SwigDelegateGLAdapter_404(this.SwigDirectorProcess_glTexBufferRangeEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDebugMessageControlKHR", GLAdapter.swigMethodTypes405))
		{
			this.swigDelegate405 = new GLAdapter.SwigDelegateGLAdapter_405(this.SwigDirectorProcess_glDebugMessageControlKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDebugMessageInsertKHR", GLAdapter.swigMethodTypes406))
		{
			this.swigDelegate406 = new GLAdapter.SwigDelegateGLAdapter_406(this.SwigDirectorProcess_glDebugMessageInsertKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDebugMessageCallbackKHR", GLAdapter.swigMethodTypes407))
		{
			this.swigDelegate407 = new GLAdapter.SwigDelegateGLAdapter_407(this.SwigDirectorProcess_glDebugMessageCallbackKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetDebugMessageLogKHR", GLAdapter.swigMethodTypes408))
		{
			this.swigDelegate408 = new GLAdapter.SwigDelegateGLAdapter_408(this.SwigDirectorProcess_glGetDebugMessageLogKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPushDebugGroupKHR", GLAdapter.swigMethodTypes409))
		{
			this.swigDelegate409 = new GLAdapter.SwigDelegateGLAdapter_409(this.SwigDirectorProcess_glPushDebugGroupKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPopDebugGroupKHR", GLAdapter.swigMethodTypes410))
		{
			this.swigDelegate410 = new GLAdapter.SwigDelegateGLAdapter_410(this.SwigDirectorProcess_glPopDebugGroupKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glObjectLabelKHR", GLAdapter.swigMethodTypes411))
		{
			this.swigDelegate411 = new GLAdapter.SwigDelegateGLAdapter_411(this.SwigDirectorProcess_glObjectLabelKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetObjectLabelKHR", GLAdapter.swigMethodTypes412))
		{
			this.swigDelegate412 = new GLAdapter.SwigDelegateGLAdapter_412(this.SwigDirectorProcess_glGetObjectLabelKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glObjectPtrLabelKHR", GLAdapter.swigMethodTypes413))
		{
			this.swigDelegate413 = new GLAdapter.SwigDelegateGLAdapter_413(this.SwigDirectorProcess_glObjectPtrLabelKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetObjectPtrLabelKHR", GLAdapter.swigMethodTypes414))
		{
			this.swigDelegate414 = new GLAdapter.SwigDelegateGLAdapter_414(this.SwigDirectorProcess_glGetObjectPtrLabelKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetPointervKHR", GLAdapter.swigMethodTypes415))
		{
			this.swigDelegate415 = new GLAdapter.SwigDelegateGLAdapter_415(this.SwigDirectorProcess_glGetPointervKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPrimitiveBoundingBoxEXT", GLAdapter.swigMethodTypes416))
		{
			this.swigDelegate416 = new GLAdapter.SwigDelegateGLAdapter_416(this.SwigDirectorProcess_glPrimitiveBoundingBoxEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPatchParameteriEXT", GLAdapter.swigMethodTypes417))
		{
			this.swigDelegate417 = new GLAdapter.SwigDelegateGLAdapter_417(this.SwigDirectorProcess_glPatchParameteriEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawElementsBaseVertex", GLAdapter.swigMethodTypes418))
		{
			this.swigDelegate418 = new GLAdapter.SwigDelegateGLAdapter_418(this.SwigDirectorProcess_glDrawElementsBaseVertex);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawRangeElementsBaseVertex", GLAdapter.swigMethodTypes419))
		{
			this.swigDelegate419 = new GLAdapter.SwigDelegateGLAdapter_419(this.SwigDirectorProcess_glDrawRangeElementsBaseVertex);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDrawElementsInstancedBaseVertex", GLAdapter.swigMethodTypes420))
		{
			this.swigDelegate420 = new GLAdapter.SwigDelegateGLAdapter_420(this.SwigDirectorProcess_glDrawElementsInstancedBaseVertex);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferTextureEXT", GLAdapter.swigMethodTypes421))
		{
			this.swigDelegate421 = new GLAdapter.SwigDelegateGLAdapter_421(this.SwigDirectorProcess_glFramebufferTextureEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferTextureMultiviewOVR", GLAdapter.swigMethodTypes422))
		{
			this.swigDelegate422 = new GLAdapter.SwigDelegateGLAdapter_422(this.SwigDirectorProcess_glFramebufferTextureMultiviewOVR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferTextureMultisampleMultiviewOVR", GLAdapter.swigMethodTypes423))
		{
			this.swigDelegate423 = new GLAdapter.SwigDelegateGLAdapter_423(this.SwigDirectorProcess_glFramebufferTextureMultisampleMultiviewOVR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBufferStorageEXT", GLAdapter.swigMethodTypes424))
		{
			this.swigDelegate424 = new GLAdapter.SwigDelegateGLAdapter_424(this.SwigDirectorProcess_glBufferStorageEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetGraphicsResetStatus", GLAdapter.swigMethodTypes425))
		{
			this.swigDelegate425 = new GLAdapter.SwigDelegateGLAdapter_425(this.SwigDirectorProcess_glGetGraphicsResetStatus);
		}
		if (this.SwigDerivedClassHasMethod("Process_glReadnPixels", GLAdapter.swigMethodTypes426))
		{
			this.swigDelegate426 = new GLAdapter.SwigDelegateGLAdapter_426(this.SwigDirectorProcess_glReadnPixels);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetnUniformfv", GLAdapter.swigMethodTypes427))
		{
			this.swigDelegate427 = new GLAdapter.SwigDelegateGLAdapter_427(this.SwigDirectorProcess_glGetnUniformfv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetnUniformiv", GLAdapter.swigMethodTypes428))
		{
			this.swigDelegate428 = new GLAdapter.SwigDelegateGLAdapter_428(this.SwigDirectorProcess_glGetnUniformiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetnUniformuiv", GLAdapter.swigMethodTypes429))
		{
			this.swigDelegate429 = new GLAdapter.SwigDelegateGLAdapter_429(this.SwigDirectorProcess_glGetnUniformuiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexParameterIiv", GLAdapter.swigMethodTypes430))
		{
			this.swigDelegate430 = new GLAdapter.SwigDelegateGLAdapter_430(this.SwigDirectorProcess_glTexParameterIiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexParameterIuiv", GLAdapter.swigMethodTypes431))
		{
			this.swigDelegate431 = new GLAdapter.SwigDelegateGLAdapter_431(this.SwigDirectorProcess_glTexParameterIuiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetTexParameterIiv", GLAdapter.swigMethodTypes432))
		{
			this.swigDelegate432 = new GLAdapter.SwigDelegateGLAdapter_432(this.SwigDirectorProcess_glGetTexParameterIiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetTexParameterIuiv", GLAdapter.swigMethodTypes433))
		{
			this.swigDelegate433 = new GLAdapter.SwigDelegateGLAdapter_433(this.SwigDirectorProcess_glGetTexParameterIuiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSamplerParameterIiv", GLAdapter.swigMethodTypes434))
		{
			this.swigDelegate434 = new GLAdapter.SwigDelegateGLAdapter_434(this.SwigDirectorProcess_glSamplerParameterIiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSamplerParameterIuiv", GLAdapter.swigMethodTypes435))
		{
			this.swigDelegate435 = new GLAdapter.SwigDelegateGLAdapter_435(this.SwigDirectorProcess_glSamplerParameterIuiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetSamplerParameterIiv", GLAdapter.swigMethodTypes436))
		{
			this.swigDelegate436 = new GLAdapter.SwigDelegateGLAdapter_436(this.SwigDirectorProcess_glGetSamplerParameterIiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetSamplerParameterIuiv", GLAdapter.swigMethodTypes437))
		{
			this.swigDelegate437 = new GLAdapter.SwigDelegateGLAdapter_437(this.SwigDirectorProcess_glGetSamplerParameterIuiv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glNumBinsPerSubmitQCOM", GLAdapter.swigMethodTypes438))
		{
			this.swigDelegate438 = new GLAdapter.SwigDelegateGLAdapter_438(this.SwigDirectorProcess_glNumBinsPerSubmitQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCopyImageSubData", GLAdapter.swigMethodTypes439))
		{
			this.swigDelegate439 = new GLAdapter.SwigDelegateGLAdapter_439(this.SwigDirectorProcess_glCopyImageSubData);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendBarrier", GLAdapter.swigMethodTypes440))
		{
			this.swigDelegate440 = new GLAdapter.SwigDelegateGLAdapter_440(this.SwigDirectorProcess_glBlendBarrier);
		}
		if (this.SwigDerivedClassHasMethod("Process_glMinSampleShading", GLAdapter.swigMethodTypes441))
		{
			this.swigDelegate441 = new GLAdapter.SwigDelegateGLAdapter_441(this.SwigDirectorProcess_glMinSampleShading);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEnablei", GLAdapter.swigMethodTypes442))
		{
			this.swigDelegate442 = new GLAdapter.SwigDelegateGLAdapter_442(this.SwigDirectorProcess_glEnablei);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDisablei", GLAdapter.swigMethodTypes443))
		{
			this.swigDelegate443 = new GLAdapter.SwigDelegateGLAdapter_443(this.SwigDirectorProcess_glDisablei);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendEquationi", GLAdapter.swigMethodTypes444))
		{
			this.swigDelegate444 = new GLAdapter.SwigDelegateGLAdapter_444(this.SwigDirectorProcess_glBlendEquationi);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendEquationSeparatei", GLAdapter.swigMethodTypes445))
		{
			this.swigDelegate445 = new GLAdapter.SwigDelegateGLAdapter_445(this.SwigDirectorProcess_glBlendEquationSeparatei);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendFunci", GLAdapter.swigMethodTypes446))
		{
			this.swigDelegate446 = new GLAdapter.SwigDelegateGLAdapter_446(this.SwigDirectorProcess_glBlendFunci);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlendFuncSeparatei", GLAdapter.swigMethodTypes447))
		{
			this.swigDelegate447 = new GLAdapter.SwigDelegateGLAdapter_447(this.SwigDirectorProcess_glBlendFuncSeparatei);
		}
		if (this.SwigDerivedClassHasMethod("Process_glColorMaski", GLAdapter.swigMethodTypes448))
		{
			this.swigDelegate448 = new GLAdapter.SwigDelegateGLAdapter_448(this.SwigDirectorProcess_glColorMaski);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsEnabledi", GLAdapter.swigMethodTypes449))
		{
			this.swigDelegate449 = new GLAdapter.SwigDelegateGLAdapter_449(this.SwigDirectorProcess_glIsEnabledi);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexBuffer", GLAdapter.swigMethodTypes450))
		{
			this.swigDelegate450 = new GLAdapter.SwigDelegateGLAdapter_450(this.SwigDirectorProcess_glTexBuffer);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexBufferRange", GLAdapter.swigMethodTypes451))
		{
			this.swigDelegate451 = new GLAdapter.SwigDelegateGLAdapter_451(this.SwigDirectorProcess_glTexBufferRange);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDebugMessageControl", GLAdapter.swigMethodTypes452))
		{
			this.swigDelegate452 = new GLAdapter.SwigDelegateGLAdapter_452(this.SwigDirectorProcess_glDebugMessageControl);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDebugMessageInsert", GLAdapter.swigMethodTypes453))
		{
			this.swigDelegate453 = new GLAdapter.SwigDelegateGLAdapter_453(this.SwigDirectorProcess_glDebugMessageInsert);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDebugMessageCallback", GLAdapter.swigMethodTypes454))
		{
			this.swigDelegate454 = new GLAdapter.SwigDelegateGLAdapter_454(this.SwigDirectorProcess_glDebugMessageCallback);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetDebugMessageLog", GLAdapter.swigMethodTypes455))
		{
			this.swigDelegate455 = new GLAdapter.SwigDelegateGLAdapter_455(this.SwigDirectorProcess_glGetDebugMessageLog);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPushDebugGroup", GLAdapter.swigMethodTypes456))
		{
			this.swigDelegate456 = new GLAdapter.SwigDelegateGLAdapter_456(this.SwigDirectorProcess_glPushDebugGroup);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPopDebugGroup", GLAdapter.swigMethodTypes457))
		{
			this.swigDelegate457 = new GLAdapter.SwigDelegateGLAdapter_457(this.SwigDirectorProcess_glPopDebugGroup);
		}
		if (this.SwigDerivedClassHasMethod("Process_glObjectLabel", GLAdapter.swigMethodTypes458))
		{
			this.swigDelegate458 = new GLAdapter.SwigDelegateGLAdapter_458(this.SwigDirectorProcess_glObjectLabel);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetObjectLabel", GLAdapter.swigMethodTypes459))
		{
			this.swigDelegate459 = new GLAdapter.SwigDelegateGLAdapter_459(this.SwigDirectorProcess_glGetObjectLabel);
		}
		if (this.SwigDerivedClassHasMethod("Process_glObjectPtrLabel", GLAdapter.swigMethodTypes460))
		{
			this.swigDelegate460 = new GLAdapter.SwigDelegateGLAdapter_460(this.SwigDirectorProcess_glObjectPtrLabel);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetObjectPtrLabel", GLAdapter.swigMethodTypes461))
		{
			this.swigDelegate461 = new GLAdapter.SwigDelegateGLAdapter_461(this.SwigDirectorProcess_glGetObjectPtrLabel);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetPointerv", GLAdapter.swigMethodTypes462))
		{
			this.swigDelegate462 = new GLAdapter.SwigDelegateGLAdapter_462(this.SwigDirectorProcess_glGetPointerv);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPrimitiveBoundingBox", GLAdapter.swigMethodTypes463))
		{
			this.swigDelegate463 = new GLAdapter.SwigDelegateGLAdapter_463(this.SwigDirectorProcess_glPrimitiveBoundingBox);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlitBlendColor", GLAdapter.swigMethodTypes464))
		{
			this.swigDelegate464 = new GLAdapter.SwigDelegateGLAdapter_464(this.SwigDirectorProcess_glBlitBlendColor);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlitBlendEquationSeparate", GLAdapter.swigMethodTypes465))
		{
			this.swigDelegate465 = new GLAdapter.SwigDelegateGLAdapter_465(this.SwigDirectorProcess_glBlitBlendEquationSeparate);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlitBlendFuncSeparate", GLAdapter.swigMethodTypes466))
		{
			this.swigDelegate466 = new GLAdapter.SwigDelegateGLAdapter_466(this.SwigDirectorProcess_glBlitBlendFuncSeparate);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBlitRotation", GLAdapter.swigMethodTypes467))
		{
			this.swigDelegate467 = new GLAdapter.SwigDelegateGLAdapter_467(this.SwigDirectorProcess_glBlitRotation);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindSharedBufferQCOM", GLAdapter.swigMethodTypes468))
		{
			this.swigDelegate468 = new GLAdapter.SwigDelegateGLAdapter_468(this.SwigDirectorProcess_glBindSharedBufferQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCreateSharedBufferQCOM", GLAdapter.swigMethodTypes469))
		{
			this.swigDelegate469 = new GLAdapter.SwigDelegateGLAdapter_469(this.SwigDirectorProcess_glCreateSharedBufferQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDestroySharedBufferQCOM", GLAdapter.swigMethodTypes470))
		{
			this.swigDelegate470 = new GLAdapter.SwigDelegateGLAdapter_470(this.SwigDirectorProcess_glDestroySharedBufferQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTextureBarrier", GLAdapter.swigMethodTypes471))
		{
			this.swigDelegate471 = new GLAdapter.SwigDelegateGLAdapter_471(this.SwigDirectorProcess_glTextureBarrier);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferFoveationConfigQCOM", GLAdapter.swigMethodTypes472))
		{
			this.swigDelegate472 = new GLAdapter.SwigDelegateGLAdapter_472(this.SwigDirectorProcess_glFramebufferFoveationConfigQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferFoveationParametersQCOM", GLAdapter.swigMethodTypes473))
		{
			this.swigDelegate473 = new GLAdapter.SwigDelegateGLAdapter_473(this.SwigDirectorProcess_glFramebufferFoveationParametersQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBufferStorageExternalEXT", GLAdapter.swigMethodTypes474))
		{
			this.swigDelegate474 = new GLAdapter.SwigDelegateGLAdapter_474(this.SwigDirectorProcess_glBufferStorageExternalEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferFetchBarrierQCOM", GLAdapter.swigMethodTypes475))
		{
			this.swigDelegate475 = new GLAdapter.SwigDelegateGLAdapter_475(this.SwigDirectorProcess_glFramebufferFetchBarrierQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glCreateMemoryObjectsEXT", GLAdapter.swigMethodTypes476))
		{
			this.swigDelegate476 = new GLAdapter.SwigDelegateGLAdapter_476(this.SwigDirectorProcess_glCreateMemoryObjectsEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteMemoryObjectsEXT", GLAdapter.swigMethodTypes477))
		{
			this.swigDelegate477 = new GLAdapter.SwigDelegateGLAdapter_477(this.SwigDirectorProcess_glDeleteMemoryObjectsEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsMemoryObjectEXT", GLAdapter.swigMethodTypes478))
		{
			this.swigDelegate478 = new GLAdapter.SwigDelegateGLAdapter_478(this.SwigDirectorProcess_glIsMemoryObjectEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glMemoryObjectParameterivEXT", GLAdapter.swigMethodTypes479))
		{
			this.swigDelegate479 = new GLAdapter.SwigDelegateGLAdapter_479(this.SwigDirectorProcess_glMemoryObjectParameterivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetMemoryObjectParameterivEXT", GLAdapter.swigMethodTypes480))
		{
			this.swigDelegate480 = new GLAdapter.SwigDelegateGLAdapter_480(this.SwigDirectorProcess_glGetMemoryObjectParameterivEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexStorageMem2DEXT", GLAdapter.swigMethodTypes481))
		{
			this.swigDelegate481 = new GLAdapter.SwigDelegateGLAdapter_481(this.SwigDirectorProcess_glTexStorageMem2DEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexStorageMem2DMultisampleEXT", GLAdapter.swigMethodTypes482))
		{
			this.swigDelegate482 = new GLAdapter.SwigDelegateGLAdapter_482(this.SwigDirectorProcess_glTexStorageMem2DMultisampleEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexStorageMem3DEXT", GLAdapter.swigMethodTypes483))
		{
			this.swigDelegate483 = new GLAdapter.SwigDelegateGLAdapter_483(this.SwigDirectorProcess_glTexStorageMem3DEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexStorageMem3DMultisampleEXT", GLAdapter.swigMethodTypes484))
		{
			this.swigDelegate484 = new GLAdapter.SwigDelegateGLAdapter_484(this.SwigDirectorProcess_glTexStorageMem3DMultisampleEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBufferStorageMemEXT", GLAdapter.swigMethodTypes485))
		{
			this.swigDelegate485 = new GLAdapter.SwigDelegateGLAdapter_485(this.SwigDirectorProcess_glBufferStorageMemEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGenSemaphoresKHR", GLAdapter.swigMethodTypes486))
		{
			this.swigDelegate486 = new GLAdapter.SwigDelegateGLAdapter_486(this.SwigDirectorProcess_glGenSemaphoresKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glDeleteSemaphoresKHR", GLAdapter.swigMethodTypes487))
		{
			this.swigDelegate487 = new GLAdapter.SwigDelegateGLAdapter_487(this.SwigDirectorProcess_glDeleteSemaphoresKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glIsSemaphoreKHR", GLAdapter.swigMethodTypes488))
		{
			this.swigDelegate488 = new GLAdapter.SwigDelegateGLAdapter_488(this.SwigDirectorProcess_glIsSemaphoreKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glWaitSemaphoreKHR", GLAdapter.swigMethodTypes489))
		{
			this.swigDelegate489 = new GLAdapter.SwigDelegateGLAdapter_489(this.SwigDirectorProcess_glWaitSemaphoreKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glSignalSemaphoreKHR", GLAdapter.swigMethodTypes490))
		{
			this.swigDelegate490 = new GLAdapter.SwigDelegateGLAdapter_490(this.SwigDirectorProcess_glSignalSemaphoreKHR);
		}
		if (this.SwigDerivedClassHasMethod("Process_glImportMemoryFdEXT", GLAdapter.swigMethodTypes491))
		{
			this.swigDelegate491 = new GLAdapter.SwigDelegateGLAdapter_491(this.SwigDirectorProcess_glImportMemoryFdEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glImportSemaphoreFdEXT", GLAdapter.swigMethodTypes492))
		{
			this.swigDelegate492 = new GLAdapter.SwigDelegateGLAdapter_492(this.SwigDirectorProcess_glImportSemaphoreFdEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetUnsignedBytevEXT", GLAdapter.swigMethodTypes493))
		{
			this.swigDelegate493 = new GLAdapter.SwigDelegateGLAdapter_493(this.SwigDirectorProcess_glGetUnsignedBytevEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetUnsignedBytei_vEXT", GLAdapter.swigMethodTypes494))
		{
			this.swigDelegate494 = new GLAdapter.SwigDelegateGLAdapter_494(this.SwigDirectorProcess_glGetUnsignedBytei_vEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTextureFoveationParametersQCOM", GLAdapter.swigMethodTypes495))
		{
			this.swigDelegate495 = new GLAdapter.SwigDelegateGLAdapter_495(this.SwigDirectorProcess_glTextureFoveationParametersQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindFragDataLocationIndexedEXT", GLAdapter.swigMethodTypes496))
		{
			this.swigDelegate496 = new GLAdapter.SwigDelegateGLAdapter_496(this.SwigDirectorProcess_glBindFragDataLocationIndexedEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glBindFragDataLocationEXT", GLAdapter.swigMethodTypes497))
		{
			this.swigDelegate497 = new GLAdapter.SwigDelegateGLAdapter_497(this.SwigDirectorProcess_glBindFragDataLocationEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetProgramResourceLocationIndexEXT", GLAdapter.swigMethodTypes498))
		{
			this.swigDelegate498 = new GLAdapter.SwigDelegateGLAdapter_498(this.SwigDirectorProcess_glGetProgramResourceLocationIndexEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetFragDataIndexEXT", GLAdapter.swigMethodTypes499))
		{
			this.swigDelegate499 = new GLAdapter.SwigDelegateGLAdapter_499(this.SwigDirectorProcess_glGetFragDataIndexEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glShadingRateQCOM", GLAdapter.swigMethodTypes500))
		{
			this.swigDelegate500 = new GLAdapter.SwigDelegateGLAdapter_500(this.SwigDirectorProcess_glShadingRateQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glExtrapolateTex2DQCOM", GLAdapter.swigMethodTypes501))
		{
			this.swigDelegate501 = new GLAdapter.SwigDelegateGLAdapter_501(this.SwigDirectorProcess_glExtrapolateTex2DQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTextureViewOES", GLAdapter.swigMethodTypes502))
		{
			this.swigDelegate502 = new GLAdapter.SwigDelegateGLAdapter_502(this.SwigDirectorProcess_glTextureViewOES);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexEstimateMotionQCOM", GLAdapter.swigMethodTypes503))
		{
			this.swigDelegate503 = new GLAdapter.SwigDelegateGLAdapter_503(this.SwigDirectorProcess_glTexEstimateMotionQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glTexEstimateMotionRegionsQCOM", GLAdapter.swigMethodTypes504))
		{
			this.swigDelegate504 = new GLAdapter.SwigDelegateGLAdapter_504(this.SwigDirectorProcess_glTexEstimateMotionRegionsQCOM);
		}
		if (this.SwigDerivedClassHasMethod("Process_glEGLImageTargetTexStorageEXT", GLAdapter.swigMethodTypes505))
		{
			this.swigDelegate505 = new GLAdapter.SwigDelegateGLAdapter_505(this.SwigDirectorProcess_glEGLImageTargetTexStorageEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glPolygonOffsetClampEXT", GLAdapter.swigMethodTypes506))
		{
			this.swigDelegate506 = new GLAdapter.SwigDelegateGLAdapter_506(this.SwigDirectorProcess_glPolygonOffsetClampEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glGetFragmentShadingRatesEXT", GLAdapter.swigMethodTypes507))
		{
			this.swigDelegate507 = new GLAdapter.SwigDelegateGLAdapter_507(this.SwigDirectorProcess_glGetFragmentShadingRatesEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glShadingRateEXT", GLAdapter.swigMethodTypes508))
		{
			this.swigDelegate508 = new GLAdapter.SwigDelegateGLAdapter_508(this.SwigDirectorProcess_glShadingRateEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glShadingRateCombinerOpsEXT", GLAdapter.swigMethodTypes509))
		{
			this.swigDelegate509 = new GLAdapter.SwigDelegateGLAdapter_509(this.SwigDirectorProcess_glShadingRateCombinerOpsEXT);
		}
		if (this.SwigDerivedClassHasMethod("Process_glFramebufferShadingRateEXT", GLAdapter.swigMethodTypes510))
		{
			this.swigDelegate510 = new GLAdapter.SwigDelegateGLAdapter_510(this.SwigDirectorProcess_glFramebufferShadingRateEXT);
		}
		libDCAPPINVOKE.GLAdapter_director_connect(this.swigCPtr, this.swigDelegate0, this.swigDelegate1, this.swigDelegate2, this.swigDelegate3, this.swigDelegate4, this.swigDelegate5, this.swigDelegate6, this.swigDelegate7, this.swigDelegate8, this.swigDelegate9, this.swigDelegate10, this.swigDelegate11, this.swigDelegate12, this.swigDelegate13, this.swigDelegate14, this.swigDelegate15, this.swigDelegate16, this.swigDelegate17, this.swigDelegate18, this.swigDelegate19, this.swigDelegate20, this.swigDelegate21, this.swigDelegate22, this.swigDelegate23, this.swigDelegate24, this.swigDelegate25, this.swigDelegate26, this.swigDelegate27, this.swigDelegate28, this.swigDelegate29, this.swigDelegate30, this.swigDelegate31, this.swigDelegate32, this.swigDelegate33, this.swigDelegate34, this.swigDelegate35, this.swigDelegate36, this.swigDelegate37, this.swigDelegate38, this.swigDelegate39, this.swigDelegate40, this.swigDelegate41, this.swigDelegate42, this.swigDelegate43, this.swigDelegate44, this.swigDelegate45, this.swigDelegate46, this.swigDelegate47, this.swigDelegate48, this.swigDelegate49, this.swigDelegate50, this.swigDelegate51, this.swigDelegate52, this.swigDelegate53, this.swigDelegate54, this.swigDelegate55, this.swigDelegate56, this.swigDelegate57, this.swigDelegate58, this.swigDelegate59, this.swigDelegate60, this.swigDelegate61, this.swigDelegate62, this.swigDelegate63, this.swigDelegate64, this.swigDelegate65, this.swigDelegate66, this.swigDelegate67, this.swigDelegate68, this.swigDelegate69, this.swigDelegate70, this.swigDelegate71, this.swigDelegate72, this.swigDelegate73, this.swigDelegate74, this.swigDelegate75, this.swigDelegate76, this.swigDelegate77, this.swigDelegate78, this.swigDelegate79, this.swigDelegate80, this.swigDelegate81, this.swigDelegate82, this.swigDelegate83, this.swigDelegate84, this.swigDelegate85, this.swigDelegate86, this.swigDelegate87, this.swigDelegate88, this.swigDelegate89, this.swigDelegate90, this.swigDelegate91, this.swigDelegate92, this.swigDelegate93, this.swigDelegate94, this.swigDelegate95, this.swigDelegate96, this.swigDelegate97, this.swigDelegate98, this.swigDelegate99, this.swigDelegate100, this.swigDelegate101, this.swigDelegate102, this.swigDelegate103, this.swigDelegate104, this.swigDelegate105, this.swigDelegate106, this.swigDelegate107, this.swigDelegate108, this.swigDelegate109, this.swigDelegate110, this.swigDelegate111, this.swigDelegate112, this.swigDelegate113, this.swigDelegate114, this.swigDelegate115, this.swigDelegate116, this.swigDelegate117, this.swigDelegate118, this.swigDelegate119, this.swigDelegate120, this.swigDelegate121, this.swigDelegate122, this.swigDelegate123, this.swigDelegate124, this.swigDelegate125, this.swigDelegate126, this.swigDelegate127, this.swigDelegate128, this.swigDelegate129, this.swigDelegate130, this.swigDelegate131, this.swigDelegate132, this.swigDelegate133, this.swigDelegate134, this.swigDelegate135, this.swigDelegate136, this.swigDelegate137, this.swigDelegate138, this.swigDelegate139, this.swigDelegate140, this.swigDelegate141, this.swigDelegate142, this.swigDelegate143, this.swigDelegate144, this.swigDelegate145, this.swigDelegate146, this.swigDelegate147, this.swigDelegate148, this.swigDelegate149, this.swigDelegate150, this.swigDelegate151, this.swigDelegate152, this.swigDelegate153, this.swigDelegate154, this.swigDelegate155, this.swigDelegate156, this.swigDelegate157, this.swigDelegate158, this.swigDelegate159, this.swigDelegate160, this.swigDelegate161, this.swigDelegate162, this.swigDelegate163, this.swigDelegate164, this.swigDelegate165, this.swigDelegate166, this.swigDelegate167, this.swigDelegate168, this.swigDelegate169, this.swigDelegate170, this.swigDelegate171, this.swigDelegate172, this.swigDelegate173, this.swigDelegate174, this.swigDelegate175, this.swigDelegate176, this.swigDelegate177, this.swigDelegate178, this.swigDelegate179, this.swigDelegate180, this.swigDelegate181, this.swigDelegate182, this.swigDelegate183, this.swigDelegate184, this.swigDelegate185, this.swigDelegate186, this.swigDelegate187, this.swigDelegate188, this.swigDelegate189, this.swigDelegate190, this.swigDelegate191, this.swigDelegate192, this.swigDelegate193, this.swigDelegate194, this.swigDelegate195, this.swigDelegate196, this.swigDelegate197, this.swigDelegate198, this.swigDelegate199, this.swigDelegate200, this.swigDelegate201, this.swigDelegate202, this.swigDelegate203, this.swigDelegate204, this.swigDelegate205, this.swigDelegate206, this.swigDelegate207, this.swigDelegate208, this.swigDelegate209, this.swigDelegate210, this.swigDelegate211, this.swigDelegate212, this.swigDelegate213, this.swigDelegate214, this.swigDelegate215, this.swigDelegate216, this.swigDelegate217, this.swigDelegate218, this.swigDelegate219, this.swigDelegate220, this.swigDelegate221, this.swigDelegate222, this.swigDelegate223, this.swigDelegate224, this.swigDelegate225, this.swigDelegate226, this.swigDelegate227, this.swigDelegate228, this.swigDelegate229, this.swigDelegate230, this.swigDelegate231, this.swigDelegate232, this.swigDelegate233, this.swigDelegate234, this.swigDelegate235, this.swigDelegate236, this.swigDelegate237, this.swigDelegate238, this.swigDelegate239, this.swigDelegate240, this.swigDelegate241, this.swigDelegate242, this.swigDelegate243, this.swigDelegate244, this.swigDelegate245, this.swigDelegate246, this.swigDelegate247, this.swigDelegate248, this.swigDelegate249, this.swigDelegate250, this.swigDelegate251, this.swigDelegate252, this.swigDelegate253, this.swigDelegate254, this.swigDelegate255, this.swigDelegate256, this.swigDelegate257, this.swigDelegate258, this.swigDelegate259, this.swigDelegate260, this.swigDelegate261, this.swigDelegate262, this.swigDelegate263, this.swigDelegate264, this.swigDelegate265, this.swigDelegate266, this.swigDelegate267, this.swigDelegate268, this.swigDelegate269, this.swigDelegate270, this.swigDelegate271, this.swigDelegate272, this.swigDelegate273, this.swigDelegate274, this.swigDelegate275, this.swigDelegate276, this.swigDelegate277, this.swigDelegate278, this.swigDelegate279, this.swigDelegate280, this.swigDelegate281, this.swigDelegate282, this.swigDelegate283, this.swigDelegate284, this.swigDelegate285, this.swigDelegate286, this.swigDelegate287, this.swigDelegate288, this.swigDelegate289, this.swigDelegate290, this.swigDelegate291, this.swigDelegate292, this.swigDelegate293, this.swigDelegate294, this.swigDelegate295, this.swigDelegate296, this.swigDelegate297, this.swigDelegate298, this.swigDelegate299, this.swigDelegate300, this.swigDelegate301, this.swigDelegate302, this.swigDelegate303, this.swigDelegate304, this.swigDelegate305, this.swigDelegate306, this.swigDelegate307, this.swigDelegate308, this.swigDelegate309, this.swigDelegate310, this.swigDelegate311, this.swigDelegate312, this.swigDelegate313, this.swigDelegate314, this.swigDelegate315, this.swigDelegate316, this.swigDelegate317, this.swigDelegate318, this.swigDelegate319, this.swigDelegate320, this.swigDelegate321, this.swigDelegate322, this.swigDelegate323, this.swigDelegate324, this.swigDelegate325, this.swigDelegate326, this.swigDelegate327, this.swigDelegate328, this.swigDelegate329, this.swigDelegate330, this.swigDelegate331, this.swigDelegate332, this.swigDelegate333, this.swigDelegate334, this.swigDelegate335, this.swigDelegate336, this.swigDelegate337, this.swigDelegate338, this.swigDelegate339, this.swigDelegate340, this.swigDelegate341, this.swigDelegate342, this.swigDelegate343, this.swigDelegate344, this.swigDelegate345, this.swigDelegate346, this.swigDelegate347, this.swigDelegate348, this.swigDelegate349, this.swigDelegate350, this.swigDelegate351, this.swigDelegate352, this.swigDelegate353, this.swigDelegate354, this.swigDelegate355, this.swigDelegate356, this.swigDelegate357, this.swigDelegate358, this.swigDelegate359, this.swigDelegate360, this.swigDelegate361, this.swigDelegate362, this.swigDelegate363, this.swigDelegate364, this.swigDelegate365, this.swigDelegate366, this.swigDelegate367, this.swigDelegate368, this.swigDelegate369, this.swigDelegate370, this.swigDelegate371, this.swigDelegate372, this.swigDelegate373, this.swigDelegate374, this.swigDelegate375, this.swigDelegate376, this.swigDelegate377, this.swigDelegate378, this.swigDelegate379, this.swigDelegate380, this.swigDelegate381, this.swigDelegate382, this.swigDelegate383, this.swigDelegate384, this.swigDelegate385, this.swigDelegate386, this.swigDelegate387, this.swigDelegate388, this.swigDelegate389, this.swigDelegate390, this.swigDelegate391, this.swigDelegate392, this.swigDelegate393, this.swigDelegate394, this.swigDelegate395, this.swigDelegate396, this.swigDelegate397, this.swigDelegate398, this.swigDelegate399, this.swigDelegate400, this.swigDelegate401, this.swigDelegate402, this.swigDelegate403, this.swigDelegate404, this.swigDelegate405, this.swigDelegate406, this.swigDelegate407, this.swigDelegate408, this.swigDelegate409, this.swigDelegate410, this.swigDelegate411, this.swigDelegate412, this.swigDelegate413, this.swigDelegate414, this.swigDelegate415, this.swigDelegate416, this.swigDelegate417, this.swigDelegate418, this.swigDelegate419, this.swigDelegate420, this.swigDelegate421, this.swigDelegate422, this.swigDelegate423, this.swigDelegate424, this.swigDelegate425, this.swigDelegate426, this.swigDelegate427, this.swigDelegate428, this.swigDelegate429, this.swigDelegate430, this.swigDelegate431, this.swigDelegate432, this.swigDelegate433, this.swigDelegate434, this.swigDelegate435, this.swigDelegate436, this.swigDelegate437, this.swigDelegate438, this.swigDelegate439, this.swigDelegate440, this.swigDelegate441, this.swigDelegate442, this.swigDelegate443, this.swigDelegate444, this.swigDelegate445, this.swigDelegate446, this.swigDelegate447, this.swigDelegate448, this.swigDelegate449, this.swigDelegate450, this.swigDelegate451, this.swigDelegate452, this.swigDelegate453, this.swigDelegate454, this.swigDelegate455, this.swigDelegate456, this.swigDelegate457, this.swigDelegate458, this.swigDelegate459, this.swigDelegate460, this.swigDelegate461, this.swigDelegate462, this.swigDelegate463, this.swigDelegate464, this.swigDelegate465, this.swigDelegate466, this.swigDelegate467, this.swigDelegate468, this.swigDelegate469, this.swigDelegate470, this.swigDelegate471, this.swigDelegate472, this.swigDelegate473, this.swigDelegate474, this.swigDelegate475, this.swigDelegate476, this.swigDelegate477, this.swigDelegate478, this.swigDelegate479, this.swigDelegate480, this.swigDelegate481, this.swigDelegate482, this.swigDelegate483, this.swigDelegate484, this.swigDelegate485, this.swigDelegate486, this.swigDelegate487, this.swigDelegate488, this.swigDelegate489, this.swigDelegate490, this.swigDelegate491, this.swigDelegate492, this.swigDelegate493, this.swigDelegate494, this.swigDelegate495, this.swigDelegate496, this.swigDelegate497, this.swigDelegate498, this.swigDelegate499, this.swigDelegate500, this.swigDelegate501, this.swigDelegate502, this.swigDelegate503, this.swigDelegate504, this.swigDelegate505, this.swigDelegate506, this.swigDelegate507, this.swigDelegate508, this.swigDelegate509, this.swigDelegate510);
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0000DE56 File Offset: 0x0000C056
	private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes)
	{
		return base.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, methodTypes, null).DeclaringType.IsSubclassOf(typeof(GLAdapter));
	}

	// Token: 0x06000351 RID: 849 RVA: 0x000044C7 File Offset: 0x000026C7
	private void SwigDirectorSetCurrentThread(uint id)
	{
		this.SetCurrentThread(id);
	}

	// Token: 0x06000352 RID: 850 RVA: 0x0000DE80 File Offset: 0x0000C080
	private void SwigDirectorProcessVertexAttribData(uint index, int size, uint type, uint normalized, int stride, IntPtr pData, uint dataSize, uint dataOffset)
	{
		this.ProcessVertexAttribData(index, size, type, normalized, stride, pData, dataSize, dataOffset);
	}

	// Token: 0x06000353 RID: 851 RVA: 0x0000DEA0 File Offset: 0x0000C0A0
	private void SwigDirectorProcessVertexAttribIData(uint index, int size, uint type, int stride, IntPtr pData, uint dataSize, uint dataOffset)
	{
		this.ProcessVertexAttribIData(index, size, type, stride, pData, dataSize, dataOffset);
	}

	// Token: 0x06000354 RID: 852 RVA: 0x0000DEB3 File Offset: 0x0000C0B3
	private void SwigDirectorProcessFlushMappedBufferRange(uint target, uint offset, uint length, IntPtr pData)
	{
		this.ProcessFlushMappedBufferRange(target, offset, length, pData);
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0000DEC0 File Offset: 0x0000C0C0
	private void SwigDirectorProcessUnmapBuffer(uint target, uint length, IntPtr pData)
	{
		this.ProcessUnmapBuffer(target, length, pData);
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0000DECB File Offset: 0x0000C0CB
	private void SwigDirectorProcess_glActiveTexture(uint texture)
	{
		this.Process_glActiveTexture(texture);
	}

	// Token: 0x06000357 RID: 855 RVA: 0x0000DED4 File Offset: 0x0000C0D4
	private void SwigDirectorProcess_glAttachShader(uint program, uint shader)
	{
		this.Process_glAttachShader(program, shader);
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0000DEDE File Offset: 0x0000C0DE
	private void SwigDirectorProcess_glBindAttribLocation(uint program, uint index, IntPtr pNamePtrData)
	{
		this.Process_glBindAttribLocation(program, index, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000359 RID: 857 RVA: 0x0000DEFF File Offset: 0x0000C0FF
	private void SwigDirectorProcess_glBindBuffer(uint target, uint buffer)
	{
		this.Process_glBindBuffer(target, buffer);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x0000DF09 File Offset: 0x0000C109
	private void SwigDirectorProcess_glBindFramebuffer(uint target, uint framebuffer)
	{
		this.Process_glBindFramebuffer(target, framebuffer);
	}

	// Token: 0x0600035B RID: 859 RVA: 0x0000DF13 File Offset: 0x0000C113
	private void SwigDirectorProcess_glBindRenderbuffer(uint target, uint renderbuffer)
	{
		this.Process_glBindRenderbuffer(target, renderbuffer);
	}

	// Token: 0x0600035C RID: 860 RVA: 0x0000DF1D File Offset: 0x0000C11D
	private void SwigDirectorProcess_glBindTexture(uint target, uint texture)
	{
		this.Process_glBindTexture(target, texture);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x0000DF27 File Offset: 0x0000C127
	private void SwigDirectorProcess_glBlendColor(float red, float green, float blue, float alpha)
	{
		this.Process_glBlendColor(red, green, blue, alpha);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x0000DF34 File Offset: 0x0000C134
	private void SwigDirectorProcess_glBlendEquation(uint mode)
	{
		this.Process_glBlendEquation(mode);
	}

	// Token: 0x0600035F RID: 863 RVA: 0x0000DF3D File Offset: 0x0000C13D
	private void SwigDirectorProcess_glBlendEquationSeparate(uint modeRGB, uint modeAlpha)
	{
		this.Process_glBlendEquationSeparate(modeRGB, modeAlpha);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x0000DF47 File Offset: 0x0000C147
	private void SwigDirectorProcess_glBlendFunc(uint sfactor, uint dfactor)
	{
		this.Process_glBlendFunc(sfactor, dfactor);
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0000DF51 File Offset: 0x0000C151
	private void SwigDirectorProcess_glBlendFuncSeparate(uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
	{
		this.Process_glBlendFuncSeparate(srcRGB, dstRGB, srcAlpha, dstAlpha);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x0000DF5E File Offset: 0x0000C15E
	private void SwigDirectorProcess_glBufferData(uint target, int size, IntPtr pDataPtrData, uint usage)
	{
		this.Process_glBufferData(target, size, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false), usage);
	}

	// Token: 0x06000363 RID: 867 RVA: 0x0000DF81 File Offset: 0x0000C181
	private void SwigDirectorProcess_glBufferSubData(uint target, int offset, int size, IntPtr pDataPtrData)
	{
		this.Process_glBufferSubData(target, offset, size, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x06000364 RID: 868 RVA: 0x0000DFA5 File Offset: 0x0000C1A5
	private void SwigDirectorProcess_glCheckFramebufferStatus(uint returnVal, uint target)
	{
		this.Process_glCheckFramebufferStatus(returnVal, target);
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0000DFAF File Offset: 0x0000C1AF
	private void SwigDirectorProcess_glClear(uint mask)
	{
		this.Process_glClear(mask);
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
	private void SwigDirectorProcess_glClearColor(float red, float green, float blue, float alpha)
	{
		this.Process_glClearColor(red, green, blue, alpha);
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0000DFC5 File Offset: 0x0000C1C5
	private void SwigDirectorProcess_glClearDepthf(float depth)
	{
		this.Process_glClearDepthf(depth);
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0000DFCE File Offset: 0x0000C1CE
	private void SwigDirectorProcess_glClearStencil(int s)
	{
		this.Process_glClearStencil(s);
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0000DFD7 File Offset: 0x0000C1D7
	private void SwigDirectorProcess_glColorMask(int red, int green, int blue, int alpha)
	{
		this.Process_glColorMask(red, green, blue, alpha);
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0000DFE4 File Offset: 0x0000C1E4
	private void SwigDirectorProcess_glCompileShader(uint shader)
	{
		this.Process_glCompileShader(shader);
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0000DFF0 File Offset: 0x0000C1F0
	private void SwigDirectorProcess_glCompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr pDataPtrData)
	{
		this.Process_glCompressedTexImage2D(target, level, internalformat, width, height, border, imageSize, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0000E028 File Offset: 0x0000C228
	private void SwigDirectorProcess_glCompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr pDataPtrData)
	{
		this.Process_glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0000E064 File Offset: 0x0000C264
	private void SwigDirectorProcess_glCopyTexImage2D(uint target, int level, uint internalformat, int x, int y, int width, int height, int border)
	{
		this.Process_glCopyTexImage2D(target, level, internalformat, x, y, width, height, border);
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0000E084 File Offset: 0x0000C284
	private void SwigDirectorProcess_glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height)
	{
		this.Process_glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height);
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
	private void SwigDirectorProcess_glCreateProgram(uint returnVal)
	{
		this.Process_glCreateProgram(returnVal);
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0000E0AD File Offset: 0x0000C2AD
	private void SwigDirectorProcess_glCreateShader(uint returnVal, uint type)
	{
		this.Process_glCreateShader(returnVal, type);
	}

	// Token: 0x06000371 RID: 881 RVA: 0x0000E0B7 File Offset: 0x0000C2B7
	private void SwigDirectorProcess_glCullFace(uint mode)
	{
		this.Process_glCullFace(mode);
	}

	// Token: 0x06000372 RID: 882 RVA: 0x0000E0C0 File Offset: 0x0000C2C0
	private void SwigDirectorProcess_glDeleteBuffers(int n, IntPtr pBuffersPtrData)
	{
		this.Process_glDeleteBuffers(n, (pBuffersPtrData == IntPtr.Zero) ? null : new PointerData(pBuffersPtrData, false));
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0000E0E0 File Offset: 0x0000C2E0
	private void SwigDirectorProcess_glDeleteFramebuffers(int n, IntPtr pFramebuffersPtrData)
	{
		this.Process_glDeleteFramebuffers(n, (pFramebuffersPtrData == IntPtr.Zero) ? null : new PointerData(pFramebuffersPtrData, false));
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0000E100 File Offset: 0x0000C300
	private void SwigDirectorProcess_glDeleteProgram(uint program)
	{
		this.Process_glDeleteProgram(program);
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0000E109 File Offset: 0x0000C309
	private void SwigDirectorProcess_glDeleteRenderbuffers(int n, IntPtr pRenderbuffersPtrData)
	{
		this.Process_glDeleteRenderbuffers(n, (pRenderbuffersPtrData == IntPtr.Zero) ? null : new PointerData(pRenderbuffersPtrData, false));
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0000E129 File Offset: 0x0000C329
	private void SwigDirectorProcess_glDeleteShader(uint shader)
	{
		this.Process_glDeleteShader(shader);
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0000E132 File Offset: 0x0000C332
	private void SwigDirectorProcess_glDeleteTextures(int n, IntPtr pTexturesPtrData)
	{
		this.Process_glDeleteTextures(n, (pTexturesPtrData == IntPtr.Zero) ? null : new PointerData(pTexturesPtrData, false));
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0000E152 File Offset: 0x0000C352
	private void SwigDirectorProcess_glDepthFunc(uint func)
	{
		this.Process_glDepthFunc(func);
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0000E15B File Offset: 0x0000C35B
	private void SwigDirectorProcess_glDepthMask(int flag)
	{
		this.Process_glDepthMask(flag);
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0000E164 File Offset: 0x0000C364
	private void SwigDirectorProcess_glDepthRangef(float n, float f)
	{
		this.Process_glDepthRangef(n, f);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0000E16E File Offset: 0x0000C36E
	private void SwigDirectorProcess_glDetachShader(uint program, uint shader)
	{
		this.Process_glDetachShader(program, shader);
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0000E178 File Offset: 0x0000C378
	private void SwigDirectorProcess_glDisable(uint cap)
	{
		this.Process_glDisable(cap);
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0000E181 File Offset: 0x0000C381
	private void SwigDirectorProcess_glDisableVertexAttribArray(uint index)
	{
		this.Process_glDisableVertexAttribArray(index);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x0000E18A File Offset: 0x0000C38A
	private void SwigDirectorProcess_glDrawArrays(uint mode, int first, int count)
	{
		this.Process_glDrawArrays(mode, first, count);
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0000E195 File Offset: 0x0000C395
	private void SwigDirectorProcess_glDrawElements(uint mode, int count, uint type, IntPtr pIndicesPtrData)
	{
		this.Process_glDrawElements(mode, count, type, (pIndicesPtrData == IntPtr.Zero) ? null : new PointerData(pIndicesPtrData, false));
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0000E1B9 File Offset: 0x0000C3B9
	private void SwigDirectorProcess_glEnable(uint cap)
	{
		this.Process_glEnable(cap);
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0000E1C2 File Offset: 0x0000C3C2
	private void SwigDirectorProcess_glEnableVertexAttribArray(uint index)
	{
		this.Process_glEnableVertexAttribArray(index);
	}

	// Token: 0x06000382 RID: 898 RVA: 0x0000E1CB File Offset: 0x0000C3CB
	private void SwigDirectorProcess_glFinish()
	{
		this.Process_glFinish();
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0000E1D3 File Offset: 0x0000C3D3
	private void SwigDirectorProcess_glFlush()
	{
		this.Process_glFlush();
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0000E1DB File Offset: 0x0000C3DB
	private void SwigDirectorProcess_glFramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer)
	{
		this.Process_glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer);
	}

	// Token: 0x06000385 RID: 901 RVA: 0x0000E1E8 File Offset: 0x0000C3E8
	private void SwigDirectorProcess_glFramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level)
	{
		this.Process_glFramebufferTexture2D(target, attachment, textarget, texture, level);
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0000E1F7 File Offset: 0x0000C3F7
	private void SwigDirectorProcess_glFrontFace(uint mode)
	{
		this.Process_glFrontFace(mode);
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0000E200 File Offset: 0x0000C400
	private void SwigDirectorProcess_glGenBuffers(int n, IntPtr pBuffersPtrData)
	{
		this.Process_glGenBuffers(n, (pBuffersPtrData == IntPtr.Zero) ? null : new PointerData(pBuffersPtrData, false));
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0000E220 File Offset: 0x0000C420
	private void SwigDirectorProcess_glGenerateMipmap(uint target)
	{
		this.Process_glGenerateMipmap(target);
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0000E229 File Offset: 0x0000C429
	private void SwigDirectorProcess_glGenFramebuffers(int n, IntPtr pFramebuffersPtrData)
	{
		this.Process_glGenFramebuffers(n, (pFramebuffersPtrData == IntPtr.Zero) ? null : new PointerData(pFramebuffersPtrData, false));
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0000E249 File Offset: 0x0000C449
	private void SwigDirectorProcess_glGenRenderbuffers(int n, IntPtr pRenderbuffersPtrData)
	{
		this.Process_glGenRenderbuffers(n, (pRenderbuffersPtrData == IntPtr.Zero) ? null : new PointerData(pRenderbuffersPtrData, false));
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0000E269 File Offset: 0x0000C469
	private void SwigDirectorProcess_glGenTextures(int n, IntPtr pTexturesPtrData)
	{
		this.Process_glGenTextures(n, (pTexturesPtrData == IntPtr.Zero) ? null : new PointerData(pTexturesPtrData, false));
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0000E28C File Offset: 0x0000C48C
	private void SwigDirectorProcess_glGetActiveAttrib(uint program, uint index, int bufsize, IntPtr pLengthPtrData, IntPtr pSizePtrData, IntPtr pTypePtrData, IntPtr pNamePtrData)
	{
		this.Process_glGetActiveAttrib(program, index, bufsize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pSizePtrData == IntPtr.Zero) ? null : new PointerData(pSizePtrData, false), (pTypePtrData == IntPtr.Zero) ? null : new PointerData(pTypePtrData, false), (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0000E308 File Offset: 0x0000C508
	private void SwigDirectorProcess_glGetActiveUniform(uint program, uint index, int bufsize, IntPtr pLengthPtrData, IntPtr pSizePtrData, IntPtr pTypePtrData, IntPtr pNamePtrData)
	{
		this.Process_glGetActiveUniform(program, index, bufsize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pSizePtrData == IntPtr.Zero) ? null : new PointerData(pSizePtrData, false), (pTypePtrData == IntPtr.Zero) ? null : new PointerData(pTypePtrData, false), (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x0600038E RID: 910 RVA: 0x0000E382 File Offset: 0x0000C582
	private void SwigDirectorProcess_glGetAttachedShaders(uint program, int maxcount, IntPtr pCountPtrData, IntPtr pShadersPtrData)
	{
		this.Process_glGetAttachedShaders(program, maxcount, (pCountPtrData == IntPtr.Zero) ? null : new PointerData(pCountPtrData, false), (pShadersPtrData == IntPtr.Zero) ? null : new PointerData(pShadersPtrData, false));
	}

	// Token: 0x0600038F RID: 911 RVA: 0x0000E3BC File Offset: 0x0000C5BC
	private void SwigDirectorProcess_glGetAttribLocation(uint returnVal, uint program, IntPtr pNamePtrData)
	{
		this.Process_glGetAttribLocation(returnVal, program, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0000E3DD File Offset: 0x0000C5DD
	private void SwigDirectorProcess_glGetBooleanv(uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetBooleanv(pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0000E3FD File Offset: 0x0000C5FD
	private void SwigDirectorProcess_glGetBufferParameteriv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetBufferParameteriv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0000E41E File Offset: 0x0000C61E
	private void SwigDirectorProcess_glGetError(uint returnVal)
	{
		this.Process_glGetError(returnVal);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0000E427 File Offset: 0x0000C627
	private void SwigDirectorProcess_glGetFloatv(uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetFloatv(pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0000E447 File Offset: 0x0000C647
	private void SwigDirectorProcess_glGetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetFramebufferAttachmentParameteriv(target, attachment, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000395 RID: 917 RVA: 0x0000E46B File Offset: 0x0000C66B
	private void SwigDirectorProcess_glGetIntegerv(uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetIntegerv(pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0000E48B File Offset: 0x0000C68B
	private void SwigDirectorProcess_glGetProgramiv(uint program, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetProgramiv(program, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000397 RID: 919 RVA: 0x0000E4AC File Offset: 0x0000C6AC
	private void SwigDirectorProcess_glGetProgramInfoLog(uint program, int bufsize, IntPtr pLengthPtrData, IntPtr pInfologPtrData)
	{
		this.Process_glGetProgramInfoLog(program, bufsize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pInfologPtrData == IntPtr.Zero) ? null : new PointerData(pInfologPtrData, false));
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0000E4E6 File Offset: 0x0000C6E6
	private void SwigDirectorProcess_glGetRenderbufferParameteriv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetRenderbufferParameteriv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0000E507 File Offset: 0x0000C707
	private void SwigDirectorProcess_glGetShaderiv(uint shader, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetShaderiv(shader, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0000E528 File Offset: 0x0000C728
	private void SwigDirectorProcess_glGetShaderInfoLog(uint shader, int bufsize, IntPtr pLengthPtrData, IntPtr pInfologPtrData)
	{
		this.Process_glGetShaderInfoLog(shader, bufsize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pInfologPtrData == IntPtr.Zero) ? null : new PointerData(pInfologPtrData, false));
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0000E562 File Offset: 0x0000C762
	private void SwigDirectorProcess_glGetShaderPrecisionFormat(uint shadertype, uint precisiontype, IntPtr pRangePtrData, IntPtr pPrecisionPtrData)
	{
		this.Process_glGetShaderPrecisionFormat(shadertype, precisiontype, (pRangePtrData == IntPtr.Zero) ? null : new PointerData(pRangePtrData, false), (pPrecisionPtrData == IntPtr.Zero) ? null : new PointerData(pPrecisionPtrData, false));
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0000E59C File Offset: 0x0000C79C
	private void SwigDirectorProcess_glGetShaderSource(uint shader, int bufsize, IntPtr pLengthPtrData, IntPtr pSourcePtrData)
	{
		this.Process_glGetShaderSource(shader, bufsize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pSourcePtrData == IntPtr.Zero) ? null : new PointerData(pSourcePtrData, false));
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0000E5D6 File Offset: 0x0000C7D6
	private void SwigDirectorProcess_glGetString(IntPtr pReturnPtrData, uint name)
	{
		this.Process_glGetString((pReturnPtrData == IntPtr.Zero) ? null : new PointerData(pReturnPtrData, false), name);
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0000E5F6 File Offset: 0x0000C7F6
	private void SwigDirectorProcess_glGetTexParameterfv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetTexParameterfv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0000E617 File Offset: 0x0000C817
	private void SwigDirectorProcess_glGetTexParameteriv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetTexParameteriv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0000E638 File Offset: 0x0000C838
	private void SwigDirectorProcess_glGetUniformfv(uint program, uint location, IntPtr pParamsPtrData)
	{
		this.Process_glGetUniformfv(program, location, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0000E659 File Offset: 0x0000C859
	private void SwigDirectorProcess_glGetUniformiv(uint program, uint location, IntPtr pParamsPtrData)
	{
		this.Process_glGetUniformiv(program, location, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0000E67A File Offset: 0x0000C87A
	private void SwigDirectorProcess_glGetUniformLocation(uint returnVal, uint program, IntPtr pNamePtrData)
	{
		this.Process_glGetUniformLocation(returnVal, program, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0000E69B File Offset: 0x0000C89B
	private void SwigDirectorProcess_glGetVertexAttribfv(uint index, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetVertexAttribfv(index, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0000E6BC File Offset: 0x0000C8BC
	private void SwigDirectorProcess_glGetVertexAttribiv(uint index, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetVertexAttribiv(index, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0000E6DD File Offset: 0x0000C8DD
	private void SwigDirectorProcess_glGetVertexAttribPointerv(uint index, uint pname, IntPtr pPointerPtrData)
	{
		this.Process_glGetVertexAttribPointerv(index, pname, (pPointerPtrData == IntPtr.Zero) ? null : new PointerData(pPointerPtrData, false));
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0000E6FE File Offset: 0x0000C8FE
	private void SwigDirectorProcess_glHint(uint target, uint mode)
	{
		this.Process_glHint(target, mode);
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0000E708 File Offset: 0x0000C908
	private void SwigDirectorProcess_glIsBuffer(int returnVal, uint buffer)
	{
		this.Process_glIsBuffer(returnVal, buffer);
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0000E712 File Offset: 0x0000C912
	private void SwigDirectorProcess_glIsEnabled(int returnVal, uint cap)
	{
		this.Process_glIsEnabled(returnVal, cap);
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0000E71C File Offset: 0x0000C91C
	private void SwigDirectorProcess_glIsFramebuffer(int returnVal, uint framebuffer)
	{
		this.Process_glIsFramebuffer(returnVal, framebuffer);
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0000E726 File Offset: 0x0000C926
	private void SwigDirectorProcess_glIsProgram(int returnVal, uint program)
	{
		this.Process_glIsProgram(returnVal, program);
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0000E730 File Offset: 0x0000C930
	private void SwigDirectorProcess_glIsRenderbuffer(int returnVal, uint renderbuffer)
	{
		this.Process_glIsRenderbuffer(returnVal, renderbuffer);
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0000E73A File Offset: 0x0000C93A
	private void SwigDirectorProcess_glIsShader(int returnVal, uint shader)
	{
		this.Process_glIsShader(returnVal, shader);
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0000E744 File Offset: 0x0000C944
	private void SwigDirectorProcess_glIsTexture(int returnVal, uint texture)
	{
		this.Process_glIsTexture(returnVal, texture);
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0000E74E File Offset: 0x0000C94E
	private void SwigDirectorProcess_glLineWidth(float width)
	{
		this.Process_glLineWidth(width);
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0000E757 File Offset: 0x0000C957
	private void SwigDirectorProcess_glLinkProgram(uint program)
	{
		this.Process_glLinkProgram(program);
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0000E760 File Offset: 0x0000C960
	private void SwigDirectorProcess_glPixelStorei(uint pname, int param)
	{
		this.Process_glPixelStorei(pname, param);
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0000E76A File Offset: 0x0000C96A
	private void SwigDirectorProcess_glPolygonOffset(float factor, float units)
	{
		this.Process_glPolygonOffset(factor, units);
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0000E774 File Offset: 0x0000C974
	private void SwigDirectorProcess_glReadPixels(int x, int y, int width, int height, uint format, uint type, IntPtr pPixelsPtrData)
	{
		this.Process_glReadPixels(x, y, width, height, format, type, (pPixelsPtrData == IntPtr.Zero) ? null : new PointerData(pPixelsPtrData, false));
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0000E7A9 File Offset: 0x0000C9A9
	private void SwigDirectorProcess_glReleaseShaderCompiler()
	{
		this.Process_glReleaseShaderCompiler();
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0000E7B1 File Offset: 0x0000C9B1
	private void SwigDirectorProcess_glRenderbufferStorage(uint target, uint internalformat, int width, int height)
	{
		this.Process_glRenderbufferStorage(target, internalformat, width, height);
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0000E7BE File Offset: 0x0000C9BE
	private void SwigDirectorProcess_glSampleCoverage(float value, int invert)
	{
		this.Process_glSampleCoverage(value, invert);
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x0000E7C8 File Offset: 0x0000C9C8
	private void SwigDirectorProcess_glScissor(int x, int y, int width, int height)
	{
		this.Process_glScissor(x, y, width, height);
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x0000E7D5 File Offset: 0x0000C9D5
	private void SwigDirectorProcess_glShaderBinary(int n, IntPtr pShadersPtrData, uint binaryformat, IntPtr pBinaryPtrData, int length)
	{
		this.Process_glShaderBinary(n, (pShadersPtrData == IntPtr.Zero) ? null : new PointerData(pShadersPtrData, false), binaryformat, (pBinaryPtrData == IntPtr.Zero) ? null : new PointerData(pBinaryPtrData, false), length);
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0000E811 File Offset: 0x0000CA11
	private void SwigDirectorProcess_glShaderSource(uint shader, int count, IntPtr pStrPtrData, IntPtr pLengthPtrData)
	{
		this.Process_glShaderSource(shader, count, (pStrPtrData == IntPtr.Zero) ? null : new PointerData(pStrPtrData, false), (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false));
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0000E84B File Offset: 0x0000CA4B
	private void SwigDirectorProcess_glStencilFunc(uint func, int arg1, uint mask)
	{
		this.Process_glStencilFunc(func, arg1, mask);
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0000E856 File Offset: 0x0000CA56
	private void SwigDirectorProcess_glStencilFuncSeparate(uint face, uint func, int arg2, uint mask)
	{
		this.Process_glStencilFuncSeparate(face, func, arg2, mask);
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0000E863 File Offset: 0x0000CA63
	private void SwigDirectorProcess_glStencilMask(uint mask)
	{
		this.Process_glStencilMask(mask);
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0000E86C File Offset: 0x0000CA6C
	private void SwigDirectorProcess_glStencilMaskSeparate(uint face, uint mask)
	{
		this.Process_glStencilMaskSeparate(face, mask);
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0000E876 File Offset: 0x0000CA76
	private void SwigDirectorProcess_glStencilOp(uint fail, uint zfail, uint zpass)
	{
		this.Process_glStencilOp(fail, zfail, zpass);
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0000E881 File Offset: 0x0000CA81
	private void SwigDirectorProcess_glStencilOpSeparate(uint face, uint fail, uint zfail, uint zpass)
	{
		this.Process_glStencilOpSeparate(face, fail, zfail, zpass);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0000E890 File Offset: 0x0000CA90
	private void SwigDirectorProcess_glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pPixelsPtrData)
	{
		this.Process_glTexImage2D(target, level, internalformat, width, height, border, format, type, (pPixelsPtrData == IntPtr.Zero) ? null : new PointerData(pPixelsPtrData, false));
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0000E8C9 File Offset: 0x0000CAC9
	private void SwigDirectorProcess_glTexParameterf(uint target, uint pname, float param)
	{
		this.Process_glTexParameterf(target, pname, param);
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0000E8D4 File Offset: 0x0000CAD4
	private void SwigDirectorProcess_glTexParameterfv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glTexParameterfv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0000E8F5 File Offset: 0x0000CAF5
	private void SwigDirectorProcess_glTexParameteri(uint target, uint pname, int param)
	{
		this.Process_glTexParameteri(target, pname, param);
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0000E900 File Offset: 0x0000CB00
	private void SwigDirectorProcess_glTexParameteriv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glTexParameteriv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0000E924 File Offset: 0x0000CB24
	private void SwigDirectorProcess_glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pPixelsPtrData)
	{
		this.Process_glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, (pPixelsPtrData == IntPtr.Zero) ? null : new PointerData(pPixelsPtrData, false));
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0000E95D File Offset: 0x0000CB5D
	private void SwigDirectorProcess_glUniform1f(uint location, float x)
	{
		this.Process_glUniform1f(location, x);
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0000E967 File Offset: 0x0000CB67
	private void SwigDirectorProcess_glUniform1fv(uint location, int count, IntPtr pVPtrData)
	{
		this.Process_glUniform1fv(location, count, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0000E988 File Offset: 0x0000CB88
	private void SwigDirectorProcess_glUniform1i(uint location, int x)
	{
		this.Process_glUniform1i(location, x);
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0000E992 File Offset: 0x0000CB92
	private void SwigDirectorProcess_glUniform1iv(uint location, int count, IntPtr pVPtrData)
	{
		this.Process_glUniform1iv(location, count, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x0000E9B3 File Offset: 0x0000CBB3
	private void SwigDirectorProcess_glUniform2f(uint location, float x, float y)
	{
		this.Process_glUniform2f(location, x, y);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0000E9BE File Offset: 0x0000CBBE
	private void SwigDirectorProcess_glUniform2fv(uint location, int count, IntPtr pVPtrData)
	{
		this.Process_glUniform2fv(location, count, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0000E9DF File Offset: 0x0000CBDF
	private void SwigDirectorProcess_glUniform2i(uint location, int x, int y)
	{
		this.Process_glUniform2i(location, x, y);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0000E9EA File Offset: 0x0000CBEA
	private void SwigDirectorProcess_glUniform2iv(uint location, int count, IntPtr pVPtrData)
	{
		this.Process_glUniform2iv(location, count, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0000EA0B File Offset: 0x0000CC0B
	private void SwigDirectorProcess_glUniform3f(uint location, float x, float y, float z)
	{
		this.Process_glUniform3f(location, x, y, z);
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0000EA18 File Offset: 0x0000CC18
	private void SwigDirectorProcess_glUniform3fv(uint location, int count, IntPtr pVPtrData)
	{
		this.Process_glUniform3fv(location, count, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0000EA39 File Offset: 0x0000CC39
	private void SwigDirectorProcess_glUniform3i(uint location, int x, int y, int z)
	{
		this.Process_glUniform3i(location, x, y, z);
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0000EA46 File Offset: 0x0000CC46
	private void SwigDirectorProcess_glUniform3iv(uint location, int count, IntPtr pVPtrData)
	{
		this.Process_glUniform3iv(location, count, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0000EA67 File Offset: 0x0000CC67
	private void SwigDirectorProcess_glUniform4f(uint location, float x, float y, float z, float w)
	{
		this.Process_glUniform4f(location, x, y, z, w);
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0000EA76 File Offset: 0x0000CC76
	private void SwigDirectorProcess_glUniform4fv(uint location, int count, IntPtr pVPtrData)
	{
		this.Process_glUniform4fv(location, count, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0000EA97 File Offset: 0x0000CC97
	private void SwigDirectorProcess_glUniform4i(uint location, int x, int y, int z, int w)
	{
		this.Process_glUniform4i(location, x, y, z, w);
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0000EAA6 File Offset: 0x0000CCA6
	private void SwigDirectorProcess_glUniform4iv(uint location, int count, IntPtr pVPtrData)
	{
		this.Process_glUniform4iv(location, count, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0000EAC7 File Offset: 0x0000CCC7
	private void SwigDirectorProcess_glUniformMatrix2fv(uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glUniformMatrix2fv(location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0000EAEB File Offset: 0x0000CCEB
	private void SwigDirectorProcess_glUniformMatrix3fv(uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glUniformMatrix3fv(location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0000EB0F File Offset: 0x0000CD0F
	private void SwigDirectorProcess_glUniformMatrix4fv(uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glUniformMatrix4fv(location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0000EB33 File Offset: 0x0000CD33
	private void SwigDirectorProcess_glUseProgram(uint program)
	{
		this.Process_glUseProgram(program);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0000EB3C File Offset: 0x0000CD3C
	private void SwigDirectorProcess_glValidateProgram(uint program)
	{
		this.Process_glValidateProgram(program);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0000EB45 File Offset: 0x0000CD45
	private void SwigDirectorProcess_glVertexAttrib1f(uint indx, float x)
	{
		this.Process_glVertexAttrib1f(indx, x);
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0000EB4F File Offset: 0x0000CD4F
	private void SwigDirectorProcess_glVertexAttrib1fv(uint indx, IntPtr pValuesPtrData)
	{
		this.Process_glVertexAttrib1fv(indx, (pValuesPtrData == IntPtr.Zero) ? null : new PointerData(pValuesPtrData, false));
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0000EB6F File Offset: 0x0000CD6F
	private void SwigDirectorProcess_glVertexAttrib2f(uint indx, float x, float y)
	{
		this.Process_glVertexAttrib2f(indx, x, y);
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0000EB7A File Offset: 0x0000CD7A
	private void SwigDirectorProcess_glVertexAttrib2fv(uint indx, IntPtr pValuesPtrData)
	{
		this.Process_glVertexAttrib2fv(indx, (pValuesPtrData == IntPtr.Zero) ? null : new PointerData(pValuesPtrData, false));
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0000EB9A File Offset: 0x0000CD9A
	private void SwigDirectorProcess_glVertexAttrib3f(uint indx, float x, float y, float z)
	{
		this.Process_glVertexAttrib3f(indx, x, y, z);
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0000EBA7 File Offset: 0x0000CDA7
	private void SwigDirectorProcess_glVertexAttrib3fv(uint indx, IntPtr pValuesPtrData)
	{
		this.Process_glVertexAttrib3fv(indx, (pValuesPtrData == IntPtr.Zero) ? null : new PointerData(pValuesPtrData, false));
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x0000EBC7 File Offset: 0x0000CDC7
	private void SwigDirectorProcess_glVertexAttrib4f(uint indx, float x, float y, float z, float w)
	{
		this.Process_glVertexAttrib4f(indx, x, y, z, w);
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0000EBD6 File Offset: 0x0000CDD6
	private void SwigDirectorProcess_glVertexAttrib4fv(uint indx, IntPtr pValuesPtrData)
	{
		this.Process_glVertexAttrib4fv(indx, (pValuesPtrData == IntPtr.Zero) ? null : new PointerData(pValuesPtrData, false));
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x0000EBF6 File Offset: 0x0000CDF6
	private void SwigDirectorProcess_glVertexAttribPointer(uint indx, int size, uint type, int normalized, int stride, IntPtr pPtrPtrData)
	{
		this.Process_glVertexAttribPointer(indx, size, type, normalized, stride, (pPtrPtrData == IntPtr.Zero) ? null : new PointerData(pPtrPtrData, false));
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x0000EC1E File Offset: 0x0000CE1E
	private void SwigDirectorProcess_glViewport(int x, int y, int width, int height)
	{
		this.Process_glViewport(x, y, width, height);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x0000EC2B File Offset: 0x0000CE2B
	private void SwigDirectorProcess_glReadBuffer(uint mode)
	{
		this.Process_glReadBuffer(mode);
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0000EC34 File Offset: 0x0000CE34
	private void SwigDirectorProcess_glDrawRangeElements(uint mode, uint start, uint end, int count, uint type, IntPtr pIndicesPtrData)
	{
		this.Process_glDrawRangeElements(mode, start, end, count, type, (pIndicesPtrData == IntPtr.Zero) ? null : new PointerData(pIndicesPtrData, false));
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x0000EC5C File Offset: 0x0000CE5C
	private void SwigDirectorProcess_glTexImage3D(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pPixelsPtrData)
	{
		this.Process_glTexImage3D(target, level, internalformat, width, height, depth, border, format, type, (pPixelsPtrData == IntPtr.Zero) ? null : new PointerData(pPixelsPtrData, false));
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0000EC98 File Offset: 0x0000CE98
	private void SwigDirectorProcess_glTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pPixelsPtrData)
	{
		this.Process_glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, (pPixelsPtrData == IntPtr.Zero) ? null : new PointerData(pPixelsPtrData, false));
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x0000ECD8 File Offset: 0x0000CED8
	private void SwigDirectorProcess_glCopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height)
	{
		this.Process_glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width, height);
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0000ECFC File Offset: 0x0000CEFC
	private void SwigDirectorProcess_glCompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr pDataPtrData)
	{
		this.Process_glCompressedTexImage3D(target, level, internalformat, width, height, depth, border, imageSize, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0000ED38 File Offset: 0x0000CF38
	private void SwigDirectorProcess_glCompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr pDataPtrData)
	{
		this.Process_glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0000ED75 File Offset: 0x0000CF75
	private void SwigDirectorProcess_glGenQueries(int n, IntPtr pIdsPtrData)
	{
		this.Process_glGenQueries(n, (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false));
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0000ED95 File Offset: 0x0000CF95
	private void SwigDirectorProcess_glDeleteQueries(int n, IntPtr pIdsPtrData)
	{
		this.Process_glDeleteQueries(n, (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false));
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0000EDB5 File Offset: 0x0000CFB5
	private void SwigDirectorProcess_glIsQuery(int returnVal, uint id)
	{
		this.Process_glIsQuery(returnVal, id);
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0000EDBF File Offset: 0x0000CFBF
	private void SwigDirectorProcess_glBeginQuery(uint target, uint id)
	{
		this.Process_glBeginQuery(target, id);
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0000EDC9 File Offset: 0x0000CFC9
	private void SwigDirectorProcess_glEndQuery(uint target)
	{
		this.Process_glEndQuery(target);
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0000EDD2 File Offset: 0x0000CFD2
	private void SwigDirectorProcess_glGetQueryiv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetQueryiv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0000EDF3 File Offset: 0x0000CFF3
	private void SwigDirectorProcess_glGetQueryObjectuiv(uint id, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetQueryObjectuiv(id, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0000EE14 File Offset: 0x0000D014
	private void SwigDirectorProcess_glUnmapBuffer(int returnVal, uint target)
	{
		this.Process_glUnmapBuffer(returnVal, target);
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0000EE1E File Offset: 0x0000D01E
	private void SwigDirectorProcess_glGetBufferPointerv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetBufferPointerv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0000EE3F File Offset: 0x0000D03F
	private void SwigDirectorProcess_glDrawBuffers(int n, IntPtr pBufsPtrData)
	{
		this.Process_glDrawBuffers(n, (pBufsPtrData == IntPtr.Zero) ? null : new PointerData(pBufsPtrData, false));
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0000EE5F File Offset: 0x0000D05F
	private void SwigDirectorProcess_glUniformMatrix2x3fv(uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glUniformMatrix2x3fv(location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0000EE83 File Offset: 0x0000D083
	private void SwigDirectorProcess_glUniformMatrix3x2fv(uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glUniformMatrix3x2fv(location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0000EEA7 File Offset: 0x0000D0A7
	private void SwigDirectorProcess_glUniformMatrix2x4fv(uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glUniformMatrix2x4fv(location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0000EECB File Offset: 0x0000D0CB
	private void SwigDirectorProcess_glUniformMatrix4x2fv(uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glUniformMatrix4x2fv(location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0000EEEF File Offset: 0x0000D0EF
	private void SwigDirectorProcess_glUniformMatrix3x4fv(uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glUniformMatrix3x4fv(location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0000EF13 File Offset: 0x0000D113
	private void SwigDirectorProcess_glUniformMatrix4x3fv(uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glUniformMatrix4x3fv(location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0000EF38 File Offset: 0x0000D138
	private void SwigDirectorProcess_glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter)
	{
		this.Process_glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0000EF5C File Offset: 0x0000D15C
	private void SwigDirectorProcess_glRenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height)
	{
		this.Process_glRenderbufferStorageMultisample(target, samples, internalformat, width, height);
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0000EF6B File Offset: 0x0000D16B
	private void SwigDirectorProcess_glFramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer)
	{
		this.Process_glFramebufferTextureLayer(target, attachment, texture, level, layer);
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0000EF7A File Offset: 0x0000D17A
	private void SwigDirectorProcess_glMapBufferRange(IntPtr pReturnPtrData, uint target, int offset, int length, uint access)
	{
		this.Process_glMapBufferRange((pReturnPtrData == IntPtr.Zero) ? null : new PointerData(pReturnPtrData, false), target, offset, length, access);
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0000EF9F File Offset: 0x0000D19F
	private void SwigDirectorProcess_glFlushMappedBufferRange(uint target, int offset, int length)
	{
		this.Process_glFlushMappedBufferRange(target, offset, length);
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0000EFAA File Offset: 0x0000D1AA
	private void SwigDirectorProcess_glBindVertexArray(uint array)
	{
		this.Process_glBindVertexArray(array);
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0000EFB3 File Offset: 0x0000D1B3
	private void SwigDirectorProcess_glDeleteVertexArrays(int n, IntPtr pArraysPtrData)
	{
		this.Process_glDeleteVertexArrays(n, (pArraysPtrData == IntPtr.Zero) ? null : new PointerData(pArraysPtrData, false));
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0000EFD3 File Offset: 0x0000D1D3
	private void SwigDirectorProcess_glGenVertexArrays(int n, IntPtr pArraysPtrData)
	{
		this.Process_glGenVertexArrays(n, (pArraysPtrData == IntPtr.Zero) ? null : new PointerData(pArraysPtrData, false));
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0000EFF3 File Offset: 0x0000D1F3
	private void SwigDirectorProcess_glIsVertexArray(int returnVal, uint array)
	{
		this.Process_glIsVertexArray(returnVal, array);
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0000EFFD File Offset: 0x0000D1FD
	private void SwigDirectorProcess_glGetIntegeri_v(uint target, uint index, IntPtr pDataPtrData)
	{
		this.Process_glGetIntegeri_v(target, index, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0000F01E File Offset: 0x0000D21E
	private void SwigDirectorProcess_glGetBooleani_v(uint target, uint index, IntPtr pDataPtrData)
	{
		this.Process_glGetBooleani_v(target, index, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0000F03F File Offset: 0x0000D23F
	private void SwigDirectorProcess_glBeginTransformFeedback(uint primitiveMode)
	{
		this.Process_glBeginTransformFeedback(primitiveMode);
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0000F048 File Offset: 0x0000D248
	private void SwigDirectorProcess_glEndTransformFeedback()
	{
		this.Process_glEndTransformFeedback();
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0000F050 File Offset: 0x0000D250
	private void SwigDirectorProcess_glBindBufferRange(uint target, uint index, uint buffer, int offset, int size)
	{
		this.Process_glBindBufferRange(target, index, buffer, offset, size);
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0000F05F File Offset: 0x0000D25F
	private void SwigDirectorProcess_glBindBufferBase(uint target, uint index, uint buffer)
	{
		this.Process_glBindBufferBase(target, index, buffer);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0000F06A File Offset: 0x0000D26A
	private void SwigDirectorProcess_glTransformFeedbackVaryings(uint program, int count, IntPtr pVaryingsPtrData, uint bufferMode)
	{
		this.Process_glTransformFeedbackVaryings(program, count, (pVaryingsPtrData == IntPtr.Zero) ? null : new PointerData(pVaryingsPtrData, false), bufferMode);
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0000F090 File Offset: 0x0000D290
	private void SwigDirectorProcess_glGetTransformFeedbackVarying(uint program, uint index, int bufSize, IntPtr pLengthPtrData, IntPtr pSizePtrData, IntPtr pTypePtrData, IntPtr pNamePtrData)
	{
		this.Process_glGetTransformFeedbackVarying(program, index, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pSizePtrData == IntPtr.Zero) ? null : new PointerData(pSizePtrData, false), (pTypePtrData == IntPtr.Zero) ? null : new PointerData(pTypePtrData, false), (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0000F10A File Offset: 0x0000D30A
	private void SwigDirectorProcess_glVertexAttribIPointer(uint index, int size, uint type, int stride, IntPtr pPointerPtrData)
	{
		this.Process_glVertexAttribIPointer(index, size, type, stride, (pPointerPtrData == IntPtr.Zero) ? null : new PointerData(pPointerPtrData, false));
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x0000F130 File Offset: 0x0000D330
	private void SwigDirectorProcess_glGetVertexAttribIiv(uint index, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetVertexAttribIiv(index, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x0000F151 File Offset: 0x0000D351
	private void SwigDirectorProcess_glGetVertexAttribIuiv(uint index, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetVertexAttribIuiv(index, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0000F172 File Offset: 0x0000D372
	private void SwigDirectorProcess_glVertexAttribI4i(uint index, int x, int y, int z, int w)
	{
		this.Process_glVertexAttribI4i(index, x, y, z, w);
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x0000F181 File Offset: 0x0000D381
	private void SwigDirectorProcess_glVertexAttribI4ui(uint index, uint x, uint y, uint z, uint w)
	{
		this.Process_glVertexAttribI4ui(index, x, y, z, w);
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0000F190 File Offset: 0x0000D390
	private void SwigDirectorProcess_glVertexAttribI4iv(uint index, IntPtr pVPtrData)
	{
		this.Process_glVertexAttribI4iv(index, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
	private void SwigDirectorProcess_glVertexAttribI4uiv(uint index, IntPtr pVPtrData)
	{
		this.Process_glVertexAttribI4uiv(index, (pVPtrData == IntPtr.Zero) ? null : new PointerData(pVPtrData, false));
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0000F1D0 File Offset: 0x0000D3D0
	private void SwigDirectorProcess_glGetUniformuiv(uint program, uint location, IntPtr pParamsPtrData)
	{
		this.Process_glGetUniformuiv(program, location, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0000F1F1 File Offset: 0x0000D3F1
	private void SwigDirectorProcess_glGetFragDataLocation(uint returnVal, uint program, IntPtr pNamePtrData)
	{
		this.Process_glGetFragDataLocation(returnVal, program, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0000F212 File Offset: 0x0000D412
	private void SwigDirectorProcess_glUniform1ui(uint location, uint v0)
	{
		this.Process_glUniform1ui(location, v0);
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0000F21C File Offset: 0x0000D41C
	private void SwigDirectorProcess_glUniform2ui(uint location, uint v0, uint v1)
	{
		this.Process_glUniform2ui(location, v0, v1);
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0000F227 File Offset: 0x0000D427
	private void SwigDirectorProcess_glUniform3ui(uint location, uint v0, uint v1, uint v2)
	{
		this.Process_glUniform3ui(location, v0, v1, v2);
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x0000F234 File Offset: 0x0000D434
	private void SwigDirectorProcess_glUniform4ui(uint location, uint v0, uint v1, uint v2, uint v3)
	{
		this.Process_glUniform4ui(location, v0, v1, v2, v3);
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0000F243 File Offset: 0x0000D443
	private void SwigDirectorProcess_glUniform1uiv(uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glUniform1uiv(location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0000F264 File Offset: 0x0000D464
	private void SwigDirectorProcess_glUniform2uiv(uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glUniform2uiv(location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0000F285 File Offset: 0x0000D485
	private void SwigDirectorProcess_glUniform3uiv(uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glUniform3uiv(location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0000F2A6 File Offset: 0x0000D4A6
	private void SwigDirectorProcess_glUniform4uiv(uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glUniform4uiv(location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0000F2C7 File Offset: 0x0000D4C7
	private void SwigDirectorProcess_glClearBufferiv(uint buffer, int drawbuffer, IntPtr pValuePtrData)
	{
		this.Process_glClearBufferiv(buffer, drawbuffer, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0000F2E8 File Offset: 0x0000D4E8
	private void SwigDirectorProcess_glClearBufferuiv(uint buffer, int drawbuffer, IntPtr pValuePtrData)
	{
		this.Process_glClearBufferuiv(buffer, drawbuffer, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0000F309 File Offset: 0x0000D509
	private void SwigDirectorProcess_glClearBufferfv(uint buffer, int drawbuffer, IntPtr pValuePtrData)
	{
		this.Process_glClearBufferfv(buffer, drawbuffer, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0000F32A File Offset: 0x0000D52A
	private void SwigDirectorProcess_glClearBufferfi(uint buffer, int drawbuffer, float depth, int stencil)
	{
		this.Process_glClearBufferfi(buffer, drawbuffer, depth, stencil);
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0000F337 File Offset: 0x0000D537
	private void SwigDirectorProcess_glGetStringi(IntPtr pReturnPtrData, uint name, uint index)
	{
		this.Process_glGetStringi((pReturnPtrData == IntPtr.Zero) ? null : new PointerData(pReturnPtrData, false), name, index);
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0000F358 File Offset: 0x0000D558
	private void SwigDirectorProcess_glCopyBufferSubData(uint readTarget, uint writeTarget, int readOffset, int writeOffset, int size)
	{
		this.Process_glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size);
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0000F367 File Offset: 0x0000D567
	private void SwigDirectorProcess_glGetUniformIndices(uint program, int uniformCount, IntPtr pUniformNamesPtrData, IntPtr pUniformIndicesPtrData)
	{
		this.Process_glGetUniformIndices(program, uniformCount, (pUniformNamesPtrData == IntPtr.Zero) ? null : new PointerData(pUniformNamesPtrData, false), (pUniformIndicesPtrData == IntPtr.Zero) ? null : new PointerData(pUniformIndicesPtrData, false));
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0000F3A1 File Offset: 0x0000D5A1
	private void SwigDirectorProcess_glGetActiveUniformsiv(uint program, int uniformCount, IntPtr pUniformIndicesPtrData, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetActiveUniformsiv(program, uniformCount, (pUniformIndicesPtrData == IntPtr.Zero) ? null : new PointerData(pUniformIndicesPtrData, false), pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0000F3DD File Offset: 0x0000D5DD
	private void SwigDirectorProcess_glGetUniformBlockIndex(uint returnVal, uint program, IntPtr pUniformBlockNamePtrData)
	{
		this.Process_glGetUniformBlockIndex(returnVal, program, (pUniformBlockNamePtrData == IntPtr.Zero) ? null : new PointerData(pUniformBlockNamePtrData, false));
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0000F3FE File Offset: 0x0000D5FE
	private void SwigDirectorProcess_glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0000F422 File Offset: 0x0000D622
	private void SwigDirectorProcess_glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, IntPtr pLengthPtrData, IntPtr pUniformBlockNamePtrData)
	{
		this.Process_glGetActiveUniformBlockName(program, uniformBlockIndex, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pUniformBlockNamePtrData == IntPtr.Zero) ? null : new PointerData(pUniformBlockNamePtrData, false));
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x0000F45F File Offset: 0x0000D65F
	private void SwigDirectorProcess_glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding)
	{
		this.Process_glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x0000F46A File Offset: 0x0000D66A
	private void SwigDirectorProcess_glDrawArraysInstanced(uint mode, int first, int count, int instanceCount)
	{
		this.Process_glDrawArraysInstanced(mode, first, count, instanceCount);
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0000F477 File Offset: 0x0000D677
	private void SwigDirectorProcess_glDrawElementsInstanced(uint mode, int count, uint type, IntPtr pIndicesPtrData, int instanceCount)
	{
		this.Process_glDrawElementsInstanced(mode, count, type, (pIndicesPtrData == IntPtr.Zero) ? null : new PointerData(pIndicesPtrData, false), instanceCount);
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0000F49D File Offset: 0x0000D69D
	private void SwigDirectorProcess_glFenceSync(uint returnVal, uint condition, uint flags)
	{
		this.Process_glFenceSync(returnVal, condition, flags);
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0000F4A8 File Offset: 0x0000D6A8
	private void SwigDirectorProcess_glIsSync(int returnVal, uint sync)
	{
		this.Process_glIsSync(returnVal, sync);
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x0000F4B2 File Offset: 0x0000D6B2
	private void SwigDirectorProcess_glDeleteSync(uint sync)
	{
		this.Process_glDeleteSync(sync);
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0000F4BB File Offset: 0x0000D6BB
	private void SwigDirectorProcess_glClientWaitSync(uint returnVal, uint sync, uint flags, ulong timeout)
	{
		this.Process_glClientWaitSync(returnVal, sync, flags, timeout);
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x0000F4C8 File Offset: 0x0000D6C8
	private void SwigDirectorProcess_glWaitSync(uint sync, uint flags, ulong timeout)
	{
		this.Process_glWaitSync(sync, flags, timeout);
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0000F4D3 File Offset: 0x0000D6D3
	private void SwigDirectorProcess_glGetInteger64v(uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetInteger64v(pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0000F4F3 File Offset: 0x0000D6F3
	private void SwigDirectorProcess_glGetSynciv(uint sync, uint pname, int bufSize, IntPtr pLengthPtrData, IntPtr pValuesPtrData)
	{
		this.Process_glGetSynciv(sync, pname, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pValuesPtrData == IntPtr.Zero) ? null : new PointerData(pValuesPtrData, false));
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0000F530 File Offset: 0x0000D730
	private void SwigDirectorProcess_glGetInteger64i_v(uint target, uint index, IntPtr pDataPtrData)
	{
		this.Process_glGetInteger64i_v(target, index, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x0000F551 File Offset: 0x0000D751
	private void SwigDirectorProcess_glGetBufferParameteri64v(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetBufferParameteri64v(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0000F572 File Offset: 0x0000D772
	private void SwigDirectorProcess_glGenSamplers(int count, IntPtr pSamplersPtrData)
	{
		this.Process_glGenSamplers(count, (pSamplersPtrData == IntPtr.Zero) ? null : new PointerData(pSamplersPtrData, false));
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x0000F592 File Offset: 0x0000D792
	private void SwigDirectorProcess_glDeleteSamplers(int count, IntPtr pSamplersPtrData)
	{
		this.Process_glDeleteSamplers(count, (pSamplersPtrData == IntPtr.Zero) ? null : new PointerData(pSamplersPtrData, false));
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x0000F5B2 File Offset: 0x0000D7B2
	private void SwigDirectorProcess_glIsSampler(int returnVal, uint sampler)
	{
		this.Process_glIsSampler(returnVal, sampler);
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0000F5BC File Offset: 0x0000D7BC
	private void SwigDirectorProcess_glBindSampler(uint unit, uint sampler)
	{
		this.Process_glBindSampler(unit, sampler);
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0000F5C6 File Offset: 0x0000D7C6
	private void SwigDirectorProcess_glSamplerParameteri(uint sampler, uint pname, int param)
	{
		this.Process_glSamplerParameteri(sampler, pname, param);
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0000F5D1 File Offset: 0x0000D7D1
	private void SwigDirectorProcess_glSamplerParameteriv(uint sampler, uint pname, IntPtr pParamPtrData)
	{
		this.Process_glSamplerParameteriv(sampler, pname, (pParamPtrData == IntPtr.Zero) ? null : new PointerData(pParamPtrData, false));
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x0000F5F2 File Offset: 0x0000D7F2
	private void SwigDirectorProcess_glSamplerParameterf(uint sampler, uint pname, float param)
	{
		this.Process_glSamplerParameterf(sampler, pname, param);
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x0000F5FD File Offset: 0x0000D7FD
	private void SwigDirectorProcess_glSamplerParameterfv(uint sampler, uint pname, IntPtr pParamPtrData)
	{
		this.Process_glSamplerParameterfv(sampler, pname, (pParamPtrData == IntPtr.Zero) ? null : new PointerData(pParamPtrData, false));
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x0000F61E File Offset: 0x0000D81E
	private void SwigDirectorProcess_glGetSamplerParameteriv(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetSamplerParameteriv(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x0000F63F File Offset: 0x0000D83F
	private void SwigDirectorProcess_glGetSamplerParameterfv(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetSamplerParameterfv(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x0000F660 File Offset: 0x0000D860
	private void SwigDirectorProcess_glVertexAttribDivisor(uint index, uint divisor)
	{
		this.Process_glVertexAttribDivisor(index, divisor);
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0000F66A File Offset: 0x0000D86A
	private void SwigDirectorProcess_glBindTransformFeedback(uint target, uint id)
	{
		this.Process_glBindTransformFeedback(target, id);
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x0000F674 File Offset: 0x0000D874
	private void SwigDirectorProcess_glDeleteTransformFeedbacks(int n, IntPtr pIdsPtrData)
	{
		this.Process_glDeleteTransformFeedbacks(n, (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false));
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0000F694 File Offset: 0x0000D894
	private void SwigDirectorProcess_glGenTransformFeedbacks(int n, IntPtr pIdsPtrData)
	{
		this.Process_glGenTransformFeedbacks(n, (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false));
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0000F6B4 File Offset: 0x0000D8B4
	private void SwigDirectorProcess_glIsTransformFeedback(int returnVal, uint id)
	{
		this.Process_glIsTransformFeedback(returnVal, id);
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x0000F6BE File Offset: 0x0000D8BE
	private void SwigDirectorProcess_glPauseTransformFeedback()
	{
		this.Process_glPauseTransformFeedback();
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x0000F6C6 File Offset: 0x0000D8C6
	private void SwigDirectorProcess_glResumeTransformFeedback()
	{
		this.Process_glResumeTransformFeedback();
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0000F6D0 File Offset: 0x0000D8D0
	private void SwigDirectorProcess_glGetProgramBinary(uint program, int bufSize, IntPtr pLengthPtrData, IntPtr pBinaryFormatPtrData, IntPtr pBinaryPtrData)
	{
		this.Process_glGetProgramBinary(program, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pBinaryFormatPtrData == IntPtr.Zero) ? null : new PointerData(pBinaryFormatPtrData, false), (pBinaryPtrData == IntPtr.Zero) ? null : new PointerData(pBinaryPtrData, false));
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0000F72E File Offset: 0x0000D92E
	private void SwigDirectorProcess_glProgramBinary(uint program, uint binaryFormat, IntPtr pBinaryPtrData, int length)
	{
		this.Process_glProgramBinary(program, binaryFormat, (pBinaryPtrData == IntPtr.Zero) ? null : new PointerData(pBinaryPtrData, false), length);
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0000F751 File Offset: 0x0000D951
	private void SwigDirectorProcess_glProgramParameteri(uint program, uint pname, int value)
	{
		this.Process_glProgramParameteri(program, pname, value);
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x0000F75C File Offset: 0x0000D95C
	private void SwigDirectorProcess_glInvalidateFramebuffer(uint target, int numAttachments, IntPtr pAttachmentsPtrData)
	{
		this.Process_glInvalidateFramebuffer(target, numAttachments, (pAttachmentsPtrData == IntPtr.Zero) ? null : new PointerData(pAttachmentsPtrData, false));
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x0000F77D File Offset: 0x0000D97D
	private void SwigDirectorProcess_glInvalidateSubFramebuffer(uint target, int numAttachments, IntPtr pAttachmentsPtrData, int x, int y, int width, int height)
	{
		this.Process_glInvalidateSubFramebuffer(target, numAttachments, (pAttachmentsPtrData == IntPtr.Zero) ? null : new PointerData(pAttachmentsPtrData, false), x, y, width, height);
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0000F7A6 File Offset: 0x0000D9A6
	private void SwigDirectorProcess_glTexStorage2D(uint target, int levels, uint internalformat, int width, int height)
	{
		this.Process_glTexStorage2D(target, levels, internalformat, width, height);
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x0000F7B5 File Offset: 0x0000D9B5
	private void SwigDirectorProcess_glTexStorage3D(uint target, int levels, uint internalformat, int width, int height, int depth)
	{
		this.Process_glTexStorage3D(target, levels, internalformat, width, height, depth);
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x0000F7C6 File Offset: 0x0000D9C6
	private void SwigDirectorProcess_glGetInternalformativ(uint target, uint internalformat, uint pname, int bufSize, IntPtr pParamsPtrData)
	{
		this.Process_glGetInternalformativ(target, internalformat, pname, bufSize, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0000F7EC File Offset: 0x0000D9EC
	private void SwigDirectorProcess_glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z)
	{
		this.Process_glDispatchCompute(num_groups_x, num_groups_y, num_groups_z);
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0000F7F7 File Offset: 0x0000D9F7
	private void SwigDirectorProcess_glDispatchComputeIndirect(int indirect)
	{
		this.Process_glDispatchComputeIndirect(indirect);
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0000F800 File Offset: 0x0000DA00
	private void SwigDirectorProcess_glDrawArraysIndirect(uint mode, IntPtr pIndirectPtrData)
	{
		this.Process_glDrawArraysIndirect(mode, (pIndirectPtrData == IntPtr.Zero) ? null : new PointerData(pIndirectPtrData, false));
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x0000F820 File Offset: 0x0000DA20
	private void SwigDirectorProcess_glDrawElementsIndirect(uint mode, uint type, IntPtr pIndirectPtrData)
	{
		this.Process_glDrawElementsIndirect(mode, type, (pIndirectPtrData == IntPtr.Zero) ? null : new PointerData(pIndirectPtrData, false));
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x0000F841 File Offset: 0x0000DA41
	private void SwigDirectorProcess_glFramebufferParameteri(uint target, uint pname, int param)
	{
		this.Process_glFramebufferParameteri(target, pname, param);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0000F84C File Offset: 0x0000DA4C
	private void SwigDirectorProcess_glGetFramebufferParameteriv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetFramebufferParameteriv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0000F86D File Offset: 0x0000DA6D
	private void SwigDirectorProcess_glGetProgramInterfaceiv(uint program, uint programInterface, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetProgramInterfaceiv(program, programInterface, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0000F891 File Offset: 0x0000DA91
	private void SwigDirectorProcess_glGetProgramResourceIndex(uint returnVal, uint program, uint programInterface, IntPtr pNamePtrData)
	{
		this.Process_glGetProgramResourceIndex(returnVal, program, programInterface, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0000F8B5 File Offset: 0x0000DAB5
	private void SwigDirectorProcess_glGetProgramResourceName(uint program, uint programInterface, uint index, int bufSize, IntPtr pLengthPtrData, IntPtr pNamePtrData)
	{
		this.Process_glGetProgramResourceName(program, programInterface, index, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x0000F8F4 File Offset: 0x0000DAF4
	private void SwigDirectorProcess_glGetProgramResourceiv(uint program, uint programInterface, uint index, int propCount, IntPtr pPropsPtrData, int bufSize, IntPtr pLengthPtrData, IntPtr pParamsPtrData)
	{
		this.Process_glGetProgramResourceiv(program, programInterface, index, propCount, (pPropsPtrData == IntPtr.Zero) ? null : new PointerData(pPropsPtrData, false), bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0000F959 File Offset: 0x0000DB59
	private void SwigDirectorProcess_glGetProgramResourceLocation(uint returnVal, uint program, uint programInterface, IntPtr pNamePtrData)
	{
		this.Process_glGetProgramResourceLocation(returnVal, program, programInterface, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0000F97D File Offset: 0x0000DB7D
	private void SwigDirectorProcess_glUseProgramStages(uint pipeline, uint stages, uint program)
	{
		this.Process_glUseProgramStages(pipeline, stages, program);
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0000F988 File Offset: 0x0000DB88
	private void SwigDirectorProcess_glActiveShaderProgram(uint pipeline, uint program)
	{
		this.Process_glActiveShaderProgram(pipeline, program);
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0000F992 File Offset: 0x0000DB92
	private void SwigDirectorProcess_glCreateShaderProgramv(uint returnVal, uint type, int count, IntPtr pAPtrData)
	{
		this.Process_glCreateShaderProgramv(returnVal, type, count, (pAPtrData == IntPtr.Zero) ? null : new PointerData(pAPtrData, false));
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0000F9B6 File Offset: 0x0000DBB6
	private void SwigDirectorProcess_glBindProgramPipeline(uint pipeline)
	{
		this.Process_glBindProgramPipeline(pipeline);
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x0000F9BF File Offset: 0x0000DBBF
	private void SwigDirectorProcess_glDeleteProgramPipelines(int n, IntPtr pPipelinesPtrData)
	{
		this.Process_glDeleteProgramPipelines(n, (pPipelinesPtrData == IntPtr.Zero) ? null : new PointerData(pPipelinesPtrData, false));
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0000F9DF File Offset: 0x0000DBDF
	private void SwigDirectorProcess_glGenProgramPipelines(int n, IntPtr pPipelinesPtrData)
	{
		this.Process_glGenProgramPipelines(n, (pPipelinesPtrData == IntPtr.Zero) ? null : new PointerData(pPipelinesPtrData, false));
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0000F9FF File Offset: 0x0000DBFF
	private void SwigDirectorProcess_glIsProgramPipeline(int returnVal, uint pipeline)
	{
		this.Process_glIsProgramPipeline(returnVal, pipeline);
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0000FA09 File Offset: 0x0000DC09
	private void SwigDirectorProcess_glGetProgramPipelineiv(uint pipeline, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetProgramPipelineiv(pipeline, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0000FA2A File Offset: 0x0000DC2A
	private void SwigDirectorProcess_glProgramUniform1i(uint program, uint location, int v0)
	{
		this.Process_glProgramUniform1i(program, location, v0);
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0000FA35 File Offset: 0x0000DC35
	private void SwigDirectorProcess_glProgramUniform2i(uint program, uint location, int v0, int v1)
	{
		this.Process_glProgramUniform2i(program, location, v0, v1);
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0000FA42 File Offset: 0x0000DC42
	private void SwigDirectorProcess_glProgramUniform3i(uint program, uint location, int v0, int v1, int v2)
	{
		this.Process_glProgramUniform3i(program, location, v0, v1, v2);
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0000FA51 File Offset: 0x0000DC51
	private void SwigDirectorProcess_glProgramUniform4i(uint program, uint location, int v0, int v1, int v2, int v3)
	{
		this.Process_glProgramUniform4i(program, location, v0, v1, v2, v3);
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0000FA62 File Offset: 0x0000DC62
	private void SwigDirectorProcess_glProgramUniform1ui(uint program, uint location, uint v0)
	{
		this.Process_glProgramUniform1ui(program, location, v0);
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0000FA6D File Offset: 0x0000DC6D
	private void SwigDirectorProcess_glProgramUniform2ui(uint program, uint location, uint v0, uint v1)
	{
		this.Process_glProgramUniform2ui(program, location, v0, v1);
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0000FA7A File Offset: 0x0000DC7A
	private void SwigDirectorProcess_glProgramUniform3ui(uint program, uint location, uint v0, uint v1, uint v2)
	{
		this.Process_glProgramUniform3ui(program, location, v0, v1, v2);
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0000FA89 File Offset: 0x0000DC89
	private void SwigDirectorProcess_glProgramUniform4ui(uint program, uint location, uint v0, uint v1, uint v2, uint v3)
	{
		this.Process_glProgramUniform4ui(program, location, v0, v1, v2, v3);
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0000FA9A File Offset: 0x0000DC9A
	private void SwigDirectorProcess_glProgramUniform1f(uint program, uint location, float v0)
	{
		this.Process_glProgramUniform1f(program, location, v0);
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0000FAA5 File Offset: 0x0000DCA5
	private void SwigDirectorProcess_glProgramUniform2f(uint program, uint location, float v0, float v1)
	{
		this.Process_glProgramUniform2f(program, location, v0, v1);
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0000FAB2 File Offset: 0x0000DCB2
	private void SwigDirectorProcess_glProgramUniform3f(uint program, uint location, float v0, float v1, float v2)
	{
		this.Process_glProgramUniform3f(program, location, v0, v1, v2);
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0000FAC1 File Offset: 0x0000DCC1
	private void SwigDirectorProcess_glProgramUniform4f(uint program, uint location, float v0, float v1, float v2, float v3)
	{
		this.Process_glProgramUniform4f(program, location, v0, v1, v2, v3);
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0000FAD2 File Offset: 0x0000DCD2
	private void SwigDirectorProcess_glProgramUniform1iv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform1iv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0000FAF6 File Offset: 0x0000DCF6
	private void SwigDirectorProcess_glProgramUniform2iv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform2iv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0000FB1A File Offset: 0x0000DD1A
	private void SwigDirectorProcess_glProgramUniform3iv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform3iv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0000FB3E File Offset: 0x0000DD3E
	private void SwigDirectorProcess_glProgramUniform4iv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform4iv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0000FB62 File Offset: 0x0000DD62
	private void SwigDirectorProcess_glProgramUniform1uiv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform1uiv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0000FB86 File Offset: 0x0000DD86
	private void SwigDirectorProcess_glProgramUniform2uiv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform2uiv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0000FBAA File Offset: 0x0000DDAA
	private void SwigDirectorProcess_glProgramUniform3uiv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform3uiv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0000FBCE File Offset: 0x0000DDCE
	private void SwigDirectorProcess_glProgramUniform4uiv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform4uiv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0000FBF2 File Offset: 0x0000DDF2
	private void SwigDirectorProcess_glProgramUniform1fv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform1fv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0000FC16 File Offset: 0x0000DE16
	private void SwigDirectorProcess_glProgramUniform2fv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform2fv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0000FC3A File Offset: 0x0000DE3A
	private void SwigDirectorProcess_glProgramUniform3fv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform3fv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0000FC5E File Offset: 0x0000DE5E
	private void SwigDirectorProcess_glProgramUniform4fv(uint program, uint location, int count, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniform4fv(program, location, count, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0000FC82 File Offset: 0x0000DE82
	private void SwigDirectorProcess_glProgramUniformMatrix2fv(uint program, uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniformMatrix2fv(program, location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0000FCA8 File Offset: 0x0000DEA8
	private void SwigDirectorProcess_glProgramUniformMatrix3fv(uint program, uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniformMatrix3fv(program, location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0000FCCE File Offset: 0x0000DECE
	private void SwigDirectorProcess_glProgramUniformMatrix4fv(uint program, uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniformMatrix4fv(program, location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
	private void SwigDirectorProcess_glProgramUniformMatrix2x3fv(uint program, uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniformMatrix2x3fv(program, location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0000FD1A File Offset: 0x0000DF1A
	private void SwigDirectorProcess_glProgramUniformMatrix3x2fv(uint program, uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniformMatrix3x2fv(program, location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0000FD40 File Offset: 0x0000DF40
	private void SwigDirectorProcess_glProgramUniformMatrix2x4fv(uint program, uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniformMatrix2x4fv(program, location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0000FD66 File Offset: 0x0000DF66
	private void SwigDirectorProcess_glProgramUniformMatrix4x2fv(uint program, uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniformMatrix4x2fv(program, location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0000FD8C File Offset: 0x0000DF8C
	private void SwigDirectorProcess_glProgramUniformMatrix3x4fv(uint program, uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniformMatrix3x4fv(program, location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0000FDB2 File Offset: 0x0000DFB2
	private void SwigDirectorProcess_glProgramUniformMatrix4x3fv(uint program, uint location, int count, int transpose, IntPtr pValuePtrData)
	{
		this.Process_glProgramUniformMatrix4x3fv(program, location, count, transpose, (pValuePtrData == IntPtr.Zero) ? null : new PointerData(pValuePtrData, false));
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
	private void SwigDirectorProcess_glValidateProgramPipeline(uint pipeline)
	{
		this.Process_glValidateProgramPipeline(pipeline);
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0000FDE1 File Offset: 0x0000DFE1
	private void SwigDirectorProcess_glGetProgramPipelineInfoLog(uint pipeline, int bufSize, IntPtr pLengthPtrData, IntPtr pInfoLogPtrData)
	{
		this.Process_glGetProgramPipelineInfoLog(pipeline, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pInfoLogPtrData == IntPtr.Zero) ? null : new PointerData(pInfoLogPtrData, false));
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0000FE1B File Offset: 0x0000E01B
	private void SwigDirectorProcess_glGetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetActiveAtomicCounterBufferiv(program, bufferIndex, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0000FE3F File Offset: 0x0000E03F
	private void SwigDirectorProcess_glBindImageTexture(uint unit, uint texture, int level, int layered, int layer, uint access, uint format)
	{
		this.Process_glBindImageTexture(unit, texture, level, layered, layer, access, format);
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0000FE52 File Offset: 0x0000E052
	private void SwigDirectorProcess_glMemoryBarrier(uint barriers)
	{
		this.Process_glMemoryBarrier(barriers);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0000FE5B File Offset: 0x0000E05B
	private void SwigDirectorProcess_glMemoryBarrierByRegion(uint barriers)
	{
		this.Process_glMemoryBarrierByRegion(barriers);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0000FE64 File Offset: 0x0000E064
	private void SwigDirectorProcess_glTexStorage2DMultisample(uint target, int samples, uint internalformat, int width, int height, int fixedsamplelocations)
	{
		this.Process_glTexStorage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0000FE75 File Offset: 0x0000E075
	private void SwigDirectorProcess_glTexStorage3DMultisampleOES(uint target, int samples, uint internalformat, int width, int height, int depth, int fixedsamplelocations)
	{
		this.Process_glTexStorage3DMultisampleOES(target, samples, internalformat, width, height, depth, fixedsamplelocations);
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0000FE88 File Offset: 0x0000E088
	private void SwigDirectorProcess_glGetMultisamplefv(uint pname, uint index, IntPtr pValPtrData)
	{
		this.Process_glGetMultisamplefv(pname, index, (pValPtrData == IntPtr.Zero) ? null : new PointerData(pValPtrData, false));
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0000FEA9 File Offset: 0x0000E0A9
	private void SwigDirectorProcess_glSampleMaski(uint index, uint mask)
	{
		this.Process_glSampleMaski(index, mask);
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0000FEB3 File Offset: 0x0000E0B3
	private void SwigDirectorProcess_glGetTexLevelParameteriv(uint target, int level, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetTexLevelParameteriv(target, level, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x0000FED7 File Offset: 0x0000E0D7
	private void SwigDirectorProcess_glGetTexLevelParameterfv(uint target, int level, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetTexLevelParameterfv(target, level, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0000FEFB File Offset: 0x0000E0FB
	private void SwigDirectorProcess_glBindVertexBuffer(uint bindingindex, uint buffer, int offset, int stride)
	{
		this.Process_glBindVertexBuffer(bindingindex, buffer, offset, stride);
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0000FF08 File Offset: 0x0000E108
	private void SwigDirectorProcess_glVertexAttribFormat(uint attribindex, int size, uint type, int normalized, uint relativeoffset)
	{
		this.Process_glVertexAttribFormat(attribindex, size, type, normalized, relativeoffset);
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0000FF17 File Offset: 0x0000E117
	private void SwigDirectorProcess_glVertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset)
	{
		this.Process_glVertexAttribIFormat(attribindex, size, type, relativeoffset);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0000FF24 File Offset: 0x0000E124
	private void SwigDirectorProcess_glVertexAttribBinding(uint attribindex, uint bindingindex)
	{
		this.Process_glVertexAttribBinding(attribindex, bindingindex);
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x0000FF2E File Offset: 0x0000E12E
	private void SwigDirectorProcess_glVertexBindingDivisor(uint bindingindex, uint divisor)
	{
		this.Process_glVertexBindingDivisor(bindingindex, divisor);
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x0000FF38 File Offset: 0x0000E138
	private void SwigDirectorProcess_glPatchParameteri(uint pname, int value)
	{
		this.Process_glPatchParameteri(pname, value);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x0000FF42 File Offset: 0x0000E142
	private void SwigDirectorProcess_glGetFixedvAMD(uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetFixedvAMD(pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0000FF62 File Offset: 0x0000E162
	private void SwigDirectorProcess_glLogicOpAMD(uint op)
	{
		this.Process_glLogicOpAMD(op);
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0000FF6B File Offset: 0x0000E16B
	private void SwigDirectorProcess_glFogfvAMD(uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glFogfvAMD(pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0000FF8B File Offset: 0x0000E18B
	private void SwigDirectorProcess_glGetMemoryStatsQCOM(uint pname, uint usage, IntPtr pParamPtrData)
	{
		this.Process_glGetMemoryStatsQCOM(pname, usage, (pParamPtrData == IntPtr.Zero) ? null : new PointerData(pParamPtrData, false));
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0000FFAC File Offset: 0x0000E1AC
	private void SwigDirectorProcess_glGetSizedMemoryStatsQCOM(int maxcount, IntPtr pCountPtrData, IntPtr pBufPtrData)
	{
		this.Process_glGetSizedMemoryStatsQCOM(maxcount, (pCountPtrData == IntPtr.Zero) ? null : new PointerData(pCountPtrData, false), (pBufPtrData == IntPtr.Zero) ? null : new PointerData(pBufPtrData, false));
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0000FFE3 File Offset: 0x0000E1E3
	private void SwigDirectorProcess_glBlitOverlapQCOM(int dest_x, int dest_y, int src_x, int src_y, int src_width, int src_height)
	{
		this.Process_glBlitOverlapQCOM(dest_x, dest_y, src_x, src_y, src_width, src_height);
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0000FFF4 File Offset: 0x0000E1F4
	private void SwigDirectorProcess_glGetShaderStatsQCOM(uint shader, int maxLength, IntPtr pLengthPtrData, IntPtr pDataPtrData)
	{
		this.Process_glGetShaderStatsQCOM(shader, maxLength, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0001002E File Offset: 0x0000E22E
	private void SwigDirectorProcess_glExtGetSamplersQCOM(IntPtr pSamplersPtrData, int maxSamplers, IntPtr pNumSamplersPtrData)
	{
		this.Process_glExtGetSamplersQCOM((pSamplersPtrData == IntPtr.Zero) ? null : new PointerData(pSamplersPtrData, false), maxSamplers, (pNumSamplersPtrData == IntPtr.Zero) ? null : new PointerData(pNumSamplersPtrData, false));
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00010065 File Offset: 0x0000E265
	private void SwigDirectorProcess_glClipPlanefQCOM(uint p, IntPtr pEquationPtrData)
	{
		this.Process_glClipPlanefQCOM(p, (pEquationPtrData == IntPtr.Zero) ? null : new PointerData(pEquationPtrData, false));
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00010085 File Offset: 0x0000E285
	private void SwigDirectorProcess_glFramebufferTexture2DExternalQCOM(uint target, uint attachment, uint textarget, uint texture, int level)
	{
		this.Process_glFramebufferTexture2DExternalQCOM(target, attachment, textarget, texture, level);
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00010094 File Offset: 0x0000E294
	private void SwigDirectorProcess_glFramebufferRenderbufferExternalQCOM(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer)
	{
		this.Process_glFramebufferRenderbufferExternalQCOM(target, attachment, renderbuffertarget, renderbuffer);
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x000100A1 File Offset: 0x0000E2A1
	private void SwigDirectorProcess_glEGLImageTargetTexture2DOES(uint target, uint image)
	{
		this.Process_glEGLImageTargetTexture2DOES(target, image);
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x000100AB File Offset: 0x0000E2AB
	private void SwigDirectorProcess_glEGLImageTargetRenderbufferStorageOES(uint target, uint image)
	{
		this.Process_glEGLImageTargetRenderbufferStorageOES(target, image);
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x000100B8 File Offset: 0x0000E2B8
	private void SwigDirectorProcess_glGetProgramBinaryOES(uint program, int bufSize, IntPtr pLengthPtrData, IntPtr pBinaryFormatPtrData, IntPtr pBinaryPtrData)
	{
		this.Process_glGetProgramBinaryOES(program, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pBinaryFormatPtrData == IntPtr.Zero) ? null : new PointerData(pBinaryFormatPtrData, false), (pBinaryPtrData == IntPtr.Zero) ? null : new PointerData(pBinaryPtrData, false));
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00010116 File Offset: 0x0000E316
	private void SwigDirectorProcess_glProgramBinaryOES(uint program, uint binaryFormat, IntPtr pBinaryPtrData, int length)
	{
		this.Process_glProgramBinaryOES(program, binaryFormat, (pBinaryPtrData == IntPtr.Zero) ? null : new PointerData(pBinaryPtrData, false), length);
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0001013C File Offset: 0x0000E33C
	private void SwigDirectorProcess_glTexImage3DOES(uint target, int level, uint internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pPixelsPtrData)
	{
		this.Process_glTexImage3DOES(target, level, internalformat, width, height, depth, border, format, type, (pPixelsPtrData == IntPtr.Zero) ? null : new PointerData(pPixelsPtrData, false));
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x00010178 File Offset: 0x0000E378
	private void SwigDirectorProcess_glTexSubImage3DOES(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pPixelsPtrData)
	{
		this.Process_glTexSubImage3DOES(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, (pPixelsPtrData == IntPtr.Zero) ? null : new PointerData(pPixelsPtrData, false));
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x000101B8 File Offset: 0x0000E3B8
	private void SwigDirectorProcess_glCopyTexSubImage3DOES(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height)
	{
		this.Process_glCopyTexSubImage3DOES(target, level, xoffset, yoffset, zoffset, x, y, width, height);
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x000101DC File Offset: 0x0000E3DC
	private void SwigDirectorProcess_glCompressedTexImage3DOES(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr pDataPtrData)
	{
		this.Process_glCompressedTexImage3DOES(target, level, internalformat, width, height, depth, border, imageSize, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00010218 File Offset: 0x0000E418
	private void SwigDirectorProcess_glCompressedTexSubImage3DOES(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr pDataPtrData)
	{
		this.Process_glCompressedTexSubImage3DOES(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00010255 File Offset: 0x0000E455
	private void SwigDirectorProcess_glFramebufferTexture3DOES(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset)
	{
		this.Process_glFramebufferTexture3DOES(target, attachment, textarget, texture, level, zoffset);
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x00010266 File Offset: 0x0000E466
	private void SwigDirectorProcess_glBindVertexArrayOES(uint array)
	{
		this.Process_glBindVertexArrayOES(array);
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0001026F File Offset: 0x0000E46F
	private void SwigDirectorProcess_glDeleteVertexArraysOES(int n, IntPtr pArraysPtrData)
	{
		this.Process_glDeleteVertexArraysOES(n, (pArraysPtrData == IntPtr.Zero) ? null : new PointerData(pArraysPtrData, false));
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0001028F File Offset: 0x0000E48F
	private void SwigDirectorProcess_glGenVertexArraysOES(int n, IntPtr pArraysPtrData)
	{
		this.Process_glGenVertexArraysOES(n, (pArraysPtrData == IntPtr.Zero) ? null : new PointerData(pArraysPtrData, false));
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x000102AF File Offset: 0x0000E4AF
	private void SwigDirectorProcess_glIsVertexArrayOES(int returnVal, uint array)
	{
		this.Process_glIsVertexArrayOES(returnVal, array);
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x000102B9 File Offset: 0x0000E4B9
	private void SwigDirectorProcess_glGetPerfMonitorGroupsAMD(IntPtr pNumGroupsPtrData, int groupsSize, IntPtr pGroupsPtrData)
	{
		this.Process_glGetPerfMonitorGroupsAMD((pNumGroupsPtrData == IntPtr.Zero) ? null : new PointerData(pNumGroupsPtrData, false), groupsSize, (pGroupsPtrData == IntPtr.Zero) ? null : new PointerData(pGroupsPtrData, false));
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x000102F0 File Offset: 0x0000E4F0
	private void SwigDirectorProcess_glGetPerfMonitorCountersAMD(uint group, IntPtr pNumCountersPtrData, IntPtr pMaxActiveCountersPtrData, int counterSize, IntPtr pCountersPtrData)
	{
		this.Process_glGetPerfMonitorCountersAMD(group, (pNumCountersPtrData == IntPtr.Zero) ? null : new PointerData(pNumCountersPtrData, false), (pMaxActiveCountersPtrData == IntPtr.Zero) ? null : new PointerData(pMaxActiveCountersPtrData, false), counterSize, (pCountersPtrData == IntPtr.Zero) ? null : new PointerData(pCountersPtrData, false));
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0001034D File Offset: 0x0000E54D
	private void SwigDirectorProcess_glGetPerfMonitorGroupStringAMD(uint group, int bufSize, IntPtr pLengthPtrData, IntPtr pGroupStringPtrData)
	{
		this.Process_glGetPerfMonitorGroupStringAMD(group, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pGroupStringPtrData == IntPtr.Zero) ? null : new PointerData(pGroupStringPtrData, false));
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00010387 File Offset: 0x0000E587
	private void SwigDirectorProcess_glGetPerfMonitorCounterStringAMD(uint group, uint counter, int bufSize, IntPtr pLengthPtrData, IntPtr pCounterStringPtrData)
	{
		this.Process_glGetPerfMonitorCounterStringAMD(group, counter, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pCounterStringPtrData == IntPtr.Zero) ? null : new PointerData(pCounterStringPtrData, false));
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x000103C4 File Offset: 0x0000E5C4
	private void SwigDirectorProcess_glGetPerfMonitorCounterInfoAMD(uint group, uint counter, uint pname, IntPtr pDataPtrData)
	{
		this.Process_glGetPerfMonitorCounterInfoAMD(group, counter, pname, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x000103E8 File Offset: 0x0000E5E8
	private void SwigDirectorProcess_glGenPerfMonitorsAMD(int n, IntPtr pMonitorsPtrData)
	{
		this.Process_glGenPerfMonitorsAMD(n, (pMonitorsPtrData == IntPtr.Zero) ? null : new PointerData(pMonitorsPtrData, false));
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00010408 File Offset: 0x0000E608
	private void SwigDirectorProcess_glDeletePerfMonitorsAMD(int n, IntPtr pMonitorsPtrData)
	{
		this.Process_glDeletePerfMonitorsAMD(n, (pMonitorsPtrData == IntPtr.Zero) ? null : new PointerData(pMonitorsPtrData, false));
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00010428 File Offset: 0x0000E628
	private void SwigDirectorProcess_glSelectPerfMonitorCountersAMD(uint monitor, int enable, uint group, int numCounters, IntPtr pCountersListPtrData)
	{
		this.Process_glSelectPerfMonitorCountersAMD(monitor, enable, group, numCounters, (pCountersListPtrData == IntPtr.Zero) ? null : new PointerData(pCountersListPtrData, false));
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0001044E File Offset: 0x0000E64E
	private void SwigDirectorProcess_glBeginPerfMonitorAMD(uint monitor)
	{
		this.Process_glBeginPerfMonitorAMD(monitor);
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00010457 File Offset: 0x0000E657
	private void SwigDirectorProcess_glEndPerfMonitorAMD(uint monitor)
	{
		this.Process_glEndPerfMonitorAMD(monitor);
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00010460 File Offset: 0x0000E660
	private void SwigDirectorProcess_glGetPerfMonitorCounterDataAMD(uint monitor, uint pname, int dataSize, IntPtr pDataPtrData, IntPtr pBytesWrittenPtrData)
	{
		this.Process_glGetPerfMonitorCounterDataAMD(monitor, pname, dataSize, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false), (pBytesWrittenPtrData == IntPtr.Zero) ? null : new PointerData(pBytesWrittenPtrData, false));
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x0001049D File Offset: 0x0000E69D
	private void SwigDirectorProcess_glLabelObjectEXT(uint type, uint arg1, int length, IntPtr pLabelPtrData)
	{
		this.Process_glLabelObjectEXT(type, arg1, length, (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x000104C1 File Offset: 0x0000E6C1
	private void SwigDirectorProcess_glGetObjectLabelEXT(uint type, uint arg1, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData)
	{
		this.Process_glGetObjectLabelEXT(type, arg1, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x000104FE File Offset: 0x0000E6FE
	private void SwigDirectorProcess_glInsertEventMarkerEXT(int length, IntPtr pMarkerPtrData)
	{
		this.Process_glInsertEventMarkerEXT(length, (pMarkerPtrData == IntPtr.Zero) ? null : new PointerData(pMarkerPtrData, false));
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0001051E File Offset: 0x0000E71E
	private void SwigDirectorProcess_glPushGroupMarkerEXT(int length, IntPtr pMarkerPtrData)
	{
		this.Process_glPushGroupMarkerEXT(length, (pMarkerPtrData == IntPtr.Zero) ? null : new PointerData(pMarkerPtrData, false));
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0001053E File Offset: 0x0000E73E
	private void SwigDirectorProcess_glPopGroupMarkerEXT()
	{
		this.Process_glPopGroupMarkerEXT();
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00010546 File Offset: 0x0000E746
	private void SwigDirectorProcess_glDiscardFramebufferEXT(uint target, int numAttachments, IntPtr pAttachmentsPtrData)
	{
		this.Process_glDiscardFramebufferEXT(target, numAttachments, (pAttachmentsPtrData == IntPtr.Zero) ? null : new PointerData(pAttachmentsPtrData, false));
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00010567 File Offset: 0x0000E767
	private void SwigDirectorProcess_glGenQueriesEXT(int n, IntPtr pIdsPtrData)
	{
		this.Process_glGenQueriesEXT(n, (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false));
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00010587 File Offset: 0x0000E787
	private void SwigDirectorProcess_glDeleteQueriesEXT(int n, IntPtr pIdsPtrData)
	{
		this.Process_glDeleteQueriesEXT(n, (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false));
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x000105A7 File Offset: 0x0000E7A7
	private void SwigDirectorProcess_glIsQueryEXT(int returnVal, uint id)
	{
		this.Process_glIsQueryEXT(returnVal, id);
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x000105B1 File Offset: 0x0000E7B1
	private void SwigDirectorProcess_glBeginQueryEXT(uint target, uint id)
	{
		this.Process_glBeginQueryEXT(target, id);
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x000105BB File Offset: 0x0000E7BB
	private void SwigDirectorProcess_glEndQueryEXT(uint target)
	{
		this.Process_glEndQueryEXT(target);
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x000105C4 File Offset: 0x0000E7C4
	private void SwigDirectorProcess_glQueryCounterEXT(uint id, uint target)
	{
		this.Process_glQueryCounterEXT(id, target);
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x000105CE File Offset: 0x0000E7CE
	private void SwigDirectorProcess_glGetQueryivEXT(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetQueryivEXT(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x000105EF File Offset: 0x0000E7EF
	private void SwigDirectorProcess_glGetQueryObjectivEXT(uint id, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetQueryObjectivEXT(id, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x00010610 File Offset: 0x0000E810
	private void SwigDirectorProcess_glGetQueryObjectuivEXT(uint id, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetQueryObjectuivEXT(id, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00010631 File Offset: 0x0000E831
	private void SwigDirectorProcess_glGetQueryObjecti64vEXT(uint id, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetQueryObjecti64vEXT(id, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00010652 File Offset: 0x0000E852
	private void SwigDirectorProcess_glGetQueryObjectui64vEXT(uint id, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetQueryObjectui64vEXT(id, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00010673 File Offset: 0x0000E873
	private void SwigDirectorProcess_glGetGraphicsResetStatusEXT(uint returnVal)
	{
		this.Process_glGetGraphicsResetStatusEXT(returnVal);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0001067C File Offset: 0x0000E87C
	private void SwigDirectorProcess_glReadnPixelsEXT(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr pDataPtrData)
	{
		this.Process_glReadnPixelsEXT(x, y, width, height, format, type, bufSize, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x000106B3 File Offset: 0x0000E8B3
	private void SwigDirectorProcess_glGetnUniformfvEXT(uint program, uint location, int bufSize, IntPtr pParamsPtrData)
	{
		this.Process_glGetnUniformfvEXT(program, location, bufSize, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x000106D7 File Offset: 0x0000E8D7
	private void SwigDirectorProcess_glGetnUniformivEXT(uint program, uint location, int bufSize, IntPtr pParamsPtrData)
	{
		this.Process_glGetnUniformivEXT(program, location, bufSize, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x000106FB File Offset: 0x0000E8FB
	private void SwigDirectorProcess_glTexParameterIivEXT(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glTexParameterIivEXT(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0001071C File Offset: 0x0000E91C
	private void SwigDirectorProcess_glTexParameterIuivEXT(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glTexParameterIuivEXT(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0001073D File Offset: 0x0000E93D
	private void SwigDirectorProcess_glGetTexParameterIivEXT(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetTexParameterIivEXT(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0001075E File Offset: 0x0000E95E
	private void SwigDirectorProcess_glGetTexParameterIuivEXT(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetTexParameterIuivEXT(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001077F File Offset: 0x0000E97F
	private void SwigDirectorProcess_glSamplerParameterIivEXT(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glSamplerParameterIivEXT(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x000107A0 File Offset: 0x0000E9A0
	private void SwigDirectorProcess_glSamplerParameterIuivEXT(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glSamplerParameterIuivEXT(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x000107C1 File Offset: 0x0000E9C1
	private void SwigDirectorProcess_glGetSamplerParameterIivEXT(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetSamplerParameterIivEXT(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x000107E2 File Offset: 0x0000E9E2
	private void SwigDirectorProcess_glGetSamplerParameterIuivEXT(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetSamplerParameterIuivEXT(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00010803 File Offset: 0x0000EA03
	private void SwigDirectorProcess_glRenderbufferStorageMultisampleEXT(uint target, int samples, uint internalformat, int width, int height)
	{
		this.Process_glRenderbufferStorageMultisampleEXT(target, samples, internalformat, width, height);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00010812 File Offset: 0x0000EA12
	private void SwigDirectorProcess_glFramebufferTexture2DMultisampleEXT(uint target, uint attachment, uint textarget, uint texture, int level, int samples)
	{
		this.Process_glFramebufferTexture2DMultisampleEXT(target, attachment, textarget, texture, level, samples);
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00010823 File Offset: 0x0000EA23
	private void SwigDirectorProcess_glAlphaFuncQCOM(uint func, float arg1)
	{
		this.Process_glAlphaFuncQCOM(func, arg1);
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x0001082D File Offset: 0x0000EA2D
	private void SwigDirectorProcess_glStartTilingQCOM(uint x, uint y, uint width, uint height, uint preserveMask)
	{
		this.Process_glStartTilingQCOM(x, y, width, height, preserveMask);
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0001083C File Offset: 0x0000EA3C
	private void SwigDirectorProcess_glEndTilingQCOM(uint preserveMask)
	{
		this.Process_glEndTilingQCOM(preserveMask);
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00010848 File Offset: 0x0000EA48
	private void SwigDirectorProcess_glCopyImageSubDataEXT(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth)
	{
		this.Process_glCopyImageSubDataEXT(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00010876 File Offset: 0x0000EA76
	private void SwigDirectorProcess_glBlendBarrierKHR()
	{
		this.Process_glBlendBarrierKHR();
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0001087E File Offset: 0x0000EA7E
	private void SwigDirectorProcess_glMinSampleShadingOES(float value)
	{
		this.Process_glMinSampleShadingOES(value);
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00010887 File Offset: 0x0000EA87
	private void SwigDirectorProcess_glEnableiEXT(uint target, uint index)
	{
		this.Process_glEnableiEXT(target, index);
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00010891 File Offset: 0x0000EA91
	private void SwigDirectorProcess_glDisableiEXT(uint target, uint index)
	{
		this.Process_glDisableiEXT(target, index);
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0001089B File Offset: 0x0000EA9B
	private void SwigDirectorProcess_glBlendEquationiEXT(uint buf, uint mode)
	{
		this.Process_glBlendEquationiEXT(buf, mode);
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x000108A5 File Offset: 0x0000EAA5
	private void SwigDirectorProcess_glBlendEquationSeparateiEXT(uint buf, uint modeRGB, uint modeAlpha)
	{
		this.Process_glBlendEquationSeparateiEXT(buf, modeRGB, modeAlpha);
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x000108B0 File Offset: 0x0000EAB0
	private void SwigDirectorProcess_glBlendFunciEXT(uint buf, uint src, uint dst)
	{
		this.Process_glBlendFunciEXT(buf, src, dst);
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x000108BB File Offset: 0x0000EABB
	private void SwigDirectorProcess_glBlendFuncSeparateiEXT(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
	{
		this.Process_glBlendFuncSeparateiEXT(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x000108CA File Offset: 0x0000EACA
	private void SwigDirectorProcess_glColorMaskiEXT(uint buf, int r, int g, int b, int a)
	{
		this.Process_glColorMaskiEXT(buf, r, g, b, a);
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x000108D9 File Offset: 0x0000EAD9
	private void SwigDirectorProcess_glIsEnablediEXT(int returnVal, uint target, uint index)
	{
		this.Process_glIsEnablediEXT(returnVal, target, index);
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x000108E4 File Offset: 0x0000EAE4
	private void SwigDirectorProcess_glTexBufferEXT(uint target, uint internalFormat, uint buffer)
	{
		this.Process_glTexBufferEXT(target, internalFormat, buffer);
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x000108EF File Offset: 0x0000EAEF
	private void SwigDirectorProcess_glTexBufferRangeEXT(uint target, uint internalFormat, uint buffer, int offset, int size)
	{
		this.Process_glTexBufferRangeEXT(target, internalFormat, buffer, offset, size);
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x000108FE File Offset: 0x0000EAFE
	private void SwigDirectorProcess_glDebugMessageControlKHR(uint source, uint type, uint severity, int count, IntPtr pIdsPtrData, int enabled)
	{
		this.Process_glDebugMessageControlKHR(source, type, severity, count, (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false), enabled);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00010926 File Offset: 0x0000EB26
	private void SwigDirectorProcess_glDebugMessageInsertKHR(uint source, uint type, uint id, uint severity, int length, IntPtr pBufPtrData)
	{
		this.Process_glDebugMessageInsertKHR(source, type, id, severity, length, (pBufPtrData == IntPtr.Zero) ? null : new PointerData(pBufPtrData, false));
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0001094E File Offset: 0x0000EB4E
	private void SwigDirectorProcess_glDebugMessageCallbackKHR(IntPtr pCallbackPtrData, IntPtr pUserParamPtrData)
	{
		this.Process_glDebugMessageCallbackKHR((pCallbackPtrData == IntPtr.Zero) ? null : new PointerData(pCallbackPtrData, false), (pUserParamPtrData == IntPtr.Zero) ? null : new PointerData(pUserParamPtrData, false));
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00010984 File Offset: 0x0000EB84
	private void SwigDirectorProcess_glGetDebugMessageLogKHR(uint returnVal, uint count, int bufSize, IntPtr pSourcesPtrData, IntPtr pTypesPtrData, IntPtr pIdsPtrData, IntPtr pSeveritiesPtrData, IntPtr pLengthsPtrData, IntPtr pMessageLogPtrData)
	{
		this.Process_glGetDebugMessageLogKHR(returnVal, count, bufSize, (pSourcesPtrData == IntPtr.Zero) ? null : new PointerData(pSourcesPtrData, false), (pTypesPtrData == IntPtr.Zero) ? null : new PointerData(pTypesPtrData, false), (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false), (pSeveritiesPtrData == IntPtr.Zero) ? null : new PointerData(pSeveritiesPtrData, false), (pLengthsPtrData == IntPtr.Zero) ? null : new PointerData(pLengthsPtrData, false), (pMessageLogPtrData == IntPtr.Zero) ? null : new PointerData(pMessageLogPtrData, false));
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00010A30 File Offset: 0x0000EC30
	private void SwigDirectorProcess_glPushDebugGroupKHR(uint source, uint id, int length, IntPtr pMessagePtrData)
	{
		this.Process_glPushDebugGroupKHR(source, id, length, (pMessagePtrData == IntPtr.Zero) ? null : new PointerData(pMessagePtrData, false));
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00010A54 File Offset: 0x0000EC54
	private void SwigDirectorProcess_glPopDebugGroupKHR()
	{
		this.Process_glPopDebugGroupKHR();
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00010A5C File Offset: 0x0000EC5C
	private void SwigDirectorProcess_glObjectLabelKHR(uint identifier, uint name, int length, IntPtr pLabelPtrData)
	{
		this.Process_glObjectLabelKHR(identifier, name, length, (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00010A80 File Offset: 0x0000EC80
	private void SwigDirectorProcess_glGetObjectLabelKHR(uint identifier, uint name, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData)
	{
		this.Process_glGetObjectLabelKHR(identifier, name, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00010ABD File Offset: 0x0000ECBD
	private void SwigDirectorProcess_glObjectPtrLabelKHR(uint ptr, int length, IntPtr pLabelPtrData)
	{
		this.Process_glObjectPtrLabelKHR(ptr, length, (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00010ADE File Offset: 0x0000ECDE
	private void SwigDirectorProcess_glGetObjectPtrLabelKHR(uint ptr, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData)
	{
		this.Process_glGetObjectPtrLabelKHR(ptr, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00010B18 File Offset: 0x0000ED18
	private void SwigDirectorProcess_glGetPointervKHR(uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetPointervKHR(pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00010B38 File Offset: 0x0000ED38
	private void SwigDirectorProcess_glPrimitiveBoundingBoxEXT(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW)
	{
		this.Process_glPrimitiveBoundingBoxEXT(minX, minY, minZ, minW, maxX, maxY, maxZ, maxW);
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00010B58 File Offset: 0x0000ED58
	private void SwigDirectorProcess_glPatchParameteriEXT(uint pname, int value)
	{
		this.Process_glPatchParameteriEXT(pname, value);
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00010B62 File Offset: 0x0000ED62
	private void SwigDirectorProcess_glDrawElementsBaseVertex(uint mode, int count, uint type, IntPtr pIndicesPtrData, int basevertex)
	{
		this.Process_glDrawElementsBaseVertex(mode, count, type, (pIndicesPtrData == IntPtr.Zero) ? null : new PointerData(pIndicesPtrData, false), basevertex);
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00010B88 File Offset: 0x0000ED88
	private void SwigDirectorProcess_glDrawRangeElementsBaseVertex(uint mode, uint start, uint end, int count, uint type, IntPtr pIndicesPtrData, int basevertex)
	{
		this.Process_glDrawRangeElementsBaseVertex(mode, start, end, count, type, (pIndicesPtrData == IntPtr.Zero) ? null : new PointerData(pIndicesPtrData, false), basevertex);
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00010BB2 File Offset: 0x0000EDB2
	private void SwigDirectorProcess_glDrawElementsInstancedBaseVertex(uint mode, int count, uint type, IntPtr pIndicesPtrData, int instanceCount, int basevertex)
	{
		this.Process_glDrawElementsInstancedBaseVertex(mode, count, type, (pIndicesPtrData == IntPtr.Zero) ? null : new PointerData(pIndicesPtrData, false), instanceCount, basevertex);
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00010BDA File Offset: 0x0000EDDA
	private void SwigDirectorProcess_glFramebufferTextureEXT(uint target, uint attachment, uint texture, int level)
	{
		this.Process_glFramebufferTextureEXT(target, attachment, texture, level);
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00010BE7 File Offset: 0x0000EDE7
	private void SwigDirectorProcess_glFramebufferTextureMultiviewOVR(uint target, uint attachment, uint texture, int level, int baseViewIndex, int numViews)
	{
		this.Process_glFramebufferTextureMultiviewOVR(target, attachment, texture, level, baseViewIndex, numViews);
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00010BF8 File Offset: 0x0000EDF8
	private void SwigDirectorProcess_glFramebufferTextureMultisampleMultiviewOVR(uint target, uint attachment, uint texture, int level, int samples, int baseView, int numViews)
	{
		this.Process_glFramebufferTextureMultisampleMultiviewOVR(target, attachment, texture, level, samples, baseView, numViews);
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00010C0B File Offset: 0x0000EE0B
	private void SwigDirectorProcess_glBufferStorageEXT(uint target, int size, IntPtr pDataPtrData, uint flags)
	{
		this.Process_glBufferStorageEXT(target, size, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false), flags);
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00010C2E File Offset: 0x0000EE2E
	private void SwigDirectorProcess_glGetGraphicsResetStatus(uint returnVal)
	{
		this.Process_glGetGraphicsResetStatus(returnVal);
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x00010C38 File Offset: 0x0000EE38
	private void SwigDirectorProcess_glReadnPixels(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr pDataPtrData)
	{
		this.Process_glReadnPixels(x, y, width, height, format, type, bufSize, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00010C6F File Offset: 0x0000EE6F
	private void SwigDirectorProcess_glGetnUniformfv(uint program, uint location, int bufSize, IntPtr pParamsPtrData)
	{
		this.Process_glGetnUniformfv(program, location, bufSize, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00010C93 File Offset: 0x0000EE93
	private void SwigDirectorProcess_glGetnUniformiv(uint program, uint location, int bufSize, IntPtr pParamsPtrData)
	{
		this.Process_glGetnUniformiv(program, location, bufSize, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00010CB7 File Offset: 0x0000EEB7
	private void SwigDirectorProcess_glGetnUniformuiv(uint program, uint location, int bufSize, IntPtr pParamsPtrData)
	{
		this.Process_glGetnUniformuiv(program, location, bufSize, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x00010CDB File Offset: 0x0000EEDB
	private void SwigDirectorProcess_glTexParameterIiv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glTexParameterIiv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00010CFC File Offset: 0x0000EEFC
	private void SwigDirectorProcess_glTexParameterIuiv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glTexParameterIuiv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00010D1D File Offset: 0x0000EF1D
	private void SwigDirectorProcess_glGetTexParameterIiv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetTexParameterIiv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00010D3E File Offset: 0x0000EF3E
	private void SwigDirectorProcess_glGetTexParameterIuiv(uint target, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetTexParameterIuiv(target, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00010D5F File Offset: 0x0000EF5F
	private void SwigDirectorProcess_glSamplerParameterIiv(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glSamplerParameterIiv(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00010D80 File Offset: 0x0000EF80
	private void SwigDirectorProcess_glSamplerParameterIuiv(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glSamplerParameterIuiv(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00010DA1 File Offset: 0x0000EFA1
	private void SwigDirectorProcess_glGetSamplerParameterIiv(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetSamplerParameterIiv(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00010DC2 File Offset: 0x0000EFC2
	private void SwigDirectorProcess_glGetSamplerParameterIuiv(uint sampler, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetSamplerParameterIuiv(sampler, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00010DE3 File Offset: 0x0000EFE3
	private void SwigDirectorProcess_glNumBinsPerSubmitQCOM(uint numBins, int separateBinningPass)
	{
		this.Process_glNumBinsPerSubmitQCOM(numBins, separateBinningPass);
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00010DF0 File Offset: 0x0000EFF0
	private void SwigDirectorProcess_glCopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth)
	{
		this.Process_glCopyImageSubData(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00010E1E File Offset: 0x0000F01E
	private void SwigDirectorProcess_glBlendBarrier()
	{
		this.Process_glBlendBarrier();
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00010E26 File Offset: 0x0000F026
	private void SwigDirectorProcess_glMinSampleShading(float value)
	{
		this.Process_glMinSampleShading(value);
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00010E2F File Offset: 0x0000F02F
	private void SwigDirectorProcess_glEnablei(uint target, uint index)
	{
		this.Process_glEnablei(target, index);
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00010E39 File Offset: 0x0000F039
	private void SwigDirectorProcess_glDisablei(uint target, uint index)
	{
		this.Process_glDisablei(target, index);
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00010E43 File Offset: 0x0000F043
	private void SwigDirectorProcess_glBlendEquationi(uint buf, uint mode)
	{
		this.Process_glBlendEquationi(buf, mode);
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00010E4D File Offset: 0x0000F04D
	private void SwigDirectorProcess_glBlendEquationSeparatei(uint buf, uint modeRGB, uint modeAlpha)
	{
		this.Process_glBlendEquationSeparatei(buf, modeRGB, modeAlpha);
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00010E58 File Offset: 0x0000F058
	private void SwigDirectorProcess_glBlendFunci(uint buf, uint src, uint dst)
	{
		this.Process_glBlendFunci(buf, src, dst);
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x00010E63 File Offset: 0x0000F063
	private void SwigDirectorProcess_glBlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
	{
		this.Process_glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00010E72 File Offset: 0x0000F072
	private void SwigDirectorProcess_glColorMaski(uint buf, int r, int g, int b, int a)
	{
		this.Process_glColorMaski(buf, r, g, b, a);
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00010E81 File Offset: 0x0000F081
	private void SwigDirectorProcess_glIsEnabledi(int returnVal, uint target, uint index)
	{
		this.Process_glIsEnabledi(returnVal, target, index);
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x00010E8C File Offset: 0x0000F08C
	private void SwigDirectorProcess_glTexBuffer(uint target, uint internalFormat, uint buffer)
	{
		this.Process_glTexBuffer(target, internalFormat, buffer);
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00010E97 File Offset: 0x0000F097
	private void SwigDirectorProcess_glTexBufferRange(uint target, uint internalFormat, uint buffer, int offset, int size)
	{
		this.Process_glTexBufferRange(target, internalFormat, buffer, offset, size);
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00010EA6 File Offset: 0x0000F0A6
	private void SwigDirectorProcess_glDebugMessageControl(uint source, uint type, uint severity, int count, IntPtr pIdsPtrData, int enabled)
	{
		this.Process_glDebugMessageControl(source, type, severity, count, (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false), enabled);
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00010ECE File Offset: 0x0000F0CE
	private void SwigDirectorProcess_glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, IntPtr pBufPtrData)
	{
		this.Process_glDebugMessageInsert(source, type, id, severity, length, (pBufPtrData == IntPtr.Zero) ? null : new PointerData(pBufPtrData, false));
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00010EF6 File Offset: 0x0000F0F6
	private void SwigDirectorProcess_glDebugMessageCallback(IntPtr pCallbackPtrData, IntPtr pUserParamPtrData)
	{
		this.Process_glDebugMessageCallback((pCallbackPtrData == IntPtr.Zero) ? null : new PointerData(pCallbackPtrData, false), (pUserParamPtrData == IntPtr.Zero) ? null : new PointerData(pUserParamPtrData, false));
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00010F2C File Offset: 0x0000F12C
	private void SwigDirectorProcess_glGetDebugMessageLog(uint returnVal, uint count, int bufSize, IntPtr pSourcesPtrData, IntPtr pTypesPtrData, IntPtr pIdsPtrData, IntPtr pSeveritiesPtrData, IntPtr pLengthsPtrData, IntPtr pMessageLogPtrData)
	{
		this.Process_glGetDebugMessageLog(returnVal, count, bufSize, (pSourcesPtrData == IntPtr.Zero) ? null : new PointerData(pSourcesPtrData, false), (pTypesPtrData == IntPtr.Zero) ? null : new PointerData(pTypesPtrData, false), (pIdsPtrData == IntPtr.Zero) ? null : new PointerData(pIdsPtrData, false), (pSeveritiesPtrData == IntPtr.Zero) ? null : new PointerData(pSeveritiesPtrData, false), (pLengthsPtrData == IntPtr.Zero) ? null : new PointerData(pLengthsPtrData, false), (pMessageLogPtrData == IntPtr.Zero) ? null : new PointerData(pMessageLogPtrData, false));
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00010FD8 File Offset: 0x0000F1D8
	private void SwigDirectorProcess_glPushDebugGroup(uint source, uint id, int length, IntPtr pMessagePtrData)
	{
		this.Process_glPushDebugGroup(source, id, length, (pMessagePtrData == IntPtr.Zero) ? null : new PointerData(pMessagePtrData, false));
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00010FFC File Offset: 0x0000F1FC
	private void SwigDirectorProcess_glPopDebugGroup()
	{
		this.Process_glPopDebugGroup();
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00011004 File Offset: 0x0000F204
	private void SwigDirectorProcess_glObjectLabel(uint identifier, uint name, int length, IntPtr pLabelPtrData)
	{
		this.Process_glObjectLabel(identifier, name, length, (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00011028 File Offset: 0x0000F228
	private void SwigDirectorProcess_glGetObjectLabel(uint identifier, uint name, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData)
	{
		this.Process_glGetObjectLabel(identifier, name, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00011065 File Offset: 0x0000F265
	private void SwigDirectorProcess_glObjectPtrLabel(uint ptr, int length, IntPtr pLabelPtrData)
	{
		this.Process_glObjectPtrLabel(ptr, length, (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00011086 File Offset: 0x0000F286
	private void SwigDirectorProcess_glGetObjectPtrLabel(uint ptr, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData)
	{
		this.Process_glGetObjectPtrLabel(ptr, bufSize, (pLengthPtrData == IntPtr.Zero) ? null : new PointerData(pLengthPtrData, false), (pLabelPtrData == IntPtr.Zero) ? null : new PointerData(pLabelPtrData, false));
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x000110C0 File Offset: 0x0000F2C0
	private void SwigDirectorProcess_glGetPointerv(uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetPointerv(pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x000110E0 File Offset: 0x0000F2E0
	private void SwigDirectorProcess_glPrimitiveBoundingBox(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW)
	{
		this.Process_glPrimitiveBoundingBox(minX, minY, minZ, minW, maxX, maxY, maxZ, maxW);
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x00011100 File Offset: 0x0000F300
	private void SwigDirectorProcess_glBlitBlendColor(float red, float green, float blue, float alpha)
	{
		this.Process_glBlitBlendColor(red, green, blue, alpha);
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0001110D File Offset: 0x0000F30D
	private void SwigDirectorProcess_glBlitBlendEquationSeparate(uint modeRGB, uint modeAlpha)
	{
		this.Process_glBlitBlendEquationSeparate(modeRGB, modeAlpha);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00011117 File Offset: 0x0000F317
	private void SwigDirectorProcess_glBlitBlendFuncSeparate(uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
	{
		this.Process_glBlitBlendFuncSeparate(srcRGB, dstRGB, srcAlpha, dstAlpha);
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00011124 File Offset: 0x0000F324
	private void SwigDirectorProcess_glBlitRotation(uint rot)
	{
		this.Process_glBlitRotation(rot);
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0001112D File Offset: 0x0000F32D
	private void SwigDirectorProcess_glBindSharedBufferQCOM(uint target, int sizeInBytes, int fd)
	{
		this.Process_glBindSharedBufferQCOM(target, sizeInBytes, fd);
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00011138 File Offset: 0x0000F338
	private void SwigDirectorProcess_glCreateSharedBufferQCOM(int sizeInBytes, uint cacheMode, uint sharedList, IntPtr pOutFdPtrData)
	{
		this.Process_glCreateSharedBufferQCOM(sizeInBytes, cacheMode, sharedList, (pOutFdPtrData == IntPtr.Zero) ? null : new PointerData(pOutFdPtrData, false));
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0001115C File Offset: 0x0000F35C
	private void SwigDirectorProcess_glDestroySharedBufferQCOM(int fd)
	{
		this.Process_glDestroySharedBufferQCOM(fd);
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00011165 File Offset: 0x0000F365
	private void SwigDirectorProcess_glTextureBarrier()
	{
		this.Process_glTextureBarrier();
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0001116D File Offset: 0x0000F36D
	private void SwigDirectorProcess_glFramebufferFoveationConfigQCOM(uint framebuffer, uint numLayers, uint focalPointsPerLayer, uint requestedFeatures, IntPtr pProvidedFeaturesPtrData)
	{
		this.Process_glFramebufferFoveationConfigQCOM(framebuffer, numLayers, focalPointsPerLayer, requestedFeatures, (pProvidedFeaturesPtrData == IntPtr.Zero) ? null : new PointerData(pProvidedFeaturesPtrData, false));
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00011194 File Offset: 0x0000F394
	private void SwigDirectorProcess_glFramebufferFoveationParametersQCOM(uint framebuffer, uint layer, uint focalPoint, float focalX, float focalY, float gainX, float gainY, float foveaArea)
	{
		this.Process_glFramebufferFoveationParametersQCOM(framebuffer, layer, focalPoint, focalX, focalY, gainX, gainY, foveaArea);
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x000111B4 File Offset: 0x0000F3B4
	private void SwigDirectorProcess_glBufferStorageExternalEXT(uint target, int offset, int size, uint clientBuffer, uint flags)
	{
		this.Process_glBufferStorageExternalEXT(target, offset, size, clientBuffer, flags);
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x000111C3 File Offset: 0x0000F3C3
	private void SwigDirectorProcess_glFramebufferFetchBarrierQCOM()
	{
		this.Process_glFramebufferFetchBarrierQCOM();
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x000111CB File Offset: 0x0000F3CB
	private void SwigDirectorProcess_glCreateMemoryObjectsEXT(int n, IntPtr pMemoryObjectsPtrData)
	{
		this.Process_glCreateMemoryObjectsEXT(n, (pMemoryObjectsPtrData == IntPtr.Zero) ? null : new PointerData(pMemoryObjectsPtrData, false));
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x000111EB File Offset: 0x0000F3EB
	private void SwigDirectorProcess_glDeleteMemoryObjectsEXT(int n, IntPtr pMemoryObjectsPtrData)
	{
		this.Process_glDeleteMemoryObjectsEXT(n, (pMemoryObjectsPtrData == IntPtr.Zero) ? null : new PointerData(pMemoryObjectsPtrData, false));
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0001120B File Offset: 0x0000F40B
	private void SwigDirectorProcess_glIsMemoryObjectEXT(int returnVal, uint memoryObject)
	{
		this.Process_glIsMemoryObjectEXT(returnVal, memoryObject);
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00011215 File Offset: 0x0000F415
	private void SwigDirectorProcess_glMemoryObjectParameterivEXT(uint memoryObject, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glMemoryObjectParameterivEXT(memoryObject, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00011236 File Offset: 0x0000F436
	private void SwigDirectorProcess_glGetMemoryObjectParameterivEXT(uint memoryObject, uint pname, IntPtr pParamsPtrData)
	{
		this.Process_glGetMemoryObjectParameterivEXT(memoryObject, pname, (pParamsPtrData == IntPtr.Zero) ? null : new PointerData(pParamsPtrData, false));
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00011257 File Offset: 0x0000F457
	private void SwigDirectorProcess_glTexStorageMem2DEXT(uint target, int levels, uint internalFormat, int width, int height, uint memory, ulong offset)
	{
		this.Process_glTexStorageMem2DEXT(target, levels, internalFormat, width, height, memory, offset);
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x0001126C File Offset: 0x0000F46C
	private void SwigDirectorProcess_glTexStorageMem2DMultisampleEXT(uint target, int samples, uint internalFormat, int width, int height, int fixedSampleLocations, uint memory, ulong offset)
	{
		this.Process_glTexStorageMem2DMultisampleEXT(target, samples, internalFormat, width, height, fixedSampleLocations, memory, offset);
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x0001128C File Offset: 0x0000F48C
	private void SwigDirectorProcess_glTexStorageMem3DEXT(uint target, int levels, uint internalFormat, int width, int height, int depth, uint memory, ulong offset)
	{
		this.Process_glTexStorageMem3DEXT(target, levels, internalFormat, width, height, depth, memory, offset);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x000112AC File Offset: 0x0000F4AC
	private void SwigDirectorProcess_glTexStorageMem3DMultisampleEXT(uint target, int samples, uint internalFormat, int width, int height, int depth, int fixedSampleLocations, uint memory, ulong offset)
	{
		this.Process_glTexStorageMem3DMultisampleEXT(target, samples, internalFormat, width, height, depth, fixedSampleLocations, memory, offset);
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x000112CE File Offset: 0x0000F4CE
	private void SwigDirectorProcess_glBufferStorageMemEXT(uint target, int size, uint memory, ulong offset)
	{
		this.Process_glBufferStorageMemEXT(target, size, memory, offset);
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x000112DB File Offset: 0x0000F4DB
	private void SwigDirectorProcess_glGenSemaphoresKHR(int n, IntPtr pSemaphoresPtrData)
	{
		this.Process_glGenSemaphoresKHR(n, (pSemaphoresPtrData == IntPtr.Zero) ? null : new PointerData(pSemaphoresPtrData, false));
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x000112FB File Offset: 0x0000F4FB
	private void SwigDirectorProcess_glDeleteSemaphoresKHR(int n, IntPtr pSemaphoresPtrData)
	{
		this.Process_glDeleteSemaphoresKHR(n, (pSemaphoresPtrData == IntPtr.Zero) ? null : new PointerData(pSemaphoresPtrData, false));
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0001131B File Offset: 0x0000F51B
	private void SwigDirectorProcess_glIsSemaphoreKHR(int returnVal, uint semaphore)
	{
		this.Process_glIsSemaphoreKHR(returnVal, semaphore);
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00011325 File Offset: 0x0000F525
	private void SwigDirectorProcess_glWaitSemaphoreKHR(uint semaphore, IntPtr pSrcExternalUasgePtrData)
	{
		this.Process_glWaitSemaphoreKHR(semaphore, (pSrcExternalUasgePtrData == IntPtr.Zero) ? null : new PointerData(pSrcExternalUasgePtrData, false));
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00011345 File Offset: 0x0000F545
	private void SwigDirectorProcess_glSignalSemaphoreKHR(IntPtr pDstExternalUsagePtrData, uint semaphore)
	{
		this.Process_glSignalSemaphoreKHR((pDstExternalUsagePtrData == IntPtr.Zero) ? null : new PointerData(pDstExternalUsagePtrData, false), semaphore);
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00011365 File Offset: 0x0000F565
	private void SwigDirectorProcess_glImportMemoryFdEXT(uint memory, ulong size, uint handleType, int fd)
	{
		this.Process_glImportMemoryFdEXT(memory, size, handleType, fd);
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00011372 File Offset: 0x0000F572
	private void SwigDirectorProcess_glImportSemaphoreFdEXT(uint semaphore, uint handleType, int fd)
	{
		this.Process_glImportSemaphoreFdEXT(semaphore, handleType, fd);
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0001137D File Offset: 0x0000F57D
	private void SwigDirectorProcess_glGetUnsignedBytevEXT(uint pname, IntPtr pDataPtrData)
	{
		this.Process_glGetUnsignedBytevEXT(pname, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0001139D File Offset: 0x0000F59D
	private void SwigDirectorProcess_glGetUnsignedBytei_vEXT(uint target, uint index, IntPtr pDataPtrData)
	{
		this.Process_glGetUnsignedBytei_vEXT(target, index, (pDataPtrData == IntPtr.Zero) ? null : new PointerData(pDataPtrData, false));
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x000113C0 File Offset: 0x0000F5C0
	private void SwigDirectorProcess_glTextureFoveationParametersQCOM(uint texture, uint layer, uint focalPoint, float focalX, float focalY, float gainX, float gainY, float foveaArea)
	{
		this.Process_glTextureFoveationParametersQCOM(texture, layer, focalPoint, focalX, focalY, gainX, gainY, foveaArea);
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x000113E0 File Offset: 0x0000F5E0
	private void SwigDirectorProcess_glBindFragDataLocationIndexedEXT(uint program, uint colorNumber, uint index, IntPtr pNamePtrData)
	{
		this.Process_glBindFragDataLocationIndexedEXT(program, colorNumber, index, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00011404 File Offset: 0x0000F604
	private void SwigDirectorProcess_glBindFragDataLocationEXT(uint program, uint color, IntPtr pNamePtrData)
	{
		this.Process_glBindFragDataLocationEXT(program, color, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00011425 File Offset: 0x0000F625
	private void SwigDirectorProcess_glGetProgramResourceLocationIndexEXT(uint returnVal, uint program, uint programInterface, IntPtr pNamePtrData)
	{
		this.Process_glGetProgramResourceLocationIndexEXT(returnVal, program, programInterface, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x00011449 File Offset: 0x0000F649
	private void SwigDirectorProcess_glGetFragDataIndexEXT(uint returnVal, uint program, IntPtr pNamePtrData)
	{
		this.Process_glGetFragDataIndexEXT(returnVal, program, (pNamePtrData == IntPtr.Zero) ? null : new PointerData(pNamePtrData, false));
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0001146A File Offset: 0x0000F66A
	private void SwigDirectorProcess_glShadingRateQCOM(uint rate)
	{
		this.Process_glShadingRateQCOM(rate);
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x00011473 File Offset: 0x0000F673
	private void SwigDirectorProcess_glExtrapolateTex2DQCOM(uint src1, uint src2, uint output, float scaleFactor)
	{
		this.Process_glExtrapolateTex2DQCOM(src1, src2, output, scaleFactor);
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00011480 File Offset: 0x0000F680
	private void SwigDirectorProcess_glTextureViewOES(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers)
	{
		this.Process_glTextureViewOES(texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers);
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x000114A0 File Offset: 0x0000F6A0
	private void SwigDirectorProcess_glTexEstimateMotionQCOM(uint arg0, uint target, uint output)
	{
		this.Process_glTexEstimateMotionQCOM(arg0, target, output);
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x000114AB File Offset: 0x0000F6AB
	private void SwigDirectorProcess_glTexEstimateMotionRegionsQCOM(uint arg0, uint target, uint output, uint mask)
	{
		this.Process_glTexEstimateMotionRegionsQCOM(arg0, target, output, mask);
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x000114B8 File Offset: 0x0000F6B8
	private void SwigDirectorProcess_glEGLImageTargetTexStorageEXT(uint target, uint image, IntPtr pAttribListPtrData)
	{
		this.Process_glEGLImageTargetTexStorageEXT(target, image, (pAttribListPtrData == IntPtr.Zero) ? null : new PointerData(pAttribListPtrData, false));
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x000114D9 File Offset: 0x0000F6D9
	private void SwigDirectorProcess_glPolygonOffsetClampEXT(float factor, float units, float clamp)
	{
		this.Process_glPolygonOffsetClampEXT(factor, units, clamp);
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x000114E4 File Offset: 0x0000F6E4
	private void SwigDirectorProcess_glGetFragmentShadingRatesEXT(int samples, int maxCount, IntPtr pCountPtrData, IntPtr pShadingRatesPtrData)
	{
		this.Process_glGetFragmentShadingRatesEXT(samples, maxCount, (pCountPtrData == IntPtr.Zero) ? null : new PointerData(pCountPtrData, false), (pShadingRatesPtrData == IntPtr.Zero) ? null : new PointerData(pShadingRatesPtrData, false));
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0001151E File Offset: 0x0000F71E
	private void SwigDirectorProcess_glShadingRateEXT(uint rate)
	{
		this.Process_glShadingRateEXT(rate);
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00011527 File Offset: 0x0000F727
	private void SwigDirectorProcess_glShadingRateCombinerOpsEXT(uint combinerOp0, uint combinerOp1)
	{
		this.Process_glShadingRateCombinerOpsEXT(combinerOp0, combinerOp1);
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00011531 File Offset: 0x0000F731
	private void SwigDirectorProcess_glFramebufferShadingRateEXT(uint target, uint attachment, uint texture, int baseLayer, int numLayers, int texelWidth, int texelHeight)
	{
		this.Process_glFramebufferShadingRateEXT(target, attachment, texture, baseLayer, numLayers, texelWidth, texelHeight);
	}

	// Token: 0x0400059D RID: 1437
	private HandleRef swigCPtr;

	// Token: 0x0400059E RID: 1438
	private GLAdapter.SwigDelegateGLAdapter_0 swigDelegate0;

	// Token: 0x0400059F RID: 1439
	private GLAdapter.SwigDelegateGLAdapter_1 swigDelegate1;

	// Token: 0x040005A0 RID: 1440
	private GLAdapter.SwigDelegateGLAdapter_2 swigDelegate2;

	// Token: 0x040005A1 RID: 1441
	private GLAdapter.SwigDelegateGLAdapter_3 swigDelegate3;

	// Token: 0x040005A2 RID: 1442
	private GLAdapter.SwigDelegateGLAdapter_4 swigDelegate4;

	// Token: 0x040005A3 RID: 1443
	private GLAdapter.SwigDelegateGLAdapter_5 swigDelegate5;

	// Token: 0x040005A4 RID: 1444
	private GLAdapter.SwigDelegateGLAdapter_6 swigDelegate6;

	// Token: 0x040005A5 RID: 1445
	private GLAdapter.SwigDelegateGLAdapter_7 swigDelegate7;

	// Token: 0x040005A6 RID: 1446
	private GLAdapter.SwigDelegateGLAdapter_8 swigDelegate8;

	// Token: 0x040005A7 RID: 1447
	private GLAdapter.SwigDelegateGLAdapter_9 swigDelegate9;

	// Token: 0x040005A8 RID: 1448
	private GLAdapter.SwigDelegateGLAdapter_10 swigDelegate10;

	// Token: 0x040005A9 RID: 1449
	private GLAdapter.SwigDelegateGLAdapter_11 swigDelegate11;

	// Token: 0x040005AA RID: 1450
	private GLAdapter.SwigDelegateGLAdapter_12 swigDelegate12;

	// Token: 0x040005AB RID: 1451
	private GLAdapter.SwigDelegateGLAdapter_13 swigDelegate13;

	// Token: 0x040005AC RID: 1452
	private GLAdapter.SwigDelegateGLAdapter_14 swigDelegate14;

	// Token: 0x040005AD RID: 1453
	private GLAdapter.SwigDelegateGLAdapter_15 swigDelegate15;

	// Token: 0x040005AE RID: 1454
	private GLAdapter.SwigDelegateGLAdapter_16 swigDelegate16;

	// Token: 0x040005AF RID: 1455
	private GLAdapter.SwigDelegateGLAdapter_17 swigDelegate17;

	// Token: 0x040005B0 RID: 1456
	private GLAdapter.SwigDelegateGLAdapter_18 swigDelegate18;

	// Token: 0x040005B1 RID: 1457
	private GLAdapter.SwigDelegateGLAdapter_19 swigDelegate19;

	// Token: 0x040005B2 RID: 1458
	private GLAdapter.SwigDelegateGLAdapter_20 swigDelegate20;

	// Token: 0x040005B3 RID: 1459
	private GLAdapter.SwigDelegateGLAdapter_21 swigDelegate21;

	// Token: 0x040005B4 RID: 1460
	private GLAdapter.SwigDelegateGLAdapter_22 swigDelegate22;

	// Token: 0x040005B5 RID: 1461
	private GLAdapter.SwigDelegateGLAdapter_23 swigDelegate23;

	// Token: 0x040005B6 RID: 1462
	private GLAdapter.SwigDelegateGLAdapter_24 swigDelegate24;

	// Token: 0x040005B7 RID: 1463
	private GLAdapter.SwigDelegateGLAdapter_25 swigDelegate25;

	// Token: 0x040005B8 RID: 1464
	private GLAdapter.SwigDelegateGLAdapter_26 swigDelegate26;

	// Token: 0x040005B9 RID: 1465
	private GLAdapter.SwigDelegateGLAdapter_27 swigDelegate27;

	// Token: 0x040005BA RID: 1466
	private GLAdapter.SwigDelegateGLAdapter_28 swigDelegate28;

	// Token: 0x040005BB RID: 1467
	private GLAdapter.SwigDelegateGLAdapter_29 swigDelegate29;

	// Token: 0x040005BC RID: 1468
	private GLAdapter.SwigDelegateGLAdapter_30 swigDelegate30;

	// Token: 0x040005BD RID: 1469
	private GLAdapter.SwigDelegateGLAdapter_31 swigDelegate31;

	// Token: 0x040005BE RID: 1470
	private GLAdapter.SwigDelegateGLAdapter_32 swigDelegate32;

	// Token: 0x040005BF RID: 1471
	private GLAdapter.SwigDelegateGLAdapter_33 swigDelegate33;

	// Token: 0x040005C0 RID: 1472
	private GLAdapter.SwigDelegateGLAdapter_34 swigDelegate34;

	// Token: 0x040005C1 RID: 1473
	private GLAdapter.SwigDelegateGLAdapter_35 swigDelegate35;

	// Token: 0x040005C2 RID: 1474
	private GLAdapter.SwigDelegateGLAdapter_36 swigDelegate36;

	// Token: 0x040005C3 RID: 1475
	private GLAdapter.SwigDelegateGLAdapter_37 swigDelegate37;

	// Token: 0x040005C4 RID: 1476
	private GLAdapter.SwigDelegateGLAdapter_38 swigDelegate38;

	// Token: 0x040005C5 RID: 1477
	private GLAdapter.SwigDelegateGLAdapter_39 swigDelegate39;

	// Token: 0x040005C6 RID: 1478
	private GLAdapter.SwigDelegateGLAdapter_40 swigDelegate40;

	// Token: 0x040005C7 RID: 1479
	private GLAdapter.SwigDelegateGLAdapter_41 swigDelegate41;

	// Token: 0x040005C8 RID: 1480
	private GLAdapter.SwigDelegateGLAdapter_42 swigDelegate42;

	// Token: 0x040005C9 RID: 1481
	private GLAdapter.SwigDelegateGLAdapter_43 swigDelegate43;

	// Token: 0x040005CA RID: 1482
	private GLAdapter.SwigDelegateGLAdapter_44 swigDelegate44;

	// Token: 0x040005CB RID: 1483
	private GLAdapter.SwigDelegateGLAdapter_45 swigDelegate45;

	// Token: 0x040005CC RID: 1484
	private GLAdapter.SwigDelegateGLAdapter_46 swigDelegate46;

	// Token: 0x040005CD RID: 1485
	private GLAdapter.SwigDelegateGLAdapter_47 swigDelegate47;

	// Token: 0x040005CE RID: 1486
	private GLAdapter.SwigDelegateGLAdapter_48 swigDelegate48;

	// Token: 0x040005CF RID: 1487
	private GLAdapter.SwigDelegateGLAdapter_49 swigDelegate49;

	// Token: 0x040005D0 RID: 1488
	private GLAdapter.SwigDelegateGLAdapter_50 swigDelegate50;

	// Token: 0x040005D1 RID: 1489
	private GLAdapter.SwigDelegateGLAdapter_51 swigDelegate51;

	// Token: 0x040005D2 RID: 1490
	private GLAdapter.SwigDelegateGLAdapter_52 swigDelegate52;

	// Token: 0x040005D3 RID: 1491
	private GLAdapter.SwigDelegateGLAdapter_53 swigDelegate53;

	// Token: 0x040005D4 RID: 1492
	private GLAdapter.SwigDelegateGLAdapter_54 swigDelegate54;

	// Token: 0x040005D5 RID: 1493
	private GLAdapter.SwigDelegateGLAdapter_55 swigDelegate55;

	// Token: 0x040005D6 RID: 1494
	private GLAdapter.SwigDelegateGLAdapter_56 swigDelegate56;

	// Token: 0x040005D7 RID: 1495
	private GLAdapter.SwigDelegateGLAdapter_57 swigDelegate57;

	// Token: 0x040005D8 RID: 1496
	private GLAdapter.SwigDelegateGLAdapter_58 swigDelegate58;

	// Token: 0x040005D9 RID: 1497
	private GLAdapter.SwigDelegateGLAdapter_59 swigDelegate59;

	// Token: 0x040005DA RID: 1498
	private GLAdapter.SwigDelegateGLAdapter_60 swigDelegate60;

	// Token: 0x040005DB RID: 1499
	private GLAdapter.SwigDelegateGLAdapter_61 swigDelegate61;

	// Token: 0x040005DC RID: 1500
	private GLAdapter.SwigDelegateGLAdapter_62 swigDelegate62;

	// Token: 0x040005DD RID: 1501
	private GLAdapter.SwigDelegateGLAdapter_63 swigDelegate63;

	// Token: 0x040005DE RID: 1502
	private GLAdapter.SwigDelegateGLAdapter_64 swigDelegate64;

	// Token: 0x040005DF RID: 1503
	private GLAdapter.SwigDelegateGLAdapter_65 swigDelegate65;

	// Token: 0x040005E0 RID: 1504
	private GLAdapter.SwigDelegateGLAdapter_66 swigDelegate66;

	// Token: 0x040005E1 RID: 1505
	private GLAdapter.SwigDelegateGLAdapter_67 swigDelegate67;

	// Token: 0x040005E2 RID: 1506
	private GLAdapter.SwigDelegateGLAdapter_68 swigDelegate68;

	// Token: 0x040005E3 RID: 1507
	private GLAdapter.SwigDelegateGLAdapter_69 swigDelegate69;

	// Token: 0x040005E4 RID: 1508
	private GLAdapter.SwigDelegateGLAdapter_70 swigDelegate70;

	// Token: 0x040005E5 RID: 1509
	private GLAdapter.SwigDelegateGLAdapter_71 swigDelegate71;

	// Token: 0x040005E6 RID: 1510
	private GLAdapter.SwigDelegateGLAdapter_72 swigDelegate72;

	// Token: 0x040005E7 RID: 1511
	private GLAdapter.SwigDelegateGLAdapter_73 swigDelegate73;

	// Token: 0x040005E8 RID: 1512
	private GLAdapter.SwigDelegateGLAdapter_74 swigDelegate74;

	// Token: 0x040005E9 RID: 1513
	private GLAdapter.SwigDelegateGLAdapter_75 swigDelegate75;

	// Token: 0x040005EA RID: 1514
	private GLAdapter.SwigDelegateGLAdapter_76 swigDelegate76;

	// Token: 0x040005EB RID: 1515
	private GLAdapter.SwigDelegateGLAdapter_77 swigDelegate77;

	// Token: 0x040005EC RID: 1516
	private GLAdapter.SwigDelegateGLAdapter_78 swigDelegate78;

	// Token: 0x040005ED RID: 1517
	private GLAdapter.SwigDelegateGLAdapter_79 swigDelegate79;

	// Token: 0x040005EE RID: 1518
	private GLAdapter.SwigDelegateGLAdapter_80 swigDelegate80;

	// Token: 0x040005EF RID: 1519
	private GLAdapter.SwigDelegateGLAdapter_81 swigDelegate81;

	// Token: 0x040005F0 RID: 1520
	private GLAdapter.SwigDelegateGLAdapter_82 swigDelegate82;

	// Token: 0x040005F1 RID: 1521
	private GLAdapter.SwigDelegateGLAdapter_83 swigDelegate83;

	// Token: 0x040005F2 RID: 1522
	private GLAdapter.SwigDelegateGLAdapter_84 swigDelegate84;

	// Token: 0x040005F3 RID: 1523
	private GLAdapter.SwigDelegateGLAdapter_85 swigDelegate85;

	// Token: 0x040005F4 RID: 1524
	private GLAdapter.SwigDelegateGLAdapter_86 swigDelegate86;

	// Token: 0x040005F5 RID: 1525
	private GLAdapter.SwigDelegateGLAdapter_87 swigDelegate87;

	// Token: 0x040005F6 RID: 1526
	private GLAdapter.SwigDelegateGLAdapter_88 swigDelegate88;

	// Token: 0x040005F7 RID: 1527
	private GLAdapter.SwigDelegateGLAdapter_89 swigDelegate89;

	// Token: 0x040005F8 RID: 1528
	private GLAdapter.SwigDelegateGLAdapter_90 swigDelegate90;

	// Token: 0x040005F9 RID: 1529
	private GLAdapter.SwigDelegateGLAdapter_91 swigDelegate91;

	// Token: 0x040005FA RID: 1530
	private GLAdapter.SwigDelegateGLAdapter_92 swigDelegate92;

	// Token: 0x040005FB RID: 1531
	private GLAdapter.SwigDelegateGLAdapter_93 swigDelegate93;

	// Token: 0x040005FC RID: 1532
	private GLAdapter.SwigDelegateGLAdapter_94 swigDelegate94;

	// Token: 0x040005FD RID: 1533
	private GLAdapter.SwigDelegateGLAdapter_95 swigDelegate95;

	// Token: 0x040005FE RID: 1534
	private GLAdapter.SwigDelegateGLAdapter_96 swigDelegate96;

	// Token: 0x040005FF RID: 1535
	private GLAdapter.SwigDelegateGLAdapter_97 swigDelegate97;

	// Token: 0x04000600 RID: 1536
	private GLAdapter.SwigDelegateGLAdapter_98 swigDelegate98;

	// Token: 0x04000601 RID: 1537
	private GLAdapter.SwigDelegateGLAdapter_99 swigDelegate99;

	// Token: 0x04000602 RID: 1538
	private GLAdapter.SwigDelegateGLAdapter_100 swigDelegate100;

	// Token: 0x04000603 RID: 1539
	private GLAdapter.SwigDelegateGLAdapter_101 swigDelegate101;

	// Token: 0x04000604 RID: 1540
	private GLAdapter.SwigDelegateGLAdapter_102 swigDelegate102;

	// Token: 0x04000605 RID: 1541
	private GLAdapter.SwigDelegateGLAdapter_103 swigDelegate103;

	// Token: 0x04000606 RID: 1542
	private GLAdapter.SwigDelegateGLAdapter_104 swigDelegate104;

	// Token: 0x04000607 RID: 1543
	private GLAdapter.SwigDelegateGLAdapter_105 swigDelegate105;

	// Token: 0x04000608 RID: 1544
	private GLAdapter.SwigDelegateGLAdapter_106 swigDelegate106;

	// Token: 0x04000609 RID: 1545
	private GLAdapter.SwigDelegateGLAdapter_107 swigDelegate107;

	// Token: 0x0400060A RID: 1546
	private GLAdapter.SwigDelegateGLAdapter_108 swigDelegate108;

	// Token: 0x0400060B RID: 1547
	private GLAdapter.SwigDelegateGLAdapter_109 swigDelegate109;

	// Token: 0x0400060C RID: 1548
	private GLAdapter.SwigDelegateGLAdapter_110 swigDelegate110;

	// Token: 0x0400060D RID: 1549
	private GLAdapter.SwigDelegateGLAdapter_111 swigDelegate111;

	// Token: 0x0400060E RID: 1550
	private GLAdapter.SwigDelegateGLAdapter_112 swigDelegate112;

	// Token: 0x0400060F RID: 1551
	private GLAdapter.SwigDelegateGLAdapter_113 swigDelegate113;

	// Token: 0x04000610 RID: 1552
	private GLAdapter.SwigDelegateGLAdapter_114 swigDelegate114;

	// Token: 0x04000611 RID: 1553
	private GLAdapter.SwigDelegateGLAdapter_115 swigDelegate115;

	// Token: 0x04000612 RID: 1554
	private GLAdapter.SwigDelegateGLAdapter_116 swigDelegate116;

	// Token: 0x04000613 RID: 1555
	private GLAdapter.SwigDelegateGLAdapter_117 swigDelegate117;

	// Token: 0x04000614 RID: 1556
	private GLAdapter.SwigDelegateGLAdapter_118 swigDelegate118;

	// Token: 0x04000615 RID: 1557
	private GLAdapter.SwigDelegateGLAdapter_119 swigDelegate119;

	// Token: 0x04000616 RID: 1558
	private GLAdapter.SwigDelegateGLAdapter_120 swigDelegate120;

	// Token: 0x04000617 RID: 1559
	private GLAdapter.SwigDelegateGLAdapter_121 swigDelegate121;

	// Token: 0x04000618 RID: 1560
	private GLAdapter.SwigDelegateGLAdapter_122 swigDelegate122;

	// Token: 0x04000619 RID: 1561
	private GLAdapter.SwigDelegateGLAdapter_123 swigDelegate123;

	// Token: 0x0400061A RID: 1562
	private GLAdapter.SwigDelegateGLAdapter_124 swigDelegate124;

	// Token: 0x0400061B RID: 1563
	private GLAdapter.SwigDelegateGLAdapter_125 swigDelegate125;

	// Token: 0x0400061C RID: 1564
	private GLAdapter.SwigDelegateGLAdapter_126 swigDelegate126;

	// Token: 0x0400061D RID: 1565
	private GLAdapter.SwigDelegateGLAdapter_127 swigDelegate127;

	// Token: 0x0400061E RID: 1566
	private GLAdapter.SwigDelegateGLAdapter_128 swigDelegate128;

	// Token: 0x0400061F RID: 1567
	private GLAdapter.SwigDelegateGLAdapter_129 swigDelegate129;

	// Token: 0x04000620 RID: 1568
	private GLAdapter.SwigDelegateGLAdapter_130 swigDelegate130;

	// Token: 0x04000621 RID: 1569
	private GLAdapter.SwigDelegateGLAdapter_131 swigDelegate131;

	// Token: 0x04000622 RID: 1570
	private GLAdapter.SwigDelegateGLAdapter_132 swigDelegate132;

	// Token: 0x04000623 RID: 1571
	private GLAdapter.SwigDelegateGLAdapter_133 swigDelegate133;

	// Token: 0x04000624 RID: 1572
	private GLAdapter.SwigDelegateGLAdapter_134 swigDelegate134;

	// Token: 0x04000625 RID: 1573
	private GLAdapter.SwigDelegateGLAdapter_135 swigDelegate135;

	// Token: 0x04000626 RID: 1574
	private GLAdapter.SwigDelegateGLAdapter_136 swigDelegate136;

	// Token: 0x04000627 RID: 1575
	private GLAdapter.SwigDelegateGLAdapter_137 swigDelegate137;

	// Token: 0x04000628 RID: 1576
	private GLAdapter.SwigDelegateGLAdapter_138 swigDelegate138;

	// Token: 0x04000629 RID: 1577
	private GLAdapter.SwigDelegateGLAdapter_139 swigDelegate139;

	// Token: 0x0400062A RID: 1578
	private GLAdapter.SwigDelegateGLAdapter_140 swigDelegate140;

	// Token: 0x0400062B RID: 1579
	private GLAdapter.SwigDelegateGLAdapter_141 swigDelegate141;

	// Token: 0x0400062C RID: 1580
	private GLAdapter.SwigDelegateGLAdapter_142 swigDelegate142;

	// Token: 0x0400062D RID: 1581
	private GLAdapter.SwigDelegateGLAdapter_143 swigDelegate143;

	// Token: 0x0400062E RID: 1582
	private GLAdapter.SwigDelegateGLAdapter_144 swigDelegate144;

	// Token: 0x0400062F RID: 1583
	private GLAdapter.SwigDelegateGLAdapter_145 swigDelegate145;

	// Token: 0x04000630 RID: 1584
	private GLAdapter.SwigDelegateGLAdapter_146 swigDelegate146;

	// Token: 0x04000631 RID: 1585
	private GLAdapter.SwigDelegateGLAdapter_147 swigDelegate147;

	// Token: 0x04000632 RID: 1586
	private GLAdapter.SwigDelegateGLAdapter_148 swigDelegate148;

	// Token: 0x04000633 RID: 1587
	private GLAdapter.SwigDelegateGLAdapter_149 swigDelegate149;

	// Token: 0x04000634 RID: 1588
	private GLAdapter.SwigDelegateGLAdapter_150 swigDelegate150;

	// Token: 0x04000635 RID: 1589
	private GLAdapter.SwigDelegateGLAdapter_151 swigDelegate151;

	// Token: 0x04000636 RID: 1590
	private GLAdapter.SwigDelegateGLAdapter_152 swigDelegate152;

	// Token: 0x04000637 RID: 1591
	private GLAdapter.SwigDelegateGLAdapter_153 swigDelegate153;

	// Token: 0x04000638 RID: 1592
	private GLAdapter.SwigDelegateGLAdapter_154 swigDelegate154;

	// Token: 0x04000639 RID: 1593
	private GLAdapter.SwigDelegateGLAdapter_155 swigDelegate155;

	// Token: 0x0400063A RID: 1594
	private GLAdapter.SwigDelegateGLAdapter_156 swigDelegate156;

	// Token: 0x0400063B RID: 1595
	private GLAdapter.SwigDelegateGLAdapter_157 swigDelegate157;

	// Token: 0x0400063C RID: 1596
	private GLAdapter.SwigDelegateGLAdapter_158 swigDelegate158;

	// Token: 0x0400063D RID: 1597
	private GLAdapter.SwigDelegateGLAdapter_159 swigDelegate159;

	// Token: 0x0400063E RID: 1598
	private GLAdapter.SwigDelegateGLAdapter_160 swigDelegate160;

	// Token: 0x0400063F RID: 1599
	private GLAdapter.SwigDelegateGLAdapter_161 swigDelegate161;

	// Token: 0x04000640 RID: 1600
	private GLAdapter.SwigDelegateGLAdapter_162 swigDelegate162;

	// Token: 0x04000641 RID: 1601
	private GLAdapter.SwigDelegateGLAdapter_163 swigDelegate163;

	// Token: 0x04000642 RID: 1602
	private GLAdapter.SwigDelegateGLAdapter_164 swigDelegate164;

	// Token: 0x04000643 RID: 1603
	private GLAdapter.SwigDelegateGLAdapter_165 swigDelegate165;

	// Token: 0x04000644 RID: 1604
	private GLAdapter.SwigDelegateGLAdapter_166 swigDelegate166;

	// Token: 0x04000645 RID: 1605
	private GLAdapter.SwigDelegateGLAdapter_167 swigDelegate167;

	// Token: 0x04000646 RID: 1606
	private GLAdapter.SwigDelegateGLAdapter_168 swigDelegate168;

	// Token: 0x04000647 RID: 1607
	private GLAdapter.SwigDelegateGLAdapter_169 swigDelegate169;

	// Token: 0x04000648 RID: 1608
	private GLAdapter.SwigDelegateGLAdapter_170 swigDelegate170;

	// Token: 0x04000649 RID: 1609
	private GLAdapter.SwigDelegateGLAdapter_171 swigDelegate171;

	// Token: 0x0400064A RID: 1610
	private GLAdapter.SwigDelegateGLAdapter_172 swigDelegate172;

	// Token: 0x0400064B RID: 1611
	private GLAdapter.SwigDelegateGLAdapter_173 swigDelegate173;

	// Token: 0x0400064C RID: 1612
	private GLAdapter.SwigDelegateGLAdapter_174 swigDelegate174;

	// Token: 0x0400064D RID: 1613
	private GLAdapter.SwigDelegateGLAdapter_175 swigDelegate175;

	// Token: 0x0400064E RID: 1614
	private GLAdapter.SwigDelegateGLAdapter_176 swigDelegate176;

	// Token: 0x0400064F RID: 1615
	private GLAdapter.SwigDelegateGLAdapter_177 swigDelegate177;

	// Token: 0x04000650 RID: 1616
	private GLAdapter.SwigDelegateGLAdapter_178 swigDelegate178;

	// Token: 0x04000651 RID: 1617
	private GLAdapter.SwigDelegateGLAdapter_179 swigDelegate179;

	// Token: 0x04000652 RID: 1618
	private GLAdapter.SwigDelegateGLAdapter_180 swigDelegate180;

	// Token: 0x04000653 RID: 1619
	private GLAdapter.SwigDelegateGLAdapter_181 swigDelegate181;

	// Token: 0x04000654 RID: 1620
	private GLAdapter.SwigDelegateGLAdapter_182 swigDelegate182;

	// Token: 0x04000655 RID: 1621
	private GLAdapter.SwigDelegateGLAdapter_183 swigDelegate183;

	// Token: 0x04000656 RID: 1622
	private GLAdapter.SwigDelegateGLAdapter_184 swigDelegate184;

	// Token: 0x04000657 RID: 1623
	private GLAdapter.SwigDelegateGLAdapter_185 swigDelegate185;

	// Token: 0x04000658 RID: 1624
	private GLAdapter.SwigDelegateGLAdapter_186 swigDelegate186;

	// Token: 0x04000659 RID: 1625
	private GLAdapter.SwigDelegateGLAdapter_187 swigDelegate187;

	// Token: 0x0400065A RID: 1626
	private GLAdapter.SwigDelegateGLAdapter_188 swigDelegate188;

	// Token: 0x0400065B RID: 1627
	private GLAdapter.SwigDelegateGLAdapter_189 swigDelegate189;

	// Token: 0x0400065C RID: 1628
	private GLAdapter.SwigDelegateGLAdapter_190 swigDelegate190;

	// Token: 0x0400065D RID: 1629
	private GLAdapter.SwigDelegateGLAdapter_191 swigDelegate191;

	// Token: 0x0400065E RID: 1630
	private GLAdapter.SwigDelegateGLAdapter_192 swigDelegate192;

	// Token: 0x0400065F RID: 1631
	private GLAdapter.SwigDelegateGLAdapter_193 swigDelegate193;

	// Token: 0x04000660 RID: 1632
	private GLAdapter.SwigDelegateGLAdapter_194 swigDelegate194;

	// Token: 0x04000661 RID: 1633
	private GLAdapter.SwigDelegateGLAdapter_195 swigDelegate195;

	// Token: 0x04000662 RID: 1634
	private GLAdapter.SwigDelegateGLAdapter_196 swigDelegate196;

	// Token: 0x04000663 RID: 1635
	private GLAdapter.SwigDelegateGLAdapter_197 swigDelegate197;

	// Token: 0x04000664 RID: 1636
	private GLAdapter.SwigDelegateGLAdapter_198 swigDelegate198;

	// Token: 0x04000665 RID: 1637
	private GLAdapter.SwigDelegateGLAdapter_199 swigDelegate199;

	// Token: 0x04000666 RID: 1638
	private GLAdapter.SwigDelegateGLAdapter_200 swigDelegate200;

	// Token: 0x04000667 RID: 1639
	private GLAdapter.SwigDelegateGLAdapter_201 swigDelegate201;

	// Token: 0x04000668 RID: 1640
	private GLAdapter.SwigDelegateGLAdapter_202 swigDelegate202;

	// Token: 0x04000669 RID: 1641
	private GLAdapter.SwigDelegateGLAdapter_203 swigDelegate203;

	// Token: 0x0400066A RID: 1642
	private GLAdapter.SwigDelegateGLAdapter_204 swigDelegate204;

	// Token: 0x0400066B RID: 1643
	private GLAdapter.SwigDelegateGLAdapter_205 swigDelegate205;

	// Token: 0x0400066C RID: 1644
	private GLAdapter.SwigDelegateGLAdapter_206 swigDelegate206;

	// Token: 0x0400066D RID: 1645
	private GLAdapter.SwigDelegateGLAdapter_207 swigDelegate207;

	// Token: 0x0400066E RID: 1646
	private GLAdapter.SwigDelegateGLAdapter_208 swigDelegate208;

	// Token: 0x0400066F RID: 1647
	private GLAdapter.SwigDelegateGLAdapter_209 swigDelegate209;

	// Token: 0x04000670 RID: 1648
	private GLAdapter.SwigDelegateGLAdapter_210 swigDelegate210;

	// Token: 0x04000671 RID: 1649
	private GLAdapter.SwigDelegateGLAdapter_211 swigDelegate211;

	// Token: 0x04000672 RID: 1650
	private GLAdapter.SwigDelegateGLAdapter_212 swigDelegate212;

	// Token: 0x04000673 RID: 1651
	private GLAdapter.SwigDelegateGLAdapter_213 swigDelegate213;

	// Token: 0x04000674 RID: 1652
	private GLAdapter.SwigDelegateGLAdapter_214 swigDelegate214;

	// Token: 0x04000675 RID: 1653
	private GLAdapter.SwigDelegateGLAdapter_215 swigDelegate215;

	// Token: 0x04000676 RID: 1654
	private GLAdapter.SwigDelegateGLAdapter_216 swigDelegate216;

	// Token: 0x04000677 RID: 1655
	private GLAdapter.SwigDelegateGLAdapter_217 swigDelegate217;

	// Token: 0x04000678 RID: 1656
	private GLAdapter.SwigDelegateGLAdapter_218 swigDelegate218;

	// Token: 0x04000679 RID: 1657
	private GLAdapter.SwigDelegateGLAdapter_219 swigDelegate219;

	// Token: 0x0400067A RID: 1658
	private GLAdapter.SwigDelegateGLAdapter_220 swigDelegate220;

	// Token: 0x0400067B RID: 1659
	private GLAdapter.SwigDelegateGLAdapter_221 swigDelegate221;

	// Token: 0x0400067C RID: 1660
	private GLAdapter.SwigDelegateGLAdapter_222 swigDelegate222;

	// Token: 0x0400067D RID: 1661
	private GLAdapter.SwigDelegateGLAdapter_223 swigDelegate223;

	// Token: 0x0400067E RID: 1662
	private GLAdapter.SwigDelegateGLAdapter_224 swigDelegate224;

	// Token: 0x0400067F RID: 1663
	private GLAdapter.SwigDelegateGLAdapter_225 swigDelegate225;

	// Token: 0x04000680 RID: 1664
	private GLAdapter.SwigDelegateGLAdapter_226 swigDelegate226;

	// Token: 0x04000681 RID: 1665
	private GLAdapter.SwigDelegateGLAdapter_227 swigDelegate227;

	// Token: 0x04000682 RID: 1666
	private GLAdapter.SwigDelegateGLAdapter_228 swigDelegate228;

	// Token: 0x04000683 RID: 1667
	private GLAdapter.SwigDelegateGLAdapter_229 swigDelegate229;

	// Token: 0x04000684 RID: 1668
	private GLAdapter.SwigDelegateGLAdapter_230 swigDelegate230;

	// Token: 0x04000685 RID: 1669
	private GLAdapter.SwigDelegateGLAdapter_231 swigDelegate231;

	// Token: 0x04000686 RID: 1670
	private GLAdapter.SwigDelegateGLAdapter_232 swigDelegate232;

	// Token: 0x04000687 RID: 1671
	private GLAdapter.SwigDelegateGLAdapter_233 swigDelegate233;

	// Token: 0x04000688 RID: 1672
	private GLAdapter.SwigDelegateGLAdapter_234 swigDelegate234;

	// Token: 0x04000689 RID: 1673
	private GLAdapter.SwigDelegateGLAdapter_235 swigDelegate235;

	// Token: 0x0400068A RID: 1674
	private GLAdapter.SwigDelegateGLAdapter_236 swigDelegate236;

	// Token: 0x0400068B RID: 1675
	private GLAdapter.SwigDelegateGLAdapter_237 swigDelegate237;

	// Token: 0x0400068C RID: 1676
	private GLAdapter.SwigDelegateGLAdapter_238 swigDelegate238;

	// Token: 0x0400068D RID: 1677
	private GLAdapter.SwigDelegateGLAdapter_239 swigDelegate239;

	// Token: 0x0400068E RID: 1678
	private GLAdapter.SwigDelegateGLAdapter_240 swigDelegate240;

	// Token: 0x0400068F RID: 1679
	private GLAdapter.SwigDelegateGLAdapter_241 swigDelegate241;

	// Token: 0x04000690 RID: 1680
	private GLAdapter.SwigDelegateGLAdapter_242 swigDelegate242;

	// Token: 0x04000691 RID: 1681
	private GLAdapter.SwigDelegateGLAdapter_243 swigDelegate243;

	// Token: 0x04000692 RID: 1682
	private GLAdapter.SwigDelegateGLAdapter_244 swigDelegate244;

	// Token: 0x04000693 RID: 1683
	private GLAdapter.SwigDelegateGLAdapter_245 swigDelegate245;

	// Token: 0x04000694 RID: 1684
	private GLAdapter.SwigDelegateGLAdapter_246 swigDelegate246;

	// Token: 0x04000695 RID: 1685
	private GLAdapter.SwigDelegateGLAdapter_247 swigDelegate247;

	// Token: 0x04000696 RID: 1686
	private GLAdapter.SwigDelegateGLAdapter_248 swigDelegate248;

	// Token: 0x04000697 RID: 1687
	private GLAdapter.SwigDelegateGLAdapter_249 swigDelegate249;

	// Token: 0x04000698 RID: 1688
	private GLAdapter.SwigDelegateGLAdapter_250 swigDelegate250;

	// Token: 0x04000699 RID: 1689
	private GLAdapter.SwigDelegateGLAdapter_251 swigDelegate251;

	// Token: 0x0400069A RID: 1690
	private GLAdapter.SwigDelegateGLAdapter_252 swigDelegate252;

	// Token: 0x0400069B RID: 1691
	private GLAdapter.SwigDelegateGLAdapter_253 swigDelegate253;

	// Token: 0x0400069C RID: 1692
	private GLAdapter.SwigDelegateGLAdapter_254 swigDelegate254;

	// Token: 0x0400069D RID: 1693
	private GLAdapter.SwigDelegateGLAdapter_255 swigDelegate255;

	// Token: 0x0400069E RID: 1694
	private GLAdapter.SwigDelegateGLAdapter_256 swigDelegate256;

	// Token: 0x0400069F RID: 1695
	private GLAdapter.SwigDelegateGLAdapter_257 swigDelegate257;

	// Token: 0x040006A0 RID: 1696
	private GLAdapter.SwigDelegateGLAdapter_258 swigDelegate258;

	// Token: 0x040006A1 RID: 1697
	private GLAdapter.SwigDelegateGLAdapter_259 swigDelegate259;

	// Token: 0x040006A2 RID: 1698
	private GLAdapter.SwigDelegateGLAdapter_260 swigDelegate260;

	// Token: 0x040006A3 RID: 1699
	private GLAdapter.SwigDelegateGLAdapter_261 swigDelegate261;

	// Token: 0x040006A4 RID: 1700
	private GLAdapter.SwigDelegateGLAdapter_262 swigDelegate262;

	// Token: 0x040006A5 RID: 1701
	private GLAdapter.SwigDelegateGLAdapter_263 swigDelegate263;

	// Token: 0x040006A6 RID: 1702
	private GLAdapter.SwigDelegateGLAdapter_264 swigDelegate264;

	// Token: 0x040006A7 RID: 1703
	private GLAdapter.SwigDelegateGLAdapter_265 swigDelegate265;

	// Token: 0x040006A8 RID: 1704
	private GLAdapter.SwigDelegateGLAdapter_266 swigDelegate266;

	// Token: 0x040006A9 RID: 1705
	private GLAdapter.SwigDelegateGLAdapter_267 swigDelegate267;

	// Token: 0x040006AA RID: 1706
	private GLAdapter.SwigDelegateGLAdapter_268 swigDelegate268;

	// Token: 0x040006AB RID: 1707
	private GLAdapter.SwigDelegateGLAdapter_269 swigDelegate269;

	// Token: 0x040006AC RID: 1708
	private GLAdapter.SwigDelegateGLAdapter_270 swigDelegate270;

	// Token: 0x040006AD RID: 1709
	private GLAdapter.SwigDelegateGLAdapter_271 swigDelegate271;

	// Token: 0x040006AE RID: 1710
	private GLAdapter.SwigDelegateGLAdapter_272 swigDelegate272;

	// Token: 0x040006AF RID: 1711
	private GLAdapter.SwigDelegateGLAdapter_273 swigDelegate273;

	// Token: 0x040006B0 RID: 1712
	private GLAdapter.SwigDelegateGLAdapter_274 swigDelegate274;

	// Token: 0x040006B1 RID: 1713
	private GLAdapter.SwigDelegateGLAdapter_275 swigDelegate275;

	// Token: 0x040006B2 RID: 1714
	private GLAdapter.SwigDelegateGLAdapter_276 swigDelegate276;

	// Token: 0x040006B3 RID: 1715
	private GLAdapter.SwigDelegateGLAdapter_277 swigDelegate277;

	// Token: 0x040006B4 RID: 1716
	private GLAdapter.SwigDelegateGLAdapter_278 swigDelegate278;

	// Token: 0x040006B5 RID: 1717
	private GLAdapter.SwigDelegateGLAdapter_279 swigDelegate279;

	// Token: 0x040006B6 RID: 1718
	private GLAdapter.SwigDelegateGLAdapter_280 swigDelegate280;

	// Token: 0x040006B7 RID: 1719
	private GLAdapter.SwigDelegateGLAdapter_281 swigDelegate281;

	// Token: 0x040006B8 RID: 1720
	private GLAdapter.SwigDelegateGLAdapter_282 swigDelegate282;

	// Token: 0x040006B9 RID: 1721
	private GLAdapter.SwigDelegateGLAdapter_283 swigDelegate283;

	// Token: 0x040006BA RID: 1722
	private GLAdapter.SwigDelegateGLAdapter_284 swigDelegate284;

	// Token: 0x040006BB RID: 1723
	private GLAdapter.SwigDelegateGLAdapter_285 swigDelegate285;

	// Token: 0x040006BC RID: 1724
	private GLAdapter.SwigDelegateGLAdapter_286 swigDelegate286;

	// Token: 0x040006BD RID: 1725
	private GLAdapter.SwigDelegateGLAdapter_287 swigDelegate287;

	// Token: 0x040006BE RID: 1726
	private GLAdapter.SwigDelegateGLAdapter_288 swigDelegate288;

	// Token: 0x040006BF RID: 1727
	private GLAdapter.SwigDelegateGLAdapter_289 swigDelegate289;

	// Token: 0x040006C0 RID: 1728
	private GLAdapter.SwigDelegateGLAdapter_290 swigDelegate290;

	// Token: 0x040006C1 RID: 1729
	private GLAdapter.SwigDelegateGLAdapter_291 swigDelegate291;

	// Token: 0x040006C2 RID: 1730
	private GLAdapter.SwigDelegateGLAdapter_292 swigDelegate292;

	// Token: 0x040006C3 RID: 1731
	private GLAdapter.SwigDelegateGLAdapter_293 swigDelegate293;

	// Token: 0x040006C4 RID: 1732
	private GLAdapter.SwigDelegateGLAdapter_294 swigDelegate294;

	// Token: 0x040006C5 RID: 1733
	private GLAdapter.SwigDelegateGLAdapter_295 swigDelegate295;

	// Token: 0x040006C6 RID: 1734
	private GLAdapter.SwigDelegateGLAdapter_296 swigDelegate296;

	// Token: 0x040006C7 RID: 1735
	private GLAdapter.SwigDelegateGLAdapter_297 swigDelegate297;

	// Token: 0x040006C8 RID: 1736
	private GLAdapter.SwigDelegateGLAdapter_298 swigDelegate298;

	// Token: 0x040006C9 RID: 1737
	private GLAdapter.SwigDelegateGLAdapter_299 swigDelegate299;

	// Token: 0x040006CA RID: 1738
	private GLAdapter.SwigDelegateGLAdapter_300 swigDelegate300;

	// Token: 0x040006CB RID: 1739
	private GLAdapter.SwigDelegateGLAdapter_301 swigDelegate301;

	// Token: 0x040006CC RID: 1740
	private GLAdapter.SwigDelegateGLAdapter_302 swigDelegate302;

	// Token: 0x040006CD RID: 1741
	private GLAdapter.SwigDelegateGLAdapter_303 swigDelegate303;

	// Token: 0x040006CE RID: 1742
	private GLAdapter.SwigDelegateGLAdapter_304 swigDelegate304;

	// Token: 0x040006CF RID: 1743
	private GLAdapter.SwigDelegateGLAdapter_305 swigDelegate305;

	// Token: 0x040006D0 RID: 1744
	private GLAdapter.SwigDelegateGLAdapter_306 swigDelegate306;

	// Token: 0x040006D1 RID: 1745
	private GLAdapter.SwigDelegateGLAdapter_307 swigDelegate307;

	// Token: 0x040006D2 RID: 1746
	private GLAdapter.SwigDelegateGLAdapter_308 swigDelegate308;

	// Token: 0x040006D3 RID: 1747
	private GLAdapter.SwigDelegateGLAdapter_309 swigDelegate309;

	// Token: 0x040006D4 RID: 1748
	private GLAdapter.SwigDelegateGLAdapter_310 swigDelegate310;

	// Token: 0x040006D5 RID: 1749
	private GLAdapter.SwigDelegateGLAdapter_311 swigDelegate311;

	// Token: 0x040006D6 RID: 1750
	private GLAdapter.SwigDelegateGLAdapter_312 swigDelegate312;

	// Token: 0x040006D7 RID: 1751
	private GLAdapter.SwigDelegateGLAdapter_313 swigDelegate313;

	// Token: 0x040006D8 RID: 1752
	private GLAdapter.SwigDelegateGLAdapter_314 swigDelegate314;

	// Token: 0x040006D9 RID: 1753
	private GLAdapter.SwigDelegateGLAdapter_315 swigDelegate315;

	// Token: 0x040006DA RID: 1754
	private GLAdapter.SwigDelegateGLAdapter_316 swigDelegate316;

	// Token: 0x040006DB RID: 1755
	private GLAdapter.SwigDelegateGLAdapter_317 swigDelegate317;

	// Token: 0x040006DC RID: 1756
	private GLAdapter.SwigDelegateGLAdapter_318 swigDelegate318;

	// Token: 0x040006DD RID: 1757
	private GLAdapter.SwigDelegateGLAdapter_319 swigDelegate319;

	// Token: 0x040006DE RID: 1758
	private GLAdapter.SwigDelegateGLAdapter_320 swigDelegate320;

	// Token: 0x040006DF RID: 1759
	private GLAdapter.SwigDelegateGLAdapter_321 swigDelegate321;

	// Token: 0x040006E0 RID: 1760
	private GLAdapter.SwigDelegateGLAdapter_322 swigDelegate322;

	// Token: 0x040006E1 RID: 1761
	private GLAdapter.SwigDelegateGLAdapter_323 swigDelegate323;

	// Token: 0x040006E2 RID: 1762
	private GLAdapter.SwigDelegateGLAdapter_324 swigDelegate324;

	// Token: 0x040006E3 RID: 1763
	private GLAdapter.SwigDelegateGLAdapter_325 swigDelegate325;

	// Token: 0x040006E4 RID: 1764
	private GLAdapter.SwigDelegateGLAdapter_326 swigDelegate326;

	// Token: 0x040006E5 RID: 1765
	private GLAdapter.SwigDelegateGLAdapter_327 swigDelegate327;

	// Token: 0x040006E6 RID: 1766
	private GLAdapter.SwigDelegateGLAdapter_328 swigDelegate328;

	// Token: 0x040006E7 RID: 1767
	private GLAdapter.SwigDelegateGLAdapter_329 swigDelegate329;

	// Token: 0x040006E8 RID: 1768
	private GLAdapter.SwigDelegateGLAdapter_330 swigDelegate330;

	// Token: 0x040006E9 RID: 1769
	private GLAdapter.SwigDelegateGLAdapter_331 swigDelegate331;

	// Token: 0x040006EA RID: 1770
	private GLAdapter.SwigDelegateGLAdapter_332 swigDelegate332;

	// Token: 0x040006EB RID: 1771
	private GLAdapter.SwigDelegateGLAdapter_333 swigDelegate333;

	// Token: 0x040006EC RID: 1772
	private GLAdapter.SwigDelegateGLAdapter_334 swigDelegate334;

	// Token: 0x040006ED RID: 1773
	private GLAdapter.SwigDelegateGLAdapter_335 swigDelegate335;

	// Token: 0x040006EE RID: 1774
	private GLAdapter.SwigDelegateGLAdapter_336 swigDelegate336;

	// Token: 0x040006EF RID: 1775
	private GLAdapter.SwigDelegateGLAdapter_337 swigDelegate337;

	// Token: 0x040006F0 RID: 1776
	private GLAdapter.SwigDelegateGLAdapter_338 swigDelegate338;

	// Token: 0x040006F1 RID: 1777
	private GLAdapter.SwigDelegateGLAdapter_339 swigDelegate339;

	// Token: 0x040006F2 RID: 1778
	private GLAdapter.SwigDelegateGLAdapter_340 swigDelegate340;

	// Token: 0x040006F3 RID: 1779
	private GLAdapter.SwigDelegateGLAdapter_341 swigDelegate341;

	// Token: 0x040006F4 RID: 1780
	private GLAdapter.SwigDelegateGLAdapter_342 swigDelegate342;

	// Token: 0x040006F5 RID: 1781
	private GLAdapter.SwigDelegateGLAdapter_343 swigDelegate343;

	// Token: 0x040006F6 RID: 1782
	private GLAdapter.SwigDelegateGLAdapter_344 swigDelegate344;

	// Token: 0x040006F7 RID: 1783
	private GLAdapter.SwigDelegateGLAdapter_345 swigDelegate345;

	// Token: 0x040006F8 RID: 1784
	private GLAdapter.SwigDelegateGLAdapter_346 swigDelegate346;

	// Token: 0x040006F9 RID: 1785
	private GLAdapter.SwigDelegateGLAdapter_347 swigDelegate347;

	// Token: 0x040006FA RID: 1786
	private GLAdapter.SwigDelegateGLAdapter_348 swigDelegate348;

	// Token: 0x040006FB RID: 1787
	private GLAdapter.SwigDelegateGLAdapter_349 swigDelegate349;

	// Token: 0x040006FC RID: 1788
	private GLAdapter.SwigDelegateGLAdapter_350 swigDelegate350;

	// Token: 0x040006FD RID: 1789
	private GLAdapter.SwigDelegateGLAdapter_351 swigDelegate351;

	// Token: 0x040006FE RID: 1790
	private GLAdapter.SwigDelegateGLAdapter_352 swigDelegate352;

	// Token: 0x040006FF RID: 1791
	private GLAdapter.SwigDelegateGLAdapter_353 swigDelegate353;

	// Token: 0x04000700 RID: 1792
	private GLAdapter.SwigDelegateGLAdapter_354 swigDelegate354;

	// Token: 0x04000701 RID: 1793
	private GLAdapter.SwigDelegateGLAdapter_355 swigDelegate355;

	// Token: 0x04000702 RID: 1794
	private GLAdapter.SwigDelegateGLAdapter_356 swigDelegate356;

	// Token: 0x04000703 RID: 1795
	private GLAdapter.SwigDelegateGLAdapter_357 swigDelegate357;

	// Token: 0x04000704 RID: 1796
	private GLAdapter.SwigDelegateGLAdapter_358 swigDelegate358;

	// Token: 0x04000705 RID: 1797
	private GLAdapter.SwigDelegateGLAdapter_359 swigDelegate359;

	// Token: 0x04000706 RID: 1798
	private GLAdapter.SwigDelegateGLAdapter_360 swigDelegate360;

	// Token: 0x04000707 RID: 1799
	private GLAdapter.SwigDelegateGLAdapter_361 swigDelegate361;

	// Token: 0x04000708 RID: 1800
	private GLAdapter.SwigDelegateGLAdapter_362 swigDelegate362;

	// Token: 0x04000709 RID: 1801
	private GLAdapter.SwigDelegateGLAdapter_363 swigDelegate363;

	// Token: 0x0400070A RID: 1802
	private GLAdapter.SwigDelegateGLAdapter_364 swigDelegate364;

	// Token: 0x0400070B RID: 1803
	private GLAdapter.SwigDelegateGLAdapter_365 swigDelegate365;

	// Token: 0x0400070C RID: 1804
	private GLAdapter.SwigDelegateGLAdapter_366 swigDelegate366;

	// Token: 0x0400070D RID: 1805
	private GLAdapter.SwigDelegateGLAdapter_367 swigDelegate367;

	// Token: 0x0400070E RID: 1806
	private GLAdapter.SwigDelegateGLAdapter_368 swigDelegate368;

	// Token: 0x0400070F RID: 1807
	private GLAdapter.SwigDelegateGLAdapter_369 swigDelegate369;

	// Token: 0x04000710 RID: 1808
	private GLAdapter.SwigDelegateGLAdapter_370 swigDelegate370;

	// Token: 0x04000711 RID: 1809
	private GLAdapter.SwigDelegateGLAdapter_371 swigDelegate371;

	// Token: 0x04000712 RID: 1810
	private GLAdapter.SwigDelegateGLAdapter_372 swigDelegate372;

	// Token: 0x04000713 RID: 1811
	private GLAdapter.SwigDelegateGLAdapter_373 swigDelegate373;

	// Token: 0x04000714 RID: 1812
	private GLAdapter.SwigDelegateGLAdapter_374 swigDelegate374;

	// Token: 0x04000715 RID: 1813
	private GLAdapter.SwigDelegateGLAdapter_375 swigDelegate375;

	// Token: 0x04000716 RID: 1814
	private GLAdapter.SwigDelegateGLAdapter_376 swigDelegate376;

	// Token: 0x04000717 RID: 1815
	private GLAdapter.SwigDelegateGLAdapter_377 swigDelegate377;

	// Token: 0x04000718 RID: 1816
	private GLAdapter.SwigDelegateGLAdapter_378 swigDelegate378;

	// Token: 0x04000719 RID: 1817
	private GLAdapter.SwigDelegateGLAdapter_379 swigDelegate379;

	// Token: 0x0400071A RID: 1818
	private GLAdapter.SwigDelegateGLAdapter_380 swigDelegate380;

	// Token: 0x0400071B RID: 1819
	private GLAdapter.SwigDelegateGLAdapter_381 swigDelegate381;

	// Token: 0x0400071C RID: 1820
	private GLAdapter.SwigDelegateGLAdapter_382 swigDelegate382;

	// Token: 0x0400071D RID: 1821
	private GLAdapter.SwigDelegateGLAdapter_383 swigDelegate383;

	// Token: 0x0400071E RID: 1822
	private GLAdapter.SwigDelegateGLAdapter_384 swigDelegate384;

	// Token: 0x0400071F RID: 1823
	private GLAdapter.SwigDelegateGLAdapter_385 swigDelegate385;

	// Token: 0x04000720 RID: 1824
	private GLAdapter.SwigDelegateGLAdapter_386 swigDelegate386;

	// Token: 0x04000721 RID: 1825
	private GLAdapter.SwigDelegateGLAdapter_387 swigDelegate387;

	// Token: 0x04000722 RID: 1826
	private GLAdapter.SwigDelegateGLAdapter_388 swigDelegate388;

	// Token: 0x04000723 RID: 1827
	private GLAdapter.SwigDelegateGLAdapter_389 swigDelegate389;

	// Token: 0x04000724 RID: 1828
	private GLAdapter.SwigDelegateGLAdapter_390 swigDelegate390;

	// Token: 0x04000725 RID: 1829
	private GLAdapter.SwigDelegateGLAdapter_391 swigDelegate391;

	// Token: 0x04000726 RID: 1830
	private GLAdapter.SwigDelegateGLAdapter_392 swigDelegate392;

	// Token: 0x04000727 RID: 1831
	private GLAdapter.SwigDelegateGLAdapter_393 swigDelegate393;

	// Token: 0x04000728 RID: 1832
	private GLAdapter.SwigDelegateGLAdapter_394 swigDelegate394;

	// Token: 0x04000729 RID: 1833
	private GLAdapter.SwigDelegateGLAdapter_395 swigDelegate395;

	// Token: 0x0400072A RID: 1834
	private GLAdapter.SwigDelegateGLAdapter_396 swigDelegate396;

	// Token: 0x0400072B RID: 1835
	private GLAdapter.SwigDelegateGLAdapter_397 swigDelegate397;

	// Token: 0x0400072C RID: 1836
	private GLAdapter.SwigDelegateGLAdapter_398 swigDelegate398;

	// Token: 0x0400072D RID: 1837
	private GLAdapter.SwigDelegateGLAdapter_399 swigDelegate399;

	// Token: 0x0400072E RID: 1838
	private GLAdapter.SwigDelegateGLAdapter_400 swigDelegate400;

	// Token: 0x0400072F RID: 1839
	private GLAdapter.SwigDelegateGLAdapter_401 swigDelegate401;

	// Token: 0x04000730 RID: 1840
	private GLAdapter.SwigDelegateGLAdapter_402 swigDelegate402;

	// Token: 0x04000731 RID: 1841
	private GLAdapter.SwigDelegateGLAdapter_403 swigDelegate403;

	// Token: 0x04000732 RID: 1842
	private GLAdapter.SwigDelegateGLAdapter_404 swigDelegate404;

	// Token: 0x04000733 RID: 1843
	private GLAdapter.SwigDelegateGLAdapter_405 swigDelegate405;

	// Token: 0x04000734 RID: 1844
	private GLAdapter.SwigDelegateGLAdapter_406 swigDelegate406;

	// Token: 0x04000735 RID: 1845
	private GLAdapter.SwigDelegateGLAdapter_407 swigDelegate407;

	// Token: 0x04000736 RID: 1846
	private GLAdapter.SwigDelegateGLAdapter_408 swigDelegate408;

	// Token: 0x04000737 RID: 1847
	private GLAdapter.SwigDelegateGLAdapter_409 swigDelegate409;

	// Token: 0x04000738 RID: 1848
	private GLAdapter.SwigDelegateGLAdapter_410 swigDelegate410;

	// Token: 0x04000739 RID: 1849
	private GLAdapter.SwigDelegateGLAdapter_411 swigDelegate411;

	// Token: 0x0400073A RID: 1850
	private GLAdapter.SwigDelegateGLAdapter_412 swigDelegate412;

	// Token: 0x0400073B RID: 1851
	private GLAdapter.SwigDelegateGLAdapter_413 swigDelegate413;

	// Token: 0x0400073C RID: 1852
	private GLAdapter.SwigDelegateGLAdapter_414 swigDelegate414;

	// Token: 0x0400073D RID: 1853
	private GLAdapter.SwigDelegateGLAdapter_415 swigDelegate415;

	// Token: 0x0400073E RID: 1854
	private GLAdapter.SwigDelegateGLAdapter_416 swigDelegate416;

	// Token: 0x0400073F RID: 1855
	private GLAdapter.SwigDelegateGLAdapter_417 swigDelegate417;

	// Token: 0x04000740 RID: 1856
	private GLAdapter.SwigDelegateGLAdapter_418 swigDelegate418;

	// Token: 0x04000741 RID: 1857
	private GLAdapter.SwigDelegateGLAdapter_419 swigDelegate419;

	// Token: 0x04000742 RID: 1858
	private GLAdapter.SwigDelegateGLAdapter_420 swigDelegate420;

	// Token: 0x04000743 RID: 1859
	private GLAdapter.SwigDelegateGLAdapter_421 swigDelegate421;

	// Token: 0x04000744 RID: 1860
	private GLAdapter.SwigDelegateGLAdapter_422 swigDelegate422;

	// Token: 0x04000745 RID: 1861
	private GLAdapter.SwigDelegateGLAdapter_423 swigDelegate423;

	// Token: 0x04000746 RID: 1862
	private GLAdapter.SwigDelegateGLAdapter_424 swigDelegate424;

	// Token: 0x04000747 RID: 1863
	private GLAdapter.SwigDelegateGLAdapter_425 swigDelegate425;

	// Token: 0x04000748 RID: 1864
	private GLAdapter.SwigDelegateGLAdapter_426 swigDelegate426;

	// Token: 0x04000749 RID: 1865
	private GLAdapter.SwigDelegateGLAdapter_427 swigDelegate427;

	// Token: 0x0400074A RID: 1866
	private GLAdapter.SwigDelegateGLAdapter_428 swigDelegate428;

	// Token: 0x0400074B RID: 1867
	private GLAdapter.SwigDelegateGLAdapter_429 swigDelegate429;

	// Token: 0x0400074C RID: 1868
	private GLAdapter.SwigDelegateGLAdapter_430 swigDelegate430;

	// Token: 0x0400074D RID: 1869
	private GLAdapter.SwigDelegateGLAdapter_431 swigDelegate431;

	// Token: 0x0400074E RID: 1870
	private GLAdapter.SwigDelegateGLAdapter_432 swigDelegate432;

	// Token: 0x0400074F RID: 1871
	private GLAdapter.SwigDelegateGLAdapter_433 swigDelegate433;

	// Token: 0x04000750 RID: 1872
	private GLAdapter.SwigDelegateGLAdapter_434 swigDelegate434;

	// Token: 0x04000751 RID: 1873
	private GLAdapter.SwigDelegateGLAdapter_435 swigDelegate435;

	// Token: 0x04000752 RID: 1874
	private GLAdapter.SwigDelegateGLAdapter_436 swigDelegate436;

	// Token: 0x04000753 RID: 1875
	private GLAdapter.SwigDelegateGLAdapter_437 swigDelegate437;

	// Token: 0x04000754 RID: 1876
	private GLAdapter.SwigDelegateGLAdapter_438 swigDelegate438;

	// Token: 0x04000755 RID: 1877
	private GLAdapter.SwigDelegateGLAdapter_439 swigDelegate439;

	// Token: 0x04000756 RID: 1878
	private GLAdapter.SwigDelegateGLAdapter_440 swigDelegate440;

	// Token: 0x04000757 RID: 1879
	private GLAdapter.SwigDelegateGLAdapter_441 swigDelegate441;

	// Token: 0x04000758 RID: 1880
	private GLAdapter.SwigDelegateGLAdapter_442 swigDelegate442;

	// Token: 0x04000759 RID: 1881
	private GLAdapter.SwigDelegateGLAdapter_443 swigDelegate443;

	// Token: 0x0400075A RID: 1882
	private GLAdapter.SwigDelegateGLAdapter_444 swigDelegate444;

	// Token: 0x0400075B RID: 1883
	private GLAdapter.SwigDelegateGLAdapter_445 swigDelegate445;

	// Token: 0x0400075C RID: 1884
	private GLAdapter.SwigDelegateGLAdapter_446 swigDelegate446;

	// Token: 0x0400075D RID: 1885
	private GLAdapter.SwigDelegateGLAdapter_447 swigDelegate447;

	// Token: 0x0400075E RID: 1886
	private GLAdapter.SwigDelegateGLAdapter_448 swigDelegate448;

	// Token: 0x0400075F RID: 1887
	private GLAdapter.SwigDelegateGLAdapter_449 swigDelegate449;

	// Token: 0x04000760 RID: 1888
	private GLAdapter.SwigDelegateGLAdapter_450 swigDelegate450;

	// Token: 0x04000761 RID: 1889
	private GLAdapter.SwigDelegateGLAdapter_451 swigDelegate451;

	// Token: 0x04000762 RID: 1890
	private GLAdapter.SwigDelegateGLAdapter_452 swigDelegate452;

	// Token: 0x04000763 RID: 1891
	private GLAdapter.SwigDelegateGLAdapter_453 swigDelegate453;

	// Token: 0x04000764 RID: 1892
	private GLAdapter.SwigDelegateGLAdapter_454 swigDelegate454;

	// Token: 0x04000765 RID: 1893
	private GLAdapter.SwigDelegateGLAdapter_455 swigDelegate455;

	// Token: 0x04000766 RID: 1894
	private GLAdapter.SwigDelegateGLAdapter_456 swigDelegate456;

	// Token: 0x04000767 RID: 1895
	private GLAdapter.SwigDelegateGLAdapter_457 swigDelegate457;

	// Token: 0x04000768 RID: 1896
	private GLAdapter.SwigDelegateGLAdapter_458 swigDelegate458;

	// Token: 0x04000769 RID: 1897
	private GLAdapter.SwigDelegateGLAdapter_459 swigDelegate459;

	// Token: 0x0400076A RID: 1898
	private GLAdapter.SwigDelegateGLAdapter_460 swigDelegate460;

	// Token: 0x0400076B RID: 1899
	private GLAdapter.SwigDelegateGLAdapter_461 swigDelegate461;

	// Token: 0x0400076C RID: 1900
	private GLAdapter.SwigDelegateGLAdapter_462 swigDelegate462;

	// Token: 0x0400076D RID: 1901
	private GLAdapter.SwigDelegateGLAdapter_463 swigDelegate463;

	// Token: 0x0400076E RID: 1902
	private GLAdapter.SwigDelegateGLAdapter_464 swigDelegate464;

	// Token: 0x0400076F RID: 1903
	private GLAdapter.SwigDelegateGLAdapter_465 swigDelegate465;

	// Token: 0x04000770 RID: 1904
	private GLAdapter.SwigDelegateGLAdapter_466 swigDelegate466;

	// Token: 0x04000771 RID: 1905
	private GLAdapter.SwigDelegateGLAdapter_467 swigDelegate467;

	// Token: 0x04000772 RID: 1906
	private GLAdapter.SwigDelegateGLAdapter_468 swigDelegate468;

	// Token: 0x04000773 RID: 1907
	private GLAdapter.SwigDelegateGLAdapter_469 swigDelegate469;

	// Token: 0x04000774 RID: 1908
	private GLAdapter.SwigDelegateGLAdapter_470 swigDelegate470;

	// Token: 0x04000775 RID: 1909
	private GLAdapter.SwigDelegateGLAdapter_471 swigDelegate471;

	// Token: 0x04000776 RID: 1910
	private GLAdapter.SwigDelegateGLAdapter_472 swigDelegate472;

	// Token: 0x04000777 RID: 1911
	private GLAdapter.SwigDelegateGLAdapter_473 swigDelegate473;

	// Token: 0x04000778 RID: 1912
	private GLAdapter.SwigDelegateGLAdapter_474 swigDelegate474;

	// Token: 0x04000779 RID: 1913
	private GLAdapter.SwigDelegateGLAdapter_475 swigDelegate475;

	// Token: 0x0400077A RID: 1914
	private GLAdapter.SwigDelegateGLAdapter_476 swigDelegate476;

	// Token: 0x0400077B RID: 1915
	private GLAdapter.SwigDelegateGLAdapter_477 swigDelegate477;

	// Token: 0x0400077C RID: 1916
	private GLAdapter.SwigDelegateGLAdapter_478 swigDelegate478;

	// Token: 0x0400077D RID: 1917
	private GLAdapter.SwigDelegateGLAdapter_479 swigDelegate479;

	// Token: 0x0400077E RID: 1918
	private GLAdapter.SwigDelegateGLAdapter_480 swigDelegate480;

	// Token: 0x0400077F RID: 1919
	private GLAdapter.SwigDelegateGLAdapter_481 swigDelegate481;

	// Token: 0x04000780 RID: 1920
	private GLAdapter.SwigDelegateGLAdapter_482 swigDelegate482;

	// Token: 0x04000781 RID: 1921
	private GLAdapter.SwigDelegateGLAdapter_483 swigDelegate483;

	// Token: 0x04000782 RID: 1922
	private GLAdapter.SwigDelegateGLAdapter_484 swigDelegate484;

	// Token: 0x04000783 RID: 1923
	private GLAdapter.SwigDelegateGLAdapter_485 swigDelegate485;

	// Token: 0x04000784 RID: 1924
	private GLAdapter.SwigDelegateGLAdapter_486 swigDelegate486;

	// Token: 0x04000785 RID: 1925
	private GLAdapter.SwigDelegateGLAdapter_487 swigDelegate487;

	// Token: 0x04000786 RID: 1926
	private GLAdapter.SwigDelegateGLAdapter_488 swigDelegate488;

	// Token: 0x04000787 RID: 1927
	private GLAdapter.SwigDelegateGLAdapter_489 swigDelegate489;

	// Token: 0x04000788 RID: 1928
	private GLAdapter.SwigDelegateGLAdapter_490 swigDelegate490;

	// Token: 0x04000789 RID: 1929
	private GLAdapter.SwigDelegateGLAdapter_491 swigDelegate491;

	// Token: 0x0400078A RID: 1930
	private GLAdapter.SwigDelegateGLAdapter_492 swigDelegate492;

	// Token: 0x0400078B RID: 1931
	private GLAdapter.SwigDelegateGLAdapter_493 swigDelegate493;

	// Token: 0x0400078C RID: 1932
	private GLAdapter.SwigDelegateGLAdapter_494 swigDelegate494;

	// Token: 0x0400078D RID: 1933
	private GLAdapter.SwigDelegateGLAdapter_495 swigDelegate495;

	// Token: 0x0400078E RID: 1934
	private GLAdapter.SwigDelegateGLAdapter_496 swigDelegate496;

	// Token: 0x0400078F RID: 1935
	private GLAdapter.SwigDelegateGLAdapter_497 swigDelegate497;

	// Token: 0x04000790 RID: 1936
	private GLAdapter.SwigDelegateGLAdapter_498 swigDelegate498;

	// Token: 0x04000791 RID: 1937
	private GLAdapter.SwigDelegateGLAdapter_499 swigDelegate499;

	// Token: 0x04000792 RID: 1938
	private GLAdapter.SwigDelegateGLAdapter_500 swigDelegate500;

	// Token: 0x04000793 RID: 1939
	private GLAdapter.SwigDelegateGLAdapter_501 swigDelegate501;

	// Token: 0x04000794 RID: 1940
	private GLAdapter.SwigDelegateGLAdapter_502 swigDelegate502;

	// Token: 0x04000795 RID: 1941
	private GLAdapter.SwigDelegateGLAdapter_503 swigDelegate503;

	// Token: 0x04000796 RID: 1942
	private GLAdapter.SwigDelegateGLAdapter_504 swigDelegate504;

	// Token: 0x04000797 RID: 1943
	private GLAdapter.SwigDelegateGLAdapter_505 swigDelegate505;

	// Token: 0x04000798 RID: 1944
	private GLAdapter.SwigDelegateGLAdapter_506 swigDelegate506;

	// Token: 0x04000799 RID: 1945
	private GLAdapter.SwigDelegateGLAdapter_507 swigDelegate507;

	// Token: 0x0400079A RID: 1946
	private GLAdapter.SwigDelegateGLAdapter_508 swigDelegate508;

	// Token: 0x0400079B RID: 1947
	private GLAdapter.SwigDelegateGLAdapter_509 swigDelegate509;

	// Token: 0x0400079C RID: 1948
	private GLAdapter.SwigDelegateGLAdapter_510 swigDelegate510;

	// Token: 0x0400079D RID: 1949
	private static Type[] swigMethodTypes0 = new Type[] { typeof(uint) };

	// Token: 0x0400079E RID: 1950
	private static Type[] swigMethodTypes1 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(IntPtr),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400079F RID: 1951
	private static Type[] swigMethodTypes2 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(IntPtr),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007A0 RID: 1952
	private static Type[] swigMethodTypes3 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040007A1 RID: 1953
	private static Type[] swigMethodTypes4 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(IntPtr)
	};

	// Token: 0x040007A2 RID: 1954
	private static Type[] swigMethodTypes5 = new Type[] { typeof(uint) };

	// Token: 0x040007A3 RID: 1955
	private static Type[] swigMethodTypes6 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007A4 RID: 1956
	private static Type[] swigMethodTypes7 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007A5 RID: 1957
	private static Type[] swigMethodTypes8 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007A6 RID: 1958
	private static Type[] swigMethodTypes9 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007A7 RID: 1959
	private static Type[] swigMethodTypes10 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007A8 RID: 1960
	private static Type[] swigMethodTypes11 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007A9 RID: 1961
	private static Type[] swigMethodTypes12 = new Type[]
	{
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x040007AA RID: 1962
	private static Type[] swigMethodTypes13 = new Type[] { typeof(uint) };

	// Token: 0x040007AB RID: 1963
	private static Type[] swigMethodTypes14 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007AC RID: 1964
	private static Type[] swigMethodTypes15 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007AD RID: 1965
	private static Type[] swigMethodTypes16 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007AE RID: 1966
	private static Type[] swigMethodTypes17 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(uint)
	};

	// Token: 0x040007AF RID: 1967
	private static Type[] swigMethodTypes18 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007B0 RID: 1968
	private static Type[] swigMethodTypes19 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007B1 RID: 1969
	private static Type[] swigMethodTypes20 = new Type[] { typeof(uint) };

	// Token: 0x040007B2 RID: 1970
	private static Type[] swigMethodTypes21 = new Type[]
	{
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x040007B3 RID: 1971
	private static Type[] swigMethodTypes22 = new Type[] { typeof(float) };

	// Token: 0x040007B4 RID: 1972
	private static Type[] swigMethodTypes23 = new Type[] { typeof(int) };

	// Token: 0x040007B5 RID: 1973
	private static Type[] swigMethodTypes24 = new Type[]
	{
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040007B6 RID: 1974
	private static Type[] swigMethodTypes25 = new Type[] { typeof(uint) };

	// Token: 0x040007B7 RID: 1975
	private static Type[] swigMethodTypes26 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007B8 RID: 1976
	private static Type[] swigMethodTypes27 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007B9 RID: 1977
	private static Type[] swigMethodTypes28 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040007BA RID: 1978
	private static Type[] swigMethodTypes29 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040007BB RID: 1979
	private static Type[] swigMethodTypes30 = new Type[] { typeof(uint) };

	// Token: 0x040007BC RID: 1980
	private static Type[] swigMethodTypes31 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007BD RID: 1981
	private static Type[] swigMethodTypes32 = new Type[] { typeof(uint) };

	// Token: 0x040007BE RID: 1982
	private static Type[] swigMethodTypes33 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007BF RID: 1983
	private static Type[] swigMethodTypes34 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007C0 RID: 1984
	private static Type[] swigMethodTypes35 = new Type[] { typeof(uint) };

	// Token: 0x040007C1 RID: 1985
	private static Type[] swigMethodTypes36 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007C2 RID: 1986
	private static Type[] swigMethodTypes37 = new Type[] { typeof(uint) };

	// Token: 0x040007C3 RID: 1987
	private static Type[] swigMethodTypes38 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007C4 RID: 1988
	private static Type[] swigMethodTypes39 = new Type[] { typeof(uint) };

	// Token: 0x040007C5 RID: 1989
	private static Type[] swigMethodTypes40 = new Type[] { typeof(int) };

	// Token: 0x040007C6 RID: 1990
	private static Type[] swigMethodTypes41 = new Type[]
	{
		typeof(float),
		typeof(float)
	};

	// Token: 0x040007C7 RID: 1991
	private static Type[] swigMethodTypes42 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007C8 RID: 1992
	private static Type[] swigMethodTypes43 = new Type[] { typeof(uint) };

	// Token: 0x040007C9 RID: 1993
	private static Type[] swigMethodTypes44 = new Type[] { typeof(uint) };

	// Token: 0x040007CA RID: 1994
	private static Type[] swigMethodTypes45 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040007CB RID: 1995
	private static Type[] swigMethodTypes46 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007CC RID: 1996
	private static Type[] swigMethodTypes47 = new Type[] { typeof(uint) };

	// Token: 0x040007CD RID: 1997
	private static Type[] swigMethodTypes48 = new Type[] { typeof(uint) };

	// Token: 0x040007CE RID: 1998
	private static Type[] swigMethodTypes49 = new Type[0];

	// Token: 0x040007CF RID: 1999
	private static Type[] swigMethodTypes50 = new Type[0];

	// Token: 0x040007D0 RID: 2000
	private static Type[] swigMethodTypes51 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007D1 RID: 2001
	private static Type[] swigMethodTypes52 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x040007D2 RID: 2002
	private static Type[] swigMethodTypes53 = new Type[] { typeof(uint) };

	// Token: 0x040007D3 RID: 2003
	private static Type[] swigMethodTypes54 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007D4 RID: 2004
	private static Type[] swigMethodTypes55 = new Type[] { typeof(uint) };

	// Token: 0x040007D5 RID: 2005
	private static Type[] swigMethodTypes56 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007D6 RID: 2006
	private static Type[] swigMethodTypes57 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007D7 RID: 2007
	private static Type[] swigMethodTypes58 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040007D8 RID: 2008
	private static Type[] swigMethodTypes59 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040007D9 RID: 2009
	private static Type[] swigMethodTypes60 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040007DA RID: 2010
	private static Type[] swigMethodTypes61 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040007DB RID: 2011
	private static Type[] swigMethodTypes62 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007DC RID: 2012
	private static Type[] swigMethodTypes63 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007DD RID: 2013
	private static Type[] swigMethodTypes64 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007DE RID: 2014
	private static Type[] swigMethodTypes65 = new Type[] { typeof(uint) };

	// Token: 0x040007DF RID: 2015
	private static Type[] swigMethodTypes66 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007E0 RID: 2016
	private static Type[] swigMethodTypes67 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007E1 RID: 2017
	private static Type[] swigMethodTypes68 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007E2 RID: 2018
	private static Type[] swigMethodTypes69 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007E3 RID: 2019
	private static Type[] swigMethodTypes70 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040007E4 RID: 2020
	private static Type[] swigMethodTypes71 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007E5 RID: 2021
	private static Type[] swigMethodTypes72 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007E6 RID: 2022
	private static Type[] swigMethodTypes73 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040007E7 RID: 2023
	private static Type[] swigMethodTypes74 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040007E8 RID: 2024
	private static Type[] swigMethodTypes75 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040007E9 RID: 2025
	private static Type[] swigMethodTypes76 = new Type[]
	{
		typeof(PointerData),
		typeof(uint)
	};

	// Token: 0x040007EA RID: 2026
	private static Type[] swigMethodTypes77 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007EB RID: 2027
	private static Type[] swigMethodTypes78 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007EC RID: 2028
	private static Type[] swigMethodTypes79 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007ED RID: 2029
	private static Type[] swigMethodTypes80 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007EE RID: 2030
	private static Type[] swigMethodTypes81 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007EF RID: 2031
	private static Type[] swigMethodTypes82 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007F0 RID: 2032
	private static Type[] swigMethodTypes83 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007F1 RID: 2033
	private static Type[] swigMethodTypes84 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007F2 RID: 2034
	private static Type[] swigMethodTypes85 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040007F3 RID: 2035
	private static Type[] swigMethodTypes86 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040007F4 RID: 2036
	private static Type[] swigMethodTypes87 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040007F5 RID: 2037
	private static Type[] swigMethodTypes88 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040007F6 RID: 2038
	private static Type[] swigMethodTypes89 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040007F7 RID: 2039
	private static Type[] swigMethodTypes90 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040007F8 RID: 2040
	private static Type[] swigMethodTypes91 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040007F9 RID: 2041
	private static Type[] swigMethodTypes92 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040007FA RID: 2042
	private static Type[] swigMethodTypes93 = new Type[] { typeof(float) };

	// Token: 0x040007FB RID: 2043
	private static Type[] swigMethodTypes94 = new Type[] { typeof(uint) };

	// Token: 0x040007FC RID: 2044
	private static Type[] swigMethodTypes95 = new Type[]
	{
		typeof(uint),
		typeof(int)
	};

	// Token: 0x040007FD RID: 2045
	private static Type[] swigMethodTypes96 = new Type[]
	{
		typeof(float),
		typeof(float)
	};

	// Token: 0x040007FE RID: 2046
	private static Type[] swigMethodTypes97 = new Type[]
	{
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040007FF RID: 2047
	private static Type[] swigMethodTypes98 = new Type[0];

	// Token: 0x04000800 RID: 2048
	private static Type[] swigMethodTypes99 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000801 RID: 2049
	private static Type[] swigMethodTypes100 = new Type[]
	{
		typeof(float),
		typeof(int)
	};

	// Token: 0x04000802 RID: 2050
	private static Type[] swigMethodTypes101 = new Type[]
	{
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000803 RID: 2051
	private static Type[] swigMethodTypes102 = new Type[]
	{
		typeof(int),
		typeof(PointerData),
		typeof(uint),
		typeof(PointerData),
		typeof(int)
	};

	// Token: 0x04000804 RID: 2052
	private static Type[] swigMethodTypes103 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000805 RID: 2053
	private static Type[] swigMethodTypes104 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint)
	};

	// Token: 0x04000806 RID: 2054
	private static Type[] swigMethodTypes105 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(uint)
	};

	// Token: 0x04000807 RID: 2055
	private static Type[] swigMethodTypes106 = new Type[] { typeof(uint) };

	// Token: 0x04000808 RID: 2056
	private static Type[] swigMethodTypes107 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000809 RID: 2057
	private static Type[] swigMethodTypes108 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400080A RID: 2058
	private static Type[] swigMethodTypes109 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400080B RID: 2059
	private static Type[] swigMethodTypes110 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400080C RID: 2060
	private static Type[] swigMethodTypes111 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(float)
	};

	// Token: 0x0400080D RID: 2061
	private static Type[] swigMethodTypes112 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400080E RID: 2062
	private static Type[] swigMethodTypes113 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x0400080F RID: 2063
	private static Type[] swigMethodTypes114 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000810 RID: 2064
	private static Type[] swigMethodTypes115 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000811 RID: 2065
	private static Type[] swigMethodTypes116 = new Type[]
	{
		typeof(uint),
		typeof(float)
	};

	// Token: 0x04000812 RID: 2066
	private static Type[] swigMethodTypes117 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000813 RID: 2067
	private static Type[] swigMethodTypes118 = new Type[]
	{
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000814 RID: 2068
	private static Type[] swigMethodTypes119 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000815 RID: 2069
	private static Type[] swigMethodTypes120 = new Type[]
	{
		typeof(uint),
		typeof(float),
		typeof(float)
	};

	// Token: 0x04000816 RID: 2070
	private static Type[] swigMethodTypes121 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000817 RID: 2071
	private static Type[] swigMethodTypes122 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000818 RID: 2072
	private static Type[] swigMethodTypes123 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000819 RID: 2073
	private static Type[] swigMethodTypes124 = new Type[]
	{
		typeof(uint),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x0400081A RID: 2074
	private static Type[] swigMethodTypes125 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400081B RID: 2075
	private static Type[] swigMethodTypes126 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x0400081C RID: 2076
	private static Type[] swigMethodTypes127 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400081D RID: 2077
	private static Type[] swigMethodTypes128 = new Type[]
	{
		typeof(uint),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x0400081E RID: 2078
	private static Type[] swigMethodTypes129 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400081F RID: 2079
	private static Type[] swigMethodTypes130 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000820 RID: 2080
	private static Type[] swigMethodTypes131 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000821 RID: 2081
	private static Type[] swigMethodTypes132 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000822 RID: 2082
	private static Type[] swigMethodTypes133 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000823 RID: 2083
	private static Type[] swigMethodTypes134 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000824 RID: 2084
	private static Type[] swigMethodTypes135 = new Type[] { typeof(uint) };

	// Token: 0x04000825 RID: 2085
	private static Type[] swigMethodTypes136 = new Type[] { typeof(uint) };

	// Token: 0x04000826 RID: 2086
	private static Type[] swigMethodTypes137 = new Type[]
	{
		typeof(uint),
		typeof(float)
	};

	// Token: 0x04000827 RID: 2087
	private static Type[] swigMethodTypes138 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000828 RID: 2088
	private static Type[] swigMethodTypes139 = new Type[]
	{
		typeof(uint),
		typeof(float),
		typeof(float)
	};

	// Token: 0x04000829 RID: 2089
	private static Type[] swigMethodTypes140 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400082A RID: 2090
	private static Type[] swigMethodTypes141 = new Type[]
	{
		typeof(uint),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x0400082B RID: 2091
	private static Type[] swigMethodTypes142 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400082C RID: 2092
	private static Type[] swigMethodTypes143 = new Type[]
	{
		typeof(uint),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x0400082D RID: 2093
	private static Type[] swigMethodTypes144 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400082E RID: 2094
	private static Type[] swigMethodTypes145 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400082F RID: 2095
	private static Type[] swigMethodTypes146 = new Type[]
	{
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000830 RID: 2096
	private static Type[] swigMethodTypes147 = new Type[] { typeof(uint) };

	// Token: 0x04000831 RID: 2097
	private static Type[] swigMethodTypes148 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000832 RID: 2098
	private static Type[] swigMethodTypes149 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000833 RID: 2099
	private static Type[] swigMethodTypes150 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000834 RID: 2100
	private static Type[] swigMethodTypes151 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000835 RID: 2101
	private static Type[] swigMethodTypes152 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000836 RID: 2102
	private static Type[] swigMethodTypes153 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000837 RID: 2103
	private static Type[] swigMethodTypes154 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000838 RID: 2104
	private static Type[] swigMethodTypes155 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000839 RID: 2105
	private static Type[] swigMethodTypes156 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x0400083A RID: 2106
	private static Type[] swigMethodTypes157 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400083B RID: 2107
	private static Type[] swigMethodTypes158 = new Type[] { typeof(uint) };

	// Token: 0x0400083C RID: 2108
	private static Type[] swigMethodTypes159 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400083D RID: 2109
	private static Type[] swigMethodTypes160 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400083E RID: 2110
	private static Type[] swigMethodTypes161 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x0400083F RID: 2111
	private static Type[] swigMethodTypes162 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000840 RID: 2112
	private static Type[] swigMethodTypes163 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000841 RID: 2113
	private static Type[] swigMethodTypes164 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000842 RID: 2114
	private static Type[] swigMethodTypes165 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000843 RID: 2115
	private static Type[] swigMethodTypes166 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000844 RID: 2116
	private static Type[] swigMethodTypes167 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000845 RID: 2117
	private static Type[] swigMethodTypes168 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000846 RID: 2118
	private static Type[] swigMethodTypes169 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000847 RID: 2119
	private static Type[] swigMethodTypes170 = new Type[]
	{
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000848 RID: 2120
	private static Type[] swigMethodTypes171 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000849 RID: 2121
	private static Type[] swigMethodTypes172 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x0400084A RID: 2122
	private static Type[] swigMethodTypes173 = new Type[]
	{
		typeof(PointerData),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(uint)
	};

	// Token: 0x0400084B RID: 2123
	private static Type[] swigMethodTypes174 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x0400084C RID: 2124
	private static Type[] swigMethodTypes175 = new Type[] { typeof(uint) };

	// Token: 0x0400084D RID: 2125
	private static Type[] swigMethodTypes176 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400084E RID: 2126
	private static Type[] swigMethodTypes177 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400084F RID: 2127
	private static Type[] swigMethodTypes178 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x04000850 RID: 2128
	private static Type[] swigMethodTypes179 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000851 RID: 2129
	private static Type[] swigMethodTypes180 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000852 RID: 2130
	private static Type[] swigMethodTypes181 = new Type[] { typeof(uint) };

	// Token: 0x04000853 RID: 2131
	private static Type[] swigMethodTypes182 = new Type[0];

	// Token: 0x04000854 RID: 2132
	private static Type[] swigMethodTypes183 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000855 RID: 2133
	private static Type[] swigMethodTypes184 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000856 RID: 2134
	private static Type[] swigMethodTypes185 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(uint)
	};

	// Token: 0x04000857 RID: 2135
	private static Type[] swigMethodTypes186 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000858 RID: 2136
	private static Type[] swigMethodTypes187 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000859 RID: 2137
	private static Type[] swigMethodTypes188 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400085A RID: 2138
	private static Type[] swigMethodTypes189 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400085B RID: 2139
	private static Type[] swigMethodTypes190 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x0400085C RID: 2140
	private static Type[] swigMethodTypes191 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400085D RID: 2141
	private static Type[] swigMethodTypes192 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400085E RID: 2142
	private static Type[] swigMethodTypes193 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400085F RID: 2143
	private static Type[] swigMethodTypes194 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000860 RID: 2144
	private static Type[] swigMethodTypes195 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000861 RID: 2145
	private static Type[] swigMethodTypes196 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000862 RID: 2146
	private static Type[] swigMethodTypes197 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000863 RID: 2147
	private static Type[] swigMethodTypes198 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000864 RID: 2148
	private static Type[] swigMethodTypes199 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000865 RID: 2149
	private static Type[] swigMethodTypes200 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000866 RID: 2150
	private static Type[] swigMethodTypes201 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000867 RID: 2151
	private static Type[] swigMethodTypes202 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000868 RID: 2152
	private static Type[] swigMethodTypes203 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000869 RID: 2153
	private static Type[] swigMethodTypes204 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400086A RID: 2154
	private static Type[] swigMethodTypes205 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400086B RID: 2155
	private static Type[] swigMethodTypes206 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400086C RID: 2156
	private static Type[] swigMethodTypes207 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(float),
		typeof(int)
	};

	// Token: 0x0400086D RID: 2157
	private static Type[] swigMethodTypes208 = new Type[]
	{
		typeof(PointerData),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400086E RID: 2158
	private static Type[] swigMethodTypes209 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x0400086F RID: 2159
	private static Type[] swigMethodTypes210 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000870 RID: 2160
	private static Type[] swigMethodTypes211 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000871 RID: 2161
	private static Type[] swigMethodTypes212 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000872 RID: 2162
	private static Type[] swigMethodTypes213 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000873 RID: 2163
	private static Type[] swigMethodTypes214 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000874 RID: 2164
	private static Type[] swigMethodTypes215 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000875 RID: 2165
	private static Type[] swigMethodTypes216 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000876 RID: 2166
	private static Type[] swigMethodTypes217 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(PointerData),
		typeof(int)
	};

	// Token: 0x04000877 RID: 2167
	private static Type[] swigMethodTypes218 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000878 RID: 2168
	private static Type[] swigMethodTypes219 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x04000879 RID: 2169
	private static Type[] swigMethodTypes220 = new Type[] { typeof(uint) };

	// Token: 0x0400087A RID: 2170
	private static Type[] swigMethodTypes221 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(ulong)
	};

	// Token: 0x0400087B RID: 2171
	private static Type[] swigMethodTypes222 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(ulong)
	};

	// Token: 0x0400087C RID: 2172
	private static Type[] swigMethodTypes223 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400087D RID: 2173
	private static Type[] swigMethodTypes224 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x0400087E RID: 2174
	private static Type[] swigMethodTypes225 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400087F RID: 2175
	private static Type[] swigMethodTypes226 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000880 RID: 2176
	private static Type[] swigMethodTypes227 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000881 RID: 2177
	private static Type[] swigMethodTypes228 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000882 RID: 2178
	private static Type[] swigMethodTypes229 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x04000883 RID: 2179
	private static Type[] swigMethodTypes230 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000884 RID: 2180
	private static Type[] swigMethodTypes231 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000885 RID: 2181
	private static Type[] swigMethodTypes232 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000886 RID: 2182
	private static Type[] swigMethodTypes233 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(float)
	};

	// Token: 0x04000887 RID: 2183
	private static Type[] swigMethodTypes234 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000888 RID: 2184
	private static Type[] swigMethodTypes235 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000889 RID: 2185
	private static Type[] swigMethodTypes236 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400088A RID: 2186
	private static Type[] swigMethodTypes237 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400088B RID: 2187
	private static Type[] swigMethodTypes238 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400088C RID: 2188
	private static Type[] swigMethodTypes239 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400088D RID: 2189
	private static Type[] swigMethodTypes240 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400088E RID: 2190
	private static Type[] swigMethodTypes241 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x0400088F RID: 2191
	private static Type[] swigMethodTypes242 = new Type[0];

	// Token: 0x04000890 RID: 2192
	private static Type[] swigMethodTypes243 = new Type[0];

	// Token: 0x04000891 RID: 2193
	private static Type[] swigMethodTypes244 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000892 RID: 2194
	private static Type[] swigMethodTypes245 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData),
		typeof(int)
	};

	// Token: 0x04000893 RID: 2195
	private static Type[] swigMethodTypes246 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000894 RID: 2196
	private static Type[] swigMethodTypes247 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000895 RID: 2197
	private static Type[] swigMethodTypes248 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000896 RID: 2198
	private static Type[] swigMethodTypes249 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000897 RID: 2199
	private static Type[] swigMethodTypes250 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000898 RID: 2200
	private static Type[] swigMethodTypes251 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000899 RID: 2201
	private static Type[] swigMethodTypes252 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400089A RID: 2202
	private static Type[] swigMethodTypes253 = new Type[] { typeof(int) };

	// Token: 0x0400089B RID: 2203
	private static Type[] swigMethodTypes254 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400089C RID: 2204
	private static Type[] swigMethodTypes255 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400089D RID: 2205
	private static Type[] swigMethodTypes256 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x0400089E RID: 2206
	private static Type[] swigMethodTypes257 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400089F RID: 2207
	private static Type[] swigMethodTypes258 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008A0 RID: 2208
	private static Type[] swigMethodTypes259 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008A1 RID: 2209
	private static Type[] swigMethodTypes260 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040008A2 RID: 2210
	private static Type[] swigMethodTypes261 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040008A3 RID: 2211
	private static Type[] swigMethodTypes262 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008A4 RID: 2212
	private static Type[] swigMethodTypes263 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008A5 RID: 2213
	private static Type[] swigMethodTypes264 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008A6 RID: 2214
	private static Type[] swigMethodTypes265 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008A7 RID: 2215
	private static Type[] swigMethodTypes266 = new Type[] { typeof(uint) };

	// Token: 0x040008A8 RID: 2216
	private static Type[] swigMethodTypes267 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008A9 RID: 2217
	private static Type[] swigMethodTypes268 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008AA RID: 2218
	private static Type[] swigMethodTypes269 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040008AB RID: 2219
	private static Type[] swigMethodTypes270 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008AC RID: 2220
	private static Type[] swigMethodTypes271 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x040008AD RID: 2221
	private static Type[] swigMethodTypes272 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040008AE RID: 2222
	private static Type[] swigMethodTypes273 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040008AF RID: 2223
	private static Type[] swigMethodTypes274 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040008B0 RID: 2224
	private static Type[] swigMethodTypes275 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008B1 RID: 2225
	private static Type[] swigMethodTypes276 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008B2 RID: 2226
	private static Type[] swigMethodTypes277 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008B3 RID: 2227
	private static Type[] swigMethodTypes278 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008B4 RID: 2228
	private static Type[] swigMethodTypes279 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(float)
	};

	// Token: 0x040008B5 RID: 2229
	private static Type[] swigMethodTypes280 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(float),
		typeof(float)
	};

	// Token: 0x040008B6 RID: 2230
	private static Type[] swigMethodTypes281 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x040008B7 RID: 2231
	private static Type[] swigMethodTypes282 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x040008B8 RID: 2232
	private static Type[] swigMethodTypes283 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008B9 RID: 2233
	private static Type[] swigMethodTypes284 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008BA RID: 2234
	private static Type[] swigMethodTypes285 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008BB RID: 2235
	private static Type[] swigMethodTypes286 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008BC RID: 2236
	private static Type[] swigMethodTypes287 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008BD RID: 2237
	private static Type[] swigMethodTypes288 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008BE RID: 2238
	private static Type[] swigMethodTypes289 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008BF RID: 2239
	private static Type[] swigMethodTypes290 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C0 RID: 2240
	private static Type[] swigMethodTypes291 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C1 RID: 2241
	private static Type[] swigMethodTypes292 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C2 RID: 2242
	private static Type[] swigMethodTypes293 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C3 RID: 2243
	private static Type[] swigMethodTypes294 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C4 RID: 2244
	private static Type[] swigMethodTypes295 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C5 RID: 2245
	private static Type[] swigMethodTypes296 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C6 RID: 2246
	private static Type[] swigMethodTypes297 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C7 RID: 2247
	private static Type[] swigMethodTypes298 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C8 RID: 2248
	private static Type[] swigMethodTypes299 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008C9 RID: 2249
	private static Type[] swigMethodTypes300 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008CA RID: 2250
	private static Type[] swigMethodTypes301 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008CB RID: 2251
	private static Type[] swigMethodTypes302 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008CC RID: 2252
	private static Type[] swigMethodTypes303 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008CD RID: 2253
	private static Type[] swigMethodTypes304 = new Type[] { typeof(uint) };

	// Token: 0x040008CE RID: 2254
	private static Type[] swigMethodTypes305 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040008CF RID: 2255
	private static Type[] swigMethodTypes306 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008D0 RID: 2256
	private static Type[] swigMethodTypes307 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008D1 RID: 2257
	private static Type[] swigMethodTypes308 = new Type[] { typeof(uint) };

	// Token: 0x040008D2 RID: 2258
	private static Type[] swigMethodTypes309 = new Type[] { typeof(uint) };

	// Token: 0x040008D3 RID: 2259
	private static Type[] swigMethodTypes310 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040008D4 RID: 2260
	private static Type[] swigMethodTypes311 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040008D5 RID: 2261
	private static Type[] swigMethodTypes312 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008D6 RID: 2262
	private static Type[] swigMethodTypes313 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008D7 RID: 2263
	private static Type[] swigMethodTypes314 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008D8 RID: 2264
	private static Type[] swigMethodTypes315 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008D9 RID: 2265
	private static Type[] swigMethodTypes316 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040008DA RID: 2266
	private static Type[] swigMethodTypes317 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040008DB RID: 2267
	private static Type[] swigMethodTypes318 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008DC RID: 2268
	private static Type[] swigMethodTypes319 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008DD RID: 2269
	private static Type[] swigMethodTypes320 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008DE RID: 2270
	private static Type[] swigMethodTypes321 = new Type[]
	{
		typeof(uint),
		typeof(int)
	};

	// Token: 0x040008DF RID: 2271
	private static Type[] swigMethodTypes322 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008E0 RID: 2272
	private static Type[] swigMethodTypes323 = new Type[] { typeof(uint) };

	// Token: 0x040008E1 RID: 2273
	private static Type[] swigMethodTypes324 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008E2 RID: 2274
	private static Type[] swigMethodTypes325 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008E3 RID: 2275
	private static Type[] swigMethodTypes326 = new Type[]
	{
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040008E4 RID: 2276
	private static Type[] swigMethodTypes327 = new Type[]
	{
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040008E5 RID: 2277
	private static Type[] swigMethodTypes328 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040008E6 RID: 2278
	private static Type[] swigMethodTypes329 = new Type[]
	{
		typeof(PointerData),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008E7 RID: 2279
	private static Type[] swigMethodTypes330 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008E8 RID: 2280
	private static Type[] swigMethodTypes331 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x040008E9 RID: 2281
	private static Type[] swigMethodTypes332 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008EA RID: 2282
	private static Type[] swigMethodTypes333 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008EB RID: 2283
	private static Type[] swigMethodTypes334 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x040008EC RID: 2284
	private static Type[] swigMethodTypes335 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040008ED RID: 2285
	private static Type[] swigMethodTypes336 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData),
		typeof(int)
	};

	// Token: 0x040008EE RID: 2286
	private static Type[] swigMethodTypes337 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008EF RID: 2287
	private static Type[] swigMethodTypes338 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008F0 RID: 2288
	private static Type[] swigMethodTypes339 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040008F1 RID: 2289
	private static Type[] swigMethodTypes340 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008F2 RID: 2290
	private static Type[] swigMethodTypes341 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008F3 RID: 2291
	private static Type[] swigMethodTypes342 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x040008F4 RID: 2292
	private static Type[] swigMethodTypes343 = new Type[] { typeof(uint) };

	// Token: 0x040008F5 RID: 2293
	private static Type[] swigMethodTypes344 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008F6 RID: 2294
	private static Type[] swigMethodTypes345 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008F7 RID: 2295
	private static Type[] swigMethodTypes346 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x040008F8 RID: 2296
	private static Type[] swigMethodTypes347 = new Type[]
	{
		typeof(PointerData),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008F9 RID: 2297
	private static Type[] swigMethodTypes348 = new Type[]
	{
		typeof(uint),
		typeof(PointerData),
		typeof(PointerData),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008FA RID: 2298
	private static Type[] swigMethodTypes349 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040008FB RID: 2299
	private static Type[] swigMethodTypes350 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x040008FC RID: 2300
	private static Type[] swigMethodTypes351 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x040008FD RID: 2301
	private static Type[] swigMethodTypes352 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008FE RID: 2302
	private static Type[] swigMethodTypes353 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x040008FF RID: 2303
	private static Type[] swigMethodTypes354 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000900 RID: 2304
	private static Type[] swigMethodTypes355 = new Type[] { typeof(uint) };

	// Token: 0x04000901 RID: 2305
	private static Type[] swigMethodTypes356 = new Type[] { typeof(uint) };

	// Token: 0x04000902 RID: 2306
	private static Type[] swigMethodTypes357 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000903 RID: 2307
	private static Type[] swigMethodTypes358 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000904 RID: 2308
	private static Type[] swigMethodTypes359 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000905 RID: 2309
	private static Type[] swigMethodTypes360 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000906 RID: 2310
	private static Type[] swigMethodTypes361 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000907 RID: 2311
	private static Type[] swigMethodTypes362 = new Type[0];

	// Token: 0x04000908 RID: 2312
	private static Type[] swigMethodTypes363 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000909 RID: 2313
	private static Type[] swigMethodTypes364 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400090A RID: 2314
	private static Type[] swigMethodTypes365 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400090B RID: 2315
	private static Type[] swigMethodTypes366 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x0400090C RID: 2316
	private static Type[] swigMethodTypes367 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400090D RID: 2317
	private static Type[] swigMethodTypes368 = new Type[] { typeof(uint) };

	// Token: 0x0400090E RID: 2318
	private static Type[] swigMethodTypes369 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400090F RID: 2319
	private static Type[] swigMethodTypes370 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000910 RID: 2320
	private static Type[] swigMethodTypes371 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000911 RID: 2321
	private static Type[] swigMethodTypes372 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000912 RID: 2322
	private static Type[] swigMethodTypes373 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000913 RID: 2323
	private static Type[] swigMethodTypes374 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000914 RID: 2324
	private static Type[] swigMethodTypes375 = new Type[] { typeof(uint) };

	// Token: 0x04000915 RID: 2325
	private static Type[] swigMethodTypes376 = new Type[]
	{
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000916 RID: 2326
	private static Type[] swigMethodTypes377 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000917 RID: 2327
	private static Type[] swigMethodTypes378 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000918 RID: 2328
	private static Type[] swigMethodTypes379 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000919 RID: 2329
	private static Type[] swigMethodTypes380 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400091A RID: 2330
	private static Type[] swigMethodTypes381 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400091B RID: 2331
	private static Type[] swigMethodTypes382 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400091C RID: 2332
	private static Type[] swigMethodTypes383 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400091D RID: 2333
	private static Type[] swigMethodTypes384 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400091E RID: 2334
	private static Type[] swigMethodTypes385 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400091F RID: 2335
	private static Type[] swigMethodTypes386 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000920 RID: 2336
	private static Type[] swigMethodTypes387 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000921 RID: 2337
	private static Type[] swigMethodTypes388 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000922 RID: 2338
	private static Type[] swigMethodTypes389 = new Type[]
	{
		typeof(uint),
		typeof(float)
	};

	// Token: 0x04000923 RID: 2339
	private static Type[] swigMethodTypes390 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000924 RID: 2340
	private static Type[] swigMethodTypes391 = new Type[] { typeof(uint) };

	// Token: 0x04000925 RID: 2341
	private static Type[] swigMethodTypes392 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000926 RID: 2342
	private static Type[] swigMethodTypes393 = new Type[0];

	// Token: 0x04000927 RID: 2343
	private static Type[] swigMethodTypes394 = new Type[] { typeof(float) };

	// Token: 0x04000928 RID: 2344
	private static Type[] swigMethodTypes395 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000929 RID: 2345
	private static Type[] swigMethodTypes396 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400092A RID: 2346
	private static Type[] swigMethodTypes397 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400092B RID: 2347
	private static Type[] swigMethodTypes398 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400092C RID: 2348
	private static Type[] swigMethodTypes399 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400092D RID: 2349
	private static Type[] swigMethodTypes400 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400092E RID: 2350
	private static Type[] swigMethodTypes401 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x0400092F RID: 2351
	private static Type[] swigMethodTypes402 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000930 RID: 2352
	private static Type[] swigMethodTypes403 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000931 RID: 2353
	private static Type[] swigMethodTypes404 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000932 RID: 2354
	private static Type[] swigMethodTypes405 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(int)
	};

	// Token: 0x04000933 RID: 2355
	private static Type[] swigMethodTypes406 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000934 RID: 2356
	private static Type[] swigMethodTypes407 = new Type[]
	{
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000935 RID: 2357
	private static Type[] swigMethodTypes408 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000936 RID: 2358
	private static Type[] swigMethodTypes409 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000937 RID: 2359
	private static Type[] swigMethodTypes410 = new Type[0];

	// Token: 0x04000938 RID: 2360
	private static Type[] swigMethodTypes411 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000939 RID: 2361
	private static Type[] swigMethodTypes412 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x0400093A RID: 2362
	private static Type[] swigMethodTypes413 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400093B RID: 2363
	private static Type[] swigMethodTypes414 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x0400093C RID: 2364
	private static Type[] swigMethodTypes415 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400093D RID: 2365
	private static Type[] swigMethodTypes416 = new Type[]
	{
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x0400093E RID: 2366
	private static Type[] swigMethodTypes417 = new Type[]
	{
		typeof(uint),
		typeof(int)
	};

	// Token: 0x0400093F RID: 2367
	private static Type[] swigMethodTypes418 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(PointerData),
		typeof(int)
	};

	// Token: 0x04000940 RID: 2368
	private static Type[] swigMethodTypes419 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(PointerData),
		typeof(int)
	};

	// Token: 0x04000941 RID: 2369
	private static Type[] swigMethodTypes420 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(PointerData),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000942 RID: 2370
	private static Type[] swigMethodTypes421 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000943 RID: 2371
	private static Type[] swigMethodTypes422 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000944 RID: 2372
	private static Type[] swigMethodTypes423 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000945 RID: 2373
	private static Type[] swigMethodTypes424 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(uint)
	};

	// Token: 0x04000946 RID: 2374
	private static Type[] swigMethodTypes425 = new Type[] { typeof(uint) };

	// Token: 0x04000947 RID: 2375
	private static Type[] swigMethodTypes426 = new Type[]
	{
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000948 RID: 2376
	private static Type[] swigMethodTypes427 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000949 RID: 2377
	private static Type[] swigMethodTypes428 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400094A RID: 2378
	private static Type[] swigMethodTypes429 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400094B RID: 2379
	private static Type[] swigMethodTypes430 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400094C RID: 2380
	private static Type[] swigMethodTypes431 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400094D RID: 2381
	private static Type[] swigMethodTypes432 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400094E RID: 2382
	private static Type[] swigMethodTypes433 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400094F RID: 2383
	private static Type[] swigMethodTypes434 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000950 RID: 2384
	private static Type[] swigMethodTypes435 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000951 RID: 2385
	private static Type[] swigMethodTypes436 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000952 RID: 2386
	private static Type[] swigMethodTypes437 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000953 RID: 2387
	private static Type[] swigMethodTypes438 = new Type[]
	{
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000954 RID: 2388
	private static Type[] swigMethodTypes439 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000955 RID: 2389
	private static Type[] swigMethodTypes440 = new Type[0];

	// Token: 0x04000956 RID: 2390
	private static Type[] swigMethodTypes441 = new Type[] { typeof(float) };

	// Token: 0x04000957 RID: 2391
	private static Type[] swigMethodTypes442 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000958 RID: 2392
	private static Type[] swigMethodTypes443 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000959 RID: 2393
	private static Type[] swigMethodTypes444 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400095A RID: 2394
	private static Type[] swigMethodTypes445 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400095B RID: 2395
	private static Type[] swigMethodTypes446 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400095C RID: 2396
	private static Type[] swigMethodTypes447 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400095D RID: 2397
	private static Type[] swigMethodTypes448 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x0400095E RID: 2398
	private static Type[] swigMethodTypes449 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400095F RID: 2399
	private static Type[] swigMethodTypes450 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000960 RID: 2400
	private static Type[] swigMethodTypes451 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000961 RID: 2401
	private static Type[] swigMethodTypes452 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(int)
	};

	// Token: 0x04000962 RID: 2402
	private static Type[] swigMethodTypes453 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000963 RID: 2403
	private static Type[] swigMethodTypes454 = new Type[]
	{
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000964 RID: 2404
	private static Type[] swigMethodTypes455 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000965 RID: 2405
	private static Type[] swigMethodTypes456 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000966 RID: 2406
	private static Type[] swigMethodTypes457 = new Type[0];

	// Token: 0x04000967 RID: 2407
	private static Type[] swigMethodTypes458 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000968 RID: 2408
	private static Type[] swigMethodTypes459 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000969 RID: 2409
	private static Type[] swigMethodTypes460 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400096A RID: 2410
	private static Type[] swigMethodTypes461 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x0400096B RID: 2411
	private static Type[] swigMethodTypes462 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400096C RID: 2412
	private static Type[] swigMethodTypes463 = new Type[]
	{
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x0400096D RID: 2413
	private static Type[] swigMethodTypes464 = new Type[]
	{
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x0400096E RID: 2414
	private static Type[] swigMethodTypes465 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400096F RID: 2415
	private static Type[] swigMethodTypes466 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000970 RID: 2416
	private static Type[] swigMethodTypes467 = new Type[] { typeof(uint) };

	// Token: 0x04000971 RID: 2417
	private static Type[] swigMethodTypes468 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int)
	};

	// Token: 0x04000972 RID: 2418
	private static Type[] swigMethodTypes469 = new Type[]
	{
		typeof(int),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000973 RID: 2419
	private static Type[] swigMethodTypes470 = new Type[] { typeof(int) };

	// Token: 0x04000974 RID: 2420
	private static Type[] swigMethodTypes471 = new Type[0];

	// Token: 0x04000975 RID: 2421
	private static Type[] swigMethodTypes472 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000976 RID: 2422
	private static Type[] swigMethodTypes473 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x04000977 RID: 2423
	private static Type[] swigMethodTypes474 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000978 RID: 2424
	private static Type[] swigMethodTypes475 = new Type[0];

	// Token: 0x04000979 RID: 2425
	private static Type[] swigMethodTypes476 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400097A RID: 2426
	private static Type[] swigMethodTypes477 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x0400097B RID: 2427
	private static Type[] swigMethodTypes478 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x0400097C RID: 2428
	private static Type[] swigMethodTypes479 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400097D RID: 2429
	private static Type[] swigMethodTypes480 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400097E RID: 2430
	private static Type[] swigMethodTypes481 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(ulong)
	};

	// Token: 0x0400097F RID: 2431
	private static Type[] swigMethodTypes482 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(ulong)
	};

	// Token: 0x04000980 RID: 2432
	private static Type[] swigMethodTypes483 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(ulong)
	};

	// Token: 0x04000981 RID: 2433
	private static Type[] swigMethodTypes484 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(uint),
		typeof(ulong)
	};

	// Token: 0x04000982 RID: 2434
	private static Type[] swigMethodTypes485 = new Type[]
	{
		typeof(uint),
		typeof(int),
		typeof(uint),
		typeof(ulong)
	};

	// Token: 0x04000983 RID: 2435
	private static Type[] swigMethodTypes486 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000984 RID: 2436
	private static Type[] swigMethodTypes487 = new Type[]
	{
		typeof(int),
		typeof(PointerData)
	};

	// Token: 0x04000985 RID: 2437
	private static Type[] swigMethodTypes488 = new Type[]
	{
		typeof(int),
		typeof(uint)
	};

	// Token: 0x04000986 RID: 2438
	private static Type[] swigMethodTypes489 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000987 RID: 2439
	private static Type[] swigMethodTypes490 = new Type[]
	{
		typeof(PointerData),
		typeof(uint)
	};

	// Token: 0x04000988 RID: 2440
	private static Type[] swigMethodTypes491 = new Type[]
	{
		typeof(uint),
		typeof(ulong),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x04000989 RID: 2441
	private static Type[] swigMethodTypes492 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(int)
	};

	// Token: 0x0400098A RID: 2442
	private static Type[] swigMethodTypes493 = new Type[]
	{
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400098B RID: 2443
	private static Type[] swigMethodTypes494 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400098C RID: 2444
	private static Type[] swigMethodTypes495 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x0400098D RID: 2445
	private static Type[] swigMethodTypes496 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400098E RID: 2446
	private static Type[] swigMethodTypes497 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x0400098F RID: 2447
	private static Type[] swigMethodTypes498 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000990 RID: 2448
	private static Type[] swigMethodTypes499 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000991 RID: 2449
	private static Type[] swigMethodTypes500 = new Type[] { typeof(uint) };

	// Token: 0x04000992 RID: 2450
	private static Type[] swigMethodTypes501 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(float)
	};

	// Token: 0x04000993 RID: 2451
	private static Type[] swigMethodTypes502 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000994 RID: 2452
	private static Type[] swigMethodTypes503 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000995 RID: 2453
	private static Type[] swigMethodTypes504 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x04000996 RID: 2454
	private static Type[] swigMethodTypes505 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(PointerData)
	};

	// Token: 0x04000997 RID: 2455
	private static Type[] swigMethodTypes506 = new Type[]
	{
		typeof(float),
		typeof(float),
		typeof(float)
	};

	// Token: 0x04000998 RID: 2456
	private static Type[] swigMethodTypes507 = new Type[]
	{
		typeof(int),
		typeof(int),
		typeof(PointerData),
		typeof(PointerData)
	};

	// Token: 0x04000999 RID: 2457
	private static Type[] swigMethodTypes508 = new Type[] { typeof(uint) };

	// Token: 0x0400099A RID: 2458
	private static Type[] swigMethodTypes509 = new Type[]
	{
		typeof(uint),
		typeof(uint)
	};

	// Token: 0x0400099B RID: 2459
	private static Type[] swigMethodTypes510 = new Type[]
	{
		typeof(uint),
		typeof(uint),
		typeof(uint),
		typeof(int),
		typeof(int),
		typeof(int),
		typeof(int)
	};

	// Token: 0x0200007F RID: 127
	// (Invoke) Token: 0x06000AB8 RID: 2744
	public delegate void SwigDelegateGLAdapter_0(uint id);

	// Token: 0x02000080 RID: 128
	// (Invoke) Token: 0x06000ABC RID: 2748
	public delegate void SwigDelegateGLAdapter_1(uint index, int size, uint type, uint normalized, int stride, IntPtr pData, uint dataSize, uint dataOffset);

	// Token: 0x02000081 RID: 129
	// (Invoke) Token: 0x06000AC0 RID: 2752
	public delegate void SwigDelegateGLAdapter_2(uint index, int size, uint type, int stride, IntPtr pData, uint dataSize, uint dataOffset);

	// Token: 0x02000082 RID: 130
	// (Invoke) Token: 0x06000AC4 RID: 2756
	public delegate void SwigDelegateGLAdapter_3(uint target, uint offset, uint length, IntPtr pData);

	// Token: 0x02000083 RID: 131
	// (Invoke) Token: 0x06000AC8 RID: 2760
	public delegate void SwigDelegateGLAdapter_4(uint target, uint length, IntPtr pData);

	// Token: 0x02000084 RID: 132
	// (Invoke) Token: 0x06000ACC RID: 2764
	public delegate void SwigDelegateGLAdapter_5(uint texture);

	// Token: 0x02000085 RID: 133
	// (Invoke) Token: 0x06000AD0 RID: 2768
	public delegate void SwigDelegateGLAdapter_6(uint program, uint shader);

	// Token: 0x02000086 RID: 134
	// (Invoke) Token: 0x06000AD4 RID: 2772
	public delegate void SwigDelegateGLAdapter_7(uint program, uint index, IntPtr pNamePtrData);

	// Token: 0x02000087 RID: 135
	// (Invoke) Token: 0x06000AD8 RID: 2776
	public delegate void SwigDelegateGLAdapter_8(uint target, uint buffer);

	// Token: 0x02000088 RID: 136
	// (Invoke) Token: 0x06000ADC RID: 2780
	public delegate void SwigDelegateGLAdapter_9(uint target, uint framebuffer);

	// Token: 0x02000089 RID: 137
	// (Invoke) Token: 0x06000AE0 RID: 2784
	public delegate void SwigDelegateGLAdapter_10(uint target, uint renderbuffer);

	// Token: 0x0200008A RID: 138
	// (Invoke) Token: 0x06000AE4 RID: 2788
	public delegate void SwigDelegateGLAdapter_11(uint target, uint texture);

	// Token: 0x0200008B RID: 139
	// (Invoke) Token: 0x06000AE8 RID: 2792
	public delegate void SwigDelegateGLAdapter_12(float red, float green, float blue, float alpha);

	// Token: 0x0200008C RID: 140
	// (Invoke) Token: 0x06000AEC RID: 2796
	public delegate void SwigDelegateGLAdapter_13(uint mode);

	// Token: 0x0200008D RID: 141
	// (Invoke) Token: 0x06000AF0 RID: 2800
	public delegate void SwigDelegateGLAdapter_14(uint modeRGB, uint modeAlpha);

	// Token: 0x0200008E RID: 142
	// (Invoke) Token: 0x06000AF4 RID: 2804
	public delegate void SwigDelegateGLAdapter_15(uint sfactor, uint dfactor);

	// Token: 0x0200008F RID: 143
	// (Invoke) Token: 0x06000AF8 RID: 2808
	public delegate void SwigDelegateGLAdapter_16(uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);

	// Token: 0x02000090 RID: 144
	// (Invoke) Token: 0x06000AFC RID: 2812
	public delegate void SwigDelegateGLAdapter_17(uint target, int size, IntPtr pDataPtrData, uint usage);

	// Token: 0x02000091 RID: 145
	// (Invoke) Token: 0x06000B00 RID: 2816
	public delegate void SwigDelegateGLAdapter_18(uint target, int offset, int size, IntPtr pDataPtrData);

	// Token: 0x02000092 RID: 146
	// (Invoke) Token: 0x06000B04 RID: 2820
	public delegate void SwigDelegateGLAdapter_19(uint returnVal, uint target);

	// Token: 0x02000093 RID: 147
	// (Invoke) Token: 0x06000B08 RID: 2824
	public delegate void SwigDelegateGLAdapter_20(uint mask);

	// Token: 0x02000094 RID: 148
	// (Invoke) Token: 0x06000B0C RID: 2828
	public delegate void SwigDelegateGLAdapter_21(float red, float green, float blue, float alpha);

	// Token: 0x02000095 RID: 149
	// (Invoke) Token: 0x06000B10 RID: 2832
	public delegate void SwigDelegateGLAdapter_22(float depth);

	// Token: 0x02000096 RID: 150
	// (Invoke) Token: 0x06000B14 RID: 2836
	public delegate void SwigDelegateGLAdapter_23(int s);

	// Token: 0x02000097 RID: 151
	// (Invoke) Token: 0x06000B18 RID: 2840
	public delegate void SwigDelegateGLAdapter_24(int red, int green, int blue, int alpha);

	// Token: 0x02000098 RID: 152
	// (Invoke) Token: 0x06000B1C RID: 2844
	public delegate void SwigDelegateGLAdapter_25(uint shader);

	// Token: 0x02000099 RID: 153
	// (Invoke) Token: 0x06000B20 RID: 2848
	public delegate void SwigDelegateGLAdapter_26(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr pDataPtrData);

	// Token: 0x0200009A RID: 154
	// (Invoke) Token: 0x06000B24 RID: 2852
	public delegate void SwigDelegateGLAdapter_27(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr pDataPtrData);

	// Token: 0x0200009B RID: 155
	// (Invoke) Token: 0x06000B28 RID: 2856
	public delegate void SwigDelegateGLAdapter_28(uint target, int level, uint internalformat, int x, int y, int width, int height, int border);

	// Token: 0x0200009C RID: 156
	// (Invoke) Token: 0x06000B2C RID: 2860
	public delegate void SwigDelegateGLAdapter_29(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height);

	// Token: 0x0200009D RID: 157
	// (Invoke) Token: 0x06000B30 RID: 2864
	public delegate void SwigDelegateGLAdapter_30(uint returnVal);

	// Token: 0x0200009E RID: 158
	// (Invoke) Token: 0x06000B34 RID: 2868
	public delegate void SwigDelegateGLAdapter_31(uint returnVal, uint type);

	// Token: 0x0200009F RID: 159
	// (Invoke) Token: 0x06000B38 RID: 2872
	public delegate void SwigDelegateGLAdapter_32(uint mode);

	// Token: 0x020000A0 RID: 160
	// (Invoke) Token: 0x06000B3C RID: 2876
	public delegate void SwigDelegateGLAdapter_33(int n, IntPtr pBuffersPtrData);

	// Token: 0x020000A1 RID: 161
	// (Invoke) Token: 0x06000B40 RID: 2880
	public delegate void SwigDelegateGLAdapter_34(int n, IntPtr pFramebuffersPtrData);

	// Token: 0x020000A2 RID: 162
	// (Invoke) Token: 0x06000B44 RID: 2884
	public delegate void SwigDelegateGLAdapter_35(uint program);

	// Token: 0x020000A3 RID: 163
	// (Invoke) Token: 0x06000B48 RID: 2888
	public delegate void SwigDelegateGLAdapter_36(int n, IntPtr pRenderbuffersPtrData);

	// Token: 0x020000A4 RID: 164
	// (Invoke) Token: 0x06000B4C RID: 2892
	public delegate void SwigDelegateGLAdapter_37(uint shader);

	// Token: 0x020000A5 RID: 165
	// (Invoke) Token: 0x06000B50 RID: 2896
	public delegate void SwigDelegateGLAdapter_38(int n, IntPtr pTexturesPtrData);

	// Token: 0x020000A6 RID: 166
	// (Invoke) Token: 0x06000B54 RID: 2900
	public delegate void SwigDelegateGLAdapter_39(uint func);

	// Token: 0x020000A7 RID: 167
	// (Invoke) Token: 0x06000B58 RID: 2904
	public delegate void SwigDelegateGLAdapter_40(int flag);

	// Token: 0x020000A8 RID: 168
	// (Invoke) Token: 0x06000B5C RID: 2908
	public delegate void SwigDelegateGLAdapter_41(float n, float f);

	// Token: 0x020000A9 RID: 169
	// (Invoke) Token: 0x06000B60 RID: 2912
	public delegate void SwigDelegateGLAdapter_42(uint program, uint shader);

	// Token: 0x020000AA RID: 170
	// (Invoke) Token: 0x06000B64 RID: 2916
	public delegate void SwigDelegateGLAdapter_43(uint cap);

	// Token: 0x020000AB RID: 171
	// (Invoke) Token: 0x06000B68 RID: 2920
	public delegate void SwigDelegateGLAdapter_44(uint index);

	// Token: 0x020000AC RID: 172
	// (Invoke) Token: 0x06000B6C RID: 2924
	public delegate void SwigDelegateGLAdapter_45(uint mode, int first, int count);

	// Token: 0x020000AD RID: 173
	// (Invoke) Token: 0x06000B70 RID: 2928
	public delegate void SwigDelegateGLAdapter_46(uint mode, int count, uint type, IntPtr pIndicesPtrData);

	// Token: 0x020000AE RID: 174
	// (Invoke) Token: 0x06000B74 RID: 2932
	public delegate void SwigDelegateGLAdapter_47(uint cap);

	// Token: 0x020000AF RID: 175
	// (Invoke) Token: 0x06000B78 RID: 2936
	public delegate void SwigDelegateGLAdapter_48(uint index);

	// Token: 0x020000B0 RID: 176
	// (Invoke) Token: 0x06000B7C RID: 2940
	public delegate void SwigDelegateGLAdapter_49();

	// Token: 0x020000B1 RID: 177
	// (Invoke) Token: 0x06000B80 RID: 2944
	public delegate void SwigDelegateGLAdapter_50();

	// Token: 0x020000B2 RID: 178
	// (Invoke) Token: 0x06000B84 RID: 2948
	public delegate void SwigDelegateGLAdapter_51(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);

	// Token: 0x020000B3 RID: 179
	// (Invoke) Token: 0x06000B88 RID: 2952
	public delegate void SwigDelegateGLAdapter_52(uint target, uint attachment, uint textarget, uint texture, int level);

	// Token: 0x020000B4 RID: 180
	// (Invoke) Token: 0x06000B8C RID: 2956
	public delegate void SwigDelegateGLAdapter_53(uint mode);

	// Token: 0x020000B5 RID: 181
	// (Invoke) Token: 0x06000B90 RID: 2960
	public delegate void SwigDelegateGLAdapter_54(int n, IntPtr pBuffersPtrData);

	// Token: 0x020000B6 RID: 182
	// (Invoke) Token: 0x06000B94 RID: 2964
	public delegate void SwigDelegateGLAdapter_55(uint target);

	// Token: 0x020000B7 RID: 183
	// (Invoke) Token: 0x06000B98 RID: 2968
	public delegate void SwigDelegateGLAdapter_56(int n, IntPtr pFramebuffersPtrData);

	// Token: 0x020000B8 RID: 184
	// (Invoke) Token: 0x06000B9C RID: 2972
	public delegate void SwigDelegateGLAdapter_57(int n, IntPtr pRenderbuffersPtrData);

	// Token: 0x020000B9 RID: 185
	// (Invoke) Token: 0x06000BA0 RID: 2976
	public delegate void SwigDelegateGLAdapter_58(int n, IntPtr pTexturesPtrData);

	// Token: 0x020000BA RID: 186
	// (Invoke) Token: 0x06000BA4 RID: 2980
	public delegate void SwigDelegateGLAdapter_59(uint program, uint index, int bufsize, IntPtr pLengthPtrData, IntPtr pSizePtrData, IntPtr pTypePtrData, IntPtr pNamePtrData);

	// Token: 0x020000BB RID: 187
	// (Invoke) Token: 0x06000BA8 RID: 2984
	public delegate void SwigDelegateGLAdapter_60(uint program, uint index, int bufsize, IntPtr pLengthPtrData, IntPtr pSizePtrData, IntPtr pTypePtrData, IntPtr pNamePtrData);

	// Token: 0x020000BC RID: 188
	// (Invoke) Token: 0x06000BAC RID: 2988
	public delegate void SwigDelegateGLAdapter_61(uint program, int maxcount, IntPtr pCountPtrData, IntPtr pShadersPtrData);

	// Token: 0x020000BD RID: 189
	// (Invoke) Token: 0x06000BB0 RID: 2992
	public delegate void SwigDelegateGLAdapter_62(uint returnVal, uint program, IntPtr pNamePtrData);

	// Token: 0x020000BE RID: 190
	// (Invoke) Token: 0x06000BB4 RID: 2996
	public delegate void SwigDelegateGLAdapter_63(uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000BF RID: 191
	// (Invoke) Token: 0x06000BB8 RID: 3000
	public delegate void SwigDelegateGLAdapter_64(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000C0 RID: 192
	// (Invoke) Token: 0x06000BBC RID: 3004
	public delegate void SwigDelegateGLAdapter_65(uint returnVal);

	// Token: 0x020000C1 RID: 193
	// (Invoke) Token: 0x06000BC0 RID: 3008
	public delegate void SwigDelegateGLAdapter_66(uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000C2 RID: 194
	// (Invoke) Token: 0x06000BC4 RID: 3012
	public delegate void SwigDelegateGLAdapter_67(uint target, uint attachment, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000C3 RID: 195
	// (Invoke) Token: 0x06000BC8 RID: 3016
	public delegate void SwigDelegateGLAdapter_68(uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000C4 RID: 196
	// (Invoke) Token: 0x06000BCC RID: 3020
	public delegate void SwigDelegateGLAdapter_69(uint program, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000C5 RID: 197
	// (Invoke) Token: 0x06000BD0 RID: 3024
	public delegate void SwigDelegateGLAdapter_70(uint program, int bufsize, IntPtr pLengthPtrData, IntPtr pInfologPtrData);

	// Token: 0x020000C6 RID: 198
	// (Invoke) Token: 0x06000BD4 RID: 3028
	public delegate void SwigDelegateGLAdapter_71(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000C7 RID: 199
	// (Invoke) Token: 0x06000BD8 RID: 3032
	public delegate void SwigDelegateGLAdapter_72(uint shader, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000C8 RID: 200
	// (Invoke) Token: 0x06000BDC RID: 3036
	public delegate void SwigDelegateGLAdapter_73(uint shader, int bufsize, IntPtr pLengthPtrData, IntPtr pInfologPtrData);

	// Token: 0x020000C9 RID: 201
	// (Invoke) Token: 0x06000BE0 RID: 3040
	public delegate void SwigDelegateGLAdapter_74(uint shadertype, uint precisiontype, IntPtr pRangePtrData, IntPtr pPrecisionPtrData);

	// Token: 0x020000CA RID: 202
	// (Invoke) Token: 0x06000BE4 RID: 3044
	public delegate void SwigDelegateGLAdapter_75(uint shader, int bufsize, IntPtr pLengthPtrData, IntPtr pSourcePtrData);

	// Token: 0x020000CB RID: 203
	// (Invoke) Token: 0x06000BE8 RID: 3048
	public delegate void SwigDelegateGLAdapter_76(IntPtr pReturnPtrData, uint name);

	// Token: 0x020000CC RID: 204
	// (Invoke) Token: 0x06000BEC RID: 3052
	public delegate void SwigDelegateGLAdapter_77(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000CD RID: 205
	// (Invoke) Token: 0x06000BF0 RID: 3056
	public delegate void SwigDelegateGLAdapter_78(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000CE RID: 206
	// (Invoke) Token: 0x06000BF4 RID: 3060
	public delegate void SwigDelegateGLAdapter_79(uint program, uint location, IntPtr pParamsPtrData);

	// Token: 0x020000CF RID: 207
	// (Invoke) Token: 0x06000BF8 RID: 3064
	public delegate void SwigDelegateGLAdapter_80(uint program, uint location, IntPtr pParamsPtrData);

	// Token: 0x020000D0 RID: 208
	// (Invoke) Token: 0x06000BFC RID: 3068
	public delegate void SwigDelegateGLAdapter_81(uint returnVal, uint program, IntPtr pNamePtrData);

	// Token: 0x020000D1 RID: 209
	// (Invoke) Token: 0x06000C00 RID: 3072
	public delegate void SwigDelegateGLAdapter_82(uint index, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000D2 RID: 210
	// (Invoke) Token: 0x06000C04 RID: 3076
	public delegate void SwigDelegateGLAdapter_83(uint index, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000D3 RID: 211
	// (Invoke) Token: 0x06000C08 RID: 3080
	public delegate void SwigDelegateGLAdapter_84(uint index, uint pname, IntPtr pPointerPtrData);

	// Token: 0x020000D4 RID: 212
	// (Invoke) Token: 0x06000C0C RID: 3084
	public delegate void SwigDelegateGLAdapter_85(uint target, uint mode);

	// Token: 0x020000D5 RID: 213
	// (Invoke) Token: 0x06000C10 RID: 3088
	public delegate void SwigDelegateGLAdapter_86(int returnVal, uint buffer);

	// Token: 0x020000D6 RID: 214
	// (Invoke) Token: 0x06000C14 RID: 3092
	public delegate void SwigDelegateGLAdapter_87(int returnVal, uint cap);

	// Token: 0x020000D7 RID: 215
	// (Invoke) Token: 0x06000C18 RID: 3096
	public delegate void SwigDelegateGLAdapter_88(int returnVal, uint framebuffer);

	// Token: 0x020000D8 RID: 216
	// (Invoke) Token: 0x06000C1C RID: 3100
	public delegate void SwigDelegateGLAdapter_89(int returnVal, uint program);

	// Token: 0x020000D9 RID: 217
	// (Invoke) Token: 0x06000C20 RID: 3104
	public delegate void SwigDelegateGLAdapter_90(int returnVal, uint renderbuffer);

	// Token: 0x020000DA RID: 218
	// (Invoke) Token: 0x06000C24 RID: 3108
	public delegate void SwigDelegateGLAdapter_91(int returnVal, uint shader);

	// Token: 0x020000DB RID: 219
	// (Invoke) Token: 0x06000C28 RID: 3112
	public delegate void SwigDelegateGLAdapter_92(int returnVal, uint texture);

	// Token: 0x020000DC RID: 220
	// (Invoke) Token: 0x06000C2C RID: 3116
	public delegate void SwigDelegateGLAdapter_93(float width);

	// Token: 0x020000DD RID: 221
	// (Invoke) Token: 0x06000C30 RID: 3120
	public delegate void SwigDelegateGLAdapter_94(uint program);

	// Token: 0x020000DE RID: 222
	// (Invoke) Token: 0x06000C34 RID: 3124
	public delegate void SwigDelegateGLAdapter_95(uint pname, int param);

	// Token: 0x020000DF RID: 223
	// (Invoke) Token: 0x06000C38 RID: 3128
	public delegate void SwigDelegateGLAdapter_96(float factor, float units);

	// Token: 0x020000E0 RID: 224
	// (Invoke) Token: 0x06000C3C RID: 3132
	public delegate void SwigDelegateGLAdapter_97(int x, int y, int width, int height, uint format, uint type, IntPtr pPixelsPtrData);

	// Token: 0x020000E1 RID: 225
	// (Invoke) Token: 0x06000C40 RID: 3136
	public delegate void SwigDelegateGLAdapter_98();

	// Token: 0x020000E2 RID: 226
	// (Invoke) Token: 0x06000C44 RID: 3140
	public delegate void SwigDelegateGLAdapter_99(uint target, uint internalformat, int width, int height);

	// Token: 0x020000E3 RID: 227
	// (Invoke) Token: 0x06000C48 RID: 3144
	public delegate void SwigDelegateGLAdapter_100(float value, int invert);

	// Token: 0x020000E4 RID: 228
	// (Invoke) Token: 0x06000C4C RID: 3148
	public delegate void SwigDelegateGLAdapter_101(int x, int y, int width, int height);

	// Token: 0x020000E5 RID: 229
	// (Invoke) Token: 0x06000C50 RID: 3152
	public delegate void SwigDelegateGLAdapter_102(int n, IntPtr pShadersPtrData, uint binaryformat, IntPtr pBinaryPtrData, int length);

	// Token: 0x020000E6 RID: 230
	// (Invoke) Token: 0x06000C54 RID: 3156
	public delegate void SwigDelegateGLAdapter_103(uint shader, int count, IntPtr pStrPtrData, IntPtr pLengthPtrData);

	// Token: 0x020000E7 RID: 231
	// (Invoke) Token: 0x06000C58 RID: 3160
	public delegate void SwigDelegateGLAdapter_104(uint func, int arg1, uint mask);

	// Token: 0x020000E8 RID: 232
	// (Invoke) Token: 0x06000C5C RID: 3164
	public delegate void SwigDelegateGLAdapter_105(uint face, uint func, int arg2, uint mask);

	// Token: 0x020000E9 RID: 233
	// (Invoke) Token: 0x06000C60 RID: 3168
	public delegate void SwigDelegateGLAdapter_106(uint mask);

	// Token: 0x020000EA RID: 234
	// (Invoke) Token: 0x06000C64 RID: 3172
	public delegate void SwigDelegateGLAdapter_107(uint face, uint mask);

	// Token: 0x020000EB RID: 235
	// (Invoke) Token: 0x06000C68 RID: 3176
	public delegate void SwigDelegateGLAdapter_108(uint fail, uint zfail, uint zpass);

	// Token: 0x020000EC RID: 236
	// (Invoke) Token: 0x06000C6C RID: 3180
	public delegate void SwigDelegateGLAdapter_109(uint face, uint fail, uint zfail, uint zpass);

	// Token: 0x020000ED RID: 237
	// (Invoke) Token: 0x06000C70 RID: 3184
	public delegate void SwigDelegateGLAdapter_110(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pPixelsPtrData);

	// Token: 0x020000EE RID: 238
	// (Invoke) Token: 0x06000C74 RID: 3188
	public delegate void SwigDelegateGLAdapter_111(uint target, uint pname, float param);

	// Token: 0x020000EF RID: 239
	// (Invoke) Token: 0x06000C78 RID: 3192
	public delegate void SwigDelegateGLAdapter_112(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000F0 RID: 240
	// (Invoke) Token: 0x06000C7C RID: 3196
	public delegate void SwigDelegateGLAdapter_113(uint target, uint pname, int param);

	// Token: 0x020000F1 RID: 241
	// (Invoke) Token: 0x06000C80 RID: 3200
	public delegate void SwigDelegateGLAdapter_114(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020000F2 RID: 242
	// (Invoke) Token: 0x06000C84 RID: 3204
	public delegate void SwigDelegateGLAdapter_115(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pPixelsPtrData);

	// Token: 0x020000F3 RID: 243
	// (Invoke) Token: 0x06000C88 RID: 3208
	public delegate void SwigDelegateGLAdapter_116(uint location, float x);

	// Token: 0x020000F4 RID: 244
	// (Invoke) Token: 0x06000C8C RID: 3212
	public delegate void SwigDelegateGLAdapter_117(uint location, int count, IntPtr pVPtrData);

	// Token: 0x020000F5 RID: 245
	// (Invoke) Token: 0x06000C90 RID: 3216
	public delegate void SwigDelegateGLAdapter_118(uint location, int x);

	// Token: 0x020000F6 RID: 246
	// (Invoke) Token: 0x06000C94 RID: 3220
	public delegate void SwigDelegateGLAdapter_119(uint location, int count, IntPtr pVPtrData);

	// Token: 0x020000F7 RID: 247
	// (Invoke) Token: 0x06000C98 RID: 3224
	public delegate void SwigDelegateGLAdapter_120(uint location, float x, float y);

	// Token: 0x020000F8 RID: 248
	// (Invoke) Token: 0x06000C9C RID: 3228
	public delegate void SwigDelegateGLAdapter_121(uint location, int count, IntPtr pVPtrData);

	// Token: 0x020000F9 RID: 249
	// (Invoke) Token: 0x06000CA0 RID: 3232
	public delegate void SwigDelegateGLAdapter_122(uint location, int x, int y);

	// Token: 0x020000FA RID: 250
	// (Invoke) Token: 0x06000CA4 RID: 3236
	public delegate void SwigDelegateGLAdapter_123(uint location, int count, IntPtr pVPtrData);

	// Token: 0x020000FB RID: 251
	// (Invoke) Token: 0x06000CA8 RID: 3240
	public delegate void SwigDelegateGLAdapter_124(uint location, float x, float y, float z);

	// Token: 0x020000FC RID: 252
	// (Invoke) Token: 0x06000CAC RID: 3244
	public delegate void SwigDelegateGLAdapter_125(uint location, int count, IntPtr pVPtrData);

	// Token: 0x020000FD RID: 253
	// (Invoke) Token: 0x06000CB0 RID: 3248
	public delegate void SwigDelegateGLAdapter_126(uint location, int x, int y, int z);

	// Token: 0x020000FE RID: 254
	// (Invoke) Token: 0x06000CB4 RID: 3252
	public delegate void SwigDelegateGLAdapter_127(uint location, int count, IntPtr pVPtrData);

	// Token: 0x020000FF RID: 255
	// (Invoke) Token: 0x06000CB8 RID: 3256
	public delegate void SwigDelegateGLAdapter_128(uint location, float x, float y, float z, float w);

	// Token: 0x02000100 RID: 256
	// (Invoke) Token: 0x06000CBC RID: 3260
	public delegate void SwigDelegateGLAdapter_129(uint location, int count, IntPtr pVPtrData);

	// Token: 0x02000101 RID: 257
	// (Invoke) Token: 0x06000CC0 RID: 3264
	public delegate void SwigDelegateGLAdapter_130(uint location, int x, int y, int z, int w);

	// Token: 0x02000102 RID: 258
	// (Invoke) Token: 0x06000CC4 RID: 3268
	public delegate void SwigDelegateGLAdapter_131(uint location, int count, IntPtr pVPtrData);

	// Token: 0x02000103 RID: 259
	// (Invoke) Token: 0x06000CC8 RID: 3272
	public delegate void SwigDelegateGLAdapter_132(uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x02000104 RID: 260
	// (Invoke) Token: 0x06000CCC RID: 3276
	public delegate void SwigDelegateGLAdapter_133(uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x02000105 RID: 261
	// (Invoke) Token: 0x06000CD0 RID: 3280
	public delegate void SwigDelegateGLAdapter_134(uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x02000106 RID: 262
	// (Invoke) Token: 0x06000CD4 RID: 3284
	public delegate void SwigDelegateGLAdapter_135(uint program);

	// Token: 0x02000107 RID: 263
	// (Invoke) Token: 0x06000CD8 RID: 3288
	public delegate void SwigDelegateGLAdapter_136(uint program);

	// Token: 0x02000108 RID: 264
	// (Invoke) Token: 0x06000CDC RID: 3292
	public delegate void SwigDelegateGLAdapter_137(uint indx, float x);

	// Token: 0x02000109 RID: 265
	// (Invoke) Token: 0x06000CE0 RID: 3296
	public delegate void SwigDelegateGLAdapter_138(uint indx, IntPtr pValuesPtrData);

	// Token: 0x0200010A RID: 266
	// (Invoke) Token: 0x06000CE4 RID: 3300
	public delegate void SwigDelegateGLAdapter_139(uint indx, float x, float y);

	// Token: 0x0200010B RID: 267
	// (Invoke) Token: 0x06000CE8 RID: 3304
	public delegate void SwigDelegateGLAdapter_140(uint indx, IntPtr pValuesPtrData);

	// Token: 0x0200010C RID: 268
	// (Invoke) Token: 0x06000CEC RID: 3308
	public delegate void SwigDelegateGLAdapter_141(uint indx, float x, float y, float z);

	// Token: 0x0200010D RID: 269
	// (Invoke) Token: 0x06000CF0 RID: 3312
	public delegate void SwigDelegateGLAdapter_142(uint indx, IntPtr pValuesPtrData);

	// Token: 0x0200010E RID: 270
	// (Invoke) Token: 0x06000CF4 RID: 3316
	public delegate void SwigDelegateGLAdapter_143(uint indx, float x, float y, float z, float w);

	// Token: 0x0200010F RID: 271
	// (Invoke) Token: 0x06000CF8 RID: 3320
	public delegate void SwigDelegateGLAdapter_144(uint indx, IntPtr pValuesPtrData);

	// Token: 0x02000110 RID: 272
	// (Invoke) Token: 0x06000CFC RID: 3324
	public delegate void SwigDelegateGLAdapter_145(uint indx, int size, uint type, int normalized, int stride, IntPtr pPtrPtrData);

	// Token: 0x02000111 RID: 273
	// (Invoke) Token: 0x06000D00 RID: 3328
	public delegate void SwigDelegateGLAdapter_146(int x, int y, int width, int height);

	// Token: 0x02000112 RID: 274
	// (Invoke) Token: 0x06000D04 RID: 3332
	public delegate void SwigDelegateGLAdapter_147(uint mode);

	// Token: 0x02000113 RID: 275
	// (Invoke) Token: 0x06000D08 RID: 3336
	public delegate void SwigDelegateGLAdapter_148(uint mode, uint start, uint end, int count, uint type, IntPtr pIndicesPtrData);

	// Token: 0x02000114 RID: 276
	// (Invoke) Token: 0x06000D0C RID: 3340
	public delegate void SwigDelegateGLAdapter_149(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pPixelsPtrData);

	// Token: 0x02000115 RID: 277
	// (Invoke) Token: 0x06000D10 RID: 3344
	public delegate void SwigDelegateGLAdapter_150(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pPixelsPtrData);

	// Token: 0x02000116 RID: 278
	// (Invoke) Token: 0x06000D14 RID: 3348
	public delegate void SwigDelegateGLAdapter_151(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);

	// Token: 0x02000117 RID: 279
	// (Invoke) Token: 0x06000D18 RID: 3352
	public delegate void SwigDelegateGLAdapter_152(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr pDataPtrData);

	// Token: 0x02000118 RID: 280
	// (Invoke) Token: 0x06000D1C RID: 3356
	public delegate void SwigDelegateGLAdapter_153(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr pDataPtrData);

	// Token: 0x02000119 RID: 281
	// (Invoke) Token: 0x06000D20 RID: 3360
	public delegate void SwigDelegateGLAdapter_154(int n, IntPtr pIdsPtrData);

	// Token: 0x0200011A RID: 282
	// (Invoke) Token: 0x06000D24 RID: 3364
	public delegate void SwigDelegateGLAdapter_155(int n, IntPtr pIdsPtrData);

	// Token: 0x0200011B RID: 283
	// (Invoke) Token: 0x06000D28 RID: 3368
	public delegate void SwigDelegateGLAdapter_156(int returnVal, uint id);

	// Token: 0x0200011C RID: 284
	// (Invoke) Token: 0x06000D2C RID: 3372
	public delegate void SwigDelegateGLAdapter_157(uint target, uint id);

	// Token: 0x0200011D RID: 285
	// (Invoke) Token: 0x06000D30 RID: 3376
	public delegate void SwigDelegateGLAdapter_158(uint target);

	// Token: 0x0200011E RID: 286
	// (Invoke) Token: 0x06000D34 RID: 3380
	public delegate void SwigDelegateGLAdapter_159(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200011F RID: 287
	// (Invoke) Token: 0x06000D38 RID: 3384
	public delegate void SwigDelegateGLAdapter_160(uint id, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000120 RID: 288
	// (Invoke) Token: 0x06000D3C RID: 3388
	public delegate void SwigDelegateGLAdapter_161(int returnVal, uint target);

	// Token: 0x02000121 RID: 289
	// (Invoke) Token: 0x06000D40 RID: 3392
	public delegate void SwigDelegateGLAdapter_162(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000122 RID: 290
	// (Invoke) Token: 0x06000D44 RID: 3396
	public delegate void SwigDelegateGLAdapter_163(int n, IntPtr pBufsPtrData);

	// Token: 0x02000123 RID: 291
	// (Invoke) Token: 0x06000D48 RID: 3400
	public delegate void SwigDelegateGLAdapter_164(uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x02000124 RID: 292
	// (Invoke) Token: 0x06000D4C RID: 3404
	public delegate void SwigDelegateGLAdapter_165(uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x02000125 RID: 293
	// (Invoke) Token: 0x06000D50 RID: 3408
	public delegate void SwigDelegateGLAdapter_166(uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x02000126 RID: 294
	// (Invoke) Token: 0x06000D54 RID: 3412
	public delegate void SwigDelegateGLAdapter_167(uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x02000127 RID: 295
	// (Invoke) Token: 0x06000D58 RID: 3416
	public delegate void SwigDelegateGLAdapter_168(uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x02000128 RID: 296
	// (Invoke) Token: 0x06000D5C RID: 3420
	public delegate void SwigDelegateGLAdapter_169(uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x02000129 RID: 297
	// (Invoke) Token: 0x06000D60 RID: 3424
	public delegate void SwigDelegateGLAdapter_170(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);

	// Token: 0x0200012A RID: 298
	// (Invoke) Token: 0x06000D64 RID: 3428
	public delegate void SwigDelegateGLAdapter_171(uint target, int samples, uint internalformat, int width, int height);

	// Token: 0x0200012B RID: 299
	// (Invoke) Token: 0x06000D68 RID: 3432
	public delegate void SwigDelegateGLAdapter_172(uint target, uint attachment, uint texture, int level, int layer);

	// Token: 0x0200012C RID: 300
	// (Invoke) Token: 0x06000D6C RID: 3436
	public delegate void SwigDelegateGLAdapter_173(IntPtr pReturnPtrData, uint target, int offset, int length, uint access);

	// Token: 0x0200012D RID: 301
	// (Invoke) Token: 0x06000D70 RID: 3440
	public delegate void SwigDelegateGLAdapter_174(uint target, int offset, int length);

	// Token: 0x0200012E RID: 302
	// (Invoke) Token: 0x06000D74 RID: 3444
	public delegate void SwigDelegateGLAdapter_175(uint array);

	// Token: 0x0200012F RID: 303
	// (Invoke) Token: 0x06000D78 RID: 3448
	public delegate void SwigDelegateGLAdapter_176(int n, IntPtr pArraysPtrData);

	// Token: 0x02000130 RID: 304
	// (Invoke) Token: 0x06000D7C RID: 3452
	public delegate void SwigDelegateGLAdapter_177(int n, IntPtr pArraysPtrData);

	// Token: 0x02000131 RID: 305
	// (Invoke) Token: 0x06000D80 RID: 3456
	public delegate void SwigDelegateGLAdapter_178(int returnVal, uint array);

	// Token: 0x02000132 RID: 306
	// (Invoke) Token: 0x06000D84 RID: 3460
	public delegate void SwigDelegateGLAdapter_179(uint target, uint index, IntPtr pDataPtrData);

	// Token: 0x02000133 RID: 307
	// (Invoke) Token: 0x06000D88 RID: 3464
	public delegate void SwigDelegateGLAdapter_180(uint target, uint index, IntPtr pDataPtrData);

	// Token: 0x02000134 RID: 308
	// (Invoke) Token: 0x06000D8C RID: 3468
	public delegate void SwigDelegateGLAdapter_181(uint primitiveMode);

	// Token: 0x02000135 RID: 309
	// (Invoke) Token: 0x06000D90 RID: 3472
	public delegate void SwigDelegateGLAdapter_182();

	// Token: 0x02000136 RID: 310
	// (Invoke) Token: 0x06000D94 RID: 3476
	public delegate void SwigDelegateGLAdapter_183(uint target, uint index, uint buffer, int offset, int size);

	// Token: 0x02000137 RID: 311
	// (Invoke) Token: 0x06000D98 RID: 3480
	public delegate void SwigDelegateGLAdapter_184(uint target, uint index, uint buffer);

	// Token: 0x02000138 RID: 312
	// (Invoke) Token: 0x06000D9C RID: 3484
	public delegate void SwigDelegateGLAdapter_185(uint program, int count, IntPtr pVaryingsPtrData, uint bufferMode);

	// Token: 0x02000139 RID: 313
	// (Invoke) Token: 0x06000DA0 RID: 3488
	public delegate void SwigDelegateGLAdapter_186(uint program, uint index, int bufSize, IntPtr pLengthPtrData, IntPtr pSizePtrData, IntPtr pTypePtrData, IntPtr pNamePtrData);

	// Token: 0x0200013A RID: 314
	// (Invoke) Token: 0x06000DA4 RID: 3492
	public delegate void SwigDelegateGLAdapter_187(uint index, int size, uint type, int stride, IntPtr pPointerPtrData);

	// Token: 0x0200013B RID: 315
	// (Invoke) Token: 0x06000DA8 RID: 3496
	public delegate void SwigDelegateGLAdapter_188(uint index, uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200013C RID: 316
	// (Invoke) Token: 0x06000DAC RID: 3500
	public delegate void SwigDelegateGLAdapter_189(uint index, uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200013D RID: 317
	// (Invoke) Token: 0x06000DB0 RID: 3504
	public delegate void SwigDelegateGLAdapter_190(uint index, int x, int y, int z, int w);

	// Token: 0x0200013E RID: 318
	// (Invoke) Token: 0x06000DB4 RID: 3508
	public delegate void SwigDelegateGLAdapter_191(uint index, uint x, uint y, uint z, uint w);

	// Token: 0x0200013F RID: 319
	// (Invoke) Token: 0x06000DB8 RID: 3512
	public delegate void SwigDelegateGLAdapter_192(uint index, IntPtr pVPtrData);

	// Token: 0x02000140 RID: 320
	// (Invoke) Token: 0x06000DBC RID: 3516
	public delegate void SwigDelegateGLAdapter_193(uint index, IntPtr pVPtrData);

	// Token: 0x02000141 RID: 321
	// (Invoke) Token: 0x06000DC0 RID: 3520
	public delegate void SwigDelegateGLAdapter_194(uint program, uint location, IntPtr pParamsPtrData);

	// Token: 0x02000142 RID: 322
	// (Invoke) Token: 0x06000DC4 RID: 3524
	public delegate void SwigDelegateGLAdapter_195(uint returnVal, uint program, IntPtr pNamePtrData);

	// Token: 0x02000143 RID: 323
	// (Invoke) Token: 0x06000DC8 RID: 3528
	public delegate void SwigDelegateGLAdapter_196(uint location, uint v0);

	// Token: 0x02000144 RID: 324
	// (Invoke) Token: 0x06000DCC RID: 3532
	public delegate void SwigDelegateGLAdapter_197(uint location, uint v0, uint v1);

	// Token: 0x02000145 RID: 325
	// (Invoke) Token: 0x06000DD0 RID: 3536
	public delegate void SwigDelegateGLAdapter_198(uint location, uint v0, uint v1, uint v2);

	// Token: 0x02000146 RID: 326
	// (Invoke) Token: 0x06000DD4 RID: 3540
	public delegate void SwigDelegateGLAdapter_199(uint location, uint v0, uint v1, uint v2, uint v3);

	// Token: 0x02000147 RID: 327
	// (Invoke) Token: 0x06000DD8 RID: 3544
	public delegate void SwigDelegateGLAdapter_200(uint location, int count, IntPtr pValuePtrData);

	// Token: 0x02000148 RID: 328
	// (Invoke) Token: 0x06000DDC RID: 3548
	public delegate void SwigDelegateGLAdapter_201(uint location, int count, IntPtr pValuePtrData);

	// Token: 0x02000149 RID: 329
	// (Invoke) Token: 0x06000DE0 RID: 3552
	public delegate void SwigDelegateGLAdapter_202(uint location, int count, IntPtr pValuePtrData);

	// Token: 0x0200014A RID: 330
	// (Invoke) Token: 0x06000DE4 RID: 3556
	public delegate void SwigDelegateGLAdapter_203(uint location, int count, IntPtr pValuePtrData);

	// Token: 0x0200014B RID: 331
	// (Invoke) Token: 0x06000DE8 RID: 3560
	public delegate void SwigDelegateGLAdapter_204(uint buffer, int drawbuffer, IntPtr pValuePtrData);

	// Token: 0x0200014C RID: 332
	// (Invoke) Token: 0x06000DEC RID: 3564
	public delegate void SwigDelegateGLAdapter_205(uint buffer, int drawbuffer, IntPtr pValuePtrData);

	// Token: 0x0200014D RID: 333
	// (Invoke) Token: 0x06000DF0 RID: 3568
	public delegate void SwigDelegateGLAdapter_206(uint buffer, int drawbuffer, IntPtr pValuePtrData);

	// Token: 0x0200014E RID: 334
	// (Invoke) Token: 0x06000DF4 RID: 3572
	public delegate void SwigDelegateGLAdapter_207(uint buffer, int drawbuffer, float depth, int stencil);

	// Token: 0x0200014F RID: 335
	// (Invoke) Token: 0x06000DF8 RID: 3576
	public delegate void SwigDelegateGLAdapter_208(IntPtr pReturnPtrData, uint name, uint index);

	// Token: 0x02000150 RID: 336
	// (Invoke) Token: 0x06000DFC RID: 3580
	public delegate void SwigDelegateGLAdapter_209(uint readTarget, uint writeTarget, int readOffset, int writeOffset, int size);

	// Token: 0x02000151 RID: 337
	// (Invoke) Token: 0x06000E00 RID: 3584
	public delegate void SwigDelegateGLAdapter_210(uint program, int uniformCount, IntPtr pUniformNamesPtrData, IntPtr pUniformIndicesPtrData);

	// Token: 0x02000152 RID: 338
	// (Invoke) Token: 0x06000E04 RID: 3588
	public delegate void SwigDelegateGLAdapter_211(uint program, int uniformCount, IntPtr pUniformIndicesPtrData, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000153 RID: 339
	// (Invoke) Token: 0x06000E08 RID: 3592
	public delegate void SwigDelegateGLAdapter_212(uint returnVal, uint program, IntPtr pUniformBlockNamePtrData);

	// Token: 0x02000154 RID: 340
	// (Invoke) Token: 0x06000E0C RID: 3596
	public delegate void SwigDelegateGLAdapter_213(uint program, uint uniformBlockIndex, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000155 RID: 341
	// (Invoke) Token: 0x06000E10 RID: 3600
	public delegate void SwigDelegateGLAdapter_214(uint program, uint uniformBlockIndex, int bufSize, IntPtr pLengthPtrData, IntPtr pUniformBlockNamePtrData);

	// Token: 0x02000156 RID: 342
	// (Invoke) Token: 0x06000E14 RID: 3604
	public delegate void SwigDelegateGLAdapter_215(uint program, uint uniformBlockIndex, uint uniformBlockBinding);

	// Token: 0x02000157 RID: 343
	// (Invoke) Token: 0x06000E18 RID: 3608
	public delegate void SwigDelegateGLAdapter_216(uint mode, int first, int count, int instanceCount);

	// Token: 0x02000158 RID: 344
	// (Invoke) Token: 0x06000E1C RID: 3612
	public delegate void SwigDelegateGLAdapter_217(uint mode, int count, uint type, IntPtr pIndicesPtrData, int instanceCount);

	// Token: 0x02000159 RID: 345
	// (Invoke) Token: 0x06000E20 RID: 3616
	public delegate void SwigDelegateGLAdapter_218(uint returnVal, uint condition, uint flags);

	// Token: 0x0200015A RID: 346
	// (Invoke) Token: 0x06000E24 RID: 3620
	public delegate void SwigDelegateGLAdapter_219(int returnVal, uint sync);

	// Token: 0x0200015B RID: 347
	// (Invoke) Token: 0x06000E28 RID: 3624
	public delegate void SwigDelegateGLAdapter_220(uint sync);

	// Token: 0x0200015C RID: 348
	// (Invoke) Token: 0x06000E2C RID: 3628
	public delegate void SwigDelegateGLAdapter_221(uint returnVal, uint sync, uint flags, ulong timeout);

	// Token: 0x0200015D RID: 349
	// (Invoke) Token: 0x06000E30 RID: 3632
	public delegate void SwigDelegateGLAdapter_222(uint sync, uint flags, ulong timeout);

	// Token: 0x0200015E RID: 350
	// (Invoke) Token: 0x06000E34 RID: 3636
	public delegate void SwigDelegateGLAdapter_223(uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200015F RID: 351
	// (Invoke) Token: 0x06000E38 RID: 3640
	public delegate void SwigDelegateGLAdapter_224(uint sync, uint pname, int bufSize, IntPtr pLengthPtrData, IntPtr pValuesPtrData);

	// Token: 0x02000160 RID: 352
	// (Invoke) Token: 0x06000E3C RID: 3644
	public delegate void SwigDelegateGLAdapter_225(uint target, uint index, IntPtr pDataPtrData);

	// Token: 0x02000161 RID: 353
	// (Invoke) Token: 0x06000E40 RID: 3648
	public delegate void SwigDelegateGLAdapter_226(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000162 RID: 354
	// (Invoke) Token: 0x06000E44 RID: 3652
	public delegate void SwigDelegateGLAdapter_227(int count, IntPtr pSamplersPtrData);

	// Token: 0x02000163 RID: 355
	// (Invoke) Token: 0x06000E48 RID: 3656
	public delegate void SwigDelegateGLAdapter_228(int count, IntPtr pSamplersPtrData);

	// Token: 0x02000164 RID: 356
	// (Invoke) Token: 0x06000E4C RID: 3660
	public delegate void SwigDelegateGLAdapter_229(int returnVal, uint sampler);

	// Token: 0x02000165 RID: 357
	// (Invoke) Token: 0x06000E50 RID: 3664
	public delegate void SwigDelegateGLAdapter_230(uint unit, uint sampler);

	// Token: 0x02000166 RID: 358
	// (Invoke) Token: 0x06000E54 RID: 3668
	public delegate void SwigDelegateGLAdapter_231(uint sampler, uint pname, int param);

	// Token: 0x02000167 RID: 359
	// (Invoke) Token: 0x06000E58 RID: 3672
	public delegate void SwigDelegateGLAdapter_232(uint sampler, uint pname, IntPtr pParamPtrData);

	// Token: 0x02000168 RID: 360
	// (Invoke) Token: 0x06000E5C RID: 3676
	public delegate void SwigDelegateGLAdapter_233(uint sampler, uint pname, float param);

	// Token: 0x02000169 RID: 361
	// (Invoke) Token: 0x06000E60 RID: 3680
	public delegate void SwigDelegateGLAdapter_234(uint sampler, uint pname, IntPtr pParamPtrData);

	// Token: 0x0200016A RID: 362
	// (Invoke) Token: 0x06000E64 RID: 3684
	public delegate void SwigDelegateGLAdapter_235(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200016B RID: 363
	// (Invoke) Token: 0x06000E68 RID: 3688
	public delegate void SwigDelegateGLAdapter_236(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200016C RID: 364
	// (Invoke) Token: 0x06000E6C RID: 3692
	public delegate void SwigDelegateGLAdapter_237(uint index, uint divisor);

	// Token: 0x0200016D RID: 365
	// (Invoke) Token: 0x06000E70 RID: 3696
	public delegate void SwigDelegateGLAdapter_238(uint target, uint id);

	// Token: 0x0200016E RID: 366
	// (Invoke) Token: 0x06000E74 RID: 3700
	public delegate void SwigDelegateGLAdapter_239(int n, IntPtr pIdsPtrData);

	// Token: 0x0200016F RID: 367
	// (Invoke) Token: 0x06000E78 RID: 3704
	public delegate void SwigDelegateGLAdapter_240(int n, IntPtr pIdsPtrData);

	// Token: 0x02000170 RID: 368
	// (Invoke) Token: 0x06000E7C RID: 3708
	public delegate void SwigDelegateGLAdapter_241(int returnVal, uint id);

	// Token: 0x02000171 RID: 369
	// (Invoke) Token: 0x06000E80 RID: 3712
	public delegate void SwigDelegateGLAdapter_242();

	// Token: 0x02000172 RID: 370
	// (Invoke) Token: 0x06000E84 RID: 3716
	public delegate void SwigDelegateGLAdapter_243();

	// Token: 0x02000173 RID: 371
	// (Invoke) Token: 0x06000E88 RID: 3720
	public delegate void SwigDelegateGLAdapter_244(uint program, int bufSize, IntPtr pLengthPtrData, IntPtr pBinaryFormatPtrData, IntPtr pBinaryPtrData);

	// Token: 0x02000174 RID: 372
	// (Invoke) Token: 0x06000E8C RID: 3724
	public delegate void SwigDelegateGLAdapter_245(uint program, uint binaryFormat, IntPtr pBinaryPtrData, int length);

	// Token: 0x02000175 RID: 373
	// (Invoke) Token: 0x06000E90 RID: 3728
	public delegate void SwigDelegateGLAdapter_246(uint program, uint pname, int value);

	// Token: 0x02000176 RID: 374
	// (Invoke) Token: 0x06000E94 RID: 3732
	public delegate void SwigDelegateGLAdapter_247(uint target, int numAttachments, IntPtr pAttachmentsPtrData);

	// Token: 0x02000177 RID: 375
	// (Invoke) Token: 0x06000E98 RID: 3736
	public delegate void SwigDelegateGLAdapter_248(uint target, int numAttachments, IntPtr pAttachmentsPtrData, int x, int y, int width, int height);

	// Token: 0x02000178 RID: 376
	// (Invoke) Token: 0x06000E9C RID: 3740
	public delegate void SwigDelegateGLAdapter_249(uint target, int levels, uint internalformat, int width, int height);

	// Token: 0x02000179 RID: 377
	// (Invoke) Token: 0x06000EA0 RID: 3744
	public delegate void SwigDelegateGLAdapter_250(uint target, int levels, uint internalformat, int width, int height, int depth);

	// Token: 0x0200017A RID: 378
	// (Invoke) Token: 0x06000EA4 RID: 3748
	public delegate void SwigDelegateGLAdapter_251(uint target, uint internalformat, uint pname, int bufSize, IntPtr pParamsPtrData);

	// Token: 0x0200017B RID: 379
	// (Invoke) Token: 0x06000EA8 RID: 3752
	public delegate void SwigDelegateGLAdapter_252(uint num_groups_x, uint num_groups_y, uint num_groups_z);

	// Token: 0x0200017C RID: 380
	// (Invoke) Token: 0x06000EAC RID: 3756
	public delegate void SwigDelegateGLAdapter_253(int indirect);

	// Token: 0x0200017D RID: 381
	// (Invoke) Token: 0x06000EB0 RID: 3760
	public delegate void SwigDelegateGLAdapter_254(uint mode, IntPtr pIndirectPtrData);

	// Token: 0x0200017E RID: 382
	// (Invoke) Token: 0x06000EB4 RID: 3764
	public delegate void SwigDelegateGLAdapter_255(uint mode, uint type, IntPtr pIndirectPtrData);

	// Token: 0x0200017F RID: 383
	// (Invoke) Token: 0x06000EB8 RID: 3768
	public delegate void SwigDelegateGLAdapter_256(uint target, uint pname, int param);

	// Token: 0x02000180 RID: 384
	// (Invoke) Token: 0x06000EBC RID: 3772
	public delegate void SwigDelegateGLAdapter_257(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000181 RID: 385
	// (Invoke) Token: 0x06000EC0 RID: 3776
	public delegate void SwigDelegateGLAdapter_258(uint program, uint programInterface, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000182 RID: 386
	// (Invoke) Token: 0x06000EC4 RID: 3780
	public delegate void SwigDelegateGLAdapter_259(uint returnVal, uint program, uint programInterface, IntPtr pNamePtrData);

	// Token: 0x02000183 RID: 387
	// (Invoke) Token: 0x06000EC8 RID: 3784
	public delegate void SwigDelegateGLAdapter_260(uint program, uint programInterface, uint index, int bufSize, IntPtr pLengthPtrData, IntPtr pNamePtrData);

	// Token: 0x02000184 RID: 388
	// (Invoke) Token: 0x06000ECC RID: 3788
	public delegate void SwigDelegateGLAdapter_261(uint program, uint programInterface, uint index, int propCount, IntPtr pPropsPtrData, int bufSize, IntPtr pLengthPtrData, IntPtr pParamsPtrData);

	// Token: 0x02000185 RID: 389
	// (Invoke) Token: 0x06000ED0 RID: 3792
	public delegate void SwigDelegateGLAdapter_262(uint returnVal, uint program, uint programInterface, IntPtr pNamePtrData);

	// Token: 0x02000186 RID: 390
	// (Invoke) Token: 0x06000ED4 RID: 3796
	public delegate void SwigDelegateGLAdapter_263(uint pipeline, uint stages, uint program);

	// Token: 0x02000187 RID: 391
	// (Invoke) Token: 0x06000ED8 RID: 3800
	public delegate void SwigDelegateGLAdapter_264(uint pipeline, uint program);

	// Token: 0x02000188 RID: 392
	// (Invoke) Token: 0x06000EDC RID: 3804
	public delegate void SwigDelegateGLAdapter_265(uint returnVal, uint type, int count, IntPtr pAPtrData);

	// Token: 0x02000189 RID: 393
	// (Invoke) Token: 0x06000EE0 RID: 3808
	public delegate void SwigDelegateGLAdapter_266(uint pipeline);

	// Token: 0x0200018A RID: 394
	// (Invoke) Token: 0x06000EE4 RID: 3812
	public delegate void SwigDelegateGLAdapter_267(int n, IntPtr pPipelinesPtrData);

	// Token: 0x0200018B RID: 395
	// (Invoke) Token: 0x06000EE8 RID: 3816
	public delegate void SwigDelegateGLAdapter_268(int n, IntPtr pPipelinesPtrData);

	// Token: 0x0200018C RID: 396
	// (Invoke) Token: 0x06000EEC RID: 3820
	public delegate void SwigDelegateGLAdapter_269(int returnVal, uint pipeline);

	// Token: 0x0200018D RID: 397
	// (Invoke) Token: 0x06000EF0 RID: 3824
	public delegate void SwigDelegateGLAdapter_270(uint pipeline, uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200018E RID: 398
	// (Invoke) Token: 0x06000EF4 RID: 3828
	public delegate void SwigDelegateGLAdapter_271(uint program, uint location, int v0);

	// Token: 0x0200018F RID: 399
	// (Invoke) Token: 0x06000EF8 RID: 3832
	public delegate void SwigDelegateGLAdapter_272(uint program, uint location, int v0, int v1);

	// Token: 0x02000190 RID: 400
	// (Invoke) Token: 0x06000EFC RID: 3836
	public delegate void SwigDelegateGLAdapter_273(uint program, uint location, int v0, int v1, int v2);

	// Token: 0x02000191 RID: 401
	// (Invoke) Token: 0x06000F00 RID: 3840
	public delegate void SwigDelegateGLAdapter_274(uint program, uint location, int v0, int v1, int v2, int v3);

	// Token: 0x02000192 RID: 402
	// (Invoke) Token: 0x06000F04 RID: 3844
	public delegate void SwigDelegateGLAdapter_275(uint program, uint location, uint v0);

	// Token: 0x02000193 RID: 403
	// (Invoke) Token: 0x06000F08 RID: 3848
	public delegate void SwigDelegateGLAdapter_276(uint program, uint location, uint v0, uint v1);

	// Token: 0x02000194 RID: 404
	// (Invoke) Token: 0x06000F0C RID: 3852
	public delegate void SwigDelegateGLAdapter_277(uint program, uint location, uint v0, uint v1, uint v2);

	// Token: 0x02000195 RID: 405
	// (Invoke) Token: 0x06000F10 RID: 3856
	public delegate void SwigDelegateGLAdapter_278(uint program, uint location, uint v0, uint v1, uint v2, uint v3);

	// Token: 0x02000196 RID: 406
	// (Invoke) Token: 0x06000F14 RID: 3860
	public delegate void SwigDelegateGLAdapter_279(uint program, uint location, float v0);

	// Token: 0x02000197 RID: 407
	// (Invoke) Token: 0x06000F18 RID: 3864
	public delegate void SwigDelegateGLAdapter_280(uint program, uint location, float v0, float v1);

	// Token: 0x02000198 RID: 408
	// (Invoke) Token: 0x06000F1C RID: 3868
	public delegate void SwigDelegateGLAdapter_281(uint program, uint location, float v0, float v1, float v2);

	// Token: 0x02000199 RID: 409
	// (Invoke) Token: 0x06000F20 RID: 3872
	public delegate void SwigDelegateGLAdapter_282(uint program, uint location, float v0, float v1, float v2, float v3);

	// Token: 0x0200019A RID: 410
	// (Invoke) Token: 0x06000F24 RID: 3876
	public delegate void SwigDelegateGLAdapter_283(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x0200019B RID: 411
	// (Invoke) Token: 0x06000F28 RID: 3880
	public delegate void SwigDelegateGLAdapter_284(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x0200019C RID: 412
	// (Invoke) Token: 0x06000F2C RID: 3884
	public delegate void SwigDelegateGLAdapter_285(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x0200019D RID: 413
	// (Invoke) Token: 0x06000F30 RID: 3888
	public delegate void SwigDelegateGLAdapter_286(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x0200019E RID: 414
	// (Invoke) Token: 0x06000F34 RID: 3892
	public delegate void SwigDelegateGLAdapter_287(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x0200019F RID: 415
	// (Invoke) Token: 0x06000F38 RID: 3896
	public delegate void SwigDelegateGLAdapter_288(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x020001A0 RID: 416
	// (Invoke) Token: 0x06000F3C RID: 3900
	public delegate void SwigDelegateGLAdapter_289(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x020001A1 RID: 417
	// (Invoke) Token: 0x06000F40 RID: 3904
	public delegate void SwigDelegateGLAdapter_290(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x020001A2 RID: 418
	// (Invoke) Token: 0x06000F44 RID: 3908
	public delegate void SwigDelegateGLAdapter_291(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x020001A3 RID: 419
	// (Invoke) Token: 0x06000F48 RID: 3912
	public delegate void SwigDelegateGLAdapter_292(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x020001A4 RID: 420
	// (Invoke) Token: 0x06000F4C RID: 3916
	public delegate void SwigDelegateGLAdapter_293(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x020001A5 RID: 421
	// (Invoke) Token: 0x06000F50 RID: 3920
	public delegate void SwigDelegateGLAdapter_294(uint program, uint location, int count, IntPtr pValuePtrData);

	// Token: 0x020001A6 RID: 422
	// (Invoke) Token: 0x06000F54 RID: 3924
	public delegate void SwigDelegateGLAdapter_295(uint program, uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x020001A7 RID: 423
	// (Invoke) Token: 0x06000F58 RID: 3928
	public delegate void SwigDelegateGLAdapter_296(uint program, uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x020001A8 RID: 424
	// (Invoke) Token: 0x06000F5C RID: 3932
	public delegate void SwigDelegateGLAdapter_297(uint program, uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x020001A9 RID: 425
	// (Invoke) Token: 0x06000F60 RID: 3936
	public delegate void SwigDelegateGLAdapter_298(uint program, uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x020001AA RID: 426
	// (Invoke) Token: 0x06000F64 RID: 3940
	public delegate void SwigDelegateGLAdapter_299(uint program, uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x020001AB RID: 427
	// (Invoke) Token: 0x06000F68 RID: 3944
	public delegate void SwigDelegateGLAdapter_300(uint program, uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x020001AC RID: 428
	// (Invoke) Token: 0x06000F6C RID: 3948
	public delegate void SwigDelegateGLAdapter_301(uint program, uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x020001AD RID: 429
	// (Invoke) Token: 0x06000F70 RID: 3952
	public delegate void SwigDelegateGLAdapter_302(uint program, uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x020001AE RID: 430
	// (Invoke) Token: 0x06000F74 RID: 3956
	public delegate void SwigDelegateGLAdapter_303(uint program, uint location, int count, int transpose, IntPtr pValuePtrData);

	// Token: 0x020001AF RID: 431
	// (Invoke) Token: 0x06000F78 RID: 3960
	public delegate void SwigDelegateGLAdapter_304(uint pipeline);

	// Token: 0x020001B0 RID: 432
	// (Invoke) Token: 0x06000F7C RID: 3964
	public delegate void SwigDelegateGLAdapter_305(uint pipeline, int bufSize, IntPtr pLengthPtrData, IntPtr pInfoLogPtrData);

	// Token: 0x020001B1 RID: 433
	// (Invoke) Token: 0x06000F80 RID: 3968
	public delegate void SwigDelegateGLAdapter_306(uint program, uint bufferIndex, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001B2 RID: 434
	// (Invoke) Token: 0x06000F84 RID: 3972
	public delegate void SwigDelegateGLAdapter_307(uint unit, uint texture, int level, int layered, int layer, uint access, uint format);

	// Token: 0x020001B3 RID: 435
	// (Invoke) Token: 0x06000F88 RID: 3976
	public delegate void SwigDelegateGLAdapter_308(uint barriers);

	// Token: 0x020001B4 RID: 436
	// (Invoke) Token: 0x06000F8C RID: 3980
	public delegate void SwigDelegateGLAdapter_309(uint barriers);

	// Token: 0x020001B5 RID: 437
	// (Invoke) Token: 0x06000F90 RID: 3984
	public delegate void SwigDelegateGLAdapter_310(uint target, int samples, uint internalformat, int width, int height, int fixedsamplelocations);

	// Token: 0x020001B6 RID: 438
	// (Invoke) Token: 0x06000F94 RID: 3988
	public delegate void SwigDelegateGLAdapter_311(uint target, int samples, uint internalformat, int width, int height, int depth, int fixedsamplelocations);

	// Token: 0x020001B7 RID: 439
	// (Invoke) Token: 0x06000F98 RID: 3992
	public delegate void SwigDelegateGLAdapter_312(uint pname, uint index, IntPtr pValPtrData);

	// Token: 0x020001B8 RID: 440
	// (Invoke) Token: 0x06000F9C RID: 3996
	public delegate void SwigDelegateGLAdapter_313(uint index, uint mask);

	// Token: 0x020001B9 RID: 441
	// (Invoke) Token: 0x06000FA0 RID: 4000
	public delegate void SwigDelegateGLAdapter_314(uint target, int level, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001BA RID: 442
	// (Invoke) Token: 0x06000FA4 RID: 4004
	public delegate void SwigDelegateGLAdapter_315(uint target, int level, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001BB RID: 443
	// (Invoke) Token: 0x06000FA8 RID: 4008
	public delegate void SwigDelegateGLAdapter_316(uint bindingindex, uint buffer, int offset, int stride);

	// Token: 0x020001BC RID: 444
	// (Invoke) Token: 0x06000FAC RID: 4012
	public delegate void SwigDelegateGLAdapter_317(uint attribindex, int size, uint type, int normalized, uint relativeoffset);

	// Token: 0x020001BD RID: 445
	// (Invoke) Token: 0x06000FB0 RID: 4016
	public delegate void SwigDelegateGLAdapter_318(uint attribindex, int size, uint type, uint relativeoffset);

	// Token: 0x020001BE RID: 446
	// (Invoke) Token: 0x06000FB4 RID: 4020
	public delegate void SwigDelegateGLAdapter_319(uint attribindex, uint bindingindex);

	// Token: 0x020001BF RID: 447
	// (Invoke) Token: 0x06000FB8 RID: 4024
	public delegate void SwigDelegateGLAdapter_320(uint bindingindex, uint divisor);

	// Token: 0x020001C0 RID: 448
	// (Invoke) Token: 0x06000FBC RID: 4028
	public delegate void SwigDelegateGLAdapter_321(uint pname, int value);

	// Token: 0x020001C1 RID: 449
	// (Invoke) Token: 0x06000FC0 RID: 4032
	public delegate void SwigDelegateGLAdapter_322(uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001C2 RID: 450
	// (Invoke) Token: 0x06000FC4 RID: 4036
	public delegate void SwigDelegateGLAdapter_323(uint op);

	// Token: 0x020001C3 RID: 451
	// (Invoke) Token: 0x06000FC8 RID: 4040
	public delegate void SwigDelegateGLAdapter_324(uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001C4 RID: 452
	// (Invoke) Token: 0x06000FCC RID: 4044
	public delegate void SwigDelegateGLAdapter_325(uint pname, uint usage, IntPtr pParamPtrData);

	// Token: 0x020001C5 RID: 453
	// (Invoke) Token: 0x06000FD0 RID: 4048
	public delegate void SwigDelegateGLAdapter_326(int maxcount, IntPtr pCountPtrData, IntPtr pBufPtrData);

	// Token: 0x020001C6 RID: 454
	// (Invoke) Token: 0x06000FD4 RID: 4052
	public delegate void SwigDelegateGLAdapter_327(int dest_x, int dest_y, int src_x, int src_y, int src_width, int src_height);

	// Token: 0x020001C7 RID: 455
	// (Invoke) Token: 0x06000FD8 RID: 4056
	public delegate void SwigDelegateGLAdapter_328(uint shader, int maxLength, IntPtr pLengthPtrData, IntPtr pDataPtrData);

	// Token: 0x020001C8 RID: 456
	// (Invoke) Token: 0x06000FDC RID: 4060
	public delegate void SwigDelegateGLAdapter_329(IntPtr pSamplersPtrData, int maxSamplers, IntPtr pNumSamplersPtrData);

	// Token: 0x020001C9 RID: 457
	// (Invoke) Token: 0x06000FE0 RID: 4064
	public delegate void SwigDelegateGLAdapter_330(uint p, IntPtr pEquationPtrData);

	// Token: 0x020001CA RID: 458
	// (Invoke) Token: 0x06000FE4 RID: 4068
	public delegate void SwigDelegateGLAdapter_331(uint target, uint attachment, uint textarget, uint texture, int level);

	// Token: 0x020001CB RID: 459
	// (Invoke) Token: 0x06000FE8 RID: 4072
	public delegate void SwigDelegateGLAdapter_332(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);

	// Token: 0x020001CC RID: 460
	// (Invoke) Token: 0x06000FEC RID: 4076
	public delegate void SwigDelegateGLAdapter_333(uint target, uint image);

	// Token: 0x020001CD RID: 461
	// (Invoke) Token: 0x06000FF0 RID: 4080
	public delegate void SwigDelegateGLAdapter_334(uint target, uint image);

	// Token: 0x020001CE RID: 462
	// (Invoke) Token: 0x06000FF4 RID: 4084
	public delegate void SwigDelegateGLAdapter_335(uint program, int bufSize, IntPtr pLengthPtrData, IntPtr pBinaryFormatPtrData, IntPtr pBinaryPtrData);

	// Token: 0x020001CF RID: 463
	// (Invoke) Token: 0x06000FF8 RID: 4088
	public delegate void SwigDelegateGLAdapter_336(uint program, uint binaryFormat, IntPtr pBinaryPtrData, int length);

	// Token: 0x020001D0 RID: 464
	// (Invoke) Token: 0x06000FFC RID: 4092
	public delegate void SwigDelegateGLAdapter_337(uint target, int level, uint internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pPixelsPtrData);

	// Token: 0x020001D1 RID: 465
	// (Invoke) Token: 0x06001000 RID: 4096
	public delegate void SwigDelegateGLAdapter_338(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pPixelsPtrData);

	// Token: 0x020001D2 RID: 466
	// (Invoke) Token: 0x06001004 RID: 4100
	public delegate void SwigDelegateGLAdapter_339(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);

	// Token: 0x020001D3 RID: 467
	// (Invoke) Token: 0x06001008 RID: 4104
	public delegate void SwigDelegateGLAdapter_340(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr pDataPtrData);

	// Token: 0x020001D4 RID: 468
	// (Invoke) Token: 0x0600100C RID: 4108
	public delegate void SwigDelegateGLAdapter_341(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr pDataPtrData);

	// Token: 0x020001D5 RID: 469
	// (Invoke) Token: 0x06001010 RID: 4112
	public delegate void SwigDelegateGLAdapter_342(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset);

	// Token: 0x020001D6 RID: 470
	// (Invoke) Token: 0x06001014 RID: 4116
	public delegate void SwigDelegateGLAdapter_343(uint array);

	// Token: 0x020001D7 RID: 471
	// (Invoke) Token: 0x06001018 RID: 4120
	public delegate void SwigDelegateGLAdapter_344(int n, IntPtr pArraysPtrData);

	// Token: 0x020001D8 RID: 472
	// (Invoke) Token: 0x0600101C RID: 4124
	public delegate void SwigDelegateGLAdapter_345(int n, IntPtr pArraysPtrData);

	// Token: 0x020001D9 RID: 473
	// (Invoke) Token: 0x06001020 RID: 4128
	public delegate void SwigDelegateGLAdapter_346(int returnVal, uint array);

	// Token: 0x020001DA RID: 474
	// (Invoke) Token: 0x06001024 RID: 4132
	public delegate void SwigDelegateGLAdapter_347(IntPtr pNumGroupsPtrData, int groupsSize, IntPtr pGroupsPtrData);

	// Token: 0x020001DB RID: 475
	// (Invoke) Token: 0x06001028 RID: 4136
	public delegate void SwigDelegateGLAdapter_348(uint group, IntPtr pNumCountersPtrData, IntPtr pMaxActiveCountersPtrData, int counterSize, IntPtr pCountersPtrData);

	// Token: 0x020001DC RID: 476
	// (Invoke) Token: 0x0600102C RID: 4140
	public delegate void SwigDelegateGLAdapter_349(uint group, int bufSize, IntPtr pLengthPtrData, IntPtr pGroupStringPtrData);

	// Token: 0x020001DD RID: 477
	// (Invoke) Token: 0x06001030 RID: 4144
	public delegate void SwigDelegateGLAdapter_350(uint group, uint counter, int bufSize, IntPtr pLengthPtrData, IntPtr pCounterStringPtrData);

	// Token: 0x020001DE RID: 478
	// (Invoke) Token: 0x06001034 RID: 4148
	public delegate void SwigDelegateGLAdapter_351(uint group, uint counter, uint pname, IntPtr pDataPtrData);

	// Token: 0x020001DF RID: 479
	// (Invoke) Token: 0x06001038 RID: 4152
	public delegate void SwigDelegateGLAdapter_352(int n, IntPtr pMonitorsPtrData);

	// Token: 0x020001E0 RID: 480
	// (Invoke) Token: 0x0600103C RID: 4156
	public delegate void SwigDelegateGLAdapter_353(int n, IntPtr pMonitorsPtrData);

	// Token: 0x020001E1 RID: 481
	// (Invoke) Token: 0x06001040 RID: 4160
	public delegate void SwigDelegateGLAdapter_354(uint monitor, int enable, uint group, int numCounters, IntPtr pCountersListPtrData);

	// Token: 0x020001E2 RID: 482
	// (Invoke) Token: 0x06001044 RID: 4164
	public delegate void SwigDelegateGLAdapter_355(uint monitor);

	// Token: 0x020001E3 RID: 483
	// (Invoke) Token: 0x06001048 RID: 4168
	public delegate void SwigDelegateGLAdapter_356(uint monitor);

	// Token: 0x020001E4 RID: 484
	// (Invoke) Token: 0x0600104C RID: 4172
	public delegate void SwigDelegateGLAdapter_357(uint monitor, uint pname, int dataSize, IntPtr pDataPtrData, IntPtr pBytesWrittenPtrData);

	// Token: 0x020001E5 RID: 485
	// (Invoke) Token: 0x06001050 RID: 4176
	public delegate void SwigDelegateGLAdapter_358(uint type, uint arg1, int length, IntPtr pLabelPtrData);

	// Token: 0x020001E6 RID: 486
	// (Invoke) Token: 0x06001054 RID: 4180
	public delegate void SwigDelegateGLAdapter_359(uint type, uint arg1, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData);

	// Token: 0x020001E7 RID: 487
	// (Invoke) Token: 0x06001058 RID: 4184
	public delegate void SwigDelegateGLAdapter_360(int length, IntPtr pMarkerPtrData);

	// Token: 0x020001E8 RID: 488
	// (Invoke) Token: 0x0600105C RID: 4188
	public delegate void SwigDelegateGLAdapter_361(int length, IntPtr pMarkerPtrData);

	// Token: 0x020001E9 RID: 489
	// (Invoke) Token: 0x06001060 RID: 4192
	public delegate void SwigDelegateGLAdapter_362();

	// Token: 0x020001EA RID: 490
	// (Invoke) Token: 0x06001064 RID: 4196
	public delegate void SwigDelegateGLAdapter_363(uint target, int numAttachments, IntPtr pAttachmentsPtrData);

	// Token: 0x020001EB RID: 491
	// (Invoke) Token: 0x06001068 RID: 4200
	public delegate void SwigDelegateGLAdapter_364(int n, IntPtr pIdsPtrData);

	// Token: 0x020001EC RID: 492
	// (Invoke) Token: 0x0600106C RID: 4204
	public delegate void SwigDelegateGLAdapter_365(int n, IntPtr pIdsPtrData);

	// Token: 0x020001ED RID: 493
	// (Invoke) Token: 0x06001070 RID: 4208
	public delegate void SwigDelegateGLAdapter_366(int returnVal, uint id);

	// Token: 0x020001EE RID: 494
	// (Invoke) Token: 0x06001074 RID: 4212
	public delegate void SwigDelegateGLAdapter_367(uint target, uint id);

	// Token: 0x020001EF RID: 495
	// (Invoke) Token: 0x06001078 RID: 4216
	public delegate void SwigDelegateGLAdapter_368(uint target);

	// Token: 0x020001F0 RID: 496
	// (Invoke) Token: 0x0600107C RID: 4220
	public delegate void SwigDelegateGLAdapter_369(uint id, uint target);

	// Token: 0x020001F1 RID: 497
	// (Invoke) Token: 0x06001080 RID: 4224
	public delegate void SwigDelegateGLAdapter_370(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001F2 RID: 498
	// (Invoke) Token: 0x06001084 RID: 4228
	public delegate void SwigDelegateGLAdapter_371(uint id, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001F3 RID: 499
	// (Invoke) Token: 0x06001088 RID: 4232
	public delegate void SwigDelegateGLAdapter_372(uint id, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001F4 RID: 500
	// (Invoke) Token: 0x0600108C RID: 4236
	public delegate void SwigDelegateGLAdapter_373(uint id, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001F5 RID: 501
	// (Invoke) Token: 0x06001090 RID: 4240
	public delegate void SwigDelegateGLAdapter_374(uint id, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001F6 RID: 502
	// (Invoke) Token: 0x06001094 RID: 4244
	public delegate void SwigDelegateGLAdapter_375(uint returnVal);

	// Token: 0x020001F7 RID: 503
	// (Invoke) Token: 0x06001098 RID: 4248
	public delegate void SwigDelegateGLAdapter_376(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr pDataPtrData);

	// Token: 0x020001F8 RID: 504
	// (Invoke) Token: 0x0600109C RID: 4252
	public delegate void SwigDelegateGLAdapter_377(uint program, uint location, int bufSize, IntPtr pParamsPtrData);

	// Token: 0x020001F9 RID: 505
	// (Invoke) Token: 0x060010A0 RID: 4256
	public delegate void SwigDelegateGLAdapter_378(uint program, uint location, int bufSize, IntPtr pParamsPtrData);

	// Token: 0x020001FA RID: 506
	// (Invoke) Token: 0x060010A4 RID: 4260
	public delegate void SwigDelegateGLAdapter_379(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001FB RID: 507
	// (Invoke) Token: 0x060010A8 RID: 4264
	public delegate void SwigDelegateGLAdapter_380(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001FC RID: 508
	// (Invoke) Token: 0x060010AC RID: 4268
	public delegate void SwigDelegateGLAdapter_381(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001FD RID: 509
	// (Invoke) Token: 0x060010B0 RID: 4272
	public delegate void SwigDelegateGLAdapter_382(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001FE RID: 510
	// (Invoke) Token: 0x060010B4 RID: 4276
	public delegate void SwigDelegateGLAdapter_383(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x020001FF RID: 511
	// (Invoke) Token: 0x060010B8 RID: 4280
	public delegate void SwigDelegateGLAdapter_384(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000200 RID: 512
	// (Invoke) Token: 0x060010BC RID: 4284
	public delegate void SwigDelegateGLAdapter_385(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000201 RID: 513
	// (Invoke) Token: 0x060010C0 RID: 4288
	public delegate void SwigDelegateGLAdapter_386(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000202 RID: 514
	// (Invoke) Token: 0x060010C4 RID: 4292
	public delegate void SwigDelegateGLAdapter_387(uint target, int samples, uint internalformat, int width, int height);

	// Token: 0x02000203 RID: 515
	// (Invoke) Token: 0x060010C8 RID: 4296
	public delegate void SwigDelegateGLAdapter_388(uint target, uint attachment, uint textarget, uint texture, int level, int samples);

	// Token: 0x02000204 RID: 516
	// (Invoke) Token: 0x060010CC RID: 4300
	public delegate void SwigDelegateGLAdapter_389(uint func, float arg1);

	// Token: 0x02000205 RID: 517
	// (Invoke) Token: 0x060010D0 RID: 4304
	public delegate void SwigDelegateGLAdapter_390(uint x, uint y, uint width, uint height, uint preserveMask);

	// Token: 0x02000206 RID: 518
	// (Invoke) Token: 0x060010D4 RID: 4308
	public delegate void SwigDelegateGLAdapter_391(uint preserveMask);

	// Token: 0x02000207 RID: 519
	// (Invoke) Token: 0x060010D8 RID: 4312
	public delegate void SwigDelegateGLAdapter_392(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth);

	// Token: 0x02000208 RID: 520
	// (Invoke) Token: 0x060010DC RID: 4316
	public delegate void SwigDelegateGLAdapter_393();

	// Token: 0x02000209 RID: 521
	// (Invoke) Token: 0x060010E0 RID: 4320
	public delegate void SwigDelegateGLAdapter_394(float value);

	// Token: 0x0200020A RID: 522
	// (Invoke) Token: 0x060010E4 RID: 4324
	public delegate void SwigDelegateGLAdapter_395(uint target, uint index);

	// Token: 0x0200020B RID: 523
	// (Invoke) Token: 0x060010E8 RID: 4328
	public delegate void SwigDelegateGLAdapter_396(uint target, uint index);

	// Token: 0x0200020C RID: 524
	// (Invoke) Token: 0x060010EC RID: 4332
	public delegate void SwigDelegateGLAdapter_397(uint buf, uint mode);

	// Token: 0x0200020D RID: 525
	// (Invoke) Token: 0x060010F0 RID: 4336
	public delegate void SwigDelegateGLAdapter_398(uint buf, uint modeRGB, uint modeAlpha);

	// Token: 0x0200020E RID: 526
	// (Invoke) Token: 0x060010F4 RID: 4340
	public delegate void SwigDelegateGLAdapter_399(uint buf, uint src, uint dst);

	// Token: 0x0200020F RID: 527
	// (Invoke) Token: 0x060010F8 RID: 4344
	public delegate void SwigDelegateGLAdapter_400(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);

	// Token: 0x02000210 RID: 528
	// (Invoke) Token: 0x060010FC RID: 4348
	public delegate void SwigDelegateGLAdapter_401(uint buf, int r, int g, int b, int a);

	// Token: 0x02000211 RID: 529
	// (Invoke) Token: 0x06001100 RID: 4352
	public delegate void SwigDelegateGLAdapter_402(int returnVal, uint target, uint index);

	// Token: 0x02000212 RID: 530
	// (Invoke) Token: 0x06001104 RID: 4356
	public delegate void SwigDelegateGLAdapter_403(uint target, uint internalFormat, uint buffer);

	// Token: 0x02000213 RID: 531
	// (Invoke) Token: 0x06001108 RID: 4360
	public delegate void SwigDelegateGLAdapter_404(uint target, uint internalFormat, uint buffer, int offset, int size);

	// Token: 0x02000214 RID: 532
	// (Invoke) Token: 0x0600110C RID: 4364
	public delegate void SwigDelegateGLAdapter_405(uint source, uint type, uint severity, int count, IntPtr pIdsPtrData, int enabled);

	// Token: 0x02000215 RID: 533
	// (Invoke) Token: 0x06001110 RID: 4368
	public delegate void SwigDelegateGLAdapter_406(uint source, uint type, uint id, uint severity, int length, IntPtr pBufPtrData);

	// Token: 0x02000216 RID: 534
	// (Invoke) Token: 0x06001114 RID: 4372
	public delegate void SwigDelegateGLAdapter_407(IntPtr pCallbackPtrData, IntPtr pUserParamPtrData);

	// Token: 0x02000217 RID: 535
	// (Invoke) Token: 0x06001118 RID: 4376
	public delegate void SwigDelegateGLAdapter_408(uint returnVal, uint count, int bufSize, IntPtr pSourcesPtrData, IntPtr pTypesPtrData, IntPtr pIdsPtrData, IntPtr pSeveritiesPtrData, IntPtr pLengthsPtrData, IntPtr pMessageLogPtrData);

	// Token: 0x02000218 RID: 536
	// (Invoke) Token: 0x0600111C RID: 4380
	public delegate void SwigDelegateGLAdapter_409(uint source, uint id, int length, IntPtr pMessagePtrData);

	// Token: 0x02000219 RID: 537
	// (Invoke) Token: 0x06001120 RID: 4384
	public delegate void SwigDelegateGLAdapter_410();

	// Token: 0x0200021A RID: 538
	// (Invoke) Token: 0x06001124 RID: 4388
	public delegate void SwigDelegateGLAdapter_411(uint identifier, uint name, int length, IntPtr pLabelPtrData);

	// Token: 0x0200021B RID: 539
	// (Invoke) Token: 0x06001128 RID: 4392
	public delegate void SwigDelegateGLAdapter_412(uint identifier, uint name, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData);

	// Token: 0x0200021C RID: 540
	// (Invoke) Token: 0x0600112C RID: 4396
	public delegate void SwigDelegateGLAdapter_413(uint ptr, int length, IntPtr pLabelPtrData);

	// Token: 0x0200021D RID: 541
	// (Invoke) Token: 0x06001130 RID: 4400
	public delegate void SwigDelegateGLAdapter_414(uint ptr, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData);

	// Token: 0x0200021E RID: 542
	// (Invoke) Token: 0x06001134 RID: 4404
	public delegate void SwigDelegateGLAdapter_415(uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200021F RID: 543
	// (Invoke) Token: 0x06001138 RID: 4408
	public delegate void SwigDelegateGLAdapter_416(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW);

	// Token: 0x02000220 RID: 544
	// (Invoke) Token: 0x0600113C RID: 4412
	public delegate void SwigDelegateGLAdapter_417(uint pname, int value);

	// Token: 0x02000221 RID: 545
	// (Invoke) Token: 0x06001140 RID: 4416
	public delegate void SwigDelegateGLAdapter_418(uint mode, int count, uint type, IntPtr pIndicesPtrData, int basevertex);

	// Token: 0x02000222 RID: 546
	// (Invoke) Token: 0x06001144 RID: 4420
	public delegate void SwigDelegateGLAdapter_419(uint mode, uint start, uint end, int count, uint type, IntPtr pIndicesPtrData, int basevertex);

	// Token: 0x02000223 RID: 547
	// (Invoke) Token: 0x06001148 RID: 4424
	public delegate void SwigDelegateGLAdapter_420(uint mode, int count, uint type, IntPtr pIndicesPtrData, int instanceCount, int basevertex);

	// Token: 0x02000224 RID: 548
	// (Invoke) Token: 0x0600114C RID: 4428
	public delegate void SwigDelegateGLAdapter_421(uint target, uint attachment, uint texture, int level);

	// Token: 0x02000225 RID: 549
	// (Invoke) Token: 0x06001150 RID: 4432
	public delegate void SwigDelegateGLAdapter_422(uint target, uint attachment, uint texture, int level, int baseViewIndex, int numViews);

	// Token: 0x02000226 RID: 550
	// (Invoke) Token: 0x06001154 RID: 4436
	public delegate void SwigDelegateGLAdapter_423(uint target, uint attachment, uint texture, int level, int samples, int baseView, int numViews);

	// Token: 0x02000227 RID: 551
	// (Invoke) Token: 0x06001158 RID: 4440
	public delegate void SwigDelegateGLAdapter_424(uint target, int size, IntPtr pDataPtrData, uint flags);

	// Token: 0x02000228 RID: 552
	// (Invoke) Token: 0x0600115C RID: 4444
	public delegate void SwigDelegateGLAdapter_425(uint returnVal);

	// Token: 0x02000229 RID: 553
	// (Invoke) Token: 0x06001160 RID: 4448
	public delegate void SwigDelegateGLAdapter_426(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr pDataPtrData);

	// Token: 0x0200022A RID: 554
	// (Invoke) Token: 0x06001164 RID: 4452
	public delegate void SwigDelegateGLAdapter_427(uint program, uint location, int bufSize, IntPtr pParamsPtrData);

	// Token: 0x0200022B RID: 555
	// (Invoke) Token: 0x06001168 RID: 4456
	public delegate void SwigDelegateGLAdapter_428(uint program, uint location, int bufSize, IntPtr pParamsPtrData);

	// Token: 0x0200022C RID: 556
	// (Invoke) Token: 0x0600116C RID: 4460
	public delegate void SwigDelegateGLAdapter_429(uint program, uint location, int bufSize, IntPtr pParamsPtrData);

	// Token: 0x0200022D RID: 557
	// (Invoke) Token: 0x06001170 RID: 4464
	public delegate void SwigDelegateGLAdapter_430(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200022E RID: 558
	// (Invoke) Token: 0x06001174 RID: 4468
	public delegate void SwigDelegateGLAdapter_431(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200022F RID: 559
	// (Invoke) Token: 0x06001178 RID: 4472
	public delegate void SwigDelegateGLAdapter_432(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000230 RID: 560
	// (Invoke) Token: 0x0600117C RID: 4476
	public delegate void SwigDelegateGLAdapter_433(uint target, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000231 RID: 561
	// (Invoke) Token: 0x06001180 RID: 4480
	public delegate void SwigDelegateGLAdapter_434(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000232 RID: 562
	// (Invoke) Token: 0x06001184 RID: 4484
	public delegate void SwigDelegateGLAdapter_435(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000233 RID: 563
	// (Invoke) Token: 0x06001188 RID: 4488
	public delegate void SwigDelegateGLAdapter_436(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000234 RID: 564
	// (Invoke) Token: 0x0600118C RID: 4492
	public delegate void SwigDelegateGLAdapter_437(uint sampler, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000235 RID: 565
	// (Invoke) Token: 0x06001190 RID: 4496
	public delegate void SwigDelegateGLAdapter_438(uint numBins, int separateBinningPass);

	// Token: 0x02000236 RID: 566
	// (Invoke) Token: 0x06001194 RID: 4500
	public delegate void SwigDelegateGLAdapter_439(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth);

	// Token: 0x02000237 RID: 567
	// (Invoke) Token: 0x06001198 RID: 4504
	public delegate void SwigDelegateGLAdapter_440();

	// Token: 0x02000238 RID: 568
	// (Invoke) Token: 0x0600119C RID: 4508
	public delegate void SwigDelegateGLAdapter_441(float value);

	// Token: 0x02000239 RID: 569
	// (Invoke) Token: 0x060011A0 RID: 4512
	public delegate void SwigDelegateGLAdapter_442(uint target, uint index);

	// Token: 0x0200023A RID: 570
	// (Invoke) Token: 0x060011A4 RID: 4516
	public delegate void SwigDelegateGLAdapter_443(uint target, uint index);

	// Token: 0x0200023B RID: 571
	// (Invoke) Token: 0x060011A8 RID: 4520
	public delegate void SwigDelegateGLAdapter_444(uint buf, uint mode);

	// Token: 0x0200023C RID: 572
	// (Invoke) Token: 0x060011AC RID: 4524
	public delegate void SwigDelegateGLAdapter_445(uint buf, uint modeRGB, uint modeAlpha);

	// Token: 0x0200023D RID: 573
	// (Invoke) Token: 0x060011B0 RID: 4528
	public delegate void SwigDelegateGLAdapter_446(uint buf, uint src, uint dst);

	// Token: 0x0200023E RID: 574
	// (Invoke) Token: 0x060011B4 RID: 4532
	public delegate void SwigDelegateGLAdapter_447(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);

	// Token: 0x0200023F RID: 575
	// (Invoke) Token: 0x060011B8 RID: 4536
	public delegate void SwigDelegateGLAdapter_448(uint buf, int r, int g, int b, int a);

	// Token: 0x02000240 RID: 576
	// (Invoke) Token: 0x060011BC RID: 4540
	public delegate void SwigDelegateGLAdapter_449(int returnVal, uint target, uint index);

	// Token: 0x02000241 RID: 577
	// (Invoke) Token: 0x060011C0 RID: 4544
	public delegate void SwigDelegateGLAdapter_450(uint target, uint internalFormat, uint buffer);

	// Token: 0x02000242 RID: 578
	// (Invoke) Token: 0x060011C4 RID: 4548
	public delegate void SwigDelegateGLAdapter_451(uint target, uint internalFormat, uint buffer, int offset, int size);

	// Token: 0x02000243 RID: 579
	// (Invoke) Token: 0x060011C8 RID: 4552
	public delegate void SwigDelegateGLAdapter_452(uint source, uint type, uint severity, int count, IntPtr pIdsPtrData, int enabled);

	// Token: 0x02000244 RID: 580
	// (Invoke) Token: 0x060011CC RID: 4556
	public delegate void SwigDelegateGLAdapter_453(uint source, uint type, uint id, uint severity, int length, IntPtr pBufPtrData);

	// Token: 0x02000245 RID: 581
	// (Invoke) Token: 0x060011D0 RID: 4560
	public delegate void SwigDelegateGLAdapter_454(IntPtr pCallbackPtrData, IntPtr pUserParamPtrData);

	// Token: 0x02000246 RID: 582
	// (Invoke) Token: 0x060011D4 RID: 4564
	public delegate void SwigDelegateGLAdapter_455(uint returnVal, uint count, int bufSize, IntPtr pSourcesPtrData, IntPtr pTypesPtrData, IntPtr pIdsPtrData, IntPtr pSeveritiesPtrData, IntPtr pLengthsPtrData, IntPtr pMessageLogPtrData);

	// Token: 0x02000247 RID: 583
	// (Invoke) Token: 0x060011D8 RID: 4568
	public delegate void SwigDelegateGLAdapter_456(uint source, uint id, int length, IntPtr pMessagePtrData);

	// Token: 0x02000248 RID: 584
	// (Invoke) Token: 0x060011DC RID: 4572
	public delegate void SwigDelegateGLAdapter_457();

	// Token: 0x02000249 RID: 585
	// (Invoke) Token: 0x060011E0 RID: 4576
	public delegate void SwigDelegateGLAdapter_458(uint identifier, uint name, int length, IntPtr pLabelPtrData);

	// Token: 0x0200024A RID: 586
	// (Invoke) Token: 0x060011E4 RID: 4580
	public delegate void SwigDelegateGLAdapter_459(uint identifier, uint name, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData);

	// Token: 0x0200024B RID: 587
	// (Invoke) Token: 0x060011E8 RID: 4584
	public delegate void SwigDelegateGLAdapter_460(uint ptr, int length, IntPtr pLabelPtrData);

	// Token: 0x0200024C RID: 588
	// (Invoke) Token: 0x060011EC RID: 4588
	public delegate void SwigDelegateGLAdapter_461(uint ptr, int bufSize, IntPtr pLengthPtrData, IntPtr pLabelPtrData);

	// Token: 0x0200024D RID: 589
	// (Invoke) Token: 0x060011F0 RID: 4592
	public delegate void SwigDelegateGLAdapter_462(uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200024E RID: 590
	// (Invoke) Token: 0x060011F4 RID: 4596
	public delegate void SwigDelegateGLAdapter_463(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW);

	// Token: 0x0200024F RID: 591
	// (Invoke) Token: 0x060011F8 RID: 4600
	public delegate void SwigDelegateGLAdapter_464(float red, float green, float blue, float alpha);

	// Token: 0x02000250 RID: 592
	// (Invoke) Token: 0x060011FC RID: 4604
	public delegate void SwigDelegateGLAdapter_465(uint modeRGB, uint modeAlpha);

	// Token: 0x02000251 RID: 593
	// (Invoke) Token: 0x06001200 RID: 4608
	public delegate void SwigDelegateGLAdapter_466(uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);

	// Token: 0x02000252 RID: 594
	// (Invoke) Token: 0x06001204 RID: 4612
	public delegate void SwigDelegateGLAdapter_467(uint rot);

	// Token: 0x02000253 RID: 595
	// (Invoke) Token: 0x06001208 RID: 4616
	public delegate void SwigDelegateGLAdapter_468(uint target, int sizeInBytes, int fd);

	// Token: 0x02000254 RID: 596
	// (Invoke) Token: 0x0600120C RID: 4620
	public delegate void SwigDelegateGLAdapter_469(int sizeInBytes, uint cacheMode, uint sharedList, IntPtr pOutFdPtrData);

	// Token: 0x02000255 RID: 597
	// (Invoke) Token: 0x06001210 RID: 4624
	public delegate void SwigDelegateGLAdapter_470(int fd);

	// Token: 0x02000256 RID: 598
	// (Invoke) Token: 0x06001214 RID: 4628
	public delegate void SwigDelegateGLAdapter_471();

	// Token: 0x02000257 RID: 599
	// (Invoke) Token: 0x06001218 RID: 4632
	public delegate void SwigDelegateGLAdapter_472(uint framebuffer, uint numLayers, uint focalPointsPerLayer, uint requestedFeatures, IntPtr pProvidedFeaturesPtrData);

	// Token: 0x02000258 RID: 600
	// (Invoke) Token: 0x0600121C RID: 4636
	public delegate void SwigDelegateGLAdapter_473(uint framebuffer, uint layer, uint focalPoint, float focalX, float focalY, float gainX, float gainY, float foveaArea);

	// Token: 0x02000259 RID: 601
	// (Invoke) Token: 0x06001220 RID: 4640
	public delegate void SwigDelegateGLAdapter_474(uint target, int offset, int size, uint clientBuffer, uint flags);

	// Token: 0x0200025A RID: 602
	// (Invoke) Token: 0x06001224 RID: 4644
	public delegate void SwigDelegateGLAdapter_475();

	// Token: 0x0200025B RID: 603
	// (Invoke) Token: 0x06001228 RID: 4648
	public delegate void SwigDelegateGLAdapter_476(int n, IntPtr pMemoryObjectsPtrData);

	// Token: 0x0200025C RID: 604
	// (Invoke) Token: 0x0600122C RID: 4652
	public delegate void SwigDelegateGLAdapter_477(int n, IntPtr pMemoryObjectsPtrData);

	// Token: 0x0200025D RID: 605
	// (Invoke) Token: 0x06001230 RID: 4656
	public delegate void SwigDelegateGLAdapter_478(int returnVal, uint memoryObject);

	// Token: 0x0200025E RID: 606
	// (Invoke) Token: 0x06001234 RID: 4660
	public delegate void SwigDelegateGLAdapter_479(uint memoryObject, uint pname, IntPtr pParamsPtrData);

	// Token: 0x0200025F RID: 607
	// (Invoke) Token: 0x06001238 RID: 4664
	public delegate void SwigDelegateGLAdapter_480(uint memoryObject, uint pname, IntPtr pParamsPtrData);

	// Token: 0x02000260 RID: 608
	// (Invoke) Token: 0x0600123C RID: 4668
	public delegate void SwigDelegateGLAdapter_481(uint target, int levels, uint internalFormat, int width, int height, uint memory, ulong offset);

	// Token: 0x02000261 RID: 609
	// (Invoke) Token: 0x06001240 RID: 4672
	public delegate void SwigDelegateGLAdapter_482(uint target, int samples, uint internalFormat, int width, int height, int fixedSampleLocations, uint memory, ulong offset);

	// Token: 0x02000262 RID: 610
	// (Invoke) Token: 0x06001244 RID: 4676
	public delegate void SwigDelegateGLAdapter_483(uint target, int levels, uint internalFormat, int width, int height, int depth, uint memory, ulong offset);

	// Token: 0x02000263 RID: 611
	// (Invoke) Token: 0x06001248 RID: 4680
	public delegate void SwigDelegateGLAdapter_484(uint target, int samples, uint internalFormat, int width, int height, int depth, int fixedSampleLocations, uint memory, ulong offset);

	// Token: 0x02000264 RID: 612
	// (Invoke) Token: 0x0600124C RID: 4684
	public delegate void SwigDelegateGLAdapter_485(uint target, int size, uint memory, ulong offset);

	// Token: 0x02000265 RID: 613
	// (Invoke) Token: 0x06001250 RID: 4688
	public delegate void SwigDelegateGLAdapter_486(int n, IntPtr pSemaphoresPtrData);

	// Token: 0x02000266 RID: 614
	// (Invoke) Token: 0x06001254 RID: 4692
	public delegate void SwigDelegateGLAdapter_487(int n, IntPtr pSemaphoresPtrData);

	// Token: 0x02000267 RID: 615
	// (Invoke) Token: 0x06001258 RID: 4696
	public delegate void SwigDelegateGLAdapter_488(int returnVal, uint semaphore);

	// Token: 0x02000268 RID: 616
	// (Invoke) Token: 0x0600125C RID: 4700
	public delegate void SwigDelegateGLAdapter_489(uint semaphore, IntPtr pSrcExternalUasgePtrData);

	// Token: 0x02000269 RID: 617
	// (Invoke) Token: 0x06001260 RID: 4704
	public delegate void SwigDelegateGLAdapter_490(IntPtr pDstExternalUsagePtrData, uint semaphore);

	// Token: 0x0200026A RID: 618
	// (Invoke) Token: 0x06001264 RID: 4708
	public delegate void SwigDelegateGLAdapter_491(uint memory, ulong size, uint handleType, int fd);

	// Token: 0x0200026B RID: 619
	// (Invoke) Token: 0x06001268 RID: 4712
	public delegate void SwigDelegateGLAdapter_492(uint semaphore, uint handleType, int fd);

	// Token: 0x0200026C RID: 620
	// (Invoke) Token: 0x0600126C RID: 4716
	public delegate void SwigDelegateGLAdapter_493(uint pname, IntPtr pDataPtrData);

	// Token: 0x0200026D RID: 621
	// (Invoke) Token: 0x06001270 RID: 4720
	public delegate void SwigDelegateGLAdapter_494(uint target, uint index, IntPtr pDataPtrData);

	// Token: 0x0200026E RID: 622
	// (Invoke) Token: 0x06001274 RID: 4724
	public delegate void SwigDelegateGLAdapter_495(uint texture, uint layer, uint focalPoint, float focalX, float focalY, float gainX, float gainY, float foveaArea);

	// Token: 0x0200026F RID: 623
	// (Invoke) Token: 0x06001278 RID: 4728
	public delegate void SwigDelegateGLAdapter_496(uint program, uint colorNumber, uint index, IntPtr pNamePtrData);

	// Token: 0x02000270 RID: 624
	// (Invoke) Token: 0x0600127C RID: 4732
	public delegate void SwigDelegateGLAdapter_497(uint program, uint color, IntPtr pNamePtrData);

	// Token: 0x02000271 RID: 625
	// (Invoke) Token: 0x06001280 RID: 4736
	public delegate void SwigDelegateGLAdapter_498(uint returnVal, uint program, uint programInterface, IntPtr pNamePtrData);

	// Token: 0x02000272 RID: 626
	// (Invoke) Token: 0x06001284 RID: 4740
	public delegate void SwigDelegateGLAdapter_499(uint returnVal, uint program, IntPtr pNamePtrData);

	// Token: 0x02000273 RID: 627
	// (Invoke) Token: 0x06001288 RID: 4744
	public delegate void SwigDelegateGLAdapter_500(uint rate);

	// Token: 0x02000274 RID: 628
	// (Invoke) Token: 0x0600128C RID: 4748
	public delegate void SwigDelegateGLAdapter_501(uint src1, uint src2, uint output, float scaleFactor);

	// Token: 0x02000275 RID: 629
	// (Invoke) Token: 0x06001290 RID: 4752
	public delegate void SwigDelegateGLAdapter_502(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers);

	// Token: 0x02000276 RID: 630
	// (Invoke) Token: 0x06001294 RID: 4756
	public delegate void SwigDelegateGLAdapter_503(uint arg0, uint target, uint output);

	// Token: 0x02000277 RID: 631
	// (Invoke) Token: 0x06001298 RID: 4760
	public delegate void SwigDelegateGLAdapter_504(uint arg0, uint target, uint output, uint mask);

	// Token: 0x02000278 RID: 632
	// (Invoke) Token: 0x0600129C RID: 4764
	public delegate void SwigDelegateGLAdapter_505(uint target, uint image, IntPtr pAttribListPtrData);

	// Token: 0x02000279 RID: 633
	// (Invoke) Token: 0x060012A0 RID: 4768
	public delegate void SwigDelegateGLAdapter_506(float factor, float units, float clamp);

	// Token: 0x0200027A RID: 634
	// (Invoke) Token: 0x060012A4 RID: 4772
	public delegate void SwigDelegateGLAdapter_507(int samples, int maxCount, IntPtr pCountPtrData, IntPtr pShadingRatesPtrData);

	// Token: 0x0200027B RID: 635
	// (Invoke) Token: 0x060012A8 RID: 4776
	public delegate void SwigDelegateGLAdapter_508(uint rate);

	// Token: 0x0200027C RID: 636
	// (Invoke) Token: 0x060012AC RID: 4780
	public delegate void SwigDelegateGLAdapter_509(uint combinerOp0, uint combinerOp1);

	// Token: 0x0200027D RID: 637
	// (Invoke) Token: 0x060012B0 RID: 4784
	public delegate void SwigDelegateGLAdapter_510(uint target, uint attachment, uint texture, int baseLayer, int numLayers, int texelWidth, int texelHeight);
}
