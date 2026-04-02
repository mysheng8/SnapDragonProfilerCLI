using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020002CF RID: 719
	public interface IProgramView : IView, IStatusBox
	{
		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06000EAC RID: 3756
		// (remove) Token: 0x06000EAD RID: 3757
		event EventHandler<ShaderClickedEventArgs> ShaderClicked;

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06000EAE RID: 3758
		// (remove) Token: 0x06000EAF RID: 3759
		event EventHandler<FormatChangedArgs> FormatChanged;

		// Token: 0x06000EB0 RID: 3760
		void Clear(bool toolbarVisible);

		// Token: 0x06000EB1 RID: 3761
		void AddShader(ProgramViewShader shader);

		// Token: 0x06000EB2 RID: 3762
		void AddAttribute(int index, string typeName, string attributeName);

		// Token: 0x06000EB3 RID: 3763
		void AddUniform(int location, string name);

		// Token: 0x06000EB4 RID: 3764
		void AddUniformBlock(int blockIndex, string blockName);

		// Token: 0x06000EB5 RID: 3765
		void AddUniformToUniformBlock(int blockIndex, int location, string typeName, string name);

		// Token: 0x06000EB6 RID: 3766
		void InvalidateUniformValues(int location, string data);

		// Token: 0x06000EB7 RID: 3767
		void AddDataRanges(DataRangeType dataRangeType, List<ProgramViewDataRange> dataRanges);

		// Token: 0x06000EB8 RID: 3768
		void AddVariables(DataRangeType dataRangeType, List<ProgramViewVariable> variables, bool invalidate);

		// Token: 0x06000EB9 RID: 3769
		void ExpandAll();

		// Token: 0x06000EBA RID: 3770
		void RestoreSelection();
	}
}
