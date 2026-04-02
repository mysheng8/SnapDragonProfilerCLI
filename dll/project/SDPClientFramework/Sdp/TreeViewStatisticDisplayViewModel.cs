using System;
using System.IO;

namespace Sdp
{
	// Token: 0x020001AB RID: 427
	public class TreeViewStatisticDisplayViewModel : IStatisticDisplayViewModel
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0000C091 File Offset: 0x0000A291
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x0000C099 File Offset: 0x0000A299
		public TreeModel Model { get; set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x00008AD4 File Offset: 0x00006CD4
		public StatisticDisplayType DisplayType
		{
			get
			{
				return StatisticDisplayType.TreeView;
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000C0A2 File Offset: 0x0000A2A2
		public void ExportToCSV(StreamWriter sw)
		{
			this.Model.ExportToCSV(sw);
		}
	}
}
