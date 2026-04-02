using System;
using System.ComponentModel;
using Sdp.Helpers;

namespace Sdp
{
	// Token: 0x02000265 RID: 613
	[TypeConverter(typeof(OptionsColorConverter))]
	public class OptionColor
	{
		// Token: 0x06000A53 RID: 2643 RVA: 0x0001D4E4 File Offset: 0x0001B6E4
		public static OptionColor Parse(string optionValueString)
		{
			float[] array = new float[4];
			char[] array2 = new char[] { ',' };
			string[] array3 = optionValueString.Split(array2);
			if (array3.Length == 4)
			{
				for (int i = 0; i < 4; i++)
				{
					if (!FloatConverter.IsValid(array3[i]))
					{
						throw new FormatException();
					}
					array[i] = FloatConverter.Convert(array3[i]);
				}
				return new OptionColor(array);
			}
			throw new FormatException();
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0001D545 File Offset: 0x0001B745
		public OptionColor(float[] colors)
		{
			this.m_colors = colors;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0001D554 File Offset: 0x0001B754
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				TypeDescriptor.GetConverter(this.R).ConvertToInvariantString(this.R),
				",",
				TypeDescriptor.GetConverter(this.G).ConvertToInvariantString(this.G),
				",",
				TypeDescriptor.GetConverter(this.B).ConvertToInvariantString(this.B),
				",",
				TypeDescriptor.GetConverter(this.A).ConvertToInvariantString(this.A)
			});
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0001D610 File Offset: 0x0001B810
		public override bool Equals(object obj)
		{
			OptionColor optionColor = obj as OptionColor;
			return optionColor != null && (this.R == optionColor.R && this.G == optionColor.G && this.B == optionColor.B) && this.A == optionColor.A;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x000033D9 File Offset: 0x000015D9
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0001D663 File Offset: 0x0001B863
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x0001D66D File Offset: 0x0001B86D
		public float R
		{
			get
			{
				return this.m_colors[0];
			}
			set
			{
				this.m_colors[0] = value;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0001D678 File Offset: 0x0001B878
		// (set) Token: 0x06000A5B RID: 2651 RVA: 0x0001D682 File Offset: 0x0001B882
		public float G
		{
			get
			{
				return this.m_colors[1];
			}
			set
			{
				this.m_colors[1] = value;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0001D68D File Offset: 0x0001B88D
		// (set) Token: 0x06000A5D RID: 2653 RVA: 0x0001D697 File Offset: 0x0001B897
		public float B
		{
			get
			{
				return this.m_colors[2];
			}
			set
			{
				this.m_colors[2] = value;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0001D6A2 File Offset: 0x0001B8A2
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x0001D6AC File Offset: 0x0001B8AC
		public float A
		{
			get
			{
				return this.m_colors[3];
			}
			set
			{
				this.m_colors[3] = value;
			}
		}

		// Token: 0x04000868 RID: 2152
		private float[] m_colors;
	}
}
