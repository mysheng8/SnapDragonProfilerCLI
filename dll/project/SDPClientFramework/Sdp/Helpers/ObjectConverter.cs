using System;
using System.Globalization;

namespace Sdp.Helpers
{
	// Token: 0x02000311 RID: 785
	public class ObjectConverter
	{
		// Token: 0x06001040 RID: 4160 RVA: 0x00033100 File Offset: 0x00031300
		public static string ToString(object o)
		{
			return Convert.ToString(o, CultureInfo.InvariantCulture);
		}
	}
}
