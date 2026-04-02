using System;

namespace Sdp
{
	// Token: 0x0200018C RID: 396
	public interface IDockWindow
	{
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06000491 RID: 1169
		// (remove) Token: 0x06000492 RID: 1170
		event EventHandler IsVisibleChanged;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06000493 RID: 1171
		// (remove) Token: 0x06000494 RID: 1172
		event EventHandler NameChanged;

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000495 RID: 1173
		// (set) Token: 0x06000496 RID: 1174
		string Name { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000497 RID: 1175
		// (set) Token: 0x06000498 RID: 1176
		string Layout { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000499 RID: 1177
		// (set) Token: 0x0600049A RID: 1178
		bool IsVisible { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600049B RID: 1179
		IView View { get; }

		// Token: 0x0600049C RID: 1180
		void Focus();

		// Token: 0x0600049D RID: 1181
		void Present();

		// Token: 0x0600049E RID: 1182
		bool IsVisibleInLayout(string layoutName);

		// Token: 0x0600049F RID: 1183
		void SetVisibleInAllLayouts(bool visible);
	}
}
