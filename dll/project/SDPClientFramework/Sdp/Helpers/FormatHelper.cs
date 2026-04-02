using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cairo;

namespace Sdp.Helpers
{
	// Token: 0x02000306 RID: 774
	public class FormatHelper
	{
		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003100C File Offset: 0x0002F20C
		public static string FormatTimeLabel(long ts, string msFormat = "#.##", string secFormat = "#.###")
		{
			if (ts / 1000L == 0L)
			{
				return ts.ToString() + "us";
			}
			if (ts / 1000000L == 0L)
			{
				double num = (double)ts / 1000.0;
				return DoubleConverter.ToString(num, msFormat) + "ms";
			}
			double num2 = (double)ts / 1000000.0;
			return DoubleConverter.ToString(num2, secFormat) + "s";
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0003107C File Offset: 0x0002F27C
		public static string FormatTimeLabelNS(ulong timestampNS, string usFormat = "#.##", string msFormat = "#.###", string secFormat = "#.####")
		{
			if (timestampNS / 1000UL == 0UL)
			{
				return timestampNS.ToString() + "ns";
			}
			if (timestampNS / 1000000UL == 0UL)
			{
				double num = timestampNS / 1000.0;
				return DoubleConverter.ToString(num, usFormat) + "us";
			}
			if (timestampNS / 1000000000UL == 0UL)
			{
				double num2 = timestampNS / 1000000.0;
				return DoubleConverter.ToString(num2, msFormat) + "ms";
			}
			double num3 = timestampNS / 1000000000.0;
			return DoubleConverter.ToString(num3, secFormat) + "s";
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00031118 File Offset: 0x0002F318
		public static long ParseTimeString(string timeString)
		{
			if (timeString.EndsWith("ms"))
			{
				timeString = timeString.Substring(0, timeString.Length - 2);
				return (long)(double.Parse(timeString) * 1000.0);
			}
			if (timeString.EndsWith("us"))
			{
				timeString = timeString.Substring(0, timeString.Length - 2);
				return (long)double.Parse(timeString);
			}
			if (timeString.EndsWith("s"))
			{
				timeString = timeString.Substring(0, timeString.Length - 1);
				return (long)(double.Parse(timeString) * 1000000.0);
			}
			throw new Exception("Invalid time string format");
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x000311B8 File Offset: 0x0002F3B8
		public static ulong ParseTimeStringNS(string timeString)
		{
			if (timeString.EndsWith("ms"))
			{
				timeString = timeString.Substring(0, timeString.Length - 2);
				return (ulong)(double.Parse(timeString) * 1000000.0);
			}
			if (timeString.EndsWith("us"))
			{
				timeString = timeString.Substring(0, timeString.Length - 2);
				return (ulong)(double.Parse(timeString) * 1000.0);
			}
			if (timeString.EndsWith("ns"))
			{
				timeString = timeString.Substring(0, timeString.Length - 2);
				return (ulong)double.Parse(timeString);
			}
			if (timeString.EndsWith("s"))
			{
				timeString = timeString.Substring(0, timeString.Length - 1);
				return (ulong)(double.Parse(timeString) * 1000000000.0);
			}
			throw new Exception("Invalid time string format");
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00031288 File Offset: 0x0002F488
		public static Color HsvToRgb(double hue, double saturation, double value)
		{
			double num = hue % 360.0;
			if (num < 0.0)
			{
				num += 360.0;
			}
			double num2 = Math.Min(Math.Max(0.0, saturation), 1.0);
			double num3 = Math.Min(Math.Max(0.0, value), 1.0);
			double num6;
			double num5;
			double num4;
			if (num2 <= 0.0)
			{
				num4 = (num5 = (num6 = num3));
			}
			else
			{
				double num7 = num / 60.0;
				int num8 = (int)Math.Floor(num7);
				double num9 = num7 - (double)num8;
				double num10 = num3 * (1.0 - num2);
				double num11 = num3 * (1.0 - num2 * num9);
				double num12 = num3 * (1.0 - num2 * (1.0 - num9));
				switch (num8)
				{
				case 0:
					num5 = num3;
					num4 = num12;
					num6 = num10;
					break;
				case 1:
					num5 = num11;
					num4 = num3;
					num6 = num10;
					break;
				case 2:
					num5 = num10;
					num4 = num3;
					num6 = num12;
					break;
				case 3:
					num5 = num10;
					num4 = num11;
					num6 = num3;
					break;
				case 4:
					num5 = num12;
					num4 = num10;
					num6 = num3;
					break;
				case 5:
					num5 = num3;
					num4 = num10;
					num6 = num11;
					break;
				default:
					num4 = (num5 = (num6 = num3));
					break;
				}
			}
			return new Color(num5, num4, num6);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x000313F8 File Offset: 0x0002F5F8
		public static Color PseudoRandomColor()
		{
			double num = FormatHelper.m_random.NextDouble() * 360.0;
			double num2 = Math.Abs(FormatHelper.m_pseudoRandomColorHue - num);
			while (num2 < 41.6789 || num2 > 318.3211)
			{
				num = FormatHelper.m_random.NextDouble() * 360.0;
				num2 = Math.Abs(FormatHelper.m_pseudoRandomColorHue - num);
			}
			FormatHelper.m_pseudoRandomColorHue = num;
			FormatHelper.m_pseudoRandomColorSaturation = FormatHelper.m_random.NextDouble() * 0.4 + 0.6;
			FormatHelper.m_pseudoRandomColorValue = FormatHelper.m_random.NextDouble() * 0.35 + 0.5;
			Color color = FormatHelper.HsvToRgb(FormatHelper.m_pseudoRandomColorHue, FormatHelper.m_pseudoRandomColorSaturation, FormatHelper.m_pseudoRandomColorValue);
			if (color.R > 0.5)
			{
				double num3 = Math.Min(1.0, color.G + color.B);
				color.R = Math.Max(color.R * num3, 0.5);
			}
			return color;
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00031514 File Offset: 0x0002F714
		public static PropertyDescriptor CreatePropertyDescriptorFromOption(Option option)
		{
			if (option == null)
			{
				return null;
			}
			SDPDataType optionType = option.GetOptionType();
			string name = option.GetName();
			string valueStr = option.GetValueStr();
			string description = option.GetDescription();
			bool flag = option.IsOptionReadonly();
			string categoryName = OptionsViewController.GetCategoryName(option);
			string parentCategoryName = OptionsViewController.GetParentCategoryName(option);
			if (string.IsNullOrEmpty(valueStr))
			{
				return null;
			}
			PropertyDescriptor propertyDescriptor = null;
			switch (optionType)
			{
			case SDPDataType.SDP_UINT8:
			{
				byte b = byte.Parse(valueStr);
				propertyDescriptor = new SdpPropertyDescriptor<byte>(name, typeof(byte), b, categoryName, description, flag);
				break;
			}
			case SDPDataType.SDP_UINT16:
			{
				ushort num = ushort.Parse(valueStr);
				propertyDescriptor = new SdpPropertyDescriptor<ushort>(name, typeof(ushort), num, categoryName, description, flag);
				break;
			}
			case SDPDataType.SDP_UINT32:
			{
				uint num2 = uint.Parse(valueStr);
				propertyDescriptor = new SdpPropertyDescriptor<uint>(name, typeof(uint), num2, categoryName, description, flag);
				break;
			}
			case SDPDataType.SDP_UINT64:
			{
				ulong num3 = ulong.Parse(valueStr);
				propertyDescriptor = new SdpPropertyDescriptor<ulong>(name, typeof(ulong), num3, categoryName, description, flag);
				break;
			}
			case SDPDataType.SDP_INT8:
			{
				sbyte b2 = sbyte.Parse(valueStr);
				propertyDescriptor = new SdpPropertyDescriptor<sbyte>(name, typeof(sbyte), b2, categoryName, description, flag);
				break;
			}
			case SDPDataType.SDP_INT16:
			{
				short num4 = short.Parse(valueStr);
				propertyDescriptor = new SdpPropertyDescriptor<short>(name, typeof(short), num4, categoryName, description, flag);
				break;
			}
			case SDPDataType.SDP_INT32:
			{
				int num5 = int.Parse(valueStr);
				propertyDescriptor = new SdpPropertyDescriptor<int>(name, typeof(int), num5, categoryName, description, flag);
				break;
			}
			case SDPDataType.SDP_INT64:
			{
				long num6 = long.Parse(valueStr);
				propertyDescriptor = new SdpPropertyDescriptor<long>(name, typeof(long), num6, categoryName, description, flag);
				break;
			}
			case SDPDataType.SDP_FLOAT32:
			{
				float num7;
				if (option.GetValue(out num7))
				{
					propertyDescriptor = new SdpPropertyDescriptor<float>(name, typeof(float), num7, categoryName, description, flag);
				}
				break;
			}
			case SDPDataType.SDP_FLOAT64:
			{
				double num8;
				if (option.GetValue(out num8))
				{
					propertyDescriptor = new SdpPropertyDescriptor<double>(name, typeof(double), num8, categoryName, description, flag);
				}
				break;
			}
			case SDPDataType.SDP_STRING:
				propertyDescriptor = new SdpPropertyDescriptor<string>(name, typeof(string), valueStr, categoryName, description, flag);
				break;
			case SDPDataType.SDP_BOOL:
			{
				bool flag2 = bool.Parse(valueStr);
				propertyDescriptor = new SdpPropertyDescriptor<bool>(name, typeof(bool), flag2, categoryName, description, flag);
				break;
			}
			case SDPDataType.SDP_ENUM:
			{
				string rawValueStr = option.GetRawValueStr();
				OptionEnum optionEnum = OptionEnum.Parse(rawValueStr);
				Type type = typeof(SdpPropertyDescriptor<>).MakeGenericType(new Type[] { optionEnum.EnumType });
				object[] array = new object[] { name, optionEnum.EnumType, optionEnum.EnumValue, categoryName, description, flag };
				propertyDescriptor = (PropertyDescriptor)Activator.CreateInstance(type, array);
				break;
			}
			case SDPDataType.SDP_FLOAT32_4:
			{
				float[] array2 = new float[4];
				if (option.GetValue(out array2[0], out array2[1], out array2[2], out array2[3]))
				{
					propertyDescriptor = new SdpPropertyDescriptor<OptionColor>(name, typeof(OptionColor), new OptionColor(array2), categoryName, description, flag);
				}
				break;
			}
			}
			return propertyDescriptor;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00031840 File Offset: 0x0002FA40
		public static Color GetInterpolatedPerfColor(double value)
		{
			Color[] lowToHighPerformanceColors = SdpApp.ModelManager.SettingsModel.UserPreferences.GetLowToHighPerformanceColors();
			value = ((value < 0.0) ? 0.0 : ((value > 1.0) ? 1.0 : value));
			double num = value * (double)(lowToHighPerformanceColors.Length - 1);
			int num2 = (int)num;
			double num3 = num - (double)num2;
			return FormatHelper.InterpolateColors(lowToHighPerformanceColors[num2], lowToHighPerformanceColors[Math.Min(num2 + 1, lowToHighPerformanceColors.Length - 1)], num3);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x000318C4 File Offset: 0x0002FAC4
		private static Color InterpolateColors(Color startColor, Color endColor, double amount)
		{
			double num = startColor.R + amount * (endColor.R - startColor.R);
			double num2 = startColor.G + amount * (endColor.G - startColor.G);
			double num3 = startColor.B + amount * (endColor.B - startColor.B);
			return new Color(num, num2, num3);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00031928 File Offset: 0x0002FB28
		public static float U16ToF32(ushort half)
		{
			int num = (half >> 15) & 1;
			int num2 = (half >> 10) & 31;
			int num3 = (int)(half & 1023);
			int num4;
			if (num2 == 0)
			{
				if (num3 == 0)
				{
					num4 = num << 31;
				}
				else
				{
					while ((num3 & 1024) == 0)
					{
						num3 <<= 1;
						num2--;
					}
					num2++;
					num3 &= -1025;
					num4 = (num << 31) | (num2 + 112 << 23) | (num3 << 13);
				}
			}
			else if (num2 == 31)
			{
				num4 = (num << 31) | 2139095040 | (num3 << 13);
			}
			else
			{
				num4 = (num << 31) | (num2 + 112 << 23) | (num3 << 13);
			}
			byte[] bytes = BitConverter.GetBytes(num4);
			return BitConverter.ToSingle(bytes, 0);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x000319C4 File Offset: 0x0002FBC4
		public static string FormatData(ref byte[] data, int offset, string dataType)
		{
			string text = string.Empty;
			if (offset < 0 || offset >= data.Length || offset + FormatHelper.GetElementSize(dataType) > data.Length)
			{
				return text;
			}
			if (DataTypes.IsVector(dataType))
			{
				return FormatHelper.FormatVector(ref data, offset, dataType);
			}
			if (DataTypes.IsMatrix(dataType))
			{
				return FormatHelper.FormatMatrix(ref data, offset, dataType);
			}
			bool flag = false;
			if (dataType != null)
			{
				switch (dataType.Length)
				{
				case 3:
					if (!(dataType == "int"))
					{
						goto IL_03BC;
					}
					break;
				case 4:
				{
					char c = dataType[1];
					if (c <= 'i')
					{
						if (c != 'a')
						{
							if (c != 'i')
							{
								goto IL_03BC;
							}
							if (!(dataType == "uint"))
							{
								goto IL_03BC;
							}
							goto IL_0356;
						}
						else
						{
							if (!(dataType == "half"))
							{
								goto IL_03BC;
							}
							float num = FormatHelper.U16ToF32(BitConverter.ToUInt16(data, offset));
							text = string.Format("{0:F5}", num);
							flag = num >= 0f;
							goto IL_03BC;
						}
					}
					else if (c != 'o')
					{
						if (c != 'y')
						{
							goto IL_03BC;
						}
						if (!(dataType == "byte"))
						{
							goto IL_03BC;
						}
						text = string.Format("{0}", data[offset]);
						goto IL_03BC;
					}
					else
					{
						if (!(dataType == "bool"))
						{
							goto IL_03BC;
						}
						text = BitConverter.ToBoolean(data, offset).ToString();
						goto IL_03BC;
					}
					break;
				}
				case 5:
				{
					char c = dataType[3];
					if (c <= '3')
					{
						if (c != '1')
						{
							if (c != '3')
							{
								goto IL_03BC;
							}
							if (!(dataType == "int32"))
							{
								goto IL_03BC;
							}
						}
						else
						{
							if (!(dataType == "int16"))
							{
								goto IL_03BC;
							}
							short num2 = BitConverter.ToInt16(data, offset);
							text = num2.ToString();
							flag = num2 >= 0;
							goto IL_03BC;
						}
					}
					else if (c != '6')
					{
						if (c != 'a')
						{
							if (c != 't')
							{
								goto IL_03BC;
							}
							if (!(dataType == "ubyte"))
							{
								goto IL_03BC;
							}
							text = string.Format("0x{0:X2}", data[offset]);
							goto IL_03BC;
						}
						else
						{
							if (!(dataType == "float"))
							{
								goto IL_03BC;
							}
							float num3 = BitConverter.ToSingle(data, offset);
							text = string.Format("{0:F5}", num3);
							flag = num3 >= 0f;
							goto IL_03BC;
						}
					}
					else
					{
						if (!(dataType == "int64"))
						{
							goto IL_03BC;
						}
						long num4 = BitConverter.ToInt64(data, offset);
						text = num4.ToString();
						flag = num4 >= 0L;
						goto IL_03BC;
					}
					break;
				}
				case 6:
				{
					char c = dataType[4];
					if (c <= '3')
					{
						if (c != '1')
						{
							if (c != '3')
							{
								goto IL_03BC;
							}
							if (!(dataType == "uint32"))
							{
								goto IL_03BC;
							}
							goto IL_0356;
						}
						else
						{
							if (!(dataType == "uint16"))
							{
								goto IL_03BC;
							}
							text = BitConverter.ToUInt16(data, offset).ToString();
							goto IL_03BC;
						}
					}
					else if (c != '6')
					{
						if (c != 'l')
						{
							goto IL_03BC;
						}
						if (!(dataType == "double"))
						{
							goto IL_03BC;
						}
						double num5 = BitConverter.ToDouble(data, offset);
						text = string.Format("{0:F5}", num5);
						flag = num5 >= 0.0;
						goto IL_03BC;
					}
					else
					{
						if (!(dataType == "uint64"))
						{
							goto IL_03BC;
						}
						text = BitConverter.ToUInt64(data, offset).ToString();
						goto IL_03BC;
					}
					break;
				}
				default:
					goto IL_03BC;
				}
				int num6 = BitConverter.ToInt32(data, offset);
				text = num6.ToString();
				flag = num6 >= 0;
				goto IL_03BC;
				IL_0356:
				text = BitConverter.ToUInt32(data, offset).ToString();
			}
			IL_03BC:
			return flag ? (" " + text) : text;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00031DA0 File Offset: 0x0002FFA0
		private static string FormatVector(ref byte[] data, int offset, string dataType)
		{
			string text = string.Empty;
			if (!DataTypes.IsVector(dataType))
			{
				return text;
			}
			string[] array = dataType.Split(new string[] { "vec" }, StringSplitOptions.None);
			if (array.Length < 2)
			{
				return text;
			}
			string text2 = array[0].Replace(" ", "");
			int elementSize = FormatHelper.GetElementSize(text2);
			int num;
			if (!int.TryParse(array[1], out num))
			{
				return text;
			}
			text = "( ";
			for (int i = 0; i < num; i++)
			{
				int num2 = i * elementSize;
				text += FormatHelper.FormatData(ref data, offset + num2, text2);
				text += ((i == num - 1) ? " )" : ", ");
			}
			return text;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00031E50 File Offset: 0x00030050
		private static string FormatMatrix(ref byte[] data, int offset, string dataType)
		{
			string text = string.Empty;
			if (!DataTypes.IsMatrix(dataType))
			{
				return text;
			}
			string[] array = dataType.Split(new string[] { "mat" }, StringSplitOptions.None);
			if (array.Length < 2)
			{
				return text;
			}
			string text2 = array[0].Replace(" ", "");
			int elementSize = FormatHelper.GetElementSize(text2);
			bool flag = false;
			array = array[1].Split(new string[] { "(" }, StringSplitOptions.None);
			if (array.Length > 1)
			{
				int num;
				int.TryParse(array[1].Split(new string[] { ")" }, StringSplitOptions.None)[0], out num);
				flag = num > elementSize;
			}
			int num2;
			int num3;
			if (int.TryParse(array[0], out num2))
			{
				num3 = num2;
			}
			else
			{
				string[] array2 = array[0].Split(new string[] { "x" }, StringSplitOptions.None);
				if (array2.Length < 2 || !int.TryParse(array2[0], out num2) || !int.TryParse(array2[1], out num3))
				{
					return text;
				}
			}
			int num4 = elementSize * num2;
			int num5 = elementSize * num3;
			for (int i = 0; i < num3; i++)
			{
				text += ((i == 0) ? "( " : "\n( ");
				for (int j = 0; j < num2; j++)
				{
					int num6 = (flag ? (i * elementSize + j * num5) : (i * num4 + j * elementSize));
					text += FormatHelper.FormatData(ref data, offset + num6, text2);
					text += ((j == num2 - 1) ? " )" : ", ");
				}
			}
			return text;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00031FCC File Offset: 0x000301CC
		public static int GetElementSize(string dataType)
		{
			if (dataType != null)
			{
				switch (dataType.Length)
				{
				case 3:
					if (!(dataType == "int"))
					{
						return 0;
					}
					break;
				case 4:
				{
					char c = dataType[1];
					if (c <= 'i')
					{
						if (c != 'a')
						{
							if (c != 'i')
							{
								return 0;
							}
							if (!(dataType == "uint"))
							{
								return 0;
							}
							return 4;
						}
						else
						{
							if (!(dataType == "half"))
							{
								return 0;
							}
							return 2;
						}
					}
					else if (c != 'o')
					{
						if (c != 'y')
						{
							return 0;
						}
						if (!(dataType == "byte"))
						{
							return 0;
						}
						return 1;
					}
					else
					{
						if (!(dataType == "bool"))
						{
							return 0;
						}
						return 1;
					}
					break;
				}
				case 5:
				{
					char c = dataType[3];
					if (c <= '3')
					{
						if (c != '1')
						{
							if (c != '3')
							{
								return 0;
							}
							if (!(dataType == "int32"))
							{
								return 0;
							}
						}
						else
						{
							if (!(dataType == "int16"))
							{
								return 0;
							}
							return 2;
						}
					}
					else if (c != '6')
					{
						if (c != 'a')
						{
							if (c != 't')
							{
								return 0;
							}
							if (!(dataType == "ubyte"))
							{
								return 0;
							}
							return 1;
						}
						else
						{
							if (!(dataType == "float"))
							{
								return 0;
							}
							return 4;
						}
					}
					else
					{
						if (!(dataType == "int64"))
						{
							return 0;
						}
						return 8;
					}
					break;
				}
				case 6:
				{
					char c = dataType[4];
					if (c <= '3')
					{
						if (c != '1')
						{
							if (c != '3')
							{
								return 0;
							}
							if (!(dataType == "uint32"))
							{
								return 0;
							}
							return 4;
						}
						else
						{
							if (!(dataType == "uint16"))
							{
								return 0;
							}
							return 2;
						}
					}
					else if (c != '6')
					{
						if (c != 'l')
						{
							return 0;
						}
						if (!(dataType == "double"))
						{
							return 0;
						}
						return 8;
					}
					else
					{
						if (!(dataType == "uint64"))
						{
							return 0;
						}
						return 8;
					}
					break;
				}
				default:
					return 0;
				}
				return 4;
			}
			return 0;
		}

		// Token: 0x04000AC1 RID: 2753
		private static Random m_random = new Random();

		// Token: 0x04000AC2 RID: 2754
		private static double m_pseudoRandomColorHue = 0.0;

		// Token: 0x04000AC3 RID: 2755
		private static double m_pseudoRandomColorSaturation = 1.0;

		// Token: 0x04000AC4 RID: 2756
		private static double m_pseudoRandomColorValue = 0.8;

		// Token: 0x04000AC5 RID: 2757
		public static Color Pink = new Color(1.0, 0.41, 0.71);

		// Token: 0x04000AC6 RID: 2758
		public static Color Yellow = new Color(0.99, 0.83, 0.1);

		// Token: 0x04000AC7 RID: 2759
		public static Color Purple = new Color(0.33, 0.1, 0.55);

		// Token: 0x04000AC8 RID: 2760
		public static Color Red = new Color(0.8, 0.16, 0.2);

		// Token: 0x04000AC9 RID: 2761
		public static Color Green = new Color(0.2, 0.49, 0.18);

		// Token: 0x04000ACA RID: 2762
		public static Color BrightGreen = new Color(0.2, 0.99, 0.18);

		// Token: 0x04000ACB RID: 2763
		public static Color Blue = new Color(0.07, 0.54, 0.78);

		// Token: 0x04000ACC RID: 2764
		public static Color Cyan = new Color(0.21, 0.76, 0.85);

		// Token: 0x04000ACD RID: 2765
		public static Color SeaGreen = new Color(0.2, 0.69, 0.28);

		// Token: 0x04000ACE RID: 2766
		public static Color MedOrchid = new Color(0.76, 0.33, 0.8);

		// Token: 0x04000ACF RID: 2767
		public static Color UlMarine = new Color(0.03, 0.1, 0.55);

		// Token: 0x04000AD0 RID: 2768
		public static Color BrickRed = new Color(0.8, 0.16, 0.2);

		// Token: 0x04000AD1 RID: 2769
		public static Color HanPurple = new Color(0.34, 0.06, 0.85);

		// Token: 0x04000AD2 RID: 2770
		public static Color Gray = new Color(0.45, 0.45, 0.45);

		// Token: 0x04000AD3 RID: 2771
		public static Color Denim = new Color(0.07, 0.54, 0.78);

		// Token: 0x04000AD4 RID: 2772
		public static Color CBlue = new Color(0.45, 0.41, 0.71);

		// Token: 0x04000AD5 RID: 2773
		public static Color SBlue = new Color(0.29, 0.49, 0.69);

		// Token: 0x04000AD6 RID: 2774
		public static Color BBerry = new Color(0.29, 0.09, 0.26);

		// Token: 0x04000AD7 RID: 2775
		public static Color SGum = new Color(0.29, 0.18, 0.36);

		// Token: 0x04000AD8 RID: 2776
		public static Color Maroon = new Color(0.29, 0.15, 0.0);

		// Token: 0x04000AD9 RID: 2777
		public static Color Lochin = new Color(0.29, 0.56, 0.5);

		// Token: 0x04000ADA RID: 2778
		public static Color Mariner = new Color(0.29, 0.34, 0.66);

		// Token: 0x04000ADB RID: 2779
		public static List<Color> DefaultColors = new List<Color>
		{
			FormatHelper.Pink,
			FormatHelper.Yellow,
			FormatHelper.Purple,
			FormatHelper.Red,
			FormatHelper.Green,
			FormatHelper.BrightGreen,
			FormatHelper.Blue,
			FormatHelper.Cyan
		};
	}
}
