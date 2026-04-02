using System;

namespace Sdp
{
	// Token: 0x02000057 RID: 87
	public class AddGroupCommand : Command
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00007300 File Offset: 0x00005500
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00007308 File Offset: 0x00005508
		public string GroupName { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00007311 File Offset: 0x00005511
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00007319 File Offset: 0x00005519
		public GroupLayoutController Container { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007322 File Offset: 0x00005522
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000732A File Offset: 0x0000552A
		public GroupController Result { get; set; }

		// Token: 0x060001E7 RID: 487 RVA: 0x00007333 File Offset: 0x00005533
		protected override void OnExecute()
		{
			if (this.Container != null)
			{
				this.Result = this.Container.AddGroup(this.GroupName);
			}
		}
	}
}
