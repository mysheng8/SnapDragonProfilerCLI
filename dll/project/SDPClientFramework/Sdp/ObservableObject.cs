using System;

namespace Sdp
{
	// Token: 0x02000178 RID: 376
	public class ObservableObject<T>
	{
		// Token: 0x0600044D RID: 1101 RVA: 0x0000204B File Offset: 0x0000024B
		public ObservableObject()
		{
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000AA0A File Offset: 0x00008C0A
		public ObservableObject(T initvalue)
		{
			this.m_value = initvalue;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000AA19 File Offset: 0x00008C19
		public static implicit operator T(ObservableObject<T> observable)
		{
			return observable.Value;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000AA21 File Offset: 0x00008C21
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x0000AA2C File Offset: 0x00008C2C
		public T Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				if (!this.m_value.Equals(value))
				{
					this.m_value = value;
					EventHandler<ObservableObject<T>.ValueChangedArgs> onValueChanged = this.OnValueChanged;
					if (onValueChanged == null)
					{
						return;
					}
					onValueChanged(this, new ObservableObject<T>.ValueChangedArgs
					{
						Value = this.m_value
					});
				}
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000452 RID: 1106 RVA: 0x0000AA7C File Offset: 0x00008C7C
		// (remove) Token: 0x06000453 RID: 1107 RVA: 0x0000AAB4 File Offset: 0x00008CB4
		public event EventHandler<ObservableObject<T>.ValueChangedArgs> OnValueChanged;

		// Token: 0x04000569 RID: 1385
		private T m_value;

		// Token: 0x0200036F RID: 879
		public class ValueChangedArgs : EventArgs
		{
			// Token: 0x04000C0C RID: 3084
			public T Value;
		}
	}
}
