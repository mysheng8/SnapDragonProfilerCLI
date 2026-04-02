using System;

namespace Sdp
{
	// Token: 0x020002C5 RID: 709
	public interface IDrawCallDataView : IView
	{
		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x06000E80 RID: 3712
		// (remove) Token: 0x06000E81 RID: 3713
		event EventHandler ExportVertexDataClicked;

		// Token: 0x06000E82 RID: 3714
		void SetStatus(StatusType statusType, string status, int duration);

		// Token: 0x06000E83 RID: 3715
		void InvalidateView(TreeModel indexModel, TreeModel elementsModel, TreeModel vertexModel);
	}
}
