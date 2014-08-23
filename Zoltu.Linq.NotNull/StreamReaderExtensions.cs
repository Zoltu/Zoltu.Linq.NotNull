using System;
using System.Diagnostics.Contracts;
using System.IO;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull
{
	public class StreamReaderLineEnumerable : INotNullEnumerable<String>
	{
		private readonly StreamReader _streamReader;

		public StreamReaderLineEnumerable(StreamReader streamReader)
		{
			_streamReader = streamReader;
		}

		public INotNullEnumerator<String> GetEnumerator()
		{
			return new StreamReaderLineEnumerator(_streamReader);
		}
	}

	public sealed class StreamReaderLineEnumerator : INotNullEnumerator<String>
	{
		private String _current;
		private readonly StreamReader _streamReader;
		private States _state = States.BeforeFirst;

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(_streamReader != null);
			Contract.Invariant(_current != null || _state != States.Enumerating);
		}

		public StreamReaderLineEnumerator(StreamReader streamReader)
		{
			if (streamReader == null)
				streamReader = StreamReader.Null;

			_streamReader = streamReader;
		}

		public String Current
		{
			get
			{
				Contract.Ensures(Contract.Result<String>() != null);
				CheckState();
				return _current;
			}
		}

		public void Dispose()
		{
			_state = States.Disposed;
		}

		public Boolean MoveNext()
		{
			if (_state == States.AfterLast)
				return false;
			if (_state == States.Disposed)
				return false;

			_current = _streamReader.ReadLine();
			if (_current == null)
			{
				_state = States.AfterLast;
				return false;
			}

			_state = States.Enumerating;
			return true;
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

	public static partial class StreamReaderExtensions
	{
		/// <summary>
		/// Gets a NotNullEnumerable of all of the lines in the StreamReader as obtained by StreamReader.ReadLine.
		/// </summary>
		/// <remarks>The stream reader will not be accessed until something iterates over the enumerable.  Make sure the stream reader isn't disposed until after iteration occurs.  Also be aware that only the first enumeration of the StreamReader will yield results.  Multiple enumerations will result in all others returning no results.</remarks>
		/// <returns>A stream of all the lines in the StreamReader.</returns>
		public static INotNullEnumerable<String> ToLines(this StreamReader source)
		{
			Contract.Ensures(Contract.Result<INotNullEnumerable<String>>() != null);

			return new StreamReaderLineEnumerable(source);
		}
	}
}
