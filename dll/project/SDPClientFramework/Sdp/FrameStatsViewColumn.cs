using System;

namespace Sdp
{
	// Token: 0x020001C9 RID: 457
	public class FrameStatsViewColumn
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x0000D94A File Offset: 0x0000BB4A
		public FrameStatsViewColumn(string title, int modelIndex)
		{
			this.Title = title;
			this.ModelIndex = modelIndex;
		}

		// Token: 0x0400068D RID: 1677
		public string Title;

		// Token: 0x0400068E RID: 1678
		public int ModelIndex;
	}
}
