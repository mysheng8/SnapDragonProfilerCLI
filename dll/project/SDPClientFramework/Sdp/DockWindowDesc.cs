using System;

namespace Sdp
{
	// Token: 0x02000195 RID: 405
	[Serializable]
	public class DockWindowDesc
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0000ACC0 File Offset: 0x00008EC0
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x0000ACC8 File Offset: 0x00008EC8
		public string Name { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0000ACD1 File Offset: 0x00008ED1
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x0000ACD9 File Offset: 0x00008ED9
		public ViewDesc ViewSettings { get; set; }
	}
}
