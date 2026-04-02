using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Cairo;
using Newtonsoft.Json.Linq;

namespace QGLPlugin
{
	// Token: 0x02000005 RID: 5
	internal static class VkDebugMarkers
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000025BF File Offset: 0x000007BF
		public static bool IsDebugMarkerAPICall(string apiName)
		{
			return apiName == "vkCmdDebugMarkerBeginEXT" || apiName == "vkCmdDebugMarkerEndEXT" || apiName == "vkCmdDebugMarkerInsertEXT";
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000025EC File Offset: 0x000007EC
		public static ulong ParseCommandBufferParam(string functionParams)
		{
			Regex regex = new Regex("commandBuffer\\s*=\\s*0x(?<commandbuffer>[0-9a-fA-F]+)");
			Match match = regex.Match(functionParams);
			ulong num;
			if (!ulong.TryParse(match.Groups["commandbuffer"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
			{
				throw new ArgumentException("Unable to parse command buffer from parameter string: " + functionParams);
			}
			return num;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002648 File Offset: 0x00000848
		public static DebugMarkerInfo GetDebugMarkerInfo(uint captureID, uint callID)
		{
			string vulkanTraceStructParams = QGLModel.GetVulkanTraceStructParams(captureID, callID);
			return new DebugMarkerInfo
			{
				Name = VkDebugMarkers.ParseMarkerName(vulkanTraceStructParams),
				Color = VkDebugMarkers.ParseMarkerColor(vulkanTraceStructParams)
			};
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000267C File Offset: 0x0000087C
		public static string ParseMarkerName(string structParams)
		{
			Regex regex = new Regex("markerName\\s*=\\s*\"(?<markername>[^\"]*)\"");
			Match match = regex.Match(structParams);
			if (string.IsNullOrEmpty(match.Groups["markername"].Value))
			{
				throw new ArgumentException("Unable to parse marker name.  Invalid function struct param string: '" + structParams + "'");
			}
			return match.Groups[1].Value;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000026E0 File Offset: 0x000008E0
		public static Color ParseMarkerColor(string structParams)
		{
			string text = "([0-9]*\\.?[0-9]*)";
			Regex regex = new Regex(string.Concat(new string[] { "color\\s*=\\s*red:\\s*", text, ";\\s*green:\\s*", text, ";\\s*blue:\\s*", text, ";\\s*alpha:\\s*", text }));
			Match match = regex.Match(structParams);
			float num;
			float num2;
			float num3;
			float num4;
			if (match.Groups.Count < 5 || !float.TryParse(match.Groups[1].Value, out num) || !float.TryParse(match.Groups[2].Value, out num2) || !float.TryParse(match.Groups[3].Value, out num3) || !float.TryParse(match.Groups[4].Value, out num4))
			{
				throw new ArgumentException("Unable to parse marker color.  Invalid function struct param string: '" + structParams + "'");
			}
			if (num == 0f && num3 == 0f && num2 == 0f)
			{
				Random random = new Random();
				num = (float)random.NextDouble();
				num2 = (float)random.NextDouble();
				num3 = (float)random.NextDouble();
			}
			ValueTuple<float, float, float, float> valueTuple = VkHelper.ScaleRGBValueForWhiteText(num, num2, num3);
			return new Color((double)valueTuple.Item1, (double)valueTuple.Item2, (double)valueTuple.Item3, 1.0);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000283C File Offset: 0x00000A3C
		public static string ColorMarkerTextFromJArray(JArray colorArray, string debugMarkerText, bool defaultColor = false)
		{
			float num = 0f;
			string text;
			if (defaultColor)
			{
				text = "#6D6E71";
			}
			else
			{
				ValueTuple<string, float> valueTuple = VkHelper.RGBAJArrayToHex(colorArray);
				text = valueTuple.Item1;
				num = valueTuple.Item2;
			}
			return VkDebugMarkers.ColorMarkerTextDV(debugMarkerText, text, num);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000287C File Offset: 0x00000A7C
		public static string ColorMarkerTextDV(string debugMarkerText, string colorHex = "#000000", float brightness = 0f)
		{
			if (string.IsNullOrEmpty(debugMarkerText))
			{
				return string.Empty;
			}
			if (string.Equals(colorHex, "#000000") || string.Equals(colorHex, "#00000000"))
			{
				colorHex = "#6D6E71";
			}
			string text = ((brightness < 125f) ? ("<span background='" + colorHex) : ("<span foreground='black' background='" + colorHex));
			return VkHelper.ColorfyParameterString(text + "'> ", debugMarkerText + " ");
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000028F4 File Offset: 0x00000AF4
		public static string ColorMarkerTextRV(string debugMarkerText)
		{
			if (string.IsNullOrEmpty(debugMarkerText))
			{
				return string.Empty;
			}
			string text = "<span background='#6D6E71";
			return VkHelper.ColorfyParameterString(text + "'> ", debugMarkerText + " ");
		}

		// Token: 0x040000DD RID: 221
		public const string DEBUG_MARKER_BEGIN = "vkCmdDebugMarkerBeginEXT";

		// Token: 0x040000DE RID: 222
		public const string DEBUG_MARKER_END = "vkCmdDebugMarkerEndEXT";

		// Token: 0x040000DF RID: 223
		public const string DEBUG_MARKER_INSERT = "vkCmdDebugMarkerInsertEXT";

		// Token: 0x040000E0 RID: 224
		public const string DEBUG_MARKER_SET_OBJECT_TAG = "vkDebugMarkerSetObjectTagEXT";

		// Token: 0x040000E1 RID: 225
		public const string DEBUG_MARKER_SET_OBJECT_NAME = "vkDebugMarkerSetObjectNameEXT";

		// Token: 0x040000E2 RID: 226
		public const string DEBUG_MARKER_DEFAULT_COLOR = "#6D6E71";
	}
}
