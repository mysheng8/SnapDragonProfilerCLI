using System;

namespace Sdp
{
	// Token: 0x02000279 RID: 633
	public interface ISnapshotView : IView
	{
		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06000ABC RID: 2748
		// (remove) Token: 0x06000ABD RID: 2749
		event EventHandler CaptureClicked;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06000ABE RID: 2750
		// (remove) Token: 0x06000ABF RID: 2751
		event EventHandler CancelClicked;

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06000AC0 RID: 2752
		// (remove) Token: 0x06000AC1 RID: 2753
		event EventHandler NewSnapshotClicked;

		// Token: 0x1700020D RID: 525
		// (set) Token: 0x06000AC2 RID: 2754
		bool DataSourcesVisible { set; }

		// Token: 0x1700020E RID: 526
		// (set) Token: 0x06000AC3 RID: 2755
		bool NewSnapshotButtonVisible { set; }

		// Token: 0x1700020F RID: 527
		// (set) Token: 0x06000AC4 RID: 2756
		bool NewSnapshotButtonSensitive { set; }

		// Token: 0x17000210 RID: 528
		// (set) Token: 0x06000AC5 RID: 2757
		bool CaptureButtonEnabled { set; }

		// Token: 0x17000211 RID: 529
		// (set) Token: 0x06000AC6 RID: 2758
		bool CancelCaptureButtonVisible { set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000AC7 RID: 2759
		IDataSourcesView DataSourcesView { get; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000AC8 RID: 2760
		IScreenCaptureView ScreenCaptureView { get; }

		// Token: 0x06000AC9 RID: 2761
		void ShowWaitingDialog();

		// Token: 0x06000ACA RID: 2762
		void ShowResultDialog(bool success, string message);

		// Token: 0x06000ACB RID: 2763
		void AddMainWindowErrorMessage(string message);

		// Token: 0x06000ACC RID: 2764
		void HideMainWindowErrorMessage();

		// Token: 0x06000ACD RID: 2765
		void SetName(int sessionNumber);
	}
}
