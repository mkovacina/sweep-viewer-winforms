using System;
using System.Threading.Tasks;

namespace SweepViewer.Sources
{
	internal interface AnimationSource  : IDisposable
	{
		public char[][] CurrentFrame { get; }

		// this would be nicer in some respects, but 
		// if the frame is a jagged array then reading in 
		// from the file requires less buffer copying
		//public char[,] CurrentFrame { get; }

		public Task MoveNextFrame();

		public int Rows { get; }
		public int Columns { get; }
		public int NumberOfAgents { get; }
	}
}