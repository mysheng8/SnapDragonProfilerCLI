using System;
using System.ComponentModel;
using System.Globalization;

namespace Sdp
{
	// Token: 0x02000266 RID: 614
	internal class OptionsColorConverter : TypeConverter
	{
		// Token: 0x06000A60 RID: 2656 RVA: 0x0001D6B8 File Offset: 0x0001B8B8
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			string text = value as string;
			if (text != null)
			{
				try
				{
					OptionColor.Parse(text);
					return true;
				}
				catch
				{
					return false;
				}
			}
			OptionColor optionColor = value as OptionColor;
			return optionColor != null;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0001D6FC File Offset: 0x0001B8FC
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0001D714 File Offset: 0x0001B914
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				return OptionColor.Parse(text);
			}
			throw new InvalidCastException();
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0001D738 File Offset: 0x0001B938
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			OptionColor optionColor = value as OptionColor;
			if (destinationType == typeof(string))
			{
				return optionColor.ToString();
			}
			throw new InvalidCastException();
		}
	}
}
