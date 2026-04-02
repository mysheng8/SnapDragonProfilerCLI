using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200018A RID: 394
	public interface IDockHost
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600047A RID: 1146
		// (set) Token: 0x0600047B RID: 1147
		string CurrentLayout { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600047C RID: 1148
		List<string> Layouts { get; }

		// Token: 0x0600047D RID: 1149
		IDockWindow CreateDockWindow(string window_name, IView view);

		// Token: 0x0600047E RID: 1150
		IDockWindow CreateDockWindowTabbedWith(string thisWindowName, IView view, string tabWindowName, string layout, bool canClose = true, bool canRename = false);

		// Token: 0x0600047F RID: 1151
		void RemoveDockWindow(IDockWindow dock_window);

		// Token: 0x06000480 RID: 1152
		void AppendSuffixToWindow(string windowName, string suffix);

		// Token: 0x06000481 RID: 1153
		bool HasLayout(string layout);

		// Token: 0x06000482 RID: 1154
		void AddLayout(string layout);

		// Token: 0x06000483 RID: 1155
		void RemoveLayout(string layout);

		// Token: 0x06000484 RID: 1156
		void RenameLayout(string current_name, string new_name);

		// Token: 0x06000485 RID: 1157
		string SaveLayouts(bool exiting);

		// Token: 0x06000486 RID: 1158
		void LoadLayouts(string layouts_text);

		// Token: 0x06000487 RID: 1159
		void SelectPageInLayout(string layout, string window);

		// Token: 0x06000488 RID: 1160
		void PrepareVisibleItems();

		// Token: 0x06000489 RID: 1161
		void RestoreVisibleItems(IEnumerable<IDockWindow> windows, string tabWindowName, string pendingLayout);

		// Token: 0x0600048A RID: 1162
		void ShowFloatingWindows();

		// Token: 0x0600048B RID: 1163
		void HideFloatingWindows();

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x0600048C RID: 1164
		// (remove) Token: 0x0600048D RID: 1165
		event EventHandler<CaptureWindowFocusedArgs> CaptureWindowFocused;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x0600048E RID: 1166
		// (remove) Token: 0x0600048F RID: 1167
		event EventHandler<CaptureWindowFocusedArgs> CaptureWindowHidden;
	}
}
