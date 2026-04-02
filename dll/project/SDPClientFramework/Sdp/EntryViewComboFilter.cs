using System;

namespace Sdp
{
	// Token: 0x020000D3 RID: 211
	public class EntryViewComboFilter : EntryViewFilter
	{
		// Token: 0x040002EA RID: 746
		public SearchString[] SearchStrings;

		// Token: 0x040002EB RID: 747
		public SearchMode SearchMode;

		// Token: 0x040002EC RID: 748
		public EventHandler<ComboBoxSelectionChangedArgs> OnSelectionChanged;
	}
}
