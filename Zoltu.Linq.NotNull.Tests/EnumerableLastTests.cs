using System;
using System.Collections.Generic;
using Xunit;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull.Tests
{
	public class EnumerableLastTests
	{
		[Fact]
		public void default_when_reference_sequence_is_null()
		{
			var sequence = null as INotNullEnumerable<Object>;

			var actualResult = sequence.LastOrDefault();

			Assert.Equal(default(Object), actualResult);
		}

		[Fact]
		public void default_when_value_sequence_is_null()
		{
			var sequence = null as INotNullEnumerable<Int32>;

			var actualResult = sequence.LastOrDefault();

			Assert.Equal(default(Int32), actualResult);
		}

		[Fact]
		public void default_when_reference_sequence_with_predicate_is_null()
		{
			var sequence = null as INotNullEnumerable<Object>;

			var actualResult = sequence.LastOrDefault(x => false);

			Assert.Equal(default(Object), actualResult);
		}

		[Fact]
		public void when_reference_sequence_is_null()
		{
			var sequence = null as INotNullEnumerable<Object>;

			Assert.Throws<InvalidOperationException>(() => sequence.Last());
		}

		[Fact]
		public void when_reference_sequence_with_predicate_is_null()
		{
			var sequence = null as INotNullEnumerable<Object>;

			Assert.Throws<InvalidOperationException>(() => sequence.Last(x => false));
		}

		[Fact]
		public void default_when_sequence_is_empty()
		{
			var sequence = EmptyEnumerable<Object>.Instance;

			var actualResult = sequence.LastOrDefault();

			Assert.Equal(default(Object), actualResult);
		}

		[Fact]
		public void default_when_sequence_has_single_item()
		{
			var expectedResult = 5;
			var sequence = new List<Int32> { expectedResult }.NotNull();

			var actualResult = sequence.LastOrDefault();

			Assert.Equal(expectedResult, actualResult);
		}

		[Fact]
		public void default_when_sequence_has_multiple_items()
		{
			var expectedResult = 5;
			var sequence = new List<Int32> { 1, 3, 7, 9, expectedResult }.NotNull();

			var actualResult = sequence.LastOrDefault();

			Assert.Equal(expectedResult, actualResult);
		}

		[Fact]
		public void default_when_sequence_has_multiple_and_predicate()
		{
			var expectedResult = 5;
			var sequence = new List<Int32> { 1, 3, expectedResult, 7, 9 }.NotNull();

			var actualResult = sequence.LastOrDefault(x => x != 7 && x != 9);

			Assert.Equal(expectedResult, actualResult);
		}

		[Fact]
		public void when_sequence_has_multiple_and_predicate()
		{
			var expectedResult = 5;
			var sequence = new List<Int32> { 1, 3, expectedResult, 7, 9 }.NotNull();

			var actualResult = sequence.Last(x => x != 7 && x != 9);

			Assert.Equal(expectedResult, actualResult);
		}
	}
}
