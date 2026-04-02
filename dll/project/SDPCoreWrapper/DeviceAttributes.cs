using System;
using System.Runtime.InteropServices;

// Token: 0x02000027 RID: 39
public class DeviceAttributes : IDisposable
{
	// Token: 0x060001F5 RID: 501 RVA: 0x000065FA File Offset: 0x000047FA
	internal DeviceAttributes(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00006616 File Offset: 0x00004816
	internal static HandleRef getCPtr(DeviceAttributes obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x00006630 File Offset: 0x00004830
	~DeviceAttributes()
	{
		this.Dispose();
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000665C File Offset: 0x0000485C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					SDPCorePINVOKE.delete_DeviceAttributes(this.swigCPtr);
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060001FA RID: 506 RVA: 0x000066F8 File Offset: 0x000048F8
	// (set) Token: 0x060001F9 RID: 505 RVA: 0x000066DC File Offset: 0x000048DC
	public string deviceName
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_deviceName_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_deviceName_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060001FC RID: 508 RVA: 0x0000673C File Offset: 0x0000493C
	// (set) Token: 0x060001FB RID: 507 RVA: 0x0000671F File Offset: 0x0000491F
	public string productName
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_productName_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_productName_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060001FE RID: 510 RVA: 0x00006780 File Offset: 0x00004980
	// (set) Token: 0x060001FD RID: 509 RVA: 0x00006763 File Offset: 0x00004963
	public string productModel
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_productModel_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_productModel_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000200 RID: 512 RVA: 0x000067C4 File Offset: 0x000049C4
	// (set) Token: 0x060001FF RID: 511 RVA: 0x000067A7 File Offset: 0x000049A7
	public string productManufacturer
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_productManufacturer_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_productManufacturer_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000202 RID: 514 RVA: 0x00006808 File Offset: 0x00004A08
	// (set) Token: 0x06000201 RID: 513 RVA: 0x000067EB File Offset: 0x000049EB
	public string productBrand
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_productBrand_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_productBrand_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000204 RID: 516 RVA: 0x0000684C File Offset: 0x00004A4C
	// (set) Token: 0x06000203 RID: 515 RVA: 0x0000682F File Offset: 0x00004A2F
	public string productLocaleRegion
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_productLocaleRegion_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_productLocaleRegion_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000206 RID: 518 RVA: 0x00006890 File Offset: 0x00004A90
	// (set) Token: 0x06000205 RID: 517 RVA: 0x00006873 File Offset: 0x00004A73
	public string productLocaleLanguage
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_productLocaleLanguage_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_productLocaleLanguage_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000208 RID: 520 RVA: 0x000068D4 File Offset: 0x00004AD4
	// (set) Token: 0x06000207 RID: 519 RVA: 0x000068B7 File Offset: 0x00004AB7
	public string buildProduct
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_buildProduct_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_buildProduct_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600020A RID: 522 RVA: 0x00006918 File Offset: 0x00004B18
	// (set) Token: 0x06000209 RID: 521 RVA: 0x000068FB File Offset: 0x00004AFB
	public string buildVersionRelease
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_buildVersionRelease_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_buildVersionRelease_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600020C RID: 524 RVA: 0x0000695C File Offset: 0x00004B5C
	// (set) Token: 0x0600020B RID: 523 RVA: 0x0000693F File Offset: 0x00004B3F
	public string buildVersionSDK
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_buildVersionSDK_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_buildVersionSDK_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600020E RID: 526 RVA: 0x000069A0 File Offset: 0x00004BA0
	// (set) Token: 0x0600020D RID: 525 RVA: 0x00006983 File Offset: 0x00004B83
	public string buildDate
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_buildDate_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_buildDate_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000210 RID: 528 RVA: 0x000069E4 File Offset: 0x00004BE4
	// (set) Token: 0x0600020F RID: 527 RVA: 0x000069C7 File Offset: 0x00004BC7
	public string buildDescription
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_buildDescription_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_buildDescription_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000212 RID: 530 RVA: 0x00006A28 File Offset: 0x00004C28
	// (set) Token: 0x06000211 RID: 529 RVA: 0x00006A0B File Offset: 0x00004C0B
	public string boardPlatform
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_boardPlatform_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_boardPlatform_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000214 RID: 532 RVA: 0x00006A6C File Offset: 0x00004C6C
	// (set) Token: 0x06000213 RID: 531 RVA: 0x00006A4F File Offset: 0x00004C4F
	public string osType
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_osType_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_osType_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000216 RID: 534 RVA: 0x00006AB0 File Offset: 0x00004CB0
	// (set) Token: 0x06000215 RID: 533 RVA: 0x00006A93 File Offset: 0x00004C93
	public string abiList
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_abiList_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_abiList_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000218 RID: 536 RVA: 0x00006AE8 File Offset: 0x00004CE8
	// (set) Token: 0x06000217 RID: 535 RVA: 0x00006AD7 File Offset: 0x00004CD7
	public long buildDateUTC
	{
		get
		{
			return SDPCorePINVOKE.DeviceAttributes_buildDateUTC_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_buildDateUTC_set(this.swigCPtr, value);
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600021A RID: 538 RVA: 0x00006B20 File Offset: 0x00004D20
	// (set) Token: 0x06000219 RID: 537 RVA: 0x00006B02 File Offset: 0x00004D02
	public string updatedGraphicsDriver
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_updatedGraphicsDriver_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_updatedGraphicsDriver_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x0600021C RID: 540 RVA: 0x00006B64 File Offset: 0x00004D64
	// (set) Token: 0x0600021B RID: 539 RVA: 0x00006B47 File Offset: 0x00004D47
	public string gameDriverPrereleaseOptInApps
	{
		get
		{
			string text = SDPCorePINVOKE.DeviceAttributes_gameDriverPrereleaseOptInApps_get(this.swigCPtr);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
			return text;
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_gameDriverPrereleaseOptInApps_set(this.swigCPtr, value);
			if (SDPCorePINVOKE.SWIGPendingException.Pending)
			{
				throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600021E RID: 542 RVA: 0x00006B9C File Offset: 0x00004D9C
	// (set) Token: 0x0600021D RID: 541 RVA: 0x00006B8B File Offset: 0x00004D8B
	public bool autoDetected
	{
		get
		{
			return SDPCorePINVOKE.DeviceAttributes_autoDetected_get(this.swigCPtr);
		}
		set
		{
			SDPCorePINVOKE.DeviceAttributes_autoDetected_set(this.swigCPtr, value);
		}
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00006BB8 File Offset: 0x00004DB8
	public string GetProductModel()
	{
		return SDPCorePINVOKE.DeviceAttributes_GetProductModel(this.swigCPtr);
	}

	// Token: 0x06000220 RID: 544 RVA: 0x00006BD4 File Offset: 0x00004DD4
	public string GetBuildVersionRelease()
	{
		return SDPCorePINVOKE.DeviceAttributes_GetBuildVersionRelease(this.swigCPtr);
	}

	// Token: 0x06000221 RID: 545 RVA: 0x00006BF0 File Offset: 0x00004DF0
	public long GetBuildDateUTC()
	{
		return SDPCorePINVOKE.DeviceAttributes_GetBuildDateUTC(this.swigCPtr);
	}

	// Token: 0x06000222 RID: 546 RVA: 0x00006C0A File Offset: 0x00004E0A
	public DeviceAttributes()
		: this(SDPCorePINVOKE.new_DeviceAttributes(), true)
	{
	}

	// Token: 0x04000067 RID: 103
	private HandleRef swigCPtr;

	// Token: 0x04000068 RID: 104
	protected bool swigCMemOwn;
}
