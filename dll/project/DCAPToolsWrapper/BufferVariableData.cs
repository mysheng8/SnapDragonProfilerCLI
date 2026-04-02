using System;

namespace DCAPToolsWrapper
{
	// Token: 0x02000034 RID: 52
	public struct BufferVariableData
	{
		// Token: 0x04000B7A RID: 2938
		public int m_index;

		// Token: 0x04000B7B RID: 2939
		public int m_blockIndex;

		// Token: 0x04000B7C RID: 2940
		public int m_type;

		// Token: 0x04000B7D RID: 2941
		public int m_offset;

		// Token: 0x04000B7E RID: 2942
		public int m_arraySize;

		// Token: 0x04000B7F RID: 2943
		public int m_arrayStride;

		// Token: 0x04000B80 RID: 2944
		public int m_topLevelArraySize;

		// Token: 0x04000B81 RID: 2945
		public int m_topLevelArrayStride;

		// Token: 0x04000B82 RID: 2946
		public int m_isRowMajor;

		// Token: 0x04000B83 RID: 2947
		public int m_matrixStride;

		// Token: 0x04000B84 RID: 2948
		public int m_vertexShader;

		// Token: 0x04000B85 RID: 2949
		public int m_fragmentShader;

		// Token: 0x04000B86 RID: 2950
		public int m_computeShader;

		// Token: 0x04000B87 RID: 2951
		public int m_length;

		// Token: 0x04000B88 RID: 2952
		public unsafe sbyte* m_pName;
	}
}
