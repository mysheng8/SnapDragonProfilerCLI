using System;

namespace Sdp
{
	// Token: 0x02000080 RID: 128
	internal class RenameLayoutCommand : Command
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00008ECD File Offset: 0x000070CD
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x00008ED5 File Offset: 0x000070D5
		public string OriginalName
		{
			get
			{
				return this.m_original_name;
			}
			set
			{
				this.m_original_name = value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00008EDE File Offset: 0x000070DE
		// (set) Token: 0x060002CA RID: 714 RVA: 0x00008EE6 File Offset: 0x000070E6
		public string NewName
		{
			get
			{
				return this.m_new_name;
			}
			set
			{
				this.m_new_name = value;
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00008EEF File Offset: 0x000070EF
		public RenameLayoutCommand()
		{
			this.UIName = "Rename Layout";
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00008AE7 File Offset: 0x00006CE7
		protected override void OnExecute()
		{
			this.OnRedo();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00008F18 File Offset: 0x00007118
		protected override void OnUndo()
		{
			UIManager uimanager = SdpApp.UIManager;
			if (uimanager != null)
			{
				uimanager.RenameLayout(this.m_new_name, this.m_original_name);
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00008F40 File Offset: 0x00007140
		protected override void OnRedo()
		{
			UIManager uimanager = SdpApp.UIManager;
			if (uimanager != null)
			{
				uimanager.RenameLayout(this.m_original_name, this.m_new_name);
			}
		}

		// Token: 0x040001C1 RID: 449
		private string m_original_name = "";

		// Token: 0x040001C2 RID: 450
		private string m_new_name = "";
	}
}
