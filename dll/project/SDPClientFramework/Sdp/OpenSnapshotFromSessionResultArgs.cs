using System;

namespace Sdp
{
	// Token: 0x020000B6 RID: 182
	public class OpenSnapshotFromSessionResultArgs : EventArgs
	{
		// Token: 0x04000297 RID: 663
		public string Warning;

		// Token: 0x04000298 RID: 664
		public string Error;

		// Token: 0x04000299 RID: 665
		public bool Handled;

		// Token: 0x0400029A RID: 666
		public uint SnapshotProviderID;

		// Token: 0x0400029B RID: 667
		public string SourceDisplayName;
	}
}
