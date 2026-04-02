using System;

namespace Sdp
{
	// Token: 0x02000084 RID: 132
	public class ShowMessageDialogCommand : Command
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00009077 File Offset: 0x00007277
		// (set) Token: 0x060002DB RID: 731 RVA: 0x0000907F File Offset: 0x0000727F
		public IconType IconType { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00009088 File Offset: 0x00007288
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00009090 File Offset: 0x00007290
		public ButtonLayout ButtonLayout { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00009099 File Offset: 0x00007299
		// (set) Token: 0x060002DF RID: 735 RVA: 0x000090A1 File Offset: 0x000072A1
		public string Title { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x000090AA File Offset: 0x000072AA
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x000090B2 File Offset: 0x000072B2
		public string Message { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x000090C4 File Offset: 0x000072C4
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x000090BB File Offset: 0x000072BB
		public Action<bool> OnCompleted { private get; set; } = delegate(bool result)
		{
		};

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x000090CC File Offset: 0x000072CC
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x000090D4 File Offset: 0x000072D4
		public bool HasDontShowAgainCheckBox { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x000090DD File Offset: 0x000072DD
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x000090E5 File Offset: 0x000072E5
		public bool DontShowAgainCheckBoxValue { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000090EE File Offset: 0x000072EE
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x000090F6 File Offset: 0x000072F6
		public string AffirmativeText { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002EA RID: 746 RVA: 0x000090FF File Offset: 0x000072FF
		// (set) Token: 0x060002EB RID: 747 RVA: 0x00009107 File Offset: 0x00007307
		public string NegativeText { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00009110 File Offset: 0x00007310
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00009118 File Offset: 0x00007318
		public IWindow TopLevelWindow { get; set; } = MainWindowController.TopLevelWindow;

		// Token: 0x060002EE RID: 750 RVA: 0x00009121 File Offset: 0x00007321
		public static void ShowErrorDialog(string message)
		{
			ShowMessageDialogCommand.ShowMessage(message, IconType.Error);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000912C File Offset: 0x0000732C
		public static void ShowMessage(string message, IconType iconType)
		{
			new ShowMessageDialogCommand
			{
				Message = message,
				IconType = iconType
			}.Execute();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00009154 File Offset: 0x00007354
		protected override async void OnExecute()
		{
			IDialog dialog = SdpApp.UIManager.CreateDialog("MessageDialog");
			IMessageDialogView messageDialog = dialog as IMessageDialogView;
			if (messageDialog != null)
			{
				messageDialog.Title = ((this.Title != null && this.Title != "") ? this.Title : "Snapdragon Profiler");
				messageDialog.Message = this.Message;
				messageDialog.IconType = this.IconType;
				messageDialog.ButtonLayout = this.ButtonLayout;
				messageDialog.HasDontShowAgainCheckBox = this.HasDontShowAgainCheckBox;
				messageDialog.TopLevelWindow = this.TopLevelWindow;
				messageDialog.AffirmativeText = this.AffirmativeText;
				messageDialog.NegativeText = this.NegativeText;
				bool flag = await messageDialog.ShowDialog();
				bool flag2 = flag;
				this.DontShowAgainCheckBoxValue = messageDialog.DontShowAgainCheckBoxValue;
				this.OnCompleted(flag2);
			}
			messageDialog = null;
		}

		// Token: 0x040001CF RID: 463
		private const string DEFAULT_TITLE = "Snapdragon Profiler";
	}
}
