using System;

namespace Sdp
{
	// Token: 0x020000D4 RID: 212
	public class EntryViewCheckBoxFilter : EntryViewFilter
	{
		// Token: 0x040002ED RID: 749
		public string Label;

		// Token: 0x040002EE RID: 750
		public string ToolTip;

		// Token: 0x040002EF RID: 751
		public EventHandler<CheckBoxToggledArgs> OnToggle;
	}
}
