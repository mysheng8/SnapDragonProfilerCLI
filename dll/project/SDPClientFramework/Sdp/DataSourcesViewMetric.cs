using System;

namespace Sdp
{
	// Token: 0x0200022E RID: 558
	public class DataSourcesViewMetric
	{
		// Token: 0x060008DE RID: 2270 RVA: 0x0001A5F8 File Offset: 0x000187F8
		public DataSourcesViewMetric(uint id, string name, bool enabled, string description, bool hidden, uint categoryID, string displayName, bool isCategory)
		{
			this.Id = id;
			this.Name = name;
			this.Enabled = enabled;
			this.Tooltip = description;
			this.Hidden = hidden;
			this.ParentId = categoryID;
			this.DisplayName = displayName;
			this.Category = isCategory;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001A648 File Offset: 0x00018848
		public DataSourcesViewMetric(Metric metric, bool enabled)
		{
			MetricProperties properties = metric.GetProperties();
			this.Id = properties.id;
			this.Name = properties.name;
			this.Enabled = enabled;
			this.Tooltip = properties.description;
			this.Hidden = properties.hidden;
			this.ParentId = properties.categoryID;
			MetricDescription metricDescription = new MetricDescription(metric);
			IMetricPlugin metricPlugin = SdpApp.PluginManager.GetMetricPlugin(metricDescription);
			if (metricPlugin != null)
			{
				this.DisplayName = metricPlugin.MetricDisplayName(metric);
				return;
			}
			this.DisplayName = this.Name;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001A6D8 File Offset: 0x000188D8
		public DataSourcesViewMetric(MetricCategory category, bool global)
		{
			MetricCategoryProperties properties = category.GetProperties();
			if (string.IsNullOrEmpty(properties.name))
			{
				this.Name = "Uncategorized";
			}
			else
			{
				this.Name = properties.name;
			}
			this.DisplayName = this.Name;
			this.Tooltip = properties.description;
			if (properties.parent == 0U)
			{
				this.ParentId = (global ? DataSourcesModel.GLOBAL_CATEGORY_ID : DataSourcesModel.PROCESS_CATEGORY_ID);
			}
			else
			{
				this.ParentId = properties.parent;
			}
			this.Id = properties.id;
			this.Category = true;
		}

		// Token: 0x040007E1 RID: 2017
		public uint Id;

		// Token: 0x040007E2 RID: 2018
		public string Name;

		// Token: 0x040007E3 RID: 2019
		public string Tooltip;

		// Token: 0x040007E4 RID: 2020
		public string DisplayName;

		// Token: 0x040007E5 RID: 2021
		public bool Enabled;

		// Token: 0x040007E6 RID: 2022
		public bool Hidden;

		// Token: 0x040007E7 RID: 2023
		public uint ParentId;

		// Token: 0x040007E8 RID: 2024
		public bool Category;
	}
}
