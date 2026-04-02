using System;
using System.Runtime.CompilerServices;

namespace Sdp
{
	// Token: 0x020001D8 RID: 472
	public class LaunchHistory
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0000EF77 File Offset: 0x0000D177
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x0000EF7F File Offset: 0x0000D17F
		public string Path { get; set; } = "";

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0000EF88 File Offset: 0x0000D188
		// (set) Token: 0x06000646 RID: 1606 RVA: 0x0000EF90 File Offset: 0x0000D190
		public RenderingAPI RenderingAPIs { get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0000EF99 File Offset: 0x0000D199
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x0000EFA1 File Offset: 0x0000D1A1
		public string WorkingDirectory { get; set; } = "";

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0000EFAA File Offset: 0x0000D1AA
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x0000EFB2 File Offset: 0x0000D1B2
		public string Args { get; set; } = "";

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0000EFBB File Offset: 0x0000D1BB
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x0000EFC3 File Offset: 0x0000D1C3
		public string Options { get; set; } = "";

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0000EFCC File Offset: 0x0000D1CC
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
		public string EnvironmentVariables { get; set; } = "";

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x0000EFDD File Offset: 0x0000D1DD
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x0000EFE5 File Offset: 0x0000D1E5
		public ConnectionManager.DeviceOS DeviceOS { get; set; } = ConnectionManager.DeviceOS.Other;

		// Token: 0x06000651 RID: 1617 RVA: 0x0000EFF0 File Offset: 0x0000D1F0
		public LaunchHistory(string path, string workingDir = "", string args = "", RenderingAPI apis = RenderingAPI.None, string options = "", string envList = "", ConnectionManager.DeviceOS deviceOS = ConnectionManager.DeviceOS.Other)
		{
			this.Path = path;
			this.WorkingDirectory = workingDir;
			this.Args = args;
			this.RenderingAPIs = apis;
			this.Options = options;
			this.EnvironmentVariables = envList;
			this.DeviceOS = deviceOS;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0000F078 File Offset: 0x0000D278
		public LaunchHistory(string path, string intentArgs = "", RenderingAPI apis = RenderingAPI.None, string options = "", ConnectionManager.DeviceOS deviceOS = ConnectionManager.DeviceOS.Other)
		{
			this.Path = path;
			this.Args = intentArgs;
			this.RenderingAPIs = apis;
			this.Options = options;
			this.DeviceOS = deviceOS;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		public LaunchHistory()
		{
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0000F142 File Offset: 0x0000D342
		public bool IsMatchingActivity(LaunchHistory history)
		{
			return history.DeviceOS == this.DeviceOS && history.Path == this.Path && history.WorkingDirectory == this.WorkingDirectory;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0000F178 File Offset: 0x0000D378
		public override bool Equals(object obj)
		{
			LaunchHistory launchHistory = obj as LaunchHistory;
			return launchHistory != null && (launchHistory.Path == this.Path && launchHistory.WorkingDirectory == this.WorkingDirectory && launchHistory.Args == this.Args && launchHistory.RenderingAPIs == this.RenderingAPIs && launchHistory.Options == this.Options && launchHistory.EnvironmentVariables == this.EnvironmentVariables) && launchHistory.DeviceOS == this.DeviceOS;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0000F210 File Offset: 0x0000D410
		public override int GetHashCode()
		{
			return RuntimeHelpers.GetHashCode(string.Concat(new string[]
			{
				this.Path,
				this.WorkingDirectory,
				this.Args,
				this.RenderingAPIs.ToString(),
				this.Options,
				this.EnvironmentVariables,
				this.DeviceOS.ToString()
			}));
		}
	}
}
