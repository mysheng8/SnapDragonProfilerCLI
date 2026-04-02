using System;

namespace Sdp
{
	// Token: 0x020000E6 RID: 230
	public class InspectorViewEvents
	{
		// Token: 0x0600037D RID: 893 RVA: 0x00009AE6 File Offset: 0x00007CE6
		public InspectorViewEvents()
		{
			this.Display = (EventHandler<InspectorViewDisplayEventArgs>)Delegate.Combine(this.Display, new EventHandler<InspectorViewDisplayEventArgs>(this.SetVisible));
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00009B10 File Offset: 0x00007D10
		private void SetVisible(object o, EventArgs e)
		{
			SdpApp.UIManager.PresentView("Inspector", null, false, false);
		}

		// Token: 0x0400033B RID: 827
		public EventHandler<InspectorViewDisplayEventArgs> Display;

		// Token: 0x0400033C RID: 828
		public EventHandler<MultiSelectionActivationEventArgs> MultiSelection;

		// Token: 0x0400033D RID: 829
		public EventHandler<ButtonClickedArgs> ButtonClicked;

		// Token: 0x0400033E RID: 830
		public EventHandler<SetStatusEventArgs> SetStatus;

		// Token: 0x0400033F RID: 831
		public EventHandler<MultiViewArgs> HideStatus;

		// Token: 0x04000340 RID: 832
		public const string INSPECTOR_VIEW_TYPENAME = "Inspector";
	}
}
