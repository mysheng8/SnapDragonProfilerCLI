using System;
using System.Collections.Generic;
using Cairo;

namespace Sdp.Charts.Gantt
{
	// Token: 0x020002F2 RID: 754
	public class Series
	{
		// Token: 0x06000F53 RID: 3923 RVA: 0x0002FC52 File Offset: 0x0002DE52
		public Series()
		{
			this.Children = new List<Series>();
			this.Elements = new List<Element>();
			this.Markers = new List<Marker>();
			this.BorderWidth = 0;
			this.IsExpanded = false;
			this.IgnoreFromSummary = false;
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0002FC90 File Offset: 0x0002DE90
		// (set) Token: 0x06000F55 RID: 3925 RVA: 0x0002FC98 File Offset: 0x0002DE98
		public bool IsExpanded
		{
			get
			{
				return this.m_isExpanded;
			}
			set
			{
				this.m_isExpanded = value;
				foreach (Series series in this.Children)
				{
					series.IsExpanded = false;
				}
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0002FCF4 File Offset: 0x0002DEF4
		// (set) Token: 0x06000F57 RID: 3927 RVA: 0x0002FCFC File Offset: 0x0002DEFC
		public bool IgnoreFromSummary { get; set; }

		// Token: 0x06000F58 RID: 3928 RVA: 0x0002FD05 File Offset: 0x0002DF05
		public bool IsAnnotation()
		{
			return this.Name == "Annotations";
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0002FD17 File Offset: 0x0002DF17
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x0002FD1F File Offset: 0x0002DF1F
		public string Name { get; set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x0002FD28 File Offset: 0x0002DF28
		// (set) Token: 0x06000F5C RID: 3932 RVA: 0x0002FD30 File Offset: 0x0002DF30
		public List<Series> Children { get; set; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0002FD39 File Offset: 0x0002DF39
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x0002FD41 File Offset: 0x0002DF41
		public List<Element> Elements { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0002FD4A File Offset: 0x0002DF4A
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x0002FD52 File Offset: 0x0002DF52
		public List<Marker> Markers { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0002FD5B File Offset: 0x0002DF5B
		// (set) Token: 0x06000F62 RID: 3938 RVA: 0x0002FD63 File Offset: 0x0002DF63
		public Color DefaultBackColor { get; set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0002FD6C File Offset: 0x0002DF6C
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x0002FD74 File Offset: 0x0002DF74
		public Color BorderColor { get; set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0002FD7D File Offset: 0x0002DF7D
		// (set) Token: 0x06000F66 RID: 3942 RVA: 0x0002FD85 File Offset: 0x0002DF85
		public int BorderWidth { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0002FD8E File Offset: 0x0002DF8E
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x0002FD96 File Offset: 0x0002DF96
		public int LastYOffset { get; set; }

		// Token: 0x04000A7A RID: 2682
		private bool m_isExpanded;

		// Token: 0x04000A7C RID: 2684
		public object Tag;
	}
}
