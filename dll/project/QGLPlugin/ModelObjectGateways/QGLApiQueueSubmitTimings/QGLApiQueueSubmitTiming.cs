using System;

namespace QGLPlugin.ModelObjectGateways.QGLApiQueueSubmitTimings
{
	// Token: 0x02000043 RID: 67
	internal class QGLApiQueueSubmitTiming
	{
		// Token: 0x06000159 RID: 345 RVA: 0x00012204 File Offset: 0x00010404
		public QGLApiQueueSubmitTiming(uint longID, uint captureID, uint instanceID, uint callID, uint index, ulong commandBuffer, ulong functionGPUStartTimeNS, ulong functionGPUEndTimeNS)
		{
			this.LongID = longID;
			this.CaptureID = captureID;
			this.InstanceID = instanceID;
			this.CallID = callID;
			this.Index = index;
			this.CommandBuffer = commandBuffer;
			this.FunctionGPUStartTimeNS = functionGPUStartTimeNS;
			this.FunctionGPUEndTimeNS = functionGPUEndTimeNS;
		}

		// Token: 0x040003F6 RID: 1014
		public readonly uint LongID;

		// Token: 0x040003F7 RID: 1015
		public readonly uint CaptureID;

		// Token: 0x040003F8 RID: 1016
		public readonly uint InstanceID;

		// Token: 0x040003F9 RID: 1017
		public readonly uint CallID;

		// Token: 0x040003FA RID: 1018
		public readonly uint Index;

		// Token: 0x040003FB RID: 1019
		public readonly ulong CommandBuffer;

		// Token: 0x040003FC RID: 1020
		public readonly ulong FunctionGPUStartTimeNS;

		// Token: 0x040003FD RID: 1021
		public readonly ulong FunctionGPUEndTimeNS;
	}
}
