using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001C8 RID: 456
	public class SortedBlockInfoSet : SortedSet<BlockInfo>
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x0000D93D File Offset: 0x0000BB3D
		public SortedBlockInfoSet()
			: base(new BlockInfoComparer())
		{
		}
	}
}
