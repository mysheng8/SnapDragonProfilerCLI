using System;
using System.Collections.Generic;
using System.Linq;
using Cairo;
using Sdp.Charts;
using Sdp.Charts.Roofline;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x020001CC RID: 460
	public class RooflineController : IViewController
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x0000DC10 File Offset: 0x0000BE10
		public RooflineController(IRooflineView view)
		{
			this.m_view = view;
			this.m_view.ZoomIn += this.view_ZoomIn;
			this.m_view.ZoomOut += this.view_ZoomOut;
			this.m_view.ResetViewBounds += this.view_ResetViewBounds;
			this.m_view.DataViewBoundsChanged += this.view_DataViewBoundsChanged;
			this.m_view.KernelToggled += this.view_KernelToggled;
			this.m_view.ColorChanged += this.view_ColorChanged;
			this.m_view.SelectAllToggled += this.view_SelectAllToggled;
			this.m_view.TreeFilteredEvent += this.view_TreeFiltered;
			RooflineEvents rooflineEvents = SdpApp.EventsManager.RooflineEvents;
			rooflineEvents.ProcessKernelDataEvent = (EventHandler<RooflineKernelEventArgs>)Delegate.Combine(rooflineEvents.ProcessKernelDataEvent, new EventHandler<RooflineKernelEventArgs>(this.rooflineEvents_ProcessKernelData));
			RooflineEvents rooflineEvents2 = SdpApp.EventsManager.RooflineEvents;
			rooflineEvents2.RooflineCompleteEvent = (EventHandler<RooflineCompleteEventArgs>)Delegate.Combine(rooflineEvents2.RooflineCompleteEvent, new EventHandler<RooflineCompleteEventArgs>(this.rooflineEvents_RooflineComplete));
			ClientEvents clientEvents = SdpApp.EventsManager.ClientEvents;
			clientEvents.CaptureNameChanged = (EventHandler<CaptureNameChangedArgs>)Delegate.Combine(clientEvents.CaptureNameChanged, new EventHandler<CaptureNameChangedArgs>(this.clientEvents_CaptureNameChanged));
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0000DD84 File Offset: 0x0000BF84
		private RooflineController.KernelDelta GetKernelDeltas(RooflineKernel baseline, RooflineKernel testKernel)
		{
			RooflineController.KernelDelta kernelDelta = new RooflineController.KernelDelta();
			float num = (float)testKernel.Duration - (float)baseline.Duration;
			kernelDelta.Duration = num / (float)baseline.Duration;
			num = testKernel.AluUtilization - baseline.AluUtilization;
			kernelDelta.AluUtilization = ((baseline.AluUtilization == 0f) ? 0f : (num / baseline.AluUtilization * 100f));
			num = testKernel.PercentL2 - baseline.PercentL2;
			kernelDelta.PercentL2 = ((baseline.PercentL2 == 0f) ? 0f : (num / baseline.PercentL2 * 100f));
			num = (float)testKernel.BytesRead - (float)baseline.BytesRead;
			kernelDelta.BytesRead = ((baseline.BytesRead == 0L) ? 0f : (num / (float)baseline.BytesRead * 100f));
			num = (float)testKernel.BytesWritten - (float)baseline.BytesWritten;
			kernelDelta.BytesWritten = ((baseline.BytesWritten == 0L) ? 0f : (num / (float)baseline.BytesWritten * 100f));
			num = testKernel.Perf - baseline.Perf;
			kernelDelta.Perf = ((baseline.Perf == 0f) ? 0f : (num / baseline.Perf * 100f));
			num = testKernel.OperationIntensity - baseline.OperationIntensity;
			kernelDelta.OperationIntensity = ((baseline.OperationIntensity == 0f) ? 0f : (num / baseline.OperationIntensity * 100f));
			kernelDelta.Iterations = (float)(testKernel.Iterations - baseline.Iterations);
			return kernelDelta;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0000DF10 File Offset: 0x0000C110
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

		// Token: 0x060005F7 RID: 1527 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0000DF3F File Offset: 0x0000C13F
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0000DF48 File Offset: 0x0000C148
		private void rooflineEvents_ProcessKernelData(object sender, RooflineKernelEventArgs args)
		{
			Dictionary<string, RooflineKernel> dictionary;
			if (!this.m_rooflineKernels.TryGetValue(args.CaptureID, out dictionary))
			{
				dictionary = (this.m_rooflineKernels[args.CaptureID] = new Dictionary<string, RooflineKernel>());
			}
			string nameString = RooflineModel.GetNameString(args.KernelName, args.KernelID);
			RooflineKernel rooflineKernel;
			if (dictionary.TryGetValue(nameString, out rooflineKernel))
			{
				return;
			}
			RooflineKernel rooflineKernel2 = new RooflineKernel();
			float num = this.TO_MEGA((float)args.Duration);
			float num2 = (float)SdpApp.ModelManager.RooflineModel.MaxGPUFrequency;
			float num3 = (float)args.KernelCycles / num;
			float num4 = num2 - num3;
			float num5 = ((args.Duration < 250L) ? num2 : num3);
			float num6 = this.TO_GIGA(SdpApp.ModelManager.RooflineModel.BaseRooflinePerf * num5);
			float num7 = (float)args.FullALUs / ((float)args.FullALUs + (float)args.HalfALUs) * num6;
			float num8 = (float)args.HalfALUs / ((float)args.FullALUs + (float)args.HalfALUs) * num6 * 2f;
			float num9 = (num7 + num8) * (args.AluUtilization / 100f);
			float num10 = (float)(args.BytesRead + args.BytesWritten);
			rooflineKernel2.KernelName = args.KernelName;
			rooflineKernel2.KernelID = args.KernelID;
			rooflineKernel2.ProgramID = args.ProgramID;
			rooflineKernel2.Duration = args.Duration;
			rooflineKernel2.AluUtilization = args.AluUtilization;
			rooflineKernel2.PercentL2 = args.PercentL2;
			rooflineKernel2.BytesRead = args.BytesRead;
			rooflineKernel2.BytesWritten = args.BytesWritten;
			rooflineKernel2.Perf = num9;
			rooflineKernel2.OperationIntensity = this.FROM_GIGA(num9) * num / num10;
			rooflineKernel2.CaptureID = args.CaptureID;
			rooflineKernel2.RegisterSpilling = args.RegisterSpilling;
			rooflineKernel2.ID = RooflineController.m_nextKernelId++;
			rooflineKernel2.Iterations = args.Iterations;
			dictionary[nameString] = rooflineKernel2;
			EventHandler<RooflineKernelCreatedEventArgs> kernelCreatedEvent = SdpApp.EventsManager.RooflineEvents.KernelCreatedEvent;
			if (kernelCreatedEvent != null)
			{
				kernelCreatedEvent(this, new RooflineKernelCreatedEventArgs
				{
					Kernel = rooflineKernel2,
					CaptureID = args.CaptureID
				});
			}
			if (RooflineController.m_nextKernelId == 2)
			{
				List<Series> list = new List<Series>();
				Series series = new Series();
				series.Points.AddRoofline(new Sdp.Charts.Roofline.Point(RooflineController.m_graphMin, RooflineController.m_graphMin));
				series.Points.AddRoofline(new Sdp.Charts.Roofline.Point(RooflineController.m_graphMax, 10000.0));
				series.Color = new Color(1.0, 0.41, 0.71);
				series.SeriesID = -1;
				series.Name = "";
				list.Add(series);
				double num11 = (double)SdpApp.ModelManager.RooflineModel.PeakRooflinePerf / (double)SdpApp.ModelManager.RooflineModel.PeakRooflineMemBW;
				series = new Series();
				series.IsRooflineCeiling = true;
				series.Points.AddRoofline(new Sdp.Charts.Roofline.Point(RooflineController.m_graphMin / (double)SdpApp.ModelManager.RooflineModel.PeakRooflineMemBW, RooflineController.m_graphMin));
				series.Points.AddRoofline(new Sdp.Charts.Roofline.Point(num11, (double)SdpApp.ModelManager.RooflineModel.PeakRooflinePerf));
				series.Points.AddRoofline(new Sdp.Charts.Roofline.Point(RooflineController.m_graphMax, (double)SdpApp.ModelManager.RooflineModel.PeakRooflinePerf));
				series.SeriesID = -2;
				series.Color = FormatHelper.Cyan;
				series.Name = "Full Precision";
				list.Add(series);
				series = new Series();
				series.IsRooflineCeiling = true;
				series.Points.AddRoofline(new Sdp.Charts.Roofline.Point(num11, (double)SdpApp.ModelManager.RooflineModel.PeakRooflinePerf));
				series.Points.AddRoofline(new Sdp.Charts.Roofline.Point(num11 * 2.0, (double)(SdpApp.ModelManager.RooflineModel.PeakRooflinePerf * 2f)));
				series.Points.AddRoofline(new Sdp.Charts.Roofline.Point(RooflineController.m_graphMax, (double)(SdpApp.ModelManager.RooflineModel.PeakRooflinePerf * 2f)));
				series.SeriesID = -3;
				series.Color = FormatHelper.SBlue;
				series.Name = "Half Precision";
				list.Add(series);
				this.m_view.AddSeries(list, 0.0, 0.0);
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0000E3A0 File Offset: 0x0000C5A0
		private void rooflineEvents_RooflineComplete(object sender, RooflineCompleteEventArgs args)
		{
			Dictionary<string, RooflineKernel> dictionary;
			if (this.m_rooflineKernels.TryGetValue(args.CaptureID, out dictionary))
			{
				Dictionary<uint, List<object[]>> dictionary2 = new Dictionary<uint, List<object[]>>();
				List<Series> list = new List<Series>();
				double num = double.MaxValue;
				double num2 = double.MinValue;
				foreach (RooflineKernel rooflineKernel in dictionary.Values)
				{
					List<object[]> list2;
					if (!dictionary2.TryGetValue(rooflineKernel.ProgramID, out list2))
					{
						list2 = (dictionary2[rooflineKernel.ProgramID] = new List<object[]>());
					}
					double num3 = RooflineMath.DistanceFromPeak(rooflineKernel.OperationIntensity, rooflineKernel.Perf);
					if (num3 < num)
					{
						num = num3;
					}
					if (num3 > num2)
					{
						num2 = num3;
					}
					string nameString = RooflineModel.GetNameString(rooflineKernel.KernelName, rooflineKernel.KernelID);
					list2.Add(new object[]
					{
						true,
						nameString,
						rooflineKernel.ID,
						true,
						default(Color),
						0,
						rooflineKernel.Duration,
						num3
					});
					this.m_enabledKernels.Add(rooflineKernel.ID);
					Series series = new Series();
					series.Points.AddRoofline(new Sdp.Charts.Roofline.Point((double)rooflineKernel.OperationIntensity, (double)rooflineKernel.Perf));
					series.Name = nameString;
					series.SeriesID = rooflineKernel.ID;
					series.Color = this.ColorForCapture();
					series.Duration = (float)rooflineKernel.Duration;
					list.Add(series);
				}
				this.m_view.AddSeries(list, num, num2);
				foreach (KeyValuePair<uint, List<object[]>> keyValuePair in dictionary2)
				{
					this.m_view.AddNodesToCategory(keyValuePair.Value, "Program 0x" + keyValuePair.Key.ToString("X"), args.CaptureID, this.ColorForCapture(), RooflineController.m_nextKernelId);
					RooflineController.m_nextKernelId++;
					if (keyValuePair.Key == dictionary2.First<KeyValuePair<uint, List<object[]>>>().Key)
					{
						RooflineController.m_nextKernelId++;
					}
				}
				this.InvalidateStats();
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0000E64C File Offset: 0x0000C84C
		private void AnalyzeKernel(RooflineKernel kernel, ref List<object[]> analysis)
		{
			this.m_rooflineThresholds.OperationIntensity = SdpApp.ModelManager.RooflineModel.PeakRooflinePerf / SdpApp.ModelManager.RooflineModel.PeakRooflineMemBW;
			List<RooflineModel.RuleViolation> list;
			SdpApp.ModelManager.RooflineModel.GetRuleViolations(kernel, this.m_rooflineThresholds, out list);
			int num = 0;
			foreach (RooflineModel.RuleViolation ruleViolation in list)
			{
				if (ruleViolation.Severity == RooflineModel.ViolationSeverity.Error)
				{
					List<object[]> list2 = analysis;
					object[] array = new object[3];
					int num2 = 0;
					int num3;
					num = (num3 = num + 1);
					array[num2] = num3.ToString();
					array[1] = 2;
					array[2] = ruleViolation.ErrorMessage;
					list2.Add(array);
				}
				else
				{
					List<object[]> list3 = analysis;
					object[] array2 = new object[3];
					int num4 = 0;
					int num3;
					num = (num3 = num + 1);
					array2[num4] = num3.ToString();
					array2[1] = 1;
					array2[2] = ruleViolation.ErrorMessage;
					list3.Add(array2);
				}
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0000E744 File Offset: 0x0000C944
		private void InvalidateStats()
		{
			List<object[]> list = new List<object[]>();
			List<object[]> list2 = new List<object[]>();
			if (this.m_enabledKernels.Count == 1)
			{
				RooflineKernel kernelByID = this.GetKernelByID(this.m_enabledKernels.First<int>());
				list = this.GenerateStats(kernelByID, null);
				this.m_view.InvalidateStats(list, SdpApp.UIManager.GetWindowNameFromCaptureId((int)kernelByID.CaptureID), "", "Stats for Kernel: " + kernelByID.KernelName);
				this.AnalyzeKernel(kernelByID, ref list2);
				this.m_view.InvalidateAnalysis(list2, "Analysis for kernel: " + kernelByID.KernelName);
				return;
			}
			if (this.m_enabledKernels.Count == 2)
			{
				RooflineKernel kernelByID2 = this.GetKernelByID(this.m_enabledKernels.First<int>());
				RooflineKernel kernelByID3 = this.GetKernelByID(this.m_enabledKernels.Last<int>());
				RooflineController.KernelDelta kernelDeltas = this.GetKernelDeltas(kernelByID2, kernelByID3);
				if (kernelByID2.KernelName == kernelByID3.KernelName)
				{
					list = this.GenerateStats(kernelByID2, kernelDeltas);
					string windowNameFromCaptureId = SdpApp.UIManager.GetWindowNameFromCaptureId((int)kernelByID2.CaptureID);
					string windowNameFromCaptureId2 = SdpApp.UIManager.GetWindowNameFromCaptureId((int)kernelByID3.CaptureID);
					this.m_view.InvalidateStats(list, windowNameFromCaptureId, "Δ " + windowNameFromCaptureId2, "Stats for Kernel: " + kernelByID2.KernelName);
					this.m_view.InvalidateAnalysis(list2, "");
					return;
				}
			}
			this.m_view.InvalidateStats(list, "", "", "");
			this.m_view.InvalidateAnalysis(list2, "");
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0000E8CC File Offset: 0x0000CACC
		private List<object[]> GenerateStats(RooflineKernel first, RooflineController.KernelDelta delta)
		{
			Func<float, string> format = (float x) => x.ToString("F2");
			Func<float, string> func = (float x) => format(x) + "%";
			return new List<object[]>
			{
				new object[]
				{
					"Duration (us)",
					first.Duration.ToString(),
					(delta != null) ? func(delta.Duration) : ""
				},
				new object[]
				{
					"% ALU Utilization",
					format(first.AluUtilization),
					(delta != null) ? func(delta.AluUtilization) : ""
				},
				new object[]
				{
					"% L2 Global Read",
					format(first.PercentL2),
					(delta != null) ? func(delta.PercentL2) : ""
				},
				new object[]
				{
					"Bytes Read",
					first.BytesRead.ToString(),
					(delta != null) ? func(delta.BytesRead) : ""
				},
				new object[]
				{
					"Bytes Written",
					first.BytesWritten.ToString(),
					(delta != null) ? func(delta.BytesWritten) : ""
				},
				new object[]
				{
					"Perf (GFLOPs)",
					format(first.Perf),
					(delta != null) ? func(delta.Perf) : ""
				},
				new object[]
				{
					"Operational Intensity",
					format(first.OperationIntensity),
					(delta != null) ? func(delta.OperationIntensity) : ""
				},
				new object[]
				{
					"Iterations",
					first.Iterations.ToString(),
					(delta != null) ? delta.Iterations.ToString() : ""
				}
			};
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0000EB0C File Offset: 0x0000CD0C
		private RooflineKernel GetKernelByID(int id)
		{
			foreach (KeyValuePair<uint, Dictionary<string, RooflineKernel>> keyValuePair in this.m_rooflineKernels)
			{
				foreach (KeyValuePair<string, RooflineKernel> keyValuePair2 in keyValuePair.Value)
				{
					if (keyValuePair2.Value.ID == id)
					{
						return keyValuePair2.Value;
					}
				}
			}
			return new RooflineKernel();
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0000EBB8 File Offset: 0x0000CDB8
		private Color ColorForCapture()
		{
			return FormatHelper.DefaultColors[(this.m_rooflineKernels.Count - 1) % FormatHelper.DefaultColors.Count];
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0000EBDC File Offset: 0x0000CDDC
		private void view_ColorChanged(object sender, RooflineColorChangedEventArgs e)
		{
			Dictionary<string, RooflineKernel> dictionary;
			if (this.m_rooflineKernels.TryGetValue((uint)e.CaptureId, out dictionary))
			{
				List<int> list = dictionary.Values.Select((RooflineKernel x) => x.ID).ToList<int>();
				this.m_view.UpdateTreeAndSeriesColor(e.CaptureId, list, e.Color);
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0000EC48 File Offset: 0x0000CE48
		private void view_KernelToggled(object sender, ColumnToggledEventArgs e)
		{
			this.m_view.SetSeriesVisibility((int)e.Id, e.Enabled);
			if (e.Enabled)
			{
				this.m_enabledKernels.Add((int)e.Id);
			}
			else
			{
				this.m_enabledKernels.Remove((int)e.Id);
			}
			this.InvalidateStats();
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0000EC9F File Offset: 0x0000CE9F
		private void view_ZoomIn(object sender, ViewBoundsEventArgs e)
		{
			this.m_view.SetViewBounds(1);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0000ECAD File Offset: 0x0000CEAD
		private void view_ZoomOut(object sender, ViewBoundsEventArgs e)
		{
			this.m_view.SetViewBounds(-1);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0000ECBB File Offset: 0x0000CEBB
		private void view_ResetViewBounds(object sender, EventArgs e)
		{
			this.m_view.SetViewBounds(0);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0000ECC9 File Offset: 0x0000CEC9
		private void view_DataViewBoundsChanged(object sender, SetDataViewBoundsEventArgs e)
		{
			this.m_view.UpdateTickFrequency(e.min, e.max);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0000ECE4 File Offset: 0x0000CEE4
		private void view_SelectAllToggled(object sender, SelectAllEventArgs e)
		{
			this.m_enabledKernels.Clear();
			if (e.All)
			{
				using (Dictionary<uint, Dictionary<string, RooflineKernel>>.Enumerator enumerator = this.m_rooflineKernels.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<uint, Dictionary<string, RooflineKernel>> keyValuePair = enumerator.Current;
						foreach (KeyValuePair<string, RooflineKernel> keyValuePair2 in keyValuePair.Value)
						{
							this.m_enabledKernels.Add(keyValuePair2.Value.ID);
						}
					}
					goto IL_00A2;
				}
			}
			if (e.ExceptId != 0)
			{
				this.m_enabledKernels.Add(e.ExceptId);
			}
			IL_00A2:
			this.m_view.SelectAllSeries(e.All, e.ExceptId);
			if (e.ExceptId != 0)
			{
				this.m_view.SetSeriesVisibility(e.ExceptId, true);
			}
			this.InvalidateStats();
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0000EDE8 File Offset: 0x0000CFE8
		private void view_TreeFiltered(object sender, TreeFilteredEventArgs e)
		{
			IEnumerable<int> enumerable = this.m_enabledKernels.Except(e.Filtered);
			if (enumerable.Count<int>() == 1)
			{
				List<object[]> list = new List<object[]>();
				RooflineKernel kernelByID = this.GetKernelByID(enumerable.First<int>());
				List<object[]> list2 = this.GenerateStats(kernelByID, null);
				this.m_view.InvalidateStats(list2, SdpApp.UIManager.GetWindowNameFromCaptureId((int)kernelByID.CaptureID), "", "Stats for Kernel: " + kernelByID.KernelName);
				this.AnalyzeKernel(kernelByID, ref list);
				this.m_view.InvalidateAnalysis(list, "Analysis for kernel: " + kernelByID.KernelName);
				return;
			}
			this.InvalidateStats();
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0000EE89 File Offset: 0x0000D089
		private void clientEvents_CaptureNameChanged(object sender, CaptureNameChangedArgs e)
		{
			this.m_view.UpdateCaptureName((int)e.CaptureId, e.CaptureName);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0000EEA2 File Offset: 0x0000D0A2
		private float TO_GIGA(float val)
		{
			return val / 1E+09f;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0000EEAB File Offset: 0x0000D0AB
		private float FROM_GIGA(float val)
		{
			return val * 1E+09f;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0000EEB4 File Offset: 0x0000D0B4
		private float TO_MEGA(float val)
		{
			return val / 1000000f;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0000EEBD File Offset: 0x0000D0BD
		private float FROM_MEGA(float val)
		{
			return val * 1000000f;
		}

		// Token: 0x04000692 RID: 1682
		public const int BOUNDS_SERIES_ID = -1;

		// Token: 0x04000693 RID: 1683
		private IRooflineView m_view;

		// Token: 0x04000694 RID: 1684
		private RooflineController.RooflineThresholds m_rooflineThresholds = new RooflineController.RooflineThresholds();

		// Token: 0x04000695 RID: 1685
		private static int m_nextKernelId = 1;

		// Token: 0x04000696 RID: 1686
		private static double m_graphMin = 0.001;

		// Token: 0x04000697 RID: 1687
		private static double m_graphMax = 1000000.0;

		// Token: 0x04000698 RID: 1688
		private const uint MIN_KERNEL_DURATION = 250U;

		// Token: 0x04000699 RID: 1689
		private readonly List<int> m_enabledKernels = new List<int>();

		// Token: 0x0400069A RID: 1690
		private readonly Dictionary<uint, Dictionary<string, RooflineKernel>> m_rooflineKernels = new Dictionary<uint, Dictionary<string, RooflineKernel>>();

		// Token: 0x0200037E RID: 894
		private class KernelDelta
		{
			// Token: 0x04000C31 RID: 3121
			public float Duration;

			// Token: 0x04000C32 RID: 3122
			public float AluUtilization;

			// Token: 0x04000C33 RID: 3123
			public float PercentL2;

			// Token: 0x04000C34 RID: 3124
			public float BytesRead;

			// Token: 0x04000C35 RID: 3125
			public float BytesWritten;

			// Token: 0x04000C36 RID: 3126
			public float Perf;

			// Token: 0x04000C37 RID: 3127
			public float OperationIntensity;

			// Token: 0x04000C38 RID: 3128
			public float Iterations;
		}

		// Token: 0x0200037F RID: 895
		public class RooflineThresholds
		{
			// Token: 0x04000C39 RID: 3129
			public float OperationIntensity;
		}
	}
}
