using System;

namespace Sdp.Functional
{
	// Token: 0x02000317 RID: 791
	public abstract class Result<E>
	{
		// Token: 0x0600106C RID: 4204
		public abstract void Match(Action onSuccess, Action<E> onError);

		// Token: 0x0600106D RID: 4205
		public abstract T Match<T>(Func<T> onSuccess, Func<E, T> onError);

		// Token: 0x0600106E RID: 4206 RVA: 0x00033B58 File Offset: 0x00031D58
		public override string ToString()
		{
			return this.Match<string>(() => "(Success)", (E error) => string.Format("Error({0})", error));
		}

		// Token: 0x02000420 RID: 1056
		public sealed class Success : Result<E>
		{
			// Token: 0x06001358 RID: 4952 RVA: 0x0003BCEE File Offset: 0x00039EEE
			public override void Match(Action onSuccess, Action<E> onError)
			{
				onSuccess();
			}

			// Token: 0x06001359 RID: 4953 RVA: 0x0003BCF6 File Offset: 0x00039EF6
			public override T Match<T>(Func<T> onSuccess, Func<E, T> onError)
			{
				return onSuccess();
			}
		}

		// Token: 0x02000421 RID: 1057
		public sealed class Error : Result<E>
		{
			// Token: 0x0600135B RID: 4955 RVA: 0x0003BD06 File Offset: 0x00039F06
			public Error(E error)
			{
				this.m_error = error;
			}

			// Token: 0x0600135C RID: 4956 RVA: 0x0003BD15 File Offset: 0x00039F15
			public override void Match(Action onSuccess, Action<E> onError)
			{
				onError(this.m_error);
			}

			// Token: 0x0600135D RID: 4957 RVA: 0x0003BD23 File Offset: 0x00039F23
			public override T Match<T>(Func<T> onSuccess, Func<E, T> onError)
			{
				return onError(this.m_error);
			}

			// Token: 0x04000E48 RID: 3656
			private readonly E m_error;
		}
	}
}
