using System;
using System.Collections.Generic;

namespace TracePlugin
{
	// Token: 0x02000017 RID: 23
	public class TaskTrackData
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00007384 File Offset: 0x00005584
		// (set) Token: 0x06000090 RID: 144 RVA: 0x0000738C File Offset: 0x0000558C
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00007395 File Offset: 0x00005595
		public Dictionary<int, DepthData> DepthData
		{
			get
			{
				return this.m_depthData;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000739D File Offset: 0x0000559D
		public Dictionary<string, GraphData> DataPointData
		{
			get
			{
				return this.m_dataPointLists;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000073A5 File Offset: 0x000055A5
		public Dictionary<uint, string> NameStringModel
		{
			get
			{
				return this.m_nameStringModel;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000073AD File Offset: 0x000055AD
		public Dictionary<uint, string> TooltipStringModel
		{
			get
			{
				return this.m_tooltipStringModel;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000073B5 File Offset: 0x000055B5
		public List<uint> NameHashCodesToRender
		{
			get
			{
				return this.m_nameHashCodesToRender;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000073BD File Offset: 0x000055BD
		public TaskTrackData()
		{
			this.m_depthData = new Dictionary<int, DepthData>();
			this.m_dataPointLists = new Dictionary<string, GraphData>();
			this.m_nameStringModel = new Dictionary<uint, string>();
			this.m_tooltipStringModel = new Dictionary<uint, string>();
			this.m_nameHashCodesToRender = new List<uint>();
		}

		// Token: 0x04000052 RID: 82
		private Dictionary<string, GraphData> m_dataPointLists;

		// Token: 0x04000053 RID: 83
		private Dictionary<int, DepthData> m_depthData;

		// Token: 0x04000054 RID: 84
		private Dictionary<uint, string> m_nameStringModel;

		// Token: 0x04000055 RID: 85
		private Dictionary<uint, string> m_tooltipStringModel;

		// Token: 0x04000056 RID: 86
		private List<uint> m_nameHashCodesToRender;

		// Token: 0x04000057 RID: 87
		private string m_name;
	}
}
