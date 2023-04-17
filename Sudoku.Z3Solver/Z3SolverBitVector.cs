using Microsoft.Z3;
using Sudoku.Shared;

namespace Sudoku.Z3Solver
{
    public class Z3BitVector : ISudokuSolver
    {
        // Solves a Sudoku grid using Z3 Solver.
        public SudokuGrid Solve(SudokuGrid s)
        {
            SudokuGrid solution = new SudokuGrid();

            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                Expr[,] solved = SolveSudoku(ctx, s);

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        solution.Cells[i][j] = int.Parse(solved[i, j].ToString());
                    }
                }
            }
            return solution;
        }

        // Solves the Sudoku grid and returns a 2D array of expressions representing the solution.
        private Expr[,] SolveSudoku(Context ctx, SudokuGrid s)
        {
            Solver solver = ctx.MkSolver();

            DefineConstraints(ctx, solver);
            ApplyInitialValues(ctx, solver, s);

            if (solver.Check() == Status.SATISFIABLE)
            {
                Model model = solver.Model;
                Expr[,] solution = new Expr[9, 9];

                for (int i = 1; i < 10; i++)
                {
                    for (int j = 1; j < 10; j++)
                    {
                        var cell = GetCell(ctx, i, j) as BitVecExpr;
                        solution[i - 1, j - 1] = model.Evaluate(cell);
                    }
                }

                return solution;
            }
            else
            {
                throw new InvalidOperationException("Sudoku puzzle could not be solved.");
            }
        }

        // Defines all the constraints for the Sudoku problem.
        private void DefineConstraints(Context ctx, Solver solver)
        {
            WriteNumDef(ctx, solver);
            WriteConstDef(ctx, solver);
            WriteBounds(ctx, solver);
            WriteHUnique(ctx, solver);
            WriteVUnique(ctx, solver);
            WriteSUnique(ctx, solver);
        }

        // Applies the initial values from the Sudoku grid to the solver.
        private void ApplyInitialValues(Context ctx, Solver solver, SudokuGrid s)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int value = s.Cells[i][j];

                    if (value >= 1 && value <= 9)
                    {
                        var cell = GetCell(ctx, i + 1, j + 1) as BitVecExpr;
                        solver.Assert(ctx.MkEq(cell, ctx.MkNumeral(value, ctx.MkBitVecSort(4))));
                    }
                }
            }
        }

        // Defines the numerical values in the context and solver.
        static void WriteNumDef(Context ctx, Solver solver)
        {
            for (int i = 1; i < 10; i++)
            {
                ctx.MkConst($"n{i}", ctx.MkBitVecSort(4));
                ctx.MkNumeral(i, ctx.MkBitVecSort(4));
            }
        }

        // Defines the constants for each cell in the context and solver.
        static void WriteConstDef(Context ctx, Solver solver)
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    ctx.MkConst($"f{i}{j}", ctx.MkBitVecSort(4));
                }
            }
        }

        // Returns the expression for a cell given its row and column.
        private Expr GetCell(Context ctx, int i, int j)
        {
            return ctx.MkConst($"f{i}{j}", ctx.MkBitVecSort(4));
        }

        // Defines the constraints that ensure each cell has a value between 1 and 9.
        private void WriteBounds(Context ctx, Solver solver)
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    var cell = ctx.MkConst($"f{i}{j}", ctx.MkBitVecSort(4));
                    solver.Assert(ctx.MkBVULE((BitVecExpr)ctx.MkNumeral(1, ctx.MkBitVecSort(4)), (BitVecExpr)cell));
                    solver.Assert(ctx.MkBVULE((BitVecExpr)cell, (BitVecExpr)ctx.MkNumeral(9, ctx.MkBitVecSort(4))));
                }
            }
        }

        // Defines the constraints that ensure each row has unique values.
        private void WriteHUnique(Context ctx, Solver solver)
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j1 = 1; j1 < 10; j1++)
                {
                    for (int j2 = j1 + 1; j2 < 10; j2++)
                    {
                        var cell1 = GetCell(ctx, i, j1);
                        var cell2 = GetCell(ctx, i, j2);
                        solver.Assert(ctx.MkNot(ctx.MkEq(cell1, cell2)));
                    }
                }
            }
        }

        // Defines the constraints that ensure each column has unique values.
        private void WriteVUnique(Context ctx, Solver solver)
        {
            for (int j = 1; j < 10; j++)
            {
                for (int i1 = 1; i1 < 10; i1++)
                {
                    for (int i2 = i1 + 1; i2 < 10; i2++)
                    {
                        var cell1 = GetCell(ctx, i1, j);
                        var cell2 = GetCell(ctx, i2, j);
                        solver.Assert(ctx.MkNot(ctx.MkEq(cell1, cell2)));
                    }
                }
            }
        }

        // Defines the constraints that ensure each 3x3 subgrid has unique values.
        private void WriteSUnique(Context ctx, Solver solver)
        {
            for (int m = 0; m < 3; m++)
            {
                for (int n = 0; n < 3; n++)
                {
                    Expr[] square = new Expr[9];
                    int idx = 0;
                    for (int i = 3 * m + 1; i < 3 * m + 4; i++)
                    {
                        for (int j = 3 * n + 1; j < 3 * n + 4; j++)
                        {
                            square[idx++] = GetCell(ctx, i, j);
                        }
                    }
                    solver.Assert(ctx.MkDistinct(square));
                }
            }
        }
    }
}
