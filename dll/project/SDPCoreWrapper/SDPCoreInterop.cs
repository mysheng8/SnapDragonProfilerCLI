using System;
using System.Runtime.InteropServices;

// Token: 0x0200008C RID: 140
public class SDPCoreInterop : IDisposable
{
	// Token: 0x06000A05 RID: 2565 RVA: 0x00016FE5 File Offset: 0x000151E5
	internal SDPCoreInterop(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x00017001 File Offset: 0x00015201
	internal static HandleRef getCPtr(SDPCoreInterop obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x00017018 File Offset: 0x00015218
	~SDPCoreInterop()
	{
		this.Dispose();
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x00017044 File Offset: 0x00015244
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_SDPCoreInterop(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x000170C4 File Offset: 0x000152C4
	public static Metric GetMetricFromPointer(IntPtr m)
	{
		IntPtr intPtr = SDPCorePINVOKE.SDPCoreInterop_GetMetricFromPointer(m);
		return (intPtr == IntPtr.Zero) ? null : new Metric(intPtr, false);
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x000170F4 File Offset: 0x000152F4
	public static CaptureStream GetFromPointer(IntPtr stream)
	{
		IntPtr intPtr = SDPCorePINVOKE.SDPCoreInterop_GetFromPointer(stream);
		return (intPtr == IntPtr.Zero) ? null : new CaptureStream(intPtr, false);
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00017124 File Offset: 0x00015324
	public static string GetStringFromCustomData(IntPtr val, uint index)
	{
		return SDPCorePINVOKE.SDPCoreInterop_GetStringFromCustomData__SWIG_0(val, index);
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0001713C File Offset: 0x0001533C
	public static uint GetUInt32FromCustomData(IntPtr val, uint index)
	{
		return SDPCorePINVOKE.SDPCoreInterop_GetUInt32FromCustomData(val, index);
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x00017154 File Offset: 0x00015354
	public static int GetInt32FromCustomData(IntPtr val, uint index)
	{
		return SDPCorePINVOKE.SDPCoreInterop_GetInt32FromCustomData(val, index);
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x0001716C File Offset: 0x0001536C
	public static string GetStringFromCustomData(IntPtr val, string name)
	{
		string text = SDPCorePINVOKE.SDPCoreInterop_GetStringFromCustomData__SWIG_1(val, name);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x00017190 File Offset: 0x00015390
	public static IntPtr ConvertUInt8PtrToPtr(SWIGTYPE_p_uint8_t ptr)
	{
		return SDPCorePINVOKE.SDPCoreInterop_ConvertUInt8PtrToPtr(SWIGTYPE_p_uint8_t.getCPtr(ptr));
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x000171AC File Offset: 0x000153AC
	public static double ConvertPtrToDoubleValue(IntPtr ptr)
	{
		return SDPCorePINVOKE.SDPCoreInterop_ConvertPtrToDoubleValue(ptr);
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x000171C4 File Offset: 0x000153C4
	public static uint GetIconSize()
	{
		return SDPCorePINVOKE.SDPCoreInterop_GetIconSize();
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x000171D8 File Offset: 0x000153D8
	public static uint GetIconWidth()
	{
		return SDPCorePINVOKE.SDPCoreInterop_GetIconWidth();
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x000171EC File Offset: 0x000153EC
	public static uint GetIconHeight()
	{
		return SDPCorePINVOKE.SDPCoreInterop_GetIconHeight();
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x00017200 File Offset: 0x00015400
	public SDPCoreInterop()
		: this(SDPCorePINVOKE.new_SDPCoreInterop(), true)
	{
	}

	// Token: 0x040001A2 RID: 418
	private HandleRef swigCPtr;

	// Token: 0x040001A3 RID: 419
	protected bool swigCMemOwn;
}
