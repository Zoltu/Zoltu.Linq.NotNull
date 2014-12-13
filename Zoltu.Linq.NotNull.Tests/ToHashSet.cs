using System;
using Xunit;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull.Tests
{
	public class ToHashSet
	{
		[Fact]
		public void when_enumerable_is_null_then_returns_empty_set()
		{
			var source = EmptyEnumerable<String>.Instance;
			var result = source.ToHashSet();

			Assert.False(result.Any());
		}

		[Fact]
		public void when_enumerable_contains_four_unique_items_then_returns_set_of_four()
		{
			var source = new[] { "foo", "bar", "zip", "zap"};
			var result = source.NotNull().ToHashSet();

			Assert.Equal(4, result.Count);
		}

		[Fact]
		public void when_enumerable_contains_duplicate_items_then_returns_unique_set()
		{
			var source = new[] { "foo", "bar", "foo", "bar"};
			var result = source.NotNull().ToHashSet();

			Assert.Equal(2, result.Count);
		}

		[Fact]
		public void when_selector_is_provided_then_it_is_used()
		{
			var source = new[] { "foo", "bar", "zip", "zap"};
			var result = source.NotNull().ToHashSet(x => x + "baz");

			Assert.Equal(4, result.Count);
			Assert.True(result.Contains("foobaz"));
		}

		[Fact]
		public void when_source_is_null_then_returns_empty_set()
		{
			var source = null as INotNullEnumerable<String>;
			var result = source.ToHashSet();

			Assert.NotNull(result);
			Assert.False(result.Any());
		}
	}
}
