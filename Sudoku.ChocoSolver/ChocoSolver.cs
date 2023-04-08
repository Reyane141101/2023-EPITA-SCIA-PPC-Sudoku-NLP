using Sudoku.Shared;
using sudoku.chocosolver;

namespace Sudoku.ChocoSolver;

public class ChocoSolver : ISudokuSolver
{
	public SudokuGrid Solve(SudokuGrid s)
	{
		var toSolve = s.CloneSudoku();

		var javaSolver = new sudoku.chocosolver.SudokuCP(toSolve.Cells);

		javaSolver.solve();

		return toSolve;
	}
}
