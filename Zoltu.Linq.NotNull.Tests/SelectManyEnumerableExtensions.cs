using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Zoltu.Collections.Generic.NotNull;

namespace Zoltu.Linq.NotNull.Tests
{
	public class SelectManyEnumerableExtensions
	{
		[Fact]
		public void empty()
		{
			var empty2DimensionalArray = new List<List<String>>();

			var enumerable = empty2DimensionalArray
				.NotNull()
				.SelectMany(x => x.NotNull());

			Assert.False(enumerable.Any());
		}

		[Fact]
		public void empty_inner()
		{
			var empty2DimensionalArray = new List<List<String>>
			{
				new List<String>(),
				new List<String>(),
			};

			var enumerable = empty2DimensionalArray
				.NotNull()
				.SelectMany(x => x.NotNull());

			Assert.False(enumerable.Any());
		}

		[Fact]
		public void selector_returns_null()
		{
			var empty2DimensionalArray = new List<List<String>>();

			var enumerable = empty2DimensionalArray
				.NotNull()
				.SelectMany(x => null as INotNullEnumerable<String>);

			Assert.False(enumerable.Any());
		}

		[Fact]
		public void one_item()
		{
			var empty2DimensionalArray = new List<List<String>>
			{
				new List<String>
				{
					"foo"
				}
			};

			var enumerable = empty2DimensionalArray
				.NotNull()
				.SelectMany(x => x.NotNull());

			Assert.Equal("foo", enumerable.First());
		}

		[Fact]
		public void complex()
		{
			var empty2DimensionalArray = new List<List<String>>
			{
				new List<String>(),
				null,
				new List<String>
				{
					"foo",
					"bar",
				},
				null,
				new List<String>
				{
					"zip",
					"zap",
				},
			};

			var enumerable = empty2DimensionalArray
				.NotNull()
				.SelectMany(x => x.NotNull());

			Assert.Equal("foo", enumerable.First());
			Assert.Equal("bar", enumerable.NotNullToNull().ElementAt(1));
			Assert.Equal("zip", enumerable.NotNullToNull().ElementAt(2));
			Assert.Equal("zap", enumerable.NotNullToNull().ElementAt(3));
			Assert.Equal(4, enumerable.NotNullToNull().Count());
		}
	}
}
