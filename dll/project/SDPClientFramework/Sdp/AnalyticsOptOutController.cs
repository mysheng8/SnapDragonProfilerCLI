using System;
using System.Threading.Tasks;

namespace Sdp
{
	// Token: 0x020001AE RID: 430
	public class AnalyticsOptOutController : IDialogController
	{
		// Token: 0x06000548 RID: 1352 RVA: 0x0000C3C7 File Offset: 0x0000A5C7
		public AnalyticsOptOutController(IAnalyticsOptOutDialog dialog)
		{
			this.m_dialog = dialog;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x0000C3D6 File Offset: 0x0000A5D6
		public bool OptOut
		{
			get
			{
				return this.m_dialog == null || this.m_dialog.OptOut;
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000C3F0 File Offset: 0x0000A5F0
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

		// Token: 0x04000655 RID: 1621
		private IAnalyticsOptOutDialog m_dialog;
	}
}
