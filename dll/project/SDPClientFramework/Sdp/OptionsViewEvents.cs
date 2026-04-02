using System;

namespace Sdp
{
	// Token: 0x020000EC RID: 236
	public class OptionsViewEvents
	{
		// Token: 0x0400034D RID: 845
		public EventHandler<EventArgs> DisableView;

		// Token: 0x0400034E RID: 846
		public EventHandler<EventArgs> EnableView;

		// Token: 0x0400034F RID: 847
		public EventHandler<OptionValueChangedEventArgs> OptionValueChanged;

		// Token: 0x04000350 RID: 848
		public EventHandler<EventArgs> ShowAllProcessToggled;

		// Token: 0x04000351 RID: 849
		public const string OPTIONS_VIEW_TYPENAME = "Options";
	}
}
