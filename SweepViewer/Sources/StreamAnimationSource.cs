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

			CurrentFrame = new char[Rows][];
			for (int row = 0; row < Rows; row++)
			{
				CurrentFrame[row] = new char[Columns];
			}
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

		public char[][] CurrentFrame { get; }

		/// <inheritdoc />
		public async Task MoveNextFrame()
		{
			for (int row = 0; row < Rows; row++)
			{
				// don't forget about the EOL character
				// ReadLine handles this but doesn't take a buffer...
				await reader.ReadBlockAsync(CurrentFrame[row]);
				// handle the line ending
				reader.Read();
			}

			//reader.ReadBlockAsync(CurrentFrame);
			//await Task.CompletedTask;
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