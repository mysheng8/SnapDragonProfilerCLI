using System;
using System.IO;

namespace Sdp
{
	// Token: 0x020001AA RID: 426
	public interface IStatisticDisplayViewModel
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000536 RID: 1334
		StatisticDisplayType DisplayType { get; }

		// Token: 0x06000537 RID: 1335
		void ExportToCSV(StreamWriter sw);
	}
}
