using System;

namespace Sdp
{
	// Token: 0x020000C7 RID: 199
	public class ContextViewEvents
	{
		// Token: 0x040002C7 RID: 711
		public EventHandler<PropertyGridPopulateArgs> PopulatePropertyGrid;

		// Token: 0x040002C8 RID: 712
		public EventHandler<PropertyDescriptionChangedEventArgs> PropertyDescriptorChanged;

		// Token: 0x040002C9 RID: 713
		public EventHandler<UpdateVisibiltyArgs> UpdateVisibility;

		// Token: 0x040002CA RID: 714
		public const string CONTEXT_VIEW_TYPENAME = "Context";
	}
}
