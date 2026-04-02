using System;

namespace Sdp
{
	// Token: 0x020000D5 RID: 213
	public class EntryViewRadioButtonFilter : EntryViewFilter
	{
		// Token: 0x040002F0 RID: 752
		public string[] Labels;

		// Token: 0x040002F1 RID: 753
		public string[] ToolTips;

		// Token: 0x040002F2 RID: 754
		public int SelectedIndex;

		// Token: 0x040002F3 RID: 755
		public EventHandler<RadioButtonToggledArgs> OnToggle;
	}
}
