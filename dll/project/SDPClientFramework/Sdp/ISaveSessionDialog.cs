using System;
using Sdp.Functional;

namespace Sdp
{
	// Token: 0x020001EB RID: 491
	public interface ISaveSessionDialog : IDialog
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000701 RID: 1793
		Maybe<string> SelectedFileLocation { get; }
	}
}
