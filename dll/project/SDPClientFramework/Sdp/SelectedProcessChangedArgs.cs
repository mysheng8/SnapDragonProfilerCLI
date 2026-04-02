using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200022D RID: 557
	public class SelectedProcessChangedArgs : EventArgs
	{
		// Token: 0x060008DD RID: 2269 RVA: 0x0001A5E7 File Offset: 0x000187E7
		public SelectedProcessChangedArgs(List<IdNamePair> selectedProcesses)
		{
			this.SelectedProcesses = selectedProcesses;
		}

		// Token: 0x040007E0 RID: 2016
		public readonly List<IdNamePair> SelectedProcesses;
	}
}
