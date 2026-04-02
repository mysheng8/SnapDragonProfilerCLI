using System;

namespace DCAPToolsWrapper
{
	// Token: 0x02000033 RID: 51
	public struct AtomicCounterBufferData
	{
		// Token: 0x04000B72 RID: 2930
		public int m_index;

		// Token: 0x04000B73 RID: 2931
		public int m_binding;

		// Token: 0x04000B74 RID: 2932
		public int m_dataSize;

		// Token: 0x04000B75 RID: 2933
		public int m_vertexShader;

		// Token: 0x04000B76 RID: 2934
		public int m_fragmentShader;

		// Token: 0x04000B77 RID: 2935
		public int m_computeShader;

		// Token: 0x04000B78 RID: 2936
		public int m_numCounters;

		// Token: 0x04000B79 RID: 2937
		public unsafe int* m_pCounterIndices;
	}
}
