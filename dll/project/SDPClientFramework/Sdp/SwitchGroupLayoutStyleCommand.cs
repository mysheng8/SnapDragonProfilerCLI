using System;

namespace Sdp
{
	// Token: 0x02000088 RID: 136
	public class SwitchGroupLayoutStyleCommand : Command
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000305 RID: 773 RVA: 0x000094A5 File Offset: 0x000076A5
		// (set) Token: 0x06000306 RID: 774 RVA: 0x000094AD File Offset: 0x000076AD
		public GroupController Container { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000307 RID: 775 RVA: 0x000094B6 File Offset: 0x000076B6
		// (set) Token: 0x06000308 RID: 776 RVA: 0x000094BE File Offset: 0x000076BE
		public GroupLayoutStyleType ViewType { get; set; }

		// Token: 0x06000309 RID: 777 RVA: 0x000094C7 File Offset: 0x000076C7
		protected override void OnExecute()
		{
			if (this.Container != null)
			{
				this.Container.SwitchView(this.ViewType);
			}
		}
	}
}
