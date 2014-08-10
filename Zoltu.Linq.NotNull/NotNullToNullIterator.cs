using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull
{
	internal class NotNullToNullIterator<T> : IEnumerable<T>, IEnumerator<T>
	{
		private readonly INotNullEnumerable<T> _source;
		private INotNullEnumerator<T> _sourceEnumerator;

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(_source != null);
		}

		public NotNullToNullIterator(INotNullEnumerable<T> source)
		{
			Contract.Requires(source != null);

			_source = source;
		}

		public virtual T Current
		{
			get
			{
				if (_current == null)
					throw new InvalidOperationException("Attempted to access enumerator's current value before the first iteration or after the last iteration.");
				return _current;
			}
			protected set
			{
				_current = value;
			}
		}
		private T _current;

		Object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

		protected virtual void Dispose(Boolean disposing)
		{
			Current = default(T);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Boolean MoveNext()
		{
			if (_sourceEnumerator == null)
				_sourceEnumerator = _source.GetEnumerator();

			if (!_sourceEnumerator.MoveNext())
				return false;

			_current = _sourceEnumerator.Current;
			return true;
			
		}

		public void Reset()
		{
			_sourceEnumerator = _source.GetEnumerator();
		}

	}
}
