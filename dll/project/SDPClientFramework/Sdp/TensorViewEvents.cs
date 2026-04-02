using System;

namespace Sdp
{
	// Token: 0x02000171 RID: 369
	public class TensorViewEvents
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x0000A914 File Offset: 0x00008B14
		public TensorViewEvents()
		{
			this.DisplayTensor = (EventHandler<TensorViewDisplayEventArgs>)Delegate.Combine(this.DisplayTensor, new EventHandler<TensorViewDisplayEventArgs>(this.SetVisible));
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000A93E File Offset: 0x00008B3E
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("TensorView", null, false, false);
		}

		// Token: 0x04000548 RID: 1352
		public EventHandler<TensorViewDisplayEventArgs> DisplayTensor;

		// Token: 0x04000549 RID: 1353
		public const string TENSOR_VIEW_TYPENAME = "TensorView";

		// Token: 0x0400054A RID: 1354
		public const string TENSOR_VIEW_WINDOW_NAME = "Tensor View";
	}
}
