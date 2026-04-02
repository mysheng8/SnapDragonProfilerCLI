using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020002DD RID: 733
	public class ProgramViewUniformBlock
	{
		// Token: 0x04000A25 RID: 2597
		public uint Index;

		// Token: 0x04000A26 RID: 2598
		public int UniformBlockBinding;

		// Token: 0x04000A27 RID: 2599
		public int UniformBufferOffsetAlignment;

		// Token: 0x04000A28 RID: 2600
		public string UniformBlockName;

		// Token: 0x04000A29 RID: 2601
		public readonly List<ProgramViewUniform> Uniforms = new List<ProgramViewUniform>();
	}
}
