using Sudoku.Shared;

namespace ClassLibrary1;
using Google.OrTools.ConstraintSolver;

public class Sudoku_Solver_OR_TOOLS_origin : ISudokuSolver
{
    public IntVar[][] grid;
    public SudokuGrid Solve(SudokuGrid s)
    {
        Solver solver = new Solver("CpSimple");
        DecisionBuilder db = SetUpDecisionBuilder(s, solver);
        int count = 0;
        solver.NewSearch(db);
        while (solver.NextSolution())
        {
            ++count;
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                    s.Cells[j][i] = (int)grid[j][i].Value();
            }
        }
        solver.EndSearch();
        Console.WriteLine($"Number of solutions found: {solver.Solutions()}");

        Console.WriteLine("Advanced usage:");
        Console.WriteLine($"Problem solved in {solver.WallTime()}ms");
        Console.WriteLine($"Memory usage: {Solver.MemoryUsage()}bytes");
        return s;
    }

    private DecisionBuilder SetUpDecisionBuilder(SudokuGrid sudokuGrid,Solver solver)
    {
        int gridSize = 9;

        grid = new IntVar[gridSize][];
        
        for (int i = 0; i < gridSize; ++i)
        {
            grid[i] = new IntVar[gridSize]; // Initialize the second dimension
            for (int j = 0; j < gridSize; j++)
            {
                if (sudokuGrid.Cells[i][j] != 0)
                {
                    grid[i][j] = solver.MakeIntConst(sudokuGrid.Cells[i][j]);
                }
                else
                    grid[i][j] = solver.MakeIntVar(1, 9, $"grid{i}{j}");
            }
        }

        for (int i = 0; i < gridSize; i++)
        {
            solver.Add(solver.MakeAllDifferent(grid[i]));
        }
        
        for (int i = 0; i < gridSize; i++)
        {
            var col = new IntVar[9];
            for (int j = 0; j < gridSize; j++)
            {
                col[j] = grid[j][i];
            } 
            solver.Add(solver.MakeAllDifferent(col));
        }
        
        for (int i = 0; i < gridSize; i += 3)
        {
            for (int j = 0; j < gridSize; j+=3)
            {
                var square = new IntVar[9];
                int n = 0;
                for (int k = i; k < i+3; k++)
                {
                    for (int l = j; l < j+3; l++)
                    {
                        square[n] = grid[k][l];
                        n++;
                    }
                }
                solver.Add(solver.MakeAllDifferent(square));
            }
        }

        IntVar[] flatGrid = grid.Flatten();
        return solver.MakePhase(flatGrid, Solver.CHOOSE_FIRST_UNBOUND, Solver.ASSIGN_MIN_VALUE);
    }
}