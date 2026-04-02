using System;
using System.Collections.Generic;

namespace Sdp
{
	// Token: 0x02000124 RID: 292
	public class BinningInfoAddedEventArgs : EventArgs
	{
		// Token: 0x04000416 RID: 1046
		public uint CaptureId;

		// Token: 0x04000417 RID: 1047
		public List<BinConfigChange> BinConfigMapping;

		// Token: 0x04000418 RID: 1048
		public Dictionary<uint, BinConfiguration> BinConfigs;
	}
}
