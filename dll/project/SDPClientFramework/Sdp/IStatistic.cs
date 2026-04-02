using System;

namespace Sdp
{
	// Token: 0x020001A7 RID: 423
	public interface IStatistic
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000531 RID: 1329
		string Name { get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000532 RID: 1330
		string Category { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000533 RID: 1331
		string Description { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000534 RID: 1332
		StatisticDisplayType[] AvailableDisplays { get; }

		// Token: 0x06000535 RID: 1333
		IStatisticDisplayViewModel[] GenerateViewModels(int captureID);
	}
}
