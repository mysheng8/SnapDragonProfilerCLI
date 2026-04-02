using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sdp.Concurrency;
using Sdp.Functional;
using Sdp.Helpers;
using Sdp.Logging;

namespace Sdp
{
	// Token: 0x020001E6 RID: 486
	public class OpenSessionDialogController : IDialogController
	{
		// Token: 0x060006AF RID: 1711 RVA: 0x000100F6 File Offset: 0x0000E2F6
		public OpenSessionDialogController(IOpenSessionDialog view)
		{
			this.m_view = view;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00010108 File Offset: 0x0000E308
		public async Task<bool> ShowDialog()
		{
			bool flag = await this.m_view.ShowDialog();
			bool flag2;
			if (flag)
			{
				flag2 = await Task.Factory.StartNew<Task<bool>>(async () => await this.m_view.SelectedFileLocation.Match<Task<bool>>(new Func<string, Task<bool>>(this.OpenSession), new Func<Task<bool>>(ConcurrencyHelpers.AsyncFalse))).Result;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001014C File Offset: 0x0000E34C
		private async Task<bool> OpenSession(string sessionPath)
		{
			this.m_sessionPath = sessionPath;
			if (!string.IsNullOrEmpty(this.m_sessionPath) && File.Exists(this.m_sessionPath))
			{
				try
				{
					using (ZipArchive archive = ZipFile.Open(this.m_sessionPath, ZipArchiveMode.Read))
					{
						ZipArchiveEntry zipArchiveEntry = null;
						ZipArchiveEntry zipArchiveEntry2 = null;
						List<string> files = archive.Entries.Select((ZipArchiveEntry x) => x.Name).ToList<string>();
						foreach (ZipArchiveEntry zipArchiveEntry3 in archive.Entries)
						{
							if (zipArchiveEntry3.Name == "version.txt")
							{
								zipArchiveEntry = zipArchiveEntry3;
							}
							else if (zipArchiveEntry3.Name == "sdp.db")
							{
								zipArchiveEntry2 = zipArchiveEntry3;
							}
							if (zipArchiveEntry != null && zipArchiveEntry2 != null)
							{
								break;
							}
						}
						if (zipArchiveEntry == null || zipArchiveEntry.Length == 0L || zipArchiveEntry2 == null || zipArchiveEntry2.Length == 0L)
						{
							ShowMessageDialogCommand.ShowErrorDialog("The file specified is not a valid Snapdragon Profiler session.");
							return true;
						}
						using (StreamReader streamReader = new StreamReader(zipArchiveEntry.Open()))
						{
							this.ParseVersionString(streamReader);
						}
						try
						{
							this.InitDBConnection(zipArchiveEntry2);
							this.GetImportedMetrics();
							this.GetRealTimeValues();
							this.GetCaptures();
							if (this.m_captures.Count == 0)
							{
								ShowMessageDialogCommand.ShowMessage("No captures detected in session file.", IconType.Warning);
								return false;
							}
							Maybe<CaptureInfo> maybe = await this.m_view.GetSelectedCapture(this.m_captures);
							Maybe<CaptureInfo> maybe2 = maybe;
							return maybe2.Match<bool>(delegate(CaptureInfo selectedCapture)
							{
								CaptureType captureType = (CaptureType)selectedCapture.CaptureType;
								switch (captureType)
								{
								case CaptureType.Realtime:
									this.ProcessRealTimeCapture(selectedCapture);
									return true;
								case CaptureType.Trace:
									this.ProcessTraceCapture(selectedCapture);
									return true;
								case (CaptureType)3U:
									break;
								case CaptureType.Snapshot:
									this.ProcessSnapshotCapture(selectedCapture, files);
									return true;
								default:
									if (captureType == CaptureType.Sampling)
									{
										this.ProcessSamplingCapture(selectedCapture);
										return true;
									}
									break;
								}
								throw new InvalidOperationException("Invalid Capture Type");
							}, () => false);
						}
						catch (Exception ex)
						{
							ShowMessageDialogCommand.ShowErrorDialog("There was an error while opening the session file.");
						}
					}
					ZipArchive archive = null;
				}
				catch
				{
					ShowMessageDialogCommand.ShowErrorDialog("The file specified is not a valid Snapdragon Profiler session.");
				}
			}
			return false;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00010198 File Offset: 0x0000E398
		private void ParseVersionString(StreamReader reader)
		{
			string text = reader.ReadToEnd();
			Regex regex = new Regex("\"version\":\"(\\d+).(\\d+).(\\d+).\\d+\"");
			Match match = regex.Match(text);
			if (match.Success)
			{
				this.m_versionMajor = IntConverter.Convert(match.Groups[1].Value);
				this.m_versionMinor = IntConverter.Convert(match.Groups[2].Value);
				this.m_versionSubminor = IntConverter.Convert(match.Groups[3].Value);
				return;
			}
			throw new Exception("Could not retrieve version information");
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00010228 File Offset: 0x0000E428
		private void ProcessRealTimeCapture(CaptureInfo capture)
		{
			SdpApp.Platform.Invoke(delegate
			{
				if (this.m_realtimeValues.Count > 0)
				{
					NewTraceWindowCommand newTraceWindowCommand = new NewTraceWindowCommand();
					newTraceWindowCommand.Layout = "Realtime";
					newTraceWindowCommand.CaptureID = SdpApp.ConnectionManager.CreateCaptureId(CaptureType.Trace);
					newTraceWindowCommand.Name = (string.IsNullOrEmpty(capture.CaptureName) ? (Path.GetFileName(this.m_sessionPath) + " (" + newTraceWindowCommand.CaptureID.ToString() + ")") : capture.CaptureName);
					SdpApp.CommandManager.ExecuteCommand(newTraceWindowCommand);
					if (newTraceWindowCommand.Result != null)
					{
						newTraceWindowCommand.Result.CaptureButtonVisible = false;
						newTraceWindowCommand.Result.DataSourcesVisible = false;
						newTraceWindowCommand.Result.AlreadyCaptured = true;
						foreach (KeyValuePair<string, Dictionary<string, DataPointList>> keyValuePair in this.m_realtimeValues)
						{
							if (keyValuePair.Value != null)
							{
								AddGroupCommand addGroupCommand = new AddGroupCommand();
								addGroupCommand.GroupName = keyValuePair.Key;
								addGroupCommand.Container = newTraceWindowCommand.Result;
								SdpApp.CommandManager.ExecuteCommand(addGroupCommand);
								if (addGroupCommand.Result != null)
								{
									foreach (KeyValuePair<string, DataPointList> keyValuePair2 in this.m_realtimeValues[keyValuePair.Key])
									{
										AddTrackToGroupCommand addTrackToGroupCommand = new AddTrackToGroupCommand();
										addTrackToGroupCommand.Container = addGroupCommand.Result;
										addTrackToGroupCommand.TrackType = TrackType.Graph;
										SdpApp.CommandManager.ExecuteCommand(addTrackToGroupCommand);
										if (addTrackToGroupCommand.Result != null && addTrackToGroupCommand.Result is GraphTrackController)
										{
											GraphTrackController graphTrackController = (GraphTrackController)addTrackToGroupCommand.Result;
											graphTrackController.View.ControlPanelHeaderBackColor = FormatHelper.PseudoRandomColor();
											AddMetricToTrackCommand addMetricToTrackCommand = new AddMetricToTrackCommand();
											addMetricToTrackCommand.Container = addTrackToGroupCommand.Result;
											addMetricToTrackCommand.MetricId = 0U;
											addMetricToTrackCommand.MetricName = keyValuePair2.Key;
											addMetricToTrackCommand.PID = 0U;
											SdpApp.CommandManager.ExecuteCommand(addMetricToTrackCommand);
											graphTrackController.AddTransientMetricData(addMetricToTrackCommand.MetricName, keyValuePair2.Value);
										}
									}
								}
							}
						}
						SetDataViewBoundsCommand setDataViewBoundsCommand = new SetDataViewBoundsCommand();
						setDataViewBoundsCommand.Minimum = 0.0;
						setDataViewBoundsCommand.Maximum = this.m_realtimeMax;
						setDataViewBoundsCommand.Dirty = true;
						setDataViewBoundsCommand.CaptureId = newTraceWindowCommand.CaptureID;
						SdpApp.CommandManager.ExecuteCommand(setDataViewBoundsCommand);
					}
				}
				ShowMessageDialogCommand.ShowMessage(((CaptureType)capture.CaptureType).ToString() + " opened successfully", IconType.Success);
			});
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00010260 File Offset: 0x0000E460
		private bool ProcessTraceCapture(CaptureInfo capture)
		{
			SdpApp.Platform.Invoke(delegate
			{
				ZipArchive zipArchive = ZipFile.Open(this.m_sessionPath, ZipArchiveMode.Read);
				ZipArchiveEntry DCAPEntry = zipArchive.Entries.FirstOrDefault((ZipArchiveEntry entry) => entry.Name == "sdpframe_" + capture.CaptureID.ToString().PadLeft(3, '0') + ".dcap");
				string text = "Capture";
				if (DCAPEntry != null)
				{
					text = "Insight";
				}
				NewTraceWindowCommand newTrace = new NewTraceWindowCommand();
				newTrace.CaptureID = SdpApp.ConnectionManager.CreateCaptureId(CaptureType.Trace);
				newTrace.Name = (string.IsNullOrEmpty(capture.CaptureName) ? (Path.GetFileName(this.m_sessionPath) + " (" + newTrace.CaptureID.ToString() + ")") : capture.CaptureName);
				newTrace.SDPVersion = string.Concat(new string[]
				{
					this.m_versionMajor.ToString(),
					".",
					this.m_versionMinor.ToString(),
					".",
					this.m_versionSubminor.ToString()
				});
				newTrace.Layout = text;
				SdpApp.CommandManager.ExecuteCommand(newTrace);
				if (newTrace.Result != null)
				{
					SdpApp.Platform.Invoke(delegate
					{
						SdpApp.ModelManager.TraceModel.CurrentCaptureGroupLayoutController = newTrace.Result;
						if (DCAPEntry != null)
						{
							SdpApp.EventsManager.Raise<OpenTraceFromSessionArgs>(SdpApp.EventsManager.ConnectionEvents.OpenTraceFromSession, null, new OpenTraceFromSessionArgs
							{
								SelectedCaptureID = (uint)capture.CaptureID,
								NewCaptureID = newTrace.Result.CaptureId,
								SessionPath = this.m_sessionPath
							});
						}
						newTrace.Result.CaptureButtonVisible = false;
						newTrace.Result.DataSourcesVisible = false;
						newTrace.Result.AlreadyCaptured = true;
						bool flag = false;
						if (SdpApp.IsCurrentVersionNewerThan(this.m_versionMajor, this.m_versionMinor, 2018, 2))
						{
							flag = this.GetTraceCaptureMetrics(capture.CaptureID, newTrace.CaptureID);
						}
						flag |= ProcessorPluginMgr.Get().ImportCapture(capture.CaptureID, (int)newTrace.CaptureID, this.m_tmpsdpdbFilename + ".db", this.m_versionMajor, this.m_versionMinor, this.m_versionSubminor);
						newTrace.Result.ActivateExportButton();
						if (!flag)
						{
							ShowMessageDialogCommand.ShowErrorDialog("Not a valid capture.");
							return;
						}
					});
					ShowMessageDialogCommand.ShowMessage(((CaptureType)capture.CaptureType).ToString() + " opened successfully", IconType.Success);
					return;
				}
				ShowMessageDialogCommand.ShowErrorDialog("Not a valid capture.");
			});
			return false;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00010298 File Offset: 0x0000E498
		private bool ProcessSnapshotCapture(CaptureInfo capture, List<string> files)
		{
			string text = capture.CaptureID.ToString().PadLeft(3, '0');
			foreach (string text2 in files)
			{
				if (text2.EndsWith(text + ".vkz"))
				{
					ShowMessageDialogCommand.ShowMessage("This snapshot is no longer supported.\nPlease use Snapdragon Profiler version 2024.9 or earlier to open this snapshot.", IconType.Error);
					return false;
				}
			}
			uint newSnapshotCaptureId = SdpApp.ConnectionManager.CreateCaptureId(CaptureType.Snapshot);
			int num = 0;
			Model model = this.m_dataModel.GetModel("CaptureManager");
			ModelObject modelObject = model.GetModelObject("Capture");
			SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM Capture WHERE captureID = " + capture.CaptureID.ToString(), this.m_dbConnection);
			SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
			while (sqliteDataReader.Read())
			{
				ModelObjectData dataByID = modelObject.GetDataByID((long)((ulong)newSnapshotCaptureId));
				if (dataByID.IsValid())
				{
					dataByID.SetAttributeValue("captureID", newSnapshotCaptureId.ToString());
					num = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("processID"));
					ModelObjectData modelObjectData = dataByID;
					string text3 = "processID";
					uint num2 = (uint)num;
					modelObjectData.SetAttributeValue(text3, num2.ToString());
					dataByID.SetAttributeValue("captureType", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("captureType"))).ToString());
					dataByID.SetAttributeValue("startTimeTOD", ((uint)sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("startTimeTOD"))).ToString());
					dataByID.SetAttributeValue("stopTimeTOD", ((uint)sqliteDataReader.GetInt64(sqliteDataReader.GetOrdinal("stopTimeTOD"))).ToString());
					dataByID.SetAttributeValue("startDelay", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("startDelay"))).ToString());
					dataByID.SetAttributeValue("duration", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("duration"))).ToString());
					int ordinal = sqliteDataReader.GetOrdinal("rendererString");
					if (ordinal != -1)
					{
						string @string = sqliteDataReader.GetString(ordinal);
						dataByID.SetAttributeValue("rendererString", @string);
						SdpApp.ConnectionManager.CurrentRendererString = @string;
					}
					dataByID.Update();
				}
			}
			RenderingAPI api = this.GetSnapshotRenderingAPI(capture.CaptureID, num, files);
			model = this.m_dataModel.GetModel("CaptureManager");
			modelObject = model.GetModelObject("CaptureMetrics");
			sqliteCommand = new SQLiteCommand("SELECT * FROM CaptureMetrics WHERE captureID = " + capture.CaptureID.ToString(), this.m_dbConnection);
			sqliteDataReader = sqliteCommand.ExecuteReader();
			while (sqliteDataReader.Read())
			{
				ModelObjectData modelObjectData2 = modelObject.NewData();
				modelObjectData2.SetAttributeValue("captureID", newSnapshotCaptureId.ToString());
				modelObjectData2.SetAttributeValue("processID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("processID"))).ToString());
				modelObjectData2.SetAttributeValue("metricID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("metricID"))).ToString());
				modelObjectData2.Save();
			}
			if ((SdpApp.ConnectionManager.GetConnectedDevice() == null || SdpApp.ConnectionManager.GetDeviceOS() == ConnectionManager.DeviceOS.HLM || SdpApp.ConnectionManager.GetDeviceOS() == ConnectionManager.DeviceOS.Windows) && (api == RenderingAPI.DirectX12 || api == RenderingAPI.None))
			{
				SDPProcessorPlugin plugin = ProcessorPluginMgr.Get().GetPlugin("SDP::Dx12ProcessorPlugin");
				if (plugin != null)
				{
					plugin.ImportCapture((uint)capture.CaptureID, newSnapshotCaptureId, this.m_tmpsdpdbFilename + ".db", this.m_versionMajor, this.m_versionMinor, this.m_versionSubminor);
				}
			}
			if (api == RenderingAPI.Vulkan || api == RenderingAPI.None)
			{
				bool flag = false;
				model = this.m_dataModel.GetModel("QGLModel");
				modelObject = model.GetModelObject("VulkanShaderStatProperty");
				sqliteCommand = new SQLiteCommand("SELECT * FROM sqlite_master WHERE TYPE = 'table' AND NAME = 'VulkanShaderStatProperty'", this.m_dbConnection);
				sqliteDataReader = sqliteCommand.ExecuteReader();
				if (sqliteDataReader.HasRows)
				{
					sqliteCommand = new SQLiteCommand("SELECT * FROM VulkanShaderStatProperty WHERE captureID = " + uint.MaxValue.ToString(), this.m_dbConnection);
					sqliteDataReader = sqliteCommand.ExecuteReader();
					if (sqliteDataReader.HasRows)
					{
						flag = true;
					}
					while (sqliteDataReader.Read())
					{
						ModelObjectData modelObjectData3 = modelObject.NewData();
						modelObjectData3.SetAttributeValue("captureID", newSnapshotCaptureId.ToString());
						modelObjectData3.SetAttributeValue("statID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("statID"))).ToString());
						modelObjectData3.SetAttributeValue("name", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("name")).ToString());
						modelObjectData3.SetAttributeValue("description", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("description")).ToString());
						modelObjectData3.Save();
					}
					if (flag)
					{
						bool flag2 = false;
						modelObject = model.GetModelObject("VulkanSnapshotShaderData");
						if (modelObject == null)
						{
							model = this.m_dataModel.GetModel("VulkanSnapshot");
							modelObject = model.GetModelObject("VulkanSnapshotShaderData");
						}
						sqliteCommand = new SQLiteCommand("SELECT * FROM VulkanSnapshotShaderData WHERE captureID = " + capture.CaptureID.ToString(), this.m_dbConnection);
						sqliteDataReader = sqliteCommand.ExecuteReader();
						if (sqliteDataReader.HasRows)
						{
							flag2 = true;
						}
						while (sqliteDataReader.Read() && modelObject != null)
						{
							ModelObjectData modelObjectData4 = modelObject.NewData();
							modelObjectData4.SetAttributeValue("captureID", newSnapshotCaptureId.ToString());
							modelObjectData4.SetAttributeValue("pipelineID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("pipelineID"))).ToString());
							modelObjectData4.SetAttributeValue("shaderStage", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("shaderStage"))).ToString());
							modelObjectData4.SetAttributeValue("shaderIndex", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("shaderIndex"))).ToString());
							modelObjectData4.SetAttributeValue("shaderModuleID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("shaderModuleID"))).ToString());
							long bytes = sqliteDataReader.GetBytes(sqliteDataReader.GetOrdinal("shaderStatValues"), 0L, null, 0, 0);
							byte[] array = new byte[bytes];
							sqliteDataReader.GetBytes(sqliteDataReader.GetOrdinal("shaderStatValues"), 0L, array, 0, (int)bytes);
							OpenSessionDialogController.BinaryDataPair binaryDataPair = new OpenSessionDialogController.BinaryDataPair();
							binaryDataPair.size = (uint)bytes;
							IntPtr intPtr = Marshal.AllocHGlobal((int)binaryDataPair.size);
							Marshal.Copy(array, 0, intPtr, (int)binaryDataPair.size);
							binaryDataPair.data = intPtr;
							int num3 = Marshal.SizeOf(typeof(OpenSessionDialogController.BinaryDataPair));
							IntPtr intPtr2 = Marshal.AllocHGlobal(num3);
							Marshal.StructureToPtr<OpenSessionDialogController.BinaryDataPair>(binaryDataPair, intPtr2, false);
							modelObjectData4.SetAttributeValue("shaderStatValues", intPtr2);
							modelObjectData4.SetAttributeValue("shaderStats", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("shaderStats")).ToString());
							modelObjectData4.SetAttributeValue("shaderDisasm", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("shaderDisasm")).ToString());
							modelObjectData4.Save();
							Marshal.FreeHGlobal(intPtr);
							Marshal.FreeHGlobal(intPtr2);
						}
						if (flag2)
						{
							DataProcessedEventArgs dataProcessedEventArgs = new DataProcessedEventArgs();
							dataProcessedEventArgs.CaptureID = newSnapshotCaptureId;
							dataProcessedEventArgs.BufferID = 0U;
							dataProcessedEventArgs.BufferCategory = SDPCore.BUFFER_TYPE_VULKAN_SNAPSHOT_SHADER_DATA;
							SdpApp.EventsManager.Raise<DataProcessedEventArgs>(SdpApp.EventsManager.ConnectionEvents.DataProcessed, this, dataProcessedEventArgs);
						}
					}
				}
				SDPProcessorPlugin plugin2 = ProcessorPluginMgr.Get().GetPlugin("SDP::QGLPluginProcessor");
				if (plugin2 != null)
				{
					plugin2.ImportCapture((uint)capture.CaptureID, newSnapshotCaptureId, this.m_tmpsdpdbFilename + ".db", this.m_versionMajor, this.m_versionMinor, this.m_versionSubminor);
				}
			}
			string tempFile = null;
			if (api == RenderingAPI.OpenGL || api == RenderingAPI.None)
			{
				bool flag3 = false;
				model = this.m_dataModel.GetModel("GLESModel");
				modelObject = model.GetModelObject("GLESShaderStatProperty");
				sqliteCommand = new SQLiteCommand("SELECT * FROM sqlite_master WHERE TYPE = 'table' AND NAME = 'GLESShaderStatProperty'", this.m_dbConnection);
				sqliteDataReader = sqliteCommand.ExecuteReader();
				if (sqliteDataReader.HasRows)
				{
					sqliteCommand = new SQLiteCommand("SELECT * FROM GLESShaderStatProperty WHERE captureID = " + uint.MaxValue.ToString(), this.m_dbConnection);
					sqliteDataReader = sqliteCommand.ExecuteReader();
					if (sqliteDataReader.HasRows)
					{
						flag3 = true;
					}
					while (sqliteDataReader.Read())
					{
						ModelObjectData modelObjectData5 = modelObject.NewData();
						modelObjectData5.SetAttributeValue("captureID", newSnapshotCaptureId.ToString());
						modelObjectData5.SetAttributeValue("statID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("statID"))).ToString());
						modelObjectData5.SetAttributeValue("name", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("name")).ToString());
						modelObjectData5.SetAttributeValue("description", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("description")).ToString());
						modelObjectData5.Save();
					}
					if (flag3)
					{
						bool flag4 = false;
						modelObject = model.GetModelObject("GLESSnapshotShaderStatsData");
						sqliteCommand = new SQLiteCommand("SELECT * FROM GLESSnapshotShaderStatsData WHERE captureID = " + capture.CaptureID.ToString(), this.m_dbConnection);
						sqliteDataReader = sqliteCommand.ExecuteReader();
						if (sqliteDataReader.HasRows)
						{
							flag4 = true;
						}
						while (sqliteDataReader.Read())
						{
							ModelObjectData modelObjectData6 = modelObject.NewData();
							modelObjectData6.SetAttributeValue("captureID", newSnapshotCaptureId.ToString());
							modelObjectData6.SetAttributeValue("programID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("programID"))).ToString());
							modelObjectData6.SetAttributeValue("shaderStage", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("shaderStage"))).ToString());
							modelObjectData6.SetAttributeValue("compilerError", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("compilerError"))).ToString());
							long bytes2 = sqliteDataReader.GetBytes(sqliteDataReader.GetOrdinal("shaderStatValues"), 0L, null, 0, 0);
							byte[] array2 = new byte[bytes2];
							sqliteDataReader.GetBytes(sqliteDataReader.GetOrdinal("shaderStatValues"), 0L, array2, 0, (int)bytes2);
							OpenSessionDialogController.BinaryDataPair binaryDataPair2 = new OpenSessionDialogController.BinaryDataPair();
							binaryDataPair2.size = (uint)bytes2;
							IntPtr intPtr3 = Marshal.AllocHGlobal((int)binaryDataPair2.size);
							Marshal.Copy(array2, 0, intPtr3, (int)binaryDataPair2.size);
							binaryDataPair2.data = intPtr3;
							int num4 = Marshal.SizeOf(typeof(OpenSessionDialogController.BinaryDataPair));
							IntPtr intPtr4 = Marshal.AllocHGlobal(num4);
							Marshal.StructureToPtr<OpenSessionDialogController.BinaryDataPair>(binaryDataPair2, intPtr4, false);
							modelObjectData6.SetAttributeValue("shaderStatValues", intPtr4);
							modelObjectData6.SetAttributeValue("shaderStatsErrors", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("shaderStatsErrors")).ToString());
							Marshal.FreeHGlobal(intPtr3);
							Marshal.FreeHGlobal(intPtr4);
							modelObjectData6.Save();
						}
						if (flag4)
						{
							DataProcessedEventArgs dataProcessedEventArgs2 = new DataProcessedEventArgs();
							dataProcessedEventArgs2.CaptureID = newSnapshotCaptureId;
							dataProcessedEventArgs2.BufferID = 0U;
							dataProcessedEventArgs2.BufferCategory = SDPCore.BUFFER_TYPE_GLES_SHADER_STAT;
							SdpApp.EventsManager.Raise<DataProcessedEventArgs>(SdpApp.EventsManager.ConnectionEvents.DataProcessed, this, dataProcessedEventArgs2);
						}
						flag4 = false;
						modelObject = model.GetModelObject("GLESSnapshotShaderLogsData");
						sqliteCommand = new SQLiteCommand("SELECT * FROM GLESSnapshotShaderLogsData WHERE captureID = " + capture.CaptureID.ToString(), this.m_dbConnection);
						sqliteDataReader = sqliteCommand.ExecuteReader();
						if (sqliteDataReader.HasRows)
						{
							flag4 = true;
						}
						while (sqliteDataReader.Read())
						{
							ModelObjectData modelObjectData7 = modelObject.NewData();
							modelObjectData7.SetAttributeValue("captureID", newSnapshotCaptureId.ToString());
							modelObjectData7.SetAttributeValue("programID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("programID"))).ToString());
							modelObjectData7.SetAttributeValue("shaderStage", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("shaderStage"))).ToString());
							modelObjectData7.SetAttributeValue("compilerError", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("compilerError"))).ToString());
							long bytes3 = sqliteDataReader.GetBytes(sqliteDataReader.GetOrdinal("shaderStatValues"), 0L, null, 0, 0);
							byte[] array3 = new byte[bytes3];
							sqliteDataReader.GetBytes(sqliteDataReader.GetOrdinal("shaderStatValues"), 0L, array3, 0, (int)bytes3);
							OpenSessionDialogController.BinaryDataPair binaryDataPair3 = new OpenSessionDialogController.BinaryDataPair();
							binaryDataPair3.size = (uint)bytes3;
							IntPtr intPtr5 = Marshal.AllocHGlobal((int)binaryDataPair3.size);
							Marshal.Copy(array3, 0, intPtr5, (int)binaryDataPair3.size);
							binaryDataPair3.data = intPtr5;
							int num5 = Marshal.SizeOf(typeof(OpenSessionDialogController.BinaryDataPair));
							IntPtr intPtr6 = Marshal.AllocHGlobal(num5);
							Marshal.StructureToPtr<OpenSessionDialogController.BinaryDataPair>(binaryDataPair3, intPtr6, false);
							modelObjectData7.SetAttributeValue("shaderStatValues", intPtr6);
							modelObjectData7.SetAttributeValue("shaderLogs", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("shaderLogs")).ToString());
							Marshal.FreeHGlobal(intPtr5);
							Marshal.FreeHGlobal(intPtr6);
							modelObjectData7.Save();
						}
						if (flag4)
						{
							DataProcessedEventArgs dataProcessedEventArgs3 = new DataProcessedEventArgs();
							dataProcessedEventArgs3.CaptureID = newSnapshotCaptureId;
							dataProcessedEventArgs3.BufferID = 0U;
							dataProcessedEventArgs3.BufferCategory = SDPCore.BUFFER_TYPE_GLES_SHADER_STAT_CSV;
							SdpApp.EventsManager.Raise<DataProcessedEventArgs>(SdpApp.EventsManager.ConnectionEvents.DataProcessed, this, dataProcessedEventArgs3);
						}
						flag4 = false;
						modelObject = model.GetModelObject("GLESSnapshotShaderDisasmData");
						sqliteCommand = new SQLiteCommand("SELECT * FROM GLESSnapshotShaderDisasmData WHERE captureID = " + capture.CaptureID.ToString(), this.m_dbConnection);
						sqliteDataReader = sqliteCommand.ExecuteReader();
						if (sqliteDataReader.HasRows)
						{
							flag4 = true;
						}
						while (sqliteDataReader.Read())
						{
							ModelObjectData modelObjectData8 = modelObject.NewData();
							modelObjectData8.SetAttributeValue("captureID", newSnapshotCaptureId.ToString());
							modelObjectData8.SetAttributeValue("programID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("programID"))).ToString());
							modelObjectData8.SetAttributeValue("shaderStage", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("shaderStage"))).ToString());
							modelObjectData8.SetAttributeValue("compilerError", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("compilerError"))).ToString());
							modelObjectData8.SetAttributeValue("shaderDisasm", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("shaderDisasm")).ToString());
							modelObjectData8.Save();
						}
						if (flag4)
						{
							DataProcessedEventArgs dataProcessedEventArgs4 = new DataProcessedEventArgs();
							dataProcessedEventArgs4.CaptureID = newSnapshotCaptureId;
							dataProcessedEventArgs4.BufferID = 0U;
							dataProcessedEventArgs4.BufferCategory = SDPCore.BUFFER_TYPE_GLES_SHADER_DISASM;
							SdpApp.EventsManager.Raise<DataProcessedEventArgs>(SdpApp.EventsManager.ConnectionEvents.DataProcessed, this, dataProcessedEventArgs4);
						}
					}
				}
				sqliteCommand = new SQLiteCommand("SELECT * FROM tblBinaryData WHERE capture = " + capture.CaptureID.ToString() + " AND bufferCategory = " + SDPCore.BUFFER_TYPE_GLES_CAPTURE_SCREENSHOT.ToString(), this.m_dbConnection);
				sqliteDataReader = sqliteCommand.ExecuteReader();
				if (sqliteDataReader.Read())
				{
					try
					{
						tempFile = Path.GetTempFileName();
						FileStream fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.ReadWrite);
						int num6 = 16384;
						byte[] array4 = new byte[num6];
						long num7 = 0L;
						long bytes4;
						do
						{
							bytes4 = sqliteDataReader.GetBytes(sqliteDataReader.GetOrdinal("data"), num7, array4, 0, num6);
							if (bytes4 > 0L)
							{
								fileStream.Write(array4, 0, (int)bytes4);
								num7 += bytes4;
							}
						}
						while (bytes4 > 0L);
						fileStream.Close();
					}
					catch (Exception ex)
					{
						ShowMessageDialogCommand.ShowMessage("Issue copying snapshot buffer to file", IconType.Error);
						return false;
					}
				}
			}
			SdpApp.Platform.Invoke(delegate
			{
				NewSnapshotWindowCommand newSnapshotWindowCommand = new NewSnapshotWindowCommand();
				newSnapshotWindowCommand.CaptureID = newSnapshotCaptureId;
				newSnapshotWindowCommand.Layout = "Snapshot";
				newSnapshotWindowCommand.Name = (string.IsNullOrEmpty(capture.CaptureName) ? (Path.GetFileName(this.m_sessionPath) + " (" + newSnapshotWindowCommand.CaptureID.ToString() + ")") : capture.CaptureName);
				SdpApp.CommandManager.ExecuteCommand(newSnapshotWindowCommand);
				if (newSnapshotWindowCommand.Result == null)
				{
					ShowMessageDialogCommand.ShowErrorDialog("Unable to open snapshot.");
					return;
				}
				newSnapshotWindowCommand.Result.DataSourcesVisible = false;
				newSnapshotWindowCommand.Result.AlreadyCaptured = true;
				newSnapshotWindowCommand.Result.AttachOpenSessionEvents();
				if (SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController != null)
				{
					SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.DetachEvents();
					if (!SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.AlreadyCaptured)
					{
						SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController.AlreadyCaptured = true;
					}
				}
				SdpApp.ModelManager.SnapshotModel.CurrentSnapshotController = newSnapshotWindowCommand.Result;
				SdpApp.EventsManager.Raise<OpenSnapshotFromSessionArgs>(SdpApp.EventsManager.ConnectionEvents.OpenSnapshotFromSession, null, new OpenSnapshotFromSessionArgs
				{
					SelectedCaptureID = (uint)capture.CaptureID,
					NewCaptureID = newSnapshotWindowCommand.Result.CaptureId,
					SessionPath = this.m_sessionPath,
					TempImageFile = tempFile,
					API = api
				});
			});
			return true;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00011318 File Offset: 0x0000F518
		private bool ProcessSamplingCapture(CaptureInfo capture)
		{
			SdpApp.Platform.Invoke(delegate
			{
				NewSamplingWindowCommand newSamplingWindowCommand = new NewSamplingWindowCommand();
				newSamplingWindowCommand.CaptureID = SdpApp.ConnectionManager.CreateCaptureId(CaptureType.Sampling);
				newSamplingWindowCommand.Name = (string.IsNullOrEmpty(capture.CaptureName) ? (Path.GetFileName(this.m_sessionPath) + " (" + newSamplingWindowCommand.CaptureID.ToString() + ")") : capture.CaptureName);
				SdpApp.CommandManager.ExecuteCommand(newSamplingWindowCommand);
				if (newSamplingWindowCommand.Result == null)
				{
					ShowMessageDialogCommand.ShowErrorDialog("Unable to open sampling capture.");
					return;
				}
				SdpApp.ModelManager.SamplingModel.CurrentSamplingController = newSamplingWindowCommand.Result;
				newSamplingWindowCommand.Result.DataSourcesVisible = false;
				newSamplingWindowCommand.Result.AlreadyCaptured = true;
				if (!ProcessorPluginMgr.Get().ImportCapture(capture.CaptureID, (int)newSamplingWindowCommand.CaptureID, this.m_tmpsdpdbFilename + ".db", this.m_versionMajor, this.m_versionMinor, this.m_versionSubminor))
				{
					ShowMessageDialogCommand.ShowMessage("Not a valid capture", IconType.Error);
					return;
				}
				ShowMessageDialogCommand.ShowMessage(((CaptureType)capture.CaptureType).ToString() + " opened successfully", IconType.Success);
			});
			return true;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00011350 File Offset: 0x0000F550
		private void InitDBConnection(ZipArchiveEntry sdpdbEntry)
		{
			this.m_tmpsdpdbFilename = Path.GetTempFileName();
			sdpdbEntry.ExtractToFile(this.m_tmpsdpdbFilename + ".db");
			this.m_dbConnection = new SQLiteConnection("Data Source=" + this.m_tmpsdpdbFilename + ".db;Version=3;");
			this.m_dbConnection.Open();
			this.m_dataModel = SdpApp.ConnectionManager.GetDataModel();
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x000113BC File Offset: 0x0000F5BC
		private void GetImportedMetrics()
		{
			if (this.m_dataModel != null)
			{
				Model model = this.m_dataModel.AddModel("ImportSession");
				if (model != null)
				{
					ModelObject modelObject = model.AddObject("ImportedMetrics");
					modelObject.AddAttribute("id", SDPDataType.SDP_UINT32, 4U, 0U);
					modelObject.AddAttribute("categoryID", SDPDataType.SDP_UINT32, 4U, 4U);
					modelObject.AddAttribute("name", SDPDataType.SDP_STRING, 0U, 8U);
					modelObject.Save();
					SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM sqlite_master WHERE TYPE = 'table' AND NAME = 'ImportedMetrics'", this.m_dbConnection);
					SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
					if (sqliteDataReader.HasRows)
					{
						sqliteCommand = new SQLiteCommand("SELECT id, categoryID, name FROM Metric\nUNION\nSELECT id, categoryID, name FROM ImportedMetrics", this.m_dbConnection);
					}
					else
					{
						sqliteCommand = new SQLiteCommand("SELECT id, categoryID, name FROM Metric", this.m_dbConnection);
					}
					sqliteDataReader = sqliteCommand.ExecuteReader();
					while (sqliteDataReader.Read())
					{
						uint @int = (uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("id"));
						ModelObjectDataList data = modelObject.GetData("id", @int.ToString());
						if (data.Count == 0)
						{
							ModelObjectData modelObjectData = modelObject.NewData();
							modelObjectData.SetAttributeValue("id", @int.ToString());
							modelObjectData.SetAttributeValue("categoryID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("categoryID"))).ToString());
							modelObjectData.SetAttributeValue("name", sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("name")));
							modelObjectData.Save();
						}
					}
				}
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00011520 File Offset: 0x0000F720
		private void GetRealTimeValues()
		{
			this.m_realtimeValues = new Dictionary<string, Dictionary<string, DataPointList>>();
			this.m_realtimeMax = 1.0;
			try
			{
				string text = "SELECT Process.name, Metric.name, tblDataLog.timestamp, tblMetricDouble.value FROM tblDataLog INNER JOIN tblMetricDouble ON tblMetricDouble.ROWID = tblDataLog.valueID INNER JOIN Metric ON Metric.id = tblDataLog.metric INNER JOIN Process ON Process.pid = tblDataLog.process UNION SELECT IFNULL(Process.name, 'System'), Metric.name, tblDataLog.timestamp, tblMetricDouble.value FROM tblDataLog LEFT JOIN Process ON Process.pid = tblDataLog.process INNER JOIN Metric ON Metric.id = tblDataLog.metric INNER JOIN tblMetricDouble ON tblMetricDouble.ROWID=tblDataLog.valueID WHERE Process.pid IS NULL ORDER BY tblDataLog.timestamp;";
				SQLiteCommand sqliteCommand = new SQLiteCommand(text, this.m_dbConnection);
				SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
				int num = 0;
				int num2 = 1;
				int num3 = 2;
				int num4 = 3;
				bool flag = true;
				double num5 = 0.0;
				while (sqliteDataReader.Read())
				{
					string @string = sqliteDataReader.GetString(num);
					string string2 = sqliteDataReader.GetString(num2);
					double num6 = (double)sqliteDataReader.GetInt64(num3);
					double num7 = sqliteDataReader.GetDouble(num4);
					if (!this.m_realtimeValues.ContainsKey(@string))
					{
						this.m_realtimeValues.Add(@string, new Dictionary<string, DataPointList>());
					}
					if (!this.m_realtimeValues[@string].ContainsKey(string2))
					{
						this.m_realtimeValues[@string].Add(string2, new DataPointList());
					}
					if (flag)
					{
						flag = false;
						num5 = num6;
					}
					double num8 = num6 - num5;
					this.m_realtimeValues[@string][string2].Add(new DataPoint(num8, num7));
					if (num8 > this.m_realtimeMax)
					{
						this.m_realtimeMax = num8;
					}
				}
			}
			catch (Exception ex)
			{
			}
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001166C File Offset: 0x0000F86C
		private void GetCaptures()
		{
			this.m_captures = new Dictionary<int, Tuple<string, int>>();
			SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM Capture", this.m_dbConnection);
			SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
			while (sqliteDataReader.Read())
			{
				int @int = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("captureID"));
				int ordinal = sqliteDataReader.GetOrdinal("captureName");
				string text = null;
				if (ordinal >= 0)
				{
					text = sqliteDataReader.GetString(ordinal);
				}
				if (this.m_captures.ContainsKey(@int))
				{
					throw new Exception("Two captures with same captureID detected!");
				}
				int int2 = sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("captureType"));
				if (int2 != 1 || this.m_realtimeValues.Count != 0)
				{
					this.m_captures.Add(@int, new Tuple<string, int>(text, int2));
				}
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00011730 File Offset: 0x0000F930
		private bool GetTraceCaptureMetrics(int selectedCaptureID, uint newCaptureID)
		{
			Model model = this.m_dataModel.GetModel("CaptureManager");
			AddMetricEventArgs addMetricEventArgs = new AddMetricEventArgs();
			List<uint> list = new List<uint>();
			if (model != null)
			{
				ModelObject modelObject = model.GetModelObject("CaptureMetrics");
				if (modelObject != null)
				{
					SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM CaptureMetrics WHERE captureID = " + selectedCaptureID.ToString(), this.m_dbConnection);
					SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
					while (sqliteDataReader.Read())
					{
						ModelObjectData modelObjectData = modelObject.NewData();
						modelObjectData.SetAttributeValue("captureID", newCaptureID.ToString());
						modelObjectData.SetAttributeValue("processID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("processID"))).ToString());
						modelObjectData.SetAttributeValue("metricID", ((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("metricID"))).ToString());
						list.Add((uint)sqliteDataReader.GetInt32(sqliteDataReader.GetOrdinal("metricID")));
						modelObjectData.Save();
					}
				}
				modelObject = model.GetModelObject("Capture");
				if (modelObject != null)
				{
					SQLiteCommand sqliteCommand2 = new SQLiteCommand("SELECT * FROM Capture WHERE captureID = " + selectedCaptureID.ToString(), this.m_dbConnection);
					SQLiteDataReader sqliteDataReader2 = sqliteCommand2.ExecuteReader();
					while (sqliteDataReader2.Read())
					{
						ModelObjectData dataByID = modelObject.GetDataByID((long)((ulong)newCaptureID));
						if (dataByID.IsValid())
						{
							dataByID.SetAttributeValue("captureID", newCaptureID.ToString());
							dataByID.SetAttributeValue("processID", ((uint)sqliteDataReader2.GetInt32(sqliteDataReader2.GetOrdinal("processID"))).ToString());
							dataByID.SetAttributeValue("captureType", ((uint)sqliteDataReader2.GetInt32(sqliteDataReader2.GetOrdinal("captureType"))).ToString());
							dataByID.SetAttributeValue("startTimeTOD", sqliteDataReader2.GetInt64(sqliteDataReader2.GetOrdinal("startTimeTOD")).ToString());
							dataByID.SetAttributeValue("stopTimeTOD", sqliteDataReader2.GetInt64(sqliteDataReader2.GetOrdinal("stopTimeTOD")).ToString());
							dataByID.SetAttributeValue("startDelay", ((uint)sqliteDataReader2.GetInt32(sqliteDataReader2.GetOrdinal("startDelay"))).ToString());
							dataByID.SetAttributeValue("duration", ((uint)sqliteDataReader2.GetInt32(sqliteDataReader2.GetOrdinal("duration"))).ToString());
							int ordinal = sqliteDataReader2.GetOrdinal("rendererString");
							if (ordinal != -1)
							{
								string @string = sqliteDataReader2.GetString(ordinal);
								dataByID.SetAttributeValue("rendererString", @string);
								SdpApp.ConnectionManager.CurrentRendererString = @string;
								addMetricEventArgs.renderString = SdpApp.ConnectionManager.CurrentRendererString;
							}
							dataByID.Update();
						}
					}
				}
			}
			if (list.Count<uint>() > 0)
			{
				model = this.m_dataModel.GetModel("MetricManager");
				if (model != null)
				{
					ModelObject modelObject2 = model.GetModelObject("Metric");
					if (modelObject2 != null)
					{
						SQLiteCommand sqliteCommand3 = new SQLiteCommand("SELECT * FROM Metric ", this.m_dbConnection);
						SQLiteDataReader sqliteDataReader3 = sqliteCommand3.ExecuteReader();
						int num = 0;
						uint metricId;
						while (sqliteDataReader3.Read() && num < list.Count<uint>())
						{
							metricId = (uint)sqliteDataReader3.GetDecimal(sqliteDataReader3.GetOrdinal("id"));
							bool flag = Convert.ToBoolean(sqliteDataReader3.GetDecimal(sqliteDataReader3.GetOrdinal("state")));
							if (list.Exists((uint x) => x == metricId) && flag)
							{
								string string2 = sqliteDataReader3.GetString(sqliteDataReader3.GetOrdinal("name"));
								if (string2 != "OpenCL Trace" && string2 != "GPU Roofline Analysis")
								{
									addMetricEventArgs.metricsList.Add(string2);
								}
								num++;
							}
						}
					}
				}
			}
			SdpApp.EventsManager.Raise<AddMetricEventArgs>(SdpApp.EventsManager.ConnectionEvents.MetricsAddition, this, addMetricEventArgs);
			SQLiteCommand sqliteCommand4 = new SQLiteCommand(string.Concat(new string[]
			{
				"SELECT data FROM tblBinaryData WHERE capture = ",
				selectedCaptureID.ToString(),
				" AND bufferCategory = ",
				SDPCore.BUFFER_TYPE_CL_ROOFLINE_DATA.ToString(),
				" AND bufferID = 1"
			}), this.m_dbConnection);
			SQLiteDataReader sqliteDataReader4 = sqliteCommand4.ExecuteReader();
			if (sqliteDataReader4.Read())
			{
				byte[] array = (byte[])sqliteDataReader4[0];
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				SdpApp.EventsManager.Raise<BufferTransferEventArgs>(SdpApp.EventsManager.ConnectionEvents.ClientBufferTransfer, this, new BufferTransferEventArgs
				{
					CaptureID = newCaptureID,
					BufferID = 0U,
					BufferCategory = SDPCore.BUFFER_TYPE_CL_ROOFLINE_DATA,
					ProviderID = 0U,
					BufferData = intPtr,
					BufferDataLength = (uint)array.Length
				});
				Marshal.FreeHGlobal(intPtr);
			}
			SQLiteCommand sqliteCommand5 = new SQLiteCommand("SELECT data FROM tblBinaryData WHERE capture = " + selectedCaptureID.ToString() + " AND bufferCategory = " + SDPCore.BUFFER_TYPE_CL_TRACE_DATA.ToString(), this.m_dbConnection);
			SQLiteDataReader sqliteDataReader5 = sqliteCommand5.ExecuteReader();
			if (sqliteDataReader5.Read())
			{
				byte[] array2 = (byte[])sqliteDataReader5[0];
				IntPtr intPtr2 = Marshal.AllocHGlobal(array2.Length);
				Marshal.Copy(array2, 0, intPtr2, array2.Length);
				SdpApp.EventsManager.Raise<BufferTransferEventArgs>(SdpApp.EventsManager.ConnectionEvents.ClientBufferTransfer, this, new BufferTransferEventArgs
				{
					CaptureID = newCaptureID,
					BufferID = 0U,
					BufferCategory = SDPCore.BUFFER_TYPE_CL_TRACE_DATA,
					ProviderID = 0U,
					BufferData = intPtr2,
					BufferDataLength = (uint)array2.Length
				});
				Marshal.FreeHGlobal(intPtr2);
				return true;
			}
			return false;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00011CC8 File Offset: 0x0000FEC8
		private RenderingAPI GetSnapshotRenderingAPI(int captureID, int pid, List<string> files)
		{
			string text = captureID.ToString().PadLeft(3, '0');
			foreach (string text2 in files)
			{
				if (text2.EndsWith(text + ".dcap"))
				{
					return RenderingAPI.OpenGL;
				}
				if (text2.EndsWith(text + ".gfxrz"))
				{
					SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM Metric WHERE name LIKE '%Snapshot' AND pid = " + pid.ToString(), this.m_dbConnection);
					SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
					while (sqliteDataReader.Read())
					{
						string @string = sqliteDataReader.GetString(sqliteDataReader.GetOrdinal("name"));
						if (@string == "DX12 Snapshot")
						{
							return RenderingAPI.DirectX12;
						}
						if (@string == "Vulkan Snapshot")
						{
							return RenderingAPI.Vulkan;
						}
					}
				}
			}
			OpenSessionDialogController.Logger.LogWarning("Unable to detect snapshot graphics API in capture " + captureID.ToString());
			return RenderingAPI.None;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x00011DDC File Offset: 0x0000FFDC
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x00011DE3 File Offset: 0x0000FFE3
		public static ILogger Logger { private get; set; } = new Sdp.Logging.Logger("OpenSession");

		// Token: 0x040006FD RID: 1789
		private string m_sessionPath;

		// Token: 0x040006FE RID: 1790
		private DataModel m_dataModel;

		// Token: 0x040006FF RID: 1791
		private int m_versionMajor;

		// Token: 0x04000700 RID: 1792
		private int m_versionMinor;

		// Token: 0x04000701 RID: 1793
		private int m_versionSubminor;

		// Token: 0x04000702 RID: 1794
		private string m_tmpsdpdbFilename;

		// Token: 0x04000703 RID: 1795
		private Dictionary<string, Dictionary<string, DataPointList>> m_realtimeValues;

		// Token: 0x04000704 RID: 1796
		private double m_realtimeMax;

		// Token: 0x04000705 RID: 1797
		private Dictionary<int, Tuple<string, int>> m_captures;

		// Token: 0x04000706 RID: 1798
		private SQLiteConnection m_dbConnection;

		// Token: 0x04000707 RID: 1799
		private IOpenSessionDialog m_view;

		// Token: 0x02000383 RID: 899
		[StructLayout(LayoutKind.Sequential)]
		public class BinaryDataPair
		{
			// Token: 0x04000C42 RID: 3138
			public uint size;

			// Token: 0x04000C43 RID: 3139
			public IntPtr data;
		}
	}
}
