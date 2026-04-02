using System;

namespace Sdp
{
	// Token: 0x020001B7 RID: 439
	public class MetricViewModel : IEquatable<MetricViewModel>
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x0000D5EB File Offset: 0x0000B7EB
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0000D5F3 File Offset: 0x0000B7F3
		public bool Enabled { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x0000D604 File Offset: 0x0000B804
		public string Name { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0000D60D File Offset: 0x0000B80D
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x0000D615 File Offset: 0x0000B815
		public uint ID { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0000D61E File Offset: 0x0000B81E
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x0000D626 File Offset: 0x0000B826
		public bool Active { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0000D62F File Offset: 0x0000B82F
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x0000D637 File Offset: 0x0000B837
		public MetricColor Color { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0000D640 File Offset: 0x0000B840
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x0000D648 File Offset: 0x0000B848
		public string Tooltip { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0000D651 File Offset: 0x0000B851
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x0000D659 File Offset: 0x0000B859
		public bool Hidden { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0000D662 File Offset: 0x0000B862
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x0000D66A File Offset: 0x0000B86A
		public string DisplayName { get; set; }

		// Token: 0x060005A7 RID: 1447 RVA: 0x0000D674 File Offset: 0x0000B874
		public bool Equals(MetricViewModel other)
		{
			return other != null && (this == other || (this.Enabled == other.Enabled && this.Name == other.Name && this.ID == other.ID && this.Active == other.Active && object.Equals(this.Color, other.Color) && this.Tooltip == other.Tooltip && this.Hidden == other.Hidden && this.DisplayName == other.DisplayName));
		}
	}
}
