using System;

namespace Sdp
{
	// Token: 0x02000204 RID: 516
	public class SdpPropertyDescriptorCategory : SdpPropertyDescriptor<bool>
	{
		// Token: 0x060007A5 RID: 1957 RVA: 0x00014DD0 File Offset: 0x00012FD0
		public SdpPropertyDescriptorCategory(string name, string parent, uint parentPid)
			: base(name, typeof(string), true, parent, "", false)
		{
			this.Pid = parentPid;
		}

		// Token: 0x04000752 RID: 1874
		public uint Pid;
	}
}
