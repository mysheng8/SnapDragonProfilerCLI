using System;

namespace Sdp
{
	// Token: 0x0200027E RID: 638
	public class CodeItem
	{
		// Token: 0x06000B01 RID: 2817 RVA: 0x000203C0 File Offset: 0x0001E5C0
		public override bool Equals(object b)
		{
			if (b is CodeItem)
			{
				CodeItem codeItem = (CodeItem)b;
				if (codeItem.Id == this.Id && codeItem.SourceID == this.SourceID && codeItem.CaptureID == this.CaptureID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00020409 File Offset: 0x0001E609
		public override int GetHashCode()
		{
			return this.SourceID.GetHashCode() ^ this.Id.GetHashCode() ^ this.CaptureID.GetHashCode();
		}

		// Token: 0x040008AA RID: 2218
		public int SourceID;

		// Token: 0x040008AB RID: 2219
		public int CaptureID;

		// Token: 0x040008AC RID: 2220
		public uint Id;
	}
}
