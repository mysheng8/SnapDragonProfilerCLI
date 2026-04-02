using System;

namespace Sdp
{
	// Token: 0x02000127 RID: 295
	public class BinConfiguration
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x00009E8C File Offset: 0x0000808C
		public uint GetTotalWidth()
		{
			uint num = 0U;
			foreach (BinGroup binGroup in this.Groups)
			{
				uint num2 = binGroup.OffsetX + binGroup.NumBinsInX * this.BinWidth;
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00009ED4 File Offset: 0x000080D4
		public uint GetTotalHeight()
		{
			uint num = 0U;
			foreach (BinGroup binGroup in this.Groups)
			{
				uint num2 = binGroup.OffsetY + binGroup.NumBinsInY * this.BinHeight;
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00009F1C File Offset: 0x0000811C
		public uint TransformedGroupWidth(int groupIndex)
		{
			switch (this.Rotations)
			{
			default:
				return this.GroupWidth(groupIndex);
			case 1U:
			case 3U:
				return this.GroupHeight(groupIndex);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00009F58 File Offset: 0x00008158
		public uint TransformedGroupHeight(int groupIndex)
		{
			switch (this.Rotations)
			{
			default:
				return this.GroupHeight(groupIndex);
			case 1U:
			case 3U:
				return this.GroupWidth(groupIndex);
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00009F91 File Offset: 0x00008191
		public uint GroupWidth(int groupIndex)
		{
			return this.BinWidth * this.Groups[groupIndex].NumBinsInX;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00009FA7 File Offset: 0x000081A7
		public uint GroupHeight(int groupIndex)
		{
			return this.BinHeight * this.Groups[groupIndex].NumBinsInY;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00009FC0 File Offset: 0x000081C0
		public double TransformedGroupOffsetX(int groupIndex, double imageWidth)
		{
			switch (this.Rotations % 4U)
			{
			default:
				switch (this.OriginLocation)
				{
				default:
					return this.Groups[groupIndex].OffsetX;
				case OriginLocation.TOP_RIGHT:
				case OriginLocation.BOTTOM_RIGHT:
					return imageWidth - this.Groups[groupIndex].OffsetX - this.GroupWidth(groupIndex);
				}
				break;
			case 1U:
			{
				OriginLocation originLocation = this.OriginLocation;
				if (originLocation <= OriginLocation.TOP_RIGHT || originLocation - OriginLocation.BOTTOM_RIGHT > 1)
				{
					return imageWidth - this.Groups[groupIndex].OffsetY - this.GroupHeight(groupIndex);
				}
				return this.Groups[groupIndex].OffsetY;
			}
			case 2U:
				switch (this.OriginLocation)
				{
				default:
					return imageWidth - this.Groups[groupIndex].OffsetX - this.GroupWidth(groupIndex);
				case OriginLocation.TOP_RIGHT:
				case OriginLocation.BOTTOM_RIGHT:
					return this.Groups[groupIndex].OffsetX;
				}
				break;
			case 3U:
			{
				OriginLocation originLocation2 = this.OriginLocation;
				if (originLocation2 <= OriginLocation.TOP_RIGHT || originLocation2 - OriginLocation.BOTTOM_RIGHT > 1)
				{
					return this.Groups[groupIndex].OffsetY;
				}
				return imageWidth - this.Groups[groupIndex].OffsetY - this.GroupHeight(groupIndex);
			}
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000A0FC File Offset: 0x000082FC
		public double TransformedGroupOffsetY(int groupIndex, double imageHeight)
		{
			switch (this.Rotations % 4U)
			{
			default:
			{
				OriginLocation originLocation = this.OriginLocation;
				if (originLocation <= OriginLocation.TOP_RIGHT || originLocation - OriginLocation.BOTTOM_RIGHT > 1)
				{
					return this.Groups[groupIndex].OffsetY;
				}
				return imageHeight - this.Groups[groupIndex].OffsetY - this.GroupHeight(groupIndex);
			}
			case 1U:
				switch (this.OriginLocation)
				{
				default:
					return this.Groups[groupIndex].OffsetX;
				case OriginLocation.TOP_RIGHT:
				case OriginLocation.BOTTOM_RIGHT:
					return imageHeight - this.Groups[groupIndex].OffsetX - this.GroupWidth(groupIndex);
				}
				break;
			case 2U:
			{
				OriginLocation originLocation2 = this.OriginLocation;
				if (originLocation2 <= OriginLocation.TOP_RIGHT || originLocation2 - OriginLocation.BOTTOM_RIGHT > 1)
				{
					return imageHeight - this.Groups[groupIndex].OffsetY - this.GroupHeight(groupIndex);
				}
				return this.Groups[groupIndex].OffsetY;
			}
			case 3U:
				switch (this.OriginLocation)
				{
				default:
					return imageHeight - this.Groups[groupIndex].OffsetX - this.GroupWidth(groupIndex);
				case OriginLocation.TOP_RIGHT:
				case OriginLocation.BOTTOM_RIGHT:
					return this.Groups[groupIndex].OffsetX;
				}
				break;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000A238 File Offset: 0x00008438
		public uint GetTransformedOriginX()
		{
			switch (this.OriginLocation)
			{
			case OriginLocation.TOP_RIGHT:
			case OriginLocation.BOTTOM_RIGHT:
				return this.GetTotalWidth();
			}
			return 0U;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000A26C File Offset: 0x0000846C
		public uint GetTranformedOriginY()
		{
			OriginLocation originLocation = this.OriginLocation;
			if (originLocation > OriginLocation.TOP_RIGHT && originLocation - OriginLocation.BOTTOM_RIGHT <= 1)
			{
				return this.GetTotalHeight();
			}
			return 0U;
		}

		// Token: 0x04000421 RID: 1057
		public OriginLocation OriginLocation;

		// Token: 0x04000422 RID: 1058
		public uint TotalNumBins;

		// Token: 0x04000423 RID: 1059
		public uint BinWidth;

		// Token: 0x04000424 RID: 1060
		public uint BinHeight;

		// Token: 0x04000425 RID: 1061
		public BinGroup[] Groups;

		// Token: 0x04000426 RID: 1062
		public uint NumGroups;

		// Token: 0x04000427 RID: 1063
		public uint NumMSAABins;

		// Token: 0x04000428 RID: 1064
		public uint Rotations;
	}
}
