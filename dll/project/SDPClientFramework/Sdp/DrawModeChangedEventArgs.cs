using System;

namespace Sdp
{
	// Token: 0x02000120 RID: 288
	public class DrawModeChangedEventArgs : EventArgs
	{
		// Token: 0x0400040C RID: 1036
		public uint SelectedContext;

		// Token: 0x0400040D RID: 1037
		public ScreenCaptureViewDrawMode DrawMode;

		// Token: 0x0400040E RID: 1038
		public ScreenCaptureTargetSelection Target;

		// Token: 0x0400040F RID: 1039
		public bool TriggerReplay;
	}
}
