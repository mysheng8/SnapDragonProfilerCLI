using System;
using System.ComponentModel;

namespace Sdp
{
	// Token: 0x02000202 RID: 514
	public class PropertyGridDescriptorArg : EventArgs
	{
		// Token: 0x060007A2 RID: 1954 RVA: 0x00014DB9 File Offset: 0x00012FB9
		public PropertyGridDescriptorArg(PropertyDescriptor arg)
		{
			this.m_desc = arg;
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00014DC8 File Offset: 0x00012FC8
		public PropertyDescriptor Descriptor
		{
			get
			{
				return this.m_desc;
			}
		}

		// Token: 0x0400074F RID: 1871
		private PropertyDescriptor m_desc;
	}
}
