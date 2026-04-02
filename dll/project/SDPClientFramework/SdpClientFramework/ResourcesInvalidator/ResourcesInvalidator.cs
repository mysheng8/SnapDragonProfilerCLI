using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sdp;
using Sdp.Logging;
using SdpClientFramework.DesignPatterns.SingleConsumer;

namespace SdpClientFramework.ResourcesInvalidator
{
	// Token: 0x02000011 RID: 17
	public abstract class ResourcesInvalidator<TRequest> : IResourcesInvalidator<TRequest> where TRequest : class, IInvalidateRequest
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000024A4 File Offset: 0x000006A4
		protected ResourcesInvalidator(ResourceViewEvents resourceViewEventDelegates, IActionQueue clientCommandQueue)
		{
			this.m_resourcesViewEventDelegates = resourceViewEventDelegates;
			this.m_clientCommandQueue = clientCommandQueue;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600001F RID: 31 RVA: 0x00002514 File Offset: 0x00000714
		// (remove) Token: 0x06000020 RID: 32 RVA: 0x0000254C File Offset: 0x0000074C
		public event EventHandler<InvalidateEventArgs<TRequest>> InvalidateBegan;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000021 RID: 33 RVA: 0x00002584 File Offset: 0x00000784
		// (remove) Token: 0x06000022 RID: 34 RVA: 0x000025BC File Offset: 0x000007BC
		public event EventHandler<InvalidateEventArgs<TRequest>> InvalidateEnded;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000023 RID: 35 RVA: 0x000025F4 File Offset: 0x000007F4
		// (remove) Token: 0x06000024 RID: 36 RVA: 0x0000262C File Offset: 0x0000082C
		public event EventHandler<InvalidateEventArgs<TRequest>> InvalidateCanceled;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000025 RID: 37 RVA: 0x00002664 File Offset: 0x00000864
		// (remove) Token: 0x06000026 RID: 38 RVA: 0x0000269C File Offset: 0x0000089C
		public event EventHandler<InvalidateEventArgs<TRequest>> InvalidateFailed;

		// Token: 0x06000027 RID: 39 RVA: 0x000026D4 File Offset: 0x000008D4
		public virtual void Invalidate(TRequest request)
		{
			object resourceInvalLock = this.m_resourceInvalLock;
			lock (resourceInvalLock)
			{
				if (this.m_requestManagerTask == null)
				{
					this.StartRequestManagerTask();
				}
				this.m_invalidateQueue.Add(request);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002728 File Offset: 0x00000928
		public virtual void DisableResourceView()
		{
			DisableEventArgs disableEventArgs = new DisableEventArgs();
			disableEventArgs.Disable = true;
			EventHandler<DisableEventArgs> disableResourceView = this.m_resourcesViewEventDelegates.DisableResourceView;
			if (disableResourceView == null)
			{
				return;
			}
			disableResourceView(this, disableEventArgs);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000275C File Offset: 0x0000095C
		public virtual void EnableResourceView()
		{
			DisableEventArgs disableEventArgs = new DisableEventArgs();
			disableEventArgs.Disable = false;
			EventHandler<DisableEventArgs> disableResourceView = this.m_resourcesViewEventDelegates.DisableResourceView;
			if (disableResourceView == null)
			{
				return;
			}
			disableResourceView(this, disableEventArgs);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002790 File Offset: 0x00000990
		public virtual void SetResourceViewStatus(string message, StatusType type, int duration)
		{
			SetStatusEventArgs setStatusEventArgs = new SetStatusEventArgs();
			setStatusEventArgs.Duration = duration;
			setStatusEventArgs.StatusText = message;
			setStatusEventArgs.Status = type;
			EventHandler<SetStatusEventArgs> setStatus = this.m_resourcesViewEventDelegates.SetStatus;
			if (setStatus == null)
			{
				return;
			}
			setStatus(this, setStatusEventArgs);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000027D0 File Offset: 0x000009D0
		public virtual void HideResourceViewStatus()
		{
			foreach (Tuple<uint, Task> tuple in this.m_spawnedTasks)
			{
				if (!tuple.Item2.IsCompleted)
				{
					return;
				}
			}
			EventHandler<EventArgs> hideStatus = this.m_resourcesViewEventDelegates.HideStatus;
			if (hideStatus == null)
			{
				return;
			}
			hideStatus(this, EventArgs.Empty);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002848 File Offset: 0x00000A48
		public virtual void ClearResourcesView()
		{
			EventHandler<EventArgs> clear = this.m_resourcesViewEventDelegates.Clear;
			if (clear == null)
			{
				return;
			}
			clear(this, EventArgs.Empty);
		}

		// Token: 0x0600002D RID: 45
		protected abstract void HandleRequest(TRequest request);

		// Token: 0x0600002E RID: 46 RVA: 0x00002868 File Offset: 0x00000A68
		protected void QueueNewInvalidate(TRequest request)
		{
			ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo info;
			if (!this.m_invalidateTasksInfos.TryGetValue(request.InvalidateType, out info))
			{
				info = new ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo();
				this.m_invalidateTasksInfos[request.InvalidateType] = info;
			}
			Task task2;
			if (this.m_spawnedTasks.Count == 0 || request.Parallel)
			{
				task2 = Task.Factory.StartNew(delegate
				{
					this.ExecuteInvalidate(request, info.CancelSource.Token);
				}, info.CancelSource.Token);
			}
			else
			{
				Task item = this.m_spawnedTasks[this.m_spawnedTasks.Count - 1].Item2;
				task2 = item.ContinueWith(delegate(Task task)
				{
					this.ExecuteInvalidate(request, info.CancelSource.Token);
				}, info.CancelSource.Token);
			}
			this.m_spawnedTasks.Add(new Tuple<uint, Task>(request.InvalidateType, task2));
			info.TaskRequestTuples.Add(new Tuple<Task, TRequest>(task2, request));
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000029A0 File Offset: 0x00000BA0
		protected void CancelAllInvalidates()
		{
			foreach (KeyValuePair<uint, ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo> keyValuePair in this.m_invalidateTasksInfos)
			{
				uint key = keyValuePair.Key;
				ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo value = keyValuePair.Value;
				this.CancelInvalidateForCategory(key, value);
				value.Dispose();
			}
			this.m_invalidateTasksInfos.Clear();
			this.m_spawnedTasks.Clear();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002A20 File Offset: 0x00000C20
		protected void CancelInvalidateForCategory(uint category)
		{
			ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo perCategoryInvalidateInfo;
			if (this.m_invalidateTasksInfos.TryGetValue(category, out perCategoryInvalidateInfo))
			{
				this.CancelInvalidateForCategory(category, perCategoryInvalidateInfo);
				perCategoryInvalidateInfo.Dispose();
				this.m_invalidateTasksInfos.Remove(category);
			}
			this.m_spawnedTasks.RemoveAll((Tuple<uint, Task> tuple) => tuple.Item1 == category);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002A8C File Offset: 0x00000C8C
		protected void RaiseInvalidRequest(TRequest request)
		{
			EventHandler<InvalidateEventArgs<TRequest>> invalidateFailed = this.InvalidateFailed;
			if (invalidateFailed == null)
			{
				return;
			}
			invalidateFailed(this, new InvalidateEventArgs<TRequest>
			{
				Request = request
			});
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002AAB File Offset: 0x00000CAB
		protected void AddPopulator(IResourcePopulator<TRequest> populator)
		{
			this.m_resourcePopulators.Add(populator);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002AB9 File Offset: 0x00000CB9
		protected void ClearPopulators()
		{
			this.m_resourcePopulators.Clear();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002AC6 File Offset: 0x00000CC6
		private void StartRequestManagerTask()
		{
			this.m_requestManagerTask = Task.Factory.StartNew(delegate
			{
				for (;;)
				{
					TRequest trequest = default(TRequest);
					if (this.m_invalidateTasksInfos.Count == 0)
					{
						trequest = this.m_invalidateQueue.Take();
						this.HandleRequest(trequest);
					}
					else
					{
						this.CheckForFinishedTasks(50);
						if (this.m_invalidateQueue.TryTake(out trequest, 50))
						{
							this.HandleRequest(trequest);
						}
					}
				}
			}, TaskCreationOptions.LongRunning);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002AE8 File Offset: 0x00000CE8
		private void CheckForFinishedTasks(int timeoutMS)
		{
			Dictionary<uint, ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo> dictionary = new Dictionary<uint, ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo>();
			foreach (KeyValuePair<uint, ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo> keyValuePair in this.m_invalidateTasksInfos)
			{
				uint key = keyValuePair.Key;
				ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo value = keyValuePair.Value;
				List<Tuple<Task, TRequest>> taskRequestTuples = value.TaskRequestTuples;
				for (int i = taskRequestTuples.Count - 1; i >= 0; i--)
				{
					Tuple<Task, TRequest> tuple = value.TaskRequestTuples[i];
					Task item = tuple.Item1;
					TRequest request = tuple.Item2;
					try
					{
						if (item.Wait(timeoutMS))
						{
							taskRequestTuples.RemoveAt(i);
						}
					}
					catch (AggregateException ex)
					{
						taskRequestTuples.RemoveAt(i);
						AggregateException ex2;
						ex2.Handle((Exception ex) => this.InvalidateRequestExHandler(ex, request));
					}
				}
				if (value.TaskRequestTuples.Count > 0)
				{
					dictionary[key] = value;
				}
				else
				{
					value.Dispose();
				}
			}
			this.m_invalidateTasksInfos = dictionary;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002C14 File Offset: 0x00000E14
		private void CancelInvalidateForCategory(uint category, ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo invalidateInfo)
		{
			invalidateInfo.CancelSource.Cancel();
			foreach (Tuple<Task, TRequest> tuple in invalidateInfo.TaskRequestTuples)
			{
				Task item = tuple.Item1;
				TRequest request = tuple.Item2;
				try
				{
					item.Wait();
				}
				catch (AggregateException ex)
				{
					AggregateException ex2;
					ex2.Handle((Exception ex) => this.InvalidateRequestExHandler(ex, request));
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002CB8 File Offset: 0x00000EB8
		private bool InvalidateRequestExHandler(Exception ex, TRequest request)
		{
			if (ex is TaskCanceledException)
			{
				if (request != null)
				{
					this.m_clientCommandQueue.Queue(delegate
					{
						EventHandler<InvalidateEventArgs<TRequest>> invalidateCanceled = this.InvalidateCanceled;
						if (invalidateCanceled == null)
						{
							return;
						}
						invalidateCanceled(this, new InvalidateEventArgs<TRequest>
						{
							Request = request
						});
					});
				}
			}
			else
			{
				this.m_errorLogger.LogException(ex);
				this.m_clientCommandQueue.Queue(delegate
				{
					EventHandler<InvalidateEventArgs<TRequest>> invalidateFailed = this.InvalidateFailed;
					if (invalidateFailed == null)
					{
						return;
					}
					invalidateFailed(this, new InvalidateEventArgs<TRequest>
					{
						Request = request
					});
				});
			}
			return true;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D2C File Offset: 0x00000F2C
		protected virtual void ExecuteInvalidate(TRequest request, CancellationToken cancelToken)
		{
			this.ExecuteBegin(request);
			foreach (IResourcePopulator<TRequest> resourcePopulator in this.m_resourcePopulators)
			{
				resourcePopulator.PopulateResourceObjects(request, cancelToken);
				if (cancelToken.IsCancellationRequested)
				{
					throw new TaskCanceledException();
				}
			}
			this.ExecuteEnd(request);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002DA0 File Offset: 0x00000FA0
		protected void ExecuteBegin(TRequest request)
		{
			EventHandler<InvalidateEventArgs<TRequest>> invalidateBegan = this.InvalidateBegan;
			if (invalidateBegan == null)
			{
				return;
			}
			invalidateBegan(this, new InvalidateEventArgs<TRequest>
			{
				Request = request
			});
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002DBF File Offset: 0x00000FBF
		protected void ExecuteEnd(TRequest request)
		{
			EventHandler<InvalidateEventArgs<TRequest>> invalidateEnded = this.InvalidateEnded;
			if (invalidateEnded == null)
			{
				return;
			}
			invalidateEnded(this, new InvalidateEventArgs<TRequest>
			{
				Request = request
			});
		}

		// Token: 0x0400009D RID: 157
		public ILogger m_errorLogger = new global::Sdp.Logging.Logger("ResourcesInvalidator");

		// Token: 0x040000A2 RID: 162
		private object m_resourceInvalLock = new object();

		// Token: 0x040000A3 RID: 163
		private Dictionary<uint, ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo> m_invalidateTasksInfos = new Dictionary<uint, ResourcesInvalidator<TRequest>.PerCategoryInvalidateInfo>();

		// Token: 0x040000A4 RID: 164
		private Task m_requestManagerTask;

		// Token: 0x040000A5 RID: 165
		private readonly BlockingCollection<TRequest> m_invalidateQueue = new BlockingCollection<TRequest>(new ConcurrentQueue<TRequest>());

		// Token: 0x040000A6 RID: 166
		protected readonly List<IResourcePopulator<TRequest>> m_resourcePopulators = new List<IResourcePopulator<TRequest>>();

		// Token: 0x040000A7 RID: 167
		private const int TIMEOUT_WAIT = 50;

		// Token: 0x040000A8 RID: 168
		private readonly List<Tuple<uint, Task>> m_spawnedTasks = new List<Tuple<uint, Task>>();

		// Token: 0x040000A9 RID: 169
		private readonly IActionQueue m_clientCommandQueue;

		// Token: 0x040000AA RID: 170
		private readonly ResourceViewEvents m_resourcesViewEventDelegates;

		// Token: 0x0200031E RID: 798
		private class PerCategoryInvalidateInfo : IDisposable
		{
			// Token: 0x06001091 RID: 4241 RVA: 0x00034110 File Offset: 0x00032310
			public void Dispose()
			{
				this.CancelSource.Dispose();
			}

			// Token: 0x04000B10 RID: 2832
			public readonly CancellationTokenSource CancelSource = new CancellationTokenSource();

			// Token: 0x04000B11 RID: 2833
			public readonly List<Tuple<Task, TRequest>> TaskRequestTuples = new List<Tuple<Task, TRequest>>();
		}
	}
}
