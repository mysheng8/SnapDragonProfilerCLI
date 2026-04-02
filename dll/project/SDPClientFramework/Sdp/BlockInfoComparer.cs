using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001C7 RID: 455
	public class BlockInfoComparer : IComparer<BlockInfo>
	{
		// Token: 0x060005DB RID: 1499 RVA: 0x0000D908 File Offset: 0x0000BB08
		public int Compare(BlockInfo x, BlockInfo y)
		{
			int num = x.BlockValue.CompareTo(y.BlockValue);
			if (num == 0)
			{
				return x.BlockTitle.CompareTo(y.BlockTitle);
			}
			return num;
		}
	}
}
