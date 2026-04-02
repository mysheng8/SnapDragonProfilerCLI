using System;

namespace Sdp
{
	// Token: 0x0200007D RID: 125
	public class RemoveGroupCommand : Command
	{
		// Token: 0x060002B5 RID: 693 RVA: 0x00008D84 File Offset: 0x00006F84
		protected override void OnExecute()
		{
			if (this.Container != null && this.Group != null)
			{
				this.Container.RemoveGroup(this.Group);
			}
		}

		// Token: 0x040001B7 RID: 439
		public GroupLayoutController Container;

		// Token: 0x040001B8 RID: 440
		public GroupController Group;
	}
}
