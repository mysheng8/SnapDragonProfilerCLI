using System;
using System.Runtime.InteropServices;

// Token: 0x02000008 RID: 8
public class DataReader : IDisposable
{
	// Token: 0x0600003B RID: 59 RVA: 0x000025FB File Offset: 0x000007FB
	internal DataReader(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwnBase = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002617 File Offset: 0x00000817
	internal static HandleRef getCPtr(DataReader obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002630 File Offset: 0x00000830
	~DataReader()
	{
		this.Dispose();
	}

	// Token: 0x0600003E RID: 62 RVA: 0x0000265C File Offset: 0x0000085C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwnBase)
				{
					this.swigCMemOwnBase = false;
					libDCAPPINVOKE.delete_DataReader(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x000026DC File Offset: 0x000008DC
	public virtual bool IsValid()
	{
		bool flag = libDCAPPINVOKE.DataReader_IsValid(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000026F6 File Offset: 0x000008F6
	public virtual DCAPStatus Open()
	{
		DCAPStatus dcapstatus = (DCAPStatus)libDCAPPINVOKE.DataReader_Open(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return dcapstatus;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00002710 File Offset: 0x00000910
	public virtual uint Read(IntPtr pData, uint len)
	{
		uint num = libDCAPPINVOKE.DataReader_Read(this.swigCPtr, pData, len);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000272C File Offset: 0x0000092C
	public virtual void Close()
	{
		libDCAPPINVOKE.DataReader_Close(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00002746 File Offset: 0x00000946
	public virtual bool AtEof()
	{
		bool flag = libDCAPPINVOKE.DataReader_AtEof(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00002760 File Offset: 0x00000960
	public virtual DCAPStatus Seek(DataReader.SeekFrom seekType, ulong offset)
	{
		DCAPStatus dcapstatus = (DCAPStatus)libDCAPPINVOKE.DataReader_Seek(this.swigCPtr, (int)seekType, offset);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return dcapstatus;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0000277C File Offset: 0x0000097C
	public virtual ulong Tell()
	{
		ulong num = libDCAPPINVOKE.DataReader_Tell(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return num;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00002796 File Offset: 0x00000996
	public virtual string GetName()
	{
		string text = libDCAPPINVOKE.DataReader_GetName(this.swigCPtr);
		if (libDCAPPINVOKE.SWIGPendingException.Pending)
		{
			throw libDCAPPINVOKE.SWIGPendingException.Retrieve();
		}
		return text;
	}

	// Token: 0x040004E2 RID: 1250
	private HandleRef swigCPtr;

	// Token: 0x040004E3 RID: 1251
	private bool swigCMemOwnBase;

	// Token: 0x0200003B RID: 59
	public enum SeekFrom
	{
		// Token: 0x04000BAA RID: 2986
		SeekFromStart,
		// Token: 0x04000BAB RID: 2987
		SeekFromCurrent,
		// Token: 0x04000BAC RID: 2988
		SeekFromEnd
	}
}
