using System;

namespace Sdp
{
	// Token: 0x020000E7 RID: 231
	public class InspectorViewDisplayEventArgs : EventArgs
	{
		// Token: 0x04000341 RID: 833
		public PropertyGridDescriptionObject Content;

		// Token: 0x04000342 RID: 834
		public string Description = "";

		// Token: 0x04000343 RID: 835
		public uint CaptureID;

		// Token: 0x04000344 RID: 836
		public bool ShowASButton;

		// Token: 0x04000345 RID: 837
		public bool ShowTensorButton;
	}
}
