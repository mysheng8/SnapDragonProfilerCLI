using System;
using System.Collections.Generic;
using Sdp.Functional;
using SDPClientFramework.Views.EventHandlers.KeyboardEventHandler;

namespace Sdp
{
	// Token: 0x02000293 RID: 659
	public interface IGroupLayoutView : IView, IKeyboardEventHandler
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000C07 RID: 3079
		// (set) Token: 0x06000C06 RID: 3078
		Maybe<InteractionMode> InteractionMode { get; set; }

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06000C08 RID: 3080
		// (remove) Token: 0x06000C09 RID: 3081
		event EventHandler InteractionModeUpdated;

		// Token: 0x17000271 RID: 625
		// (set) Token: 0x06000C0A RID: 3082
		bool DataSourcesVisible { set; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000C0B RID: 3083
		IDataSourcesView DataSourcesView { get; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000C0C RID: 3084
		ITraceSummaryView TraceSummaryView { get; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000C0D RID: 3085
		ITimelineView TimelineView { get; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000C0E RID: 3086
		IDiagramView PieView { get; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000C0F RID: 3087
		IDiagramView BlockFlowView { get; }

		// Token: 0x06000C10 RID: 3088
		void HideDataSourcesPanel();

		// Token: 0x06000C11 RID: 3089
		void AddTraceSummaryPanel();

		// Token: 0x06000C12 RID: 3090
		IGroupView AddGroup(string title);

		// Token: 0x06000C13 RID: 3091
		void RemoveGroup(IGroupView group);

		// Token: 0x06000C14 RID: 3092
		void SetViewType(GroupLayoutStyleType viewType);

		// Token: 0x17000277 RID: 631
		// (set) Token: 0x06000C15 RID: 3093
		bool NewCaptureButtonVisible { set; }

		// Token: 0x17000278 RID: 632
		// (set) Token: 0x06000C16 RID: 3094
		bool ZoomButtonsVisible { set; }

		// Token: 0x17000279 RID: 633
		// (set) Token: 0x06000C17 RID: 3095
		bool ToolbarTraceItemsVisible { set; }

		// Token: 0x1700027A RID: 634
		// (set) Token: 0x06000C18 RID: 3096
		bool PausingVisible { set; }

		// Token: 0x1700027B RID: 635
		// (set) Token: 0x06000C19 RID: 3097
		bool PausingEnabled { set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000C1A RID: 3098
		// (set) Token: 0x06000C1B RID: 3099
		bool IsPaused { get; set; }

		// Token: 0x1700027D RID: 637
		// (set) Token: 0x06000C1C RID: 3100
		bool ScrollLockVisible { set; }

		// Token: 0x1700027E RID: 638
		// (set) Token: 0x06000C1D RID: 3101
		bool ScrollLockEnabled { set; }

		// Token: 0x1700027F RID: 639
		// (set) Token: 0x06000C1E RID: 3102
		bool ScrollLockToggled { set; }

		// Token: 0x17000280 RID: 640
		// (set) Token: 0x06000C1F RID: 3103
		bool CaptureButtonVisible { set; }

		// Token: 0x17000281 RID: 641
		// (set) Token: 0x06000C20 RID: 3104
		bool CaptureButtonEnabled { set; }

		// Token: 0x17000282 RID: 642
		// (set) Token: 0x06000C21 RID: 3105
		bool CaptureButtonActive { set; }

		// Token: 0x17000283 RID: 643
		// (set) Token: 0x06000C22 RID: 3106
		bool ExportButtonVisible { set; }

		// Token: 0x17000284 RID: 644
		// (set) Token: 0x06000C23 RID: 3107
		bool StatisticsButtonEnabled { set; }

		// Token: 0x17000285 RID: 645
		// (set) Token: 0x06000C24 RID: 3108
		int DataSourcesWidth { set; }

		// Token: 0x17000286 RID: 646
		// (set) Token: 0x06000C25 RID: 3109
		bool FixedScale { set; }

		// Token: 0x06000C26 RID: 3110
		void UpdateActiveMetricComboBox(HashSet<string> valuesToAdd);

		// Token: 0x06000C27 RID: 3111
		void SetName(CaptureType capturType, int sessionNumber);

		// Token: 0x06000C28 RID: 3112
		void SetExportTootlTip(string tooltip);

		// Token: 0x06000C29 RID: 3113
		void SetExportButtonInteractable(bool active);

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000C2A RID: 3114
		int CurrentScrollPos { get; }

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06000C2B RID: 3115
		// (remove) Token: 0x06000C2C RID: 3116
		event EventHandler<FilePathEventArgs> ExportButtonToggled;

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06000C2D RID: 3117
		// (remove) Token: 0x06000C2E RID: 3118
		event EventHandler NewCaptureButtonClicked;

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06000C2F RID: 3119
		// (remove) Token: 0x06000C30 RID: 3120
		event EventHandler<AddGroupEventArgs> AddGroupActivated;

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06000C31 RID: 3121
		// (remove) Token: 0x06000C32 RID: 3122
		event EventHandler ExpandAllActivated;

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06000C33 RID: 3123
		// (remove) Token: 0x06000C34 RID: 3124
		event EventHandler CollapseAllActivated;

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06000C35 RID: 3125
		// (remove) Token: 0x06000C36 RID: 3126
		event EventHandler StatisticsActivated;

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06000C37 RID: 3127
		// (remove) Token: 0x06000C38 RID: 3128
		event EventHandler<ViewBoundsEventArgs> ZoomIn;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06000C39 RID: 3129
		// (remove) Token: 0x06000C3A RID: 3130
		event EventHandler<ViewBoundsEventArgs> ZoomOut;

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06000C3B RID: 3131
		// (remove) Token: 0x06000C3C RID: 3132
		event EventHandler<ViewBoundsEventArgs> PanLeft;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06000C3D RID: 3133
		// (remove) Token: 0x06000C3E RID: 3134
		event EventHandler<ViewBoundsEventArgs> PanRight;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06000C3F RID: 3135
		// (remove) Token: 0x06000C40 RID: 3136
		event EventHandler ResetViewBounds;

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06000C41 RID: 3137
		// (remove) Token: 0x06000C42 RID: 3138
		event EventHandler PauseClicked;

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06000C43 RID: 3139
		// (remove) Token: 0x06000C44 RID: 3140
		event EventHandler ResumeClicked;

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06000C45 RID: 3141
		// (remove) Token: 0x06000C46 RID: 3142
		event EventHandler<ScrollLockEventArgs> ScrollLockClicked;

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06000C47 RID: 3143
		// (remove) Token: 0x06000C48 RID: 3144
		event EventHandler<AutoScaleEventArgs> AutoScaleToggled;

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06000C49 RID: 3145
		// (remove) Token: 0x06000C4A RID: 3146
		event EventHandler<MetricDroppedEventArgs> MetricDropped;

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06000C4B RID: 3147
		// (remove) Token: 0x06000C4C RID: 3148
		event EventHandler<MetricDroppedEventArgs> CategoryDropped;

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06000C4D RID: 3149
		// (remove) Token: 0x06000C4E RID: 3150
		event EventHandler MetricDataEntered;

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x06000C4F RID: 3151
		// (remove) Token: 0x06000C50 RID: 3152
		event EventHandler MetricDataLeft;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x06000C51 RID: 3153
		// (remove) Token: 0x06000C52 RID: 3154
		event EventHandler<TracksWidthChangedEventArgs> TracksWidthChanged;

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06000C53 RID: 3155
		// (remove) Token: 0x06000C54 RID: 3156
		event EventHandler<IGroupView> GroupViewAdded;

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06000C55 RID: 3157
		// (remove) Token: 0x06000C56 RID: 3158
		event EventHandler<ComboBoxUpdatedEventArgs> ComboBoxUpdated;

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x06000C57 RID: 3159
		// (remove) Token: 0x06000C58 RID: 3160
		event EventHandler<TakeCaptureArgs> CaptureButtonToggled;
	}
}
