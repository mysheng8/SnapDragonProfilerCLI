using System;

namespace Sdp
{
	// Token: 0x02000252 RID: 594
	public interface IMainWindowController : IViewController
	{
		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060009D3 RID: 2515
		// (remove) Token: 0x060009D4 RID: 2516
		event EventHandler ViewClosing;

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x060009D5 RID: 2517
		// (remove) Token: 0x060009D6 RID: 2518
		event EventHandler ViewClosed;

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060009D7 RID: 2519
		IDockHost DockingHost { get; }

		// Token: 0x060009D8 RID: 2520
		void ShowView();

		// Token: 0x060009D9 RID: 2521
		void HideView();

		// Token: 0x060009DA RID: 2522
		void FocusCaptureWindow(string windowName);

		// Token: 0x060009DB RID: 2523
		void AddMenuItem(MenuHeader header, MenuType type, IMenuItemController item, string group = "");

		// Token: 0x060009DC RID: 2524
		void RemoveMenuItem(MenuHeader type, string view);

		// Token: 0x060009DD RID: 2525
		void RenameMenuItem(string oldName, string newName);

		// Token: 0x060009DE RID: 2526
		void EnableMenuItems(MenuHeader header);

		// Token: 0x060009DF RID: 2527
		void ToggleMenuItem(MenuHeader header, string name, bool toggled);
	}
}
