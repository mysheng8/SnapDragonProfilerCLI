using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000270 RID: 624
	public interface IScreenCaptureView : IImageView, IView
	{
		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06000A86 RID: 2694
		// (remove) Token: 0x06000A87 RID: 2695
		event EventHandler<ButtonToggledEventArgs> DisplayBinToggled;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06000A88 RID: 2696
		// (remove) Token: 0x06000A89 RID: 2697
		event EventHandler<DrawModeChangedEventArgs> DrawModeChanged;

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06000A8A RID: 2698
		// (remove) Token: 0x06000A8B RID: 2699
		event EventHandler<EventArgs> RotateClicked;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06000A8C RID: 2700
		// (remove) Token: 0x06000A8D RID: 2701
		event EventHandler<LocationSelectedEventArgs> LocationSelected;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06000A8E RID: 2702
		// (remove) Token: 0x06000A8F RID: 2703
		event EventHandler AttachmentsSelectionChanged;

		// Token: 0x17000203 RID: 515
		// (set) Token: 0x06000A90 RID: 2704
		bool ContextsVisible { set; }

		// Token: 0x17000204 RID: 516
		// (set) Token: 0x06000A91 RID: 2705
		bool DrawModeVisible { set; }

		// Token: 0x17000205 RID: 517
		// (set) Token: 0x06000A92 RID: 2706
		bool AttachmentsVisible { set; }

		// Token: 0x17000206 RID: 518
		// (set) Token: 0x06000A93 RID: 2707
		bool BinToggleVisible { set; }

		// Token: 0x17000207 RID: 519
		// (set) Token: 0x06000A94 RID: 2708
		bool PickingEnabled { set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000A95 RID: 2709
		int SelectedAttachment { get; }

		// Token: 0x06000A96 RID: 2710
		void TrySelectFirst();

		// Token: 0x06000A97 RID: 2711
		void InvalidateAttachments(Dictionary<int, ImageViewObject> attachments);

		// Token: 0x06000A98 RID: 2712
		void SetNumColorAttachments(uint numColorBuffers);

		// Token: 0x06000A99 RID: 2713
		void SetScreenWidthAndHeight(int width, int height);

		// Token: 0x06000A9A RID: 2714
		void DisableDrawModeToggle(bool disable);

		// Token: 0x06000A9B RID: 2715
		void SetDrawMode(ScreenCaptureTargetSelection target, ScreenCaptureViewDrawMode mode);

		// Token: 0x06000A9C RID: 2716
		void SetContextComboBox(List<uint> contextIDs, uint selectedContext);

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000A9E RID: 2718
		// (set) Token: 0x06000A9D RID: 2717
		BinConfiguration BinConfiguration { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000A9F RID: 2719
		IAnimationPlayer AnimationPlayer { get; }
	}
}
