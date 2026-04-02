using System;
using Sdp;
using SDPClientFramework.Views.EventHandlers.MouseEventHandler;

namespace SDPClientFramework.Views.Flow.Controllers
{
	// Token: 0x02000033 RID: 51
	internal class DataViewMouseEventController
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600011A RID: 282 RVA: 0x00004C98 File Offset: 0x00002E98
		// (remove) Token: 0x0600011B RID: 283 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public event EventHandler<DataViewDragEventArgs> DragBegin;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600011C RID: 284 RVA: 0x00004D08 File Offset: 0x00002F08
		// (remove) Token: 0x0600011D RID: 285 RVA: 0x00004D40 File Offset: 0x00002F40
		public event EventHandler<DataViewDragEventArgs> DragMoved;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600011E RID: 286 RVA: 0x00004D78 File Offset: 0x00002F78
		// (remove) Token: 0x0600011F RID: 287 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public event EventHandler<DataViewDragEventArgs> DragEnded;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000120 RID: 288 RVA: 0x00004DE8 File Offset: 0x00002FE8
		// (remove) Token: 0x06000121 RID: 289 RVA: 0x00004E20 File Offset: 0x00003020
		public event EventHandler<DataViewMouseClickEventArgs> MouseButtonClicked;

		// Token: 0x06000122 RID: 290 RVA: 0x00004E58 File Offset: 0x00003058
		public DataViewMouseEventController(IDataViewMouseEventHandler mouseHandler)
		{
			DataViewMouseEventController <>4__this = this;
			this.m_mouseController = new MouseEventController(mouseHandler);
			this.m_mouseController.DragBegin += delegate(object _s, DragEventArgs e)
			{
				DataViewDragEventArgs dataViewDragEventArgs = new DataViewDragEventArgs(e, mouseHandler);
				EventHandler<DataViewDragEventArgs> dragBegin = <>4__this.DragBegin;
				if (dragBegin != null)
				{
					dragBegin(<>4__this, dataViewDragEventArgs);
				}
				e.Handled = dataViewDragEventArgs.Handled;
			};
			this.m_mouseController.DragMove += delegate(object _s, DragEventArgs e)
			{
				DataViewDragEventArgs dataViewDragEventArgs2 = new DataViewDragEventArgs(e, mouseHandler);
				EventHandler<DataViewDragEventArgs> dragMoved = <>4__this.DragMoved;
				if (dragMoved != null)
				{
					dragMoved(<>4__this, dataViewDragEventArgs2);
				}
				e.Handled = dataViewDragEventArgs2.Handled;
			};
			this.m_mouseController.DragEnded += delegate(object _s, DragEventArgs e)
			{
				EventHandler<DataViewDragEventArgs> dragEnded = <>4__this.DragEnded;
				if (dragEnded == null)
				{
					return;
				}
				dragEnded(<>4__this, new DataViewDragEventArgs(e, mouseHandler));
			};
			this.m_mouseController.MouseButtonClicked += delegate(object _s, MouseClickEventArgs e)
			{
				EventHandler<DataViewMouseClickEventArgs> mouseButtonClicked = <>4__this.MouseButtonClicked;
				if (mouseButtonClicked == null)
				{
					return;
				}
				mouseButtonClicked(<>4__this, new DataViewMouseClickEventArgs(e, mouseHandler));
			};
		}

		// Token: 0x040000EB RID: 235
		private readonly MouseEventController m_mouseController;
	}
}
