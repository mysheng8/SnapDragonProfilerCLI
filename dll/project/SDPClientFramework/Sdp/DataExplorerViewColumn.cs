using System;

namespace Sdp
{
	// Token: 0x020002B7 RID: 695
	public class DataExplorerViewColumn
	{
		// Token: 0x06000DF2 RID: 3570 RVA: 0x0002AF37 File Offset: 0x00029137
		public DataExplorerViewColumn(string title, int modelIndex, TreeModel.ParserType type = TreeModel.ParserType.STRING)
		{
			this.Title = title;
			this.ModelIndex = modelIndex;
			this.SortIndex = modelIndex;
			this.HasPixbuf = false;
			this.ParsingType = type;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0002AF70 File Offset: 0x00029170
		public DataExplorerViewColumn(string title, int modelIndex, int sortIndex, bool hasPixbuf = false, TreeModel.ParserType type = TreeModel.ParserType.STRING)
		{
			this.Title = title;
			this.ModelIndex = modelIndex;
			this.SortIndex = sortIndex;
			this.HasPixbuf = hasPixbuf;
			this.ParsingType = type;
		}

		// Token: 0x04000987 RID: 2439
		public string Title;

		// Token: 0x04000988 RID: 2440
		public int ModelIndex;

		// Token: 0x04000989 RID: 2441
		public int SortIndex;

		// Token: 0x0400098A RID: 2442
		public bool HasPixbuf;

		// Token: 0x0400098B RID: 2443
		public TreeModel.ParserType ParsingType = TreeModel.ParserType.STRING;

		// Token: 0x0400098C RID: 2444
		public bool Visible = true;
	}
}
