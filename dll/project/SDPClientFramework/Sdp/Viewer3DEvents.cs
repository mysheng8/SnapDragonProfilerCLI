using System;

namespace Sdp
{
	// Token: 0x02000163 RID: 355
	public class Viewer3DEvents
	{
		// Token: 0x06000434 RID: 1076 RVA: 0x0000A8D6 File Offset: 0x00008AD6
		public Viewer3DEvents()
		{
			this.RequestLoadAccelerationStructure = (EventHandler<Viewer3DLoadASArgs>)Delegate.Combine(this.RequestLoadAccelerationStructure, new EventHandler<Viewer3DLoadASArgs>(this.SetVisible));
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000A900 File Offset: 0x00008B00
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("Viewer3DView", null, false, false);
		}

		// Token: 0x0400051D RID: 1309
		public EventHandler<EventArgs> OnDelete;

		// Token: 0x0400051E RID: 1310
		public EventHandler<Viewer3DOnRealizeArgs> OnRealize;

		// Token: 0x0400051F RID: 1311
		public EventHandler<Viewer3DOnResizeArgs> OnResize;

		// Token: 0x04000520 RID: 1312
		public EventHandler<Viewer3DOnMouseMoveArgs> OnMouseMove;

		// Token: 0x04000521 RID: 1313
		public EventHandler<Viewer3DOnButtonInputArgs> OnButtonInput;

		// Token: 0x04000522 RID: 1314
		public EventHandler<Viewer3DOnKeyInputArgs> OnKeyInput;

		// Token: 0x04000523 RID: 1315
		public EventHandler<Viewer3DLoadASArgs> RequestLoadAccelerationStructure;

		// Token: 0x04000524 RID: 1316
		public EventHandler<Viewer3DLoadASArgs> LoadAccelerationStructure;

		// Token: 0x04000525 RID: 1317
		public EventHandler<Viewer3DDisplayASArgs> DisplayAccelerationStructure;

		// Token: 0x04000526 RID: 1318
		public EventHandler<Viewer3DRenderOptionChangedArgs> RenderOptionChanged;

		// Token: 0x04000527 RID: 1319
		public EventHandler<Viewer3DCameraOptionChangedArgs> CameraOptionChanged;

		// Token: 0x04000528 RID: 1320
		public EventHandler<Viewer3DCameraPositionArgs> CameraPositionUpdated;

		// Token: 0x04000529 RID: 1321
		public EventHandler<Viewer3DCameraCoordinateChangedArgs> CameraCoordinateChanged;

		// Token: 0x0400052A RID: 1322
		public EventHandler<Viewer3DCameraFarPlaneArgs> CameraFarPlaneUpdated;

		// Token: 0x0400052B RID: 1323
		public const string VIEWER_3D_VIEW_TYPENAME = "Viewer3DView";
	}
}
