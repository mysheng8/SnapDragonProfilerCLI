using System;

namespace Sdp
{
	// Token: 0x02000241 RID: 577
	public class GroupViewDesc
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x0001B86F File Offset: 0x00019A6F
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x0001B877 File Offset: 0x00019A77
		public string Name { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0001B880 File Offset: 0x00019A80
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x0001B888 File Offset: 0x00019A88
		public bool IsDocked { get; set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0001B891 File Offset: 0x00019A91
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x0001B899 File Offset: 0x00019A99
		public TrackViewDescList Tracks { get; set; }
	}
}
