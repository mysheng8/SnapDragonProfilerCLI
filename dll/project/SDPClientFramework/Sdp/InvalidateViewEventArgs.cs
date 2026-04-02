using System;

namespace Sdp
{
	// Token: 0x0200009D RID: 157
	public class InvalidateViewEventArgs : EventArgs
	{
		// Token: 0x04000227 RID: 551
		public TreeModel VertexBufferModel;

		// Token: 0x04000228 RID: 552
		public TreeModel InidicesModel;

		// Token: 0x04000229 RID: 553
		public uint drawMode;
	}
}
