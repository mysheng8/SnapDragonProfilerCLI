using System;
using System.ComponentModel;

namespace Sdp
{
	// Token: 0x020000EE RID: 238
	public class OptionViewValueChangedEventArgs : EventArgs
	{
		// Token: 0x04000353 RID: 851
		public PropertyDescriptor property;

		// Token: 0x04000354 RID: 852
		public object component;

		// Token: 0x04000355 RID: 853
		public object oldValue;
	}
}
