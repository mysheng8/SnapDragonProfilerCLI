using System;

namespace Sdp
{
	// Token: 0x020001B4 RID: 436
	public class ConnectionSettings
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0000D2AC File Offset: 0x0000B4AC
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
		public string DisplayName { get; set; } = "";

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0000D2BD File Offset: 0x0000B4BD
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x0000D2C5 File Offset: 0x0000B4C5
		public string LookupName { get; set; } = "";

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0000D2CE File Offset: 0x0000B4CE
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x0000D2D6 File Offset: 0x0000B4D6
		public string GUID { get; set; } = "";

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0000D2DF File Offset: 0x0000B4DF
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x0000D2E7 File Offset: 0x0000B4E7
		public string HostIP { get; set; } = "";

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0000D2F0 File Offset: 0x0000B4F0
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
		public string Username { get; set; } = "";

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0000D301 File Offset: 0x0000B501
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x0000D309 File Offset: 0x0000B509
		public string Password { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0000D312 File Offset: 0x0000B512
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x0000D31A File Offset: 0x0000B51A
		public string DeployDir { get; set; } = "";

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0000D323 File Offset: 0x0000B523
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x0000D32B File Offset: 0x0000B52B
		public int ProcessPriority { get; set; } = 10;

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0000D334 File Offset: 0x0000B534
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x0000D33C File Offset: 0x0000B53C
		public string IdentityFile { get; set; } = "";

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0000D345 File Offset: 0x0000B545
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x0000D34D File Offset: 0x0000B54D
		public string ConnectionType { get; set; } = "";

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0000D356 File Offset: 0x0000B556
		public bool ManuallyAdded { get; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x0000D35E File Offset: 0x0000B55E
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x0000D366 File Offset: 0x0000B566
		public bool Edited { get; set; }

		// Token: 0x06000586 RID: 1414 RVA: 0x0000D36F File Offset: 0x0000B56F
		public void SetandEncryptPass(string pass)
		{
			this.Password = UserPreferenceModel.EncryptPassword(pass);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000D37D File Offset: 0x0000B57D
		public string GetDecryptedPass()
		{
			return UserPreferenceModel.DecryptPassword(this.Password);
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0000D38A File Offset: 0x0000B58A
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0000D392 File Offset: 0x0000B592
		public ConnectionManager.DeviceOS ConfiguredDeviceOS { get; set; } = ConnectionManager.DeviceOS.Other;

		// Token: 0x0600058A RID: 1418 RVA: 0x0000D39C File Offset: 0x0000B59C
		public ConnectionSettings(ConnectionSettings settings)
		{
			this.LookupName = settings.LookupName;
			this.DisplayName = settings.DisplayName;
			this.HostIP = settings.HostIP;
			this.ConfiguredDeviceOS = settings.ConfiguredDeviceOS;
			this.Username = settings.Username;
			this.Password = settings.Password;
			this.DeployDir = settings.DeployDir;
			this.ProcessPriority = settings.ProcessPriority;
			this.IdentityFile = settings.IdentityFile;
			this.ConnectionType = settings.ConnectionType;
			this.ManuallyAdded = settings.ManuallyAdded;
			this.GUID = settings.GUID;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0000D4A8 File Offset: 0x0000B6A8
		public ConnectionSettings(bool userAdded)
		{
			this.ManuallyAdded = userAdded;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0000D52C File Offset: 0x0000B72C
		public ConnectionSettings()
		{
		}
	}
}
