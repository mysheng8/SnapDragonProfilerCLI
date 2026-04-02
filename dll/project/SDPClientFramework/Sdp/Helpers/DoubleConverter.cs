using System;
using System.Globalization;
using System.Reflection;

namespace Sdp.Helpers
{
	// Token: 0x0200030F RID: 783
	public class DoubleConverter : TypeConverter
	{
		// Token: 0x06001037 RID: 4151 RVA: 0x00032F80 File Offset: 0x00031180
		public static double Convert(string s)
		{
			double num = 0.0;
			if (!double.TryParse(s.TrimEnd(new char[] { 'f' }), NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out num))
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return num;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00032FCC File Offset: 0x000311CC
		public static double Convert(object o)
		{
			double num = 0.0;
			try
			{
				num = global::System.Convert.ToDouble(o);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), o);
			}
			return num;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0003300C File Offset: 0x0003120C
		public static bool IsValid(string s)
		{
			double num = 0.0;
			return double.TryParse(s.TrimEnd(new char[] { 'f' }), NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out num);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0003304F File Offset: 0x0003124F
		public static string ToString(double d, string format = "")
		{
			return d.ToString(format, CultureInfo.InvariantCulture);
		}
	}
}
