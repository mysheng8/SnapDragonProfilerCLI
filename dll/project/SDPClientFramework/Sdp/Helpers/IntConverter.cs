using System;
using System.Globalization;
using System.Reflection;

namespace Sdp.Helpers
{
	// Token: 0x0200030C RID: 780
	public class IntConverter : TypeConverter
	{
		// Token: 0x06001020 RID: 4128 RVA: 0x00032B68 File Offset: 0x00030D68
		public static int Convert(string s)
		{
			int num = 0;
			if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num))
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return num;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00032B98 File Offset: 0x00030D98
		public static int Convert(object o)
		{
			int num = 0;
			try
			{
				num = global::System.Convert.ToInt32(o);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), o);
			}
			return num;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00032BD0 File Offset: 0x00030DD0
		public static int Convert(double d)
		{
			int num = 0;
			try
			{
				num = global::System.Convert.ToInt32(d);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), d);
			}
			return num;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00032C0C File Offset: 0x00030E0C
		public static int Convert(uint u)
		{
			int num = 0;
			try
			{
				num = global::System.Convert.ToInt32(u);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), u);
			}
			return num;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00032C48 File Offset: 0x00030E48
		public static int Convert(bool b)
		{
			int num = 0;
			try
			{
				num = global::System.Convert.ToInt32(b);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), b);
			}
			return num;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00032C84 File Offset: 0x00030E84
		public static bool IsValid(string s)
		{
			int num = 0;
			return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00032CAB File Offset: 0x00030EAB
		public static string ToString(int i, string format = "")
		{
			return i.ToString(format, CultureInfo.InvariantCulture);
		}
	}
}
