using System;
using System.Runtime.InteropServices;

// Token: 0x0200001F RID: 31
public class CommandMsg : IDisposable
{
	// Token: 0x06000151 RID: 337 RVA: 0x00004DEE File Offset: 0x00002FEE
	internal CommandMsg(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00004E0A File Offset: 0x0000300A
	internal static HandleRef getCPtr(CommandMsg obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00004E24 File Offset: 0x00003024
	~CommandMsg()
	{
		this.Dispose();
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00004E50 File Offset: 0x00003050
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_CommandMsg(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00004ED0 File Offset: 0x000030D0
	public CommandMsg(uint msgType)
		: this(SDPCorePINVOKE.new_CommandMsg(msgType), true)
	{
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00004EE0 File Offset: 0x000030E0
	public uint GetCommandType()
	{
		return SDPCorePINVOKE.CommandMsg_GetCommandType(this.swigCPtr);
	}

	// Token: 0x04000058 RID: 88
	private HandleRef swigCPtr;

	// Token: 0x04000059 RID: 89
	protected bool swigCMemOwn;
}
