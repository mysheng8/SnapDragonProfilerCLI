using System;

namespace Sdp
{
	// Token: 0x020002B0 RID: 688
	public interface IInspector : IView, IStatusBox
	{
		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x06000DDB RID: 3547
		// (remove) Token: 0x06000DDC RID: 3548
		event EventHandler<PropertyDescriptionChangedEventArgs> PropertyGridValueChanged;

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06000DDD RID: 3549
		// (remove) Token: 0x06000DDE RID: 3550
		event EventHandler<ButtonClickedArgs> ButtonClicked;

		// Token: 0x06000DDF RID: 3551
		void setPropertyGridObject(object currentObject, uint captureID, bool showASButton, bool showTensorButton);
	}
}
