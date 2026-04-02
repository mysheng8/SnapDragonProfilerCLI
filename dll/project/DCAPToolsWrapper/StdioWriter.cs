using System;
using System.Runtime.InteropServices;

// Token: 0x02000022 RID: 34
public class StdioWriter : DataWriter
{
	// Token: 0x06000965 RID: 2405 RVA: 0x00019B54 File Offset: 0x00017D54
	internal StdioWriter(IntPtr cPtr, bool cMemoryOwn)
		: base(libDCAPPINVOKE.StdioWriter_SWIGSmartPtrUpcast(cPtr), true)
	{
		this.swigCMemOwnDerived = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00019B77 File Offset: 0x00017D77
	internal static HandleRef getCPtr(StdioWriter obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00019B90 File Offset: 0x00017D90
	~StdioWriter()
	{
		this.Dispose();
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00019BBC File Offset: 0x00017DBC
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwnDerived)
				{
					this.swigCMemOwnDerived = false;
					libDCAPPINVOKE.delete_StdioWriter(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00019C40 File Offset: 0x00017E40
	public StdioWriter(string filename, bool replaceExisting, bool asText)
		: this(libDCAPPINVOKE.new_StdioWriter__SWIG_0(filename, replaceExisting, asText), true)
	{
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00019C5E File Offset: 0x00017E5E
	public StdioWriter(string filename, bool replaceExisting)
		: this(libDCAPPINVOKE.new_StdioWriter__SWIG_1(filename, replaceExisting), true)
	{
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x00019C7B File Offset: 0x00017E7B
	public StdioWriter(string filename)
		: this(libDCAPPINVOKE.new_StdioWriter__SWIG_2(filename), true)
	{
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x00019C97 File Offset: 0x00017E97
	public override bool IsValid()
	{
		bool flag = libDCAPPINVOKE.StdioWriter_IsValid(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00019CB1 File Offset: 0x00017EB1
	public override DCAPStatus Open()
	{
		DCAPStatus dcapstatus = (DCAPStatus)libDCAPPINVOKE.StdioWriter_Open(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return dcapstatus;
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00019CCB File Offset: 0x00017ECB
	public override uint Write(IntPtr data, uint len)
	{
		uint num = libDCAPPINVOKE.StdioWriter_Write(this.swigCPtr, data, len);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00019CE7 File Offset: 0x00017EE7
	public override uint WriteAt(IntPtr data, uint len, ulong pos)
	{
		uint num = libDCAPPINVOKE.StdioWriter_WriteAt(this.swigCPtr, data, len, pos);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00019D04 File Offset: 0x00017F04
	public override void Flush()
	{
		libDCAPPINVOKE.StdioWriter_Flush(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x00019D1E File Offset: 0x00017F1E
	public override void Close()
	{
		libDCAPPINVOKE.StdioWriter_Close(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00019D38 File Offset: 0x00017F38
	public override DCAPStatus Seek(DataWriter.SeekFrom seekType, ulong offset)
	{
		DCAPStatus dcapstatus = (DCAPStatus)libDCAPPINVOKE.StdioWriter_Seek(this.swigCPtr, (int)seekType, offset);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return dcapstatus;
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00019D54 File Offset: 0x00017F54
	public override ulong Tell()
	{
		ulong num = libDCAPPINVOKE.StdioWriter_Tell(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00019D6E File Offset: 0x00017F6E
	public override string GetName()
	{
		string text = libDCAPPINVOKE.StdioWriter_GetName(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x04000B46 RID: 2886
	private HandleRef swigCPtr;

	// Token: 0x04000B47 RID: 2887
	private bool swigCMemOwnDerived;
}
