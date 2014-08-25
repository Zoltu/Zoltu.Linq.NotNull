using System;
using System.Diagnostics.Contracts;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull
{
	internal sealed class WhereEnumerable<T> : INotNullEnumerable<T>
	{
		private readonly INotNullEnumerable<T> _source;
		private readonly Func<T, Boolean> _predicate;

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(_source != null);
			Contract.Invariant(_predicate != null);
		}

		public WhereEnumerable(INotNullEnumerable<T> source, Func<T, Boolean> predicate)
		{
			Contract.Requires(predicate != null);

			_source = source ?? EmptyEnumerable<T>.Instance;
			_predicate = predicate;
		}

		public INotNullEnumerator<T> GetEnumerator()
		{
			Contract.Ensures(Contract.Result<INotNullEnumerator<T>>() != null);
			return new Enumerator(_source.GetEnumerator(), _predicate);
		}

		private sealed class Enumerator : INotNullEnumerator<T>
		{
			private readonly INotNullEnumerator<T> _source;
			private readonly Func<T, Boolean> _predicate;
			private T _current;
			private States _state = States.BeforeFirst;

			[ContractInvariantMethod]
			private void ContractInvariants()
			{
				Contract.Invariant(_source != null);
				Contract.Invariant(_predicate != null);
				Contract.Invariant(_current != null || _state != States.Enumerating);
			}

			public Enumerator(INotNullEnumerator<T> source, Func<T, Boolean> predicate)
			{
				Contract.Requires(predicate != null);

				_source = source ?? EmptyEnumerable<T>.Instance;
				_predicate = predicate;
			}

			public T Current
			{
				get
				{
					Contract.Ensures(Contract.Result<T>() != null);
					CheckState();
					return _current;
				}
			}

			public void Dispose()
			{
				_state = States.Disposed;
			}

			public bool MoveNext()
			{
				if (_state == States.AfterLast)
					return false;
				if (_state == States.Disposed)
					return false;

				while (_source.MoveNext())
				{
					_current = _source.Current;
					if (!_predicate(_current))
						continue;

					_state = States.Enumerating;
					return true;
				}

				_state = States.AfterLast;
				return false;
			}

			private void CheckState()
			{
				switch (_state)
				{
					case States.Enumerating:
						return;
					case States.BeforeFirst:
						throw new InvalidOperationException("Attempted to access Current before the first iteration.");
					case States.AfterLast:
						throw new InvalidOperationException("Attempted to access Current after the last iteration.");
					case States.Disposed:
						throw new InvalidOperationException("Attempted to access Current after disposal.");
					default:
						throw new InvalidOperationException("Invalid state detected: " + _state.ToString());
				}
			}

			private enum States
			{
				BeforeFirst,
				Enumerating,
				AfterLast,
				Disposed,
			}
		}
	}
}
