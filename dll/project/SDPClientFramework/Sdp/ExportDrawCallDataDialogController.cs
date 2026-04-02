using System;
using System.Threading.Tasks;

namespace Sdp
{
	// Token: 0x020001BC RID: 444
	public class ExportDrawCallDataDialogController : IDialogController
	{
		// Token: 0x060005B5 RID: 1461 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		public async Task<bool> ShowDialog()
		{
			return await this.m_view.ShowDialog();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0000D7EF File Offset: 0x0000B9EF
		public ExportDrawCallDataDialogController(IExportDrawCallDataDialog view)
		{
			this.m_view = view;
		}

		// Token: 0x0400067A RID: 1658
		private IExportDrawCallDataDialog m_view;
	}
}
