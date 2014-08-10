using System;
using System.Diagnostics.Contracts;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull
{
	internal class WhereIterator<T> : Iterator<T, T>
	{
		private readonly INotNullEnumerable<T> _source;
		private readonly Func<T, Boolean> _predicate;
		private INotNullEnumerator<T> _sourceEnumerator;

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(_source != null);
			Contract.Invariant(_predicate != null);
		}

		public WhereIterator(INotNullEnumerable<T> source, Func<T, Boolean> predicate)
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
				var _sourceCurrent = _sourceEnumerator.Current;
				if (_predicate(_sourceCurrent))
				{
					Current = _sourceCurrent;
					return true;
				}
			}

			Dispose();
			return false;
		}
	}
}
