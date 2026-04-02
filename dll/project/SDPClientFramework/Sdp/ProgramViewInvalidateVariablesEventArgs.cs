using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x020002D5 RID: 725
	public class ProgramViewInvalidateVariablesEventArgs : EventArgs
	{
		// Token: 0x040009F9 RID: 2553
		public List<ProgramViewVariable> PushConstants = new List<ProgramViewVariable>();

		// Token: 0x040009FA RID: 2554
		public List<ProgramViewVariable> Uniforms = new List<ProgramViewVariable>();
	}
}
