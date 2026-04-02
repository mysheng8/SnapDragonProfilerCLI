using System;

namespace Sdp
{
	// Token: 0x02000111 RID: 273
	public class SortItem
	{
		// Token: 0x060003AB RID: 939 RVA: 0x0000204B File Offset: 0x0000024B
		public SortItem()
		{
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00009DC8 File Offset: 0x00007FC8
		public SortItem(string name, int column, SortItem.Sort order)
		{
			this.Name = name;
			this.Column = column;
			this.Order = order;
		}

		// Token: 0x040003D1 RID: 977
		public string Name;

		// Token: 0x040003D2 RID: 978
		public int Column;

		// Token: 0x040003D3 RID: 979
		public SortItem.Sort Order;

		// Token: 0x0200036D RID: 877
		public enum Sort
		{
			// Token: 0x04000C06 RID: 3078
			Ascending,
			// Token: 0x04000C07 RID: 3079
			Descending
		}
	}
}
