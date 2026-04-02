using System;

namespace Sdp
{
	// Token: 0x02000245 RID: 581
	public class GroupLayoutViewDesc : ViewDesc
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0001B8D4 File Offset: 0x00019AD4
		// (set) Token: 0x0600095E RID: 2398 RVA: 0x0001B8DC File Offset: 0x00019ADC
		public string ActiveConfig { get; set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0001B8E5 File Offset: 0x00019AE5
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x0001B8ED File Offset: 0x00019AED
		public GroupLayoutConfigDescList Configs { get; set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x0001B8F6 File Offset: 0x00019AF6
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x0001B8FE File Offset: 0x00019AFE
		public bool DataSourcesVisible { get; set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0001B907 File Offset: 0x00019B07
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x0001B90F File Offset: 0x00019B0F
		public CaptureType CaptureType { get; set; }
	}
}
