﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull
{
	public static partial class NotNullEnumerable
	{
		public static INotNullEnumerable<TResult> Select<TSource, TResult>(this INotNullEnumerable<TSource> source, Func<TSource, TResult> predicate)
		{
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<INotNullEnumerable<TResult>>() != null);

			if (source == null)
				return EmptyEnumerable<TResult>.Instance;

			return new SelectEnumerable<TSource, TResult>(source, predicate);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static INotNullEnumerable<TResult> SelectAndSwallow<TSource, TResult, TException>(this INotNullEnumerable<TSource> source, Func<TSource, TResult> predicate) where TException : Exception
		{
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<INotNullEnumerable<TResult>>() != null);

			if (source == null)
				return EmptyEnumerable<TResult>.Instance;

			return new SelectAndSwallowEnumerable<TSource, TResult, TException>(source, predicate);
		}

		public static INotNullEnumerable<TResult> SelectAndSwallow<TSource, TResult>(this INotNullEnumerable<TSource> source, Func<TSource, TResult> predicate)
		{
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<INotNullEnumerable<TResult>>() != null);

			if (source == null)
				return EmptyEnumerable<TResult>.Instance;

			return new SelectAndSwallowEnumerable<TSource, TResult, Exception>(source, predicate);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static INotNullEnumerable<TResult> SelectMany<TSource, TResult>(this INotNullEnumerable<TSource> source, Func<TSource, INotNullEnumerable<TResult>> selector)
		{
			Contract.Requires(selector != null);
			Contract.Ensures(Contract.Result<INotNullEnumerable<TResult>>() != null);

			if (source == null)
				return EmptyEnumerable<TResult>.Instance;

			return new SelectManyEnumerable<TSource, TResult>(source, selector);
		}

		public static INotNullEnumerable<T> Where<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<INotNullEnumerable<T>>() != null);

			if (source == null)
				return EmptyEnumerable<T>.Instance;

			return new WhereEnumerable<T>(source, predicate);
		}

		public static T FirstOrDefault<T>(this INotNullEnumerable<T> source)
		{
			if (source == null)
				return default(T);

			var enumerator = source.GetEnumerator();
			if (enumerator.MoveNext())
				return enumerator.Current;
			else
				return default(T);
		}

		public static T FirstOrDefault<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires(predicate != null);

			if (source == null)
				return default(T);

			return source
				.Where(predicate)
				.FirstOrDefault();
		}

		public static T First<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires<InvalidOperationException>(source != null, "The source was null.");
			Contract.Ensures(Contract.Result<T>() != null);

			var enumerator = source.GetEnumerator();
			if (enumerator.MoveNext())
				return enumerator.Current;
			else
				throw new InvalidOperationException("The source sequence is empty.");
		}

		public static T First<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires<InvalidOperationException>(source != null, "The source was null.");
			Contract.Ensures(Contract.Result<T>() != null);

			predicate = predicate ?? (x => true);

			return source
				.Where(predicate)
				.First();
		}

		public static T LastOrDefault<T>(this INotNullEnumerable<T> source)
		{
			if (source == null)
				return default (T);

			T result;
			var enumerator = source.GetEnumerator();

			if (enumerator.MoveNext())
				result = enumerator.Current;
			else
				return default(T);

			while (enumerator.MoveNext())
			{
				result = enumerator.Current;
			}

			return result;
		}

		public static T LastOrDefault<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			if (source == null)
				return default (T);

			predicate = predicate ?? (x => true);

			return source
				.Where(predicate)
				.LastOrDefault();
		}

		public static T Last<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires<InvalidOperationException>(source != null, "The source was null.");
			Contract.Ensures(Contract.Result<T>() != null);

			T result;
			var enumerator = source.GetEnumerator();

			if (enumerator.MoveNext())
				result = enumerator.Current;
			else
				throw new InvalidOperationException("The source sequence is empty.");

			while (enumerator.MoveNext())
			{
				result = enumerator.Current;
			}

			return result;
		}

		public static T Last<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires<InvalidOperationException>(source != null, "The source was null.");
			Contract.Ensures(Contract.Result<T>() != null);

			predicate = predicate ?? (x => true);

			return source
				.Where(predicate)
				.Last();
		}

		public static T Single<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires<InvalidOperationException>(source != null, "The source was null.");
			Contract.Ensures(Contract.Result<T>() != null);

			var enumerator = source.GetEnumerator();
			if (!enumerator.MoveNext())
				throw new InvalidOperationException("The source sequence is empty.");

			var result = enumerator.Current;
			if (enumerator.MoveNext())
				throw new InvalidOperationException("The input sequence contains more than one element.");

			return result;
		}

		public static T Single<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires<InvalidOperationException>(source != null, "The source was null.");
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<T>() != null);

			return source
				.Where(predicate)
				.Single();
		}

		public static T SingleOrDefault<T>(this INotNullEnumerable<T> source)
		{
			if (source == null)
				return default(T);

			var enumerator = source.GetEnumerator();
			if (!enumerator.MoveNext())
				return default(T);

			var result = enumerator.Current;
			if (enumerator.MoveNext())
				throw new InvalidOperationException("The input sequence contains more than one element.");

			return result;
		}

		public static T SingleOrDefault<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires(predicate != null);

			if (source == null)
				return default(T);

			return source
				.Where(predicate)
				.SingleOrDefault();
		}

		public static INotNullEnumerable<T> Concat<T>(this INotNullEnumerable<T> first, INotNullEnumerable<T> second)
		{
			Contract.Ensures(Contract.Result<INotNullEnumerable<T>>() != null);

			first = first ?? EmptyEnumerable<T>.Instance;
			second = second ?? EmptyEnumerable<T>.Instance;

			return first
				.NotNullToNull()
				.Concat(second.NotNullToNull())
				.NotNull();
		}

		public static INotNullEnumerable<T> Distinct<T>(this INotNullEnumerable<T> source)
		{
			return source.Distinct(null);
		}

		public static INotNullEnumerable<T> Distinct<T>(this INotNullEnumerable<T> source, IEqualityComparer<T> comparer)
		{
			if (source == null)
				return EmptyEnumerable<T>.Instance;

			return source
				.NotNullToNull()
				.Distinct(comparer)
				.NotNull();
		}

		public static NotNullList<T> ToList<T>(this INotNullEnumerable<T> source)
		{
			Contract.Ensures(Contract.Result<NotNullList<T>>() != null);

			if (source == null)
				return new NotNullList<T>();

			var list = new NotNullList<T>();
			foreach (var item in source)
			{
				list.Add(item);
			}
			return list;
		}

		public static INotNullDictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(this INotNullEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
		{
			Contract.Requires(keySelector != null);
			Contract.Requires(valueSelector != null);
			Contract.Ensures(Contract.Result<INotNullDictionary<TKey, TValue>>() != null);

			if (source == null)
				return new NotNullDictionary<TKey, TValue>();

			var dictionary = new NotNullDictionary<TKey, TValue>();
			foreach (var item in source)
			{
				var key = keySelector(item);
				if (key == null)
					continue;

				var value = valueSelector(item);
				if (value == null)
					continue;

				dictionary.Add(key, value);
			}
			return dictionary;
		}

		public static INotNullDictionary<TKey, TSource> ToDictionary<TSource, TKey>(this INotNullEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			Contract.Requires(keySelector != null);
			Contract.Ensures(Contract.Result<INotNullDictionary<TKey, TSource>>() != null);

			if (source == null)
				return new NotNullDictionary<TKey, TSource>();

			return ToDictionary(source, keySelector, x => x);
		}

		public static INotNullSet<T> ToHashSet<T>(this INotNullEnumerable<T> source)
		{
			Contract.Ensures(Contract.Result<INotNullSet<T>>() != null);

			if (source == null)
				return new NotNullHashSet<T>();

			return new NotNullHashSet<T>(source);
		}

		public static INotNullSet<TKey> ToHashSet<TSource, TKey>(this INotNullEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			Contract.Requires(selector != null);
			Contract.Ensures(Contract.Result<INotNullSet<TKey>>() != null);

			if (source == null)
				return new NotNullHashSet<TKey>();

			return new NotNullHashSet<TKey>(source.Select(selector));
		}

		public static Int32 Count<T>(this INotNullEnumerable<T> source)
		{
			Contract.Ensures(Contract.Result<Int32>() >= 0);

			if (source == null)
				return 0;

			var sourceList = source as NotNullList<T>;
			if (sourceList != null)
				return sourceList.Count;

			var count = 0;
			var sourceEnumerator = source.GetEnumerator();
			while (sourceEnumerator.MoveNext())
			{
				++count;
			}
			return count;
		}

		public static Boolean Any<T>(this INotNullEnumerable<T> source)
		{
			if (source == null)
				return false;

			using (var enumerator = source.GetEnumerator())
			{
				return enumerator.MoveNext();
			}
		}

		/// <summary>
		/// Returns a task that will complete when all of the items in the enumerable have completed.
		/// </summary>
		/// <remarks>This will enumerate and copy the enumerable, be aware of this when using it with very large collections.</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static async Task<INotNullEnumerable<T>> WhenAllAsync<T>(this INotNullEnumerable<Task<T>> source)
		{
			if (source == null)
				return EmptyEnumerable<T>.Instance;

			var results = await Task.WhenAll(source.NotNullToNull());
			return results.NotNull();
		}

		/// <summary>
		/// Blocks until all of the items in the enumerable have completed.
		/// </summary>
		/// <remarks>This will enumerate and copy the enumerable, be aware of this when using it with very large collections.</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static INotNullEnumerable<T> WhenAllSync<T>(this INotNullEnumerable<Task<T>> source)
		{
			if (source == null)
				return EmptyEnumerable<T>.Instance;

			return source.WhenAllAsync().Result;
		}
	}
}
