using System;
using System.Globalization;
using System.Reflection;

namespace Sdp.Helpers
{
	// Token: 0x0200030E RID: 782
	public class FloatConverter : TypeConverter
	{
		// Token: 0x0600102D RID: 4141 RVA: 0x00032D68 File Offset: 0x00030F68
		public static float Convert(string s)
		{
			float num = 0f;
			if (!float.TryParse(s.TrimEnd(new char[] { 'f' }), NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out num))
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return num;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00032DB0 File Offset: 0x00030FB0
		public static float Convert(sbyte s)
		{
			float num = 0f;
			try
			{
				num = global::System.Convert.ToSingle(s);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return num;
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00032DF0 File Offset: 0x00030FF0
		public static float Convert(byte b)
		{
			float num = 0f;
			try
			{
				num = global::System.Convert.ToSingle(b);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), b);
			}
			return num;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00032E30 File Offset: 0x00031030
		public static float Convert(int i)
		{
			float num = 0f;
			try
			{
				num = global::System.Convert.ToSingle(i);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), i);
			}
			return num;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00032E70 File Offset: 0x00031070
		public static float Convert(uint u)
		{
			float num = 0f;
			try
			{
				num = global::System.Convert.ToSingle(u);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), u);
			}
			return num;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00032EB0 File Offset: 0x000310B0
		public static float Convert(short s)
		{
			float num = 0f;
			try
			{
				num = global::System.Convert.ToSingle(s);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return num;
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x00032EF0 File Offset: 0x000310F0
		public static float Convert(ushort u)
		{
			float num = 0f;
			try
			{
				num = global::System.Convert.ToSingle(u);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), u);
			}
			return num;
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x00032F30 File Offset: 0x00031130
		public static bool IsValid(string s)
		{
			float num = 0f;
			return float.TryParse(s.TrimEnd(new char[] { 'f' }), NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out num);
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00032F6F File Offset: 0x0003116F
		public static string ToString(float f, string format = "")
		{
			return f.ToString(format, CultureInfo.InvariantCulture);
		}
	}
}
