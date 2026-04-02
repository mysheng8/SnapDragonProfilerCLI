using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000236 RID: 566
	public interface IConnectionView : IView
	{
		// Token: 0x1400006B RID: 107
		// (add) Token: 0x060008ED RID: 2285
		// (remove) Token: 0x060008EE RID: 2286
		event EventHandler<ConnectItemEventArgs> SelectedDeviceChanged;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x060008EF RID: 2287
		// (remove) Token: 0x060008F0 RID: 2288
		event EventHandler ConnectClicked;

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x060008F1 RID: 2289
		// (remove) Token: 0x060008F2 RID: 2290
		event EventHandler<AutoConnectEventArgs> AutoConnectClicked;

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x060008F3 RID: 2291
		// (remove) Token: 0x060008F4 RID: 2292
		event EventHandler<ConnectItemEventArgs> EditDeviceClicked;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x060008F5 RID: 2293
		// (remove) Token: 0x060008F6 RID: 2294
		event EventHandler<ConnectItemEventArgs> DeleteDeviceClicked;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x060008F7 RID: 2295
		// (remove) Token: 0x060008F8 RID: 2296
		event EventHandler AddDeviceClicked;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x060008F9 RID: 2297
		// (remove) Token: 0x060008FA RID: 2298
		event EventHandler ImportClicked;

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x060008FB RID: 2299
		// (remove) Token: 0x060008FC RID: 2300
		event EventHandler<ConfigureDeviceCompleteArgs> ConfigureDeviceComplete;

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060008FD RID: 2301
		// (set) Token: 0x060008FE RID: 2302
		string SelectedDevice { get; set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060008FF RID: 2303
		string LastSelectedDevice { get; }

		// Token: 0x06000900 RID: 2304
		void Clear();

		// Token: 0x06000901 RID: 2305
		void ShowAddEditDevice(ConnectionSettings settings, bool editing, List<string> availableOS);

		// Token: 0x06000902 RID: 2306
		void AddDevice(ConnectionSettings settings, DeviceState deviceState, bool allowEvents);

		// Token: 0x06000903 RID: 2307
		void RemoveDevice(string lookupGUID, List<string> availableOS);

		// Token: 0x06000904 RID: 2308
		void UpdateDeviceState(string lookupName, DeviceState deviceState, string deviceStateMsg);

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000905 RID: 2309
		// (set) Token: 0x06000906 RID: 2310
		bool ConnectSensitive { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000907 RID: 2311
		// (set) Token: 0x06000908 RID: 2312
		string ConnectLabel { get; set; }

		// Token: 0x170001B4 RID: 436
		// (set) Token: 0x06000909 RID: 2313
		bool AutoConnect { set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600090A RID: 2314
		IWindow TopLevelWindow { get; }
	}
}
