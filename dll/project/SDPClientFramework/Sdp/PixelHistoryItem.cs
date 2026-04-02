using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000269 RID: 617
	public class PixelHistoryItem
	{
		// Token: 0x04000869 RID: 2153
		public int DrawcallID;

		// Token: 0x0400086A RID: 2154
		public readonly List<PixelHistoryColor> Colors = new List<PixelHistoryColor>();

		// Token: 0x0400086B RID: 2155
		public readonly List<PixelHistoryFailReason> FailReasons = new List<PixelHistoryFailReason>();
	}
}
