using System;

namespace SDPClientFramework.Helpers
{
	// Token: 0x02000049 RID: 73
	public static class MathExtensions
	{
		// Token: 0x06000189 RID: 393 RVA: 0x00006218 File Offset: 0x00004418
		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			if (min.CompareTo(max) > 0)
			{
				throw new ArgumentException("Invalid clamp paramaters. Min can not be greater than max");
			}
			if (value.CompareTo(max) > 0)
			{
				return max;
			}
			if (value.CompareTo(min) < 0)
			{
				return min;
			}
			return value;
		}
	}
}
