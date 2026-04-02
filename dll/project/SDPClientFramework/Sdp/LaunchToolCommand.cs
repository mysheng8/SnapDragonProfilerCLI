using System;

namespace Sdp
{
	// Token: 0x02000064 RID: 100
	internal class LaunchToolCommand : Command
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00007DBF File Offset: 0x00005FBF
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00007DC7 File Offset: 0x00005FC7
		public IToolPlugin ToolPlugin { get; set; }

		// Token: 0x06000246 RID: 582 RVA: 0x00007DD0 File Offset: 0x00005FD0
		protected override void OnExecute()
		{
			if (this.ToolPlugin != null)
			{
				this.ToolPlugin.Launch();
			}
		}
	}
}
