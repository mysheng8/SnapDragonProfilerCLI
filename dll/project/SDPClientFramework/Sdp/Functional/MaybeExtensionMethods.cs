using System;

namespace Sdp.Functional
{
	// Token: 0x02000315 RID: 789
	public static class MaybeExtensionMethods
	{
		// Token: 0x06001066 RID: 4198 RVA: 0x00033AB3 File Offset: 0x00031CB3
		public static Maybe<T> ToMaybe<T>(this T value) where T : class
		{
			if (value != null)
			{
				return new Maybe<T>.Some(value);
			}
			return new Maybe<T>.None();
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00033AC9 File Offset: 0x00031CC9
		public static Maybe<string> ToMaybe(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return new Maybe<string>.None();
			}
			return new Maybe<string>.Some(value);
		}
	}
}
