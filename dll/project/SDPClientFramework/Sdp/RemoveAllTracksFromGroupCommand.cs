using System;

namespace Sdp
{
	// Token: 0x0200006E RID: 110
	public class RemoveAllTracksFromGroupCommand : Command
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00008540 File Offset: 0x00006740
		protected override void OnExecute()
		{
			if (this.Container != null)
			{
				this.Container.RemoveAllTracks();
			}
		}

		// Token: 0x04000195 RID: 405
		public GroupController Container;
	}
}
