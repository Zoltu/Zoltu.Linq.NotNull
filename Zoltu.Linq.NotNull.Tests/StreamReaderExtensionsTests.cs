using System.IO;
using System.Text;
using Xunit;

namespace Zoltu.Linq.NotNull.Tests
{
	public class StreamReaderExtensionsTests
	{
		[Fact]
		public void when_stream_is_empty_then_ToLineEnumerable_returns_empty_enumerable()
		{
			// Arrange
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(""));
			var streamReader = new StreamReader(stream, Encoding.UTF8);

			// Act
			var enumerable = streamReader.ToLines();

			// Assert
			Assert.False(enumerable.Any());
		}

		[Fact]
		public void when_stream_has_single_line_then_ToLineEnumerable_returns_one_item_enumerable()
		{
			// Arrange
			var stream = new MemoryStream(Encoding.UTF8.GetBytes("one line"));
			var streamReader = new StreamReader(stream, Encoding.UTF8);

			// Act
			var enumerable = streamReader.ToLines();

			// Assert
			Assert.Equal(1, enumerable.Count());
		}

		[Fact]
		public void when_stream_has_multile_lines_then_ToLineEnumerable_returns_correct_line_count()
		{
			// Arrange
			var inputString =
@"foo
bar
baz";
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputString));
			var streamReader = new StreamReader(stream, Encoding.UTF8);

			// Act
			var enumerable = streamReader.ToLines();

			// Assert
			Assert.Equal(3, enumerable.Count());
		}

		[Fact]
		public void when_stream_has_empty_first_line_then_it_is_counted()
		{
			// Arrange
			var inputString =
@"
bar
baz";
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputString));
			var streamReader = new StreamReader(stream, Encoding.UTF8);

			// Act
			var enumerable = streamReader.ToLines();

			// Assert
			Assert.Equal(3, enumerable.Count());
		}

		[Fact]
		public void when_stream_has_empty_last_line_then_it_is_not_counted()
		{
			// Arrange
			var inputString =
@"foo
bar
";
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputString));
			var streamReader = new StreamReader(stream, Encoding.UTF8);

			// Act
			var enumerable = streamReader.ToLines();

			// Assert
			Assert.Equal(2, enumerable.Count());
		}

		[Fact]
		public void when_stream_has_empty_middle_line_then_it_is_counted()
		{
			// Arrange
			var inputString =
@"foo

baz";
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputString));
			var streamReader = new StreamReader(stream, Encoding.UTF8);

			// Act
			var enumerable = streamReader.ToLines();

			// Assert
			Assert.Equal(3, enumerable.Count());
		}

		[Fact]
		public void when_stream_has_all_empty_lines_then_all_but_last_are_counted()
		{
			// Arrange
			var inputString =
@"

";
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputString));
			var streamReader = new StreamReader(stream, Encoding.UTF8);

			// Act
			var enumerable = streamReader.ToLines();

			// Assert
			Assert.Equal(2, enumerable.Count());
		}

		[Fact]
		public void when_enumerated_multiple_times_then_only_first_enumeration_yields_results()
		{
			// Arrange
			var inputString =
@"foo
bar
baz";
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputString));
			var streamReader = new StreamReader(stream, Encoding.UTF8);

			// Act
			var firstEnumeration = streamReader.ToLines().ToList();
			var secondEnumeration = streamReader.ToLines().ToList();

			// Assert
			Assert.Equal(3, firstEnumeration.Count());
			Assert.Equal(0, secondEnumeration.Count());
		}
	}
}
