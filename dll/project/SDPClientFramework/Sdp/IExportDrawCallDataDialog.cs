using System;

namespace Sdp
{
	// Token: 0x020001BD RID: 445
	public interface IExportDrawCallDataDialog : IDialog
	{
		// Token: 0x060005B7 RID: 1463
		void SetAttributes(string[] attributes);

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060005B8 RID: 1464
		string PositionSelection { get; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060005B9 RID: 1465
		string NormalSelection { get; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060005BA RID: 1466
		string TexCoordSelection { get; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060005BB RID: 1467
		string SaveFileLocation { get; }
	}
}
