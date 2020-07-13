using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SweepViewer.Sources;

namespace SweepViewer.Tests
{
	public class StreamAnimationSourceTests
	{
		private const int NumberOfRows = 2;
		private const int NumberOfColumns = 3;
		private const int NumberOfAgents = 4;

		private readonly byte[] AnimationData =
			Encoding.UTF8.GetBytes($"{NumberOfRows} {NumberOfColumns}\n{NumberOfAgents}\n123\n456\nGARBAGE");

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

			source.CurrentFrame.Length.Should().Be(NumberOfRows);
			foreach(var row in source.CurrentFrame)
			{
				row.Length.Should().Be(NumberOfColumns);
			}
		}

		[Test]
		public async Task CanReadTheFirstFrame()
		{
			await using var stream = new MemoryStream(AnimationData);
			using var source = new StreamAnimationSource(stream);

			await source.MoveNextFrame();

			char counter = '1';

			for (int row = 0; row < NumberOfRows; row++)
			{
				for (int col = 0; col < NumberOfColumns; col++)
				{
					source.CurrentFrame[row][col].Should().Be(counter);
					counter++;
				}
			}
		}
	}
}