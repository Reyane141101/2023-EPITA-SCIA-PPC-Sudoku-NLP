using Google.OrTools.Sat;
using Sudoku.Shared;

namespace Sudoku.OrTools
{
    public class OrToolsSolver : ISudokuSolver
    {
        private const int Size = 9;

        public SudokuGrid Solve(SudokuGrid s)
        {
            return s;
        }

        /**
		 * Initialize a model with n*n from a sudoku grid
		 */
        private CpModel InitModel(SudokuGrid grid)
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

            return model;
        }
    }
}