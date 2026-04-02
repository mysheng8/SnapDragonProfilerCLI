using System;

namespace Sdp.Functional
{
	// Token: 0x02000318 RID: 792
	public abstract class Result<T, E>
	{
		// Token: 0x06001070 RID: 4208
		public abstract TReturn Match<TReturn>(Func<T, TReturn> onSuccess, Func<E, TReturn> onError);

		// Token: 0x06001071 RID: 4209
		public abstract void Match(Action<T> onSuccess, Action<E> onError);

		// Token: 0x06001072 RID: 4210 RVA: 0x00033BAC File Offset: 0x00031DAC
		public override string ToString()
		{
			return this.Match<string>((T success) => string.Format("Success({0})", success), (E error) => string.Format("Error({0})", error));
		}

		// Token: 0x02000423 RID: 1059
		public sealed class Success : Result<T, E>
		{
			// Token: 0x06001362 RID: 4962 RVA: 0x0003BD56 File Offset: 0x00039F56
			public Success(T value)
			{
				this.m_value = value;
			}

			// Token: 0x06001363 RID: 4963 RVA: 0x0003BD65 File Offset: 0x00039F65
			public override TReturn Match<TReturn>(Func<T, TReturn> onSuccess, Func<E, TReturn> onError)
			{
				return onSuccess(this.m_value);
			}

			// Token: 0x06001364 RID: 4964 RVA: 0x0003BD73 File Offset: 0x00039F73
			public override void Match(Action<T> onSuccess, Action<E> onError)
			{
				onSuccess(this.m_value);
			}

			// Token: 0x04000E4C RID: 3660
			private readonly T m_value;
		}

		// Token: 0x02000424 RID: 1060
		public sealed class Error : Result<T, E>
		{
			// Token: 0x06001365 RID: 4965 RVA: 0x0003BD81 File Offset: 0x00039F81
			public Error(E error)
			{
				this.m_error = error;
			}

			// Token: 0x06001366 RID: 4966 RVA: 0x0003BD90 File Offset: 0x00039F90
			public override TReturn Match<TReturn>(Func<T, TReturn> onSuccess, Func<E, TReturn> onError)
			{
				return onError(this.m_error);
			}

			// Token: 0x06001367 RID: 4967 RVA: 0x0003BD9E File Offset: 0x00039F9E
			public override void Match(Action<T> onSuccess, Action<E> onError)
			{
				onError(this.m_error);
			}

			// Token: 0x04000E4D RID: 3661
			private readonly E m_error;
		}
	}
}
