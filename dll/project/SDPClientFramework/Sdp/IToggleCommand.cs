using System;

namespace Sdp
{
	// Token: 0x02000191 RID: 401
	public interface IToggleCommand : ICommand
	{
		// Token: 0x14000032 RID: 50
		// (add) Token: 0x060004B6 RID: 1206
		// (remove) Token: 0x060004B7 RID: 1207
		event EventHandler ToggleStateChanged;

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004B8 RID: 1208
		// (set) Token: 0x060004B9 RID: 1209
		bool Toggled { get; set; }
	}
}
