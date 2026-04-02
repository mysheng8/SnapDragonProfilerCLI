using System;
using System.Collections.Generic;

namespace QGLPlugin
{
	// Token: 0x0200002F RID: 47
	public class DescSetBindings
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00008209 File Offset: 0x00006409
		public DescSetBindings(ulong id)
		{
			this.DescSetID = id;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00008224 File Offset: 0x00006424
		public DescSetBindings(DescSetBindings rhs)
		{
			if (rhs == null)
			{
				return;
			}
			this.DescSetID = rhs.DescSetID;
			foreach (KeyValuePair<ulong, DescSetBindings.DescBindings> keyValuePair in rhs.Bindings)
			{
				this.Bindings[keyValuePair.Key] = new DescSetBindings.DescBindings(keyValuePair.Value);
			}
		}

		// Token: 0x04000380 RID: 896
		public ulong DescSetID;

		// Token: 0x04000381 RID: 897
		public Dictionary<ulong, DescSetBindings.DescBindings> Bindings = new Dictionary<ulong, DescSetBindings.DescBindings>();

		// Token: 0x0200005C RID: 92
		public struct DescBindings
		{
			// Token: 0x060001AD RID: 429 RVA: 0x00013F78 File Offset: 0x00012178
			public DescBindings(DescSetBindings.DescBindings rhs)
			{
				this.captureID = rhs.captureID;
				this.descriptorSetID = rhs.descriptorSetID;
				this.apiID = rhs.apiID;
				this.slotNum = rhs.slotNum;
				this.samplerID = rhs.samplerID;
				this.imageViewID = rhs.imageViewID;
				this.imageLayout = rhs.imageLayout;
				this.bufferID = rhs.bufferID;
				this.offset = rhs.offset;
				this.range = rhs.range;
				this.texBufferview = rhs.texBufferview;
				this.accelStructID = rhs.accelStructID;
				this.tensorID = rhs.tensorID;
				this.tensorViewID = rhs.tensorViewID;
			}

			// Token: 0x060001AE RID: 430 RVA: 0x00014030 File Offset: 0x00012230
			public DescBindings(ulong _samplerID, ulong _imageViewID, uint _imageLayout, ulong _bufferID, ulong _offset, ulong _range, ulong _texBufferview, ulong _accelStructID, ulong _tensorID, ulong _tensorViewID)
			{
				this.captureID = 0U;
				this.descriptorSetID = 0UL;
				this.apiID = 0U;
				this.slotNum = 0U;
				this.samplerID = _samplerID;
				this.imageViewID = _imageViewID;
				this.imageLayout = _imageLayout;
				this.bufferID = _bufferID;
				this.offset = _offset;
				this.range = _range;
				this.texBufferview = _texBufferview;
				this.accelStructID = _accelStructID;
				this.tensorID = _tensorID;
				this.tensorViewID = _tensorViewID;
			}

			// Token: 0x04000474 RID: 1140
			public uint captureID;

			// Token: 0x04000475 RID: 1141
			public ulong descriptorSetID;

			// Token: 0x04000476 RID: 1142
			public uint apiID;

			// Token: 0x04000477 RID: 1143
			public uint slotNum;

			// Token: 0x04000478 RID: 1144
			public ulong samplerID;

			// Token: 0x04000479 RID: 1145
			public ulong imageViewID;

			// Token: 0x0400047A RID: 1146
			public uint imageLayout;

			// Token: 0x0400047B RID: 1147
			public ulong texBufferview;

			// Token: 0x0400047C RID: 1148
			public ulong bufferID;

			// Token: 0x0400047D RID: 1149
			public ulong offset;

			// Token: 0x0400047E RID: 1150
			public ulong range;

			// Token: 0x0400047F RID: 1151
			public ulong accelStructID;

			// Token: 0x04000480 RID: 1152
			public ulong tensorID;

			// Token: 0x04000481 RID: 1153
			public ulong tensorViewID;
		}
	}
}
