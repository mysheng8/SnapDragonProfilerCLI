using System;

namespace Sdp
{
	// Token: 0x02000097 RID: 151
	public class ProcessWithWarningSelectedEventArgs : EventArgs
	{
		// Token: 0x04000212 RID: 530
		public ProcessWarnings WarningType;

		// Token: 0x04000213 RID: 531
		public uint Pid;

		// Token: 0x04000214 RID: 532
		public CaptureType CaptureType;
	}
}
