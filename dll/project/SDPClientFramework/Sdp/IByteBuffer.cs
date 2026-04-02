using System;

namespace Sdp
{
	// Token: 0x0200019A RID: 410
	public interface IByteBuffer
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004E5 RID: 1253
		uint CaptureID { get; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004E6 RID: 1254
		ulong ResourceID { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004E7 RID: 1255
		uint SequenceID { get; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004E8 RID: 1256
		uint Offset { get; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004E9 RID: 1257
		BinaryDataPair BDP { get; }
	}
}
