using Sudoku.Shared;
using Google.OrTools.Sat;

namespace Sudoku.OrTools
{
    // 2 SAT solver
    public class OrToolsSatSolver : ISudokuSolver
    {
        private CpModel model;

        public SudokuGrid Solve(SudokuGrid s)
        {
            model = new CpModel();

            var variables = createVariables(s);
            createConstraints(variables, s);

            if (!solveModel(s, variables))
            {
                Console.WriteLine("Cannot solve problem");
            }

            return s;
        }

        private BoolVar[,,] createVariables(SudokuGrid s)
        {
            // height, width, number
            var variables = new BoolVar[9, 9, 9];
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    for (int k = 0; k < 9; ++k)
                    {
                        variables[i, j, k] = model.NewBoolVar($"v{i}{j}{k}");
                    }
                }
            }

            return variables;
        }

        private void createConstraints(BoolVar[,,] variables, SudokuGrid s)
        {
            // Unfree cells
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    if (s.Cells[i][j] != 0)
                    {
                        // Unfree cell, all false except the target one
                        var exprs = new ILiteral[9];
                        for (int k = 0; k < 9; ++k)
                        {
                            exprs[k] = variables[i, j, k];
                            if (k + 1 != s.Cells[i][j])
                            {
                                exprs[k] = exprs[k].Not();
                            }
                        }

                        model.AddBoolAnd(exprs);
                    }
                }
            }

            // At least one number per cell
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    var exprs = new ILiteral[9];
                    for (int k = 0; k < 9; ++k)
                    {
                        exprs[k] = variables[i, j, k];
                    }

                    model.AddBoolOr(exprs);
                }
            }

            // Max one number per cell
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    for (int u = 0; u < 9; ++u)
                    {
                        for (int v = u + 1; v < 9; ++v)
                        {
                            model.AddBoolOr(new[] { variables[i, j, u].Not(), variables[i, j, v].Not() });
                        }
                    }
                }
            }

            // Columns
            for (int j = 0; j < 9; ++j)
            {
                for (int i1 = 0; i1 < 9; ++i1)
                {
                    for (int i2 = 0; i2 < 9; ++i2)
                    {
                        if (i1 == i2) continue;

                        // (i1, j) must be != to (i2, j)
                        for (int k = 0; k < 9; ++k)
                        {
                            // not ( (i1, j, k) and (i2, j, k) )
                            model.AddBoolOr(new[] { variables[i1, j, k].Not(), variables[i2, j, k].Not() });
                        }
                    }
                }
            }

            // Rows
            for (int i = 0; i < 9; ++i)
            {
                for (int j1 = 0; j1 < 9; ++j1)
                {
                    for (int j2 = 0; j2 < 9; ++j2)
                    {
                        if (j1 == j2) continue;

                        for (int k = 0; k < 9; ++k)
                        {
                            model.AddBoolOr(new[] { variables[i, j1, k].Not(), variables[i, j2, k].Not() });
                        }
                    }
                }
            }

            // Squares
            for (int i = 0; i < 9; i += 3)
            {
                for (int j = 0; j < 9; j += 3)
                {
                    for (int di1 = 0; di1 < 3; ++di1)
                    {
                        for (int dj1 = 0; dj1 < 3; ++dj1)
                        {
                            for (int di2 = 0; di2 < 3; ++di2)
                            {
                                for (int dj2 = 0; dj2 < 3; ++dj2)
                                {
                                    int ifirst = i + di1;
                                    int isecond = i + di2;
                                    int jfirst = j + dj1;
                                    int jsecond = j + dj2;
                                    if (!(ifirst == isecond && jfirst == jsecond))
                                    {
                                        // (ifirst, jfirst) != (isecond, jsecond)
                                        for (int k = 0; k < 9; ++k)
                                        {
                                            model.AddBoolOr(new[]
                                            {
                                                variables[ifirst, jfirst, k].Not(),
                                                variables[isecond, jsecond, k].Not()
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool solveModel(SudokuGrid s, BoolVar[,,] variables)
        {
            var solver = new CpSolver();
            var status = solver.Solve(model);

            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                for (int i = 0; i < 9; ++i)
                {
                    for (int j = 0; j < 9; ++j)
                    {
                        for (int k = 0; k < 9; ++k)
                        {
                            if (solver.Value(variables[i, j, k]) != 0)
                                s.Cells[i][j] = k + 1;
                        }

                        if (s.Cells[i][j] == 0) return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}