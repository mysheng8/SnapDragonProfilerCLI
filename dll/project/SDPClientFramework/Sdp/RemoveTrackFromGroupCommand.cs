using System;

namespace Sdp
{
	// Token: 0x0200007F RID: 127
	public class RemoveTrackFromGroupCommand : Command
	{
		// Token: 0x060002C5 RID: 709 RVA: 0x00008EAA File Offset: 0x000070AA
		protected override void OnExecute()
		{
			if (this.Container != null && this.Track != null)
			{
				this.Container.RemoveTrack(this.Track);
			}
		}

		// Token: 0x040001BF RID: 447
		public GroupController Container;

		// Token: 0x040001C0 RID: 448
		public TrackControllerBase Track;
	}
}
