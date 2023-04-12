using Sudoku.Shared;
using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Z3;
using System.Diagnostics.CodeAnalysis;

namespace Sudoku.Z3Solver
{
    public class Z3SolverSimple : Z3SolverBase
    {
        public override SudokuGrid Solve(SudokuGrid s)
        {
            SudokuGrid solution = new SudokuGrid();

            SudokuSolve(s, ref solution);

            return solution;
        }

        public void SudokuSolve(SudokuGrid grid, ref SudokuGrid solution)
        {
            var sudoku_c = GenericContraints;
            var instance_c = GetPuzzleConstraints(grid);
            Solver s = ctx.MkSolver();
            s.Assert(sudoku_c);
            s.Assert(instance_c);

            if (s.Check() == Status.SATISFIABLE)
            {
                Model m = s.Model;
                for (uint i = 0; i < 9; i++)
                {
                    for (uint j = 0; j < 9; j++)
                    {
                        solution.Cells[i][j] = ((IntNum)m.Evaluate(X[i][j])).Int;
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
}
