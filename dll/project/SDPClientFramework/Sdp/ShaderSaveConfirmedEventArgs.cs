using System;

namespace Sdp
{
	// Token: 0x020002E6 RID: 742
	public class ShaderSaveConfirmedEventArgs : EventArgs
	{
		// Token: 0x04000A3F RID: 2623
		public string Filename;

		// Token: 0x04000A40 RID: 2624
		public string ShaderText;

		// Token: 0x04000A41 RID: 2625
		public string ShaderLanguage;

		// Token: 0x04000A42 RID: 2626
		public uint ShaderIndex;
	}
}
