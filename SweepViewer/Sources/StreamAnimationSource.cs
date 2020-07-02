using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SweepViewer.Sources
{
	internal class StreamAnimationSource : AnimationSource
	{
		private readonly StreamReader reader;

		public StreamAnimationSource(Stream stream)
		{
			reader = new StreamReader(stream);

			initializeFromHeader();

			CurrentFrame = new int[Rows,Columns];
		}

		private void initializeFromHeader()
		{
			var dimensionsLine = reader.ReadLine();
			var dimensionsText = dimensionsLine?.Split(" ");

			if (dimensionsText?.Length != 2)
			{
				throw new InvalidFileFormatException("incorrect row and column specification");
			}

			Rows = Int32.Parse(dimensionsText[0]);
			Columns = Int32.Parse(dimensionsText[1]);
			
			var numberOfAgentsLine = reader.ReadLine();

			if (numberOfAgentsLine == null)
			{
				throw new InvalidFileFormatException("incorrect number of agents specification");
			}

			NumberOfAgents = Int32.Parse(numberOfAgentsLine);
		}

		public int[,] CurrentFrame { get; }

		/// <inheritdoc />
		public async Task MoveNextFrame()
		{
			await Task.CompletedTask;
		}

		public int Rows { get; private set; }
		public int Columns { get; private set; }
		public int NumberOfAgents { get; private set; }

		public void Dispose()
		{
			reader.Dispose();
		}
	}

	internal sealed class InvalidFileFormatException : Exception
	{
		public InvalidFileFormatException(string incorrectRowAndColumnSpecification)
		{
			throw new NotImplementedException();
		}
	}
}