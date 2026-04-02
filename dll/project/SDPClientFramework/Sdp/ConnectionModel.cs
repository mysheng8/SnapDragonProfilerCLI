using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cairo;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x0200020A RID: 522
	public class ConnectionModel
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000150FC File Offset: 0x000132FC
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x00015104 File Offset: 0x00013304
		public string MinimumADBVersion { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0001510D File Offset: 0x0001330D
		public Dictionary<string, IconObject> IconDictionary
		{
			get
			{
				return this.m_iconDictionary;
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00015118 File Offset: 0x00013318
		public void AddIconToDictionary(string processName, IntPtr data)
		{
			if (data != IntPtr.Zero)
			{
				uint iconSize = SDPCoreInterop.GetIconSize();
				uint iconWidth = SDPCoreInterop.GetIconWidth();
				uint iconHeight = SDPCoreInterop.GetIconHeight();
				uint bytesPerPixel = IconObject.BytesPerPixel;
				byte[] array = new byte[iconSize];
				Marshal.Copy(data, array, 0, (int)iconSize);
				uint num = iconSize / bytesPerPixel;
				int num2 = 0;
				while ((long)num2 < (long)((ulong)num))
				{
					checked
					{
						byte b = array[(int)((IntPtr)(unchecked((ulong)bytesPerPixel * (ulong)((long)num2))))];
						byte b2 = array[(int)((IntPtr)(unchecked((ulong)bytesPerPixel * (ulong)((long)num2) + 2UL)))];
						array[(int)((IntPtr)(unchecked((ulong)bytesPerPixel * (ulong)((long)num2))))] = b2;
						array[(int)((IntPtr)(unchecked((ulong)bytesPerPixel * (ulong)((long)num2) + 2UL)))] = b;
					}
					num2++;
				}
				IconObject iconObject = new IconObject(iconSize, iconWidth, iconHeight, array);
				if (this.m_iconDictionary.ContainsKey(processName))
				{
					this.m_iconDictionary.Remove(processName);
				}
				this.m_iconDictionary.Add(processName, iconObject);
			}
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x000151E4 File Offset: 0x000133E4
		public IconObject GetIconObject(string name)
		{
			if (this.m_iconDictionary.ContainsKey(name))
			{
				IconObject iconObject;
				this.m_iconDictionary.TryGetValue(name, out iconObject);
				return iconObject;
			}
			return null;
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00015211 File Offset: 0x00013411
		public Dictionary<uint, DataProvider> Providers
		{
			get
			{
				return this.m_providers;
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001521C File Offset: 0x0001341C
		public bool TryAddProvider(DataProvider provider)
		{
			if (!this.m_providers.ContainsKey(provider.GetID()))
			{
				this.m_providers.Add(provider.GetID(), provider);
				SdpApp.EventsManager.Raise(SdpApp.EventsManager.ConnectionEvents.ProviderListChanged, this, EventArgs.Empty);
				return true;
			}
			return false;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00015270 File Offset: 0x00013470
		public bool RemoveProvider(uint providerID)
		{
			if (this.m_providers.ContainsKey(providerID))
			{
				this.m_providers.Remove(providerID);
				SdpApp.EventsManager.Raise(SdpApp.EventsManager.ConnectionEvents.ProviderListChanged, this, EventArgs.Empty);
				return true;
			}
			return false;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x000152AF File Offset: 0x000134AF
		public DataProvider GetProviderById(uint id)
		{
			if (this.m_providers.ContainsKey(id))
			{
				return this.m_providers[id];
			}
			return null;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000152D0 File Offset: 0x000134D0
		public void AddMetricCategoryListToMetricDictionary(List<uint> categoryIds)
		{
			if (this.m_categoryColors == null)
			{
				this.m_categoryColors = new Dictionary<uint, double[]>();
			}
			foreach (uint num in categoryIds)
			{
				this.AddMetricCategoryColor(num);
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00015334 File Offset: 0x00013534
		public void AddMetricCategoryIdToMetricDictionary(uint categoryId)
		{
			if (this.m_categoryColors == null)
			{
				this.m_categoryColors = new Dictionary<uint, double[]>();
			}
			this.AddMetricCategoryColor(categoryId);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00015350 File Offset: 0x00013550
		private void AddMetricCategoryColor(uint categoryId)
		{
			if (!this.m_categoryColors.ContainsKey(categoryId))
			{
				this.m_categoryColors.Add(categoryId, SdpApp.ModelManager.SettingsModel.GetColorPalletteAtIndex(this.m_categoryColors.Count));
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00015388 File Offset: 0x00013588
		public double[] GetMetricCategoryColor(uint id)
		{
			double[] array;
			if (this.m_categoryColors != null && this.m_categoryColors.TryGetValue(id, out array))
			{
				return array;
			}
			Color color = FormatHelper.PseudoRandomColor();
			return new double[] { color.R, color.G, color.B };
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000153DC File Offset: 0x000135DC
		public void AddData(Data data)
		{
			DataReceivedEventArgs dataReceivedEventArgs = new DataReceivedEventArgs();
			dataReceivedEventArgs.ReceivedData = data;
			SdpApp.EventsManager.Raise<DataReceivedEventArgs>(SdpApp.EventsManager.ConnectionEvents.DataReceived, this, dataReceivedEventArgs);
		}

		// Token: 0x0400075D RID: 1885
		private Dictionary<string, IconObject> m_iconDictionary = new Dictionary<string, IconObject>();

		// Token: 0x0400075E RID: 1886
		private Dictionary<uint, DataProvider> m_providers = new Dictionary<uint, DataProvider>();

		// Token: 0x0400075F RID: 1887
		private Dictionary<uint, double[]> m_categoryColors;
	}
}
