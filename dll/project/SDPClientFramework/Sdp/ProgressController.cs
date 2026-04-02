using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200026E RID: 622
	public class ProgressController
	{
		// Token: 0x06000A80 RID: 2688 RVA: 0x0001DEB8 File Offset: 0x0001C0B8
		public ProgressController(IProgressView view)
		{
			this.m_view = view;
			ProgressEvents progressEvents = SdpApp.EventsManager.ProgressEvents;
			progressEvents.BeginProgress = (EventHandler<ProgressEventArgs>)Delegate.Combine(progressEvents.BeginProgress, new EventHandler<ProgressEventArgs>(this.progressEvents_BeginProgress));
			ProgressEvents progressEvents2 = SdpApp.EventsManager.ProgressEvents;
			progressEvents2.UpdateProgress = (EventHandler<ProgressEventArgs>)Delegate.Combine(progressEvents2.UpdateProgress, new EventHandler<ProgressEventArgs>(this.progressEvents_UpdateProgress));
			ProgressEvents progressEvents3 = SdpApp.EventsManager.ProgressEvents;
			progressEvents3.EndProgress = (EventHandler<ProgressEventArgs>)Delegate.Combine(progressEvents3.EndProgress, new EventHandler<ProgressEventArgs>(this.progressEvents_EndProgress));
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0001DF6C File Offset: 0x0001C16C
		private void UpdateGlobalProgress()
		{
			double num = 0.0;
			object obj = this.progressObjectLock;
			lock (obj)
			{
				if (this.m_progressObjects.Count != 0)
				{
					foreach (KeyValuePair<int, ProgressObject> keyValuePair in this.m_progressObjects)
					{
						num += keyValuePair.Value.CurrentValue;
					}
					num /= (double)this.m_progressObjects.Count;
				}
			}
			this.m_view.GlobalValue = num;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0001E024 File Offset: 0x0001C224
		private void progressEvents_BeginProgress(object sender, ProgressEventArgs args)
		{
			if (args.ProgressObject != null)
			{
				ProgressObject progressObject = args.ProgressObject;
				int currentProgressItemIndex = this.m_currentProgressItemIndex;
				this.m_currentProgressItemIndex = currentProgressItemIndex + 1;
				progressObject.ID = currentProgressItemIndex;
				object obj = this.progressObjectLock;
				lock (obj)
				{
					this.m_progressObjects.Add(args.ProgressObject.ID, args.ProgressObject);
				}
				this.m_view.AddProgressItem(args.ProgressObject.ID, args.ProgressObject.Title, args.ProgressObject.Description, args.ProgressObject.CurrentValue, args.ProgressObject.ShowProgressBar);
			}
			this.UpdateGlobalProgress();
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0001E0EC File Offset: 0x0001C2EC
		private void progressEvents_UpdateProgress(object sender, ProgressEventArgs args)
		{
			if (args.ProgressObject != null)
			{
				object obj = this.progressObjectLock;
				lock (obj)
				{
					if (this.m_progressObjects.ContainsKey(args.ProgressObject.ID))
					{
						this.m_progressObjects[args.ProgressObject.ID].CurrentValue = ((args.ProgressObject.CurrentValue > 1.0) ? 1.0 : args.ProgressObject.CurrentValue);
					}
				}
				this.m_view.UpdateProgressItem(args.ProgressObject.ID, args.ProgressObject.CurrentValue);
			}
			this.UpdateGlobalProgress();
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0001E1B8 File Offset: 0x0001C3B8
		private void progressEvents_EndProgress(object sender, ProgressEventArgs args)
		{
			if (args.ProgressObject != null)
			{
				object obj = this.progressObjectLock;
				lock (obj)
				{
					if (this.m_progressObjects.ContainsKey(args.ProgressObject.ID))
					{
						this.m_progressObjects.Remove(args.ProgressObject.ID);
					}
				}
				this.m_view.RemoveProgressItem(args.ProgressObject.ID);
			}
			this.UpdateGlobalProgress();
		}

		// Token: 0x04000877 RID: 2167
		private IProgressView m_view;

		// Token: 0x04000878 RID: 2168
		private readonly Dictionary<int, ProgressObject> m_progressObjects = new Dictionary<int, ProgressObject>();

		// Token: 0x04000879 RID: 2169
		private int m_currentProgressItemIndex;

		// Token: 0x0400087A RID: 2170
		private object progressObjectLock = new object();
	}
}
