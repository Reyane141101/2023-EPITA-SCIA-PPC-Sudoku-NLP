using Sudoku.Shared;
using sudoku.chocosolver;

namespace Sudoku.ChocoSolver;

public class ChocoSolver : ISudokuSolver
{
	public SudokuGrid Solve(SudokuGrid s)
	{
		var toSolve = s.CloneSudoku();
		var javaSolver = new sudoku.chocosolver.SudokuCP(toSolve.Cells);

		Console.WriteLine("Choose a strategy :\n1 - Default\n2 - InputOrderLB\n3 - MinDomLB\n4 - ActivityBased\n5 - DomOverWDeg");
		string strategie = Console.ReadLine();
		
		switch (strategie)
		{
			case "1":
				javaSolver.solve();
				break;
			
			case "2":
				javaSolver.solve();
				break;

			case "3":
				javaSolver.solve();
				break;
			
			case "4":
				javaSolver.solve();
				break;
			
			case "5":
				javaSolver.solve();
				break;

			default:
				Console.WriteLine("Please select a valid strategy.");
				break;
		}

		return toSolve;
	}
}
