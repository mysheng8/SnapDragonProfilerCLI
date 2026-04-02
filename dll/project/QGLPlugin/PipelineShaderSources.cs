using System;
using System.Collections.Generic;
using Sdp;

namespace QGLPlugin
{
	// Token: 0x0200003B RID: 59
	public struct PipelineShaderSources
	{
		// Token: 0x040003D3 RID: 979
		public Dictionary<ShaderStage, Dictionary<int, bool>> succeeded;

		// Token: 0x040003D4 RID: 980
		public Dictionary<ShaderStage, Dictionary<int, string>> shaders;
	}
}
