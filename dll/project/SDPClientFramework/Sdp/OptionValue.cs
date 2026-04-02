using System;
using System.Runtime.InteropServices;

namespace Sdp
{
	// Token: 0x0200016A RID: 362
	[StructLayout(LayoutKind.Explicit)]
	public struct OptionValue
	{
		// Token: 0x0400053B RID: 1339
		[FieldOffset(0)]
		public uint Uint;

		// Token: 0x0400053C RID: 1340
		[FieldOffset(0)]
		public float Float;
	}
}
