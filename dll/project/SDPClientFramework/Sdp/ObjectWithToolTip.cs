using System;

namespace Sdp
{
	// Token: 0x020002BC RID: 700
	public class ObjectWithToolTip
	{
		// Token: 0x06000E37 RID: 3639 RVA: 0x0002BC2B File Offset: 0x00029E2B
		public ObjectWithToolTip(object value, string tooltip)
		{
			this.Value = value;
			this.ToolTip = tooltip;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0002BC41 File Offset: 0x00029E41
		public override string ToString()
		{
			if (this.Value == null)
			{
				return "";
			}
			return this.Value.ToString();
		}

		// Token: 0x040009A0 RID: 2464
		public object Value;

		// Token: 0x040009A1 RID: 2465
		public string ToolTip;
	}
}
