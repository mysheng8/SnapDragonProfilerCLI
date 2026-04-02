using System;

namespace Sdp
{
	// Token: 0x02000082 RID: 130
	public class SetScrollLockCommand : Command
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x00008F98 File Offset: 0x00007198
		public SetScrollLockCommand(int captureId, bool scrollLock)
		{
			this.CaptureID = captureId;
			this.ScrollLock = scrollLock;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00008FAE File Offset: 0x000071AE
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x00008FB6 File Offset: 0x000071B6
		public int CaptureID { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00008FBF File Offset: 0x000071BF
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x00008FC7 File Offset: 0x000071C7
		public bool ScrollLock { get; set; }

		// Token: 0x060002D7 RID: 727 RVA: 0x00008FD0 File Offset: 0x000071D0
		protected override void OnExecute()
		{
			TimeModel timeModel = SdpApp.ModelManager.TimeModelCollection.GetTimeModel((uint)this.CaptureID);
			if (timeModel != null && SdpApp.ModelManager.TraceModel.GroupLayoutControllers.ContainsKey(this.CaptureID))
			{
				GroupLayoutController groupLayoutController = SdpApp.ModelManager.TraceModel.GroupLayoutControllers[this.CaptureID];
				timeModel.IsScrolling = this.ScrollLock;
				groupLayoutController.ScrollLockToggled = this.ScrollLock;
			}
		}
	}
}
