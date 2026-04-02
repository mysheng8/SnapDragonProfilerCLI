using System;
using System.Collections.Generic;
using Cairo;
using Sdp;
using Sdp.Charts.Gantt;

namespace TracePlugin
{
	// Token: 0x02000016 RID: 22
	public class SystraceModel
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00006DF6 File Offset: 0x00004FF6
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00006DFE File Offset: 0x00004FFE
		public int CaptureID { get; set; }

		// Token: 0x06000086 RID: 134 RVA: 0x00006E07 File Offset: 0x00005007
		public SystraceModel()
		{
			this.m_taskGroupIDData = new Dictionary<int, Dictionary<string, TaskTrackData>>();
			this.m_colorsModel = new Dictionary<uint, Color>();
			this.m_blockId = 1U;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00006E2C File Offset: 0x0000502C
		public void ClearData()
		{
			this.m_taskGroupIDData.Clear();
			this.m_colorsModel = new Dictionary<uint, Color>();
			this.m_blockId = 1U;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00006E4C File Offset: 0x0000504C
		public uint GetNextBlockID()
		{
			uint blockId = this.m_blockId;
			this.m_blockId = blockId + 1U;
			return blockId;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00006E6A File Offset: 0x0000506A
		public Dictionary<int, Dictionary<string, TaskTrackData>> TaskGroupIDData
		{
			get
			{
				return this.m_taskGroupIDData;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00006E72 File Offset: 0x00005072
		public Dictionary<uint, Color> ColorsModel
		{
			get
			{
				return this.m_colorsModel;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00006E7C File Offset: 0x0000507C
		public void AddElement(int taskGroupID, string taskName, int depth, Element element, string depthName = null)
		{
			if (!this.m_taskGroupIDData.ContainsKey(taskGroupID))
			{
				this.m_taskGroupIDData.Add(taskGroupID, new Dictionary<string, TaskTrackData>());
			}
			if (!this.m_taskGroupIDData[taskGroupID].ContainsKey(taskName))
			{
				this.m_taskGroupIDData[taskGroupID].Add(taskName, new TaskTrackData());
			}
			this.m_taskGroupIDData[taskGroupID][taskName].Name = taskName;
			if (!this.m_taskGroupIDData[taskGroupID][taskName].DepthData.ContainsKey(depth))
			{
				this.m_taskGroupIDData[taskGroupID][taskName].DepthData.Add(depth, new DepthData(depth));
			}
			this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].BlockElements.Add(element);
			if (!string.IsNullOrEmpty(depthName))
			{
				this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].Name = depthName;
			}
			if (element.Start < this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].MinTimestamp)
			{
				this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].MinTimestamp = element.Start;
			}
			if (element.End > this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].MaxTimestamp)
			{
				this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].MaxTimestamp = element.End;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000702C File Offset: 0x0000522C
		public void AddMarker(int taskGroupID, string taskName, int depth, Marker marker, string depthName = null)
		{
			if (!this.m_taskGroupIDData.ContainsKey(taskGroupID))
			{
				this.m_taskGroupIDData.Add(taskGroupID, new Dictionary<string, TaskTrackData>());
			}
			if (!this.m_taskGroupIDData[taskGroupID].ContainsKey(taskName))
			{
				this.m_taskGroupIDData[taskGroupID].Add(taskName, new TaskTrackData());
			}
			this.m_taskGroupIDData[taskGroupID][taskName].Name = taskName;
			if (!this.m_taskGroupIDData[taskGroupID][taskName].DepthData.ContainsKey(depth))
			{
				this.m_taskGroupIDData[taskGroupID][taskName].DepthData.Add(depth, new DepthData(depth));
			}
			this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].MarkerElements.Add(marker);
			if (!string.IsNullOrEmpty(depthName))
			{
				this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].Name = depthName;
			}
			if (marker.Position < this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].MinTimestamp)
			{
				this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].MinTimestamp = marker.Position;
			}
			if (marker.Position > this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].MaxTimestamp)
			{
				this.m_taskGroupIDData[taskGroupID][taskName].DepthData[depth].MaxTimestamp = marker.Position;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000071DC File Offset: 0x000053DC
		public void AddStringToModel(int taskGroupID, string taskName, string name, string tooltip)
		{
			if (!this.m_taskGroupIDData.ContainsKey(taskGroupID))
			{
				return;
			}
			if (!this.m_taskGroupIDData[taskGroupID].ContainsKey(taskName))
			{
				return;
			}
			uint hashCode = (uint)name.GetHashCode();
			if (!string.IsNullOrEmpty(name) && !this.m_taskGroupIDData[taskGroupID][taskName].NameStringModel.ContainsKey(hashCode))
			{
				this.m_taskGroupIDData[taskGroupID][taskName].NameStringModel.Add(hashCode, name);
				this.m_taskGroupIDData[taskGroupID][taskName].NameHashCodesToRender.Add(hashCode);
			}
			uint hashCode2 = (uint)tooltip.GetHashCode();
			if (!string.IsNullOrEmpty(tooltip) && !this.m_taskGroupIDData[taskGroupID][taskName].TooltipStringModel.ContainsKey(hashCode2))
			{
				this.m_taskGroupIDData[taskGroupID][taskName].TooltipStringModel.Add(hashCode2, tooltip);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000072C8 File Offset: 0x000054C8
		public void AddDataPoint(int taskGroupID, string taskName, string trackName, DataPoint point)
		{
			if (!this.m_taskGroupIDData.ContainsKey(taskGroupID))
			{
				this.m_taskGroupIDData.Add(taskGroupID, new Dictionary<string, TaskTrackData>());
			}
			if (!this.m_taskGroupIDData[taskGroupID].ContainsKey(taskName))
			{
				this.m_taskGroupIDData[taskGroupID].Add(taskName, new TaskTrackData());
			}
			if (!this.m_taskGroupIDData[taskGroupID][taskName].DataPointData.ContainsKey(trackName))
			{
				this.m_taskGroupIDData[taskGroupID][taskName].DataPointData.Add(trackName, new GraphData());
			}
			this.m_taskGroupIDData[taskGroupID][taskName].DataPointData[trackName].AddData(point);
		}

		// Token: 0x0400004D RID: 77
		public EventHandler Changed;

		// Token: 0x0400004F RID: 79
		private uint m_blockId;

		// Token: 0x04000050 RID: 80
		private Dictionary<int, Dictionary<string, TaskTrackData>> m_taskGroupIDData;

		// Token: 0x04000051 RID: 81
		private Dictionary<uint, Color> m_colorsModel;
	}
}
