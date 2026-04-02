using System;
using System.Threading;

namespace Sdp
{
	// Token: 0x02000061 RID: 97
	public class ExportAllMetricCommand : Command
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00007D21 File Offset: 0x00005F21
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00007D29 File Offset: 0x00005F29
		public string path { get; set; }

		// Token: 0x0600023D RID: 573 RVA: 0x00007D34 File Offset: 0x00005F34
		protected override void OnExecute()
		{
			Thread thread = new Thread(new ThreadStart(this.ExportThread));
			thread.Start();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00007D59 File Offset: 0x00005F59
		protected void ExportThread()
		{
			SdpApp.ConnectionManager.ExportAllMetricData(this.path);
		}
	}
}
