using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200009A RID: 154
	public class InvalidateDebugMarkerViewEventArgs : EventArgs
	{
		// Token: 0x0400021A RID: 538
		public List<DebugMarkerViewModel> DebugMarkerModel = new List<DebugMarkerViewModel>();
	}
}
