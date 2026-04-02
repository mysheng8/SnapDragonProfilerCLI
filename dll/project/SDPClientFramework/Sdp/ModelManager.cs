using System;

namespace Sdp
{
	// Token: 0x0200028D RID: 653
	public class ModelManager : IModelManager
	{
		// Token: 0x06000BD2 RID: 3026 RVA: 0x00022DD8 File Offset: 0x00020FD8
		public ModelManager()
		{
			this.ApplicationModel = new ApplicationModel();
			this.TraceModel = new TraceModel();
			this.ConnectionModel = new ConnectionModel();
			this.ContextModel = new ContextModel();
			this.SettingsModel = new SettingsModel();
			this.DataSourcesModel = new DataSourcesModel(this.SettingsModel);
			this.InstrumentedCodeModel = new InstrumentedCodeModel();
			this.TimeModelCollection = new TimeModelCollection();
			this.PreviewMetricsModel = new PreviewMetricsModel();
			this.SnapshotModel = new SnapshotModel();
			this.DataExplorerViewModel = new DataExplorerViewModel();
			this.RealtimeModel = new RealtimeModel();
			this.SamplingModel = new SamplingModel();
			this.ResourcesViewModel = new ResourcesViewModel();
			this.StatisticsModel = new StatisticsModel();
			this.RooflineModel = new RooflineModel();
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x00022EA1 File Offset: 0x000210A1
		// (set) Token: 0x06000BD4 RID: 3028 RVA: 0x00022EA9 File Offset: 0x000210A9
		public ApplicationModel ApplicationModel { get; set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x00022EB2 File Offset: 0x000210B2
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x00022EBA File Offset: 0x000210BA
		public ConnectionModel ConnectionModel { get; set; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x00022EC3 File Offset: 0x000210C3
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x00022ECB File Offset: 0x000210CB
		public ContextModel ContextModel { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x00022ED4 File Offset: 0x000210D4
		// (set) Token: 0x06000BDA RID: 3034 RVA: 0x00022EDC File Offset: 0x000210DC
		public InstrumentedCodeModel InstrumentedCodeModel { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x00022EE5 File Offset: 0x000210E5
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x00022EED File Offset: 0x000210ED
		public TimeModelCollection TimeModelCollection { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x00022EF6 File Offset: 0x000210F6
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x00022EFE File Offset: 0x000210FE
		public SettingsModel SettingsModel { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x00022F07 File Offset: 0x00021107
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x00022F0F File Offset: 0x0002110F
		public PreviewMetricsModel PreviewMetricsModel { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x00022F18 File Offset: 0x00021118
		// (set) Token: 0x06000BE2 RID: 3042 RVA: 0x00022F20 File Offset: 0x00021120
		public DataSourcesModel DataSourcesModel { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00022F29 File Offset: 0x00021129
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x00022F31 File Offset: 0x00021131
		public SnapshotModel SnapshotModel { get; set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00022F3A File Offset: 0x0002113A
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x00022F42 File Offset: 0x00021142
		public DataExplorerViewModel DataExplorerViewModel { get; set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00022F4B File Offset: 0x0002114B
		// (set) Token: 0x06000BE8 RID: 3048 RVA: 0x00022F53 File Offset: 0x00021153
		public TraceModel TraceModel { get; set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00022F5C File Offset: 0x0002115C
		// (set) Token: 0x06000BEA RID: 3050 RVA: 0x00022F64 File Offset: 0x00021164
		public RealtimeModel RealtimeModel { get; set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00022F6D File Offset: 0x0002116D
		// (set) Token: 0x06000BEC RID: 3052 RVA: 0x00022F75 File Offset: 0x00021175
		public SamplingModel SamplingModel { get; set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x00022F7E File Offset: 0x0002117E
		// (set) Token: 0x06000BEE RID: 3054 RVA: 0x00022F86 File Offset: 0x00021186
		public ResourcesViewModel ResourcesViewModel { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x00022F8F File Offset: 0x0002118F
		// (set) Token: 0x06000BF0 RID: 3056 RVA: 0x00022F97 File Offset: 0x00021197
		public StatisticsModel StatisticsModel { get; set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x00022FA0 File Offset: 0x000211A0
		// (set) Token: 0x06000BF2 RID: 3058 RVA: 0x00022FA8 File Offset: 0x000211A8
		public RooflineModel RooflineModel { get; set; }
	}
}
