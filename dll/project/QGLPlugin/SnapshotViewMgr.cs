using System;
using System.Collections.Generic;
using Sdp;
using Sdp.Logging;

namespace QGLPlugin
{
	// Token: 0x0200003E RID: 62
	internal class SnapshotViewMgr
	{
		// Token: 0x06000138 RID: 312 RVA: 0x00011D98 File Offset: 0x0000FF98
		public SnapshotViewMgr()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.ProcessMetricLinked = (EventHandler<ProcessMetricLinkedEventArgs>)Delegate.Combine(connectionEvents.ProcessMetricLinked, new EventHandler<ProcessMetricLinkedEventArgs>(this.OnProcessMetricLinked));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.ProcessRemoved = (EventHandler<ProcessEventArgs>)Delegate.Combine(connectionEvents2.ProcessRemoved, new EventHandler<ProcessEventArgs>(this.OnProcessRemoved));
			SnapshotEvents snapshotEvents = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents.SelectedProcessChanged = (EventHandler<SelectedProcessChangedArgs>)Delegate.Combine(snapshotEvents.SelectedProcessChanged, new EventHandler<SelectedProcessChangedArgs>(this.SnapshotSelectedProcessChanged));
			SnapshotEvents snapshotEvents2 = SdpApp.EventsManager.SnapshotEvents;
			snapshotEvents2.CaptureClicked = (EventHandler<SnapshotRequestArgs>)Delegate.Combine(snapshotEvents2.CaptureClicked, new EventHandler<SnapshotRequestArgs>(this.OnCaptureClicked));
			QGLPlugin.ProviderID.OnValueChanged += this.OnQGLProviderIDSet;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00011E90 File Offset: 0x00010090
		private void OnProcessMetricLinked(object sender, ProcessMetricLinkedEventArgs args)
		{
			Metric metric = MetricManager.Get().GetMetric(args.MetricID);
			if (metric.GetProperties().name == "Vulkan Snapshot")
			{
				this.m_snapshotMetrics[args.PID] = metric;
				this.m_activeVulkanPIDs.Add(args.PID);
				if (this.m_selectedSnapshotProcess == args.PID)
				{
					this.AddVulkanSnapshotMode();
				}
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00011EFD File Offset: 0x000100FD
		private void OnProcessRemoved(object sender, ProcessEventArgs args)
		{
			this.m_activeVulkanPIDs.Remove(args.PID);
			this.m_snapshotMetrics.Remove(args.PID);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00011F24 File Offset: 0x00010124
		private void SnapshotSelectedProcessChanged(object sender, SelectedProcessChangedArgs args)
		{
			if (args.SelectedProcesses != null && args.SelectedProcesses.Count > 0)
			{
				this.m_selectedSnapshotProcess = args.SelectedProcesses[0].Id;
				if (this.m_activeVulkanPIDs.Contains(this.m_selectedSnapshotProcess))
				{
					this.AddVulkanSnapshotMode();
					return;
				}
			}
			else
			{
				this.m_selectedSnapshotProcess = 0U;
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00011F80 File Offset: 0x00010180
		private void OnCaptureClicked(object sender, SnapshotRequestArgs args)
		{
			uint selectedProviderID = args.SelectedProviderID;
			uint? num = QGLPlugin.ProviderID;
			if ((selectedProviderID == num.GetValueOrDefault()) & (num != null))
			{
				Metric metric;
				if (this.m_snapshotMetrics.TryGetValue(args.SelectedPID, out metric))
				{
					SnapshotCommand snapshotCommand = new SnapshotCommand();
					snapshotCommand.PID = args.SelectedPID;
					snapshotCommand.SnapshotMetric = metric;
					SdpApp.CommandManager.ExecuteCommand(snapshotCommand);
				}
				SnapshotViewMgr.Logger.LogInformation("Snapshot taken: " + SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.SelectedProcess.Name);
				this.m_snapshotStart = DateTime.Now;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00012022 File Offset: 0x00010222
		private void OnQGLProviderIDSet(object sender, ObservableObject<uint?>.ValueChangedArgs args)
		{
			if (!this.m_providerIDSet)
			{
				this.m_providerIDSet = true;
				if (this.m_activeVulkanPIDs.Contains(this.m_selectedSnapshotProcess))
				{
					this.AddVulkanSnapshotMode();
				}
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0001204C File Offset: 0x0001024C
		private void AddVulkanSnapshotMode()
		{
			if (QGLPlugin.ProviderID.Value != null)
			{
				AddSnapshotModeArgs addSnapshotModeArgs = new AddSnapshotModeArgs();
				addSnapshotModeArgs.ProviderID = QGLPlugin.ProviderID.Value.Value;
				addSnapshotModeArgs.Displayname = "Vulkan";
				addSnapshotModeArgs.ForceSelect = true;
				SdpApp.EventsManager.Raise<AddSnapshotModeArgs>(SdpApp.EventsManager.SnapshotEvents.AddAvailableSnapshotMode, this, addSnapshotModeArgs);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000120B8 File Offset: 0x000102B8
		public DateTime SnapshotStart
		{
			get
			{
				return this.m_snapshotStart;
			}
		}

		// Token: 0x040003E7 RID: 999
		private uint m_selectedSnapshotProcess;

		// Token: 0x040003E8 RID: 1000
		private HashSet<uint> m_activeVulkanPIDs = new HashSet<uint>();

		// Token: 0x040003E9 RID: 1001
		private Dictionary<uint, Metric> m_snapshotMetrics = new Dictionary<uint, Metric>();

		// Token: 0x040003EA RID: 1002
		private bool m_providerIDSet;

		// Token: 0x040003EB RID: 1003
		private DateTime m_snapshotStart = DateTime.Now;

		// Token: 0x040003EC RID: 1004
		private static ILogger Logger = new global::Sdp.Logging.Logger("QGLPlugin");
	}
}
