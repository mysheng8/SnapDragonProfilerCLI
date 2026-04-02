using System;

namespace Sdp
{
	// Token: 0x02000115 RID: 277
	public class NodeSelectedEventArgs : EventArgs
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00009DE5 File Offset: 0x00007FE5
		public NodeSelectedEventArgs(ulong stackID, uint captureID)
		{
			this.StackID = stackID;
			this.CaptureID = captureID;
		}

		// Token: 0x040003DE RID: 990
		public ulong StackID;

		// Token: 0x040003DF RID: 991
		public uint CaptureID;
	}
}
