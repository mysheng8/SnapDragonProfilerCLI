using System;
using Sdp;

namespace TracePlugin
{
	// Token: 0x02000018 RID: 24
	public class GraphData
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000073FC File Offset: 0x000055FC
		public DataPointList DataPointList
		{
			get
			{
				return this.m_dataPointList;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00007404 File Offset: 0x00005604
		public GraphData()
		{
			this.m_dataPointList = new DataPointList();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00007417 File Offset: 0x00005617
		public void AddData(DataPoint point)
		{
			this.m_dataPointList.AddData(point);
		}

		// Token: 0x04000058 RID: 88
		private DataPointList m_dataPointList;
	}
}
