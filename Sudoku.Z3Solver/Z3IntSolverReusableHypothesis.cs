using Sudoku.Shared;
using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Z3;
using System.Diagnostics.CodeAnalysis;

namespace Sudoku.Z3Solver
{

	// Exemple de réutilisation en utilisant l'API d'hypothèses (assumptions)


	// Exemple de réutilisation en utilisant l'API d'hypothèses (assumptions)
	public class Z3IntSolverReusableHypothesis: Z3IntSolverBase
	{
		public override SudokuGrid Solve(SudokuGrid s)
		{
            SudokuGrid solution = new SudokuGrid();

            SudokuSolve(s, ref solution);

            return solution;
        }

		public void SudokuSolve(SudokuGrid grid, ref SudokuGrid solution)
		{
            var solver = ReusableSolver;
			BoolExpr instance_c = GetPuzzleConstraints(grid);
			if (solver.Check(instance_c) == Status.SATISFIABLE)
			{
				Model m = solver.Model;
                for (uint i = 0; i < 9; i++)
                {   
                    for (uint j = 0; j < 9; j++)
					    solution.Cells[i][j] = ((IntNum)m.Evaluate(CellVariables[i][j])).Int;
                }
            }
            else
            {
                Console.WriteLine("Failed to solve sudoku");
                throw new Exception("Failed to solve sudoku");
            }
		}
    }
}

