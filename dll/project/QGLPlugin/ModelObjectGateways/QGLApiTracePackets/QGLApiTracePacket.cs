using System;

namespace QGLPlugin.ModelObjectGateways.QGLApiTracePackets
{
	// Token: 0x02000040 RID: 64
	internal class QGLApiTracePacket
	{
		// Token: 0x0600014B RID: 331 RVA: 0x000120D4 File Offset: 0x000102D4
		public QGLApiTracePacket(uint captureID, uint instanceID, uint callID, uint threadID, ulong cPUStartTime, ulong cPUEndTime, string functionName, string functionParams, string functionReturnValue)
		{
			this.CaptureID = captureID;
			this.InstanceID = instanceID;
			this.CallID = callID;
			this.ThreadID = threadID;
			this.CPUStartTime = cPUStartTime;
			this.CPUEndTime = cPUEndTime;
			this.FunctionName = functionName;
			this.FunctionParams = functionParams;
			this.FunctionReturnValue = functionReturnValue;
		}

		// Token: 0x040003ED RID: 1005
		public readonly uint CaptureID;

		// Token: 0x040003EE RID: 1006
		public readonly uint InstanceID;

		// Token: 0x040003EF RID: 1007
		public readonly uint CallID;

		// Token: 0x040003F0 RID: 1008
		public readonly uint ThreadID;

		// Token: 0x040003F1 RID: 1009
		public readonly ulong CPUStartTime;

		// Token: 0x040003F2 RID: 1010
		public readonly ulong CPUEndTime;

		// Token: 0x040003F3 RID: 1011
		public readonly string FunctionName;

		// Token: 0x040003F4 RID: 1012
		public readonly string FunctionParams;

		// Token: 0x040003F5 RID: 1013
		public readonly string FunctionReturnValue;
	}
}
