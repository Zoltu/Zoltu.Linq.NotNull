using System;
using System.Diagnostics.Contracts;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull
{
	internal abstract class Iterator<TSource, TResult> : INotNullEnumerable<TResult>, INotNullEnumerator<TResult>
	{
		public virtual TResult Current
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
		private TResult _current;

		protected virtual void Dispose(Boolean disposing)
		{
			Current = default(TResult);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual INotNullEnumerator<TResult> GetEnumerator()
		{
			return this;
		}

		public abstract Boolean MoveNext();
	}
}
