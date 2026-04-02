using System;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace Sdp
{
	// Token: 0x02000267 RID: 615
	public class OptionValueRetriever
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x0001D774 File Offset: 0x0001B974
		public static object GetOptionValue(Option option)
		{
			if (option.GetValueStr() == null)
			{
				return null;
			}
			switch (option.GetOptionType())
			{
			case SDPDataType.SDP_UINT8:
			{
				string valueStr = option.GetValueStr();
				return byte.Parse(valueStr);
			}
			case SDPDataType.SDP_UINT16:
			{
				string valueStr2 = option.GetValueStr();
				return ushort.Parse(valueStr2);
			}
			case SDPDataType.SDP_UINT32:
			{
				string valueStr3 = option.GetValueStr();
				return uint.Parse(valueStr3);
			}
			case SDPDataType.SDP_UINT64:
			{
				string valueStr4 = option.GetValueStr();
				return ulong.Parse(valueStr4);
			}
			case SDPDataType.SDP_INT8:
			{
				string valueStr5 = option.GetValueStr();
				return sbyte.Parse(valueStr5);
			}
			case SDPDataType.SDP_INT16:
			{
				string valueStr6 = option.GetValueStr();
				return short.Parse(valueStr6);
			}
			case SDPDataType.SDP_INT32:
			{
				string valueStr7 = option.GetValueStr();
				return int.Parse(valueStr7);
			}
			case SDPDataType.SDP_INT64:
			{
				string valueStr8 = option.GetValueStr();
				return long.Parse(valueStr8);
			}
			case SDPDataType.SDP_FLOAT32:
			{
				float num;
				option.GetValue(out num);
				return num;
			}
			case SDPDataType.SDP_FLOAT64:
			{
				double num2;
				option.GetValue(out num2);
				return num2;
			}
			case SDPDataType.SDP_STRING:
				return option.GetValueStr();
			case SDPDataType.SDP_BOOL:
			{
				string valueStr9 = option.GetValueStr();
				return bool.Parse(valueStr9);
			}
			case SDPDataType.SDP_ENUM:
			{
				string rawValueStr = option.GetRawValueStr();
				return OptionEnum.Parse(rawValueStr).EnumValue;
			}
			case SDPDataType.SDP_FLOAT32_4:
			{
				float[] array = new float[4];
				option.GetValue(out array[0], out array[1], out array[2], out array[3]);
				return new OptionColor(array);
			}
			}
			return null;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0001D90C File Offset: 0x0001BB0C
		public static void SetOptionValue(Option option, object value, bool publish)
		{
			switch (option.GetOptionType())
			{
			case SDPDataType.SDP_UINT8:
				if (value is byte)
				{
					option.SetValue((int)((byte)value), publish);
					return;
				}
				break;
			case SDPDataType.SDP_UINT16:
				if (value is ushort)
				{
					option.SetValue((int)((ushort)value), publish);
					return;
				}
				break;
			case SDPDataType.SDP_UINT32:
				if (value is uint)
				{
					option.SetValue((uint)value, publish);
					return;
				}
				break;
			case SDPDataType.SDP_UINT64:
				if (value is ulong)
				{
					option.SetValue((ulong)value, publish);
					return;
				}
				break;
			case SDPDataType.SDP_INT8:
				if (value is sbyte)
				{
					option.SetValue((int)((sbyte)value), publish);
					return;
				}
				break;
			case SDPDataType.SDP_INT16:
				if (value is short)
				{
					option.SetValue((int)((short)value), publish);
					return;
				}
				break;
			case SDPDataType.SDP_INT32:
				if (value is int)
				{
					option.SetValue((int)value, publish);
					return;
				}
				break;
			case SDPDataType.SDP_INT64:
				if (value is long)
				{
					option.SetValue((long)value, publish);
					return;
				}
				break;
			case SDPDataType.SDP_FLOAT32:
				if (value is float)
				{
					option.SetValue((float)value, publish);
					return;
				}
				break;
			case SDPDataType.SDP_FLOAT64:
				if (value is double)
				{
					option.SetValue((double)value, publish);
					return;
				}
				break;
			case SDPDataType.SDP_STRING:
				if (value is string)
				{
					option.SetValue((string)value, publish);
					return;
				}
				break;
			case SDPDataType.SDP_CUSTOM:
			case SDPDataType.SDP_BINARYDATA:
				return;
			case SDPDataType.SDP_BOOL:
				if (value is bool)
				{
					option.SetValue((bool)value, publish);
					return;
				}
				break;
			case SDPDataType.SDP_ENUM:
			{
				OptionEnum optionEnum = value as OptionEnum;
				if (optionEnum != null)
				{
					if (OptionValueRetriever.<>o__1.<>p__0 == null)
					{
						OptionValueRetriever.<>o__1.<>p__0 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(int), typeof(OptionValueRetriever)));
					}
					option.SetValue(OptionValueRetriever.<>o__1.<>p__0.Target(OptionValueRetriever.<>o__1.<>p__0, optionEnum.EnumValue), publish);
					return;
				}
				option.SetValue((int)value, publish);
				return;
			}
			case SDPDataType.SDP_FLOAT32_4:
			{
				OptionColor optionColor = value as OptionColor;
				if (optionColor != null)
				{
					option.SetValue(optionColor.R, optionColor.G, optionColor.B, optionColor.A, publish);
					return;
				}
				if (value is float[])
				{
					float[] array = (float[])value;
					if (array.Length == 4)
					{
						option.SetValue(array[0], array[1], array[2], array[3], publish);
						return;
					}
				}
				break;
			}
			default:
				return;
			}
		}
	}
}
