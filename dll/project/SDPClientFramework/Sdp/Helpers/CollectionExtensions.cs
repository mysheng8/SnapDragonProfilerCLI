using System;
using System.Collections.Generic;
using System.Linq;
using Sdp.Functional;

namespace Sdp.Helpers
{
	// Token: 0x02000304 RID: 772
	public static class CollectionExtensions
	{
		// Token: 0x06000FDC RID: 4060 RVA: 0x00030CD8 File Offset: 0x0002EED8
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			return collection.IsNull<T>() || collection.IsEmpty<T>();
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00030CEA File Offset: 0x0002EEEA
		public static bool IsNull<T>(this IEnumerable<T> collection)
		{
			return collection == null;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00030CF0 File Offset: 0x0002EEF0
		public static bool IsEmpty<T>(this IEnumerable<T> collection)
		{
			return collection.Count<T>() == 0;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00030CFB File Offset: 0x0002EEFB
		public static bool IsNotEmpty<T>(this IEnumerable<T> collection)
		{
			return !collection.IsEmpty<T>();
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00030D06 File Offset: 0x0002EF06
		public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			return !collection.IsNullOrEmpty<T>();
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00030D14 File Offset: 0x0002EF14
		public static bool All<T>(this IEnumerable<T> collection, Func<T, T, bool> predicate)
		{
			for (int i = 1; i < collection.Count<T>(); i++)
			{
				T t = collection.ElementAt(i - 1);
				T t2 = collection.ElementAt(i);
				if (!predicate(t2, t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00030D51 File Offset: 0x0002EF51
		public static void FillListUpToIndex<T>(this List<T> list, int index, Func<T> initializeEmptyElement)
		{
			while (index >= list.Count)
			{
				list.Add(initializeEmptyElement());
			}
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00030D6A File Offset: 0x0002EF6A
		public static void MoveElementsTo<T>(this List<T> source, List<T> destination)
		{
			destination.AddRange(source);
			source.Clear();
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x00030D7C File Offset: 0x0002EF7C
		public static Maybe<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			TValue tvalue;
			if (dictionary.TryGetValue(key, out tvalue))
			{
				return new Maybe<TValue>.Some(tvalue);
			}
			return new Maybe<TValue>.None();
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x00030DA0 File Offset: 0x0002EFA0
		public static Maybe<TValue> TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
		{
			TValue tvalue;
			if (dictionary.TryGetValue(key, out tvalue))
			{
				return new Maybe<TValue>.Some(tvalue);
			}
			return new Maybe<TValue>.None();
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00030DC4 File Offset: 0x0002EFC4
		public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue onAdd, Func<TValue, TValue> onUpdate)
		{
			TValue tvalue;
			if (!dictionary.TryGetValue(key, out tvalue))
			{
				dictionary[key] = onAdd;
				return;
			}
			dictionary[key] = onUpdate(tvalue);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00030DF3 File Offset: 0x0002EFF3
		public static IEnumerable<T> ToEnumerable<T>(this T value)
		{
			CollectionExtensions.<ToEnumerable>d__11<T> <ToEnumerable>d__ = new CollectionExtensions.<ToEnumerable>d__11<T>(-2);
			<ToEnumerable>d__.<>3__value = value;
			return <ToEnumerable>d__;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00030E04 File Offset: 0x0002F004
		public static IEnumerable<U> Choose<T, U>(this IEnumerable<T> xs, Func<T, Maybe<U>> chooser) where U : class
		{
			return xs.SelectMany((T x) => chooser(x).Choose<U>());
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00030E30 File Offset: 0x0002F030
		public static IEnumerable<U> ChooseValue<T, U>(this IEnumerable<T> xs, Func<T, Maybe<U>> chooser) where U : struct
		{
			return xs.SelectMany((T x) => chooser(x).ChooseValue<U>());
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00030E5C File Offset: 0x0002F05C
		public static IEnumerable<U> ChooseMany<T, U>(this IEnumerable<T> xs, Func<T, IEnumerable<Maybe<U>>> chooser) where U : class
		{
			return xs.SelectMany((T x) => chooser(x).SelectMany((Maybe<U> maybeX) => maybeX.Choose<U>()));
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00030E88 File Offset: 0x0002F088
		public static IEnumerable<U> ChooseManyValues<T, U>(this IEnumerable<T> xs, Func<T, IEnumerable<Maybe<U>>> chooser) where U : struct
		{
			return xs.SelectMany((T x) => chooser(x).SelectMany((Maybe<U> maybeX) => maybeX.ChooseValue<U>()));
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00030EB4 File Offset: 0x0002F0B4
		public static IEnumerable<T> ChooseMany<T>(this Maybe<IEnumerable<T>> maybes) where T : class
		{
			return maybes.Choose<IEnumerable<T>>().SelectMany((IEnumerable<T> id) => id);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00030EE0 File Offset: 0x0002F0E0
		public static IEnumerable<T> Choose<T>(this Maybe<T> maybe) where T : class
		{
			CollectionExtensions.<Choose>d__17<T> <Choose>d__ = new CollectionExtensions.<Choose>d__17<T>(-2);
			<Choose>d__.<>3__maybe = maybe;
			return <Choose>d__;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00030EF0 File Offset: 0x0002F0F0
		public static IEnumerable<T> ChooseValue<T>(this Maybe<T> maybe) where T : struct
		{
			CollectionExtensions.<ChooseValue>d__18<T> <ChooseValue>d__ = new CollectionExtensions.<ChooseValue>d__18<T>(-2);
			<ChooseValue>d__.<>3__maybe = maybe;
			return <ChooseValue>d__;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00030F00 File Offset: 0x0002F100
		public static void IterI<T>(this IEnumerable<T> xs, CollectionExtensions.IndexedAction<T> indexedAction)
		{
			foreach (CollectionExtensions.IndexedValue<T> indexedValue in xs.Select((T x, int index) => new CollectionExtensions.IndexedValue<T>
			{
				Value = x,
				Index = index
			}))
			{
				indexedAction(indexedValue.Value, indexedValue.Index);
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00030F78 File Offset: 0x0002F178
		public static void Iter<T>(this IEnumerable<T> xs, Action<T> action)
		{
			foreach (T t in xs)
			{
				action(t);
			}
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00030FC0 File Offset: 0x0002F1C0
		public static IEnumerable<int> Range(int start, int end)
		{
			CollectionExtensions.<Range>d__23 <Range>d__ = new CollectionExtensions.<Range>d__23(-2);
			<Range>d__.<>3__start = start;
			<Range>d__.<>3__end = end;
			return <Range>d__;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00030FD7 File Offset: 0x0002F1D7
		public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
		{
			if (count >= 0)
			{
				return source.SkipLastN(count);
			}
			return source;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00030FE6 File Offset: 0x0002F1E6
		private static IEnumerable<T> SkipLastN<T>(this IEnumerable<T> source, int count)
		{
			CollectionExtensions.<SkipLastN>d__25<T> <SkipLastN>d__ = new CollectionExtensions.<SkipLastN>d__25<T>(-2);
			<SkipLastN>d__.<>3__source = source;
			<SkipLastN>d__.<>3__count = count;
			return <SkipLastN>d__;
		}

		// Token: 0x020003F9 RID: 1017
		private struct IndexedValue<T>
		{
			// Token: 0x04000DDF RID: 3551
			public T Value;

			// Token: 0x04000DE0 RID: 3552
			public int Index;
		}

		// Token: 0x020003FA RID: 1018
		// (Invoke) Token: 0x060012CE RID: 4814
		public delegate void IndexedAction<in T>(T value, int index);
	}
}
