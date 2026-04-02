using System;
using System.Globalization;
using System.Reflection;

namespace Sdp.Helpers
{
	// Token: 0x0200030D RID: 781
	public class Int64Converter : TypeConverter
	{
		// Token: 0x06001028 RID: 4136 RVA: 0x00032CBC File Offset: 0x00030EBC
		public static long Convert(string s)
		{
			long num = 0L;
			if (!long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num))
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return num;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00032CF0 File Offset: 0x00030EF0
		public static long Convert(double d)
		{
			long num = 0L;
			try
			{
				num = global::System.Convert.ToInt64(d);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), d);
			}
			return num;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00032D30 File Offset: 0x00030F30
		public static bool IsValid(string s)
		{
			long num = 0L;
			return long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num);
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00032D58 File Offset: 0x00030F58
		public static string ToString(long i, string format = "")
		{
			return i.ToString(format, CultureInfo.InvariantCulture);
		}
	}
}
