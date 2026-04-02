using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200016D RID: 365
	public class Viewer3DDisplayASArgs : EventArgs
	{
		// Token: 0x04000540 RID: 1344
		public Tuple<ulong, byte[]> Tlas;

		// Token: 0x04000541 RID: 1345
		public Dictionary<ulong, byte[]> Blases;
	}
}
