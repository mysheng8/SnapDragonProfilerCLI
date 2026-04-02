using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020001FC RID: 508
	public class SettingsModel
	{
		// Token: 0x06000752 RID: 1874 RVA: 0x00013288 File Offset: 0x00011488
		public SettingsModel()
		{
			this.m_userPreferenceSettings = new UserPreferenceModel();
			this.m_deviceAttributes = new DeviceAttributes();
			this.m_isDeviceAttributesValid = false;
			this.InitializeColorPallette();
			DeviceEvents deviceEvents = SdpApp.EventsManager.DeviceEvents;
			deviceEvents.ClientConnectACK = (EventHandler)Delegate.Combine(deviceEvents.ClientConnectACK, new EventHandler(this.deviceEvents_ClientConnectACK));
			DeviceEvents deviceEvents2 = SdpApp.EventsManager.DeviceEvents;
			deviceEvents2.ClientDisconnectACK = (EventHandler)Delegate.Combine(deviceEvents2.ClientDisconnectACK, new EventHandler(this.deviceEvents_ClientDisconnectACK));
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x00013314 File Offset: 0x00011514
		public UserPreferenceModel UserPreferences
		{
			get
			{
				return this.m_userPreferenceSettings;
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001331C File Offset: 0x0001151C
		private void SetDeviceAttributes()
		{
			DeviceAttributes deviceAttributes = this.m_deviceAttributes;
			lock (deviceAttributes)
			{
				Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
				if (connectedDevice != null && !this.m_isDeviceAttributesValid)
				{
					this.m_deviceAttributes = connectedDevice.GetDeviceAttributes();
					this.m_isDeviceAttributesValid = true;
				}
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00013380 File Offset: 0x00011580
		public double[] GetColorPalletteAtIndex(int i)
		{
			return this.m_colorPallette[i % this.m_colorPallette.Count];
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001339C File Offset: 0x0001159C
		private void InitializeColorPallette()
		{
			this.m_colorPallette = new List<double[]>();
			double[] array = new double[] { 0.6705882352941176, 0.050980392156862744, 0.08235294117647059 };
			double[] array2 = new double[] { 0.9686274509803922, 0.5764705882352941, 0.11372549019607843 };
			double[] array3 = new double[3];
			array3[0] = 1.0;
			array3[1] = 0.9490196078431372;
			double[] array4 = array3;
			double[] array5 = new double[] { 0.050980392156862744, 0.6941176470588235, 0.29411764705882354 };
			double[] array6 = new double[] { 0.0, 0.44313725490196076, 0.7372549019607844 };
			double[] array7 = new double[] { 0.0, 0.7254901960784313, 0.9450980392156862 };
			double[] array8 = new double[] { 0.5647058823529412, 0.24705882352941178, 0.596078431372549 };
			double[] array9 = new double[] { 0.9333333333333333, 0.3764705882352941, 0.615686274509804 };
			double[] array10 = new double[] { 1.0, 0.0, 1.0 };
			double[] array11 = new double[] { 0.62, 0.8, 0.23 };
			double[] array12 = new double[] { 0.22, 0.22, 0.85 };
			double[] array13 = new double[] { 0.99, 0.83, 0.1 };
			double[] array14 = new double[] { 0.22, 0.74, 0.79 };
			double[] array15 = new double[] { 0.96, 0.57, 0.11 };
			double[] array16 = new double[] { 0.9, 0.13, 0.16 };
			double[] array17 = new double[3];
			array17[0] = 0.5;
			double[] array18 = array17;
			double[] array19 = new double[] { 0.5, 0.0, 0.5 };
			this.m_colorPallette.Add(array);
			this.m_colorPallette.Add(array2);
			this.m_colorPallette.Add(array4);
			this.m_colorPallette.Add(array5);
			this.m_colorPallette.Add(array6);
			this.m_colorPallette.Add(array7);
			this.m_colorPallette.Add(array8);
			this.m_colorPallette.Add(array9);
			this.m_colorPallette.Add(array10);
			this.m_colorPallette.Add(array11);
			this.m_colorPallette.Add(array12);
			this.m_colorPallette.Add(array13);
			this.m_colorPallette.Add(array14);
			this.m_colorPallette.Add(array15);
			this.m_colorPallette.Add(array16);
			this.m_colorPallette.Add(array18);
			this.m_colorPallette.Add(array19);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001360E File Offset: 0x0001180E
		private void deviceEvents_ClientConnectACK(object sender, EventArgs e)
		{
			this.SetDeviceAttributes();
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00013618 File Offset: 0x00011818
		private void deviceEvents_ClientDisconnectACK(object sender, EventArgs e)
		{
			DeviceAttributes deviceAttributes = this.m_deviceAttributes;
			lock (deviceAttributes)
			{
				this.m_isDeviceAttributesValid = false;
			}
		}

		// Token: 0x04000726 RID: 1830
		private UserPreferenceModel m_userPreferenceSettings;

		// Token: 0x04000727 RID: 1831
		private List<double[]> m_colorPallette;

		// Token: 0x04000728 RID: 1832
		private DeviceAttributes m_deviceAttributes;

		// Token: 0x04000729 RID: 1833
		private bool m_isDeviceAttributesValid;
	}
}
