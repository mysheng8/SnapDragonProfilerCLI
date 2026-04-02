using System;
using System.Collections.Generic;
using System.Linq;
using Sdp.Helpers;

namespace Sdp.Functional
{
	// Token: 0x02000319 RID: 793
	public static class Result
	{
		// Token: 0x06001074 RID: 4212 RVA: 0x00033BFD File Offset: 0x00031DFD
		public static Result<T, E> ToResult<T, E>(this T value, E error) where T : class
		{
			if (value != null)
			{
				return new Result<T, E>.Success(value);
			}
			return new Result<T, E>.Error(error);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00033C14 File Offset: 0x00031E14
		public static Maybe<T> ResultToMaybe<T, E>(this Result<T, E> result)
		{
			return result.Match<Maybe<T>>((T success) => new Maybe<T>.Some(success), (E error) => new Maybe<T>.None());
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00033C68 File Offset: 0x00031E68
		public static T Unwrap<T, E>(this Result<T, E> result, Func<E, Exception> onError)
		{
			return result.Match<T>((T success) => success, delegate(E error)
			{
				throw onError(error);
			});
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00033CB4 File Offset: 0x00031EB4
		public static Result<U, E> Bind<T, U, E>(this Result<T, E> result, Func<T, Result<U, E>> binder)
		{
			return result.Match<Result<U, E>>((T success) => binder(success), (E error) => new Result<U, E>.Error(error));
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00033D00 File Offset: 0x00031F00
		public static Result<E> Bind<E>(this Result<E> result, Func<Result<E>> binder)
		{
			return result.Match<Result<E>>(() => binder(), (E error) => new Result<E>.Error(error));
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00033D4C File Offset: 0x00031F4C
		public static Result<E> Map<T, E>(this Result<T, E> result, Action<T> action)
		{
			return result.Match<Result<E>>(delegate(T success)
			{
				action(success);
				return new Result<E>.Success();
			}, (E error) => new Result<E>.Error(error));
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00033D98 File Offset: 0x00031F98
		public static Result<U, E> Map<T, U, E>(this Result<T, E> result, Func<T, U> mapper)
		{
			return result.Match<Result<U, E>>((T success) => new Result<U, E>.Success(mapper(success)), (E error) => new Result<U, E>.Error(error));
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00033DE4 File Offset: 0x00031FE4
		public static Result<U, E> Map<T1, T2, U, E>(this Result<T1, E> result1, Result<T2, E> result2, Func<T1, T2, U> mapper)
		{
			return result1.Match<Result<U, E>>((T1 success1) => result2.Match<Result<U, E>>((T2 success2) => new Result<U, E>.Success(mapper(success1, success2)), (E error2) => new Result<U, E>.Error(error2)), (E error) => new Result<U, E>.Error(error));
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00033E38 File Offset: 0x00032038
		public static Result<U, E> Map<T1, T2, T3, U, E>(this Result<T1, E> result1, Result<T2, E> result2, Result<T3, E> result3, Func<T1, T2, T3, U> mapper)
		{
			return result1.Match<Result<U, E>>((T1 success1) => result2.Match<Result<U, E>>((T2 success2) => result3.Match<Result<U, E>>((T3 success3) => new Result<U, E>.Success(mapper(success1, success2, success3)), (E error3) => new Result<U, E>.Error(error3)), (E error2) => new Result<U, E>.Error(error2)), (E error) => new Result<U, E>.Error(error));
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00033E94 File Offset: 0x00032094
		public static Result<U, E> Map<T1, T2, T3, T4, T5, T6, T7, T8, U, E>(this Result<T1, E> result1, Result<T2, E> result2, Result<T3, E> result3, Result<T4, E> result4, Result<T5, E> result5, Result<T6, E> result6, Result<T7, E> result7, Result<T8, E> result8, Func<T1, T2, T3, T4, T5, T6, T7, T8, U> mapper)
		{
			return result1.Match<Result<U, E>>((T1 success) => result2.Match<Result<U, E>>((T2 success2) => result3.Match<Result<U, E>>((T3 success3) => result4.Match<Result<U, E>>((T4 success4) => result5.Match<Result<U, E>>((T5 success5) => result6.Match<Result<U, E>>((T6 success6) => result7.Match<Result<U, E>>((T7 success7) => result8.Match<Result<U, E>>((T8 success8) => new Result<U, E>.Success(mapper(success, success2, success3, success4, success5, success6, success7, success8)), (E error8) => new Result<U, E>.Error(error8)), (E error7) => new Result<U, E>.Error(error7)), (E error6) => new Result<U, E>.Error(error6)), (E error5) => new Result<U, E>.Error(error5)), (E error4) => new Result<U, E>.Error(error4)), (E error3) => new Result<U, E>.Error(error3)), (E error2) => new Result<U, E>.Error(error2)), (E error1) => new Result<U, E>.Error(error1));
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00033F18 File Offset: 0x00032118
		public static Result<U, E> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, U, E>(this Result<T1, E> result1, Result<T2, E> result2, Result<T3, E> result3, Result<T4, E> result4, Result<T5, E> result5, Result<T6, E> result6, Result<T7, E> result7, Result<T8, E> result8, Result<T9, E> result9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, U> mapper)
		{
			return result1.Match<Result<U, E>>((T1 success) => result2.Match<Result<U, E>>((T2 success2) => result3.Match<Result<U, E>>((T3 success3) => result4.Match<Result<U, E>>((T4 success4) => result5.Match<Result<U, E>>((T5 success5) => result6.Match<Result<U, E>>((T6 success6) => result7.Match<Result<U, E>>((T7 success7) => result8.Match<Result<U, E>>((T8 success8) => result9.Match<Result<U, E>>((T9 success9) => new Result<U, E>.Success(mapper(success, success2, success3, success4, success5, success6, success7, success8, success9)), (E error9) => new Result<U, E>.Error(error9)), (E error8) => new Result<U, E>.Error(error8)), (E error7) => new Result<U, E>.Error(error7)), (E error6) => new Result<U, E>.Error(error6)), (E error5) => new Result<U, E>.Error(error5)), (E error4) => new Result<U, E>.Error(error4)), (E error3) => new Result<U, E>.Error(error3)), (E error2) => new Result<U, E>.Error(error2)), (E error1) => new Result<U, E>.Error(error1));
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00033FA4 File Offset: 0x000321A4
		public static Result<IEnumerable<T>, E> TraverseResults<T, E>(this IEnumerable<Result<T, E>> results)
		{
			Result<IEnumerable<T>, E> result = new Result<IEnumerable<T>, E>.Success(new List<T>());
			return results.Aggregate(result, (Result<IEnumerable<T>, E> accumulated, Result<T, E> next) => accumulated.Map(next, (IEnumerable<T> oks, T okNext) => oks.Concat(okNext.ToEnumerable<T>())));
		}
	}
}
