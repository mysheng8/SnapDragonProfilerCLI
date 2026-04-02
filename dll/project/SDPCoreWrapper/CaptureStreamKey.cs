using System;
using System.Runtime.InteropServices;

// Token: 0x0200001C RID: 28
public class CaptureStreamKey : IDisposable
{
	// Token: 0x060000DC RID: 220 RVA: 0x000037E6 File Offset: 0x000019E6
	internal CaptureStreamKey(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00003802 File Offset: 0x00001A02
	internal static HandleRef getCPtr(CaptureStreamKey obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000381C File Offset: 0x00001A1C
	~CaptureStreamKey()
	{
		this.Dispose();
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00003848 File Offset: 0x00001A48
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CaptureStreamKey(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000038C8 File Offset: 0x00001AC8
	public CaptureStreamKey(uint process, uint thread)
		: this(SDPCorePINVOKE.new_CaptureStreamKey(process, thread), true)
	{
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x000038D8 File Offset: 0x00001AD8
	public bool LessThan(CaptureStreamKey key)
	{
		bool flag = SDPCorePINVOKE.CaptureStreamKey_LessThan(this.swigCPtr, CaptureStreamKey.getCPtr(key));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003914 File Offset: 0x00001B14
	// (set) Token: 0x060000E2 RID: 226 RVA: 0x00003905 File Offset: 0x00001B05
	public uint m_process
	{
		get
		{
			return SDPCorePINVOKE.CaptureStreamKey_m_process_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureStreamKey_m_process_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000393C File Offset: 0x00001B3C
	// (set) Token: 0x060000E4 RID: 228 RVA: 0x0000392E File Offset: 0x00001B2E
	public uint m_thread
	{
		get
		{
			return SDPCorePINVOKE.CaptureStreamKey_m_thread_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.CaptureStreamKey_m_thread_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04000023 RID: 35
	private HandleRef swigCPtr;

	// Token: 0x04000024 RID: 36
	protected bool swigCMemOwn;
}
