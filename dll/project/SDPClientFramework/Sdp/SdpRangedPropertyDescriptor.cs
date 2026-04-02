using System;

namespace Sdp
{
	// Token: 0x02000200 RID: 512
	public class SdpRangedPropertyDescriptor<T> : SdpPropertyDescriptor<T> where T : IComparable<T>
	{
		// Token: 0x06000793 RID: 1939 RVA: 0x00014BB9 File Offset: 0x00012DB9
		public SdpRangedPropertyDescriptor(string name, Type componentType, T value, string category, string description, bool isReadOnly, T lowLimit, T highLimit)
			: base(name, componentType, value, category, description, isReadOnly)
		{
			this.m_highRange = highLimit;
			this.m_lowRange = lowLimit;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00014BDC File Offset: 0x00012DDC
		public override void SetValue(object component, object value)
		{
			PropertyGridValueSetEventArgs propertyGridValueSetEventArgs = new PropertyGridValueSetEventArgs();
			propertyGridValueSetEventArgs.component = component;
			propertyGridValueSetEventArgs.oldValue = this.m_value;
			T t = (T)((object)value);
			if (t.CompareTo(this.m_lowRange) < 0)
			{
				this.m_value = this.m_lowRange;
				this.OnValueChanged(this, propertyGridValueSetEventArgs);
				return;
			}
			t = (T)((object)value);
			if (t.CompareTo(this.m_highRange) > 0)
			{
				this.m_value = this.m_highRange;
				this.OnValueChanged(this, propertyGridValueSetEventArgs);
				return;
			}
			this.m_value = (T)((object)value);
			this.OnValueChanged(this, propertyGridValueSetEventArgs);
		}

		// Token: 0x04000746 RID: 1862
		private T m_highRange;

		// Token: 0x04000747 RID: 1863
		private T m_lowRange;
	}
}
