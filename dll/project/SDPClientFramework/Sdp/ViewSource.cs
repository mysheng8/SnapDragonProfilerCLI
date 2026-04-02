using System;

namespace Sdp
{
	// Token: 0x020000A8 RID: 168
	public class ViewSource
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000334 RID: 820 RVA: 0x000097CD File Offset: 0x000079CD
		// (set) Token: 0x06000335 RID: 821 RVA: 0x000097D5 File Offset: 0x000079D5
		public int CaptureID { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000336 RID: 822 RVA: 0x000097DE File Offset: 0x000079DE
		// (set) Token: 0x06000337 RID: 823 RVA: 0x000097E6 File Offset: 0x000079E6
		public int SourceID { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000338 RID: 824 RVA: 0x000097EF File Offset: 0x000079EF
		// (set) Token: 0x06000339 RID: 825 RVA: 0x000097F7 File Offset: 0x000079F7
		public string SourceName { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00009800 File Offset: 0x00007A00
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00009808 File Offset: 0x00007A08
		public string UniqueID { get; set; }
	}
}
