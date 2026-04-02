using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sdp.Functional;

namespace Sdp
{
	// Token: 0x020001E5 RID: 485
	public interface IOpenSessionDialog : IDialog
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060006AD RID: 1709
		Maybe<string> SelectedFileLocation { get; }

		// Token: 0x060006AE RID: 1710
		Task<Maybe<CaptureInfo>> GetSelectedCapture(Dictionary<int, Tuple<string, int>> inputCaptures);
	}
}
