using System;

namespace SdpClientFramework.ResourcesInvalidator
{
	// Token: 0x0200000E RID: 14
	public class InvalidateEventArgs<TRequest> : EventArgs where TRequest : class, IInvalidateRequest
	{
		// Token: 0x0400009C RID: 156
		public TRequest Request;
	}
}
