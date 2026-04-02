using System;

namespace QGLPlugin
{
	// Token: 0x02000028 RID: 40
	internal interface IShaderProfile
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600005C RID: 92
		uint CaptureID { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005D RID: 93
		uint DrawID { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005E RID: 94
		uint Stage { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005F RID: 95
		uint InstructionIndex { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000060 RID: 96
		uint SourceMapType { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000061 RID: 97
		uint SourceLineNumber { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000062 RID: 98
		uint CounterType { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000063 RID: 99
		ulong Counter { get; }
	}
}
