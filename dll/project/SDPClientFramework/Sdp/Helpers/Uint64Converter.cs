using System;
using System.Globalization;
using System.Reflection;

namespace Sdp.Helpers
{
	// Token: 0x0200030B RID: 779
	public class Uint64Converter : TypeConverter
	{
		// Token: 0x0600101B RID: 4123 RVA: 0x00032AC4 File Offset: 0x00030CC4
		public static ulong Convert(string s)
		{
			ulong num = 0UL;
			if (!ulong.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num))
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return num;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00032AF8 File Offset: 0x00030CF8
		public static ulong Convert(object o)
		{
			ulong num = 0UL;
			try
			{
				num = global::System.Convert.ToUInt64(o);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), o);
			}
			return num;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00032B30 File Offset: 0x00030D30
		public static bool IsValid(string s)
		{
			ulong num = 0UL;
			return ulong.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out num);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00032B58 File Offset: 0x00030D58
		public static string ToString(ulong i, string format = "")
		{
			return i.ToString(format, CultureInfo.InvariantCulture);
		}
	}
}
