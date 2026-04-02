using System;
using System.Collections.Generic;
using Cairo;

namespace Sdp.Helpers
{
	// Token: 0x02000307 RID: 775
	public class ColorCycle
	{
		// Token: 0x06001005 RID: 4101 RVA: 0x00032595 File Offset: 0x00030795
		public ColorCycle()
		{
			this.m_colorIndex = 0;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x000325A4 File Offset: 0x000307A4
		public Color GetNextColor()
		{
			Color color = ColorCycle.CycleColors[this.m_colorIndex];
			this.m_colorIndex = (this.m_colorIndex + 1) % ColorCycle.CycleColors.Count;
			return color;
		}

		// Token: 0x04000ADC RID: 2780
		private int m_colorIndex;

		// Token: 0x04000ADD RID: 2781
		private static List<Color> CycleColors = new List<Color>
		{
			FormatHelper.HsvToRgb(0.0, 1.0, 0.8),
			FormatHelper.HsvToRgb(40.0, 1.0, 0.8),
			FormatHelper.HsvToRgb(80.0, 1.0, 0.8),
			FormatHelper.HsvToRgb(200.0, 1.0, 0.9),
			FormatHelper.HsvToRgb(260.0, 1.0, 1.0),
			FormatHelper.HsvToRgb(300.0, 0.7, 0.6),
			FormatHelper.HsvToRgb(40.0, 0.7, 0.6),
			FormatHelper.HsvToRgb(80.0, 0.7, 0.6),
			FormatHelper.HsvToRgb(140.0, 0.7, 0.7),
			FormatHelper.HsvToRgb(260.0, 0.7, 0.8),
			FormatHelper.HsvToRgb(0.0, 0.4, 0.8),
			FormatHelper.HsvToRgb(40.0, 0.4, 0.8),
			FormatHelper.HsvToRgb(80.0, 0.4, 0.8),
			FormatHelper.HsvToRgb(200.0, 0.4, 0.9),
			FormatHelper.HsvToRgb(260.0, 0.4, 1.0)
		};
	}
}
