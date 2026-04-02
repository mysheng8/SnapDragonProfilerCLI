using System;
using System.Collections.Generic;

namespace QGLPlugin
{
	// Token: 0x02000030 RID: 48
	public class VkBoundInfo
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x000082B0 File Offset: 0x000064B0
		public VkBoundInfo()
		{
			this.BoundDescriptorSets = new Dictionary<ulong, DescSetBindings>();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000082DC File Offset: 0x000064DC
		public VkBoundInfo(VkBoundInfo rhs)
		{
			this.BoundPipeline = rhs.BoundPipeline;
			this.BoundDescriptorSets = new Dictionary<ulong, DescSetBindings>();
			foreach (KeyValuePair<ulong, DescSetBindings> keyValuePair in rhs.BoundDescriptorSets)
			{
				this.BoundDescriptorSets[keyValuePair.Key] = new DescSetBindings(keyValuePair.Value);
				this.DescSetIDs.Add(keyValuePair.Value.DescSetID);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00008394 File Offset: 0x00006594
		public bool ContainsDescriptorSet(ulong value)
		{
			return this.DescSetIDs.Contains(value);
		}

		// Token: 0x04000382 RID: 898
		public ulong BoundPipeline;

		// Token: 0x04000383 RID: 899
		public HashSet<ulong> ParentPipelines;

		// Token: 0x04000384 RID: 900
		public bool IsDrawcallParent;

		// Token: 0x04000385 RID: 901
		public Dictionary<ulong, DescSetBindings> BoundDescriptorSets = new Dictionary<ulong, DescSetBindings>();

		// Token: 0x04000386 RID: 902
		public HashSet<ulong> DescSetIDs = new HashSet<ulong>();
	}
}
