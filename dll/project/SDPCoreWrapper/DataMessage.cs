using System;
using System.Runtime.InteropServices;

// Token: 0x02000023 RID: 35
public class DataMessage : IDisposable
{
	// Token: 0x0600017B RID: 379 RVA: 0x00005408 File Offset: 0x00003608
	internal DataMessage(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00005424 File Offset: 0x00003624
	internal static HandleRef getCPtr(DataMessage obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000543C File Offset: 0x0000363C
	~DataMessage()
	{
		this.Dispose();
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00005468 File Offset: 0x00003668
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DataMessage(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x000054E8 File Offset: 0x000036E8
	public DataMessage(uint p, uint t, uint m)
		: this(SDPCorePINVOKE.new_DataMessage__SWIG_0(p, t, m), true)
	{
	}

	// Token: 0x06000180 RID: 384 RVA: 0x000054F9 File Offset: 0x000036F9
	public DataMessage(uint p, uint t, uint c, uint m)
		: this(SDPCorePINVOKE.new_DataMessage__SWIG_1(p, t, c, m), true)
	{
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000550C File Offset: 0x0000370C
	public uint GetCommandType()
	{
		return SDPCorePINVOKE.DataMessage_GetCommandType(this.swigCPtr);
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000183 RID: 387 RVA: 0x00005534 File Offset: 0x00003734
	// (set) Token: 0x06000182 RID: 386 RVA: 0x00005526 File Offset: 0x00003726
	public uint pid
	{
		get
		{
			return SDPCorePINVOKE.DataMessage_pid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.DataMessage_pid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000185 RID: 389 RVA: 0x0000555C File Offset: 0x0000375C
	// (set) Token: 0x06000184 RID: 388 RVA: 0x0000554E File Offset: 0x0000374E
	public uint tid
	{
		get
		{
			return SDPCorePINVOKE.DataMessage_tid_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.DataMessage_tid_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000187 RID: 391 RVA: 0x00005584 File Offset: 0x00003784
	// (set) Token: 0x06000186 RID: 390 RVA: 0x00005576 File Offset: 0x00003776
	public uint metricID
	{
		get
		{
			return SDPCorePINVOKE.DataMessage_metricID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.DataMessage_metricID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000189 RID: 393 RVA: 0x000055AC File Offset: 0x000037AC
	// (set) Token: 0x06000188 RID: 392 RVA: 0x0000559E File Offset: 0x0000379E
	public uint captureID
	{
		get
		{
			return SDPCorePINVOKE.DataMessage_captureID_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.DataMessage_captureID_set(this.swigCPtr, value);
		}
	}

	// Token: 0x04000060 RID: 96
	private HandleRef swigCPtr;

	// Token: 0x04000061 RID: 97
	protected bool swigCMemOwn;
}
