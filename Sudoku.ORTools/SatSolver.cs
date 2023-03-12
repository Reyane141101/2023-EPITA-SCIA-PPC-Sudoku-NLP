using Sudoku.Shared;
using Google.OrTools.Sat;

namespace Sudoku.OrTools
{
    public class SatSolver : ISudokuSolver
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
            var variables = new BoolVar[9,9,9];
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
                            model.AddBoolOr(new []{variables[i, j, u].Not(), variables[i, j, v].Not()});
                        }
                    }
                }
            }

            // TODO
            // Column
            // Row
            // Square
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