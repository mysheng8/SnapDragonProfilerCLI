using System;
using System.Threading;

namespace SdpClientFramework.ResourcesInvalidator
{
	// Token: 0x0200000F RID: 15
	public interface IResourcePopulator<TRequest> where TRequest : class, IInvalidateRequest
	{
		// Token: 0x0600000F RID: 15
		void PopulateResourceObjects(TRequest request, CancellationToken cancelToken);
	}
}
