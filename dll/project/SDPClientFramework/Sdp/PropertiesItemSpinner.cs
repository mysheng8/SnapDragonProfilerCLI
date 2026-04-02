using System;

namespace Sdp
{
	// Token: 0x020001DC RID: 476
	public class PropertiesItemSpinner : PropertiesItem
	{
		// Token: 0x06000682 RID: 1666 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
		public PropertiesItemSpinner()
		{
			base.ItemType = ItemType.Spinner;
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0000FDE7 File Offset: 0x0000DFE7
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x0000FDEF File Offset: 0x0000DFEF
		public double Min { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0000FDF8 File Offset: 0x0000DFF8
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x0000FE00 File Offset: 0x0000E000
		public double Max { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0000FE09 File Offset: 0x0000E009
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x0000FE11 File Offset: 0x0000E011
		public double Step { get; set; }
	}
}
