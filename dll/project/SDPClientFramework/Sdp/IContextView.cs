using System;

namespace Sdp
{
	// Token: 0x02000228 RID: 552
	public interface IContextView : IView
	{
		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06000871 RID: 2161
		// (remove) Token: 0x06000872 RID: 2162
		event EventHandler<PropertyDescriptionChangedEventArgs> PropertyGridValueChanged;

		// Token: 0x06000873 RID: 2163
		void SetCurrentObject(object currentObject);

		// Token: 0x06000874 RID: 2164
		void UpdateVisibility(bool visible);

		// Token: 0x06000875 RID: 2165
		void DisableView(bool disable);
	}
}
