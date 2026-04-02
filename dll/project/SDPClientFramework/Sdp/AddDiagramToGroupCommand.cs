using System;

namespace Sdp
{
	// Token: 0x02000087 RID: 135
	public class AddDiagramToGroupCommand : Command
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000944B File Offset: 0x0000764B
		// (set) Token: 0x060002FE RID: 766 RVA: 0x00009453 File Offset: 0x00007653
		public GroupController Container { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000945C File Offset: 0x0000765C
		// (set) Token: 0x06000300 RID: 768 RVA: 0x00009464 File Offset: 0x00007664
		public DiagramType DiagramType { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000946D File Offset: 0x0000766D
		// (set) Token: 0x06000302 RID: 770 RVA: 0x00009475 File Offset: 0x00007675
		public DiagramControllerBase Result { get; set; }

		// Token: 0x06000303 RID: 771 RVA: 0x0000947E File Offset: 0x0000767E
		protected override void OnExecute()
		{
			if (this.Container != null)
			{
				this.Result = this.Container.AddDiagram(this.DiagramType, this.Data);
			}
		}

		// Token: 0x040001D6 RID: 470
		public object Data;
	}
}
