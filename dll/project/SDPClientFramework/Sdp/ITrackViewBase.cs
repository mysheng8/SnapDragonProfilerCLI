using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cairo;

namespace Sdp
{
	// Token: 0x0200029E RID: 670
	public interface ITrackViewBase
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000C63 RID: 3171
		// (set) Token: 0x06000C64 RID: 3172
		int ExpandedHeight { get; set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000C65 RID: 3173
		// (set) Token: 0x06000C66 RID: 3174
		int MinimumHeight { get; set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000C67 RID: 3175
		// (set) Token: 0x06000C68 RID: 3176
		bool IsExpanded { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000C69 RID: 3177
		// (set) Token: 0x06000C6A RID: 3178
		bool IsSettingsShown { get; set; }

		// Token: 0x06000C6B RID: 3179
		void ExpandCollapse();

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000C6C RID: 3180
		// (set) Token: 0x06000C6D RID: 3181
		bool CanExpandCollapse { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000C6E RID: 3182
		// (set) Token: 0x06000C6F RID: 3183
		bool ShowSettingsButton { get; set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000C70 RID: 3184
		// (set) Token: 0x06000C71 RID: 3185
		Color ControlPanelHeaderBackColor { get; set; }

		// Token: 0x1700028F RID: 655
		// (set) Token: 0x06000C72 RID: 3186
		List<long> SelectedBookmarkTimestamps { set; }

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x06000C73 RID: 3187
		// (remove) Token: 0x06000C74 RID: 3188
		event EventHandler ExpandCollapseClicked;

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x06000C75 RID: 3189
		// (remove) Token: 0x06000C76 RID: 3190
		event EventHandler RemoveClicked;

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06000C77 RID: 3191
		// (remove) Token: 0x06000C78 RID: 3192
		event EventHandler SettingsClicked;

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x06000C79 RID: 3193
		// (remove) Token: 0x06000C7A RID: 3194
		event EventHandler DragDataEntered;

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06000C7B RID: 3195
		// (remove) Token: 0x06000C7C RID: 3196
		event EventHandler DragDataLeft;

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06000C7D RID: 3197
		// (remove) Token: 0x06000C7E RID: 3198
		event EventHandler<ResizeTrackRequestEventArgs> ResizeRequested;

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x06000C7F RID: 3199
		// (remove) Token: 0x06000C80 RID: 3200
		event EventHandler<MetricDroppedEventArgs> MetricDropped;

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x06000C81 RID: 3201
		// (remove) Token: 0x06000C82 RID: 3202
		event EventHandler<MetricDroppedEventArgs> CategoryDropped;

		// Token: 0x06000C83 RID: 3203
		Task<DeleteEmptyTracksDialogResult> ShowDeleteEmptyTracksDialog();
	}
}
