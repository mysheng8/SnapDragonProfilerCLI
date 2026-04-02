using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001AD RID: 429
	public class StatisticsManager
	{
		// Token: 0x06000544 RID: 1348 RVA: 0x0000C278 File Offset: 0x0000A478
		public async void ShowDialog()
		{
			if (this.m_view == null && this.m_controller == null)
			{
				this.m_view = SdpApp.UIManager.CreateDialog("StatisticsView") as IStatisticsView;
				this.m_controller = new StatisticsController(this.m_view);
			}
			await this.m_controller.ShowDialog();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0000C2B0 File Offset: 0x0000A4B0
		public void RegisterStatistic(IStatistic statistic)
		{
			SdpApp.ModelManager.StatisticsModel.Statistics.Add(statistic);
			StatisticAddedArgs statisticAddedArgs = new StatisticAddedArgs();
			statisticAddedArgs.Statistic = statistic;
			SdpApp.EventsManager.Raise<StatisticAddedArgs>(SdpApp.EventsManager.StatisticEvents.StatisticAdded, this, statisticAddedArgs);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000C2FC File Offset: 0x0000A4FC
		public void StatisticAvailable(int captureID, IStatistic statistic)
		{
			if (!SdpApp.ModelManager.StatisticsModel.Statistics.Contains(statistic))
			{
				this.RegisterStatistic(statistic);
			}
			if (!SdpApp.ModelManager.StatisticsModel.StatisticsStatePerCapture.ContainsKey(captureID))
			{
				SdpApp.ModelManager.StatisticsModel.StatisticsStatePerCapture[captureID] = new HashSet<IStatistic>();
			}
			StatisticPerCaptureArgs statisticPerCaptureArgs = new StatisticPerCaptureArgs();
			statisticPerCaptureArgs.State = StatisticState.Available;
			statisticPerCaptureArgs.Statistic = statistic;
			statisticPerCaptureArgs.CaptureID = captureID;
			if (!SdpApp.ModelManager.StatisticsModel.StatisticsStatePerCapture[captureID].Contains(statistic))
			{
				SdpApp.ModelManager.StatisticsModel.StatisticsStatePerCapture[captureID].Add(statistic);
				SdpApp.EventsManager.Raise<StatisticPerCaptureArgs>(SdpApp.EventsManager.StatisticEvents.StatisticPerCaptureAdded, null, statisticPerCaptureArgs);
			}
		}

		// Token: 0x04000653 RID: 1619
		private IStatisticsView m_view;

		// Token: 0x04000654 RID: 1620
		private StatisticsController m_controller;
	}
}
