using System;
using System.Collections.Generic;
using Cairo;

namespace Sdp
{
	// Token: 0x02000246 RID: 582
	public class ICGanttTrackController : TrackControllerBase
	{
		// Token: 0x06000966 RID: 2406 RVA: 0x0001B920 File Offset: 0x00019B20
		public ICGanttTrackController(ITrackViewBase view, GroupLayoutController layoutContainer, GroupController groupContainer)
			: base(view, layoutContainer, groupContainer)
		{
			this.m_metrics = new List<uint>();
			ICEvents icevents = SdpApp.EventsManager.ICEvents;
			icevents.DataProcessed = (EventHandler)Delegate.Combine(icevents.DataProcessed, new EventHandler(this.icEvents_DataProcessed));
			IICGanttTrackView iicganttTrackView = view as IICGanttTrackView;
			iicganttTrackView.NameStringsModel = SdpApp.ModelManager.InstrumentedCodeModel.Functions;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0001B988 File Offset: 0x00019B88
		public override void AddMetric(uint metricId, string metricName, uint pid, bool isPreview, string tooltip, bool isCustom, Color? color)
		{
			this.m_processId = pid;
			this.m_metrics.Add(metricId);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00008AEF File Offset: 0x00006CEF
		public override void RemoveMetric(uint metricId, string metricName, uint pid, bool forceDeleteTrackIfEmpty, bool isPreview)
		{
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00008AD4 File Offset: 0x00006CD4
		public override bool ContainsMetric(MetricDesc desc)
		{
			return false;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001AE5C File Offset: 0x0001905C
		public override int MetricCount()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001B9A0 File Offset: 0x00019BA0
		private void icEvents_DataProcessed(object sender, EventArgs e)
		{
			IICGanttTrackView iicganttTrackView = this.m_view as IICGanttTrackView;
			ICProcessInfo processInfo = SdpApp.ICProcessor.GetProcessInfo(this.m_processId);
			if (processInfo != null)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				foreach (uint num in this.m_metrics)
				{
					string name = SdpApp.ConnectionManager.GetMetricByID(num).GetProperties().name;
					if (string.Compare(name, "Function Trace") == 0)
					{
						flag = true;
					}
					if (string.Compare(name, "Debug Markers") == 0)
					{
						flag2 = true;
					}
					if (string.Compare(name, "Region Trace") == 0)
					{
						flag3 = true;
					}
				}
				foreach (KeyValuePair<uint, ICThreadInfo> keyValuePair in processInfo.FunctionBreakdowns)
				{
					uint key = keyValuePair.Key;
					ICThreadInfo value = keyValuePair.Value;
					if (flag)
					{
						List<List<ICFunctionBreakdown>> list = new List<List<ICFunctionBreakdown>>();
						for (int i = value.MinDepth; i <= value.MaxDepth; i++)
						{
							list.Add(new List<ICFunctionBreakdown>());
						}
						foreach (ICFunctionBreakdown icfunctionBreakdown in value.Functions)
						{
							int num2 = icfunctionBreakdown.RelativeDepth - value.MinDepth;
							list[num2].Add(icfunctionBreakdown);
						}
						iicganttTrackView.AddThreadData(key, list);
					}
					if (flag2)
					{
						iicganttTrackView.AddThreadMarkers(key, value.Markers);
					}
				}
				if (flag3)
				{
					List<ICEventRegion> list2 = new List<ICEventRegion>();
					foreach (KeyValuePair<uint, List<ICEventRegion>> keyValuePair2 in processInfo.DebugRegions)
					{
						foreach (ICEventRegion iceventRegion in keyValuePair2.Value)
						{
							if (iceventRegion.Complete)
							{
								list2.Add(iceventRegion);
							}
						}
					}
					iicganttTrackView.AddRegions(list2);
				}
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001BC0C File Offset: 0x00019E0C
		public override TrackViewDesc SaveSettings()
		{
			GanttTrackViewDesc ganttTrackViewDesc = new GanttTrackViewDesc();
			base.SaveCommonSettings(ganttTrackViewDesc);
			ganttTrackViewDesc.TrackType = "Gantt";
			ganttTrackViewDesc.ProcessId = this.m_processId;
			ganttTrackViewDesc.MetricIds = new List<uint>();
			foreach (uint num in this.m_metrics)
			{
				ganttTrackViewDesc.MetricIds.Add(num);
			}
			return ganttTrackViewDesc;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001BC94 File Offset: 0x00019E94
		public override void LoadSettings(TrackViewDesc track_desc)
		{
			base.LoadSettings(track_desc);
		}

		// Token: 0x04000821 RID: 2081
		private uint m_processId;

		// Token: 0x04000822 RID: 2082
		private List<uint> m_metrics;
	}
}
