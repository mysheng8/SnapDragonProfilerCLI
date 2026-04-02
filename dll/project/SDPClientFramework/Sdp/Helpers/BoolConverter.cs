using System;
using System.Reflection;

namespace Sdp.Helpers
{
	// Token: 0x02000310 RID: 784
	public class BoolConverter : TypeConverter
	{
		// Token: 0x0600103C RID: 4156 RVA: 0x00033060 File Offset: 0x00031260
		public static bool Convert(string s)
		{
			bool flag = false;
			if (!bool.TryParse(s, out flag))
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), s);
			}
			return flag;
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00033088 File Offset: 0x00031288
		public static bool Convert(int i)
		{
			bool flag = false;
			try
			{
				flag = global::System.Convert.ToBoolean(i);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), i);
			}
			return flag;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x000330C4 File Offset: 0x000312C4
		public static bool Convert(uint u)
		{
			bool flag = false;
			try
			{
				flag = global::System.Convert.ToBoolean(u);
			}
			catch
			{
				TypeConverter.LogError(MethodBase.GetCurrentMethod(), u);
			}
			return flag;
		}
	}
}
