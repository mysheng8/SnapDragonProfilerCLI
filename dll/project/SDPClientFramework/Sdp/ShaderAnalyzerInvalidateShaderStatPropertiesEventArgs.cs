using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x0200012B RID: 299
	public class ShaderAnalyzerInvalidateShaderStatPropertiesEventArgs : EventArgs
	{
		// Token: 0x04000447 RID: 1095
		public Dictionary<uint, string> Names = new Dictionary<uint, string>();

		// Token: 0x04000448 RID: 1096
		public Dictionary<uint, string> Descriptions = new Dictionary<uint, string>();
	}
}
