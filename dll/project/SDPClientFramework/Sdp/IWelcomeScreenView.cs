using System;

namespace Sdp
{
	// Token: 0x020002C7 RID: 711
	public interface IWelcomeScreenView : IView
	{
		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x06000E8C RID: 3724
		// (remove) Token: 0x06000E8D RID: 3725
		event EventHandler<ActionSelectedEventArgs> ActionSelected;

		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06000E8E RID: 3726
		// (remove) Token: 0x06000E8F RID: 3727
		event EventHandler<RecentCaptureSelectedEventArgs> RecentCaptureSelected;

		// Token: 0x06000E90 RID: 3728
		void ShowButton(ActionEnum capture);

		// Token: 0x06000E91 RID: 3729
		void Connected(uint deviceErrorMask);

		// Token: 0x06000E92 RID: 3730
		void Disconnected();

		// Token: 0x06000E93 RID: 3731
		void AddAction(WelcomeScreenAction action);

		// Token: 0x06000E94 RID: 3732
		void RemoveAction(ActionEnum Action);
	}
}
