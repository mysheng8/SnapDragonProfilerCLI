using System;

namespace Sdp
{
	// Token: 0x020001DA RID: 474
	public interface IPropertiesWidgetView
	{
		// Token: 0x14000047 RID: 71
		// (add) Token: 0x0600066C RID: 1644
		// (remove) Token: 0x0600066D RID: 1645
		event EventHandler SelectedCategoryChanged;

		// Token: 0x0600066E RID: 1646
		void AddCategory(string category);

		// Token: 0x0600066F RID: 1647
		void ClearProperties();

		// Token: 0x06000670 RID: 1648
		void AddProperty(PropertiesItem propertyItem);

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000671 RID: 1649
		// (set) Token: 0x06000672 RID: 1650
		string SelectedCategory { get; set; }
	}
}
