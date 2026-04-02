using System;
using System.Collections.Generic;
using System.Linq;
using Cairo;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x0200019C RID: 412
	public class DataSourcesModel
	{
		// Token: 0x060004F0 RID: 1264 RVA: 0x0000B230 File Offset: 0x00009430
		public DataSourcesModel(SettingsModel settingsModel)
		{
			this.m_metricColors = new Dictionary<uint, Dictionary<MetricIDSet, Color>>();
			this.m_expandedCategories = new Dictionary<uint, List<uint>>();
			string text = settingsModel.UserPreferences.RetrieveSetting(UserPreferenceModel.UserPreference.MaxCaptureDurationMs);
			if (text != null)
			{
				this.m_duration = UintConverter.Convert(text);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000B2A4 File Offset: 0x000094A4
		public void Clear(uint captureId)
		{
			object dataSourcesModelMutex = this.m_dataSourcesModelMutex;
			lock (dataSourcesModelMutex)
			{
				if (this.m_metricColors.ContainsKey(captureId))
				{
					this.m_metricColors[captureId].Clear();
				}
			}
			if (this.m_expandedCategories.ContainsKey(captureId))
			{
				this.m_expandedCategories[captureId].Clear();
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000B31C File Offset: 0x0000951C
		public List<uint> GetExpandedCategories(uint captureID)
		{
			if (!this.m_expandedCategories.ContainsKey(captureID))
			{
				this.m_expandedCategories[captureID] = new List<uint>();
			}
			return this.m_expandedCategories[captureID];
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000B34C File Offset: 0x0000954C
		public void AddExpandedCategory(uint captureID, uint categoryID)
		{
			if (!this.m_expandedCategories.ContainsKey(captureID))
			{
				this.m_expandedCategories[captureID] = new List<uint>();
			}
			if (!this.m_expandedCategories[captureID].Contains(categoryID))
			{
				this.m_expandedCategories[captureID].Add(categoryID);
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000B39E File Offset: 0x0000959E
		public void RemoveExpandedCategory(uint captureID, uint categoryID)
		{
			if (!this.m_expandedCategories.ContainsKey(captureID))
			{
				return;
			}
			if (this.m_expandedCategories[captureID].Contains(categoryID))
			{
				this.m_expandedCategories[captureID].Remove(categoryID);
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000B3D6 File Offset: 0x000095D6
		public void CopyExpandedCategories(uint captureIDSource, uint captureIDDest, bool overwrite)
		{
			if (!this.m_expandedCategories.ContainsKey(captureIDSource))
			{
				return;
			}
			if (!overwrite && this.m_expandedCategories.ContainsKey(captureIDDest))
			{
				return;
			}
			this.m_expandedCategories[captureIDDest] = this.m_expandedCategories[captureIDSource];
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000B414 File Offset: 0x00009614
		public void UpdateMetricColor(uint captureID, MetricIDSet metricIDSet, Color color)
		{
			object dataSourcesModelMutex = this.m_dataSourcesModelMutex;
			lock (dataSourcesModelMutex)
			{
				if (!this.m_metricColors.ContainsKey(captureID))
				{
					this.m_metricColors.Add(captureID, new Dictionary<MetricIDSet, Color>(new MetricIDSetComparer()));
				}
				this.m_metricColors[captureID][metricIDSet] = color;
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000B488 File Offset: 0x00009688
		public void AddMetricColor(uint captureID, MetricIDSet metricIDSet, Color color)
		{
			object dataSourcesModelMutex = this.m_dataSourcesModelMutex;
			lock (dataSourcesModelMutex)
			{
				if (!this.m_metricColors.ContainsKey(captureID))
				{
					this.m_metricColors.Add(captureID, new Dictionary<MetricIDSet, Color>(new MetricIDSetComparer()));
				}
				if (!this.m_metricColors[captureID].ContainsKey(metricIDSet))
				{
					this.m_metricColors[captureID][metricIDSet] = color;
				}
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000B510 File Offset: 0x00009710
		public void RemoveMetricColor(uint captureID, MetricIDSet metricIDSet)
		{
			object dataSourcesModelMutex = this.m_dataSourcesModelMutex;
			lock (dataSourcesModelMutex)
			{
				if (this.m_metricColors.ContainsKey(captureID))
				{
					this.m_metricColors[captureID].Remove(metricIDSet);
				}
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0000B570 File Offset: 0x00009770
		public void CopyMetricColors(uint captureIDSource, uint captureIDDest, bool overwrite)
		{
			object dataSourcesModelMutex = this.m_dataSourcesModelMutex;
			lock (dataSourcesModelMutex)
			{
				if (this.m_metricColors.ContainsKey(captureIDSource) && (overwrite || !this.m_metricColors.ContainsKey(captureIDDest)))
				{
					this.m_metricColors[captureIDDest] = this.m_metricColors[captureIDSource];
				}
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000B5E4 File Offset: 0x000097E4
		public Dictionary<MetricIDSet, Color> GetMetricColors(uint captureID)
		{
			object dataSourcesModelMutex = this.m_dataSourcesModelMutex;
			Dictionary<MetricIDSet, Color> dictionary;
			lock (dataSourcesModelMutex)
			{
				if (!this.m_metricColors.ContainsKey(captureID))
				{
					this.m_metricColors[captureID] = new Dictionary<MetricIDSet, Color>(new MetricIDSetComparer());
				}
				dictionary = new Dictionary<MetricIDSet, Color>(this.m_metricColors[captureID]);
			}
			return dictionary;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000B658 File Offset: 0x00009858
		public void RemoveOptions(uint pid)
		{
			object dataSourcesModelMutex = this.m_dataSourcesModelMutex;
			lock (dataSourcesModelMutex)
			{
				this.m_procInfoOptions.Remove(pid);
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000B6A0 File Offset: 0x000098A0
		public void AddOption(uint pid, uint oid)
		{
			object dataSourcesModelMutex = this.m_dataSourcesModelMutex;
			lock (dataSourcesModelMutex)
			{
				HashSet<uint> hashSet;
				if (!this.m_procInfoOptions.TryGetValue(pid, out hashSet))
				{
					hashSet = (this.m_procInfoOptions[pid] = new HashSet<uint>());
				}
				hashSet.Add(oid);
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000B708 File Offset: 0x00009908
		public List<uint> GetOptions(uint pid)
		{
			object dataSourcesModelMutex = this.m_dataSourcesModelMutex;
			List<uint> list;
			lock (dataSourcesModelMutex)
			{
				HashSet<uint> hashSet;
				if (this.m_procInfoOptions.TryGetValue(pid, out hashSet))
				{
					list = hashSet.ToList<uint>();
				}
				else
				{
					list = new List<uint>();
				}
			}
			return list;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0000B764 File Offset: 0x00009964
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x0000B76C File Offset: 0x0000996C
		public bool PerfHintsEnabled
		{
			get
			{
				return this.m_perfHintsEnabled;
			}
			set
			{
				this.m_perfHintsEnabled = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0000B775 File Offset: 0x00009975
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x0000B77D File Offset: 0x0000997D
		public uint CaptureDuration
		{
			get
			{
				return this.m_duration;
			}
			set
			{
				this.m_duration = value;
			}
		}

		// Token: 0x0400061A RID: 1562
		private object m_dataSourcesModelMutex = new object();

		// Token: 0x0400061B RID: 1563
		private Dictionary<uint, Dictionary<MetricIDSet, Color>> m_metricColors;

		// Token: 0x0400061C RID: 1564
		private Dictionary<uint, List<uint>> m_expandedCategories;

		// Token: 0x0400061D RID: 1565
		private Dictionary<uint, HashSet<uint>> m_procInfoOptions = new Dictionary<uint, HashSet<uint>>();

		// Token: 0x0400061E RID: 1566
		public Dictionary<ulong, AppStartSettings> m_procStartSettings = new Dictionary<ulong, AppStartSettings>();

		// Token: 0x0400061F RID: 1567
		private uint m_duration = 2000U;

		// Token: 0x04000620 RID: 1568
		public static readonly uint GLOBAL_CATEGORY_ID = 1U;

		// Token: 0x04000621 RID: 1569
		public static readonly uint PROCESS_CATEGORY_ID = 2U;

		// Token: 0x04000622 RID: 1570
		public static readonly int LAUNCH_PAGE = 0;

		// Token: 0x04000623 RID: 1571
		public static readonly int RUNNING_PAGE = 1;

		// Token: 0x04000624 RID: 1572
		public const string PROCESS_HELP_LABEL = "Vulkan apps must be launched with the Launch Application button.\n \nOpenGL and OpenCL apps must be launched after Profiler is connected or with the Launch Application button.";

		// Token: 0x04000625 RID: 1573
		public const string PROCESS_DETAILED_TOOLTIP = "If an app is running before Profiler connects to the device, it may not show up in this list or it may not provide GPU related metrics.";

		// Token: 0x04000626 RID: 1574
		public const string WRONG_LAYER_RESTART = "This app has {0} profiling enabled. Restart app with {1} profiling instead?";

		// Token: 0x04000627 RID: 1575
		public const uint HELP_TIP_PID = 4294967294U;

		// Token: 0x04000628 RID: 1576
		private bool m_perfHintsEnabled;
	}
}
