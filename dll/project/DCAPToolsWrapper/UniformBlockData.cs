using System;

namespace DCAPToolsWrapper
{
	// Token: 0x02000031 RID: 49
	public struct UniformBlockData
	{
		// Token: 0x04000B5B RID: 2907
		public int m_position;

		// Token: 0x04000B5C RID: 2908
		public int m_index;

		// Token: 0x04000B5D RID: 2909
		public int m_length;

		// Token: 0x04000B5E RID: 2910
		public unsafe sbyte* m_pName;

		// Token: 0x04000B5F RID: 2911
		public int m_binding;

		// Token: 0x04000B60 RID: 2912
		public int m_dataSize;

		// Token: 0x04000B61 RID: 2913
		public int m_vertexShader;

		// Token: 0x04000B62 RID: 2914
		public int m_fragmentShader;

		// Token: 0x04000B63 RID: 2915
		public int m_numUniforms;

		// Token: 0x04000B64 RID: 2916
		public unsafe int* m_pUniformIndices;

		// Token: 0x04000B65 RID: 2917
		public unsafe int* m_pUniformSizes;

		// Token: 0x04000B66 RID: 2918
		public unsafe int* m_pUniformTypes;

		// Token: 0x04000B67 RID: 2919
		public unsafe int* m_pUniformBlockIndexes;

		// Token: 0x04000B68 RID: 2920
		public unsafe int* m_pUniformOffsets;

		// Token: 0x04000B69 RID: 2921
		public unsafe int* m_pUniformArrayStrides;

		// Token: 0x04000B6A RID: 2922
		public unsafe int* m_pUniformMatrixStride;

		// Token: 0x04000B6B RID: 2923
		public unsafe int* m_pUniformIsRowMajor;
	}
}
