using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200011D RID: 285
	public class DrawBuffersSetEventArgs : EventArgs
	{
		// Token: 0x040003FE RID: 1022
		public Dictionary<uint, List<uint>> BufferIds;
	}
}
