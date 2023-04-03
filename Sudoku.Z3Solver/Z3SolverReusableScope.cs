using Sudoku.Shared;
using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Z3;
using System.Diagnostics.CodeAnalysis;

namespace Sudoku.Z3Solver
{
    // Exemple de réutilisation en utilisant l'API de scope
    public class Z3SolverReusableScope : Z3SolverBase
    {
        public static Solver ReusableSolver = ctx.MkSolver();
		public Z3SolverReusableScope()
		{
			ReusableSolver.Assert(GenericContraints);
		}
        public override SudokuGrid Solve(SudokuGrid s)
        {
            SudokuGrid solution = new SudokuGrid();

            SudokuSolve(s, ref solution);

            return solution;
        }

        public void SudokuSolve(SudokuGrid grid, ref SudokuGrid solution)
        {

            ReusableSolver.Push();
			BoolExpr instance_c = GetPuzzleConstraints(grid);
            ReusableSolver.Assert(instance_c);
			if (ReusableSolver.Check() == Status.SATISFIABLE)
			{
				Model m = ReusableSolver.Model;
                for (uint i = 0; i < 9; i++)
                {   
                    for (uint j = 0; j < 9; j++)
					    solution.Cells[i][j] = ((IntNum)m.Evaluate(X[i][j])).Int;
                }

                /*
                Console.WriteLine("Sudoku solution:");
                for (uint i = 0; i < 9; i++)
                {
                    for (uint j = 0; j < 9; j++)
                        Console.Write(" " + solution.Cells[i][j]]);
                    Console.WriteLine();
                }
                */
            }
            else
            {
                Console.WriteLine("Failed to solve sudoku");
                throw new Exception("Failed to solve sudoku");
            }
			ReusableSolver.Pop();

        }
    }   
}
