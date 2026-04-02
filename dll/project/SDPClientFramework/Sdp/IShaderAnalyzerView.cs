using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020002E2 RID: 738
	public interface IShaderAnalyzerView : IView
	{
		// Token: 0x06000ECB RID: 3787
		void Reset();

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06000ECC RID: 3788
		// (remove) Token: 0x06000ECD RID: 3789
		event EventHandler OverrideButtonClicked;

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06000ECE RID: 3790
		// (remove) Token: 0x06000ECF RID: 3791
		event EventHandler RevertButtonClicked;

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06000ED0 RID: 3792
		// (remove) Token: 0x06000ED1 RID: 3793
		event EventHandler CurrentShaderChanged;

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06000ED2 RID: 3794
		// (remove) Token: 0x06000ED3 RID: 3795
		event EventHandler CurrentShaderEdited;

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06000ED4 RID: 3796
		// (remove) Token: 0x06000ED5 RID: 3797
		event EventHandler<DrawcallComboChangedEventArgs> DrawcallComboChanged;

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06000ED6 RID: 3798
		// (remove) Token: 0x06000ED7 RID: 3799
		event EventHandler SaveButtonClicked;

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06000ED8 RID: 3800
		// (remove) Token: 0x06000ED9 RID: 3801
		event EventHandler SaveAllButtonClicked;

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06000EDA RID: 3802
		// (remove) Token: 0x06000EDB RID: 3803
		event EventHandler<ShaderSaveConfirmedEventArgs> SaveConfirmed;

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000EDC RID: 3804
		// (set) Token: 0x06000EDD RID: 3805
		ShaderStage CurrentShaderType { get; set; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000EDE RID: 3806
		// (set) Token: 0x06000EDF RID: 3807
		int CurrentShaderIndex { get; set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000EE0 RID: 3808
		string CurrentShaderText { get; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000EE1 RID: 3809
		// (set) Token: 0x06000EE2 RID: 3810
		bool OverrideButtonEnabled { get; set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000EE3 RID: 3811
		// (set) Token: 0x06000EE4 RID: 3812
		bool RevertButtonEnabled { get; set; }

		// Token: 0x06000EE5 RID: 3813
		void AddShaderEntry(uint index, ulong shaderIndex, ulong moduleID, ShaderStage stage);

		// Token: 0x06000EE6 RID: 3814
		void InvalidateShaderSource(bool suceeded, string shaderString, string statusString);

		// Token: 0x06000EE7 RID: 3815
		void InvalidateShaderCost(List<ShaderCostValue> shaderCyclePercentages);

		// Token: 0x06000EE8 RID: 3816
		void InvalidateShader(ShaderStage shaderType, string text, string resourceType, bool isEditable, Dictionary<uint, Tuple<uint, uint>> HitCyclePercentages, bool isDX12 = false);

		// Token: 0x06000EE9 RID: 3817
		void HandleNoShader(ProgramViewShaderError errorType, string resourceType);

		// Token: 0x06000EEA RID: 3818
		void ClearShaderStatsColumns();

		// Token: 0x06000EEB RID: 3819
		void ClearShaderLogColumns();

		// Token: 0x06000EEC RID: 3820
		void AddShaderStatsTextColumn(string title, int modelIndex);

		// Token: 0x06000EED RID: 3821
		void AddShaderLogTextColumn(string title, int modelIndex);

		// Token: 0x06000EEE RID: 3822
		void InvalidateShaderStatsModel(TreeModel model);

		// Token: 0x06000EEF RID: 3823
		void InvalidateShaderLogStatsModel(TreeModel model);

		// Token: 0x06000EF0 RID: 3824
		void ToggleDrawcallWidgets(bool show);

		// Token: 0x06000EF1 RID: 3825
		void ToggleShaderSourceTab(bool show);

		// Token: 0x06000EF2 RID: 3826
		void ToggleShaderCostTab(bool show);

		// Token: 0x06000EF3 RID: 3827
		void SetSelectedDrawcall(uint captureID, uint callID);

		// Token: 0x06000EF4 RID: 3828
		void AddDrawcall(uint captureID, uint callID, string displayID);

		// Token: 0x06000EF5 RID: 3829
		void UpdateDrawcallComboBatch(uint captureID, uint[] callIDs);

		// Token: 0x06000EF6 RID: 3830
		void OpenSaveDialog(string[] stringFilters);

		// Token: 0x06000EF7 RID: 3831
		void OpenSaveAllDialog(string[] stringFilters);

		// Token: 0x06000EF8 RID: 3832
		void ShowMessageDialog(string message, IconType type);

		// Token: 0x06000EF9 RID: 3833
		void SetStatusBox(StatusType statusType, string text, string toolTipText);

		// Token: 0x06000EFA RID: 3834
		void SetSelectedShaderCost();
	}
}
