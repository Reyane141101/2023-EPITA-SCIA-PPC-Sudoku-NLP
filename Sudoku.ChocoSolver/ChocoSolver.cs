using Sudoku.Shared;
using sudoku.chocosolver;

namespace Sudoku.ChocoSolver;

public abstract class ChocoSolverBase : ISudokuSolver
{
	public SudokuGrid Solve(SudokuGrid s)
	{
		var toSolve = s.CloneSudoku();
		var javaSolver = new sudoku.chocosolver.SudokuCP(toSolve.Cells);
		

		//Console.WriteLine("Choose a strategy :\n1 - Default\n2 - InputOrderLB\n3 - MinDomLB\n4 - ActivityBased\n5 - DomOverWDeg");
		//string strategie = Console.ReadLine();

		int strategie = GetStrategy();
		
		switch (strategie)
		{
			case 1:
				javaSolver.solveDefault();
				break;
			
			case 2:
				javaSolver.solveInputOrderLB();
				break;

			case 3:
				javaSolver.solveMinDomLB();
				break;
			
			case 4:
				javaSolver.solveActivityBased();
				break;
			
			case 5:
				javaSolver.solveDomOverWDeg();
				break;

			default:
				Console.WriteLine("Please select a valid strategy.");
				break;
		}

		return toSolve;
	}

	protected abstract int GetStrategy();
}

public class  ChocoSolverDefault : ChocoSolverBase
{
	protected override int GetStrategy()
	{
		return 1;
	}
}

public class ChocoSolverOrderLB : ChocoSolverBase
{
	protected override int GetStrategy()
	{
		return 2;
	}
}

public class ChocoSolverMinDomLB : ChocoSolverBase
{
	protected override int GetStrategy()
	{
		return 3;
	}
}

public class ChocoSolverActivityBased : ChocoSolverBase
{
	protected override int GetStrategy()
	{
		return 4;
	}
}

public class ChocoSolverDomOverWDeg : ChocoSolverBase
{
	protected override int GetStrategy()
	{
		return 5;
	}
}