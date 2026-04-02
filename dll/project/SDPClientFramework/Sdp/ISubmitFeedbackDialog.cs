using System;

namespace Sdp
{
	// Token: 0x020001EE RID: 494
	public interface ISubmitFeedbackDialog : IDialog
	{
		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06000714 RID: 1812
		// (remove) Token: 0x06000715 RID: 1813
		event EventHandler<FileAddedEventArgs> FileAdded;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06000716 RID: 1814
		// (remove) Token: 0x06000717 RID: 1815
		event EventHandler<FileAddedEventArgs> FileRemoved;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06000718 RID: 1816
		// (remove) Token: 0x06000719 RID: 1817
		event EventHandler GenerateReportClicked;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x0600071A RID: 1818
		// (remove) Token: 0x0600071B RID: 1819
		event EventHandler CancelReportClicked;

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600071C RID: 1820
		string IssueTitle { get; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600071D RID: 1821
		string IssueDescription { get; }

		// Token: 0x0600071E RID: 1822
		void ShowProgressTab();

		// Token: 0x0600071F RID: 1823
		void SetProgress(double progress);

		// Token: 0x06000720 RID: 1824
		void SetProgressText(string text);

		// Token: 0x06000721 RID: 1825
		void PulseProgressBar();

		// Token: 0x06000722 RID: 1826
		void SetReportCompleted(string displayLocation, string linkLocation);

		// Token: 0x06000723 RID: 1827
		void SetReportErrorDisplay(string text);

		// Token: 0x06000724 RID: 1828
		void AddWarningLine(string text);

		// Token: 0x17000154 RID: 340
		// (set) Token: 0x06000725 RID: 1829
		string InitialTitle { set; }

		// Token: 0x17000155 RID: 341
		// (set) Token: 0x06000726 RID: 1830
		string InitialDescription { set; }
	}
}
