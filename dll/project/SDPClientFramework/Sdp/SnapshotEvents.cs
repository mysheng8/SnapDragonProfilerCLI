using System;

namespace Sdp
{
	// Token: 0x02000137 RID: 311
	public class SnapshotEvents
	{
		// Token: 0x0400046D RID: 1133
		public EventHandler<SnapshotProviderChangedArgs> SnapshotProviderChanged;

		// Token: 0x0400046E RID: 1134
		public EventHandler CurrentSnapshotControllerChanged;

		// Token: 0x0400046F RID: 1135
		public EventHandler<SelectedProcessChangedArgs> SelectedProcessChanged;

		// Token: 0x04000470 RID: 1136
		public EventHandler<SnapshotRequestArgs> CaptureClicked;

		// Token: 0x04000471 RID: 1137
		public EventHandler<AddSnapshotModeArgs> AddAvailableSnapshotMode;

		// Token: 0x04000472 RID: 1138
		public EventHandler<DCAPLoadedArgs> DCAPLoaded;

		// Token: 0x04000473 RID: 1139
		public EventHandler<AddMainWindowErrorMessageArgs> AddMainWindowErrorMessage;
	}
}
