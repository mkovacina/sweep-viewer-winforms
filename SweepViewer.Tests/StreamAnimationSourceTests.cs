using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using SweepViewer.Sources;

namespace SweepViewer.Tests
{
	public class StreamAnimationSourceTests
	{
		private const int NumberOfRows = 100;
		private const int NumberOfColumns = 200;
		private const int NumberOfAgents = 300;

		private readonly byte[] AnimationData =
			Encoding.UTF8.GetBytes($"{NumberOfRows} {NumberOfColumns}\n{NumberOfAgents}\nGARBAGE");

		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void CanReadGridSize()
		{
			using var stream = new MemoryStream(AnimationData);
			using var source = new StreamAnimationSource(stream);

			source.Rows.Should().Be(NumberOfRows);
			source.Columns.Should().Be(NumberOfColumns);
		}

		[Test]
		public void CanReadNumberOfAgents()
		{
			using var stream = new MemoryStream(AnimationData);
			using var source = new StreamAnimationSource(stream);

			source.NumberOfAgents.Should().Be(NumberOfAgents);
		}

		[Test]
		public void GridIsInitializedToTheProperSize()
		{
			using var stream = new MemoryStream(AnimationData);
			using var source = new StreamAnimationSource(stream);

			var expectedLength = NumberOfRows * NumberOfColumns;
			source.CurrentFrame.Length.Should().Be(expectedLength);

			var expectedRank = 2;
			source.CurrentFrame.Rank.Should().Be(expectedRank);
		}

	}
}