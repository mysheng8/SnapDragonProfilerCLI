using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Sdp;

namespace TraceImportExport
{
	// Token: 0x02000002 RID: 2
	public class TraceImport : IToolPlugin
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public TraceImport()
		{
			PluginEvents pluginEvents = SdpApp.EventsManager.PluginEvents;
			pluginEvents.RegisterDefaultToolPlugins = (EventHandler)Delegate.Combine(pluginEvents.RegisterDefaultToolPlugins, new EventHandler(this.pluginevents_registerDefaultToolPlugins));
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000209C File Offset: 0x0000029C
		public void Launch()
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog("Import trace file", null, false, null))
			{
				if (-3 == openFileDialog.ShowDialog())
				{
					string fileName = openFileDialog.Filename;
					string text = fileName;
					if (text.Length > this.m_windowNameElipsizeLenght)
					{
						text = ".." + text.Substring(text.Length - this.m_windowNameElipsizeLenght);
					}
					NewTraceWindowCommand newTrace = new NewTraceWindowCommand();
					newTrace.Name = text;
					newTrace.CaptureID = SdpApp.ConnectionManager.CreateCaptureId(CaptureType.Trace);
					SdpApp.ConnectionManager.SetCaptureName(newTrace.CaptureID, newTrace.Name);
					SdpApp.CommandManager.ExecuteCommand(newTrace);
					if (newTrace.Result != null)
					{
						newTrace.Result.CaptureButtonVisible = false;
						newTrace.Result.DataSourcesVisible = false;
						SDPProcessorPlugin processorPlugin = SdpApp.ConnectionManager.GetProcessorPlugin("SDP::SystraceProcessorPlugin");
						if (processorPlugin != null)
						{
							Thread thread = new Thread(delegate
							{
								ProgressObject progressObject = new ProgressObject();
								progressObject.Title = "Import Trace";
								progressObject.Description = "Importing Trace File";
								SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.BeginProgress, this, new ProgressEventArgs(progressObject));
								byte[] array = File.ReadAllBytes(fileName);
								IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
								Marshal.Copy(array, 0, intPtr, array.Length);
								Void_Double_Fn void_Double_Fn = delegate(double p)
								{
									progressObject.CurrentValue = p;
									SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.UpdateProgress, this, new ProgressEventArgs(progressObject));
								};
								processorPlugin.ProcessData(this.BUFFER_TYPE_SYSTRACE_DATA, 1U, newTrace.CaptureID, intPtr, (uint)array.Length, void_Double_Fn);
								Marshal.FreeHGlobal(intPtr);
								SdpApp.EventsManager.Raise<ProgressEventArgs>(SdpApp.EventsManager.ProgressEvents.EndProgress, this, new ProgressEventArgs(progressObject));
							});
							thread.Start();
						}
					}
				}
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021FC File Offset: 0x000003FC
		private void pluginevents_registerDefaultToolPlugins(object sender, EventArgs e)
		{
			SdpApp.EventsManager.Raise(SdpApp.EventsManager.PluginEvents.RegisterToolPlugin, this, EventArgs.Empty);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x0000221D File Offset: 0x0000041D
		public string Name
		{
			get
			{
				return "Import Systrace File";
			}
		}

		// Token: 0x04000001 RID: 1
		private readonly uint BUFFER_TYPE_SYSTRACE_DATA = 65536U;

		// Token: 0x04000002 RID: 2
		private readonly int m_windowNameElipsizeLenght = 20;
	}
}
