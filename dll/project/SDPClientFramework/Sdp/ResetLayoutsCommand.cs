using System;

namespace Sdp
{
	// Token: 0x02000081 RID: 129
	internal class ResetLayoutsCommand : Command
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public override bool ClearsUndo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00008F68 File Offset: 0x00007168
		public ResetLayoutsCommand()
		{
			this.UIName = "Reset Layouts";
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00008F7C File Offset: 0x0000717C
		protected override void OnExecute()
		{
			UIManager uimanager = SdpApp.UIManager;
			if (uimanager != null)
			{
				uimanager.ConfirmResetLayouts();
			}
		}
	}
}
