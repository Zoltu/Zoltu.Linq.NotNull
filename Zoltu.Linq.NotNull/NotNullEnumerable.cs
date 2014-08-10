using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull
{
	public static partial class NotNullEnumerable
	{
		public static INotNullEnumerable<TResult> Select<TSource, TResult>(this INotNullEnumerable<TSource> source, Func<TSource, TResult> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return new SelectIterator<TSource, TResult>(source, predicate);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static INotNullEnumerable<TResult> SelectAndSwallow<TSource, TResult, TException>(this INotNullEnumerable<TSource> source, Func<TSource, TResult> predicate) where TException : Exception
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return new SelectAndSwallowIterator<TSource, TResult, TException>(source, predicate);
		}

		public static INotNullEnumerable<TResult> SelectAndSwallow<TSource, TResult>(this INotNullEnumerable<TSource> source, Func<TSource, TResult> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return new SelectAndSwallowIterator<TSource, TResult, Exception>(source, predicate);
		}

		public static INotNullEnumerable<T> Where<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return new WhereIterator<T>(source, predicate);
		}

		public static T FirstOrDefault<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires(source != null);

			var enumerator = source.GetEnumerator();
			if (enumerator.MoveNext())
				return enumerator.Current;
			else
				return default(T);
		}

		public static T FirstOrDefault<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return source
				.Where(predicate)
				.FirstOrDefault();
		}

		public static T First<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires(source != null);

			var enumerator = source.GetEnumerator();
			if (enumerator.MoveNext())
				return enumerator.Current;
			else
				throw new InvalidOperationException("The source sequence is empty.");
		}

		public static T First<T>(this INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return source
				.Where(predicate)
				.First();
		}

		public static T Single<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires(source != null);

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
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return source
				.Where(predicate)
				.Single();
		}

		public static T SingleOrDefault<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires(source != null);

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
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return source
				.Where(predicate)
				.SingleOrDefault();
		}

		public static IEnumerable<T> NotNullToNull<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires(source != null);

			return new NotNullToNullIterator<T>(source);
		}

		public static NotNullList<T> ToList<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires(source != null);

			var list = new NotNullList<T>();
			foreach (var item in source)
			{
				list.Add(item);
			}
			return list;
		}

		public static Int32 Count<T>(this INotNullEnumerable<T> source)
		{
			Contract.Requires(source != null);

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

	}
}
