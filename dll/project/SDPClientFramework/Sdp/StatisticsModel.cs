using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001A4 RID: 420
	public class StatisticsModel
	{
		// Token: 0x04000644 RID: 1604
		public readonly Dictionary<int, HashSet<IStatistic>> StatisticsStatePerCapture = new Dictionary<int, HashSet<IStatistic>>();

		// Token: 0x04000645 RID: 1605
		public readonly HashSet<IStatistic> Statistics = new HashSet<IStatistic>();
	}
}
