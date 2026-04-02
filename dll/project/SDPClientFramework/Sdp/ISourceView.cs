using System;

namespace Sdp
{
	// Token: 0x0200027B RID: 635
	public interface ISourceView : IView
	{
		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06000AF1 RID: 2801
		// (remove) Token: 0x06000AF2 RID: 2802
		event EventHandler<SourceWindowClosedArgs> WindowClosed;

		// Token: 0x06000AF3 RID: 2803
		void FocusOnWindow(CodeItem tracker);

		// Token: 0x06000AF4 RID: 2804
		void Add(CodeItem item, string title, string code);

		// Token: 0x06000AF5 RID: 2805
		void MakeButtonsSensitive();

		// Token: 0x06000AF6 RID: 2806
		void MakeButtonsInsensitive();
	}
}
