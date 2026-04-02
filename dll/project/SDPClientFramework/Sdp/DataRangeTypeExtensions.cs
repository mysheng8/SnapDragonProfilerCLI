using System;

namespace Sdp
{
	// Token: 0x020002E1 RID: 737
	public static class DataRangeTypeExtensions
	{
		// Token: 0x06000ECA RID: 3786 RVA: 0x0002DBA6 File Offset: 0x0002BDA6
		public static string GetHeaderString(this DataRangeType dataRangeType)
		{
			if (dataRangeType == DataRangeType.PushConstant)
			{
				return "<b>Push Constants</b>";
			}
			if (dataRangeType != DataRangeType.Uniform)
			{
				return "<b>" + dataRangeType.ToString() + "</b>";
			}
			return "<b>Uniforms</b>";
		}
	}
}
