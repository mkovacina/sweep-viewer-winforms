using System;
using System.Threading.Tasks;

namespace SweepViewer.Sources
{
	internal interface AnimationSource  : IDisposable
	{
		public int[,] CurrentFrame { get; }
		public Task MoveNextFrame();

		public int Rows { get; }
		public int Columns { get; }
		public int NumberOfAgents { get; }
	}
}