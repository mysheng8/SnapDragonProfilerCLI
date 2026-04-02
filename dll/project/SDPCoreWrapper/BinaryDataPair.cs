using System;
using System.Runtime.InteropServices;

// Token: 0x02000011 RID: 17
public class BinaryDataPair : IDisposable
{
	// Token: 0x06000057 RID: 87 RVA: 0x000025E0 File Offset: 0x000007E0
	internal BinaryDataPair(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwnBase = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000025FC File Offset: 0x000007FC
	internal static HandleRef getCPtr(BinaryDataPair obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00002614 File Offset: 0x00000814
	~BinaryDataPair()
	{
		this.Dispose();
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00002640 File Offset: 0x00000840
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwnBase)
				{
					this.swigCMemOwnBase = false;
					SDPCorePINVOKE.delete_BinaryDataPair(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x0600005B RID: 91 RVA: 0x000026C0 File Offset: 0x000008C0
	public BinaryDataPair()
		: this(SDPCorePINVOKE.new_BinaryDataPair__SWIG_0(), true)
	{
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000026CE File Offset: 0x000008CE
	public BinaryDataPair(BinaryDataPair bdp2)
		: this(SDPCorePINVOKE.new_BinaryDataPair__SWIG_1(BinaryDataPair.getCPtr(bdp2)), true)
	{
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x000026F0 File Offset: 0x000008F0
	public bool IsValid()
	{
		bool flag = SDPCorePINVOKE.BinaryDataPair_IsValid(this.swigCPtr);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00002718 File Offset: 0x00000918
	public BinaryDataPair Equal(BinaryDataPair bdp2)
	{
		BinaryDataPair binaryDataPair = new BinaryDataPair(SDPCorePINVOKE.BinaryDataPair_Equal(this.swigCPtr, BinaryDataPair.getCPtr(bdp2)), true);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return binaryDataPair;
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000060 RID: 96 RVA: 0x00002768 File Offset: 0x00000968
	// (set) Token: 0x0600005F RID: 95 RVA: 0x0000274B File Offset: 0x0000094B
	public uint size
	{
		get
		{
			uint num = SDPCorePINVOKE.BinaryDataPair_size_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return num;
		}
		set
		{
			SDPCorePINVOKE.BinaryDataPair_size_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000062 RID: 98 RVA: 0x000027AC File Offset: 0x000009AC
	// (set) Token: 0x06000061 RID: 97 RVA: 0x0000278F File Offset: 0x0000098F
	public IntPtr data
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.BinaryDataPair_data_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return intPtr;
		}
		set
		{
			SDPCorePINVOKE.BinaryDataPair_data_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000064 RID: 100 RVA: 0x000027F4 File Offset: 0x000009F4
	// (set) Token: 0x06000063 RID: 99 RVA: 0x000027D3 File Offset: 0x000009D3
	public SWIGTYPE_p_std__functionT_void_fSDP__BinaryDataPair_RF_t FinalizeCallback
	{
		get
		{
			IntPtr intPtr = SDPCorePINVOKE.BinaryDataPair_FinalizeCallback_get(this.swigCPtr);
			SWIGTYPE_p_std__functionT_void_fSDP__BinaryDataPair_RF_t swigtype_p_std__functionT_void_fSDP__BinaryDataPair_RF_t = ((intPtr == IntPtr.Zero) ? null : new SWIGTYPE_p_std__functionT_void_fSDP__BinaryDataPair_RF_t(intPtr, false));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return swigtype_p_std__functionT_void_fSDP__BinaryDataPair_RF_t;
		}
		set
		{
			SDPCorePINVOKE.BinaryDataPair_FinalizeCallback_set(this.swigCPtr, SWIGTYPE_p_std__functionT_void_fSDP__BinaryDataPair_RF_t.getCPtr(value));
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x0400000A RID: 10
	private HandleRef swigCPtr;

	// Token: 0x0400000B RID: 11
	private bool swigCMemOwnBase;
}
