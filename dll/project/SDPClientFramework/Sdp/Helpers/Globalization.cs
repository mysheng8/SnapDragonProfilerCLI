using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Sdp.Helpers
{
	// Token: 0x02000308 RID: 776
	public class Globalization
	{
		// Token: 0x06001008 RID: 4104 RVA: 0x0003282D File Offset: 0x00030A2D
		public static void SetLocale()
		{
			Globalization.timer.Start();
			Globalization.userRegion = RegionInfo.CurrentRegion;
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		}

		// Token: 0x04000ADE RID: 2782
		public static RegionInfo userRegion;

		// Token: 0x04000ADF RID: 2783
		public static readonly Stopwatch timer = new Stopwatch();
	}
}
