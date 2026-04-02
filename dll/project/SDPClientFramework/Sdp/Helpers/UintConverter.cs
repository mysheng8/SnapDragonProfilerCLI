using System;
using System.Globalization;
using System.Reflection;
using Sdp.Functional;

namespace Sdp.Helpers
{
	// Token: 0x0200030A RID: 778
	public class UintConverter : TypeConverter
	{
		// Token: 0x06001011 RID: 4113 RVA: 0x000328CC File Offset: 0x00030ACC
		public static Result<uint, string> TryConvert(string s)
		{
			uint num;
			if (!uint.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num))
			{
				return new Result<uint, string>.Error(TypeConverter.GetErrorString(MethodBase.GetCurrentMethod(), s));
			}
			return new Result<uint, string>.Success(num);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00032908 File Offset: 0x00030B08
		[Obsolete("Convert is deprecated please use TryConvert instead")]
		public static uint Convert(string s)
		{
			return UintConverter.TryConvert(s).Match<uint>((uint i) => i, delegate(string error)
			{
				TypeConverter.Logger.LogError(error);
				return 0U;
			});
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00032960 File Offset: 0x00030B60
		public static uint Convert(string s, int fromBase)
		{
			uint num = 0U;
			try
			{
				num = global::System.Convert.ToUInt32(s, fromBase);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return num;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00032998 File Offset: 0x00030B98
		public static uint Convert(object o)
		{
			uint num = 0U;
			try
			{
				num = global::System.Convert.ToUInt32(o);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), o);
			}
			return num;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x000329D0 File Offset: 0x00030BD0
		public static uint Convert(byte b)
		{
			uint num = 0U;
			try
			{
				num = global::System.Convert.ToUInt32(b);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), b);
			}
			return num;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00032A0C File Offset: 0x00030C0C
		public static uint Convert(ushort s)
		{
			uint num = 0U;
			try
			{
				num = global::System.Convert.ToUInt32(s);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return num;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00032A48 File Offset: 0x00030C48
		public static uint Convert(bool b)
		{
			uint num = 0U;
			try
			{
				num = global::System.Convert.ToUInt32(b);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), b);
			}
			return num;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00032A84 File Offset: 0x00030C84
		public static bool IsValid(string s)
		{
			uint num = 0U;
			return uint.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00032AAB File Offset: 0x00030CAB
		public static string ToString(uint i, string format = "")
		{
			return i.ToString(format, CultureInfo.InvariantCulture);
		}
	}
}
