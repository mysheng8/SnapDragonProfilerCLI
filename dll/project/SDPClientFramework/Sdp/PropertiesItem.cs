using System;

namespace Sdp
{
	// Token: 0x020001DB RID: 475
	public class PropertiesItem
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0000FD48 File Offset: 0x0000DF48
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x0000FD50 File Offset: 0x0000DF50
		public int PropertiesItemID { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0000FD59 File Offset: 0x0000DF59
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x0000FD61 File Offset: 0x0000DF61
		public string Category { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0000FD6A File Offset: 0x0000DF6A
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x0000FD72 File Offset: 0x0000DF72
		public ItemType ItemType { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0000FD7B File Offset: 0x0000DF7B
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x0000FD83 File Offset: 0x0000DF83
		public string Name { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0000FD8C File Offset: 0x0000DF8C
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x0000FD94 File Offset: 0x0000DF94
		public string Description { get; set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0000FD9D File Offset: 0x0000DF9D
		// (set) Token: 0x0600067E RID: 1662 RVA: 0x0000FDA5 File Offset: 0x0000DFA5
		public bool Error { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0000FDAE File Offset: 0x0000DFAE
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x0000FDB6 File Offset: 0x0000DFB6
		public object Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
				if (this.Changed != null)
				{
					this.Changed(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x040006E0 RID: 1760
		public EventHandler Changed;

		// Token: 0x040006E7 RID: 1767
		protected object m_value;
	}
}
