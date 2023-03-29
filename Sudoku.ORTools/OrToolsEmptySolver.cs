using Google.OrTools.Sat;
using Sudoku.Shared;

namespace Sudoku.OrTools
{
    public class OrToolsEmptySolver : ISudokuSolver
    {
        private const int Size = 9;

        public SudokuGrid Solve(SudokuGrid s)
        {
            var solver = new CpSolver();
            var (model, vars) = InitModel(s);
            solver.Solve(model);

            for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                s.Cells[i][j] = (int)solver.Value(vars[i, j]);

            return s;
        }

        /**
		 * Initialize a model with n*n from a sudoku grid
		 */
        private static (CpModel, IntVar[,]) InitModel(SudokuGrid grid)
        {
            var model = new CpModel();

            var cellVars = new IntVar[Size, Size];
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (grid.Cells[i][j] != 0)
                        cellVars[i, j] = model.NewConstant(grid.Cells[i][j]);
                    else
                        cellVars[i, j] = model.NewIntVar(1, Size, $"x{i}{j}");
                }
            }

            AddConstraints(model, cellVars);

            return (model, cellVars);
        }

        private static void AddConstraints(CpModel model, IntVar[,] vars)
        {
            for (var i = 0; i < Size; i++)
            {
                var row = new IntVar[Size];
                var col = new IntVar[Size];
                for (var j = 0; j < Size; j++)
                {
                    row[j] = vars[i, j];
                    col[j] = vars[j, i];

                    if (i % 3 == 0 && j % 3 == 0)
                        model.AddAllDifferent(AddBoxConstraints(vars, i, j));
                }

                model.AddAllDifferent(row);
                model.AddAllDifferent(col);
            }
        }

        private static IEnumerable<IntVar> AddBoxConstraints(IntVar[,] vars, int startX, int startY)
        {
            var box = new IntVar[9];
            for (var i = startX; i < startX + 3; i++)
            for (var j = startY; j < startY + 3; j++)
                box[i * 3 * j] = vars[i, j];

            return box;
        }
    }
}