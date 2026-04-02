using System;
using System.Runtime.InteropServices;

// Token: 0x0200004C RID: 76
public class Option : IDisposable
{
	// Token: 0x060004CB RID: 1227 RVA: 0x0000CAD6 File Offset: 0x0000ACD6
	internal Option(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = new HandleRef(this, cPtr);
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x0000CAF2 File Offset: 0x0000ACF2
	internal static HandleRef getCPtr(Option obj)
	{
		if (obj != null)
		{
			return obj.swigCPtr;
		}
		return new HandleRef(null, IntPtr.Zero);
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0000CB0C File Offset: 0x0000AD0C
	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr.Handle != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					throw new MethodAccessException("C++ destructor does not have public access");
				}
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
			GC.SuppressFinalize(this);
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0000CB8C File Offset: 0x0000AD8C
	public uint GetID()
	{
		return SDPCorePINVOKE.Option_GetID(this.swigCPtr);
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0000CBA8 File Offset: 0x0000ADA8
	public string GetName()
	{
		return SDPCorePINVOKE.Option_GetName(this.swigCPtr);
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0000CBC4 File Offset: 0x0000ADC4
	public string GetDescription()
	{
		return SDPCorePINVOKE.Option_GetDescription(this.swigCPtr);
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
	public SDPDataType GetOptionType()
	{
		return (SDPDataType)SDPCorePINVOKE.Option_GetOptionType(this.swigCPtr);
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x0000CBFC File Offset: 0x0000ADFC
	public uint GetDataProviderId()
	{
		return SDPCorePINVOKE.Option_GetDataProviderId(this.swigCPtr);
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0000CC18 File Offset: 0x0000AE18
	public OptionCategory GetCategory()
	{
		IntPtr intPtr = SDPCorePINVOKE.Option_GetCategory(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new OptionCategory(intPtr, false);
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x0000CC4C File Offset: 0x0000AE4C
	public uint GetProcessId()
	{
		return SDPCorePINVOKE.Option_GetProcessId(this.swigCPtr);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x0000CC68 File Offset: 0x0000AE68
	public virtual bool IsOptionHidden()
	{
		return SDPCorePINVOKE.Option_IsOptionHidden(this.swigCPtr);
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0000CC84 File Offset: 0x0000AE84
	public virtual bool IsOptionReadonly()
	{
		return SDPCorePINVOKE.Option_IsOptionReadonly(this.swigCPtr);
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x0000CCA0 File Offset: 0x0000AEA0
	public virtual bool IsOptionContextState()
	{
		return SDPCorePINVOKE.Option_IsOptionContextState(this.swigCPtr);
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0000CCBC File Offset: 0x0000AEBC
	public virtual bool IsOptionProcInfo()
	{
		return SDPCorePINVOKE.Option_IsOptionProcInfo(this.swigCPtr);
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0000CCD8 File Offset: 0x0000AED8
	public virtual bool IsOptionLaunchApplication()
	{
		return SDPCorePINVOKE.Option_IsOptionLaunchApplication(this.swigCPtr);
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x0000CCF4 File Offset: 0x0000AEF4
	public virtual bool GetAttributes(SWIGTYPE_p_unsigned_int value)
	{
		bool flag = SDPCorePINVOKE.Option_GetAttributes(this.swigCPtr, SWIGTYPE_p_unsigned_int.getCPtr(value));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0000CD24 File Offset: 0x0000AF24
	public virtual bool SetAttributes(uint attribute)
	{
		return SDPCorePINVOKE.Option_SetAttributes(this.swigCPtr, attribute);
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0000CD40 File Offset: 0x0000AF40
	public virtual bool SetValue(bool arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_0(this.swigCPtr, arg0, arg1);
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x0000CD5C File Offset: 0x0000AF5C
	public virtual bool SetValue(bool arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_1(this.swigCPtr, arg0);
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0000CD78 File Offset: 0x0000AF78
	public virtual bool SetValue(int arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_2(this.swigCPtr, arg0, arg1);
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x0000CD94 File Offset: 0x0000AF94
	public virtual bool SetValue(int arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_3(this.swigCPtr, arg0);
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0000CDB0 File Offset: 0x0000AFB0
	public virtual bool SetValue(long arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_4(this.swigCPtr, arg0, arg1);
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0000CDCC File Offset: 0x0000AFCC
	public virtual bool SetValue(long arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_5(this.swigCPtr, arg0);
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
	public virtual bool SetValue(uint arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_6(this.swigCPtr, arg0, arg1);
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0000CE04 File Offset: 0x0000B004
	public virtual bool SetValue(uint arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_7(this.swigCPtr, arg0);
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0000CE20 File Offset: 0x0000B020
	public virtual bool SetValue(ulong arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_8(this.swigCPtr, arg0, arg1);
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x0000CE3C File Offset: 0x0000B03C
	public virtual bool SetValue(ulong arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_9(this.swigCPtr, arg0);
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x0000CE58 File Offset: 0x0000B058
	public virtual bool SetValue(float arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_10(this.swigCPtr, arg0, arg1);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x0000CE74 File Offset: 0x0000B074
	public virtual bool SetValue(float arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_11(this.swigCPtr, arg0);
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0000CE90 File Offset: 0x0000B090
	public virtual bool SetValue(double arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_12(this.swigCPtr, arg0, arg1);
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0000CEAC File Offset: 0x0000B0AC
	public virtual bool SetValue(double arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_13(this.swigCPtr, arg0);
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x0000CEC8 File Offset: 0x0000B0C8
	public virtual bool SetValue(IntPtr arg0, uint arg1, bool arg2)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_14(this.swigCPtr, arg0, arg1, arg2);
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0000CEE8 File Offset: 0x0000B0E8
	public virtual bool SetValue(IntPtr arg0, uint arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_15(this.swigCPtr, arg0, arg1);
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0000CF04 File Offset: 0x0000B104
	public virtual bool SetValue(SWIGTYPE_p_float arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_16(this.swigCPtr, SWIGTYPE_p_float.getCPtr(arg0), arg1);
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x0000CF28 File Offset: 0x0000B128
	public virtual bool SetValue(SWIGTYPE_p_float arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_17(this.swigCPtr, SWIGTYPE_p_float.getCPtr(arg0));
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x0000CF48 File Offset: 0x0000B148
	public virtual bool SetValue(float arg0, float arg1, float arg2, float arg3, bool arg4)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_18(this.swigCPtr, arg0, arg1, arg2, arg3, arg4);
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x0000CF6C File Offset: 0x0000B16C
	public virtual bool SetValue(float arg0, float arg1, float arg2, float arg3)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_19(this.swigCPtr, arg0, arg1, arg2, arg3);
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0000CF8C File Offset: 0x0000B18C
	public virtual bool SetValue(OptionStructData arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_20(this.swigCPtr, OptionStructData.getCPtr(arg0), arg1);
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x0000CFB0 File Offset: 0x0000B1B0
	public virtual bool SetValue(OptionStructData arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_21(this.swigCPtr, OptionStructData.getCPtr(arg0));
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x0000CFD0 File Offset: 0x0000B1D0
	public virtual bool SetValue(OptionStructDef arg0, bool arg1)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_22(this.swigCPtr, OptionStructDef.getCPtr(arg0), arg1);
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x0000CFF4 File Offset: 0x0000B1F4
	public virtual bool SetValue(OptionStructDef arg0)
	{
		return SDPCorePINVOKE.Option_SetValue__SWIG_23(this.swigCPtr, OptionStructDef.getCPtr(arg0));
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0000D014 File Offset: 0x0000B214
	public virtual bool SetValue(string value, bool publishValue)
	{
		bool flag = SDPCorePINVOKE.Option_SetValue__SWIG_24(this.swigCPtr, value, publishValue);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x0000D040 File Offset: 0x0000B240
	public virtual bool SetValue(string value)
	{
		bool flag = SDPCorePINVOKE.Option_SetValue__SWIG_25(this.swigCPtr, value);
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x0000D068 File Offset: 0x0000B268
	public virtual string GetValueStr()
	{
		return SDPCorePINVOKE.Option_GetValueStr(this.swigCPtr);
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x0000D084 File Offset: 0x0000B284
	public virtual string GetRawValueStr()
	{
		return SDPCorePINVOKE.Option_GetRawValueStr(this.swigCPtr);
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x0000D0A0 File Offset: 0x0000B2A0
	public virtual bool GetValue(out bool value)
	{
		return SDPCorePINVOKE.Option_GetValue__SWIG_0(this.swigCPtr, out value);
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x0000D0BC File Offset: 0x0000B2BC
	public virtual bool GetValue(SWIGTYPE_p_std__string arg0)
	{
		bool flag = SDPCorePINVOKE.Option_GetValue__SWIG_1(this.swigCPtr, SWIGTYPE_p_std__string.getCPtr(arg0));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0000D0EC File Offset: 0x0000B2EC
	public virtual bool GetValue(SWIGTYPE_p_int arg0)
	{
		bool flag = SDPCorePINVOKE.Option_GetValue__SWIG_2(this.swigCPtr, SWIGTYPE_p_int.getCPtr(arg0));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0000D11C File Offset: 0x0000B31C
	public virtual bool GetValue(SWIGTYPE_p_long_long arg0)
	{
		bool flag = SDPCorePINVOKE.Option_GetValue__SWIG_3(this.swigCPtr, SWIGTYPE_p_long_long.getCPtr(arg0));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0000D14C File Offset: 0x0000B34C
	public virtual bool GetValue(SWIGTYPE_p_unsigned_int arg0)
	{
		bool flag = SDPCorePINVOKE.Option_GetValue__SWIG_4(this.swigCPtr, SWIGTYPE_p_unsigned_int.getCPtr(arg0));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x0000D17C File Offset: 0x0000B37C
	public virtual bool GetValue(SWIGTYPE_p_unsigned_long_long arg0)
	{
		bool flag = SDPCorePINVOKE.Option_GetValue__SWIG_5(this.swigCPtr, SWIGTYPE_p_unsigned_long_long.getCPtr(arg0));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
		return flag;
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x0000D1AC File Offset: 0x0000B3AC
	public virtual bool GetValue(out float value)
	{
		return SDPCorePINVOKE.Option_GetValue__SWIG_6(this.swigCPtr, out value);
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
	public virtual bool GetValue(out double value)
	{
		return SDPCorePINVOKE.Option_GetValue__SWIG_7(this.swigCPtr, out value);
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0000D1E4 File Offset: 0x0000B3E4
	public virtual bool GetValue(IntPtr arg0, uint arg1)
	{
		return SDPCorePINVOKE.Option_GetValue__SWIG_8(this.swigCPtr, arg0, arg1);
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0000D200 File Offset: 0x0000B400
	public virtual bool GetValue(out float r, out float g, out float b, out float a)
	{
		return SDPCorePINVOKE.Option_GetValue__SWIG_9(this.swigCPtr, out r, out g, out b, out a);
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0000D220 File Offset: 0x0000B420
	public virtual bool GetValue(OptionStructData arg0)
	{
		return SDPCorePINVOKE.Option_GetValue__SWIG_10(this.swigCPtr, OptionStructData.getCPtr(arg0));
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0000D240 File Offset: 0x0000B440
	public virtual uint GetValueSize()
	{
		return SDPCorePINVOKE.Option_GetValueSize(this.swigCPtr);
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0000D25A File Offset: 0x0000B45A
	public virtual void Reset()
	{
		SDPCorePINVOKE.Option_Reset(this.swigCPtr);
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0000D268 File Offset: 0x0000B468
	public virtual OptionStructData GetOptionStructData()
	{
		IntPtr intPtr = SDPCorePINVOKE.Option_GetOptionStructData(this.swigCPtr);
		return (intPtr == IntPtr.Zero) ? null : new OptionStructData(intPtr, false);
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x0000D29A File Offset: 0x0000B49A
	public virtual void RegisterOptionChangeHandler(Void_UInt_UInt_UInt_Fn handler)
	{
		SDPCorePINVOKE.Option_RegisterOptionChangeHandler__SWIG_0(this.swigCPtr, handler);
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x0000D2A8 File Offset: 0x0000B4A8
	public virtual void RegisterOptionChangeHandler(SWIGTYPE_p_std__functionT_void_funsigned_int_const_R_unsigned_int_const_R_unsigned_int_const_RF_t callback)
	{
		SDPCorePINVOKE.Option_RegisterOptionChangeHandler__SWIG_1(this.swigCPtr, SWIGTYPE_p_std__functionT_void_funsigned_int_const_R_unsigned_int_const_R_unsigned_int_const_RF_t.getCPtr(callback));
		if (SDPCorePINVOKE.SWIGPendingException.Pending)
		{
			throw SDPCorePINVOKE.SWIGPendingException.Retrieve();
		}
	}

	// Token: 0x0400012E RID: 302
	private HandleRef swigCPtr;

	// Token: 0x0400012F RID: 303
	protected bool swigCMemOwn;
}
