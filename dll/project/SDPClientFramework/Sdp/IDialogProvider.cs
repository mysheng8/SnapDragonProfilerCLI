using System;

namespace Sdp
{
	// Token: 0x02000188 RID: 392
	public interface IDialogProvider
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000477 RID: 1143
		string DialogTypeName { get; }

		// Token: 0x06000478 RID: 1144
		IDialog CreateDialog();
	}
}
