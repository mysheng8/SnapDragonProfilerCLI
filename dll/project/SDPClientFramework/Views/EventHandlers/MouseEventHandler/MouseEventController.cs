using System;
using Sdp.Functional;

namespace SDPClientFramework.Views.EventHandlers.MouseEventHandler
{
	// Token: 0x02000045 RID: 69
	internal class MouseEventController : IMouseEventController
	{
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000170 RID: 368 RVA: 0x00005E74 File Offset: 0x00004074
		// (remove) Token: 0x06000171 RID: 369 RVA: 0x00005EAC File Offset: 0x000040AC
		public event EventHandler<DragEventArgs> DragBegin;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000172 RID: 370 RVA: 0x00005EE4 File Offset: 0x000040E4
		// (remove) Token: 0x06000173 RID: 371 RVA: 0x00005F1C File Offset: 0x0000411C
		public event EventHandler<DragEventArgs> DragMove;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000174 RID: 372 RVA: 0x00005F54 File Offset: 0x00004154
		// (remove) Token: 0x06000175 RID: 373 RVA: 0x00005F8C File Offset: 0x0000418C
		public event EventHandler<DragEventArgs> DragEnded;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000176 RID: 374 RVA: 0x00005FC4 File Offset: 0x000041C4
		// (remove) Token: 0x06000177 RID: 375 RVA: 0x00005FFC File Offset: 0x000041FC
		public event EventHandler<MouseClickEventArgs> MouseButtonClicked;

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00006031 File Offset: 0x00004231
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00006039 File Offset: 0x00004239
		private Maybe<Click> CurrentClickType { get; set; } = new Maybe<Click>.None();

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00006042 File Offset: 0x00004242
		// (set) Token: 0x0600017B RID: 379 RVA: 0x0000604A File Offset: 0x0000424A
		private Point CurrentLocation { get; set; } = new Point();

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00006053 File Offset: 0x00004253
		// (set) Token: 0x0600017D RID: 381 RVA: 0x0000605B File Offset: 0x0000425B
		private bool IsDragging { get; set; }

		// Token: 0x0600017E RID: 382 RVA: 0x00006064 File Offset: 0x00004264
		public MouseEventController(IMouseEventHandler mouseEventHandler)
		{
			mouseEventHandler.MouseButtonPressed += this.OnMouseButtonPressed;
			mouseEventHandler.MouseMoved += this.OnMouseMoved;
			mouseEventHandler.MouseButtonReleased += this.OnMouseButtonReleased;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000060C3 File Offset: 0x000042C3
		private void OnMouseButtonPressed(object sender, MouseButtonPressedArgs e)
		{
			if (e.ClickType == ClickType.DoubleClick)
			{
				this.OnDoubleClickPressedEvent(e);
				return;
			}
			this.OnSingleClickedPressedEvent(e);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000060E0 File Offset: 0x000042E0
		private void OnSingleClickedPressedEvent(MouseButtonPressedArgs e)
		{
			this.CurrentClickType.Match(delegate(Click someClickType)
			{
			}, delegate
			{
				this.CurrentLocation = e.Location;
				this.CurrentClickType = new Maybe<Click>.Some(new Click
				{
					Location = e.Location,
					Button = e.Button,
					ClickType = e.ClickType,
					Modifiers = e.Modifiers
				});
			});
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006137 File Offset: 0x00004337
		private void OnDoubleClickPressedEvent(MouseButtonPressedArgs e)
		{
			if (!this.IsDragging)
			{
				EventHandler<MouseClickEventArgs> mouseButtonClicked = this.MouseButtonClicked;
				if (mouseButtonClicked == null)
				{
					return;
				}
				mouseButtonClicked(this, new MouseClickEventArgs(e));
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006158 File Offset: 0x00004358
		private void OnMouseMoved(object sender, MouseMovedArgs e)
		{
			this.CurrentLocation = e.NewPosition;
			this.CurrentClickType.Match(delegate(Click someClickType)
			{
				DragEventArgs dragEventArgs = new DragEventArgs
				{
					PreviousLocation = someClickType.Location,
					CurrentLocation = e.NewPosition,
					Button = someClickType.Button,
					Modifiers = e.Modifiers
				};
				if (this.IsDragging)
				{
					EventHandler<DragEventArgs> dragMove = this.DragMove;
					if (dragMove != null)
					{
						dragMove(this, dragEventArgs);
					}
				}
				else
				{
					EventHandler<DragEventArgs> dragBegin = this.DragBegin;
					if (dragBegin != null)
					{
						dragBegin(this, dragEventArgs);
					}
				}
				this.IsDragging = true;
				someClickType.Location = e.NewPosition;
				e.Handled = dragEventArgs.Handled;
			}, delegate
			{
			});
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000061C0 File Offset: 0x000043C0
		private void OnMouseButtonReleased(object sender, MouseButtonReleasedArgs e)
		{
			this.CurrentClickType.Match(delegate(Click someClickType)
			{
				if (someClickType.Button == e.Button)
				{
					if (this.IsDragging)
					{
						EventHandler<DragEventArgs> dragEnded = this.DragEnded;
						if (dragEnded != null)
						{
							dragEnded(this, new DragEventArgs
							{
								PreviousLocation = someClickType.Location,
								CurrentLocation = someClickType.Location,
								Button = someClickType.Button,
								Modifiers = someClickType.Modifiers
							});
						}
						this.IsDragging = false;
					}
					else
					{
						EventHandler<MouseClickEventArgs> mouseButtonClicked = this.MouseButtonClicked;
						if (mouseButtonClicked != null)
						{
							mouseButtonClicked(this, new MouseClickEventArgs
							{
								Location = someClickType.Location,
								Button = someClickType.Button,
								ClickType = someClickType.ClickType,
								Modifiers = someClickType.Modifiers
							});
						}
					}
					this.CurrentClickType = new Maybe<Click>.None();
				}
			}, delegate
			{
			});
		}
	}
}
