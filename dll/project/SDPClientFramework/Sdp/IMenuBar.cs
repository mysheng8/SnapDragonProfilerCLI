using System;

namespace Sdp
{
	// Token: 0x02000253 RID: 595
	public interface IMenuBar
	{
		// Token: 0x060009E0 RID: 2528
		void AddTopLevelMenu(string name);

		// Token: 0x060009E1 RID: 2529
		void AddSeparator(MenuHeader header);

		// Token: 0x060009E2 RID: 2530
		void AddMenuItem(MenuHeader header, MenuType type, IMenuItemController item, string group = "");

		// Token: 0x060009E3 RID: 2531
		void RemoveMenuItem(MenuHeader type, string view);

		// Token: 0x060009E4 RID: 2532
		void RenameMenuItem(string oldName, string newName);

		// Token: 0x060009E5 RID: 2533
		void SetMenuSensitive(string name, bool visibility);

		// Token: 0x060009E6 RID: 2534
		void EnableMenuItems(MenuHeader header);

		// Token: 0x060009E7 RID: 2535
		void ToggleMenuItem(MenuHeader header, string name, bool toggled);

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x060009E8 RID: 2536
		// (remove) Token: 0x060009E9 RID: 2537
		event EventHandler MenuActivated;
	}
}
