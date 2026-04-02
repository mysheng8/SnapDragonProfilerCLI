using System;

namespace Sdp.Functional
{
	// Token: 0x02000314 RID: 788
	public abstract class Maybe<T>
	{
		// Token: 0x06001054 RID: 4180
		public abstract TReturn Match<TReturn>(Func<T, TReturn> onSome, Func<TReturn> onNone);

		// Token: 0x06001055 RID: 4181
		public abstract void Match(Action<T> onSome, Action onNone);

		// Token: 0x06001056 RID: 4182 RVA: 0x00033704 File Offset: 0x00031904
		public override string ToString()
		{
			return this.Match<string>((T some) => string.Format("Some({0})", some), () => "None()");
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00033758 File Offset: 0x00031958
		public override bool Equals(object obj)
		{
			Maybe<T> rhs = obj as Maybe<T>;
			return rhs != null && this.Match<bool>((T some) => rhs.Match<bool>((T someRhs) => some.Equals(someRhs), () => false), () => rhs.Match<bool>((T someRhs) => false, () => true));
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0003379F File Offset: 0x0003199F
		public override int GetHashCode()
		{
			return this.Match<int>((T some) => some.GetHashCode(), () => base.GetHashCode());
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x000337D2 File Offset: 0x000319D2
		public bool IsNone()
		{
			return !this.IsSome();
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x000337E0 File Offset: 0x000319E0
		public bool IsSome()
		{
			return this.Match<bool>((T some) => true, () => false);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00033834 File Offset: 0x00031A34
		public void IfSome(Action<T> onSome)
		{
			this.Match(delegate(T some)
			{
				onSome(some);
			}, delegate
			{
			});
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0003387F File Offset: 0x00031A7F
		public Maybe<U> Cast<U>() where U : class
		{
			return this.Map<U>((T maybe) => maybe as U);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x000338A8 File Offset: 0x00031AA8
		public Maybe<U> Map<U>(Func<T, U> mapper)
		{
			return this.Match<Maybe<U>>((T some) => new Maybe<U>.Some(mapper(some)), () => new Maybe<U>.None());
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000338F4 File Offset: 0x00031AF4
		public Maybe<U> Bind<U>(Func<T, Maybe<U>> binder)
		{
			return this.Match<Maybe<U>>((T some) => binder(some), () => new Maybe<U>.None());
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00033940 File Offset: 0x00031B40
		public void Bind(Action<T> binder)
		{
			this.Match(delegate(T some)
			{
				binder(some);
			}, delegate
			{
			});
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0003398C File Offset: 0x00031B8C
		public void Bind2<U>(Maybe<U> option2, Action<T, U> onSome, Action onNone)
		{
			Action <>9__3;
			this.Match(delegate(T some)
			{
				Maybe<U> option3 = option2;
				Action<U> action = delegate(U some2)
				{
					onSome(some, some2);
				};
				Action action2;
				if ((action2 = <>9__3) == null)
				{
					action2 = (<>9__3 = delegate
					{
						onNone();
					});
				}
				option3.Match(action, action2);
			}, delegate
			{
				onNone();
			});
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x000339D4 File Offset: 0x00031BD4
		public V Match2<U, V>(Maybe<U> option2, Func<T, U, V> onSome, Func<V> onNone)
		{
			Func<V> <>9__3;
			return this.Match<V>(delegate(T some)
			{
				Maybe<U> option3 = option2;
				Func<U, V> func = (U some2) => onSome(some, some2);
				Func<V> func2;
				if ((func2 = <>9__3) == null)
				{
					func2 = (<>9__3 = () => onNone());
				}
				return option3.Match<V>(func, func2);
			}, () => onNone());
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00033A1C File Offset: 0x00031C1C
		public T UnwrapOr(T onNone)
		{
			return this.Match<T>((T some) => some, () => onNone);
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00033A68 File Offset: 0x00031C68
		public T Expect(Exception onNone)
		{
			return this.Match<T>((T some) => some, delegate
			{
				throw onNone;
			});
		}

		// Token: 0x0200040C RID: 1036
		public sealed class Some : Maybe<T>
		{
			// Token: 0x0600131A RID: 4890 RVA: 0x0003B920 File Offset: 0x00039B20
			public Some(T value)
			{
				this.m_value = value;
			}

			// Token: 0x0600131B RID: 4891 RVA: 0x0003B92F File Offset: 0x00039B2F
			public override TReturn Match<TReturn>(Func<T, TReturn> onSome, Func<TReturn> onNone)
			{
				return onSome(this.m_value);
			}

			// Token: 0x0600131C RID: 4892 RVA: 0x0003B93D File Offset: 0x00039B3D
			public override void Match(Action<T> onSome, Action onNone)
			{
				onSome(this.m_value);
			}

			// Token: 0x04000E19 RID: 3609
			private readonly T m_value;
		}

		// Token: 0x0200040D RID: 1037
		public sealed class None : Maybe<T>
		{
			// Token: 0x0600131D RID: 4893 RVA: 0x0003B94B File Offset: 0x00039B4B
			public override TReturn Match<TReturn>(Func<T, TReturn> onSome, Func<TReturn> onNone)
			{
				return onNone();
			}

			// Token: 0x0600131E RID: 4894 RVA: 0x0003B953 File Offset: 0x00039B53
			public override void Match(Action<T> onSome, Action onNone)
			{
				onNone();
			}
		}
	}
}
