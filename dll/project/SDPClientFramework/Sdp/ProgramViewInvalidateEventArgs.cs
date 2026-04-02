using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020002D3 RID: 723
	public class ProgramViewInvalidateEventArgs : EventArgs
	{
		// Token: 0x040009EC RID: 2540
		public int SourceId;

		// Token: 0x040009ED RID: 2541
		public int ProgramId;

		// Token: 0x040009EE RID: 2542
		public bool ToolbarVisible;

		// Token: 0x040009EF RID: 2543
		public List<ProgramViewShader> Shaders = new List<ProgramViewShader>();

		// Token: 0x040009F0 RID: 2544
		public List<ProgramViewAttribute> Attributes = new List<ProgramViewAttribute>();

		// Token: 0x040009F1 RID: 2545
		public List<ProgramViewUniform> Uniforms = new List<ProgramViewUniform>();

		// Token: 0x040009F2 RID: 2546
		public List<ProgramViewUniformBlock> UniformBlocks = new List<ProgramViewUniformBlock>();

		// Token: 0x040009F3 RID: 2547
		public List<ProgramViewDataRange> PushConstantRanges = new List<ProgramViewDataRange>();

		// Token: 0x040009F4 RID: 2548
		public List<ProgramViewVariable> PushConstants = new List<ProgramViewVariable>();

		// Token: 0x040009F5 RID: 2549
		public List<ProgramViewDataRange> UniformRanges = new List<ProgramViewDataRange>();

		// Token: 0x040009F6 RID: 2550
		public List<ProgramViewVariable> VkUniforms = new List<ProgramViewVariable>();
	}
}
