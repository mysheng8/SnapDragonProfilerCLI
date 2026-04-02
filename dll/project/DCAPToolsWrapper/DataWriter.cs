using System;
using System.Runtime.InteropServices;

// Token: 0x02000009 RID: 9
public class DataWriter : IDisposable
{
	// Token: 0x06000047 RID: 71 RVA: 0x000027B0 File Offset: 0x000009B0
	internal DataWriter(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwnBase = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x000027CC File Offset: 0x000009CC
	internal static HandleRef getCPtr(DataWriter obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000027E4 File Offset: 0x000009E4
	~DataWriter()
	{
		this.Dispose();
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00002810 File Offset: 0x00000A10
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwnBase)
				{
					this.swigCMemOwnBase = false;
					libDCAPPINVOKE.delete_DataWriter(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00002890 File Offset: 0x00000A90
	public virtual bool IsValid()
	{
		bool flag = libDCAPPINVOKE.DataWriter_IsValid(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000028AA File Offset: 0x00000AAA
	public virtual DCAPStatus Open()
	{
		DCAPStatus dcapstatus = (DCAPStatus)libDCAPPINVOKE.DataWriter_Open(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return dcapstatus;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000028C4 File Offset: 0x00000AC4
	public virtual uint Write(IntPtr data, uint len)
	{
		uint num = libDCAPPINVOKE.DataWriter_Write(this.swigCPtr, data, len);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000028E0 File Offset: 0x00000AE0
	public virtual uint WriteAt(IntPtr data, uint len, ulong pos)
	{
		uint num = libDCAPPINVOKE.DataWriter_WriteAt(this.swigCPtr, data, len, pos);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000028FD File Offset: 0x00000AFD
	public virtual void Flush()
	{
		libDCAPPINVOKE.DataWriter_Flush(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00002917 File Offset: 0x00000B17
	public virtual void Close()
	{
		libDCAPPINVOKE.DataWriter_Close(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00002931 File Offset: 0x00000B31
	public virtual DCAPStatus Seek(DataWriter.SeekFrom seekType, ulong offset)
	{
		DCAPStatus dcapstatus = (DCAPStatus)libDCAPPINVOKE.DataWriter_Seek(this.swigCPtr, (int)seekType, offset);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return dcapstatus;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0000294D File Offset: 0x00000B4D
	public virtual ulong Tell()
	{
		ulong num = libDCAPPINVOKE.DataWriter_Tell(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00002967 File Offset: 0x00000B67
	public virtual string GetName()
	{
		string text = libDCAPPINVOKE.DataWriter_GetName(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x040004E4 RID: 1252
	private HandleRef swigCPtr;

	// Token: 0x040004E5 RID: 1253
	private bool swigCMemOwnBase;

	// Token: 0x0200003C RID: 60
	public enum SeekFrom
	{
		// Token: 0x04000BAE RID: 2990
		SeekFromStart,
		// Token: 0x04000BAF RID: 2991
		SeekFromCurrent,
		// Token: 0x04000BB0 RID: 2992
		SeekFromEnd
	}
}
