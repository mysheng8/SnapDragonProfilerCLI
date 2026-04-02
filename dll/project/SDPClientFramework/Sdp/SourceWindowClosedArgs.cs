using System;

namespace Sdp
{
	// Token: 0x0200027D RID: 637
	public class SourceWindowClosedArgs : EventArgs
	{
		// Token: 0x06000AFF RID: 2815 RVA: 0x000203A7 File Offset: 0x0001E5A7
		public SourceWindowClosedArgs(CodeItem item)
		{
			this.m_item = item;
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x000203B6 File Offset: 0x0001E5B6
		public CodeItem Item
		{
			get
			{
				return this.m_item;
			}
		}

		// Token: 0x040008A9 RID: 2217
		private CodeItem m_item;
	}
}
