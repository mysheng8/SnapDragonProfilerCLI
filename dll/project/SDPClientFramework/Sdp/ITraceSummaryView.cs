using System;

namespace Sdp
{
	// Token: 0x02000223 RID: 547
	public interface ITraceSummaryView
	{
		// Token: 0x06000857 RID: 2135
		void SetGenericInfo(PropertyGridDescriptionObject content);

		// Token: 0x06000858 RID: 2136
		void SetMetrics(PropertyGridDescriptionObject content);

		// Token: 0x06000859 RID: 2137
		void SetErrorsAndWarnings(PropertyGridDescriptionObject content);
	}
}
