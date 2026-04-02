using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sdp
{
	// Token: 0x020002E7 RID: 743
	public class ShaderAnalyzerController : IViewController
	{
		// Token: 0x06000EFF RID: 3839 RVA: 0x0002DBDC File Offset: 0x0002BDDC
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

		// Token: 0x06000F00 RID: 3840 RVA: 0x00008AD1 File Offset: 0x00006CD1
		public bool LoadSettings(ViewDesc view_desc)
		{
			return true;
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0002DC0B File Offset: 0x0002BE0B
		public IView View
		{
			get
			{
				return this.m_view;
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0002DC14 File Offset: 0x0002BE14
		public ShaderAnalyzerController(IShaderAnalyzerView view)
		{
			this.m_view = view;
			this.m_view.OverrideButtonClicked += this.m_view_OverrideButtonClicked;
			this.m_view.CurrentShaderChanged += this.m_view_CurrentShaderChanged;
			this.m_view.CurrentShaderEdited += this.m_view_CurrentShaderEdited;
			this.m_view.RevertButtonClicked += this.m_view_RevertButtonClicked;
			this.m_view.DrawcallComboChanged += this.m_view_DrawcallComboChanged;
			this.m_view.SaveButtonClicked += this.m_view_SaveButtonClicked;
			this.m_view.SaveAllButtonClicked += this.m_view_SaveAllButtonClicked;
			this.m_view.SaveConfirmed += this.m_view_SaveConfirmed;
			ShaderAnalyzerEvents shaderAnalyzerEvents = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents.Invalidate = (EventHandler<ShaderAnalyzerProgramEventArgs>)Delegate.Combine(shaderAnalyzerEvents.Invalidate, new EventHandler<ShaderAnalyzerProgramEventArgs>(this.shaderAnalyzerEvents_Invalidate));
			ShaderAnalyzerEvents shaderAnalyzerEvents2 = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents2.InvalidateShaderStatProperties = (EventHandler<ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs>)Delegate.Combine(shaderAnalyzerEvents2.InvalidateShaderStatProperties, new EventHandler<ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs>(this.shaderAnalyzerEvents_InvalidateShaderStatProperties));
			ShaderAnalyzerEvents shaderAnalyzerEvents3 = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents3.InvalidateCurrentShaderStats = (EventHandler<ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs>)Delegate.Combine(shaderAnalyzerEvents3.InvalidateCurrentShaderStats, new EventHandler<ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs>(this.shaderAnalyzerEvents_InvalidateCurrentShaderStats));
			ShaderAnalyzerEvents shaderAnalyzerEvents4 = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents4.ClearShaderAnalyzerView = (EventHandler<EventArgs>)Delegate.Combine(shaderAnalyzerEvents4.ClearShaderAnalyzerView, new EventHandler<EventArgs>(this.shaderAnalyzerEvents_ClearShaderAnalyzerView));
			ShaderAnalyzerEvents shaderAnalyzerEvents5 = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents5.InvalidateCurrentShaderSource = (EventHandler<ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs>)Delegate.Combine(shaderAnalyzerEvents5.InvalidateCurrentShaderSource, new EventHandler<ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs>(this.shaderAnalyzerEvents_InvalidateCurrentShaderSource));
			ShaderAnalyzerEvents shaderAnalyzerEvents6 = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents6.SaveFailed = (EventHandler<ShaderAnalyzerSaveFailedEventArgs>)Delegate.Combine(shaderAnalyzerEvents6.SaveFailed, new EventHandler<ShaderAnalyzerSaveFailedEventArgs>(this.shaderAnalyzerEvents_SaveFailed));
			ShaderAnalyzerEvents shaderAnalyzerEvents7 = SdpApp.EventsManager.ShaderAnalyzerEvents;
			shaderAnalyzerEvents7.AddSnapshotAPIs = (EventHandler<ShaderAnalyzerAddSnapshotAPIsEventArgs>)Delegate.Combine(shaderAnalyzerEvents7.AddSnapshotAPIs, new EventHandler<ShaderAnalyzerAddSnapshotAPIsEventArgs>(this.shaderAnalyzerEvents_AddSnapshotAPIs));
			DataExplorerViewEvents dataExplorerViewEvents = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents.SelectRow = (EventHandler<DataExplorerViewSelectRowEventArgs>)Delegate.Combine(dataExplorerViewEvents.SelectRow, new EventHandler<DataExplorerViewSelectRowEventArgs>(this.dataExplorerViewEvents_SelectRow));
			DataExplorerViewEvents dataExplorerViewEvents2 = SdpApp.EventsManager.DataExplorerViewEvents;
			dataExplorerViewEvents2.RowSelected = (EventHandler<DataExplorerViewRowSelectedEventArgs>)Delegate.Combine(dataExplorerViewEvents2.RowSelected, new EventHandler<DataExplorerViewRowSelectedEventArgs>(this.dataExplorerViewEvents_RowSelected));
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0002DEB8 File Offset: 0x0002C0B8
		private void InvalidateShaderStats()
		{
			Dictionary<int, List<uint>> dictionary;
			List<uint> list;
			if (this.m_currentShaderStats.TryGetValue(this.m_view.CurrentShaderType, out dictionary) && dictionary.TryGetValue(this.m_view.CurrentShaderIndex, out list))
			{
				this.m_view.OverrideButtonEnabled = this.m_isEditable;
				this.m_view.ClearShaderStatsColumns();
				this.m_view.AddShaderStatsTextColumn("Description", 0);
				this.m_view.AddShaderStatsTextColumn("Current", 1);
				TreeModel treeModel = new TreeModel(new Type[]
				{
					typeof(string),
					typeof(uint),
					typeof(string)
				});
				if (list != null && this.m_shaderStatNames != null && this.m_shaderStatDescriptions != null && list.Count == this.m_shaderStatNames.Count)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i] != 4294967295U)
						{
							TreeNode treeNode = new TreeNode();
							treeNode.Values = new object[]
							{
								this.m_shaderStatNames[(uint)i],
								list[i],
								this.m_shaderStatDescriptions[(uint)i]
							};
							treeModel.Nodes.Add(treeNode);
						}
					}
				}
				else
				{
					Logger.Get().Write(LogLevel.LOG_ERROR, "ShaderAnalyzerController", "Couldn't find valid shader stat data for the selected shader");
				}
				this.m_view.InvalidateShaderStatsModel(treeModel);
				return;
			}
			this.m_view.OverrideButtonEnabled = false;
			this.m_view.ClearShaderStatsColumns();
			this.m_view.AddShaderStatsTextColumn("Shader", 0);
			this.m_view.AddShaderStatsTextColumn("Error", 1);
			TreeModel treeModel2 = new TreeModel(new Type[]
			{
				typeof(string),
				typeof(string)
			});
			foreach (KeyValuePair<ShaderStage, Dictionary<int, string>> keyValuePair in this.m_currentShaderErrors)
			{
				foreach (KeyValuePair<int, string> keyValuePair2 in keyValuePair.Value)
				{
					TreeNode treeNode2 = new TreeNode();
					treeNode2.Values = new object[]
					{
						keyValuePair.Key.ToString() + "_" + keyValuePair2.Key.ToString(),
						keyValuePair2.Value
					};
					treeModel2.Nodes.Add(treeNode2);
				}
			}
			this.m_view.InvalidateShaderStatsModel(treeModel2);
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0002E178 File Offset: 0x0002C378
		private void InvalidateShaderSource()
		{
			if (this.m_currentShaderSources.ContainsKey(this.m_view.CurrentShaderType) && this.m_currentShaderSources[this.m_view.CurrentShaderType].ContainsKey(this.m_view.CurrentShaderIndex))
			{
				string text = this.m_currentShaderSources[this.m_view.CurrentShaderType][this.m_view.CurrentShaderIndex];
				if (text != null)
				{
					bool flag = this.m_currentShaderSourcesSucceeded[this.m_view.CurrentShaderType][this.m_view.CurrentShaderIndex];
					this.m_view.InvalidateShaderSource(flag, text, "SPIRV-Cross conversion failed");
				}
			}
			if (this.m_view.CurrentShaderType == ShaderStage.Traversal)
			{
				this.m_view.InvalidateShaderSource(false, "", "Traversal Shader has no source");
			}
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0002E250 File Offset: 0x0002C450
		private void m_view_OverrideButtonClicked(object sender, EventArgs e)
		{
			ShaderAnalyzerProgramEventArgs shaderAnalyzerProgramEventArgs = new ShaderAnalyzerProgramEventArgs();
			shaderAnalyzerProgramEventArgs.CategoryID = this.m_currentCategoryID;
			shaderAnalyzerProgramEventArgs.ResourceID = this.m_currentResourceID;
			shaderAnalyzerProgramEventArgs.ShaderObjectGroup = this.m_currentShaderObjectGroup;
			shaderAnalyzerProgramEventArgs.Source = this.m_currentSource;
			SdpApp.EventsManager.Raise<ShaderAnalyzerProgramEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.OverrideShader, this, shaderAnalyzerProgramEventArgs);
			this.m_view.RevertButtonEnabled = this.m_isEditable;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0002E2C0 File Offset: 0x0002C4C0
		private void m_view_RevertButtonClicked(object sender, EventArgs e)
		{
			ShaderAnalyzerProgramEventArgs shaderAnalyzerProgramEventArgs = new ShaderAnalyzerProgramEventArgs();
			shaderAnalyzerProgramEventArgs.CategoryID = this.m_currentCategoryID;
			shaderAnalyzerProgramEventArgs.ResourceID = this.m_currentResourceID;
			shaderAnalyzerProgramEventArgs.ShaderObjectGroup = this.m_currentShaderObjectGroup;
			shaderAnalyzerProgramEventArgs.Source = this.m_currentSource;
			SdpApp.EventsManager.Raise<ShaderAnalyzerProgramEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.RevertShaderGroup, this, shaderAnalyzerProgramEventArgs);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0002E320 File Offset: 0x0002C520
		private void m_view_DrawcallComboChanged(object sender, DrawcallComboChangedEventArgs e)
		{
			DataExplorerViewSelectRowEventArgs dataExplorerViewSelectRowEventArgs = new DataExplorerViewSelectRowEventArgs();
			dataExplorerViewSelectRowEventArgs.SourceID = this.m_currentSource;
			dataExplorerViewSelectRowEventArgs.CaptureID = (int)e.CaptureID;
			dataExplorerViewSelectRowEventArgs.RowElement = e.DrawID;
			dataExplorerViewSelectRowEventArgs.SearchColumn = 7;
			SdpApp.EventsManager.Raise<DataExplorerViewSelectRowEventArgs>(SdpApp.EventsManager.DataExplorerViewEvents.SelectRow, this, dataExplorerViewSelectRowEventArgs);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0002E380 File Offset: 0x0002C580
		private void m_view_CurrentShaderEdited(object sender, EventArgs e)
		{
			ShaderAnalyzerProgramEventArgs shaderAnalyzerProgramEventArgs = new ShaderAnalyzerProgramEventArgs();
			if (this.m_currentShaderObjectGroup.ContainShaderStage(this.m_view.CurrentShaderType))
			{
				foreach (ShaderObject shaderObject in this.m_currentShaderObjectGroup.GetShaders(this.m_view.CurrentShaderType))
				{
					if ((ulong)shaderObject.ShaderIndex == (ulong)((long)this.m_view.CurrentShaderIndex))
					{
						shaderObject.Source = this.m_view.CurrentShaderText;
						break;
					}
				}
			}
			shaderAnalyzerProgramEventArgs.ShaderObjectGroup = this.m_currentShaderObjectGroup;
			shaderAnalyzerProgramEventArgs.Source = this.m_currentSource;
			SdpApp.EventsManager.Raise<ShaderAnalyzerProgramEventArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.ShaderEdited, this, shaderAnalyzerProgramEventArgs);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0002E458 File Offset: 0x0002C658
		private void m_view_CurrentShaderChanged(object sender, EventArgs e)
		{
			this.InvalidateShaderStats();
			this.InvalidateShaderSource();
			this.m_view.SetSelectedShaderCost();
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0002E474 File Offset: 0x0002C674
		private void m_view_SaveButtonClicked(object sender, EventArgs e)
		{
			string text = ".glsl";
			if (this.m_currentSource == 353)
			{
				string text2 = ".hlsl";
				string text3 = ".spirv";
				this.m_view.OpenSaveDialog(new string[] { text, text2, text3 });
				return;
			}
			this.m_view.OpenSaveDialog(new string[] { text });
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0002E4D4 File Offset: 0x0002C6D4
		private void m_view_SaveAllButtonClicked(object sender, EventArgs e)
		{
			string text = ".glsl";
			if (this.m_currentSource == 353)
			{
				string text2 = ".hlsl";
				string text3 = ".spirv";
				this.m_view.OpenSaveAllDialog(new string[] { text, text2, text3 });
				return;
			}
			this.m_view.OpenSaveAllDialog(new string[] { text });
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0002E534 File Offset: 0x0002C734
		private void m_view_SaveConfirmed(object sender, ShaderSaveConfirmedEventArgs e)
		{
			if (this.m_currentSource != 353)
			{
				File.WriteAllText(e.Filename, e.ShaderText);
				return;
			}
			if (e.ShaderLanguage == "spirv")
			{
				File.WriteAllText(e.Filename, e.ShaderText);
				return;
			}
			ShaderAnalyzerExportShaderArgs shaderAnalyzerExportShaderArgs = new ShaderAnalyzerExportShaderArgs();
			shaderAnalyzerExportShaderArgs.Filename = e.Filename;
			shaderAnalyzerExportShaderArgs.ShaderLanguage = e.ShaderLanguage;
			shaderAnalyzerExportShaderArgs.ShaderIndex = e.ShaderIndex;
			SdpApp.EventsManager.Raise<ShaderAnalyzerExportShaderArgs>(SdpApp.EventsManager.ShaderAnalyzerEvents.ExportShader, this, shaderAnalyzerExportShaderArgs);
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0002E5CC File Offset: 0x0002C7CC
		private void shaderAnalyzerEvents_Invalidate(object sender, ShaderAnalyzerProgramEventArgs e)
		{
			ShaderStage currentShaderType = this.m_view.CurrentShaderType;
			this.m_currentCategoryID = e.CategoryID;
			this.m_currentResourceID = e.ResourceID;
			this.m_currentShaderObjectGroup = e.ShaderObjectGroup;
			this.m_currentSource = e.Source;
			this.m_isEditable = e.IsEditable;
			this.m_view.Reset();
			this.m_view.OverrideButtonEnabled = e.EnableOverride;
			this.m_view.RevertButtonEnabled = e.IsModified;
			if (this.m_currentShaderObjectGroup.Count() == 0)
			{
				if (this.m_currentShaderObjectGroup.IsBinary)
				{
					this.m_view.HandleNoShader(ProgramViewShaderError.Binary, this.m_currentShaderObjectGroup.ResourceType);
				}
				else if (e.IsWaiting)
				{
					this.m_view.HandleNoShader(ProgramViewShaderError.Waiting, this.m_currentShaderObjectGroup.ResourceType);
				}
				else
				{
					this.m_view.HandleNoShader(ProgramViewShaderError.Incomplete, this.m_currentShaderObjectGroup.ResourceType);
				}
			}
			uint num = 0U;
			ulong num2 = 0UL;
			List<ShaderCostValue> list = new List<ShaderCostValue>();
			foreach (ShaderStage shaderStage in e.ShaderObjectGroup.GetShaderStages())
			{
				foreach (ShaderObject shaderObject in e.ShaderObjectGroup.GetShaders(shaderStage))
				{
					this.m_view.AddShaderEntry(num, (ulong)shaderObject.ShaderIndex, shaderObject.ShaderModuleID, shaderStage);
					this.m_view.InvalidateShader(shaderStage, shaderObject.Source, this.m_currentShaderObjectGroup.ResourceType, this.m_isEditable, shaderObject.HitCyclePercentages, e.IsDX12Shader);
					if (shaderObject.ShaderCycleCount != 0UL)
					{
						ShaderCostValue shaderCostValue = new ShaderCostValue(num, shaderStage, shaderObject.ShaderIndex, shaderObject.ShaderCycleCount);
						list.Add(shaderCostValue);
						num2 += shaderObject.ShaderCycleCount;
					}
					num += 1U;
				}
			}
			List<ShaderCostValue> list2 = new List<ShaderCostValue>();
			foreach (ShaderCostValue shaderCostValue2 in list)
			{
				if (num2 > 0UL)
				{
					ulong num3 = (ulong)Math.Min(Math.Max(0U, (uint)(shaderCostValue2.CycleValue * 100UL / num2)), 100U);
					ShaderCostValue shaderCostValue3 = new ShaderCostValue(shaderCostValue2.Index, shaderCostValue2.Stage, shaderCostValue2.ShaderIndex, num3);
					list2.Add(shaderCostValue3);
				}
			}
			if (list2.Count > 1)
			{
				this.m_view.InvalidateShaderCost(list2);
				this.m_view.SetSelectedShaderCost();
				this.m_view.ToggleShaderCostTab(true);
			}
			else
			{
				this.m_view.ToggleShaderCostTab(false);
			}
			bool flag = this.m_currentSource == 353;
			this.m_view.ToggleDrawcallWidgets(flag);
			this.m_view.ToggleShaderSourceTab(flag);
			this.m_view.CurrentShaderType = currentShaderType;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0002E8E0 File Offset: 0x0002CAE0
		private void shaderAnalyzerEvents_InvalidateShaderStatProperties(object sender, ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs e)
		{
			this.m_shaderStatNames = e.Names;
			this.m_shaderStatDescriptions = e.Descriptions;
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0002E8FA File Offset: 0x0002CAFA
		private void shaderAnalyzerEvents_InvalidateCurrentShaderStats(object sender, ShaderAnalyzerInvalidateCurrentShaderStatsEventArgs e)
		{
			this.m_currentShaderStats = e.Stats;
			this.m_currentShaderErrors = e.Errors;
			this.InvalidateShaderStats();
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0002E91A File Offset: 0x0002CB1A
		private void shaderAnalyzerEvents_InvalidateCurrentShaderSource(object sender, ShaderAnalyzerInvalidateCurrentShaderSourceEventArgs e)
		{
			this.m_currentShaderSources = e.ShaderSources;
			this.m_currentShaderSourcesSucceeded = e.Succeeded;
			this.InvalidateShaderSource();
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0002E93A File Offset: 0x0002CB3A
		private void shaderAnalyzerEvents_SaveFailed(object sender, ShaderAnalyzerSaveFailedEventArgs e)
		{
			this.m_view.ShowMessageDialog(e.ErrorString, IconType.Error);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0002E950 File Offset: 0x0002CB50
		private void shaderAnalyzerEvents_AddSnapshotAPIs(object sender, ShaderAnalyzerAddSnapshotAPIsEventArgs e)
		{
			if (e.Source != 353)
			{
				return;
			}
			this.ProcessTreeModel(e.Model, (uint)e.CaptureID);
			uint[] array = new uint[] { this.m_selectedDrawcall.Item2 };
			this.m_view.UpdateDrawcallComboBatch(this.m_selectedDrawcall.Item1, array);
			this.m_view.SetSelectedDrawcall(this.m_selectedDrawcall.Item1, this.m_selectedDrawcall.Item2);
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0002E9CA File Offset: 0x0002CBCA
		private void shaderAnalyzerEvents_ClearShaderAnalyzerView(object sender, EventArgs e)
		{
			this.m_view.ClearShaderStatsColumns();
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0002E9D8 File Offset: 0x0002CBD8
		private void dataExplorerViewEvents_SelectRow(object sender, DataExplorerViewSelectRowEventArgs e)
		{
			int sourceID = e.SourceID;
			int captureID = e.CaptureID;
			if (sourceID != 353)
			{
				return;
			}
			if (e.HighlightElements == null || e.HighlightElements.Length == 0)
			{
				return;
			}
			this.m_view.UpdateDrawcallComboBatch((uint)captureID, e.HighlightElements.Cast<uint>().ToArray<uint>());
			if (this.m_selectedDrawcall.Item1 == 4294967295U && this.m_selectedDrawcall.Item2 == 4294967295U)
			{
				this.m_view.SetSelectedDrawcall((uint)captureID, (uint)e.HighlightElements.Last<object>());
				return;
			}
			this.m_view.SetSelectedDrawcall(this.m_selectedDrawcall.Item1, this.m_selectedDrawcall.Item2);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0002EA84 File Offset: 0x0002CC84
		private void dataExplorerViewEvents_RowSelected(object sender, DataExplorerViewRowSelectedEventArgs e)
		{
			if (e.SourceID != 353)
			{
				return;
			}
			uint captureID = (uint)e.CaptureID;
			uint num = (uint)e.SelectedRow[7];
			this.m_selectedDrawcall = new Tuple<uint, uint>(captureID, num);
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0002EAC4 File Offset: 0x0002CCC4
		private void ProcessTreeModel(TreeModel model, uint captureID)
		{
			foreach (TreeNode treeNode in model.Nodes)
			{
				this.ProcessTreeNode(treeNode, captureID);
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0002EB18 File Offset: 0x0002CD18
		private void ProcessTreeNode(TreeNode node, uint captureID)
		{
			object obj = node.Values[7];
			string text = node.Values[1] as string;
			if (!string.IsNullOrEmpty(text) && obj is uint)
			{
				uint num = (uint)obj;
				this.m_view.AddDrawcall(captureID, num, text);
				this.m_selectedDrawcall = new Tuple<uint, uint>(captureID, num);
			}
			foreach (TreeNode treeNode in node.Children)
			{
				this.ProcessTreeNode(treeNode, captureID);
			}
		}

		// Token: 0x04000A43 RID: 2627
		private IShaderAnalyzerView m_view;

		// Token: 0x04000A44 RID: 2628
		private Dictionary<uint, string> m_shaderStatNames = new Dictionary<uint, string>();

		// Token: 0x04000A45 RID: 2629
		private Dictionary<uint, string> m_shaderStatDescriptions = new Dictionary<uint, string>();

		// Token: 0x04000A46 RID: 2630
		private Dictionary<ShaderStage, Dictionary<int, List<uint>>> m_currentShaderStats = new Dictionary<ShaderStage, Dictionary<int, List<uint>>>();

		// Token: 0x04000A47 RID: 2631
		private Dictionary<ShaderStage, Dictionary<int, string>> m_currentShaderErrors = new Dictionary<ShaderStage, Dictionary<int, string>>();

		// Token: 0x04000A48 RID: 2632
		private Dictionary<ShaderStage, Dictionary<int, bool>> m_currentShaderSourcesSucceeded = new Dictionary<ShaderStage, Dictionary<int, bool>>();

		// Token: 0x04000A49 RID: 2633
		private Dictionary<ShaderStage, Dictionary<int, string>> m_currentShaderSources = new Dictionary<ShaderStage, Dictionary<int, string>>();

		// Token: 0x04000A4A RID: 2634
		private Tuple<uint, uint> m_selectedDrawcall = new Tuple<uint, uint>(uint.MaxValue, uint.MaxValue);

		// Token: 0x04000A4B RID: 2635
		private ShaderGroup m_currentShaderObjectGroup;

		// Token: 0x04000A4C RID: 2636
		private int m_currentSource;

		// Token: 0x04000A4D RID: 2637
		private int m_currentCategoryID;

		// Token: 0x04000A4E RID: 2638
		private int m_currentResourceID;

		// Token: 0x04000A4F RID: 2639
		private bool m_isEditable;

		// Token: 0x04000A50 RID: 2640
		private const uint UNSELECTED = 4294967295U;

		// Token: 0x04000A51 RID: 2641
		private const int VK_SNAPSHOT_SOURCE = 353;

		// Token: 0x04000A52 RID: 2642
		private const int SNAPSHOT_COLUMN_DISPLAY_ID = 1;

		// Token: 0x04000A53 RID: 2643
		private const int SNAPSHOT_COLUMN_DRAW_CALL_API = 7;
	}
}
