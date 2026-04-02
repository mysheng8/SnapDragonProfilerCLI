using System;
using System.Threading.Tasks;

namespace Sdp
{
	// Token: 0x020001F2 RID: 498
	public class LicensesDialogController : IDialogController
	{
		// Token: 0x06000735 RID: 1845 RVA: 0x000130B4 File Offset: 0x000112B4
		public LicensesDialogController(ILicensesDialog dialog)
		{
			this.m_dialog = dialog;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000130C4 File Offset: 0x000112C4
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

		// Token: 0x0400071F RID: 1823
		private ILicensesDialog m_dialog;
	}
}
