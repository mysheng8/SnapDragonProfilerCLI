using System;

namespace Sdp
{
	// Token: 0x02000243 RID: 579
	public class GroupLayoutConfigDesc
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x0001B8AA File Offset: 0x00019AAA
		// (set) Token: 0x06000958 RID: 2392 RVA: 0x0001B8B2 File Offset: 0x00019AB2
		public string Name { get; set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0001B8BB File Offset: 0x00019ABB
		// (set) Token: 0x0600095A RID: 2394 RVA: 0x0001B8C3 File Offset: 0x00019AC3
		public GroupViewDescList Groups { get; set; }
	}
}
