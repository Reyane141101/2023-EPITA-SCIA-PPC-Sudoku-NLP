using Sudoku.Shared;

namespace Sudoku.DlxLib;

public class DlxLibEmptySolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        Console.WriteLine(@"===== DlixLib Empty Solver =====");
        return s.CloneSudoku();
    }
}