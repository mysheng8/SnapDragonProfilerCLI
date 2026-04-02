using System;
using System.Runtime.InteropServices;

// Token: 0x0200001F RID: 31
public class PointerData : IDisposable
{
	// Token: 0x0600093A RID: 2362 RVA: 0x0001971D File Offset: 0x0001791D
	internal PointerData(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00019739 File Offset: 0x00017939
	internal static HandleRef getCPtr(PointerData obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00019750 File Offset: 0x00017950
	~PointerData()
	{
		this.Dispose();
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0001977C File Offset: 0x0001797C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_PointerData(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x000197FC File Offset: 0x000179FC
	public PointerData()
		: this(libDCAPPINVOKE.new_PointerData(), true)
	{
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0001980A File Offset: 0x00017A0A
	public bool IsInitialized()
	{
		return libDCAPPINVOKE.PointerData_IsInitialized(this.swigCPtr);
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x00019817 File Offset: 0x00017A17
	public bool IsNull()
	{
		return libDCAPPINVOKE.PointerData_IsNull(this.swigCPtr);
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00019824 File Offset: 0x00017A24
	public bool HaveValue()
	{
		return libDCAPPINVOKE.PointerData_HaveValue(this.swigCPtr);
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x00019831 File Offset: 0x00017A31
	public bool HaveArray()
	{
		return libDCAPPINVOKE.PointerData_HaveArray(this.swigCPtr);
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0001983E File Offset: 0x00017A3E
	public bool HaveString()
	{
		return libDCAPPINVOKE.PointerData_HaveString(this.swigCPtr);
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0001984B File Offset: 0x00017A4B
	public bool HaveStringArray()
	{
		return libDCAPPINVOKE.PointerData_HaveStringArray(this.swigCPtr);
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00019858 File Offset: 0x00017A58
	public bool HaveAddress()
	{
		return libDCAPPINVOKE.PointerData_HaveAddress(this.swigCPtr);
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x00019865 File Offset: 0x00017A65
	public bool HaveData()
	{
		return libDCAPPINVOKE.PointerData_HaveData(this.swigCPtr);
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00019872 File Offset: 0x00017A72
	public bool HaveDataStripped()
	{
		return libDCAPPINVOKE.PointerData_HaveDataStripped(this.swigCPtr);
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0001987F File Offset: 0x00017A7F
	public uint GetMask()
	{
		return libDCAPPINVOKE.PointerData_GetMask(this.swigCPtr);
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0001988C File Offset: 0x00017A8C
	public ulong GetAddress()
	{
		return libDCAPPINVOKE.PointerData_GetAddress(this.swigCPtr);
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x00019899 File Offset: 0x00017A99
	public uint GetElementSize()
	{
		return libDCAPPINVOKE.PointerData_GetElementSize(this.swigCPtr);
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x000198A6 File Offset: 0x00017AA6
	public uint GetCount()
	{
		return libDCAPPINVOKE.PointerData_GetCount(this.swigCPtr);
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x000198B3 File Offset: 0x00017AB3
	public uint GetByteSize()
	{
		return libDCAPPINVOKE.PointerData_GetByteSize(this.swigCPtr);
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x000198C0 File Offset: 0x00017AC0
	public IntPtr GetValueData()
	{
		return libDCAPPINVOKE.PointerData_GetValueData(this.swigCPtr);
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x000198CD File Offset: 0x00017ACD
	public IntPtr GetArrayData()
	{
		return libDCAPPINVOKE.PointerData_GetArrayData(this.swigCPtr);
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x000198DA File Offset: 0x00017ADA
	public IntPtr GetStringData()
	{
		return libDCAPPINVOKE.PointerData_GetStringData(this.swigCPtr);
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x000198E7 File Offset: 0x00017AE7
	public IntPtr GetStringMasks()
	{
		return libDCAPPINVOKE.PointerData_GetStringMasks(this.swigCPtr);
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x000198F4 File Offset: 0x00017AF4
	public SWIGTYPE_p_unsigned_long_long GetStringAddresses()
	{
		IntPtr intPtr = libDCAPPINVOKE.PointerData_GetStringAddresses(this.swigCPtr);
		if (!(intPtr == IntPtr.Zero))
		{
			return new SWIGTYPE_p_unsigned_long_long(intPtr, false);
		}
		return null;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x00019923 File Offset: 0x00017B23
	public IntPtr GetStringLengths()
	{
		return libDCAPPINVOKE.PointerData_GetStringLengths(this.swigCPtr);
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x00019930 File Offset: 0x00017B30
	public IntPtr GetStringArrayData()
	{
		return libDCAPPINVOKE.PointerData_GetStringArrayData(this.swigCPtr);
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0001993D File Offset: 0x00017B3D
	public IntPtr GetPointer()
	{
		return libDCAPPINVOKE.PointerData_GetPointer(this.swigCPtr);
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0001994A File Offset: 0x00017B4A
	public uint Decode(IntPtr pBuffer, uint bufferSize, uint bufferOffset, uint elementSize)
	{
		return libDCAPPINVOKE.PointerData_Decode(this.swigCPtr, pBuffer, bufferSize, bufferOffset, elementSize);
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0001995C File Offset: 0x00017B5C
	public DCAPStatus ResizeArray(uint count)
	{
		return (DCAPStatus)libDCAPPINVOKE.PointerData_ResizeArray(this.swigCPtr, count);
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0001996A File Offset: 0x00017B6A
	public DCAPStatus ResizeString(uint count)
	{
		return (DCAPStatus)libDCAPPINVOKE.PointerData_ResizeString(this.swigCPtr, count);
	}

	// Token: 0x040009F8 RID: 2552
	private HandleRef swigCPtr;

	// Token: 0x040009F9 RID: 2553
	protected bool swigCMemOwn;
}
