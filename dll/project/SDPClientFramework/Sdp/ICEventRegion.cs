using System;

namespace Sdp
{
	// Token: 0x02000215 RID: 533
	public class ICEventRegion
	{
		// Token: 0x060007DA RID: 2010 RVA: 0x000157DD File Offset: 0x000139DD
		public ICEventRegion()
		{
			this.Complete = false;
		}

		// Token: 0x0400077F RID: 1919
		public long StartTimestamp;

		// Token: 0x04000780 RID: 1920
		public long EndTimestamp;

		// Token: 0x04000781 RID: 1921
		public uint NameId;

		// Token: 0x04000782 RID: 1922
		public bool Complete;
	}
}
