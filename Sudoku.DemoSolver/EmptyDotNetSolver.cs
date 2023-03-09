using Sudoku.Shared;

namespace Sudoku.DemoSolver
{
	public class EmptyDotNetSolver:ISudokuSolver
	{
		public SudokuGrid Solve(SudokuGrid s)
		{
			return s;
		}
	}
}