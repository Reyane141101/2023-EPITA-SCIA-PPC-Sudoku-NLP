using Google.OrTools.Sat;
using Sudoku.Shared;

namespace Sudoku.OrTools;

public class OrToolsSolver : ISudokuSolver
{
    private const int Size = 9;

    public SudokuGrid Solve(SudokuGrid s)
    {
        CpSolver solver = new();
        (CpModel model, IntVar[,] vars) = InitModel(s);
        solver.Solve(model);

        for (int i = 0; i < Size; i++)
        for (int j = 0; j < Size; j++)
            s.Cells[i][j] = (int)solver.Value(vars[i, j]);

        return s;
    }

    /**
		 * Initialize a model with n*n from a sudoku grid
		 */
    private static (CpModel, IntVar[,]) InitModel(SudokuGrid grid)
    {
        CpModel model = new();

        IntVar[,] cellVars = new IntVar[Size, Size];
        for (int i = 0; i < Size; i++)
        for (int j = 0; j < Size; j++)
            if (grid.Cells[i][j] != 0)
                cellVars[i, j] = model.NewConstant(grid.Cells[i][j]);
            else
                cellVars[i, j] = model.NewIntVar(1, Size, $"x{i}{j}");

        AddConstraints(model, cellVars);

        return (model, cellVars);
    }

    private static void AddConstraints(CpModel model, IntVar[,] vars)
    {
        for (int i = 0; i < Size; i++)
        {
            IntVar[] row = new IntVar[Size];
            IntVar[] col = new IntVar[Size];
            for (int j = 0; j < Size; j++)
            {
                row[j] = vars[i, j];
                col[j] = vars[j, i];
            }

            model.AddAllDifferent(row);
            model.AddAllDifferent(col);
        }

        for (int i = 0; i < Size; i++)
        for (int j = 0; j < Size; j++)
            if (i % 3 == 0 && j % 3 == 0)
                model.AddAllDifferent(AddBoxConstraints(vars, i, j));
    }

    private static IEnumerable<IntVar> AddBoxConstraints(IntVar[,] vars, int startX, int startY)
    {
        IntVar[] box = new IntVar[9];
        for (int i = startX; i < startX + 3; i++)
        for (int j = startY; j < startY + 3; j++)
            box[(i - startX) * 3 + (j - startY)] = vars[i, j];
        return box;
    }
}
