using System;
using Sdp;

namespace SdpClientFramework.ResourcesInvalidator
{
	// Token: 0x02000010 RID: 16
	public interface IResourcesInvalidator<TRequest> where TRequest : class, IInvalidateRequest
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000010 RID: 16
		// (remove) Token: 0x06000011 RID: 17
		event EventHandler<InvalidateEventArgs<TRequest>> InvalidateBegan;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000012 RID: 18
		// (remove) Token: 0x06000013 RID: 19
		event EventHandler<InvalidateEventArgs<TRequest>> InvalidateEnded;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000014 RID: 20
		// (remove) Token: 0x06000015 RID: 21
		event EventHandler<InvalidateEventArgs<TRequest>> InvalidateCanceled;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000016 RID: 22
		// (remove) Token: 0x06000017 RID: 23
		event EventHandler<InvalidateEventArgs<TRequest>> InvalidateFailed;

		// Token: 0x06000018 RID: 24
		void Invalidate(TRequest settings);

		// Token: 0x06000019 RID: 25
		void DisableResourceView();

		// Token: 0x0600001A RID: 26
		void EnableResourceView();

		// Token: 0x0600001B RID: 27
		void SetResourceViewStatus(string message, StatusType type, int duration);

		// Token: 0x0600001C RID: 28
		void ClearResourcesView();

		// Token: 0x0600001D RID: 29
		void HideResourceViewStatus();
	}
}
