using Sudoku.Shared;

namespace Sudoku.DlxLib;

public class DlxLibEmptySolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        Console.WriteLine(@"===== DlixLib Solver =====");
        return s.CloneSudoku();
    }
}