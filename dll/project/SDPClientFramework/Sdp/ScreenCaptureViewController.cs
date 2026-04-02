using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000275 RID: 629
	public class ScreenCaptureViewController : ImageViewController
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0001E26D File Offset: 0x0001C46D
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x0001E275 File Offset: 0x0001C475
		public uint CaptureID
		{
			get
			{
				return this.m_captureID;
			}
			set
			{
				this.m_captureID = value;
			}
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0001E280 File Offset: 0x0001C480
		public ScreenCaptureViewController(IScreenCaptureView view)
			: base(view)
		{
			this.m_view = view;
			ImageViewEvents imageViewEvents = SdpApp.EventsManager.ImageViewEvents;
			imageViewEvents.Display = (EventHandler<ImageViewDisplayEventArgs>)Delegate.Remove(imageViewEvents.Display, new EventHandler<ImageViewDisplayEventArgs>(this.OnDisplayImage));
			ImageViewEvents imageViewEvents2 = SdpApp.EventsManager.ImageViewEvents;
			imageViewEvents2.DisplayText = (EventHandler<ImageViewTextDisplayEventArgs>)Delegate.Remove(imageViewEvents2.DisplayText, new EventHandler<ImageViewTextDisplayEventArgs>(this.OnDisplayText));
			ScreenCaptureViewEvents screenCaptureViewEvents = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents.DisplayScreenCapture = (EventHandler<ScreenCaptureViewDisplayEventArgs>)Delegate.Combine(screenCaptureViewEvents.DisplayScreenCapture, new EventHandler<ScreenCaptureViewDisplayEventArgs>(this.OnDisplayScreenCapture));
			ScreenCaptureViewEvents screenCaptureViewEvents2 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents2.BinningInfoAdded = (EventHandler<BinningInfoAddedEventArgs>)Delegate.Combine(screenCaptureViewEvents2.BinningInfoAdded, new EventHandler<BinningInfoAddedEventArgs>(this.OnBinningInfoReceived));
			ScreenCaptureViewEvents screenCaptureViewEvents3 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents3.DrawBuffersSet = (EventHandler<DrawBuffersSetEventArgs>)Delegate.Combine(screenCaptureViewEvents3.DrawBuffersSet, new EventHandler<DrawBuffersSetEventArgs>(this.OnDrawBuffersSet));
			ScreenCaptureViewEvents screenCaptureViewEvents4 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents4.DisableReplay = (EventHandler<DisableReplayEventArgs>)Delegate.Combine(screenCaptureViewEvents4.DisableReplay, new EventHandler<DisableReplayEventArgs>(this.OnDisableDrawModeToggle));
			ScreenCaptureViewEvents screenCaptureViewEvents5 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents5.SelectDrawMode = (EventHandler<SelectDrawModeEventArgs>)Delegate.Combine(screenCaptureViewEvents5.SelectDrawMode, new EventHandler<SelectDrawModeEventArgs>(this.OnSelectDrawMode));
			ScreenCaptureViewEvents screenCaptureViewEvents6 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents6.SetContext = (EventHandler<SetContextEventArgs>)Delegate.Combine(screenCaptureViewEvents6.SetContext, new EventHandler<SetContextEventArgs>(this.OnContextSet));
			ScreenCaptureViewEvents screenCaptureViewEvents7 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents7.SetHeight = (EventHandler<SetSurfaceHeightEventArgs>)Delegate.Combine(screenCaptureViewEvents7.SetHeight, new EventHandler<SetSurfaceHeightEventArgs>(this.OnHeightSet));
			ScreenCaptureViewEvents screenCaptureViewEvents8 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents8.SetWidth = (EventHandler<SetSurfaceWidthEventArgs>)Delegate.Combine(screenCaptureViewEvents8.SetWidth, new EventHandler<SetSurfaceWidthEventArgs>(this.OnWidthSet));
			ScreenCaptureViewEvents screenCaptureViewEvents9 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents9.EnablePicking = (EventHandler<EnableEventArgs>)Delegate.Combine(screenCaptureViewEvents9.EnablePicking, new EventHandler<EnableEventArgs>(this.OnEnablePicking));
			ScreenCaptureViewEvents screenCaptureViewEvents10 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents10.Invalidate = (EventHandler<ScreenCaptureViewInvalidateEventArgs>)Delegate.Combine(screenCaptureViewEvents10.Invalidate, new EventHandler<ScreenCaptureViewInvalidateEventArgs>(this.OnInvalidate));
			ScreenCaptureViewEvents screenCaptureViewEvents11 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents11.ToolbarConfig = (EventHandler<ScreenCaptureViewToolbarConfigEventArgs>)Delegate.Combine(screenCaptureViewEvents11.ToolbarConfig, new EventHandler<ScreenCaptureViewToolbarConfigEventArgs>(this.OnToolbarConfig));
			this.m_view.DrawModeChanged += this.OnDrawModeChanged;
			this.m_view.RotateClicked += this.OnRotateClicked;
			this.m_view.DisplayBinToggled += this.OnBinToggled;
			this.m_view.LocationSelected += this.OnLocationSelected;
			this.m_view.AttachmentsSelectionChanged += this.OnAttachmentsSelectionChanged;
			this.m_currentDrawMode = ScreenCaptureViewDrawMode.Color;
			this.m_currentTarget = ScreenCaptureTargetSelection.Normal;
			this.m_currentAttachments = null;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0001E570 File Offset: 0x0001C770
		public void DetachEvents()
		{
			ScreenCaptureViewEvents screenCaptureViewEvents = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents.DisplayScreenCapture = (EventHandler<ScreenCaptureViewDisplayEventArgs>)Delegate.Remove(screenCaptureViewEvents.DisplayScreenCapture, new EventHandler<ScreenCaptureViewDisplayEventArgs>(this.OnDisplayScreenCapture));
			ScreenCaptureViewEvents screenCaptureViewEvents2 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents2.BinningInfoAdded = (EventHandler<BinningInfoAddedEventArgs>)Delegate.Remove(screenCaptureViewEvents2.BinningInfoAdded, new EventHandler<BinningInfoAddedEventArgs>(this.OnBinningInfoReceived));
			ScreenCaptureViewEvents screenCaptureViewEvents3 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents3.DrawBuffersSet = (EventHandler<DrawBuffersSetEventArgs>)Delegate.Remove(screenCaptureViewEvents3.DrawBuffersSet, new EventHandler<DrawBuffersSetEventArgs>(this.OnDrawBuffersSet));
			ScreenCaptureViewEvents screenCaptureViewEvents4 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents4.DisableReplay = (EventHandler<DisableReplayEventArgs>)Delegate.Remove(screenCaptureViewEvents4.DisableReplay, new EventHandler<DisableReplayEventArgs>(this.OnDisableDrawModeToggle));
			ScreenCaptureViewEvents screenCaptureViewEvents5 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents5.SelectDrawMode = (EventHandler<SelectDrawModeEventArgs>)Delegate.Remove(screenCaptureViewEvents5.SelectDrawMode, new EventHandler<SelectDrawModeEventArgs>(this.OnSelectDrawMode));
			ScreenCaptureViewEvents screenCaptureViewEvents6 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents6.SetContext = (EventHandler<SetContextEventArgs>)Delegate.Remove(screenCaptureViewEvents6.SetContext, new EventHandler<SetContextEventArgs>(this.OnContextSet));
			ScreenCaptureViewEvents screenCaptureViewEvents7 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents7.SetHeight = (EventHandler<SetSurfaceHeightEventArgs>)Delegate.Remove(screenCaptureViewEvents7.SetHeight, new EventHandler<SetSurfaceHeightEventArgs>(this.OnHeightSet));
			ScreenCaptureViewEvents screenCaptureViewEvents8 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents8.SetWidth = (EventHandler<SetSurfaceWidthEventArgs>)Delegate.Remove(screenCaptureViewEvents8.SetWidth, new EventHandler<SetSurfaceWidthEventArgs>(this.OnWidthSet));
			ScreenCaptureViewEvents screenCaptureViewEvents9 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents9.EnablePicking = (EventHandler<EnableEventArgs>)Delegate.Remove(screenCaptureViewEvents9.EnablePicking, new EventHandler<EnableEventArgs>(this.OnEnablePicking));
			ScreenCaptureViewEvents screenCaptureViewEvents10 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents10.Invalidate = (EventHandler<ScreenCaptureViewInvalidateEventArgs>)Delegate.Remove(screenCaptureViewEvents10.Invalidate, new EventHandler<ScreenCaptureViewInvalidateEventArgs>(this.OnInvalidate));
			ScreenCaptureViewEvents screenCaptureViewEvents11 = SdpApp.EventsManager.ScreenCaptureViewEvents;
			screenCaptureViewEvents11.ToolbarConfig = (EventHandler<ScreenCaptureViewToolbarConfigEventArgs>)Delegate.Remove(screenCaptureViewEvents11.ToolbarConfig, new EventHandler<ScreenCaptureViewToolbarConfigEventArgs>(this.OnToolbarConfig));
			this.m_view.DrawModeChanged -= this.OnDrawModeChanged;
			this.m_view.LocationSelected -= this.OnLocationSelected;
			this.m_view.AttachmentsSelectionChanged -= this.OnAttachmentsSelectionChanged;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0001E79C File Offset: 0x0001C99C
		private void OnLocationSelected(object sender, LocationSelectedEventArgs args)
		{
			if (SdpApp.ConnectionManager.IsConnected())
			{
				args.CaptureID = this.m_captureID;
				args.DrawCallID = this.m_drawCallID;
				SdpApp.EventsManager.Raise<LocationSelectedEventArgs>(SdpApp.EventsManager.ScreenCaptureViewEvents.LocationSelected, this, args);
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0001E7E8 File Offset: 0x0001C9E8
		private void OnDisplayScreenCapture(object sender, ScreenCaptureViewDisplayEventArgs args)
		{
			this.m_captureID = args.CaptureID;
			this.m_drawCallID = args.DrawCallID;
			this.SetBinConfig();
			if (args.Type == ScreenCaptureType.Replay || args.Type == ScreenCaptureType.Highlight)
			{
				if (this.m_currentDrawCallHighlightPair == null)
				{
					this.m_currentDrawCallHighlightPair = new ScreenCaptureViewController.DrawCallHighlightPair();
				}
				if (this.m_currentDrawCallHighlightPair.ReplayID != args.ReplayID)
				{
					this.m_currentDrawCallHighlightPair.ReplayID = args.ReplayID;
					this.m_currentDrawCallHighlightPair.DrawCallImage = null;
					this.m_currentDrawCallHighlightPair.HighlightImage = null;
				}
				if (args.Type == ScreenCaptureType.Replay)
				{
					this.m_currentDrawCallHighlightPair.DrawCallImage = args.CaptureImage;
				}
				else
				{
					this.m_currentDrawCallHighlightPair.HighlightImage = args.CaptureImage;
				}
				if (this.DrawCallHighlightPairSet())
				{
					Queue<ImageViewObject> queue = new Queue<ImageViewObject>();
					for (int i = 0; i < 4; i++)
					{
						queue.Enqueue(this.m_currentDrawCallHighlightPair.HighlightImage);
						queue.Enqueue(this.m_currentDrawCallHighlightPair.DrawCallImage);
					}
					AnimationClass animationClass = new AnimationClass(queue, this.m_view, 500U, false);
					this.m_view.AnimationPlayer.LoadAnimation(animationClass);
					this.m_currentDrawCallHighlightPair.HighlightImage = null;
					this.m_currentDrawCallHighlightPair.DrawCallImage = null;
					return;
				}
				if (args.Type == ScreenCaptureType.Replay)
				{
					Queue<ImageViewObject> queue2 = new Queue<ImageViewObject>();
					queue2.Enqueue(this.m_currentDrawCallHighlightPair.DrawCallImage);
					AnimationClass animationClass2 = new AnimationClass(queue2, this.m_view, 0U, false);
					this.m_view.AnimationPlayer.LoadAnimation(animationClass2);
					return;
				}
			}
			else
			{
				Queue<ImageViewObject> queue3 = new Queue<ImageViewObject>();
				queue3.Enqueue(args.CaptureImage);
				bool flag = args.Type == ScreenCaptureType.Capture;
				AnimationClass animationClass3 = new AnimationClass(queue3, this.m_view, 0U, flag);
				this.m_view.AnimationPlayer.LoadAnimation(animationClass3);
			}
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0001E9A3 File Offset: 0x0001CBA3
		private bool DrawCallHighlightPairSet()
		{
			return this.m_currentDrawCallHighlightPair.DrawCallImage != null && this.m_currentDrawCallHighlightPair.HighlightImage != null;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0001E9C4 File Offset: 0x0001CBC4
		private void OnBinningInfoReceived(object sender, BinningInfoAddedEventArgs args)
		{
			if (this.m_binningInfos.ContainsKey(args.CaptureId))
			{
				this.m_binningInfos[args.CaptureId] = new BinConfigPair();
			}
			else
			{
				this.m_binningInfos.Add(args.CaptureId, new BinConfigPair());
			}
			this.m_binningInfos[args.CaptureId].BinConfigMapping = args.BinConfigMapping;
			this.m_binningInfos[args.CaptureId].BinConfigs = args.BinConfigs;
			this.SetBinConfig();
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0001EA50 File Offset: 0x0001CC50
		private void SetBinConfig()
		{
			if (!this.m_binningInfos.ContainsKey(this.m_captureID) || this.m_binningInfos[this.m_captureID].BinConfigMapping.Count == 0)
			{
				this.m_view.BinConfiguration = null;
				return;
			}
			Dictionary<uint, BinConfigChange> dictionary = new Dictionary<uint, BinConfigChange>();
			dictionary[this.m_binningInfos[this.m_captureID].BinConfigMapping[0].ContextID] = this.m_binningInfos[this.m_captureID].BinConfigMapping[0];
			for (int i = 0; i < this.m_binningInfos[this.m_captureID].BinConfigMapping.Count; i++)
			{
				BinConfigChange binConfigChange = this.m_binningInfos[this.m_captureID].BinConfigMapping[i];
				if (this.m_drawCallID < this.m_binningInfos[this.m_captureID].BinConfigMapping[i].DrawCallID)
				{
					break;
				}
				dictionary[binConfigChange.ContextID] = binConfigChange;
			}
			if (dictionary.ContainsKey(this.m_selectedContext) && this.m_binningInfos.ContainsKey(this.m_captureID) && this.m_binningInfos[this.m_captureID].BinConfigs.ContainsKey(dictionary[this.m_selectedContext].BinConfigID))
			{
				this.m_view.BinConfiguration = this.m_binningInfos[this.m_captureID].BinConfigs[dictionary[this.m_selectedContext].BinConfigID];
				return;
			}
			this.m_view.BinConfiguration = null;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0001EBF4 File Offset: 0x0001CDF4
		private void OnDrawModeChanged(object sender, DrawModeChangedEventArgs args)
		{
			if (args.DrawMode != this.m_currentDrawMode || args.Target != this.m_currentTarget || args.SelectedContext != this.m_selectedContext)
			{
				this.m_currentDrawMode = args.DrawMode;
				this.m_currentTarget = args.Target;
				this.m_selectedContext = args.SelectedContext;
				SdpApp.EventsManager.Raise<DrawModeChangedEventArgs>(SdpApp.EventsManager.ScreenCaptureViewEvents.DrawModeChanged, this, args);
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0001EC6A File Offset: 0x0001CE6A
		private void OnRotateClicked(object sender, EventArgs args)
		{
			this.m_view.Rotate(1.5707963267948966);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0001EC80 File Offset: 0x0001CE80
		private void OnBinToggled(object sender, ButtonToggledEventArgs args)
		{
			this.m_view.DrawImage();
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0001EC90 File Offset: 0x0001CE90
		private void OnDrawBuffersSet(object sender, DrawBuffersSetEventArgs args)
		{
			if (args.BufferIds.ContainsKey(this.m_selectedContext))
			{
				List<uint> list = args.BufferIds[this.m_selectedContext];
				this.m_view.SetNumColorAttachments((uint)Math.Max(1, list.Count));
			}
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0001ECD9 File Offset: 0x0001CED9
		private void OnDisableDrawModeToggle(object sender, DisableReplayEventArgs args)
		{
			this.m_view.DisableDrawModeToggle(args.Disable);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0001ECEC File Offset: 0x0001CEEC
		private void OnSelectDrawMode(object sender, SelectDrawModeEventArgs args)
		{
			this.m_view.SetDrawMode(args.Target, args.DrawMode);
			this.m_currentDrawMode = args.DrawMode;
			this.m_currentTarget = args.Target;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0001ED1D File Offset: 0x0001CF1D
		private void OnContextSet(object sender, SetContextEventArgs args)
		{
			this.m_selectedContext = args.SelectedContext;
			this.m_view.SetContextComboBox(args.ContextIDs, this.m_selectedContext);
			this.SetBinConfig();
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0001ED48 File Offset: 0x0001CF48
		private void OnHeightSet(object sender, SetSurfaceHeightEventArgs args)
		{
			if (args.CaptureID != this.m_captureID)
			{
				return;
			}
			this.m_surfaceHeight = args.Height;
			if (this.m_surfaceWidth != 0U && this.m_surfaceHeight != 0U)
			{
				this.m_view.SetScreenWidthAndHeight((int)this.m_surfaceWidth, (int)this.m_surfaceHeight);
			}
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0001ED98 File Offset: 0x0001CF98
		private void OnWidthSet(object sender, SetSurfaceWidthEventArgs args)
		{
			if (args.CaptureID != this.m_captureID)
			{
				return;
			}
			this.m_surfaceWidth = args.Width;
			if (this.m_surfaceWidth != 0U && this.m_surfaceHeight != 0U)
			{
				this.m_view.SetScreenWidthAndHeight((int)this.m_surfaceWidth, (int)this.m_surfaceHeight);
			}
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0001EDE7 File Offset: 0x0001CFE7
		private void OnEnablePicking(object sender, EnableEventArgs args)
		{
			this.m_view.PickingEnabled = args.Enable;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0001EDFC File Offset: 0x0001CFFC
		private void OnInvalidate(object sender, ScreenCaptureViewInvalidateEventArgs args)
		{
			if ((ulong)this.m_captureID == (ulong)((long)args.CaptureID))
			{
				this.m_currentAttachments = args.Attachments;
				this.m_view.InvalidateAttachments(args.Attachments);
				this.m_view.ClearImage();
				this.m_view.TrySelectFirst();
			}
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0001EE4C File Offset: 0x0001D04C
		private void OnToolbarConfig(object sender, ScreenCaptureViewToolbarConfigEventArgs args)
		{
			if ((ulong)this.m_captureID == (ulong)((long)args.CaptureID))
			{
				this.m_view.AttachmentsVisible = args.ShowAttachmentsComboBox;
				this.m_view.ContextsVisible = args.ShowContextComboBox;
				this.m_view.DrawModeVisible = args.ShowLegacyAttachmentsComboBox;
				this.m_view.BinToggleVisible = args.ShowBinningToggle;
			}
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0001EEB0 File Offset: 0x0001D0B0
		private void OnAttachmentsSelectionChanged(object sender, EventArgs e)
		{
			if (this.m_currentAttachments != null)
			{
				int selectedAttachment = this.m_view.SelectedAttachment;
				ImageViewObject imageViewObject = null;
				if (this.m_currentAttachments.TryGetValue(selectedAttachment, out imageViewObject))
				{
					this.m_view.UpdateImage(imageViewObject);
					this.m_surfaceWidth = (uint)imageViewObject.Width;
					this.m_surfaceHeight = (uint)imageViewObject.Height;
					this.m_view.SetScreenWidthAndHeight(imageViewObject.Width, imageViewObject.Height);
				}
			}
		}

		// Token: 0x0400088B RID: 2187
		private IScreenCaptureView m_view;

		// Token: 0x0400088C RID: 2188
		private ScreenCaptureViewDrawMode m_currentDrawMode;

		// Token: 0x0400088D RID: 2189
		private ScreenCaptureTargetSelection m_currentTarget;

		// Token: 0x0400088E RID: 2190
		private ScreenCaptureViewController.DrawCallHighlightPair m_currentDrawCallHighlightPair;

		// Token: 0x0400088F RID: 2191
		private const uint NOT_SET = 4294967295U;

		// Token: 0x04000890 RID: 2192
		private uint m_captureID = uint.MaxValue;

		// Token: 0x04000891 RID: 2193
		private uint m_drawCallID = uint.MaxValue;

		// Token: 0x04000892 RID: 2194
		private uint m_surfaceWidth;

		// Token: 0x04000893 RID: 2195
		private uint m_surfaceHeight;

		// Token: 0x04000894 RID: 2196
		private uint m_selectedContext;

		// Token: 0x04000895 RID: 2197
		private Dictionary<uint, BinConfigPair> m_binningInfos = new Dictionary<uint, BinConfigPair>();

		// Token: 0x04000896 RID: 2198
		private Dictionary<int, ImageViewObject> m_currentAttachments;

		// Token: 0x020003B7 RID: 951
		private class DrawCallHighlightPair
		{
			// Token: 0x04000D19 RID: 3353
			public ImageViewObject DrawCallImage;

			// Token: 0x04000D1A RID: 3354
			public ImageViewObject HighlightImage;

			// Token: 0x04000D1B RID: 3355
			public uint ReplayID;
		}
	}
}
