using System;
using Sdp.Logging;

namespace Sdp
{
	// Token: 0x020002EC RID: 748
	public class TensorViewController : IViewController
	{
		// Token: 0x06000F44 RID: 3908 RVA: 0x0002F763 File Offset: 0x0002D963
		public TensorViewController(ITensorView view)
		{
			this.m_view = view;
			TensorViewEvents tensorViewEvents = SdpApp.EventsManager.TensorViewEvents;
			tensorViewEvents.DisplayTensor = (EventHandler<TensorViewDisplayEventArgs>)Delegate.Combine(tensorViewEvents.DisplayTensor, new EventHandler<TensorViewDisplayEventArgs>(this.OnDisplayTensor));
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0002F7A0 File Offset: 0x0002D9A0
		private void OnDisplayTensor(object sender, TensorViewDisplayEventArgs args)
		{
			if (args.Matrices == null || args.Matrices.Length == 0)
			{
				TensorViewController.Logger.LogError(string.Format("Received empty matrices for tensor {0}", args.TensorID));
				return;
			}
			TensorViewController.Logger.LogDebug(string.Format("Displaying tensor {0} from capture {1}", args.TensorID, args.CaptureID));
			TensorViewController.Logger.LogDebug("  Format: " + args.FormatName);
			TensorViewController.Logger.LogDebug("  Dimensions: [" + string.Join<long>(",", args.Dims ?? new long[0]) + "]");
			TensorViewController.Logger.LogDebug(string.Format("  Channels: {0}", args.NumChannels));
			TensorViewController.Logger.LogDebug("  Tiling: " + args.Tiling);
			double formatMaxValue = this.GetFormatMaxValue(args.FormatName);
			TensorViewController.Logger.LogDebug(string.Format("  Format max value: {0}", formatMaxValue));
			this.m_view.DisplayTensor(args.FormatName, args.Dims, args.NumChannels, args.Matrices, args.Tiling, formatMaxValue);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0002F8E0 File Offset: 0x0002DAE0
		private double GetFormatMaxValue(string formatName)
		{
			if (string.IsNullOrEmpty(formatName))
			{
				return 255.0;
			}
			string text = formatName.ToUpper();
			if ((text.Contains("R8") || text.Contains("G8") || text.Contains("B8") || text.Contains("A8")) && (text.Contains("UINT") || text.Contains("UNORM") || text.Contains("SRGB")))
			{
				return 255.0;
			}
			if (text.Contains("R16") || text.Contains("G16") || text.Contains("B16") || text.Contains("A16"))
			{
				if (text.Contains("UINT"))
				{
					return 65535.0;
				}
				if (text.Contains("UNORM"))
				{
					return 1.0;
				}
			}
			if ((text.Contains("R32") || text.Contains("G32") || text.Contains("B32") || text.Contains("A32")) && text.Contains("UINT"))
			{
				return 4294967295.0;
			}
			if (text.Contains("SINT"))
			{
				if (text.Contains("R8") || text.Contains("G8") || text.Contains("B8") || text.Contains("A8"))
				{
					return 127.0;
				}
				if (text.Contains("R16") || text.Contains("G16") || text.Contains("B16") || text.Contains("A16"))
				{
					return 32767.0;
				}
				if (text.Contains("R32") || text.Contains("G32") || text.Contains("B32") || text.Contains("A32"))
				{
					return 2147483647.0;
				}
			}
			if (text.Contains("SFLOAT") || text.Contains("UFLOAT"))
			{
				return 1.0;
			}
			TensorViewController.Logger.LogWarning("Unknown format '" + formatName + "', defaulting to max value of 255");
			return 255.0;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0002FB2C File Offset: 0x0002DD2C
		public ViewDesc SaveSettings()
		{
			ViewDesc viewDesc = null;
			if (this.m_view != null)
			{
				viewDesc = new ViewDesc();
				viewDesc.TypeName = this.m_view.TypeName;
			}
			return viewDesc;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc viewDesc)
		{
			return true;
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x0002FB5B File Offset: 0x0002DD5B
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x04000A65 RID: 2661
		private ITensorView m_view;

		// Token: 0x04000A66 RID: 2662
		private static ILogger Logger = new Sdp.Logging.Logger("TensorView Controller");
	}
}
