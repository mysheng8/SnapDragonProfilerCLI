using System;

namespace DCAPToolsWrapper
{
	// Token: 0x02000035 RID: 53
	public struct ShaderStorageBlockData
	{
		// Token: 0x04000B89 RID: 2953
		public int m_index;

		// Token: 0x04000B8A RID: 2954
		public int m_length;

		// Token: 0x04000B8B RID: 2955
		public unsafe sbyte* m_pName;

		// Token: 0x04000B8C RID: 2956
		public int m_binding;

		// Token: 0x04000B8D RID: 2957
		public int m_dataSize;

		// Token: 0x04000B8E RID: 2958
		public int m_vertexShader;

		// Token: 0x04000B8F RID: 2959
		public int m_fragmentShader;

		// Token: 0x04000B90 RID: 2960
		public int m_computeShader;

		// Token: 0x04000B91 RID: 2961
		public int m_numVariables;

		// Token: 0x04000B92 RID: 2962
		public unsafe int* m_pVariableIndices;
	}
}
