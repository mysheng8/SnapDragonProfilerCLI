using System;
using System.Runtime.InteropServices;

// Token: 0x0200002F RID: 47
public class DeviceModel : IDisposable
{
	// Token: 0x060002A4 RID: 676 RVA: 0x00007E3F File Offset: 0x0000603F
	internal DeviceModel(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00007E5B File Offset: 0x0000605B
	internal static HandleRef getCPtr(DeviceModel obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00007E74 File Offset: 0x00006074
	~DeviceModel()
	{
		this.Dispose();
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00007EA0 File Offset: 0x000060A0
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DeviceModel(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00007F20 File Offset: 0x00006120
	public DeviceModel()
		: this(SDPCorePINVOKE.new_DeviceModel(), true)
	{
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060002AA RID: 682 RVA: 0x00007F3C File Offset: 0x0000613C
	// (set) Token: 0x060002A9 RID: 681 RVA: 0x00007F2E File Offset: 0x0000612E
	public uint id
	{
		get
		{
			return SDPCorePINVOKE.DeviceModel_id_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_id_set(this.swigCPtr, value);
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060002AC RID: 684 RVA: 0x00007F74 File Offset: 0x00006174
	// (set) Token: 0x060002AB RID: 683 RVA: 0x00007F56 File Offset: 0x00006156
	public string serialNumber
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_serialNumber_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_serialNumber_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060002AE RID: 686 RVA: 0x00007FB8 File Offset: 0x000061B8
	// (set) Token: 0x060002AD RID: 685 RVA: 0x00007F9B File Offset: 0x0000619B
	public string productName
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_productName_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_productName_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060002B0 RID: 688 RVA: 0x00007FFC File Offset: 0x000061FC
	// (set) Token: 0x060002AF RID: 687 RVA: 0x00007FDF File Offset: 0x000061DF
	public string productModel
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_productModel_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_productModel_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060002B2 RID: 690 RVA: 0x00008040 File Offset: 0x00006240
	// (set) Token: 0x060002B1 RID: 689 RVA: 0x00008023 File Offset: 0x00006223
	public string productManufacturer
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_productManufacturer_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_productManufacturer_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060002B4 RID: 692 RVA: 0x00008084 File Offset: 0x00006284
	// (set) Token: 0x060002B3 RID: 691 RVA: 0x00008067 File Offset: 0x00006267
	public string productBrand
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_productBrand_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_productBrand_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060002B6 RID: 694 RVA: 0x000080C8 File Offset: 0x000062C8
	// (set) Token: 0x060002B5 RID: 693 RVA: 0x000080AB File Offset: 0x000062AB
	public string productLocaleRegion
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_productLocaleRegion_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_productLocaleRegion_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000810C File Offset: 0x0000630C
	// (set) Token: 0x060002B7 RID: 695 RVA: 0x000080EF File Offset: 0x000062EF
	public string productLocaleLanguage
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_productLocaleLanguage_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_productLocaleLanguage_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060002BA RID: 698 RVA: 0x00008150 File Offset: 0x00006350
	// (set) Token: 0x060002B9 RID: 697 RVA: 0x00008133 File Offset: 0x00006333
	public string buildProduct
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_buildProduct_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_buildProduct_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060002BC RID: 700 RVA: 0x00008194 File Offset: 0x00006394
	// (set) Token: 0x060002BB RID: 699 RVA: 0x00008177 File Offset: 0x00006377
	public string buildVersionRelease
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_buildVersionRelease_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_buildVersionRelease_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060002BE RID: 702 RVA: 0x000081D8 File Offset: 0x000063D8
	// (set) Token: 0x060002BD RID: 701 RVA: 0x000081BB File Offset: 0x000063BB
	public string buildVersionSDK
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_buildVersionSDK_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_buildVersionSDK_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000821C File Offset: 0x0000641C
	// (set) Token: 0x060002BF RID: 703 RVA: 0x000081FF File Offset: 0x000063FF
	public string buildDate
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_buildDate_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_buildDate_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060002C2 RID: 706 RVA: 0x00008260 File Offset: 0x00006460
	// (set) Token: 0x060002C1 RID: 705 RVA: 0x00008243 File Offset: 0x00006443
	public string buildDescription
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_buildDescription_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_buildDescription_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x060002C4 RID: 708 RVA: 0x000082A4 File Offset: 0x000064A4
	// (set) Token: 0x060002C3 RID: 707 RVA: 0x00008287 File Offset: 0x00006487
	public string boardPlatform
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceModel_boardPlatform_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceModel_boardPlatform_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x0400008C RID: 140
	private HandleRef swigCPtr;

	// Token: 0x0400008D RID: 141
	protected bool swigCMemOwn;
}
