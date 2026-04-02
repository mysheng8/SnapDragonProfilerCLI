using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sdp
{
	// Token: 0x020001F0 RID: 496
	public class SubmitFeedbackController : IDialogController, IDisposable
	{
		// Token: 0x06000728 RID: 1832 RVA: 0x00012E18 File Offset: 0x00011018
		public SubmitFeedbackController(ISubmitFeedbackDialog dialog, string sessionPath = null, string suggestedTitle = null, string suggestedDescription = null)
		{
			this.m_dialog = dialog;
			this.m_customSessionPath = sessionPath;
			this.m_initialTitle = suggestedTitle;
			this.m_initialDescription = suggestedDescription;
			if (!string.IsNullOrEmpty(sessionPath))
			{
				this.m_reportGenerator = new SubmitFeedbackController.GenerateReportClass(sessionPath);
			}
			this.m_dialog.FileAdded += this.OnAttachmentFileAdded;
			this.m_dialog.FileRemoved += this.OnAttachmentFileRemoved;
			this.m_dialog.GenerateReportClicked += this.OnGenerateReportClicked;
			this.m_dialog.CancelReportClicked += this.OnCancelReportGenerateClicked;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00008AEF File Offset: 0x00006CEF
		public void Dispose()
		{
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00012ED0 File Offset: 0x000110D0
		public async Task<bool> ShowDialog()
		{
			bool flag = false;
			if (this.m_dialog != null)
			{
				if (!string.IsNullOrEmpty(this.m_initialTitle))
				{
					this.m_dialog.InitialTitle = this.m_initialTitle;
				}
				if (!string.IsNullOrEmpty(this.m_initialDescription))
				{
					this.m_dialog.InitialDescription = this.m_initialDescription;
				}
				bool flag2 = await this.m_dialog.ShowDialog();
				flag = flag2;
			}
			return flag;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00012F14 File Offset: 0x00011114
		public void SetFinished()
		{
			string text = this.m_customSessionPath ?? SdpApp.ConnectionManager.GetSessionPath();
			text = text.Replace('\\', Path.DirectorySeparatorChar);
			text = text.Replace('/', Path.DirectorySeparatorChar);
			string text2 = Path.Combine("file:///", text, "Report" + Path.DirectorySeparatorChar.ToString());
			this.m_dialog.SetReportCompleted(this.m_reportGenerator.ZipLocation, text2);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00012F8C File Offset: 0x0001118C
		public void SetProgress(double progress)
		{
			this.m_dialog.SetProgress(progress);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00012F9A File Offset: 0x0001119A
		public void PulseProgressBar()
		{
			this.m_dialog.PulseProgressBar();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00012FA7 File Offset: 0x000111A7
		public void SetProgressText(string text)
		{
			this.m_dialog.SetProgressText(text);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00012FB5 File Offset: 0x000111B5
		private void OnAttachmentFileAdded(object sender, FileAddedEventArgs e)
		{
			this.m_attachments.Add(e.FileName);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00012FC8 File Offset: 0x000111C8
		private void OnAttachmentFileRemoved(object sender, FileAddedEventArgs e)
		{
			this.m_attachments.Remove(e.FileName);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00012FDC File Offset: 0x000111DC
		private void OnGenerateReportClicked(object sender, EventArgs e)
		{
			if (this.m_dialog.IssueTitle.Length == 0)
			{
				ShowMessageDialogCommand.ShowErrorDialog("Please enter a title");
				return;
			}
			if (this.m_dialog.IssueDescription.Length == 0)
			{
				ShowMessageDialogCommand.ShowErrorDialog("Please enter a description");
				return;
			}
			this.m_reportGenerator.Title = this.m_dialog.IssueTitle.Replace(' ', '_');
			this.m_reportGenerator.Description = this.m_dialog.IssueDescription;
			this.m_reportGenerator.Attachments = this.m_attachments;
			this.m_dialog.ShowProgressTab();
			this.m_reportGenerator.GenerateReport(this);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00013080 File Offset: 0x00011280
		private void OnCancelReportGenerateClicked(object sender, EventArgs e)
		{
			this.SetReportError("Canceled By User");
			this.m_reportGenerator.Abort();
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00013098 File Offset: 0x00011298
		private void SetReportError(string text)
		{
			this.m_dialog.SetReportErrorDisplay(text);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x000130A6 File Offset: 0x000112A6
		private void AddWarningLine(string text)
		{
			this.m_dialog.AddWarningLine(text);
		}

		// Token: 0x04000719 RID: 1817
		private ISubmitFeedbackDialog m_dialog;

		// Token: 0x0400071A RID: 1818
		private SubmitFeedbackController.GenerateReportClass m_reportGenerator = new SubmitFeedbackController.GenerateReportClass(null);

		// Token: 0x0400071B RID: 1819
		private List<string> m_attachments = new List<string>();

		// Token: 0x0400071C RID: 1820
		private string m_customSessionPath;

		// Token: 0x0400071D RID: 1821
		private string m_initialTitle;

		// Token: 0x0400071E RID: 1822
		private string m_initialDescription;

		// Token: 0x02000394 RID: 916
		private class GenerateReportClass
		{
			// Token: 0x060011F1 RID: 4593 RVA: 0x00038092 File Offset: 0x00036292
			public GenerateReportClass(string customSessionPath = null)
			{
				this.m_previousSessionPath = customSessionPath;
			}

			// Token: 0x170002FB RID: 763
			// (get) Token: 0x060011F2 RID: 4594 RVA: 0x000380A1 File Offset: 0x000362A1
			public string ZipLocation
			{
				get
				{
					return this.m_zipLocation;
				}
			}

			// Token: 0x060011F3 RID: 4595 RVA: 0x000380A9 File Offset: 0x000362A9
			public void GenerateReport(SubmitFeedbackController parent)
			{
				this.Parent = parent;
				this.m_generateThread = new Thread(new ThreadStart(this.GenerateThread));
				this.m_generateThread.Start();
			}

			// Token: 0x060011F4 RID: 4596 RVA: 0x000380D4 File Offset: 0x000362D4
			public void Abort()
			{
				this.m_generateThread.Abort();
				this.m_success = false;
			}

			// Token: 0x060011F5 RID: 4597 RVA: 0x000380E8 File Offset: 0x000362E8
			private void GenerateThread()
			{
				try
				{
					string text = this.m_previousSessionPath ?? SdpApp.ConnectionManager.GetSessionPath();
					text = text.Replace('\\', Path.DirectorySeparatorChar);
					text = text.Replace('/', Path.DirectorySeparatorChar);
					this.m_zipLocation = Path.Combine(text, "Report", this.Title + ".zip");
					if (!Directory.Exists(Path.GetDirectoryName(this.m_zipLocation)))
					{
						try
						{
							Directory.CreateDirectory(Path.GetDirectoryName(this.m_zipLocation));
						}
						catch (Exception)
						{
							this.Parent.SetReportError(string.Format("Unable to write to directory: {0}", Path.GetDirectoryName(this.m_zipLocation)));
							this.Abort();
						}
					}
					if (File.Exists(this.m_zipLocation))
					{
						int num = 0;
						string text2;
						do
						{
							text2 = Path.Combine(text, "Report", this.Title + "(" + num.ToString() + ").zip");
							num++;
						}
						while (File.Exists(text2));
						this.m_zipLocation = text2;
					}
					try
					{
						List<string> list = new List<string>();
						this.Parent.SetProgressText("Collecting Log");
						this.Parent.PulseProgressBar();
						string text3 = Path.Combine(text, "logcat.txt");
						string text4 = Path.Combine(text, "logcat_sdp.txt");
						Device connectedDevice = SdpApp.ConnectionManager.GetConnectedDevice();
						if (connectedDevice != null)
						{
							if (this.CaptureLogcat(connectedDevice, "", text3))
							{
								list.Add(text3);
							}
							if (this.CaptureLogcat(connectedDevice, "-s SDP", text4))
							{
								list.Add(text4);
							}
						}
						this.Parent.SetProgressText("Packaging Data");
						foreach (string text5 in Directory.GetFiles(text))
						{
							string fileName = Path.GetFileName(text5);
							if (fileName.StartsWith("sdpframestripped_"))
							{
								list.Add(text5);
							}
						}
						foreach (string text6 in this.Attachments)
						{
							list.Add(text6);
						}
						list.Add(Path.Combine(text, "sdp.db"));
						list.Add(Path.Combine(text, "sdplog.txt"));
						list.Add(Path.Combine(text, "version.txt"));
						string text7 = Path.Combine(text, "crash.txt");
						if (File.Exists(text7))
						{
							list.Add(text7);
						}
						CompressFilesCommand compressFilesCommand = new CompressFilesCommand();
						compressFilesCommand.Output = this.m_zipLocation;
						compressFilesCommand.Files = list;
						compressFilesCommand.InMemoryData = new Dictionary<string, byte[]>();
						compressFilesCommand.InMemoryData.Add("description.txt", Encoding.ASCII.GetBytes(this.Description));
						compressFilesCommand.ProgressChanged += delegate
						{
							this.Parent.SetProgress(compressFilesCommand.Progress);
						};
						try
						{
							SdpApp.CommandManager.ExecuteCommand(compressFilesCommand);
						}
						catch (Exception ex)
						{
							this.Parent.AddWarningLine(ex.Message);
						}
						File.Delete(text3);
						File.Delete(text4);
						this.Parent.SetFinished();
					}
					catch (ThreadAbortException)
					{
						try
						{
							if (File.Exists(this.m_zipLocation))
							{
								File.Delete(this.m_zipLocation);
							}
						}
						catch (IOException)
						{
							this.Parent.AddWarningLine("Error deleting zipping files");
						}
					}
				}
				catch (Exception ex2)
				{
					this.Parent.SetReportError(string.Format("Error generating report: {0}", ex2.Message));
				}
			}

			// Token: 0x060011F6 RID: 4598 RVA: 0x000384EC File Offset: 0x000366EC
			private bool CaptureLogcat(Device connectedDevice, string args, string outFile)
			{
				bool flag = false;
				string text = "/data/local/tmp/logcat.txt";
				global::System.Diagnostics.Process process = new global::System.Diagnostics.Process();
				process.StartInfo.FileName = "adb";
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.Arguments = string.Concat(new string[]
				{
					"-s ",
					connectedDevice.GetName(),
					" logcat -d -f ",
					text,
					" ",
					args
				});
				process.Start();
				process.WaitForExit();
				if (process.ExitCode == 0)
				{
					process.Close();
					process.StartInfo.Arguments = string.Concat(new string[]
					{
						"-s ",
						connectedDevice.GetName(),
						" pull ",
						text,
						" \"",
						outFile,
						"\""
					});
					process.Start();
					process.WaitForExit();
					if (process.ExitCode == 0)
					{
						flag = true;
					}
					else
					{
						char[] array = new char[1024];
						process.StandardOutput.Read(array, 0, array.Length);
						SubmitFeedbackController parent = this.Parent;
						string text2 = "Error retrieving logcat output: ";
						char[] array2 = array;
						parent.AddWarningLine(text2 + ((array2 != null) ? array2.ToString() : null));
					}
					process.StartInfo.Arguments = "-s " + connectedDevice.GetName() + " shell rm " + text;
					process.Start();
					process.WaitForExit();
				}
				else
				{
					char[] array3 = new char[1024];
					process.StandardOutput.Read(array3, 0, array3.Length);
					SubmitFeedbackController parent2 = this.Parent;
					string text3 = "Error capturing logcat: ";
					char[] array4 = array3;
					parent2.AddWarningLine(text3 + ((array4 != null) ? array4.ToString() : null));
				}
				process.Close();
				return flag;
			}

			// Token: 0x170002FC RID: 764
			// (get) Token: 0x060011F7 RID: 4599 RVA: 0x000386AF File Offset: 0x000368AF
			// (set) Token: 0x060011F8 RID: 4600 RVA: 0x000386B7 File Offset: 0x000368B7
			private bool m_success { get; set; }

			// Token: 0x04000C7A RID: 3194
			public string Title;

			// Token: 0x04000C7B RID: 3195
			public string Description;

			// Token: 0x04000C7C RID: 3196
			public List<string> Attachments;

			// Token: 0x04000C7D RID: 3197
			public SubmitFeedbackController Parent;

			// Token: 0x04000C7E RID: 3198
			private string m_zipLocation;

			// Token: 0x04000C7F RID: 3199
			private Thread m_generateThread;

			// Token: 0x04000C81 RID: 3201
			private string m_previousSessionPath;
		}
	}
}
