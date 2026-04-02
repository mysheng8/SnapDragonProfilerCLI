using System;
using System.Collections.Generic;

namespace SDPClientFramework.WidgetViewModels
{
	// Token: 0x02000016 RID: 22
	public interface ITreeViewModel<T> where T : new()
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000047 RID: 71
		IEnumerable<ITreeViewRow<T>> Rows { get; }
	}
}
