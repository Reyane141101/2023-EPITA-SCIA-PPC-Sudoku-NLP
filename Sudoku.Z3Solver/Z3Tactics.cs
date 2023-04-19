using Microsoft.Z3;
using Sudoku.Shared;

namespace Sudoku.Z3Solver
{
    public class Z3Tactics : Z3BitVectorSolverBase
    {
        public override SudokuGrid Solve(SudokuGrid s)
        {
            SudokuGrid solution = new SudokuGrid();
            SudokuSolve(s, ref solution);
            return solution;
        }

        public void SudokuSolve(SudokuGrid grid, ref SudokuGrid solution)
        {
            Solver solver = ReusableSolver;
            solver.Push();

            BoolExpr puzzleConstraints = GetPuzzleConstraints(grid);
            solver.Assert(puzzleConstraints);

            Tactic tactic = ctx.MkTactic("simplify");
            Goal goal = ctx.MkGoal();
            goal.Assert(ctx.MkAnd(GenericContraints, puzzleConstraints));
            ApplyResult applyResult = tactic.Apply(goal);

            if (applyResult.NumSubgoals > 0)
            {
                Goal newGoal = applyResult.Subgoals[0];
                solver.Assert(newGoal.Formulas);

                if (solver.Check() == Status.SATISFIABLE)
                {
                    Model model = solver.Model;
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            solution.Cells[i][j] = ((BitVecNum)model.Evaluate(CellVariables[i][j])).Int;
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Sudoku puzzle could not be solved.");
                }
            }
            else
            {
                throw new InvalidOperationException("No subgoals were produced by the tactic.");
            }

            solver.Pop();
        }
    }
}