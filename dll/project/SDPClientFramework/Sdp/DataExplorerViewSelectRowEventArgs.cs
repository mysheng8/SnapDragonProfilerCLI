using System;

namespace Sdp
{
	// Token: 0x02000143 RID: 323
	public class DataExplorerViewSelectRowEventArgs : MultiViewArgs
	{
		// Token: 0x040004A8 RID: 1192
		public int SourceID;

		// Token: 0x040004A9 RID: 1193
		public int CaptureID;

		// Token: 0x040004AA RID: 1194
		public object RowElement;

		// Token: 0x040004AB RID: 1195
		public int SearchColumn;

		// Token: 0x040004AC RID: 1196
		public bool Expand = true;

		// Token: 0x040004AD RID: 1197
		public object[] HighlightElements;

		// Token: 0x040004AE RID: 1198
		public string Instigator;
	}
}
