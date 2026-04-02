using System;
using System.Xml;
using System.Xml.Serialization;

namespace Sdp
{
	// Token: 0x02000197 RID: 407
	[Serializable]
	public class UIDesc
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0000ACEA File Offset: 0x00008EEA
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x0000ACF2 File Offset: 0x00008EF2
		public decimal Version { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0000ACFB File Offset: 0x00008EFB
		public DockWindowDescList DockingWindows
		{
			get
			{
				return this.m_docking_windows;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0000AD03 File Offset: 0x00008F03
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x0000AD0B File Offset: 0x00008F0B
		public string CurrentLayout { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0000AD14 File Offset: 0x00008F14
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x0000AD1C File Offset: 0x00008F1C
		public string DefaultLayout { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0000AD25 File Offset: 0x00008F25
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x0000AD2D File Offset: 0x00008F2D
		[XmlIgnore]
		public string DockingXml { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0000AD38 File Offset: 0x00008F38
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x0000AD57 File Offset: 0x00008F57
		[XmlElement("DockingXml")]
		public XmlCDataSection DockingXmlCData
		{
			get
			{
				XmlDocument xmlDocument = new XmlDocument();
				return xmlDocument.CreateCDataSection(this.DockingXml);
			}
			set
			{
				this.DockingXml = value.Value;
			}
		}

		// Token: 0x0400060E RID: 1550
		public static readonly decimal CurrentVersion = 1.5m;

		// Token: 0x04000613 RID: 1555
		private DockWindowDescList m_docking_windows = new DockWindowDescList();
	}
}
