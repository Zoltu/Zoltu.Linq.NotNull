using System;
using System.Diagnostics.Contracts;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
	internal class SelectAndSwallowIterator<TSource, TResult, TException> : Iterator<TSource, TResult> where TException : Exception
	{
		private readonly INotNullEnumerable<TSource> _source;
		private readonly Func<TSource, TResult> _predicate;
		private INotNullEnumerator<TSource> _sourceEnumerator;

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(_source != null);
			Contract.Invariant(_predicate != null);
		}

		public SelectAndSwallowIterator(INotNullEnumerable<TSource> source, Func<TSource, TResult> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			_source = source;
			_predicate = predicate;
		}

		public override Boolean MoveNext()
		{
			if (_sourceEnumerator == null)
				_sourceEnumerator = _source.GetEnumerator();

			while (_sourceEnumerator.MoveNext())
			{
				var sourceCurrent = _sourceEnumerator.Current;
				try
				{
					var maybeCurrent = _predicate(sourceCurrent);
					if (maybeCurrent == null)
						continue;
					Current = maybeCurrent;
					return true;
				}
				catch (TException)
				{
					continue;
				}
			}

			Dispose();
			return false;
		}
	}
}
