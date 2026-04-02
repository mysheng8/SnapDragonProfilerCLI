using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Sdp
{
	// Token: 0x020001ED RID: 493
	internal class StatisticsController : IDialogController
	{
		// Token: 0x06000707 RID: 1799 RVA: 0x000129BC File Offset: 0x00010BBC
		public StatisticsController(IStatisticsView view)
		{
			this.m_view = view;
			this.m_view.SelectedCaptureChanged += this.m_view_SelectedCaptureChanged;
			this.m_view.SelectedStatisticChanged += this.m_view_SelectedStatisticChanged;
			this.m_view.ShowAllToggled += this.m_view_ShowAllToggled;
			this.m_view.ZoomIn += this.m_view_ZoomIn;
			this.m_view.ZoomOut += this.m_view_ZoomOut;
			this.m_view.ResetViewBounds += this.m_view_ResetViewBounds;
			StatisticEvents statisticEvents = SdpApp.EventsManager.StatisticEvents;
			statisticEvents.StatisticPerCaptureAdded = (EventHandler<StatisticPerCaptureArgs>)Delegate.Combine(statisticEvents.StatisticPerCaptureAdded, new EventHandler<StatisticPerCaptureArgs>(this.statisticEvents_StatisticPerCaptureAdded));
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00012A98 File Offset: 0x00010C98
		public async Task<bool> ShowDialog()
		{
			TaskAwaiter<bool> taskAwaiter = this.m_view.ShowDialog().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				this.InvalidateCapturesList();
			}
			return true;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00012ADC File Offset: 0x00010CDC
		private void InvalidateCapturesList()
		{
			this.m_capturesWithStatistics.Clear();
			if (SdpApp.ModelManager.StatisticsModel.StatisticsStatePerCapture != null)
			{
				foreach (int num in SdpApp.ModelManager.StatisticsModel.StatisticsStatePerCapture.Keys)
				{
					this.NewCaptureAvailable(num);
				}
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00012B5C File Offset: 0x00010D5C
		private void NewCaptureAvailable(int captureID)
		{
			Capture capture = CaptureManager.Get().GetCapture((uint)captureID);
			this.m_view.AddCapture(captureID, (CaptureType)capture.GetProperties().captureType);
			this.m_capturesWithStatistics.Add(captureID, (CaptureType)capture.GetProperties().captureType);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00012BA4 File Offset: 0x00010DA4
		private void InvalidateStatisticsList(bool clearSelection)
		{
			Dictionary<IStatistic, StatisticState> dictionary = new Dictionary<IStatistic, StatisticState>();
			HashSet<IStatistic> hashSet = null;
			SdpApp.ModelManager.StatisticsModel.StatisticsStatePerCapture.TryGetValue(this.m_view.SelectedCaptureID, out hashSet);
			foreach (IStatistic statistic in SdpApp.ModelManager.StatisticsModel.Statistics)
			{
				if (hashSet == null || !hashSet.Contains(statistic))
				{
					dictionary.Add(statistic, StatisticState.Unavailable);
				}
				else
				{
					dictionary.Add(statistic, StatisticState.Available);
				}
			}
			IStatistic statistic2 = (clearSelection ? null : this.m_view.SelectedStatistic);
			bool flag = false;
			if (this.m_currentStats != null && statistic2 != null)
			{
				flag = this.m_currentStats[statistic2] != dictionary[statistic2];
			}
			this.m_currentStats = dictionary;
			this.m_view.InvalidateStatisticsList(dictionary, statistic2);
			if (flag)
			{
				this.SelectedStatisticStateChanged(statistic2);
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00012C9C File Offset: 0x00010E9C
		private void SelectedStatisticStateChanged(IStatistic stat)
		{
			int captureID = this.m_view.SelectedCaptureID;
			this.m_view.ClearOutputArea();
			if (stat != null)
			{
				StatisticState statisticState;
				if (this.m_currentStats != null && this.m_currentStats.TryGetValue(stat, out statisticState))
				{
					if (statisticState == StatisticState.Unavailable)
					{
						this.m_view.SelectedDataPage = DataPage.Unavailable;
						return;
					}
					if (statisticState == StatisticState.Available)
					{
						if (this.m_worker != null)
						{
							this.m_worker.Join();
						}
						this.m_view.SelectedDataPage = DataPage.Waiting;
						this.m_view.StatisticsListSensitive = false;
						IStatisticDisplayViewModel[] statVM = null;
						this.m_worker = new Thread(delegate
						{
							statVM = stat.GenerateViewModels(captureID);
							this.m_view.InvalidateOutputArea(stat, statVM);
							this.m_view.SelectedDataPage = DataPage.Available;
							this.m_view.StatisticsListSensitive = true;
						});
						this.m_worker.Start();
						return;
					}
				}
			}
			else
			{
				this.m_view.SelectedDataPage = DataPage.Start;
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00012D79 File Offset: 0x00010F79
		private void m_view_ShowAllToggled(object sender, EventArgs e)
		{
			this.InvalidateStatisticsList(false);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00012D82 File Offset: 0x00010F82
		private void m_view_ZoomIn(object sender, ViewBoundsEventArgs e)
		{
			this.m_view.SetViewBounds(1);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00012D90 File Offset: 0x00010F90
		private void m_view_ZoomOut(object sender, ViewBoundsEventArgs e)
		{
			this.m_view.SetViewBounds(-1);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00012D9E File Offset: 0x00010F9E
		private void m_view_ResetViewBounds(object sender, EventArgs e)
		{
			this.m_view.SetViewBounds(0);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00012DAC File Offset: 0x00010FAC
		private void statisticEvents_StatisticPerCaptureAdded(object sender, StatisticPerCaptureArgs e)
		{
			if (!this.m_capturesWithStatistics.ContainsKey(e.CaptureID))
			{
				this.NewCaptureAvailable(e.CaptureID);
				return;
			}
			if (this.m_view.SelectedCaptureID == e.CaptureID)
			{
				this.InvalidateStatisticsList(false);
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00012DE8 File Offset: 0x00010FE8
		private void m_view_SelectedStatisticChanged(object sender, EventArgs e)
		{
			IStatistic selectedStatistic = this.m_view.SelectedStatistic;
			this.SelectedStatisticStateChanged(selectedStatistic);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00012E08 File Offset: 0x00011008
		private void m_view_SelectedCaptureChanged(object sender, EventArgs e)
		{
			this.SelectedStatisticStateChanged(null);
			this.InvalidateStatisticsList(true);
		}

		// Token: 0x04000714 RID: 1812
		private IStatisticsView m_view;

		// Token: 0x04000715 RID: 1813
		private readonly Dictionary<int, CaptureType> m_capturesWithStatistics = new Dictionary<int, CaptureType>();

		// Token: 0x04000716 RID: 1814
		private Dictionary<IStatistic, StatisticState> m_currentStats;

		// Token: 0x04000717 RID: 1815
		private Thread m_worker;
	}
}
