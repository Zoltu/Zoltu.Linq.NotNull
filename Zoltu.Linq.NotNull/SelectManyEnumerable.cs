using System;
using System.Diagnostics.Contracts;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull
{
	internal sealed class SelectManyEnumerable<TSource, TResult> : INotNullEnumerable<TResult>
	{
		private readonly INotNullEnumerable<TSource> _source;
		private readonly Func<TSource, INotNullEnumerable<TResult>> _selector;

		[ContractInvariantMethod]
		private void ContractInvariants()
		{
			Contract.Invariant(_source != null);
			Contract.Invariant(_selector != null);
		}

		public SelectManyEnumerable(INotNullEnumerable<TSource> source, Func<TSource, INotNullEnumerable<TResult>> selector)
		{
			Contract.Requires(selector != null);

			_source = source ?? EmptyEnumerable<TSource>.Instance;
			_selector = selector;
		}

		public INotNullEnumerator<TResult> GetEnumerator()
		{
			Contract.Ensures(Contract.Result<INotNullEnumerator<TResult>>() != null);
			return new Enumerator(_source.GetEnumerator(), _selector);
		}

		private sealed class Enumerator : INotNullEnumerator<TResult>
		{
			private readonly INotNullEnumerator<TSource> _source;
			private readonly Func<TSource, INotNullEnumerable<TResult>> _selector;
			private INotNullEnumerator<TResult> _currentInnerEnumerable;
			private TResult _current;
			private States _state = States.BeforeFirst;

			[ContractInvariantMethod]
			private void ContractInvariants()
			{
				Contract.Invariant(_source != null);
				Contract.Invariant(_selector != null);
				Contract.Invariant(_current != null || _state != States.Enumerating);
			}

			public Enumerator(INotNullEnumerator<TSource> source, Func<TSource, INotNullEnumerable<TResult>> selector)
			{
				Contract.Requires(selector != null);

				_source = source ?? EmptyEnumerable<TSource>.Instance;
				_selector = selector;
			}

			public TResult Current
			{
				get
				{
					Contract.Ensures(Contract.Result<TResult>() != null);
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

				for (;;)
				{
					if (_currentInnerEnumerable == null)
					{
						if (!_source.MoveNext())
							break;

						var outerCurrent = _selector(_source.Current);
						if (outerCurrent == null)
							continue;

						_currentInnerEnumerable = outerCurrent.GetEnumerator();
					}

					while (_currentInnerEnumerable.MoveNext())
					{
						_current = _currentInnerEnumerable.Current;
						if (_current == null)
							continue;

						_state = States.Enumerating;
						return true;
					}

					_currentInnerEnumerable = null;
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
