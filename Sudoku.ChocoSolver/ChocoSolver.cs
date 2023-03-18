using Sudoku.Shared;
namespace Sudoku.ChocoSolver;

public class ChocoSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
}
