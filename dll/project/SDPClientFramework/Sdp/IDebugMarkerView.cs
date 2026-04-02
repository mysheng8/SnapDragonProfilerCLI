using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020002C2 RID: 706
	public interface IDebugMarkerView : IView
	{
		// Token: 0x06000E76 RID: 3702
		void AddDebugMarkerVMList(List<DebugMarkerViewModel> list);

		// Token: 0x06000E77 RID: 3703
		void Clear();
	}
}
