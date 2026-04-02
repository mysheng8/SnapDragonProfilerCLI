using System;

namespace Sdp
{
	// Token: 0x02000185 RID: 389
	public interface ICommand
	{
		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06000466 RID: 1126
		// (remove) Token: 0x06000467 RID: 1127
		event EventHandler CanExecuteChanged;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000468 RID: 1128
		// (remove) Token: 0x06000469 RID: 1129
		event EventHandler IconIDChanged;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x0600046A RID: 1130
		// (remove) Token: 0x0600046B RID: 1131
		event EventHandler UINameChanged;

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600046C RID: 1132
		// (set) Token: 0x0600046D RID: 1133
		string UIName { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600046E RID: 1134
		// (set) Token: 0x0600046F RID: 1135
		string IconID { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000470 RID: 1136
		bool CanExecute { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000471 RID: 1137
		bool ClearsUndo { get; }

		// Token: 0x06000472 RID: 1138
		void Execute();

		// Token: 0x06000473 RID: 1139
		void Undo();

		// Token: 0x06000474 RID: 1140
		void Redo();
	}
}
