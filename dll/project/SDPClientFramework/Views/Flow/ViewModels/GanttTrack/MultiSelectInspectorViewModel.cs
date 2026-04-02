using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sdp;
using Sdp.Helpers;

namespace SDPClientFramework.Views.Flow.ViewModels.GanttTrack
{
	// Token: 0x02000024 RID: 36
	public class MultiSelectInspectorViewModel : InspectorViewModel
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x0000354C File Offset: 0x0000174C
		public InspectorViewDisplayEventArgs ToEventArgs()
		{
			this.SeriesVMs = this.SeriesVMs.OrderByDescending((SeriesInspectorViewModel seriesVM) => seriesVM.AccumulatedDuration);
			IEnumerable<PropertyDescriptor> enumerable = this.CreateTotalSelectionProperties();
			IEnumerable<PropertyDescriptor> enumerable2 = this.SeriesVMs.SelectMany(new Func<SeriesInspectorViewModel, IEnumerable<PropertyDescriptor>>(this.CreateSeriesSelectionProperties));
			IEnumerable<PropertyDescriptor> enumerable3 = enumerable.Concat(enumerable2);
			return new InspectorViewDisplayEventArgs
			{
				Content = new PropertyGridDescriptionObject(enumerable3.ToList<PropertyDescriptor>()),
				Description = "GanttTrack"
			};
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000035D4 File Offset: 0x000017D4
		private IEnumerable<PropertyDescriptor> CreateTotalSelectionProperties()
		{
			List<PropertyDescriptor> list = new List<PropertyDescriptor>
			{
				new SdpPropertyDescriptor<string>("Time Duration", typeof(string), FormatHelper.FormatTimeLabel(this.TimeDuration, "#.##", "#.###"), "Total Selection", "Overall start to end time", true),
				new SdpPropertyDescriptor<string>("   Start Time", typeof(string), FormatHelper.FormatTimeLabel(this.StartTime, "#.##", "#.###"), "Total Selection", "Start time of selection", true),
				new SdpPropertyDescriptor<string>("   End Time", typeof(string), FormatHelper.FormatTimeLabel(this.EndTime, "#.##", "#.###"), "Total Selection", "End time of selection", true)
			};
			foreach (SeriesInspectorViewModel seriesInspectorViewModel in this.SeriesVMs)
			{
				double num = (double)seriesInspectorViewModel.AccumulatedDuration / (double)this.TimeDuration;
				list.Add(new SdpPropertyDescriptor<string>(string.Format("{0} [{1}]", seriesInspectorViewModel.SeriesName, seriesInspectorViewModel.NumberSelected), typeof(string), FormatHelper.FormatTimeLabel(seriesInspectorViewModel.AccumulatedDuration, "#.##", "#.###"), "Total Selection", "Accumulated duration for all selected " + seriesInspectorViewModel.SeriesName + " blocks", true));
			}
			return list;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003744 File Offset: 0x00001944
		private IEnumerable<PropertyDescriptor> CreateSeriesSelectionProperties(SeriesInspectorViewModel seriesVM)
		{
			return new List<PropertyDescriptor>
			{
				new SdpPropertyDescriptor<int>("Number Selected", typeof(int), seriesVM.NumberSelected, seriesVM.SeriesName, "Number of selected " + seriesVM.SeriesName + " blocks", true),
				new SdpPropertyDescriptor<string>("Accumulated Duration", typeof(string), FormatHelper.FormatTimeLabel(seriesVM.AccumulatedDuration, "#.##", "#.###"), seriesVM.SeriesName, "Accumulated duration for all selected " + seriesVM.SeriesName + " blocks", true)
			};
		}

		// Token: 0x040000C7 RID: 199
		public long TimeDuration;

		// Token: 0x040000C8 RID: 200
		public long StartTime;

		// Token: 0x040000C9 RID: 201
		public long EndTime;

		// Token: 0x040000CA RID: 202
		public IEnumerable<SeriesInspectorViewModel> SeriesVMs;

		// Token: 0x040000CB RID: 203
		public IEnumerable<SeriesSelection> SeriesSelections;
	}
}
