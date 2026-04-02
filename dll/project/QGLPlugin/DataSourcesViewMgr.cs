using System;
using System.Collections.Generic;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x02000035 RID: 53
	internal class DataSourcesViewMgr
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x0000B074 File Offset: 0x00009274
		public DataSourcesViewMgr()
		{
			DataSourceViewEvents dataSourceViewEvents = SdpApp.EventsManager.DataSourceViewEvents;
			dataSourceViewEvents.ProcessWithWarningSelected = (EventHandler<ProcessWithWarningSelectedEventArgs>)Delegate.Combine(dataSourceViewEvents.ProcessWithWarningSelected, new EventHandler<ProcessWithWarningSelectedEventArgs>(this.OnDataSourceProcessWithWarningSelected));
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000B0B4 File Offset: 0x000092B4
		public void OnDataSourceProcessWithWarningSelected(object o, ProcessWithWarningSelectedEventArgs e)
		{
			ProcessWarnings warningType = e.WarningType;
			switch (warningType)
			{
			case ProcessWarnings.EXTERNAL_STORAGE_PERMISSIONS_RUNTIME:
			case ProcessWarnings.EXTERNAL_STORAGE_PERMISSIONS_REQUESTED:
			case (ProcessWarnings)3:
			case ProcessWarnings.EXTERNAL_STORAGE_PERMISSIONS_UNDETERMINED:
				break;
			default:
				if (warningType == ProcessWarnings.CAPTURETYPE_METRIC_UNAVAILABLE)
				{
					this.HandleVulkanMetricUnavailable(o as DataSourcesController, e.Pid);
				}
				break;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000B0F8 File Offset: 0x000092F8
		public void HandleVulkanMetricUnavailable(DataSourcesController controller, uint pid)
		{
			Process process = ProcessManager.Get().GetProcess(pid);
			if (controller != null && process != null)
			{
				uint pid2 = process.GetProperties().pid;
				if (!this.m_alreadyShownMissingCaptureType.Contains(pid2))
				{
					controller.RequestRestartForWrongLayer();
					this.m_alreadyShownMissingCaptureType.Add(pid2);
					return;
				}
				controller.ShowEnableMetricsButton();
			}
		}

		// Token: 0x040003A9 RID: 937
		private HashSet<uint> m_alreadyShownMissingCaptureType = new HashSet<uint>();
	}
}
