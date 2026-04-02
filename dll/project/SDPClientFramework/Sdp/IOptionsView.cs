using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sdp
{
	// Token: 0x02000262 RID: 610
	public interface IOptionsView : IView
	{
		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06000A2F RID: 2607
		// (remove) Token: 0x06000A30 RID: 2608
		event EventHandler<OptionViewValueChangedEventArgs> OptionViewValueChanged;

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06000A31 RID: 2609
		// (remove) Token: 0x06000A32 RID: 2610
		event EventHandler<EventArgs> ShowAllProcessToggled;

		// Token: 0x06000A33 RID: 2611
		void SetCurrentObject(object currentObject);

		// Token: 0x06000A34 RID: 2612
		void RefreshPropertyGrid();

		// Token: 0x06000A35 RID: 2613
		void DisableView(bool disable);

		// Token: 0x06000A36 RID: 2614
		void AddOption(PropertyDescriptor propertyDesc);

		// Token: 0x06000A37 RID: 2615
		void SetCategoryFilters(List<string> categories);
	}
}
