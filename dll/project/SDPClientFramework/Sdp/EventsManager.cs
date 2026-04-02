using System;

namespace Sdp
{
	// Token: 0x0200028B RID: 651
	public class EventsManager
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00022A43 File Offset: 0x00020C43
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00022A4B File Offset: 0x00020C4B
		public ConnectionEvents ConnectionEvents { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00022A54 File Offset: 0x00020C54
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00022A5C File Offset: 0x00020C5C
		public DeviceEvents DeviceEvents { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00022A65 File Offset: 0x00020C65
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x00022A6D File Offset: 0x00020C6D
		public ICEvents ICEvents { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00022A76 File Offset: 0x00020C76
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x00022A7E File Offset: 0x00020C7E
		public DataExplorerViewEvents DataExplorerViewEvents { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00022A87 File Offset: 0x00020C87
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x00022A8F File Offset: 0x00020C8F
		public ResourceViewEvents ResourceViewEvents { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00022A98 File Offset: 0x00020C98
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x00022AA0 File Offset: 0x00020CA0
		public SourceViewEvents SourceViewEvents { get; set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00022AA9 File Offset: 0x00020CA9
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x00022AB1 File Offset: 0x00020CB1
		public ImageViewEvents ImageViewEvents { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00022ABA File Offset: 0x00020CBA
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x00022AC2 File Offset: 0x00020CC2
		public PluginEvents PluginEvents { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00022ACB File Offset: 0x00020CCB
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x00022AD3 File Offset: 0x00020CD3
		public TimeEventsCollection TimeEventsCollection { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00022ADC File Offset: 0x00020CDC
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x00022AE4 File Offset: 0x00020CE4
		public InspectorViewEvents InspectorViewEvents { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x00022AED File Offset: 0x00020CED
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x00022AF5 File Offset: 0x00020CF5
		public OptionsViewEvents OptionsViewEvents { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00022AFE File Offset: 0x00020CFE
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x00022B06 File Offset: 0x00020D06
		public ContextViewEvents ContextViewEvents { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x00022B0F File Offset: 0x00020D0F
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x00022B17 File Offset: 0x00020D17
		public ClientEvents ClientEvents { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x00022B20 File Offset: 0x00020D20
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x00022B28 File Offset: 0x00020D28
		public ProgramViewEvents ProgramViewEvents { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00022B31 File Offset: 0x00020D31
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x00022B39 File Offset: 0x00020D39
		public ScreenCaptureViewEvents ScreenCaptureViewEvents { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x00022B42 File Offset: 0x00020D42
		// (set) Token: 0x06000BAF RID: 2991 RVA: 0x00022B4A File Offset: 0x00020D4A
		public ShaderAnalyzerEvents ShaderAnalyzerEvents { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00022B53 File Offset: 0x00020D53
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x00022B5B File Offset: 0x00020D5B
		public DrawCallDataViewEvents DrawCallDataViewEvents { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x00022B64 File Offset: 0x00020D64
		// (set) Token: 0x06000BB3 RID: 2995 RVA: 0x00022B6C File Offset: 0x00020D6C
		public FrameStatsEvents FrameStatsEvents { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x00022B75 File Offset: 0x00020D75
		// (set) Token: 0x06000BB5 RID: 2997 RVA: 0x00022B7D File Offset: 0x00020D7D
		public ProgressEvents ProgressEvents { get; set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x00022B86 File Offset: 0x00020D86
		// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x00022B8E File Offset: 0x00020D8E
		public PixelHistoryEvents PixelHistoryEvents { get; set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x00022B97 File Offset: 0x00020D97
		// (set) Token: 0x06000BB9 RID: 3001 RVA: 0x00022B9F File Offset: 0x00020D9F
		public BufferViewerEvents BufferViewerEvents { get; set; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x00022BA8 File Offset: 0x00020DA8
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x00022BB0 File Offset: 0x00020DB0
		public DataSourceViewEvents DataSourceViewEvents { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00022BB9 File Offset: 0x00020DB9
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x00022BC1 File Offset: 0x00020DC1
		public MetricEvents MetricEvents { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00022BCA File Offset: 0x00020DCA
		// (set) Token: 0x06000BBF RID: 3007 RVA: 0x00022BD2 File Offset: 0x00020DD2
		public SamplingEvents SamplingEvents { get; set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00022BDB File Offset: 0x00020DDB
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x00022BE3 File Offset: 0x00020DE3
		public StatisticEvents StatisticEvents { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x00022BEC File Offset: 0x00020DEC
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x00022BF4 File Offset: 0x00020DF4
		public SnapshotEvents SnapshotEvents { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x00022BFD File Offset: 0x00020DFD
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x00022C05 File Offset: 0x00020E05
		public AnalyticsEvents AnalyticsEvents { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x00022C0E File Offset: 0x00020E0E
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x00022C16 File Offset: 0x00020E16
		public RooflineEvents RooflineEvents { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00022C1F File Offset: 0x00020E1F
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x00022C27 File Offset: 0x00020E27
		public Viewer3DEvents Viewer3DEvents { get; set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x00022C30 File Offset: 0x00020E30
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x00022C38 File Offset: 0x00020E38
		public DebugMarkerViewEvents DebugMarkerViewEvents { get; set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00022C41 File Offset: 0x00020E41
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x00022C49 File Offset: 0x00020E49
		public TensorViewEvents TensorViewEvents { get; set; }

		// Token: 0x06000BCE RID: 3022 RVA: 0x00022C54 File Offset: 0x00020E54
		public EventsManager()
		{
			this.ConnectionEvents = new ConnectionEvents();
			this.DeviceEvents = new DeviceEvents();
			this.ICEvents = new ICEvents();
			this.DataExplorerViewEvents = new DataExplorerViewEvents();
			this.ResourceViewEvents = new ResourceViewEvents();
			this.SourceViewEvents = new SourceViewEvents();
			this.ImageViewEvents = new ImageViewEvents();
			this.PluginEvents = new PluginEvents();
			this.TimeEventsCollection = new TimeEventsCollection();
			this.InspectorViewEvents = new InspectorViewEvents();
			this.OptionsViewEvents = new OptionsViewEvents();
			this.ContextViewEvents = new ContextViewEvents();
			this.ClientEvents = new ClientEvents();
			this.ProgramViewEvents = new ProgramViewEvents();
			this.ScreenCaptureViewEvents = new ScreenCaptureViewEvents();
			this.ShaderAnalyzerEvents = new ShaderAnalyzerEvents();
			this.DrawCallDataViewEvents = new DrawCallDataViewEvents();
			this.FrameStatsEvents = new FrameStatsEvents();
			this.ProgressEvents = new ProgressEvents();
			this.PixelHistoryEvents = new PixelHistoryEvents();
			this.BufferViewerEvents = new BufferViewerEvents();
			this.DataSourceViewEvents = new DataSourceViewEvents();
			this.MetricEvents = new MetricEvents();
			this.SamplingEvents = new SamplingEvents();
			this.StatisticEvents = new StatisticEvents();
			this.SnapshotEvents = new SnapshotEvents();
			this.AnalyticsEvents = new AnalyticsEvents();
			this.RooflineEvents = new RooflineEvents();
			this.Viewer3DEvents = new Viewer3DEvents();
			this.DebugMarkerViewEvents = new DebugMarkerViewEvents();
			this.TensorViewEvents = new TensorViewEvents();
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00022DBC File Offset: 0x00020FBC
		public void Raise(EventHandler e, object sender, EventArgs args)
		{
			if (e != null)
			{
				e(sender, args);
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00022DC9 File Offset: 0x00020FC9
		public void Raise<T>(EventHandler<T> e, object sender, T args) where T : EventArgs
		{
			if (e != null)
			{
				e(sender, args);
			}
		}
	}
}
