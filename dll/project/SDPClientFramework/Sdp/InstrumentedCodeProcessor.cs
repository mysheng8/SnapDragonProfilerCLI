using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000222 RID: 546
	public class InstrumentedCodeProcessor
	{
		// Token: 0x0600084D RID: 2125 RVA: 0x000167BC File Offset: 0x000149BC
		public InstrumentedCodeProcessor()
		{
			ConnectionEvents connectionEvents = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents.DataReceived = (EventHandler<DataReceivedEventArgs>)Delegate.Combine(connectionEvents.DataReceived, new EventHandler<DataReceivedEventArgs>(this.connectionEvents_DataReceived));
			ConnectionEvents connectionEvents2 = SdpApp.EventsManager.ConnectionEvents;
			connectionEvents2.CaptureCompleted = (EventHandler<CaptureCompletedEventArgs>)Delegate.Combine(connectionEvents2.CaptureCompleted, new EventHandler<CaptureCompletedEventArgs>(this.connectionEvents_CaptureCompleted));
			this.m_functionBreakdowns = new Dictionary<uint, ICProcessInfo>();
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x00016830 File Offset: 0x00014A30
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x00016838 File Offset: 0x00014A38
		public uint ProviderID
		{
			get
			{
				return this.m_providerID;
			}
			set
			{
				this.m_providerID = value;
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00016841 File Offset: 0x00014A41
		public ICProcessInfo GetProcessInfo(uint pid)
		{
			if (this.m_functionBreakdowns.ContainsKey(pid))
			{
				return this.m_functionBreakdowns[pid];
			}
			return null;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00016860 File Offset: 0x00014A60
		public void StringPairReceived(uint nameId, string name)
		{
			string text = null;
			SdpApp.ModelManager.InstrumentedCodeModel.Functions.TryGetValue(nameId, out text);
			if (text == null)
			{
				SdpApp.ModelManager.InstrumentedCodeModel.Functions.Add(nameId, name);
				return;
			}
			SdpApp.ModelManager.InstrumentedCodeModel.Functions[nameId] = text;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x000168B8 File Offset: 0x00014AB8
		private void connectionEvents_ProviderListChanged(object sender, EventArgs e)
		{
			foreach (KeyValuePair<uint, DataProvider> keyValuePair in SdpApp.ModelManager.ConnectionModel.Providers)
			{
				if (keyValuePair.Value.GetProviderDesc().Name.StartsWith("ICP"))
				{
					this.m_providerID = keyValuePair.Value.GetID();
					break;
				}
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00008AEF File Offset: 0x00006CEF
		private void connectionEvents_DataReceived(object sender, DataReceivedEventArgs args)
		{
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00016940 File Offset: 0x00014B40
		private void connectionEvents_CaptureCompleted(object sender, CaptureCompletedEventArgs args)
		{
			if (args.ProviderId == this.m_providerID)
			{
				foreach (KeyValuePair<uint, ICProcessInfo> keyValuePair in this.m_functionBreakdowns)
				{
					foreach (KeyValuePair<uint, ICThreadInfo> keyValuePair2 in keyValuePair.Value.FunctionBreakdowns)
					{
						foreach (ICMarker icmarker in keyValuePair2.Value.Markers)
						{
							icmarker.AbsoluteDepth = icmarker.RelativeDepth - keyValuePair2.Value.MinDepth;
						}
					}
				}
				SdpApp.EventsManager.Raise(SdpApp.EventsManager.ICEvents.DataProcessed, this, EventArgs.Empty);
			}
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00008AEF File Offset: 0x00006CEF
		private void ConstructICFunctionBreakdownTreesAndStore()
		{
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00008AEF File Offset: 0x00006CEF
		private void ConstructDebugRegionsAndStore()
		{
		}

		// Token: 0x040007C8 RID: 1992
		private uint m_providerID;

		// Token: 0x040007C9 RID: 1993
		private Dictionary<uint, ICProcessInfo> m_functionBreakdowns;
	}
}
