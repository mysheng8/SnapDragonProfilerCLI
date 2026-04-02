using System;
using System.Runtime.InteropServices;

// Token: 0x02000021 RID: 33
public class StdioReader : DataReader
{
	// Token: 0x06000958 RID: 2392 RVA: 0x00019978 File Offset: 0x00017B78
	internal StdioReader(IntPtr cPtr, bool cMemoryOwn)
		: base(libDCAPPINVOKE.StdioReader_SWIGSmartPtrUpcast(cPtr), true)
	{
		this.swigCMemOwnDerived = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0001999B File Offset: 0x00017B9B
	internal static HandleRef getCPtr(StdioReader obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x000199B4 File Offset: 0x00017BB4
	~StdioReader()
	{
		this.Dispose();
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x000199E0 File Offset: 0x00017BE0
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwnDerived)
				{
					this.swigCMemOwnDerived = false;
					libDCAPPINVOKE.delete_StdioReader(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x00019A64 File Offset: 0x00017C64
	public StdioReader(string filename)
		: this(libDCAPPINVOKE.new_StdioReader(filename), true)
	{
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x00019A80 File Offset: 0x00017C80
	public override bool IsValid()
	{
		bool flag = libDCAPPINVOKE.StdioReader_IsValid(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00019A9A File Offset: 0x00017C9A
	public override DCAPStatus Open()
	{
		DCAPStatus dcapstatus = (DCAPStatus)libDCAPPINVOKE.StdioReader_Open(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return dcapstatus;
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x00019AB4 File Offset: 0x00017CB4
	public override uint Read(IntPtr pData, uint len)
	{
		uint num = libDCAPPINVOKE.StdioReader_Read(this.swigCPtr, pData, len);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00019AD0 File Offset: 0x00017CD0
	public override void Close()
	{
		libDCAPPINVOKE.StdioReader_Close(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00019AEA File Offset: 0x00017CEA
	public override bool AtEof()
	{
		bool flag = libDCAPPINVOKE.StdioReader_AtEof(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00019B04 File Offset: 0x00017D04
	public override DCAPStatus Seek(DataReader.SeekFrom seekType, ulong offset)
	{
		DCAPStatus dcapstatus = (DCAPStatus)libDCAPPINVOKE.StdioReader_Seek(this.swigCPtr, (int)seekType, offset);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return dcapstatus;
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00019B20 File Offset: 0x00017D20
	public override ulong Tell()
	{
		ulong num = libDCAPPINVOKE.StdioReader_Tell(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00019B3A File Offset: 0x00017D3A
	public override string GetName()
	{
		string text = libDCAPPINVOKE.StdioReader_GetName(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x04000B44 RID: 2884
	private HandleRef swigCPtr;

	// Token: 0x04000B45 RID: 2885
	private bool swigCMemOwnDerived;
}
