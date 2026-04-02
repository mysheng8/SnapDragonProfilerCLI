using System;
using System.Collections.Generic;
using Cairo;

namespace Sdp
{
	// Token: 0x020002A1 RID: 673
	public abstract class TrackControllerBase : IDisposable
	{
		// Token: 0x06000CEC RID: 3308 RVA: 0x00025D70 File Offset: 0x00023F70
		public TrackControllerBase(ITrackViewBase view, GroupLayoutController layoutContainer, GroupController groupContainer)
		{
			this.m_view = view;
			this.m_layoutContainer = layoutContainer;
			this.m_groupContainer = groupContainer;
			this.rand = new Random();
			if (this.m_view != null)
			{
				this.m_view.RemoveClicked += this.OnRemoveClicked;
				this.m_view.ExpandCollapseClicked += this.OnExpandCollapseClicked;
				this.m_view.ResizeRequested += this.OnResizeRequested;
				this.m_view.MetricDropped += this.m_viewMetricDropped;
				this.m_view.CategoryDropped += this.m_view_CategoryDropped;
			}
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00025E20 File Offset: 0x00024020
		public virtual void Dispose()
		{
			this.m_view.RemoveClicked -= this.OnRemoveClicked;
			this.m_view.ExpandCollapseClicked -= this.OnExpandCollapseClicked;
			this.m_view.ResizeRequested -= this.OnResizeRequested;
			this.m_view.MetricDropped -= this.m_viewMetricDropped;
			this.m_view.CategoryDropped -= this.m_view_CategoryDropped;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00025EA2 File Offset: 0x000240A2
		public void AddMetric(uint metricId, string metricName, uint pid, string tooltip, bool isCustom, Color? color)
		{
			this.AddMetric(metricId, metricName, pid, false, tooltip, isCustom, color);
		}

		// Token: 0x06000CEF RID: 3311
		public abstract void AddMetric(uint metricId, string metricName, uint pid, bool isPreview, string tooltip, bool isCustom, Color? color);

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00025EB4 File Offset: 0x000240B4
		public void RemoveMetric(uint metricId, string metricName, uint pid)
		{
			this.RemoveMetric(metricId, metricName, pid, false, false);
		}

		// Token: 0x06000CF1 RID: 3313
		public abstract void RemoveMetric(uint metricId, string metricName, uint pid, bool forceDeleteTrackIfEmpty, bool isPreview);

		// Token: 0x06000CF2 RID: 3314
		public abstract bool ContainsMetric(MetricDesc desc);

		// Token: 0x06000CF3 RID: 3315
		public abstract int MetricCount();

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00025EC1 File Offset: 0x000240C1
		protected virtual void m_viewMetricDropped(object sender, MetricDroppedEventArgs e)
		{
			this.m_layoutContainer.AddMetricToFlow(e.MetricId, e.Pids, this);
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00025EDC File Offset: 0x000240DC
		protected virtual void m_view_CategoryDropped(object sender, MetricDroppedEventArgs e)
		{
			List<uint> metricsByCategory = SdpApp.ConnectionManager.GetMetricsByCategory(e.MetricId);
			foreach (uint num in metricsByCategory)
			{
				Metric metric = MetricManager.Get().GetMetric(num);
				if ((metric.GetProperties().captureTypeMask & 1U) != 0U && !metric.GetProperties().hidden)
				{
					List<uint> list = new List<uint>();
					foreach (uint num2 in e.Pids)
					{
						Process process = ProcessManager.Get().GetProcess(num2);
						if (process != null && process.IsMetricLinked(num))
						{
							list.Add(num2);
						}
					}
					this.m_layoutContainer.AddMetricToFlow(num, list, this);
				}
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00025FDC File Offset: 0x000241DC
		private void OnResizeRequested(object sender, ResizeTrackRequestEventArgs e)
		{
			if (this.ResizeTrackRequested != null)
			{
				this.ResizeTrackRequested(this, e);
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00025FF3 File Offset: 0x000241F3
		private void OnRemoveClicked(object sender, EventArgs e)
		{
			if (this.RemoveTrackRequested != null)
			{
				this.RemoveTrackRequested(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0002600E File Offset: 0x0002420E
		private void OnExpandCollapseClicked(object sender, EventArgs e)
		{
			if (this.ExpandCollapseTrackRequested != null)
			{
				this.ExpandCollapseTrackRequested(this, EventArgs.Empty);
			}
		}

		// Token: 0x170002A1 RID: 673
		// (set) Token: 0x06000CF9 RID: 3321 RVA: 0x00026029 File Offset: 0x00024229
		public List<long> SelectedBookmarkTimestamps
		{
			set
			{
				this.View.SelectedBookmarkTimestamps = value;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00026037 File Offset: 0x00024237
		public uint CaptureId
		{
			get
			{
				return this.m_layoutContainer.CaptureId;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00026044 File Offset: 0x00024244
		public ITrackViewBase View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0002604C File Offset: 0x0002424C
		public virtual TrackViewDesc SaveSettings()
		{
			TrackViewDesc trackViewDesc = new TrackViewDesc();
			this.SaveCommonSettings(trackViewDesc);
			return trackViewDesc;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00008AEF File Offset: 0x00006CEF
		public virtual void LoadSettings(TrackViewDesc track_desc)
		{
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00026067 File Offset: 0x00024267
		protected void SaveCommonSettings(TrackViewDesc track_desc)
		{
		}

		// Token: 0x0400093F RID: 2367
		public EventHandler<ResizeTrackRequestEventArgs> ResizeTrackRequested;

		// Token: 0x04000940 RID: 2368
		public EventHandler ExpandCollapseTrackRequested;

		// Token: 0x04000941 RID: 2369
		public EventHandler RemoveTrackRequested;

		// Token: 0x04000942 RID: 2370
		protected ITrackViewBase m_view;

		// Token: 0x04000943 RID: 2371
		protected Random rand;

		// Token: 0x04000944 RID: 2372
		protected GroupLayoutController m_layoutContainer;

		// Token: 0x04000945 RID: 2373
		protected GroupController m_groupContainer;
	}
}
