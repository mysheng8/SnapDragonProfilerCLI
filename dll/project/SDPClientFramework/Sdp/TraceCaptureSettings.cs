using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000220 RID: 544
	public class TraceCaptureSettings
	{
		// Token: 0x0600084B RID: 2123 RVA: 0x0001679C File Offset: 0x0001499C
		public TraceCaptureSettings()
		{
			this.StartCaptureTriggers = new List<TraceCaptureTrigger>();
			this.EndCaptureTriggers = new List<TraceCaptureTrigger>();
		}

		// Token: 0x040007BB RID: 1979
		public TraceCaptureSettings.TraceCaptureType StartCaptureType;

		// Token: 0x040007BC RID: 1980
		public TraceCaptureSettings.TraceCaptureType EndCaptureType;

		// Token: 0x040007BD RID: 1981
		public bool IsTimoutEnabled;

		// Token: 0x040007BE RID: 1982
		public ulong Timeout;

		// Token: 0x040007BF RID: 1983
		public bool IsNumberOfFramesEnabled;

		// Token: 0x040007C0 RID: 1984
		public int NumFrames;

		// Token: 0x040007C1 RID: 1985
		public bool IsClReleaseContextEnabled;

		// Token: 0x040007C2 RID: 1986
		public bool IsTriggersEnabled;

		// Token: 0x040007C3 RID: 1987
		public List<TraceCaptureTrigger> StartCaptureTriggers;

		// Token: 0x040007C4 RID: 1988
		public List<TraceCaptureTrigger> EndCaptureTriggers;

		// Token: 0x020003A3 RID: 931
		public enum TraceCaptureType
		{
			// Token: 0x04000CD9 RID: 3289
			Manual,
			// Token: 0x04000CDA RID: 3290
			Automated
		}
	}
}
