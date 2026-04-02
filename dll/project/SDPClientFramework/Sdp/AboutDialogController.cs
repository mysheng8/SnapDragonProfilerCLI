using System;
using System.Threading.Tasks;

namespace Sdp
{
	// Token: 0x02000225 RID: 549
	public class AboutDialogController : IDialogController
	{
		// Token: 0x06000863 RID: 2147 RVA: 0x00016E60 File Offset: 0x00015060
		public AboutDialogController(IAboutDialog dialog)
		{
			this.m_dialog = dialog;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00016E70 File Offset: 0x00015070
		public async Task<bool> ShowDialog()
		{
			bool flag = false;
			if (this.m_dialog != null)
			{
				bool flag2 = await this.m_dialog.ShowDialog();
				flag = flag2;
			}
			return flag;
		}

		// Token: 0x040007D0 RID: 2000
		private IAboutDialog m_dialog;
	}
}
