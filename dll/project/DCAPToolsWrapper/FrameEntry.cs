using System;
using System.Runtime.InteropServices;

// Token: 0x02000013 RID: 19
public class FrameEntry : IDisposable
{
	// Token: 0x06000142 RID: 322 RVA: 0x00005D84 File Offset: 0x00003F84
	internal FrameEntry(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00005DA0 File Offset: 0x00003FA0
	internal static HandleRef getCPtr(FrameEntry obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00005DB8 File Offset: 0x00003FB8
	~FrameEntry()
	{
		this.Dispose();
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00005DE4 File Offset: 0x00003FE4
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					libDCAPPINVOKE.delete_FrameEntry(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000147 RID: 327 RVA: 0x00005E72 File Offset: 0x00004072
	// (set) Token: 0x06000146 RID: 326 RVA: 0x00005E64 File Offset: 0x00004064
	public uint number
	{
		get
		{
			return libDCAPPINVOKE.FrameEntry_number_get(this.swigCPtr);
		}
		set
		{
			libDCAPPINVOKE.FrameEntry_number_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000149 RID: 329 RVA: 0x00005E8D File Offset: 0x0000408D
	// (set) Token: 0x06000148 RID: 328 RVA: 0x00005E7F File Offset: 0x0000407F
	public ulong position
	{
		get
		{
			return libDCAPPINVOKE.FrameEntry_position_get(this.swigCPtr);
		}
		set
		{
			libDCAPPINVOKE.FrameEntry_position_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00005E9A File Offset: 0x0000409A
	public FrameEntry()
		: this(libDCAPPINVOKE.new_FrameEntry(), true)
	{
	}

	// Token: 0x04000592 RID: 1426
	private HandleRef swigCPtr;

	// Token: 0x04000593 RID: 1427
	protected bool swigCMemOwn;
}
