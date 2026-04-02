using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SdpClientFramework.DesignPatterns.SingleConsumer;
using SdpClientFramework.ResourcesInvalidator;

namespace Sdp.ResourcesInvalidator
{
	// Token: 0x02000300 RID: 768
	public abstract class ResourcePopulator<TRequest> : IResourcePopulator<TRequest> where TRequest : class, IInvalidateRequest
	{
		// Token: 0x06000FC3 RID: 4035 RVA: 0x00030A1A File Offset: 0x0002EC1A
		protected ResourcePopulator(ResourceViewEvents resourcesViewEventDelegates, IActionQueue clientCommandQueue)
		{
			this.m_clientActionQueue = clientCommandQueue;
			this.m_resourceEventDelegates = resourcesViewEventDelegates;
		}

		// Token: 0x06000FC4 RID: 4036
		public abstract void PopulateResourceObjects(TRequest request, CancellationToken cancelToken);

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00030A30 File Offset: 0x0002EC30
		protected void AddCategory(AddCategoryArgs categoryArgs)
		{
			this.m_clientActionQueue.Queue(delegate
			{
				EventHandler<AddCategoryArgs> addCategory = this.m_resourceEventDelegates.AddCategory;
				if (addCategory == null)
				{
					return;
				}
				addCategory(this, categoryArgs);
			});
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00030A68 File Offset: 0x0002EC68
		protected void PrepopulateCategory(PrepopulateCategoryArgs categoryArgs)
		{
			this.m_clientActionQueue.Queue(delegate
			{
				EventHandler<PrepopulateCategoryArgs> prepopulateCategory = this.m_resourceEventDelegates.PrepopulateCategory;
				if (prepopulateCategory == null)
				{
					return;
				}
				prepopulateCategory(this, categoryArgs);
			});
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00030AA0 File Offset: 0x0002ECA0
		[Obsolete("For better performance use add category and add all resources at one time", false)]
		protected void AddResource(AddResourceArgs addResourceArgs)
		{
			this.m_clientActionQueue.Queue(delegate
			{
				EventHandler<AddResourceArgs> addResource = this.m_resourceEventDelegates.AddResource;
				if (addResource == null)
				{
					return;
				}
				addResource(this, addResourceArgs);
			});
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00030AD8 File Offset: 0x0002ECD8
		protected void UpdateResourcePixbuf(int categoryId, Dictionary<long, byte[]> Items)
		{
			this.m_clientActionQueue.Queue(delegate
			{
				UpdateResourcePixBufArgs updateResourcePixBufArgs = new UpdateResourcePixBufArgs();
				updateResourcePixBufArgs.CategoryID = categoryId;
				updateResourcePixBufArgs.Items = Items;
				EventHandler<UpdateResourcePixBufArgs> updateResourcePixBuf = this.m_resourceEventDelegates.UpdateResourcePixBuf;
				if (updateResourcePixBuf == null)
				{
					return;
				}
				updateResourcePixBuf(this, updateResourcePixBufArgs);
			});
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00030B18 File Offset: 0x0002ED18
		protected void UpdateResourcesActiveToggled(int categoryId, Dictionary<long, object> Items)
		{
			this.m_clientActionQueue.Queue(delegate
			{
				UpdateResourceCustomFilterDataArgs updateResourceCustomFilterDataArgs = new UpdateResourceCustomFilterDataArgs();
				updateResourceCustomFilterDataArgs.CategoryID = categoryId;
				updateResourceCustomFilterDataArgs.Items = Items;
				updateResourceCustomFilterDataArgs.Column = 6;
				EventHandler<UpdateResourceCustomFilterDataArgs> updateResourceCustomFilterData = this.m_resourceEventDelegates.UpdateResourceCustomFilterData;
				if (updateResourceCustomFilterData == null)
				{
					return;
				}
				updateResourceCustomFilterData(this, updateResourceCustomFilterDataArgs);
			});
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00030B58 File Offset: 0x0002ED58
		protected void UpdateResourcesActiveToggled(long resourceId, int categoryId, bool active)
		{
			this.m_clientActionQueue.Queue(delegate
			{
				UpdateResourceCustomFilterDataArgs updateResourceCustomFilterDataArgs = new UpdateResourceCustomFilterDataArgs();
				updateResourceCustomFilterDataArgs.Id = resourceId;
				updateResourceCustomFilterDataArgs.CategoryID = categoryId;
				updateResourceCustomFilterDataArgs.Data = active;
				updateResourceCustomFilterDataArgs.Column = 6;
				EventHandler<UpdateResourceCustomFilterDataArgs> updateResourceCustomFilterData = this.m_resourceEventDelegates.UpdateResourceCustomFilterData;
				if (updateResourceCustomFilterData == null)
				{
					return;
				}
				updateResourceCustomFilterData(this, updateResourceCustomFilterDataArgs);
			});
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00030BA0 File Offset: 0x0002EDA0
		protected void UpdateNumVisible(int categoryId, int numTotal, int numVisible)
		{
			this.m_clientActionQueue.Queue(delegate
			{
				UpdateCategoryNumVisible updateCategoryNumVisible = new UpdateCategoryNumVisible();
				updateCategoryNumVisible.CategoryID = categoryId;
				updateCategoryNumVisible.Total = numTotal;
				updateCategoryNumVisible.Visible = numVisible;
				EventHandler<UpdateCategoryNumVisible> updateCategoryNumVisible2 = this.m_resourceEventDelegates.UpdateCategoryNumVisible;
				if (updateCategoryNumVisible2 == null)
				{
					return;
				}
				updateCategoryNumVisible2(this, updateCategoryNumVisible);
			});
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00030BE6 File Offset: 0x0002EDE6
		protected void CheckForCancelRequest(CancellationToken cancelToken)
		{
			if (cancelToken.IsCancellationRequested)
			{
				throw new TaskCanceledException();
			}
		}

		// Token: 0x04000ABB RID: 2747
		protected AddCategoryArgs m_categoryArgs;

		// Token: 0x04000ABC RID: 2748
		protected ResourceViewEvents m_resourceEventDelegates;

		// Token: 0x04000ABD RID: 2749
		protected IActionQueue m_clientActionQueue;
	}
}
