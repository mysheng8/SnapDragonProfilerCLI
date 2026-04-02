using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001D7 RID: 471
	public interface ILaunchApplicationDialog
	{
		// Token: 0x14000040 RID: 64
		// (add) Token: 0x0600062F RID: 1583
		// (remove) Token: 0x06000630 RID: 1584
		event EventHandler<LaunchAppPackageChanged> SelectedPackageChanged;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06000631 RID: 1585
		// (remove) Token: 0x06000632 RID: 1586
		event EventHandler<LaunchAppActivityChanged> SelectedActivityChanged;

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06000633 RID: 1587
		// (remove) Token: 0x06000634 RID: 1588
		event EventHandler<LaunchAppFilterChanged> PackageFilterChanged;

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06000635 RID: 1589
		// (remove) Token: 0x06000636 RID: 1590
		event EventHandler<LaunchAppParamChanged> ParamChanged;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06000637 RID: 1591
		// (remove) Token: 0x06000638 RID: 1592
		event EventHandler LaunchAppClicked;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06000639 RID: 1593
		// (remove) Token: 0x0600063A RID: 1594
		event EventHandler<LaunchAppFilterChanged> RefreshAppsClicked;

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600063B RID: 1595
		bool IsAndroid { get; }

		// Token: 0x17000122 RID: 290
		// (set) Token: 0x0600063C RID: 1596
		bool LaunchButtonSensitive { set; }

		// Token: 0x0600063D RID: 1597
		void InvalidatePackageList(string[] packages, string packageToSelect = "");

		// Token: 0x0600063E RID: 1598
		void InvalidateActivityList(string[] activities, string defaultActivity = "", string activityToSelect = "");

		// Token: 0x0600063F RID: 1599
		void SetStatus(StatusType statusType, string statusText, int duration, bool hasHelp = false, string helpTooltip = null);

		// Token: 0x06000640 RID: 1600
		bool TryGetParam<T>(LaunchApplicationDialogParam param, T defaultValue, out T outValue);

		// Token: 0x06000641 RID: 1601
		void UpdateView(CaptureType captureType, List<LaunchHistory> historyData);

		// Token: 0x06000642 RID: 1602
		void UpdateRenderingAPIGrid();
	}
}
