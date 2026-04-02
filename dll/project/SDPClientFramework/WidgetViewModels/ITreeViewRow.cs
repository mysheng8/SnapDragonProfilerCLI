using System;
using System.Collections.Generic;

namespace SDPClientFramework.WidgetViewModels
{
	// Token: 0x02000017 RID: 23
	public interface ITreeViewRow<T> where T : new()
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000048 RID: 72
		T Value { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000049 RID: 73
		IEnumerable<ITreeViewRow<T>> Children { get; }
	}
}
