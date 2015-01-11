using System;
using System.Collections.Generic;
using Xunit;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull.Tests
{
	public class EnumerableConcatTests
	{
		[Fact]
		public void both_null()
		{
			INotNullEnumerable<String> first = null;
			INotNullEnumerable<String> second = null;

			var combined = first.Concat(second);

			Assert.NotNull(combined);
			Assert.False(combined.Any());
		}

		[Fact]
		public void first_null()
		{
			INotNullEnumerable<String> first = null;
			var second = new List<String> { "foo" }.NotNull();

			var combined = first.Concat(second).ToList();

			Assert.NotNull(combined);
			Assert.Equal(1, combined.Count);
			Assert.Equal("foo", combined.First());
		}

		[Fact]
		public void second_null()
		{
			var first = new List<String> { "foo" }.NotNull();
			INotNullEnumerable<String> second = null;

			var combined = first.Concat(second).ToList();

			Assert.NotNull(combined);
			Assert.Equal(1, combined.Count);
			Assert.Equal("foo", combined.First());
		}

		[Fact]
		public void one_item_each()
		{
			var first = new List<String> { "foo" }.NotNull();
			var second = new List<String> { "bar" }.NotNull();

			var combined = first.Concat(second).ToList();

			Assert.NotNull(combined);
			Assert.Equal(2, combined.Count);
			Assert.Equal("foo", combined.First());
			Assert.Equal("bar", combined.Last());
		}

		[Fact]
		public void two_items_each()
		{
			var first = new List<String> { "foo", "bar" }.NotNull();
			var second = new List<String> { "zip", "zap" }.NotNull();

			var combined = first.Concat(second).ToList();

			Assert.NotNull(combined);
			Assert.Equal(4, combined.Count);
			Assert.Equal("foo", combined[0]);
			Assert.Equal("bar", combined[1]);
			Assert.Equal("zip", combined[2]);
			Assert.Equal("zap", combined[3]);
		}
	}
}
