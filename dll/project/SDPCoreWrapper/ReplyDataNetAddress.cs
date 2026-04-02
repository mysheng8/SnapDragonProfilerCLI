using System;
using System.Runtime.InteropServices;

// Token: 0x0200006C RID: 108
public class ReplyDataNetAddress : CommandMsg
{
	// Token: 0x060006F7 RID: 1783 RVA: 0x00011CC4 File Offset: 0x0000FEC4
	internal ReplyDataNetAddress(IntPtr cPtr, bool cMemoryOwn)
		: base(SDPCorePINVOKE.ReplyDataNetAddress_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x00011CE0 File Offset: 0x0000FEE0
	internal static HandleRef getCPtr(ReplyDataNetAddress obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00011CF8 File Offset: 0x0000FEF8
	~ReplyDataNetAddress()
	{
		this.Dispose();
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00011D24 File Offset: 0x0000FF24
	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_ReplyDataNetAddress(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060006FC RID: 1788 RVA: 0x00011DB8 File Offset: 0x0000FFB8
	// (set) Token: 0x060006FB RID: 1787 RVA: 0x00011DA8 File Offset: 0x0000FFA8
	public string address
	{
		get
		{
			return SDPCorePINVOKE.ReplyDataNetAddress_address_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyDataNetAddress_address_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060006FE RID: 1790 RVA: 0x00011DE0 File Offset: 0x0000FFE0
	// (set) Token: 0x060006FD RID: 1789 RVA: 0x00011DD2 File Offset: 0x0000FFD2
	public uint filePort
	{
		get
		{
			return SDPCorePINVOKE.ReplyDataNetAddress_filePort_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyDataNetAddress_filePort_set(this.swigCPtr, value);
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000700 RID: 1792 RVA: 0x00011E08 File Offset: 0x00010008
	// (set) Token: 0x060006FF RID: 1791 RVA: 0x00011DFA File Offset: 0x0000FFFA
	public uint optionPort
	{
		get
		{
			return SDPCorePINVOKE.ReplyDataNetAddress_optionPort_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.ReplyDataNetAddress_optionPort_set(this.swigCPtr, value);
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00011E22 File Offset: 0x00010022
	public ReplyDataNetAddress(string ipAddress, uint fPort, uint oPort)
		: this(SDPCorePINVOKE.new_ReplyDataNetAddress(ipAddress, fPort, oPort), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x04000165 RID: 357
	private HandleRef swigCPtr;
}
