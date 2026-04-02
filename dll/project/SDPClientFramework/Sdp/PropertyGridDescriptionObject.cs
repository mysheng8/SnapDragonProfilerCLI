using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sdp
{
	// Token: 0x020001FF RID: 511
	public class PropertyGridDescriptionObject : CustomTypeDescriptor
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x00014B7C File Offset: 0x00012D7C
		public PropertyGridDescriptionObject()
		{
			this.properties = null;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00014B8B File Offset: 0x00012D8B
		public PropertyGridDescriptionObject(List<PropertyDescriptor> list)
		{
			this.AddPropertyGridDescriptors(list);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00014B9A File Offset: 0x00012D9A
		public void AddPropertyGridDescriptors(List<PropertyDescriptor> list)
		{
			if (list == null)
			{
				return;
			}
			this.properties = new PropertyDescriptorCollection(list.ToArray());
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00014BB1 File Offset: 0x00012DB1
		public override PropertyDescriptorCollection GetProperties()
		{
			return this.properties;
		}

		// Token: 0x04000745 RID: 1861
		private PropertyDescriptorCollection properties;
	}
}
