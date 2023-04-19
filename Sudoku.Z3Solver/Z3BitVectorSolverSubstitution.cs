using Microsoft.Z3;
using Sudoku.Shared;

namespace Sudoku.Z3Solver;

public class Z3BitVectorSolverSubstitution : Z3BitVectorSolverBase
{
	public override SudokuGrid Solve(SudokuGrid s)
	{
		SudokuGrid solution = new SudokuGrid();

		SudokuSolve(s, ref solution);

		return solution;
	}

	public void SudokuSolve(SudokuGrid grid, ref SudokuGrid solution)
	{

		var substExprs = new List<Expr>();
		var substVals = new List<Expr>();

		for (int i = 0; i < 9; i++)
		for (int j = 0; j < 9; j++)
			if (grid.Cells[i][j] != 0)
			{
				substExprs.Add(CellVariables[i][j]);
				substVals.Add(GetConstExpr(grid.Cells[i][j]));
			}

		BoolExpr instance_c = (BoolExpr)GenericContraints.Substitute(substExprs.ToArray(), substVals.ToArray());
		Solver solver = ctx.MkSolver();
		solver.Assert(instance_c);
		if (solver.Check() == Status.SATISFIABLE)
		{
			Model m = solver.Model;
			for (uint i = 0; i < 9; i++)
			{
				for (uint j = 0; j < 9; j++)
				{
					if (grid.Cells[i][j] == 0)
					{
						solution.Cells[i][j] = ((BitVecNum)m.Evaluate(CellVariables[i][j])).Int;
					}
					else
					{
						solution.Cells[i][j] = grid.Cells[i][j];
					}
				}
			}
		}
		else
		{
			Console.WriteLine("Failed to solve sudoku");
			throw new Exception("Failed to solve sudoku");
		}
	}
}