using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cairo;

namespace Sdp
{
	// Token: 0x0200022B RID: 555
	public interface IDataSourcesView : IView, IStatusBox
	{
		// Token: 0x1400005E RID: 94
		// (add) Token: 0x060008B3 RID: 2227
		// (remove) Token: 0x060008B4 RID: 2228
		event EventHandler<SelectedProcessChangedArgs> SelectedProcessesChanged;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x060008B5 RID: 2229
		// (remove) Token: 0x060008B6 RID: 2230
		event EventHandler<MetricToggledEventArgs> MetricToggled;

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x060008B7 RID: 2231
		// (remove) Token: 0x060008B8 RID: 2232
		event EventHandler<MetricDoubleClickedEventArgs> MetricDoubleClicked;

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x060008B9 RID: 2233
		// (remove) Token: 0x060008BA RID: 2234
		event EventHandler<MetricBeginDragArgs> MetricDragBegin;

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x060008BB RID: 2235
		// (remove) Token: 0x060008BC RID: 2236
		event EventHandler<MetricColorArgs> AddMetricColor;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x060008BD RID: 2237
		// (remove) Token: 0x060008BE RID: 2238
		event EventHandler<MetricColorArgs> RemoveMetricColor;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x060008BF RID: 2239
		// (remove) Token: 0x060008C0 RID: 2240
		event EventHandler RequestMetricRecolor;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x060008C1 RID: 2241
		// (remove) Token: 0x060008C2 RID: 2242
		event EventHandler<MetricCategoryArgs> MetricCategoryExpanded;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x060008C3 RID: 2243
		// (remove) Token: 0x060008C4 RID: 2244
		event EventHandler<MetricCategoryArgs> MetricCategoryCollapsed;

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x060008C5 RID: 2245
		// (remove) Token: 0x060008C6 RID: 2246
		event EventHandler<FilterEntryChangedArgs> FilterEntryChanged;

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x060008C7 RID: 2247
		// (remove) Token: 0x060008C8 RID: 2248
		event EventHandler MetricDragEnd;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x060008C9 RID: 2249
		// (remove) Token: 0x060008CA RID: 2250
		event EventHandler LaunchAppClicked;

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x060008CB RID: 2251
		// (remove) Token: 0x060008CC RID: 2252
		event EventHandler EnableMetricClicked;

		// Token: 0x060008CD RID: 2253
		void AddMetric(bool isGlobal, DataSourcesViewMetric metric, Dictionary<uint, List<DataSourcesViewMetric>> parents, List<uint> expandedCategories);

		// Token: 0x060008CE RID: 2254
		void InvalidateMetrics(Dictionary<uint, List<DataSourcesViewMetric>> global, Dictionary<uint, List<DataSourcesViewMetric>> perProcess, List<uint> expandedCategories);

		// Token: 0x060008CF RID: 2255
		void InvalidateProcesses(List<IdNamePair> processes, List<IdNamePair> selectedProcesses, CaptureType captureType);

		// Token: 0x060008D0 RID: 2256
		void UpdateMetricEnabledStatus(uint id, bool enabled, uint pid, Dictionary<MetricIDSet, Color> metricColors, uint captureID);

		// Token: 0x060008D1 RID: 2257
		void UpdateMetricHidden(uint metricID, bool isHidden);

		// Token: 0x060008D2 RID: 2258
		void RecolorMetricList(Dictionary<MetricIDSet, Color> metricColors, List<IdNamePair> selectedProcesses);

		// Token: 0x060008D3 RID: 2259
		void SetSelectedProcess(IdNamePair processName);

		// Token: 0x060008D4 RID: 2260
		void SetFilterEntry(string entry);

		// Token: 0x060008D5 RID: 2261
		string GetFilterEntry();

		// Token: 0x060008D6 RID: 2262
		List<uint> GetMetricIdsByCategory(uint categoryId);

		// Token: 0x170001AD RID: 429
		// (set) Token: 0x060008D7 RID: 2263
		bool ReadOnly { set; }

		// Token: 0x170001AE RID: 430
		// (set) Token: 0x060008D8 RID: 2264
		bool MetricButtonVisible { set; }

		// Token: 0x060008D9 RID: 2265
		void SetName(CaptureType captureType, int sessionNumber);

		// Token: 0x060008DA RID: 2266
		void ExpandCategory(bool expand, string path, CaptureType captureType);

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060008DB RID: 2267
		Task<DataSourcesViewModel> ViewModel { get; }
	}
}
